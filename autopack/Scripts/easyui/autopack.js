$(document).ready(runInit);

var clear = true;
var running = false;

function runInit() {
    $("#next").linkbutton({ disabled: false }).click(runNext);
    $("#prev").linkbutton({ disabled: false }).click(runPrev);
}

function runDisable() {
    $("#next").linkbutton({ disabled: true });
    $("#prev").linkbutton({ disabled: true });
}

function runEnable() {
    $("#next").linkbutton({ disabled: false });
    $("#prev").linkbutton({ disabled: false });
}

function runNext() {
    if (running) {
        runNull();
    } else {
        $("#output").empty();
        runDisable();
        $.get("/Home/runNext");
        window.setTimeout("runOutput()", 1000);
    }
}

function runPrev() {
    if (running) {
        runNull();
    } else {
        $("#output").remove();
        runDisable();
        $.get("/Home/runPrev");
        window.setTimeout("runOutput()", 1000);
    }
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
            if ("$end$" == data) {
                runEnable();
                clear = true;
                return;
            }
            if ("" == data) {
                window.setTimeout("runOutput()", 1000);
                return;
            }
            if (clear) {
                $("#output").append(data);
                clear = false;
            } else {
                $("#output").append("<br />" + data);
            }
            window.setTimeout("runOutput()", 1000);
        }
    });
}
