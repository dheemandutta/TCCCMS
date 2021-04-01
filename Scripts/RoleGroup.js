function validate() {
    var isValid = true;

    if ($('#RoleId').val().length === 0) {
        $('#RoleId').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#RoleId').css('border-color', 'lightgrey');
    }

    if ($('#GroupId').val().length === 0) {
        $('#GroupId').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#GroupId').css('border-color', 'lightgrey');
    }

    return isValid;
}

function clearTextBox() {
    $('#RoleId').val("");
    $('#GroupId').val("");
}

function SaveRoleGroup() {

    //alert($('textarea#Comments').val());
    //debugger;
    var posturl = $('#SaveRoleGroup').val();
    var res = validate();
    if (res == false) {
        return false;
    }
    //alert(res);
    if (res) {
        var RoleGroup = {
            RoleId: $('#RoleId').val(),
            GroupId: $('#GroupId').val()
        };

        $.ajax({
            url: posturl,
            data: JSON.stringify(RoleGroup),
            type: "POST",
            contentType: "application/json;charset=utf-8",
            dataType: "json",

            //success: function (result) {

            //    alert('Added Successfully');

            //    clearTextBox();
            //    }         
            //,



            success: function (result) {
                //loadData();
                //$('#myModal').modal('hide');
                 //alert('Added Successfully');

                toastr.options = {
                    "closeButton": false,
                    "debug": false,
                    "newestOnTop": false,
                    "progressBar": false,
                    "positionClass": "toast-bottom-full-width",
                    "preventDuplicates": false,
                    "onclick": null,
                    "showDuration": "300",
                    "hideDuration": "1000",
                    "timeOut": "5000",
                    "extendedTimeOut": "1000",
                    "showEasing": "swing",
                    "hideEasing": "linear",
                    "showMethod": "fadeIn",
                    "hideMethod": "fadeOut"
                };

                toastr.success("Added Successfully");

                clearTextBox();
            },
            error: function (errormessage) {
                console.log(errormessage.responseText);
            }
        });
    }
}