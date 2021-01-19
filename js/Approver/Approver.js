function clearTextBox() {
    $('#ID').val("");
    $('#IMONumber').val("");
    $('#btnAdd').val("Add");

    $('#ddlShip').val("-1");
    $('#ddlRank').val("-1");
    $('#ddlUser').val("");
    $('#ddlUser').find('option').remove();
    $('#ddlApproverGrade').val("-1");
    $('#ddlUser').prop('disabled', true);
    $('#ddRank').prop('disabled', true);


    //$('#ShipName').css('border-color', 'lightgrey');
    $('#ddlShip').css('border-color', 'lightgrey');
    $('#ddlRank').css('border-color', 'lightgrey');
    $('#ddlUser').css('border-color', 'lightgrey');
    $('#ddlApproverGrade').css('border-color', 'lightgrey');

}
function validate() {
    var isValid = true;

    if ($("#ddlShip").val() === 0 || $("#ddlShip").val() < 0) {
        $('#ddlShip').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#ddlShip').css('border-color', 'lightgrey');
       

    }
    if ($("#ddlRank").val() === 0 || $("#ddlRank").val() < 0) {
        $('#ddlRank').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#ddlRank').css('border-color', 'lightgrey');
       
    }
    if ($("#ddlUser").val() === 0 || $("#ddlUser").val() < 0) {
        $('#ddlUser').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#ddlUser').css('border-color', 'lightgrey');

    }
    if ($("#ddlApproverGrade").val() === 0 || $("#ddlApproverGrade").val() < 0) {
        $('#ddlApproverGrade').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#ddlApproverGrade').css('border-color', 'lightgrey');

    }

    return isValid;
}

function SetUpGrid() {
    var loadposturl = $('#loaddata').val();

    //do not throw error
    $.fn.dataTable.ext.errMode = 'none';

    //check if datatable is already created then destroy iy and then create it
    if ($.fn.dataTable.isDataTable('#approverTable')) {
        table = $('#approverTable').DataTable();
        table.destroy();
    }

    var table = $("#approverTable").DataTable({
        //"dom": 'Bfrtip',
        "processing": false, // for show progress bar
        //"serverSide": true, // for process server side
        "rowReorder": false,
        "ordering": false,
        "filter": false, // this is for disable filter (search box)
        "orderMulti": false, // for disable multiple column at once
        "bLengthChange": false, //disable entries dropdown
        "ajax": {
            "url": loadposturl,
            "type": "POST",
            "datatype": "json"
        },
        "columns": [
            {
                "data": "RowNumber", "name": "RowNumber", "autoWidth": true
            },
            {
                "data": "Ship.ShipName", "name": "ShipName", "autoWidth": true
            },
            {
                "data": "VesselIMONumber", "name": "VesselIMONumber", "autoWidth": true
            },
            {
                "data": "Rank.RankName", "name": "RankName", "autoWidth": true
            },
            {
                "data": "User.UserName", "name": "UserName", "autoWidth": true
            },
            {
                "data": "ApproverDescription", "name": "ApproverDescription", "autoWidth": true
            }
            //{
            //    "data": "ID", "width": "50px", "render": function (data) {
            //        return '<a href="#" onclick="GetShipByID(' + data + ')">Edit</a>';
            //    }
            //}

        ],
        "rowId": "ID",
    });
}


function SaveApprover() {
    // alert();
    var i = $('#urlSaveDetails').val();
    // debugger;
    var res = validate();
    if (res === false) {
        return false;
    }
    var approverObj = {

        ID: $('#ID').val(),
        ShipId: $('#ddlShip').val(),
        VesselIMONumber: $('#IMONumber').val(),

        RankId: $('#ddlRank').val(),
        UserId: $('#ddlUser').val(),
        ApproverId: $('#ddlApproverGrade').val(),

    };
    //debugger;
    $.ajax({

        url: i,
        data: JSON.stringify(approverObj),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {

            //debugger;
            $('#myModal').modal('hide');
            clearTextBox();
            
            SetUpGrid();
        },
        error: function (errormessage) {
            console.log(errormessage.responseText);
        }

    });
}

function GetIMONumberByShip(id) {
    var shipId = id;
    var x = $("#urlGetIMOByShip").val();
    if (shipId > 0) {
        $.ajax({
            url: x,
            type: "POST",
            data: JSON.stringify({ 'shipId': shipId }),
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {
                //debugger;
                $('#IMONumber').val(result.VesselIMONumber);
            },
            error: function (errormessage) {
                alert(errormessage.responseText);
            }
        });
    }
}

function GetUserListByShipForDropdown(id) {
    //var vesselTypeId = type.value;
    var shipId = id;
    var x = $("#urlGetUserByShip").val();


    $.ajax({
        url: x,
        type: "POST",
        data: JSON.stringify({ 'shipId': shipId }),
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            //debugger;
            var ddlUser = $('#ddlUser');
            ddlUser.find('option').remove();

            //drpVesselSubType.append('<option value=' + '0' + '>' + 'Select' + '</option>');

            $.each(result, function () {
                ddlUser.append('<option value=' + this.UserId + '>' + this.UserName + '</option>');
            });
            //GetVesselSubTypeIDFromShip();

        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}
function GetRankByUser(id) {
    var userId = id;
    var x = $("#urlGeRankByUser").val();
    if (userId > 0) {
        $.ajax({
            url: x,
            type: "POST",
            data: JSON.stringify({ 'userId': userId }),
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {
                //debugger;
                $('#ddlRank').val(result.RankId);
            },
            error: function (errormessage) {
                alert(errormessage.responseText);
            }
        });
    }
}

    
