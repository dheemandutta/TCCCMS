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



///////////////////////////////////////////////////////////////////////////////////////////////////////

function validate() {
    var isValid = true;

    if ($('#Chapter').val().length === 0) {
        $('#Chapter').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#Chapter').css('border-color', 'lightgrey');
    }

    if ($('#Section').val().length === 0) {
        $('#Section').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#Section').css('border-color', 'lightgrey');
    }
    if ($('#ChangeComment').val().length === 0) {
        $('#ChangeComment').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#ChangeComment').css('border-color', 'lightgrey');
    }

    if ($('#ModificationDate').val().length === 0) {
        $('#ModificationDate').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#ModificationDate').css('border-color', 'lightgrey');
    }

    return isValid;
}

function clearTextBox() {
    $('#RevisionHistoryId').val("");
    $('#Chapter').val("");
    $('#Section').val("");
    $('#ChangeComment').val("");
    $('#ModificationDate').val("");
}

function SaveRevisionHistory() {

    //alert($('textarea#Comments').val());
    //debugger;
    var posturl = $('#SaveRevisionHistory').val();
    var res = validate();
    if (res == false) {
        return false;
    }
    //alert(res);
    if (res) {
        var RevisionHistory = {
            RevisionHistoryId: $('#RevisionHistoryId').val(),
            Chapter: $('#Chapter').val(),
            Section: $('#Section').val(),
            ChangeComment: $('#ChangeComment').val(),
            ModificationDate: $('#ModificationDate').val()
        };

        $.ajax({
            url: posturl,
            data: JSON.stringify(RevisionHistory),
            type: "POST",
            contentType: "application/json;charset=utf-8",
            dataType: "json",

            //success: function (result) {

            //    alert('Added Successfully');

            //    clearTextBox();
            //    }         
            //,



            success: function (result) {
                //loadData();
                //$('#myModal').modal('hide');
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