
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
    if ($.fn.dataTable.isDataTable('#TicketTable')) {
        table = $('#TicketTable').DataTable();
        table.destroy();
    }

    // alert('hh');
    var table = $("#TicketTable").DataTable({
        //"dom": 'Bfrtip',
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
                "data": "TicketNumber", "name": "TicketNumber", "autoWidth": true
            },
            {
                "data": "Error", "name": "Error", "autoWidth": true
            },
            {
                "data": "Description", "name": "Description", "autoWidth": true
            },
            //{
            //    "data": "IsSolved", "name": "IsSolved", "autoWidth": true
            //}      
             {
                 "data": "IsSolved", "width": "50px", "render": function (data, type, row) {

                    console.log(row.Id);

                    if (data == '1') {
                        return '<a style="background-color: #7db700; border-radius: 5px;">Resolved</a>';
                    }
                    else if (data == '0') {
                        return '<a style="background-color: #e90000; border-radius: 5px;">Pending</a>';

                    }
                }
            },
        ],
        "rowId": "Id",
        //"dom": "Bfrtip"
    });
}
