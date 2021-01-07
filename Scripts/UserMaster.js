function validate() {
    var isValid = true;

    if ($('#UserName').val().length === 0) {
        $('#UserName').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#UserName').css('border-color', 'lightgrey');
    }


    return isValid;
}

function clearTextBox() {
    $('#UserId').val("");
    $('#RankId').val("");
    $('#UserName').val("");
    $('#Password').val("");
    $('#Email').val("");
    $('#CreatedBy').val("");
    $('#ModifiedBy').val("");
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
            CreatedBy: $('#CreatedBy').val(),
            ModifiedBy: $('#ModifiedBy').val(),
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
            {
                "data": "CreatedBy", "name": "CreatedBy", "autoWidth": true
            },
            {
                "data": "ModifiedBy", "name": "ModifiedBy", "autoWidth": true
            },
            {
                "data": "Gender", "name": "Gender", "autoWidth": true
            },
            {
                "data": "VesselIMO", "name": "VesselIMO", "autoWidth": true
            },
            {
                "data": "RankName", "name": "RankName", "autoWidth": true
            },
     
            //,{
            //    "data": "EquipmentsID", "width": "50px", "render": function (data) {
            //        return '<a href="#" class="btn btn-info btn-sm" onclick="GetMedicalEquipmentByID(' + data + ')"><i class="glyphicon glyphicon-edit"></i></a>';
            //    }
            //},
            //{
            //    "data": "EquipmentsID", "width": "50px", "render": function (d) {
            //        //debugger;
            //        return '<a href="#" class="btn btn-info btn-sm" onclick="DeleteEquipments(' + d + ')"><i class="glyphicon glyphicon-trash"></i></a>';


            //    }
            //}

        ],
       // "rowId": "UserId",
        //"dom": "Bfrtip"
    });
}