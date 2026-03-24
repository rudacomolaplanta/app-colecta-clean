document.addEventListener("DOMContentLoaded", function () {

    document.querySelectorAll(".btn-ques-email").forEach(i => {
        i.addEventListener("click", function (e) {
            Swal.fire({
                title: 'Ayuda',
                icon: 'info',
                text: 'Solicitamos el correo electrónico ya que algunos medios de pago requieren esta información',
                showCloseButton: true,
                showConfirmButton: false,
                allowOutsideClick: true,
                allowEscapeKey: true
            });
        });
    });

    if (document.getElementById("btn-info")) {
        document.getElementById("btn-info").addEventListener("click", function () {
            Swal.fire({
                html: '<ul class="mt-5 p-1 rounded" style="font-size:16px"> <li>Este contador de donaciones registra de manera automática todas las donaciones realizadas a través de WebPay, Servipag, Banco Santander, Flow y Khipu. Por razones técnicas, los montos podrían tener un pequeño retraso en su actualización. <br><br>Recuerda que las donaciones realizadas vía PayPal, Banco Estado, Transferencias bancarias y Caja Vecina, serán actualizadas una vez al día.</li></ul>',
                showCloseButton: true,
                showConfirmButton: true,
                confirmButtonText: 'Aceptar',
                confirmButtonColor: '#e30613',
                allowOutsideClick: true,
                allowEscapeKey: true
            });
        });
    }

});