import * as Functions from './functions.js';
import * as Communication from './modules/communication.js';

//Para cálculo de 100%
let total2021 = 370691111;

document.addEventListener('DOMContentLoaded', function () {

    Array.from(document.getElementsByClassName("btn-regresar")).forEach(el => {
        el.addEventListener("click", function (e) {
            e.preventDefault();
            history.back();
        });
    });

    document.getElementById("btnConsultaAlcancia").addEventListener("click", function (e) {
        let ref = document.getElementById("referencia").value;
        if (ref && ref.length > 0) {

            Communication.getData("Coaniquem/ConsultaAlcanciaRef", { id: ref }).then(res => {
                document.getElementById("show-monto").innerHTML = "$" + formatNumber(res.data[0].total.toString());
                document.getElementById("containerResultado").classList.remove("d-none");
            });

            /*let res = Functions.ajaxCallWithParams("/Coaniquem/ConsultaAlcanciaRef", { id: ref });
            if (res.code == 0 && res.size > 0) {

            } else {
                Swal.fire({
                    title: res.message,
                    icon: 'error',
                    showCloseButton: true,
                    showConfirmButton: false,
                    allowOutsideClick: true,
                    allowEscapeKey: true,
                    backdrop: true,
                });
            }*/

        } else {
            Swal.fire({
                title: 'Ingresa una referencia',
                showCloseButton: true,
                showConfirmButton: false,
                allowOutsideClick: true,
                allowEscapeKey: true,
                backdrop: true,
            });
        }

    });

});

function formatNumber(n) {
    // format number 1000000 to 1,234,567
    return n.replace(/\D/g, "").replace(/\B(?=(\d{3})+(?!\d))/g, ".");
}