$(document).ready(runEnable);

function runEnable() {
    $("#next").linkbutton({ disabled: false }).click(runNext);
    $("#prev").linkbutton({ disabled: false }).click(runPrev);
    window.setTimeout("runOutput()", 3);
}

function runDisable() {
    $("#next").linkbutton({ disabled: true }).click(runNull);
    $("#prev").linkbutton({ disabled: true }).click(runNull);
}

function runNext() {
    runDisable();
    $.get("/Home/Next");
}

function runPrev() {
    runDisable();
    $.get("/Home/Prev");
}

function runNull() {
    $.messager.alert('提示', '亲！稍等，命令还在执行当中!', 'ok');
}

function runOutput() {
    jQuery.ajax({
        type: 'GET',
        url: '/Home/runOutput',
        contentType: "application/json",
        async: true,
        success: function (data) {
            $("#output").append("<br />" + data);
            window.setTimeout("runOutput()", 1000);
        }
    });
}
