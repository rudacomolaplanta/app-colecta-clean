export function ajaxCall(href) {
    let ret = null;
    let request = new XMLHttpRequest();
    request.open('POST', href, false);//No asíncrono
    //request.setRequestHeader("Content-Type", "application/json");
    request.send(null);
    if (request.status === 200) {
        //ret = JSON.parse(request.responseText);
        ret = request.responseText;
    } else {
        console.error(request.statusText);
    }
    return ret;
}

//Vanilla JS
export function ajaxCallWithParams(href, data) {
    let ret = null;
    let request = new XMLHttpRequest();
    request.open('POST', href, false);//No asíncrono
    request.setRequestHeader("Content-Type", "application/json");
    request.send(JSON.stringify(data));
    if (request.status === 200) {
        ret = JSON.parse(request.responseText);
        if (ret.code != null & ret.code == 96) { //Aplicamos redirección
            window.location.href = ret.redirectionURL;
            ret = null;
        }
    } else {
        console.error(request.statusText);
    }
    return ret;
}

export function formatNumber(n) {
    // format number 1000000 to 1,234,567
    return n.replace(/\D/g, "").replace(/\B(?=(\d{3})+(?!\d))/g, ".");
}
