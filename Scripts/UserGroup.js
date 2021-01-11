function validate() {
    var isValid = true;

    if ($('#UserName').val().length === 0) {
        $('#UserName').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#UserName').css('border-color', 'lightgrey');
    }

    if ($('#Groups').val().length === 0) {
        $('#Groups').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#Groups').css('border-color', 'lightgrey');
    }


    return isValid;
}

function clearTextBox() {
    $('#UserGroupId').val("");
    $('#UserName').val("");
    $('#Groups').val("");
}

function SaveUpdateUser(userID) {

    var groupListVal = null;
    groupListVal = [];
    if (userID != '') {
        $('input:checkbox:checked').each(function () {
            groupListVal.push($(this).attr('value'));
        });

        var saveUrl = $('#SaveUserGroupMapping').val();

        $.post(saveUrl,
            { userId: userID, selectedGroups: groupListVal },
            function (data, status, jqxHR) {

            })
            .done(function (data) {
                //alert(status);
                $.notify("Data Saved Successfully!", { class:"notify.center", align: "center", verticalAlign: "top", color: "#fff", background: "#20D67B", type: "info", icon: "check"});
            })
            .fail(function (data) {
                $.notify("Data Not Saved.", { class: "notify.center", align: "center", verticalAlign: "top", color: "#fff", background: "#20D67B", type: "info", icon: "check" });
            });
    }
}

function loadData(selectedUserId) {
    var loadposturl = $('#loaddata').val();
    //alert(selectedUserId);
    if (selectedUserId != '') {
        $.post(loadposturl,
            { selectedUserId: selectedUserId },
            function (data, status, jqXHR) {
                //alert('call success');
            })
            .done(function (data) {
                //alert(data);
                var groupArr = data.split(',');
                //console.log(groupArr);

                $('.chkclass').prop('checked', false);


                for (var i = 0, l = groupArr.length; i < l; i++) {
                    var chkId = "chk" + groupArr[i];

                    $('#' + chkId + '').prop('checked', true);
                }
            })
            .fail(function () {
                alert('fetching group data failed');
            });
    }
    else {
        $('.chkclass').prop('checked', false);
    }

    
}





