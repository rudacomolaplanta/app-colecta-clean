
let winDim;

document.addEventListener('DOMContentLoaded', function () {

    //document.getElementById("main-container").style = "background-image: url('../img/2023/fondo_agradecimiento.jpg');background-size:cover;"

    /* winDim = getWinDim();
     //Si es menor de 800px ponemos la imágen móvil
     if (winDim.x < 720) {
         document.getElementById("imgbck").src = '/img/campana2022/COLECTA_paginaagradecimientos_movil.png';
         document.getElementById("video-container").style = "text-align: center;";
         document.getElementById("video-frame").width = "";
         document.getElementById("video-frame").style.width = "100%";
     }
 
     window.onresize = window.onload = function () {
         document.getElementsByTagName("body")[0].style = "text-align:center;";
         resize();
     }*/

});

function resize() {

    var img = document.getElementById("imgbck");
    img.style.height = winDim.y + "px";
    if (img.offsetWidth > winDim.x) {
        img.style.height = null;
        img.style.width = winDim.x + "px";
    }

    let imgH = img.height;
    if (imgH == 0) {
        imgH = img.style.height;
    }
}

function getWinDim() {
    var body = document.documentElement || document.body;
    return {
        x: window.innerWidth || body.clientWidth,
        y: window.innerHeight || body.clientHeight
    }
}