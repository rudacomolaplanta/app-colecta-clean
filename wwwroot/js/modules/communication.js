import * as Functions from './functions.js';

export let getUrl = window.location;
export let baseUrl = getUrl.protocol + "//" + getUrl.host + "/";

//Iniciaización de Toasts
export const Toast = Swal.mixin({
    toast: true,
    position: 'top-end',
    showConfirmButton: false,
    timer: 3000,
    timerProgressBar: true,
    didOpen: (toast) => {
        toast.addEventListener('mouseenter', Swal.stopTimer)
        toast.addEventListener('mouseleave', Swal.resumeTimer)
    }
});

function initloader() {
    //Loader
    //document.getElementById("global-loader").style.display = "";
    //document.getElementsByClassName("page")[0].style.display = "none";
}

function finishloader() {
    //Termina Loader
    //document.getElementById("global-loader").style.display = "none";
    //document.getElementsByClassName("page")[0].style.display = "";
}

export async function postData(url = '', data = {}) {
    initloader();
    // Default options are marked with *
    const response = await fetch(baseUrl + url, {
        method: 'POST', // *GET, POST, PUT, DELETE, etc.
        mode: 'cors', // no-cors, *cors, same-origin
        cache: 'no-cache', // *default, no-cache, reload, force-cache, only-if-cached
        credentials: 'same-origin', // include, *same-origin, omit
        headers: {
            'Content-Type': 'application/json'
            // 'Content-Type': 'application/x-www-form-urlencoded',
        },
        redirect: 'follow', // manual, *follow, error
        referrerPolicy: 'no-referrer', // no-referrer, *no-referrer-when-downgrade, origin, origin-when-cross-origin, same-origin, strict-origin, strict-origin-when-cross-origin, unsafe-url
        body: JSON.stringify(data) // body data type must match "Content-Type" header
    });

    //Centralizamos la respuesta para la operación
    if (response.ok) {
        let res = await response.json();
        finishloader();
        return res;
    } else {
        Swal.fire({
            icon: 'error',
            title: 'Error de comunicación con el servidor',
            html: "<strong>[" + url + "]<br>respondió con código: " + response.status + "</strong><p class='text-danger'><strong>Contactar inmediatamente a TI para revisión.</strong></p>"
        });
    }

}

export async function getData(url = '', data = {}) {
    initloader();
    // Default options are marked with *
    const response = await fetch(baseUrl + url + '?' + new URLSearchParams(data).toString(), {
        method: 'GET', // *GET, POST, PUT, DELETE, etc.
        mode: 'cors', // no-cors, *cors, same-origin
        cache: 'no-cache', // *default, no-cache, reload, force-cache, only-if-cached
        credentials: 'same-origin', // include, *same-origin, omit
        headers: {
            'Content-Type': 'application/json'
            // 'Content-Type': 'application/x-www-form-urlencoded',
        },
        redirect: 'follow', // manual, *follow, error
        referrerPolicy: 'no-referrer', // no-referrer, *no-referrer-when-downgrade, origin, origin-when-cross-origin, same-origin, strict-origin, strict-origin-when-cross-origin, unsafe-url
    });

    if (response.ok) {
        let res = await response.json();
        finishloader();
        return res;
    } else {
        Swal.fire({
            icon: 'error',
            title: 'Error de comunicación con el servidor',
            html: "<strong>[" + url + "]<br>respondió con código: " + response.status + "</strong><p class='text-danger'><strong>Contactar inmediatamente a TI para revisión.</strong></p>"
        });
    }
}

export async function putData(url = '', data = {}) {
    initloader();
    // Default options are marked with *
    const response = await fetch(baseUrl + url, {
        method: 'PUT', // *GET, POST, PUT, DELETE, etc.
        mode: 'cors', // no-cors, *cors, same-origin
        cache: 'no-cache', // *default, no-cache, reload, force-cache, only-if-cached
        credentials: 'same-origin', // include, *same-origin, omit
        headers: {
            'Content-Type': 'application/json'
            // 'Content-Type': 'application/x-www-form-urlencoded',
        },
        redirect: 'follow', // manual, *follow, error
        referrerPolicy: 'no-referrer', // no-referrer, *no-referrer-when-downgrade, origin, origin-when-cross-origin, same-origin, strict-origin, strict-origin-when-cross-origin, unsafe-url
        body: JSON.stringify(data) // body data type must match "Content-Type" header
    });
    //Centralizamos la respuesta para la operación
    if (response.ok) {
        let res = await response.json();
        finishloader();
        return res;
    } else {
        Swal.fire({
            icon: 'error',
            title: 'Error de comunicación con el servidor',
            html: "<strong>[" + url + "]<br>respondió con código: " + response.status + "</strong><p class='text-danger'><strong>Contactar inmediatamente a TI para revisión.</strong></p>"
        });
    }
}

export async function deleteData(url = '', data = {}) {
    initloader();
    // Default options are marked with *
    const response = await fetch(baseUrl + url, {
        method: 'DELETE', // *GET, POST, PUT, DELETE, etc.
        mode: 'cors', // no-cors, *cors, same-origin
        cache: 'no-cache', // *default, no-cache, reload, force-cache, only-if-cached
        credentials: 'same-origin', // include, *same-origin, omit
        headers: {
            'Content-Type': 'application/json'
            // 'Content-Type': 'application/x-www-form-urlencoded',
        },
        redirect: 'follow', // manual, *follow, error
        referrerPolicy: 'no-referrer', // no-referrer, *no-referrer-when-downgrade, origin, origin-when-cross-origin, same-origin, strict-origin, strict-origin-when-cross-origin, unsafe-url
        body: JSON.stringify(data) // body data type must match "Content-Type" header
    });
    //Centralizamos la respuesta para la operación

    if (response.ok) {
        let res = await response.json();
        finishloader();
        return res;
    } else {
        Swal.fire({
            icon: 'error',
            title: 'Error de comunicación con el servidor',
            html: "<strong>[" + url + "]<br>respondió con código: " + response.status + "</strong><p class='text-danger'><strong>Contactar inmediatamente a TI para revisión.</strong></p>"
        });
    }

}
