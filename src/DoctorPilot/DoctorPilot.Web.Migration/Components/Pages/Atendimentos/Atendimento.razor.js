let mediaRecorder = null;
let mediaStream = null;
let chunks = [];
let recordingBlob = null;
let stopPromise = null;
let uploadController = null;

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

export async function transcribeRecording(agendamentoId) {
    if (!recordingBlob || recordingBlob.size === 0) {
        throw new Error("MissingRecording: nenhuma gravacao concluida esta disponivel.");
    }

    const antiforgeryToken = document.querySelector(
        "#transcription-antiforgery input[name='__RequestVerificationToken']")?.value;

    if (!antiforgeryToken) {
        throw new Error("AntiforgeryTokenMissing: nao foi possivel validar o upload.");
    }

    uploadController?.abort();
    uploadController = new AbortController();

    const formData = new FormData();
    formData.append("audio", recordingBlob, getRecordingFileName(recordingBlob.type));

    try {
        const response = await fetch(`/api/transcription-upload/${encodeURIComponent(agendamentoId)}`, {
            method: "POST",
            body: formData,
            credentials: "same-origin",
            headers: {
                "RequestVerificationToken": antiforgeryToken
            },
            signal: uploadController.signal
        });

        if (response.redirected || response.status === 401 || response.status === 403) {
            throw new Error("AuthenticationError: sua sessao expirou. Entre novamente para transcrever a gravacao.");
        }

        const responseBody = await readResponseBody(response);
        if (!response.ok) {
            const message = responseBody?.message || responseBody?.detail || responseBody?.title ||
                "Nao foi possivel transcrever a gravacao.";
            const errorType = response.status >= 500 ? "TranscriptionServiceError" : "UploadRejected";
            throw new Error(`${errorType}: ${message}`);
        }

        if (!responseBody?.textoTranscrito) {
            throw new Error("InvalidResponse: o servidor nao retornou uma transcricao valida.");
        }

        return responseBody;
    } catch (error) {
        if (error?.name === "AbortError") {
            throw new Error("UploadCanceled: a transcricao foi interrompida.");
        }

        if (error instanceof TypeError) {
            throw new Error("NetworkError: nao foi possivel conectar ao servidor para transcrever a gravacao.");
        }

        throw error;
    } finally {
        uploadController = null;
    }
}

export async function dispose() {
    uploadController?.abort();
    uploadController = null;

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

function getRecordingFileName(mimeType) {
    const normalizedMimeType = (mimeType || "").split(";", 1)[0].toLowerCase();
    const extension = {
        "audio/ogg": "ogg",
        "audio/mp4": "m4a",
        "audio/mpeg": "mp3",
        "audio/wav": "wav",
        "audio/x-wav": "wav",
        "audio/webm": "webm"
    }[normalizedMimeType] || "webm";

    return `consulta.${extension}`;
}

async function readResponseBody(response) {
    const contentType = response.headers.get("content-type") || "";
    if (!contentType.includes("json")) {
        return null;
    }

    try {
        return await response.json();
    } catch {
        return null;
    }
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
