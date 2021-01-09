﻿function validate() {
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

//function SaveUpdateUser() {

//    //alert($('textarea#Comments').val());
//    //debugger;
//    var posturl = $('#SaveUpdateUser').val();
//    var res = validate();
//    if (res == false) {
//        return false;
//    }
//    //alert(res);
//    if (res) {
//        var UserMaster = {
//            UserId: $('#UserId').val(),
//            RankId: $('#RankId').val(),
//            UserName: $('#UserName').val(),
//            Password: $('#Password').val(),
//            Email: $('#Email').val(),
//            //CreatedBy: $('#CreatedBy').val(),
//            //ModifiedBy: $('#ModifiedBy').val(),
//            Gender: $('#Gender').val(),
//            VesselIMO: $('#VesselIMO').val()
//        };

//        $.ajax({
//            url: posturl,
//            data: JSON.stringify(UserMaster),
//            type: "POST",
//            contentType: "application/json;charset=utf-8",
//            dataType: "json",

//            //success: function (result) {

//            //    alert('Added Successfully');

//            //    clearTextBox();
//            //    }         
//            //,



//            success: function (result) {
//                loadData();
//                $('#myModal').modal('hide');
//                // alert('Added Successfully');

//                toastr.options = {
//                    "closeButton": false,
//                    "debug": false,
//                    "newestOnTop": false,
//                    "progressBar": false,
//                    "positionClass": "toast-bottom-full-width",
//                    "preventDuplicates": false,
//                    "onclick": null,
//                    "showDuration": "300",
//                    "hideDuration": "1000",
//                    "timeOut": "5000",
//                    "extendedTimeOut": "1000",
//                    "showEasing": "swing",
//                    "hideEasing": "linear",
//                    "showMethod": "fadeIn",
//                    "hideMethod": "fadeOut"
//                };

//                toastr.success("Added Successfully");

//                clearTextBox();
//            },
//            error: function (errormessage) {
//                console.log(errormessage.responseText);
//            }
//        });
//    }
//}

function loadData() {
    var loadposturl = $('#loaddata').val();
    $.ajax({
        url: loadposturl,
        type: "GET",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            SetUpGrid();
        },
        error: function (errormessage) {
            console.log(errormessage.responseText);
        }
    });
}

function SetUpGrid() {
    var loadposturl = $('#loaddata').val();

    //do not throw error
    $.fn.dataTable.ext.errMode = 'none';

    //check if datatable is already created then destroy iy and then create it
    if ($.fn.dataTable.isDataTable('#UserGroupTable')) {
        table = $('#UserGroupTable').DataTable();
        table.destroy();
    }

    // alert('hh');
    var table = $("#UserGroupTable").DataTable({
        "dom": 'Bfrtip',
        "rowReorder": false,
        "ordering": false,
        "filter": false, // this is for disable filter (search box)

        "ajax": {
            "url": loadposturl,
            "type": "POST",
            "datatype": "json"
        },
        "columns": [
            {
                "data": "UserName", "name": "UserName", "autoWidth": true
            },
            {
                "data": "SelectedGroups", "name": "SelectedGroups", "autoWidth": true
            },

            {
                "data": "UserId", "width": "50px", "render": function (data) {
                    return '<a href="#" class="btn btn-info btn-sm" style="background-color: #e90000;" onclick="GetUserByUserId(' + data + ')">Edit</a>';
                }
            },
            {
                "data": "UserId", "width": "50px", "render": function (d) {
                    //debugger;
                    return '<a href="#" class="btn btn-info btn-sm" style="background-color: #e90000;" onclick="DeleteUserMaster(' + d + ')">Delete</a>';


                }
            }

        ],
        "rowId": "UserId",
        "dom": "Bfrtip"
    });
}


//function DeleteUserMaster(UserId) {
//    var e = $('#DeleteUserMaster').val();
//    var ans = confirm("Are you sure you want to delete this Record?");
//    if (ans) {
//        // debugger;
//        $.ajax({
//            url: e,
//            data: JSON.stringify({ UserId: UserId }),
//            type: "POST",
//            contentType: "application/json;charset=UTF-8",
//            dataType: "json",
//            success: function (result) {
//                // debugger;

//                if (result == -1) {
//                    alert("Department cannot be deleted as this is already used.");
//                }
//                else if (result == 0) {
//                    alert("Department cannot be deleted as this is already used.");
//                }
//                else {
//                    loadData();
//                }
//            },
//            error: function () {
//                alert("Department cannot be deleted as this is already used in Crew");
//            }
//        });
//    }
//}



//function GetUserByUserId(UserId) {
//    $('#UserName').css('border-color', 'lightgrey');
//    var x = $("#GetUserByUserId").val();
//    //alert(x);
//    //debugger;
//    $.ajax({
//        url: x,
//        data:
//        {
//            UserId: UserId
//        },
//        type: "GET",
//        contentType: "application/json;charset=UTF-8",
//        dataType: "json",
//        success: function (result) {
//            //debugger;
//            $('#UserId').val(result.UserId);
//            $('#UserName').val(result.UserName);
//            $('#Password').val(result.Password);
//            $('#CreatedOn').val(result.CreatedOn);
//            $('#Email').val(result.Email);
//            //$('#CreatedBy').val(result.CreatedBy);
//            //$('#ModifiedBy').val(result.ModifiedBy);
//            $('#Gender').val(result.Gender);
//            $('#VesselIMO').val(result.VesselIMO);
//            $('#RankId').val(result.RankId);

//            $('#myModal').modal('show');
//            $('#btnUpdate').show();
//            $('#btnAdd').hide();

//        },
//        error: function (errormessage) {
//            //debugger;
//            console.log(errormessage.responseText);
//        }
//    });
//    return false;
//}

