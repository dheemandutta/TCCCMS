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
    if ($.fn.dataTable.isDataTable('#RevisionHistoryTable')) {
        table = $('#RevisionHistoryTable').DataTable();
        table.destroy();
    }

    // alert('hh');
    var table = $("#RevisionHistoryTable").DataTable({
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
                "data": "FormName", "name": "FormName", "autoWidth": true
            },
            {
                "data": "ModifiedSection", "name": "ModifiedSection", "autoWidth": true
            },
            {
                "data": "UpdatedOn1", "name": "UpdatedOn1", "autoWidth": true
            },
            {
                "data": "Version", "name": "Version", "autoWidth": true
            }

        ],
        "rowId": "ID",
        "dom": "Bfrtip"
    });
}


function GetFormIdForModifiedSection() {

    var x = $("#myUrlNew").val();
    $.ajax({
        url: x,
        data:
        {
            //    ID: ID
        },
        type: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            //debugger;
            $('#FormId').val(result.FormId);

            //$('#myModal').modal('show');
            //$('#btnUpdate').show();
            //$('#btnAdd').hide();
        },
        error: function (errormessage) {
            //debugger;
            console.log(errormessage.responseText);
        }
    });
    return false;
}

