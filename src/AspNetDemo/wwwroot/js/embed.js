var embedBaseUrl = window.origin;

function embedQR(elementId, contents, checkUrl, redirect, size) {

    var config = {
        elementId: elementId,
        contents: contents,
        checkUrl: checkUrl
    };
    embed(config, redirect, size);
}

function justWatch(checkUrl, redirect) {
    var config = {
        checkUrl: checkUrl
    };
    if (redirect)
        config.redirect = redirect;
    checkDone(config);
}

function embed(embedConfig, redirect, size) {
    
    if (size)
        embedConfig.size = size;
    else
        embedConfig.size = Math.floor(Math.min(window.innerWidth, window.innerHeight, 334) * 0.9);
    if (redirect)
        embedConfig.redirect = redirect;
    var element = document.getElementById(embedConfig.elementId);
    new QRCode(element, {
        text: embedConfig.contents,
        width: embedConfig.size,
        height: embedConfig.size,
        colorDark: "#000000",
        colorLight: "#ffffff",
        correctLevel: QRCode.CorrectLevel.L
    });

    var btn = document.createElement("a");
    btn.href = embedConfig.contents.replace(/https:\/\//i, "indy://");
    btn.text = "Open in Identity Wallet";
    btn.className = "btn btn-secondary";
    var div = document.createElement("div");
    btn.style.marginBottom = "10px";
    div.appendChild(btn);
    element.insertBefore(div, element.firstElementChild);

    checkDone(embedConfig);
}

function checkDone(config) {
    config.doneCheckInterval = setInterval(function () {
        httpGetAsync(config.checkUrl, function (response) {
            if (response === "true") {
                stopDoneCheck(config);
                if (config.redirect) {
                    document.location.href = config.redirect;
                } else {
                    var element = document.getElementById(config.elementId);
                    var first = element.firstElementChild;
                    while (first) {
                        first.remove();
                        first = element.firstElementChild;
                    }
                    element.innerHTML = config.completeMessage;
                }
            }
        });
    }, 1000);
    config.stopCheckTimeout = setTimeout(function () { stopDoneCheck(config); }, 300000);
}

function httpGetAsync(theUrl, callback) {
    var xmlHttp = new XMLHttpRequest();
    xmlHttp.onreadystatechange = function () {
        if (xmlHttp.readyState === 4 && xmlHttp.status === 200)
            callback(xmlHttp.responseText);
    };
    xmlHttp.open("GET", theUrl, true);
    xmlHttp.send(null);
}

function stopDoneCheck(config) {

    var element = document.getElementById(config.elementId);
    while (element && element.lastElementChild) {
        element.removeChild(element.lastElementChild);
    }

    window.clearInterval(config.doneCheckInterval);
    window.clearTimeout(config.stopCheckTimeout);

}