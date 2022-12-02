window.onload = function () {
    $('html, html *').contents().each(function () {
        if (this.nodeType === 3) { // text node
            this.textContent = this.textContent.replace(/\u00A0/g, '');
        }
    });
}

