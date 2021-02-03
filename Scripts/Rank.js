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
    if ($('#Email').val().length === 0) {//Added on 30th Jan 2021 @BK
        $('#Email').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#Email').css('border-color', 'lightgrey');
    }

    return isValid;
}

function clearTextBox() {
    $('#RankId').val("");
    $('#RankName').val("");
    $('#Description').val("");
    $('#Email').val("");//Added on 30th Jan 2021 @BK
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
            RankId:         $('#RankId').val(),
            RankName:       $('#RankName').val(),
            Description:    $('#Description').val(),
            Email:          $('#Email').val()//Added on 30th Jan 2021 @BK
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
            {//Added on 30th Jan 2021 @BK
                "data": "Email", "name": "Email", "autoWidth": true
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
            $('#Email').val(result.Email);//Added on 30th Jan 2021 @BK

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

function CheckEmail(email) {
    var regex = /^([a-zA-Z0-9_\.\-\+])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;
    var regex1 = /^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$/;
    if (!regex1.test(email)) {
        alert('Wrong formated emailID!');
        $('#Email').css('border-color', 'Red');
        $('#Email').focus();
        return false;
    } else {
        $('#Email').css('border-color', 'lightgrey');
        return true;
    }
}

