import * as Functions from './functions.js';
import * as Communication from './modules/communication.js';

let min = 400;
let interval = null;
let interval2 = null;
let isOk = false;
let redpaytoken = null;
let modalRP = null; //Modal REDPAY
let modalRPMob = null; //Modal REDPAY
let urlDO = null;

window.addEventListener("beforeunload", function (e) {
    //TODO validar status de la TRX antes de cerrar
    if (!isOk)
        callRevokeToken();
});
document.addEventListener('DOMContentLoaded', function () {

    //Paypal
    /*if (document.getElementById("btn-paypal")) {
        document.getElementById("btn-paypal").addEventListener("click", function () {
            window.open('https://www.paypal.com/donate?hosted_button_id=5VGD649VXUC7Q&source=url')
        });
    }*/

    //Transbank
    if (document.getElementById("btn-webpay")) {
        var old_element = document.getElementById("btn-webpay");
        var new_element = old_element.cloneNode(true);
        old_element.parentNode.replaceChild(new_element, old_element);
        document.getElementById("btn-webpay").addEventListener("click", function () {
            //Validamos el monto
            let val = document.getElementById("valor").value.replaceAll(".", "");
            if (parseInt(val, 10) >= min) {
                let email = getEmail();
                window.location.href = '/webpay/init?m=' + val + "&r=" + document.getElementById("r").value + "&c=WEBPAY" + (email != null ? "&e=" + email : "");//OK
            } else {
                Swal.fire({
                    title: 'Monto mínimo $' + min,
                    showCloseButton: true,
                    showConfirmButton: false,
                    allowOutsideClick: true,
                    allowEscapeKey: true
                });
            }
        });
    }

    //Bancoestado
    if (document.getElementById("btn-bancoestado")) {
        var old_element = document.getElementById("btn-bancoestado");
        var new_element = old_element.cloneNode(true);
        old_element.parentNode.replaceChild(new_element, old_element);
        document.getElementById("btn-bancoestado").addEventListener("click", function () {
            //Validamos el monto
            let val = document.getElementById("valor").value.replaceAll(".", "");
            if (parseInt(val, 10) >= 500) {
                let email = getEmail();
                let url = 'https://www.bancoestado.cl/bancoestado/aportevoluntario/aporte.asp?monto=' + val + '&id_institucion=0003';
                //Functions.ajaxCallWithParams('/Coaniquem/UrlByPass', { url: url, c: "BANCOESTADO", e: email, m: val, r: document.getElementById("r").value });

                Communication.postData('coaniquem/urlbypass', { url: url, c: "BANCOESTADO", e: email, m: val, r: document.getElementById("r").value }).then(res => {
                    window.location.href = res.redirectionURL;
                });

            } else {
                Swal.fire({
                    title: 'Monto mínimo $500',
                    showCloseButton: true,
                    showConfirmButton: false,
                    allowOutsideClick: true,
                    allowEscapeKey: true
                });
            }
        });
    }

    //Santander
    if (document.getElementById("btn-bancosantander")) {

        var old_element = document.getElementById("btn-bancosantander");
        var new_element = old_element.cloneNode(true);
        old_element.parentNode.replaceChild(new_element, old_element);
        document.getElementById("btn-bancosantander").addEventListener("click", function () {
            //Validamos el monto
            let val = document.getElementById("valor").value.replaceAll(".", "");
            if (parseInt(val, 10) >= min) {
                let email = getEmail();
                //SANTANDER sólo funciona con window.open
                window.open('/Santander/init?m=' + val + "&r=" + document.getElementById("r").value + "&c=SANTANDER" + (email != null ? "&e=" + email : ""));
            } else {
                Swal.fire({
                    title: 'Monto mínimo $' + min,
                    showCloseButton: true,
                    showConfirmButton: false,
                    allowOutsideClick: true,
                    allowEscapeKey: true
                });
            }
        });
    }

    //Servipag
    if (document.getElementById("btn-servipag")) {
        var old_element = document.getElementById("btn-servipag");
        var new_element = old_element.cloneNode(true);
        old_element.parentNode.replaceChild(new_element, old_element);
        document.getElementById("btn-servipag").addEventListener("click", function () {
            //Validamos el monto
            let val = document.getElementById("valor").value.replaceAll(".", "");
            if (parseInt(val, 10) >= min) {
                let email = getEmail();
                if (email != null) {
                    window.location.href = '/flow/init?m=' + val + "&r=" + document.getElementById("r").value + "&c=SERVIPAG&tp=2" + (email != null ? "&e=" + email : "");
                } else {
                    Swal.fire({
                        title: 'El campo de correo electrónico es obligatorio para pago con SERVIPAG',
                        showCloseButton: true,
                        showConfirmButton: false,
                        allowOutsideClick: true,
                        allowEscapeKey: true
                    });
                }
            } else {
                Swal.fire({
                    title: 'Monto mínimo $' + min,
                    showCloseButton: true,
                    showConfirmButton: false,
                    allowOutsideClick: true,
                    allowEscapeKey: true
                });
            }
        });
    }


    //Flow
    if (document.getElementById("btn-flow")) {
        var old_element = document.getElementById("btn-flow");
        var new_element = old_element.cloneNode(true);
        old_element.parentNode.replaceChild(new_element, old_element);
        document.getElementById("btn-flow").addEventListener("click", function () {
            //Validamos el monto
            let val = document.getElementById("valor").value.replaceAll(".", "");
            if (parseInt(val, 10) >= min) {
                let email = getEmail();
                if (email != null) {
                    window.location.href = '/flow/init?m=' + val + "&r=" + document.getElementById("r").value + "&c=FLOW&tp=9" + (email != null ? "&e=" + email : "");
                } else {
                    Swal.fire({
                        title: 'El campo de correo electrónico es obligatorio para pago con FLOW',
                        showCloseButton: true,
                        showConfirmButton: false,
                        allowOutsideClick: true,
                        allowEscapeKey: true
                    });
                }
            } else {
                Swal.fire({
                    title: 'Monto mínimo $' + min,
                    showCloseButton: true,
                    showConfirmButton: false,
                    allowOutsideClick: true,
                    allowEscapeKey: true
                });
            }
        });
    }

    //kiphu
    if (document.getElementById("btn-khipu")) {
        var old_element = document.getElementById("btn-khipu");
        var new_element = old_element.cloneNode(true);
        old_element.parentNode.replaceChild(new_element, old_element);
        document.getElementById("btn-khipu").addEventListener("click", function () {
            //Validamos el monto
            let val = document.getElementById("valor").value.replaceAll(".", "");
            if (parseInt(val, 10) >= min) {
                let email = getEmail();
                window.location.href = '/khipu/init?m=' + val + "&r=" + document.getElementById("r").value + "&c=KHIPU" + (email != null ? "&e=" + email : "");
            } else {
                Swal.fire({
                    title: 'Monto mínimo $' + min,
                    showCloseButton: true,
                    showConfirmButton: false,
                    allowOutsideClick: true,
                    allowEscapeKey: true
                });
            }
        });
    }

    //paypal
    if (document.getElementById("btn-paypal")) {
        var old_element = document.getElementById("btn-paypal");
        var new_element = old_element.cloneNode(true);
        old_element.parentNode.replaceChild(new_element, old_element);
        document.getElementById("btn-paypal").addEventListener("click", function () {
            //Validamos el monto
            let val = document.getElementById("valor").value.replaceAll(".", "");
            if (parseInt(val, 10) >= 0) {
                let email = getEmail();
                let url = 'https://www.paypal.com/donate?hosted_button_id=5VGD649VXUC7Q&source=url';
                Communication.postData('coaniquem/urlbypass', { url: url, c: "PAYPAL", e: email, m: val, r: document.getElementById("r").value }).then(res => {
                    window.location.href = res.redirectionURL;
                });
            } else {
                Swal.fire({
                    title: 'Monto mínimo $' + 0,
                    showCloseButton: true,
                    showConfirmButton: false,
                    allowOutsideClick: true,
                    allowEscapeKey: true
                });
            }

        });
    }

    document.getElementById("btnDirectOpen").addEventListener("click", function () {
        if (urlDO) {
            window.open(urlDO);
        }
    });

    //Añadimos los formatters para el campo de valor
    document.getElementById("valor").addEventListener("keyup", function () {
        this.value = formatNumber(this.value);
    });

    document.getElementById("valor").addEventListener("blur", function () {
        this.value = formatNumber(this.value);
    });

    //Otros medios de pago
    document.getElementById("valor").addEventListener("blur", function () {
        this.value = formatNumber(this.value);
    });

});

function verifyTrx() {
    if (redpaytoken) {
        Communication.getData("redpay/verifytrx", { t: redpaytoken }).then(res => {
            if (res) {
                let data = res.data[0];
                if (data.responseResponseCode) { //Si hay respuesta
                    isOk = true;
                    if (data.responseResponseCode == "0") {
                        window.location.href = "/coaniquem/exito";
                    } else {
                        window.location.href = "/coaniquem/error";
                    }
                }
            }
        });
    }
}

function startTimer(duration, display) {
    var timer = duration, minutes, seconds;
    if (interval) {
        clearInterval(interval);
    }
    interval = setInterval(function () {
        minutes = parseInt(timer / 60, 10);
        seconds = parseInt(timer % 60, 10);
        minutes = minutes < 10 ? "0" + minutes : minutes;
        seconds = seconds < 10 ? "0" + seconds : seconds;
        display.textContent = minutes + ":" + seconds
        if (--timer < 0) {
            timer = duration;
        }
    }, 1000);
    if (interval2) {
        clearInterval(interval2);
    }
    interval2 = setInterval(function () {
        verifyTrx();
    }, 3000);
}

function getEmail() {
    let ret = null;
    if (document.getElementById("email")) {
        if (document.getElementById("email").value.length > 0 && validateEmail(document.getElementById("email").value)) {
            ret = document.getElementById("email").value;
        }
    }
    return ret;
}

function formatNumber(n) {
    // format number 1000000 to 1,234,567
    return n.replace(/\D/g, "").replace(/\B(?=(\d{3})+(?!\d))/g, ".");
}

function validateEmail(email) {
    var re = /\S+@\S+\.\S+/;
    return re.test(email);
}