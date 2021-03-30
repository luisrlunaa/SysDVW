function NewCategoryType() {
    var x = document.getElementById("Selectcategorytype").value;
    window.location.href = "NewProduct?id=" + x + "&handler=SelectCategorytype";
}

function EditCategoryType() {
    var x = document.getElementById("Selectcategorytype").value;
    window.location.href = "EditProduct?id=" + x + "&handler=SelectCategorytype";
}

function NewSelectType() {
    var x = document.getElementById("Selecttype").value;
    window.location.href = "NewProduct?id=" + x + "&handler=Selecttype";
}

function EditSelectType() {
    var x = document.getElementById("Selecttype").value;
    window.location.href = "EditProduct?id=" + x + "&handler=Selecttype";
}
