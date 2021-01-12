function validate() {
    var isValid = true;

    if ($('#PermissionName').val().length === 0) {
        $('#PermissionName').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#PermissionName').css('border-color', 'lightgrey');
    }

    if ($('#Roles').val().length === 0) {
        $('#Roles').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#Roles').css('border-color', 'lightgrey');
    }


    return isValid;
}

function clearTextBox() {
    $('#PermissionRoleId').val("");
    $('#PermissionName').val("");
    $('#Roles').val("");
}

function SaveUpdatePermissionRole(PermissionId) {

    var roleListVal = null;
    roleListVal = [];
    if (PermissionId != '') {
        $('input:checkbox:checked').each(function () {
            roleListVal.push($(this).attr('value'));
        });

        var saveUrl = $('#SaveUpdatePermissionRole').val();

        $.post(saveUrl,
            { PermissionId: PermissionId, selectedRoles: roleListVal },
            function (data, status, jqxHR) {

            })
            .done(function (data) {
                //alert(status);
                $.notify("Data Saved Successfully!", { class: "notify.center", align: "center", verticalAlign: "top", color: "#fff", background: "#20D67B", type: "info", icon: "check" });
            })
            .fail(function (data) {
                $.notify("Data Not Saved.", { class: "notify.center", align: "center", verticalAlign: "top", color: "#fff", background: "#20D67B", type: "info", icon: "check" });
            });
    }
}

function loadData(selectedPermissionId) {
    var loadposturl = $('#loaddata').val();
    //alert(selectedUserId);
    if (selectedPermissionId != '') {
        $.post(loadposturl,
            { selectedPermissionId: selectedPermissionId },
            function (data, status, jqXHR) {
                //alert('call success');
            })
            .done(function (data) {
                //alert(data);
                var PermissionArr = data.split(',');
                //console.log(groupArr);

                $('.chkclass').prop('checked', false);


                for (var i = 0, l = PermissionArr.length; i < l; i++) {
                    var chkId = "chk" + PermissionArr[i];

                    $('#' + chkId + '').prop('checked', true);
                }
            })
            .fail(function () {
                alert('fetching Permission data failed');
            });
    }
    else {
        $('.chkclass').prop('checked', false);
    }


}





