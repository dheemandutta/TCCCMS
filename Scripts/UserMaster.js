
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