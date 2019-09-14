var IpVerifyHelper = {

    verifyIp: function (ip) {
        return /\d+\.\d+\.\d+\.\d+/.test(ip);
    },

    insertPointCondition: function (strIp) {
        //strIp.length == 3 || strIp.length == 7 || strIp.length == 11;
        return /\d\d\d/.test(strIp)
            || /\d?\d?\d\.\d\d\d/.test(strIp)
            || /\d?\d?\d\.\d?\d?\d.\d\d\d/.test(strIp);
    },

    deleteCondition: function deleteCondition(strIP) {
        return !/^\d{1,3}($|\.(\d{1,3}($|\.(\d{1,3}($|\.(\d{1,3})?))?))?$)/.test(strIP);
    }
}

function onIpSearch() {
    var ip = document.querySelector('#ipInput').value;

    if (!IpVerifyHelper.verifyIp(ip)) {
        alert('Введён некорректный ip-адрес!');

        return;
    }

    loadJson('/ip/location?ip=' + ip, function (res) {
        setBySelector('#citySpan', res.City);
        setBySelector('#countrySpan', res.Country);
        setBySelector('#regionSpan', res.Region);
        setBySelector('#orgSpan', res.Organization);
        setBySelector('#latSpan', res.latitude);
        setBySelector('#longSpan', res.longitude);
    });
}

function verifyIpInput() {
    var ipIntput = document.querySelector('#ipInput');
    var ip = ipIntput.value;
    //if (IpVerifyHelper.insertPointCondition(ip)) {
    //    //ipIntput.value = ip + '.';
    //} else
    if (IpVerifyHelper.deleteCondition(ip)) {
            ipIntput.value = ip.substr(0, ip.length - 1);
        }
    
}