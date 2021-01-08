function validate() {
    var isValid = true;

    if ($('#RankId').val().length === 0) {
        $('#RankId').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#RankId').css('border-color', 'lightgrey');
    }

    if ($('#UserName').val().length === 0) {
        $('#UserName').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#UserName').css('border-color', 'lightgrey');
    }

    if ($('#Password').val().length === 0) {
        $('#Password').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#Password').css('border-color', 'lightgrey');
    }

    //if ($('#Email').val().length === 0) {
    //    $('#Email').css('border-color', 'Red');
    //    isValid = false;
    //}
    //else {
    //    $('#Email').css('border-color', 'lightgrey');
    //}

    if ($('#Gender').val().length === 0) {
        $('#Gender').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#Gender').css('border-color', 'lightgrey');
    }

    if ($('#VesselIMO').val().length === 0) {
        $('#VesselIMO').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#VesselIMO').css('border-color', 'lightgrey');
    }


    return isValid;
}

function clearTextBox() {
    $('#UserId').val("");
    $('#RankId').val("");
    $('#UserName').val("");
    $('#Password').val("");
    $('#Email').val("");
   // $('#CreatedBy').val("");
   // $('#ModifiedBy').val("");
    $('#Gender').val("");
    $('#VesselIMO').val("");
}

function SaveUpdateUser() {

    //alert($('textarea#Comments').val());
    //debugger;
    var posturl = $('#SaveUpdateUser').val();
    var res = validate();
    if (res == false) {
        return false;
    }
    //alert(res);
    if (res) {
        var UserMaster = {
            UserId: $('#UserId').val(),
            RankId: $('#RankId').val(),
            UserName: $('#UserName').val(),
            Password: $('#Password').val(),
            Email: $('#Email').val(),
            //CreatedBy: $('#CreatedBy').val(),
            //ModifiedBy: $('#ModifiedBy').val(),
            Gender: $('#Gender').val(),
            VesselIMO: $('#VesselIMO').val()
        };

        $.ajax({
            url: posturl,
            data: JSON.stringify(UserMaster),
            type: "POST",
            contentType: "application/json;charset=utf-8",
            dataType: "json",

            //success: function (result) {

            //    alert('Added Successfully');

            //    clearTextBox();
            //    }         
            //,



            success: function (result) {
                loadData();
                $('#myModal').modal('hide');
                // alert('Added Successfully');

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
    if ($.fn.dataTable.isDataTable('#UserMasterTable')) {
        table = $('#UserMasterTable').DataTable();
        table.destroy();
    }

    // alert('hh');
    var table = $("#UserMasterTable").DataTable({
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
            //{
            //    "data": "Order", "name": "Order", "autoWidth": true, "className": 'reorder'
            //},
            {
                "data": "UserName", "name": "UserName", "autoWidth": true
            },
            {
                "data": "CreatedOn", "name": "CreatedOn", "autoWidth": true
            },
            {
                "data": "Email", "name": "Email", "autoWidth": true
            },
            //{
            //    "data": "CreatedBy", "name": "CreatedBy", "autoWidth": true
            //},
            //{
            //    "data": "ModifiedBy", "name": "ModifiedBy", "autoWidth": true
            //},
            {
                "data": "Gender", "name": "Gender", "autoWidth": true
            },
            {
                "data": "VesselIMO", "name": "VesselIMO", "autoWidth": true
            },
            {
                "data": "RankName", "name": "RankName", "autoWidth": true
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


function DeleteUserMaster(UserId) {
    var e = $('#DeleteUserMaster').val();
    var ans = confirm("Are you sure you want to delete this Record?");
    if (ans) {
        // debugger;
        $.ajax({
            url: e,
            data: JSON.stringify({ UserId: UserId }),
            type: "POST",
            contentType: "application/json;charset=UTF-8",
            dataType: "json",
            success: function (result) {
                // debugger;

                if (result == -1) {
                    alert("Department cannot be deleted as this is already used.");
                }
                else if (result == 0) {
                    alert("Department cannot be deleted as this is already used.");
                }
                else {
                    loadData();
                }
            },
            error: function () {
                alert("Department cannot be deleted as this is already used in Crew");
            }
        });
    }
}



function GetUserByUserId(UserId) {
    $('#UserName').css('border-color', 'lightgrey');
    var x = $("#GetUserByUserId").val();
    //alert(x);
    //debugger;
    $.ajax({
        url: x,
        data:
        {
            UserId: UserId
        },
        type: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            //debugger;
            $('#UserId').val(result.UserId);
            $('#UserName').val(result.UserName);
            $('#Password').val(result.Password);
            $('#CreatedOn').val(result.CreatedOn);
            $('#Email').val(result.Email);
            //$('#CreatedBy').val(result.CreatedBy);
            //$('#ModifiedBy').val(result.ModifiedBy);
            $('#Gender').val(result.Gender);
            $('#VesselIMO').val(result.VesselIMO);
            $('#RankId').val(result.RankId);

            $('#myModal').modal('show');
            $('#btnUpdate').show();
            $('#btnAdd').hide();

        },
        error: function (errormessage) {
            //debugger;
            console.log(errormessage.responseText);
        }
    });
    return false;
}

