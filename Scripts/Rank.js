function validate() {
    var isValid = true;

    if ($('#RankName').val().length === 0) {
        $('#RankName').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#RankName').css('border-color', 'lightgrey');
    }

    if ($('#Description').val().length === 0) {
        $('#Description').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#Description').css('border-color', 'lightgrey');
    }

    return isValid;
}

function clearTextBox() {
    $('#RankId').val("");
    $('#RankName').val("");
    $('#Description').val("");
}

function SaveUpdateRank() {

    //alert($('textarea#Comments').val());
    //debugger;
    var posturl = $('#SaveUpdateRank').val();
    var res = validate();
    if (res == false) {
        return false;
    }
    //alert(res);
    if (res) {
        var Rank = {
            RankId: $('#RankId').val(),
            RankName: $('#RankName').val(),
            Description: $('#Description').val(),
        };

        $.ajax({
            url: posturl,
            data: JSON.stringify(Rank),
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
    if ($.fn.dataTable.isDataTable('#RankTable')) {
        table = $('#RankTable').DataTable();
        table.destroy();
    }

    // alert('hh');
    var table = $("#RankTable").DataTable({
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
                "data": "RankName", "name": "RankName", "autoWidth": true
            },
            {
                "data": "Description", "name": "Description", "autoWidth": true
            },

            {
                "data": "RankId", "width": "50px", "render": function (data) {
                    return '<a href="#" class="btn btn-info btn-sm" style="background-color: #e90000;" onclick="GetRankByRankId(' + data + ')">Edit</a>';
                }
            },
            {
                "data": "RankId", "width": "50px", "render": function (d) {
                    //debugger;
                    return '<a href="#" class="btn btn-info btn-sm" style="background-color: #e90000;" onclick="DeleteRank(' + d + ')">Delete</a>';


                }
            }

        ],
        "rowId": "RankId",
        "dom": "Bfrtip"
    });
}


function DeleteRank(RankId) {
    var e = $('#DeleteRank').val();
    var ans = confirm("Are you sure you want to delete this Record?");
    if (ans) {
        // debugger;
        $.ajax({
            url: e,
            data: JSON.stringify({ RankId: RankId }),
            type: "POST",
            contentType: "application/json;charset=UTF-8",
            dataType: "json",
            success: function (result) {
                // debugger;

                if (result == -1) {
                    alert("Rank cannot be deleted as this is already used.");
                }
                else if (result == 0) {
                    alert("Rank cannot be deleted as this is already used.");
                }
                else {
                    loadData();
                }
            },
            error: function () {
                alert("Rank cannot be deleted as this is already used in Crew");
            }
        });
    }
}



function GetRankByRankId(RankId) {
    $('#RankName').css('border-color', 'lightgrey');
    var x = $("#GetRankByRankId").val();
    //alert(x);
    //debugger;
    $.ajax({
        url: x,
        data:
        {
            RankId: RankId
        },
        type: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            //debugger;
            $('#RankId').val(result.RankId);
            $('#RankName').val(result.RankName);
            $('#Description').val(result.Description);

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

