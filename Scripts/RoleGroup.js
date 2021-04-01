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

function validate2() {
    var isValid = true;

    if ($('#GroupId').val().length === 0) {
        $('#GroupId').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#GroupId').css('border-color', 'lightgrey');
    }

    if ($('#UserId').val().length === 0) {
        $('#UserId').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#UserId').css('border-color', 'lightgrey');
    }

    return isValid;
}


function clearTextBox() {
    $('#RoleId').val("");
    $('#GroupId').val("");
}


function clearTextBox2() {
    $('#GroupId').val("");
    $('#UserId').val("");
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

function GetRoleByGroupId(GroupId) {
   // $('#RankName').css('border-color', 'lightgrey');
    var x = $("#GetRoleByGroupId").val();
    //alert(x);
    //debugger;
    $.ajax({
        url: x,
        data:
        {
            GroupId: GroupId
        },
        type: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            //debugger;
           /* $('#RoleId').val(result.RoleId);*/

            $('#RoleName').text('Role : '+ result.RoleName);

            //$('#myModal').modal('show');
        },
        error: function (errormessage) {
            //debugger;
            console.log(errormessage.responseText);
        }
    });
    return false;
}


function SaveUserGroupMapping() {

    //alert($('textarea#Comments').val());
    //debugger;
    var posturl = $('#SaveUserGroupMapping').val();
    var res = validate2();
    if (res == false) {
        return false;
    }
    //alert(res);
    if (res) {
        var GroupUser = {
            userId: $('#UserId').val(),
            userGroupMapping: $('#GroupId').val()
            
        };
        console.log(GroupUser);
        $.ajax({
            url: posturl,
            data: JSON.stringify(GroupUser),
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

                clearTextBox2();
            },
            error: function (errormessage) {
                console.log(errormessage.responseText);
            }
        });
    }
}

