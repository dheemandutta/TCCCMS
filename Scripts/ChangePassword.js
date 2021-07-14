
//--------Change Password---------------------
function ChangePassword() {
    var posturl = $('#changePwd').val();
    var UserMaster = "";
    UserMaster = {
        UserId: $('#UserId').val(),
        Password: $('#OldPassword').val(),
        NewPassword: $('#NewPassword').val()
    };

    //$('#NewPassword').val('');
    //$('#ConfirmPassword').val('');

    $.ajax({
        url: posturl,
        //data: JSON.stringify(UserMaster),
        type: "GET",
        contentType: "application/json;charset=utf-8",
        dataType: "json",

        success: function (result) {

            if (result === '0' || result === 0) {
                $('#OldPassword').css('border-color', 'Red');
                $('#errm').prop('hidden', false);
            }
            //alert('Added Successfully');
            //CreateTableHeader(userType)


            clearTextBox();
        },
        error: function (errormessage) {
            console.log(errormessage.responseText);
        }
    });
}

//--------Change Password---------------------