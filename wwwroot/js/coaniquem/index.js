document.addEventListener("DOMContentLoaded", function () {

    document.getElementById("btn-copy-santander").addEventListener("click", function (e) {
        e.preventDefault();
        navigator.clipboard.writeText("Corporación COANIQUEM 70.715.400-4 Banco Santander Cuenta corriente 65-5 colecta@coaniquem.org");
    });

    document.getElementById("btn-copy-bestado").addEventListener("click", function (e) {
        e.preventDefault();
        navigator.clipboard.writeText("Corporación COANIQUEM 70.715.400-4 Banco Estado Cuenta corriente 4008588 colecta@coaniquem.org");
    });

});