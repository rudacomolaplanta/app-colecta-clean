import * as Functions from './functions.js';
import * as Communication from './modules/communication.js';

let meta = 276900000;

document.addEventListener('DOMContentLoaded', function () {
    let pb = document.getElementById("progressbar");
    if (pb) {
        Communication.getData('Coaniquem/GetTotalAmount', {}).then(res => {
            let total = res.data[0];
            let perc = ((total / meta) * 100).toFixed(1);
            pb.style.width = perc + '%';
            document.getElementById("total").innerHTML = '$' + Functions.formatNumber(total.toString()) + ' (' + perc + '%)';
        });
        /*let res = Functions.ajaxCallWithParams('/Coaniquem/GetTotalAmount', {});
        if (res.code == 0) {
            let total = res.data[0];
            let perc = ((total / meta) * 100).toFixed(1);
            pb.style.width = perc + '%';
            document.getElementById("total").innerHTML = '$' + Functions.formatNumber(total.toString()) + ' (' + perc + '%)';
        }*/
    }
});
