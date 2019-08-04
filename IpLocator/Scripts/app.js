function loadToElementBySelector(selector, url) {
    var request = new XMLHttpRequest();

    request.open('GET', url, true);

    request.onload = function () {
        if (request.status >= 200 && request.status < 400) {
            document.querySelector(selector).innerHTML = request.responseText;
        }
    };

    request.send();
}

function onIpSearchTab() {
    loadToElementBySelector('#partialViewPlaceholder', '/IpSearch');
}

function onCitySearchTab() {
    loadToElementBySelector('#partialViewPlaceholder', '/CitySearch');
}

function onIpSearch() {
    var ip = document.querySelector('#ipInput').value;

    loadToElementBySelector('#ipSearchResult', '/ip/location?ip=' + ip);
}

function onCitySearch() {
    var city = document.querySelector('#cityInput').value;

    loadToElementBySelector('#citySearchResult', '/city/locations?city=' + city);
}