// Tu función original, intacta
function initializeDraggableElements(dotNetHelper) {
    const container = document.getElementById('ticket-container');

    document.querySelectorAll('.draggable-element').forEach(element => {
        element.onmousedown = function (event) {
            event.preventDefault();

            const elementType = element.getAttribute('data-element-id');
            const containerRect = container.getBoundingClientRect();
            const elementRect = element.getBoundingClientRect();

            let shiftX = event.clientX - elementRect.left;
            let shiftY = event.clientY - elementRect.top;

            function moveAt(clientX, clientY) {
                let newLeft = clientX - shiftX - containerRect.left;
                let newTop = clientY - shiftY - containerRect.top;

                newLeft = Math.max(0, Math.min(newLeft, containerRect.width - element.offsetWidth));
                newTop = Math.max(0, Math.min(newTop, containerRect.height - element.offsetHeight));

                element.style.left = `${newLeft}px`;
                element.style.top = `${newTop}px`;

                dotNetHelper.invokeMethodAsync('UpdateElementPosition', elementType, Math.round(newLeft), Math.round(newTop));
            }

            function onMouseMove(event) {
                moveAt(event.clientX, event.clientY);
            }

            document.addEventListener('mousemove', onMouseMove);

            document.onmouseup = function () {
                document.removeEventListener('mousemove', onMouseMove);
                document.onmouseup = null;
            };
        };

        element.ondragstart = () => false;
    });
}

// Función adicional para mostrar PDF generado con iText
window.abrirPdf = (base64) => {
    const byteCharacters = atob(base64);
    const byteNumbers = new Array(byteCharacters.length);
    for (let i = 0; i < byteCharacters.length; i++) {
        byteNumbers[i] = byteCharacters.charCodeAt(i);
    }
    const byteArray = new Uint8Array(byteNumbers);
    const blob = new Blob([byteArray], { type: "application/pdf" });
    const url = URL.createObjectURL(blob);
    window.open(url);
};

window.printPdf = (base64Pdf) => {
    return new Promise((resolve, reject) => {
        try {
            var blob = new Blob([new Uint8Array(atob(base64Pdf).split("").map(function (c) { return c.charCodeAt(0) }))], { type: "application/pdf" });
            var url = URL.createObjectURL(blob);
            let windowOptions = "width=800,height=600,scrollbars=yes,location=no,toolbar=no,status=no,menubar=no,resizable=yes";
            let pdfWindow = window.open(url, "TicketWindow", windowOptions);

            pdfWindow.onload = () => {
                pdfWindow.print();
            };

            pdfWindow.onafterprint = () => {
                if (pdfWindow) {
                    pdfWindow.close();
                    resolve(true); // Confirmamos que se cerró después de imprimir
                }
            };

            pdfWindow.oncancel = () => {
                reject(false); // Si cancelan la impresión, devolvemos FALSE
            };
        }
        catch (err) {
            reject(false); // Si hay error devolvemos FALSE
        }
    });
};