$(document).ready(function () {
    var disablebutton = document.getElementById("disablebutton");
    if (disablebutton.value != "disabled") {
        $('.disabledbuttonAdd').prop("disabled", false);

    } else {
        $('.disabledbuttonAdd').prop("disabled", true);
    }

    var paydisablebutton = document.getElementById("disablepaybutton");
    if (paydisablebutton.value != "disabled") {
        $('.disabledbuttonpay').prop("disabled", false);

    } else {
        $('.disabledbuttonpay').prop("disabled", true);
    }

    var isChecked = document.getElementById('exampleCheck1').checked;
    if (isChecked) {
        $('.disabledbuttonseach').prop("disabled", true);
    }
    else {
        $('.disabledbuttonseach').prop("disabled", false);
    }

    var pequena = document.getElementById("inlineRadio1");
    var grande = document.getElementById("inlineRadio2");
    var type = document.getElementById("type1");

    if (type.value != null) {
        if (type.value == "Pequena") {
            pequena.checked = true;
        } else {
            grande.checked = true;
        }
    }
});

function alerta(id) {
    var opcion = confirm("Esta seguro Desea Eliminar?");
    if (opcion == true) {
        window.location.href = "NewSales?ID=" + id + "&handler=DeleteProduct";
    } else { }
}

function SelecClient() {
    var doccli = document.getElementById("doccli").value;
    var nomcli = document.getElementById("nomcli").value;
    var isChecked = document.getElementById('exampleCheck1').checked;
    if (isChecked) {
        window.location.href = "NewSales?ID=" + doccli + "&nombre=" + nomcli + "&handler=AddClient";
    }
}

function TypeNcf() {
    var x = document.getElementById("SelecttipoNCF").value;
    window.location.href = "NewSales?nfc=" + x + "&handler=Selectncf";
}

function TypeTicket() {
    var x = document.getElementById("Selecttipoticket").value;
    window.location.href = "NewSales?id=" + x + "&handler=Selecttypeticket";
}

function TypePrint() {
    var pequena = document.getElementById("inlineRadio1");
    var grande = document.getElementById("inlineRadio2");
    var type = document.getElementById("type1");

    if (pequena.checked == true) {
        type.value = "Pequena";
    }

    if (grande.checked == true) {
        type.value = "Grande";
    }
    window.location.href = "NewSales?print=" + type.value + "&handler=SelecttypePrint";
}

function clean() {
    window.location.href = "NewSales?id=cleaning&handler=Clean";
}

function SumTotal() {
    var totalapagar = document.getElementById("totalapagar");
    var pagar = document.getElementById("pagar");
    var devuelta = document.getElementById("devuelta");
    var resultado = NaN;

    if (pagar.value > 0) {
        resultado = Math.round(pagar.value - totalapagar.value);
        devuelta.value = resultado;
    }
}

function calculoporcentaje() {
    var precioreal = document.getElementById("precioventa");
    var porcentaje = document.getElementById("porcentaje");
    var divisor = document.getElementById("divisor");
    var itbis = document.getElementById("itbis");

    var precio = 0;
    var igv = 0;
    var calculoporcentaje = 0;

    if (porcentaje.value > 0) {
        calculoporcentaje =porcentaje.value / 100;

        precio = parseFloat((precioreal.value / divisor.value).toFixed(2));
        igv = parseFloat((precio* calculoporcentaje).toFixed(2));

        precioreal.value = precio;
        itbis.value = igv;
    }
}