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
            SetUpGridNew(CategoryId);
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
                   
                    var str = '<div class="col-sm-12"><div class="row"> ';
                    str = str + '<a href="#" class="btn btn-info btn-sm" style="background-color: #e90000;" onclick="PreviewModal(\''+data+'\')" >Preview</a>';
                    str = str + '<a href="' + data + '" class="btn btn-info btn-sm" style="background-color: #e90000;" >Download</a>';
                    return str;
                },


            },














            //{
            //    "data": "IsUpload", "width": "150px", "render": function (data) {
            //        var str = '<div class="col-sm-12"><div class="row"><div class="col-sm-6"><a href="            " class="btn btn-info btn-sm" style="background-color: #e90000;" >Upload</a></div>';
            //        return str;
            //    },
            //},


            {
                "data": "IsUpload", "width": "50px", "render": function (data, row) {

                    console.log(row.CategoryId);
                    console.log(data);

                    //if (data == '0') {
                    //    return '<a href="#" class="btn btn-info btn-sm" style="background-color: #e90000;" >Not Upload</a>';
                    //}
                    //else if (data == '1') {
                    //    return '<button type="button" class="btn btn-info btn-sm" style="background-color: #7db700;" data-toggle="modal" data-target="#filledUpFormModal" >Upload</button>';

                    //}
                    if (data == '1') {
                        return '<button type="button" class="btn btn-info btn-sm" style="background-color: #7db700;" data-toggle="modal" data-target="#filledUpFormModal" >Upload</button>';

                    }
                }
            },










            {
                "data": "Version", "name": "Version", "autoWidth": true
            }
        ],
        "rowId": "ID",
        /*"dom": "Bfrtip"*/
    });
}


function PreviewModal(path) {

    $('#pdfContent').html("");
    $('#pdfContent').html('<embed id="embedPDF" src="" width="100%" height="600px;" />');

    $('#hHeader').html("");
    $('#embedPDF').removeAttr("src");
    var x = decodeURI(path);
    $.ajax({
        url: "/FormsAndChecklists/PreviewModal",
        data:
        {
            relPDFPath: decodeURI(path)
        },
        type: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {

            $('#hHeader').html(result.PdfName);
            $('#embedPDF').attr('src', result.PdfPath);

            $('#pdfPreviewModal').modal('show');

        },
        error: function (errormessage) {
            //debugger;
            console.log(errormessage.responseText);
        }
    });

}





function SetUpGridNew(CategoryId) {
    var loadposturl = $('#loaddata').val();
    //do not throw error
    $.fn.dataTable.ext.errMode = 'none';
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
            {
                "data": "Path", "width": "150px", "render": function (data) {
                    var str = '<div class="col-sm-12"><div class="row"> ';
                  /*  str = str + '<a href="#" class="btn btn-info btn-sm" style="background-color: #e90000;" onclick="PreviewModal(\'' + data + '\')" >Preview</a>';*/
                    str = str + '<a href="' + data + '" class="btn btn-info btn-sm" style="background-color: #e90000;" >Download</a>';
                    return str;
                },
            },
            {
                "data": "IsUpload", "width": "50px", "render": function (data, row) {
                    console.log(row.CategoryId);
                    console.log(data);
                    if (data == '1') {
                        return '<button type="button" class="btn btn-info btn-sm" style="background-color: #7db700;" data-toggle="modal" data-target="#filledUpFormModal" >Upload</button>';
                    }
                }
            },
            {
                "data": "Version", "name": "Version", "autoWidth": true
            }
        ],
        "rowId": "ID",
        /*"dom": "Bfrtip"*/
    });
}
