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

export const ToastWithoutTimer = Swal.mixin({
    toast: true,
    position: 'top-end',
    showConfirmButton: false,
    timerProgressBar: true,
    didOpen: (toast) => {
        toast.addEventListener('mouseenter', Swal.stopTimer)
        toast.addEventListener('mouseleave', Swal.resumeTimer)
    }
});

export function formatRut(element) {
    let rut = element.value;
    if (rut.length > 1) {
        rut = rut.replace('-', '');
        rut = rut.substring(0, rut.length - 1) + "-" + rut.substring(rut.length - 1, rut.length);
    }
    element.value = rut;
}

export function rutEsValido(rut) {
    if (!rut || rut.trim().length < 3) return false;
    const rutLimpio = rut.replace(/[^0-9kK-]/g, "");

    if (rutLimpio.length < 3) return false;

    const split = rutLimpio.split("-");
    if (split.length !== 2) return false;

    const num = parseInt(split[0], 10);
    const dgv = split[1];

    const dvCalc = calculateDV(num);
    return dvCalc === dgv;
}

export function calculateDV(rut) {
    const cuerpo = `${rut}`;
    // Calcular Dígito Verificador
    let suma = 0;
    let multiplo = 2;

    // Para cada dígito del Cuerpo
    for (let i = 1; i <= cuerpo.length; i++) {
        // Obtener su Producto con el Múltiplo Correspondiente
        const index = multiplo * cuerpo.charAt(cuerpo.length - i);

        // Sumar al Contador General
        suma += index;

        // Consolidar Múltiplo dentro del rango [2,7]
        if (multiplo < 7) {
            multiplo += 1;
        } else {
            multiplo = 2;
        }
    }

    // Calcular Dígito Verificador en base al Módulo 11
    const dvEsperado = 11 - (suma % 11);
    if (dvEsperado === 10) return "k";
    if (dvEsperado === 11) return "0";
    return `${dvEsperado}`;
}

export function validarEmail(mail) {
    var mailformat = /^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/;
    if (mail.match(mailformat)) {
        return true;
    }
    else {
        return false;
    }
}

export function validarTelefono(telefono) {
    var format = /^[+]*[0-9]{11}$/;
    if (telefono.match(format)) {
        return true;
    }
    else {
        return false;
    }
}