function loadToElementBySelector(selector, url) {
    var request = new XMLHttpRequest();

    request.open('GET', url, true);

    request.onload = function () {
        if (request.status >= 200 && request.status < 400) {
            document.querySelector(selector).innerHTML = request.responseText;
        }
    };

    request.onerror = function (e) { alert(e); }

    request.send();
}

function loadJson(url, callback) {
    var request = new XMLHttpRequest();

    request.open('GET', url, true);

    request.onload = function () {
        if (request.status >= 200 && request.status < 400) {
            callback(JSON.parse(request.responseText));
        }
    };

    request.onerror = function (e) { alert(e); }

    request.send();
}

function onIpSearchTab() {
    loadToElementBySelector('#partialViewPlaceholder', '/IpSearch');
}

function onCitySearchTab() {
    loadToElementBySelector('#partialViewPlaceholder', '/CitySearch');
}

function setBySelector(selector, value) {
    document.querySelector(selector).innerText = value;
}

function onCitySearch() {
    var city = document.querySelector('#cityInput').value;

    loadToElementBySelector('#citySearchResult', '/city/locations?city=' + city);
}