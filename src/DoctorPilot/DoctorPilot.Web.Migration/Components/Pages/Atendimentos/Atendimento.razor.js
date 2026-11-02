let mediaRecorder = null;
let mediaStream = null;
let chunks = [];
let recordingBlob = null;
let stopPromise = null;

export async function startRecording() {
    if (!navigator.mediaDevices?.getUserMedia) {
        throw new Error("NotSupportedError: getUserMedia não está disponível neste navegador.");
    }

    if (typeof MediaRecorder === "undefined") {
        throw new Error("NotSupportedError: MediaRecorder não está disponível neste navegador.");
    }

    if (mediaRecorder?.state === "recording") {
        throw new Error("InvalidStateError: uma gravação já está em andamento.");
    }

    await resetRecordingState();

    try {
        mediaStream = await navigator.mediaDevices.getUserMedia({ audio: true });
        mediaRecorder = new MediaRecorder(mediaStream);
        chunks = [];
        recordingBlob = null;

        mediaRecorder.ondataavailable = event => {
            if (event.data?.size > 0) {
                chunks.push(event.data);
            }
        };

        mediaRecorder.start(1000);
    } catch (error) {
        releaseMediaStream();
        mediaRecorder = null;
        chunks = [];
        recordingBlob = null;

        const name = error?.name || "RecordingError";
        const message = error?.message || "Não foi possível iniciar a gravação.";
        throw new Error(`${name}: ${message}`);
    }
}

export function stopRecording() {
    if (stopPromise) {
        return stopPromise;
    }

    if (!mediaRecorder || mediaRecorder.state === "inactive") {
        throw new Error("InvalidStateError: não há gravação ativa.");
    }

    const recorder = mediaRecorder;
    const completion = new Promise((resolve, reject) => {
        recorder.onstop = () => {
            const mimeType = recorder.mimeType || chunks.find(chunk => chunk.type)?.type || "";
            recordingBlob = new Blob(chunks, mimeType ? { type: mimeType } : undefined);

            const result = {
                success: recordingBlob.size > 0,
                size: recordingBlob.size,
                mimeType: recordingBlob.type || mimeType || null
            };

            console.info("DoctorPilot: gravação finalizada.", {
                blobSize: result.size,
                mimeType: result.mimeType
            });

            releaseMediaStream();
            mediaRecorder = null;
            chunks = [];
            resolve(result);
        };

        recorder.onerror = event => {
            releaseMediaStream();
            mediaRecorder = null;
            chunks = [];
            recordingBlob = null;

            const error = event.error;
            const name = error?.name || "RecordingError";
            const message = error?.message || "Falha durante a gravação.";
            reject(new Error(`${name}: ${message}`));
        };

        recorder.stop();
    });

    stopPromise = completion.finally(() => {
        stopPromise = null;
    });

    return stopPromise;
}

export async function dispose() {
    if (mediaRecorder && mediaRecorder.state !== "inactive") {
        try {
            await stopRecording();
        } catch {
            // A liberação das tracks no finally abaixo continua obrigatória.
        }
    } else if (stopPromise) {
        try {
            await stopPromise;
        } catch {
            // A liberação das tracks no finally abaixo continua obrigatória.
        }
    }

    releaseMediaStream();
    mediaRecorder = null;
    chunks = [];
    recordingBlob = null;
    stopPromise = null;
}

async function resetRecordingState() {
    if (mediaRecorder && mediaRecorder.state !== "inactive") {
        await stopRecording();
    } else if (stopPromise) {
        await stopPromise;
    }

    releaseMediaStream();
    mediaRecorder = null;
    chunks = [];
    recordingBlob = null;
}

function releaseMediaStream() {
    if (!mediaStream) {
        return;
    }

    for (const track of mediaStream.getTracks()) {
        track.stop();
    }

    mediaStream = null;
}
