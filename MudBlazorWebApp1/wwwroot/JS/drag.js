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
