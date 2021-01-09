function validate() {
    var isValid = true;

    if ($('#RoleName').val().length === 0) {
        $('#RoleName').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#RoleName').css('border-color', 'lightgrey');
    }

    return isValid;
}

function clearTextBox() {
    $('#RoleId').val("");
    $('#RoleName').val("");
    // $('#CreatedBy').val("");
    // $('#ModifiedBy').val("");
}

function SaveUpdateRoleMaster() {

    //alert($('textarea#Comments').val());
    //debugger;
    var posturl = $('#SaveUpdateRoleMaster').val();
    var res = validate();
    if (res == false) {
        return false;
    }
    //alert(res);
    if (res) {
        var RoleMaster = {
            RoleId: $('#RoleId').val(),
            RoleName: $('#RoleName').val()
            //CreatedBy: $('#CreatedBy').val(),
            //ModifiedBy: $('#ModifiedBy').val()
        };
        $.ajax({
            url: posturl,
            data: JSON.stringify(RoleMaster),
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
    if ($.fn.dataTable.isDataTable('#RoleMasterTable')) {
        table = $('#RoleMasterTable').DataTable();
        table.destroy();
    }

    // alert('hh');
    var table = $("#RoleMasterTable").DataTable({
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
                "data": "RoleName", "name": "RoleName", "autoWidth": true
            },
            //{
            //    "data": "CreatedBy", "name": "CreatedBy", "autoWidth": true
            //},
            //{
            //    "data": "ModifiedBy", "name": "ModifiedBy", "autoWidth": true
            //},
            {
                "data": "RoleId", "width": "50px", "render": function (data) {
                    return '<a href="#" class="btn btn-info btn-sm" style="background-color: #e90000;" onclick="GetRoleMasterByRoleId(' + data + ')">Edit</a>';
                }
            },
            {
                "data": "RoleId", "width": "50px", "render": function (d) {
                    //debugger;
                    return '<a href="#" class="btn btn-info btn-sm" style="background-color: #e90000;" onclick="DeleteRoleMaster(' + d + ')">Delete</a>';
                }
            }

        ],
        "rowId": "RoleId",
        "dom": "Bfrtip"
    });
}


function DeleteRoleMaster(RoleId) {
    var e = $('#DeleteRoleMaster').val();
    var ans = confirm("Are you sure you want to delete this Record?");
    if (ans) {
        // debugger;
        $.ajax({
            url: e,
            data: JSON.stringify({ RoleId: RoleId }),
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



function GetRoleMasterByRoleId(RoleId) {
    $('#RoleName').css('border-color', 'lightgrey');
    var x = $("#GetRoleMasterByRoleId").val();
    //alert(x);
    //debugger;
    $.ajax({
        url: x,
        data:
        {
            RoleId: RoleId
        },
        type: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            //debugger;
            $('#RoleId').val(result.RoleId);
            $('#RoleName').val(result.RoleName);
            //$('#CreatedBy').val(result.CreatedBy);
            //$('#ModifiedBy').val(result.ModifiedBy);

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

