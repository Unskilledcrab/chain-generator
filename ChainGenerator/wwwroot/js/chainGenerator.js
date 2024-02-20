// This function is used to download the json file
export function download(json, filename, contentType) {
    var a = document.createElement("a");
    var file = new Blob([json], { type: contentType });
    a.href = URL.createObjectURL(file);
    a.download = filename;
    a.click();
}

export function scrollToElement(elementId) {
    const element = document.getElementById(elementId);
    if (element) {
        element.scrollIntoView({ behavior: 'smooth' });
    }
}
