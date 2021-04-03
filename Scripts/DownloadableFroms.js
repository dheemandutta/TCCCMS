function loadData() {
    var loadposturl = $('#loaddata').val();
    //CreateTableHeader(1);

    $.ajax({
        url: loadposturl,
        type: "GET",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            SetUpGrid(CategoryId);
        },
        error: function (errormessage) {
            console.log(errormessage.responseText);
        }
    });
}


function SetUpGrid(CategoryId) {
    var loadposturl = $('#loaddata').val();

    //do not throw error
    $.fn.dataTable.ext.errMode = 'none';

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // LIST OF ADMINISTRATION & MANAGEMENT FORMS = 4,
        // LIST OF MANAGEMENT OF SHIP PERSONNEL FORMS = 5,
        // LIST OF MAINTENANCE FORMS = 6,
        // NAVIGATION MANAGEMENT FORMS = 7,
        // SAFETY MANAGEMENT FORMS = 8.
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    //check if datatable is already created then destroy iy and then create it
    if ($.fn.dataTable.isDataTable('#DownloadableFromsTable')) {
        table = $('#DownloadableFromsTable').DataTable();
        table.destroy();
    }

    // alert('hh');
    var table = $("#DownloadableFromsTable").DataTable({
        /*"dom": 'Bfrtip',*/
        "rowReorder": false,
        "ordering": false,
        "filter": false, // this is for disable filter (search box)

        "ajax": {
            "url": loadposturl,
            "type": "POST",
            "datatype": "json",
            "data": { CategoryId: CategoryId }
        },
        "columns": [
            {
                "data": "FormName", "name": "FormName", "autoWidth": true
            },
            //{
            //    "data": "Path", "name": "Path", "autoWidth": true
            //},
            {
                "data": "Path", "width": "150px", "render": function (data) {
                    var str = '<div class="col-sm-12"><div class="row"><div class="col-sm-6"><a href="' + data + '" class="btn btn-info btn-sm" style="background-color: #e90000;" >Download</a></div>';
                    return str;
                },
            },
            {
                "data": "Version", "name": "Version", "autoWidth": true
            }
        ],
        "rowId": "ID",
        /*"dom": "Bfrtip"*/
    });
}