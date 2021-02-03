function validate() {
    var isValid = true;

    if ($("#userCode").val().length === 0) {
        $('#userCode').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#userCode').css('border-color', 'lightgrey');


    }
    if ($("#password").val().length === 0) {
        $('#password').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#password').css('border-color', 'lightgrey');

    }
    
    return isValid;
}
function CheckUserLogin() {
    var URL = $('#urlCheckLogin').val();
    var res = validate();
    if (res === false) {
        return false;
    }
    var userCode = $("#userCode").val();
    var password = $("#password").val();

    var user = {
        UserCode : userCode,
        Password : password
    };
    $.ajax({

        url: URL,
        data: JSON.stringify(user),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            if (result === "0") {
                $('#errm').prop('hidden', false);
            }

        },
        error: function (errormessage) {
            console.log(errormessage.responseText);
        }

    });
}