﻿


function GetLogForUserExcel(/*Id*/) {
    var x = $("#GetLogForUserExcel").val();
    //alert(x);
    //debugger;
    $.ajax({
        url: x,
        data:
        {
            //Id: Id
        },
        type: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            //debugger;
            //$('#Id').val(result.Id);
            $('#LogData').val(result.LogData);

        },
        error: function (errormessage) {
            //debugger;
            console.log(errormessage.responseText);
        }
    });
    return false;
}



