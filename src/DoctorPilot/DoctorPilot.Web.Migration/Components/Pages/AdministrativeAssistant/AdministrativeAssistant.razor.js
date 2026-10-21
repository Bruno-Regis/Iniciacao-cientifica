export function scrollToEnd(element) {
    if (!element) {
        return;
    }

    element.scrollTo({ top: element.scrollHeight, behavior: "smooth" });
}
