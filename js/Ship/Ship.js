function clearTextBox() {
    $('#ID').val("");
    $('#ShipName').val("");
    $('#Flag').val("");
    $('#IMONumber').val("");
    $('#btnUpdate').hide();
    $('#btnAdd').show();

    $('#VesselTypeID').val("");
    $('#ddlVesselSubType').val("");
    $('#ddlVesselSubSubType').val("");

    $('#Email1').val("");
    $('#Email2').val("");
    $('#Voice1').val("");
    $('#Voice2').val("");
    $('#Fax1').val("");
    $('#Fax2').val("");
    $('#VideoCall1').val("");
    $('#VideoCall2').val("");
    $('#Mobile1').val("");
    $('#Mobile2').val("");

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
    if ($.fn.dataTable.isDataTable('#shipsTable')) {
        table = $('#shipsTable').DataTable();
        table.destroy();
    }

    $("#shipsTable").DataTable({
        "processing": true, // for show progress bar
        "serverSide": true, // for process server side
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
                "data": "ShipName", "name": "ShipName", "autoWidth": true
            },
            {
                "data": "FlagOfShip", "name": "FlagOfShip", "autoWidth": true
            },
            {
                "data": "IMONumber", "name": "IMONumber", "autoWidth": true
            },




            {
                "data": "ID", "width": "50px", "render": function (data) {
                    return '<a href="#" onclick="GetShipByID(' + data + ')">Edit</a>';
                }
            }

        ]
    });
}

function GetShipByID(ID) {
    $('#ShipName').css('border-color', 'lightgrey');
    var x = $("#urlGetShipById").val();
    $.ajax({
        url: x,
        data:
        {
            id: ID
        },
        type: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            //debugger;
            $('#ID').val(result.ID);

            $('#ddlVesselType').val(result.VesselTypeId);
            //$("#ddlVesselType option[value='" + result.VesselTypeID+"']").attr("selected", "selected");

            GetVesselSubTypeByTypeForDropdown(result.VesselTypeId);
            $('#ddlVesselSubType').prop('disabled', false);
            //$('#ddlVesselSubType').val(result.VesselSubTypeId).attr("selected", "selected");
            $('#ddlVesselSubType').val(result.VesselSubTypeId);

            GetVesselSubSubTypeByVesselSubTypeForDropdown(result.VesselSubTypeId);
            $('#ddlVesselSubSubType').prop('disabled', false);
            $('#ddlVesselSubSubType').val(result.VesselSubSubTypeId);

            $('#ShipName').val(result.ShipName);
            $('#ShipName').prop('disabled', true);
            $('#Flag').val(result.FlagOfShip);
            $('#Flag').prop('disabled', true);
            $('#IMONumber').val(result.IMONumber);
            $('#IMONumber').prop('disabled', true);

            $('#Email1').val(result.ShipEmail1);
            $('#Email2').val(result.ShipEmai2);
            $('#Voice1').val(result.Voices1);
            $('#Voice2').val(result.Voices2);
            $('#Fax1').val(result.Fax1);
            $('#Fax2').val(result.Fax2);
            $('#VideoCall1').val(result.VOIP1);
            $('#VideoCall2').val(result.VOIP2);
            $('#Mobile1').val(result.Mobile1);
            $('#Mobile2').val(result.Mobile1);

            $('#myModal').modal('show');
            $('#btnUpdate').show();
            //  $('#btnAdd').hide();
        },
        error: function (errormessage) {
            //debugger;
            console.log(errormessage.responseText);
        }
    });
    return false;
}

function SaveShipDetails() {
    // alert();
    var i = $('#urlSaveDetails').val();
    // debugger;
    //var res = validateVessel();
    //if (res === false) {
    //    return false;
    //}
    var shipObj = {

        ID: $('#ID').val(),
        ShipName: $('#ShipName').val(),
        FlagOfShip: $('#Flag').val(),
        IMONumber: $('#IMONumber').val(),

        VesselTypeID: $('#ddlVesselType').val(),
        VesselSubTypeID: $('#ddlVesselSubType').val(),
        VesselSubSubTypeID: $('#ddlVesselSubSubType').val(),

        ShipEmail1: $('#Email1').val(),
        ShipEmail2: $('#Email2').val(),
        Voices1: $('#Voice1').val(),
        Voices2: $('#Voice2').val(),
        Fax1: $('#Fax1').val(),
        Fax2: $('#Fax2').val(),
        VOIP1: $('#VideoCall1').val(),
        VOIP2: $('#VideoCall2').val(),
        Mobile1: $('#Mobile1').val(),
        Mobile2: $('#Mobile2').val()
    };
    //debugger;
    $.ajax({

        url: i,
        data: JSON.stringify(shipObj),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {

            //debugger;
            $('#myModal').modal('hide');

            $('#ID').val("");
            $('#Vessel').val("");
            $('#Flag').val("");
            $('#IMONumber').val("");

            $('#VesselTypeID').val("");
            $('#ddlVesselSubType').val("");
            $('#ddlVesselSubSubType').val("");

            $('#Email1').val("");
            $('#Email2').val("");
            $('#Voice1').val("");
            $('#Voice2').val("");
            $('#Fax1').val("");
            $('#Fax2').val("");
            $('#VideoCall1').val("");
            $('#VideoCall2').val("");
            $('#Mobile1').val("");
            $('#Mobile2').val("");
            SetUpGrid();
            //GetShip();
        },
        error: function (errormessage) {
            console.log(errormessage.responseText);
        }

    });
}
function GetShip() {

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
            $('#ID').val(result.ID);

            $('#Vessel').val(result.ShipName);
            $('#Flag').val(result.FlagOfShip);
            $('#IMONumber').val(result.IMONumber);

            $('#VesselTypeID').val(result.VesselTypeID);
            $('#VesselSubTypeID').val(result.VesselSubTypeID);
            $('#VesselSubSubTypeID').val(result.VesselSubSubTypeID);

            $('#Email1').val(result.ShipEmail);
            $('#Email2').val(result.ShipEmail2);
            $('#Voice1').val(result.Voices1);
            $('#Voice2').val(result.Voices2);
            $('#Fax1').val(result.Fax1);
            $('#Fax2').val(result.Fax2);
            $('#VideoCall1').val(result.VOIP1);
            $('#VideoCall2').val(result.VOIP2);
            $('#Mobile1').val(result.Mobile1);
            $('#Mobile2').val(result.Mobile2);

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

function GetVesselTypeForDrp(/*VesselTypeID*/) {
    var x = $("#myUrlid0").val();

    $.ajax({
        url: x,
        type: "POST",
        //data: JSON.stringify({ /*'VesselTypeID': VesselTypeID*/ }),
        data:
        {
            //    ID: ID
        },
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {

            var drpVesselType = $('#VesselTypeID');
            drpVesselType.find('option').remove();

            drpVesselType.append('<option value=' + '0' + '>' + 'Select' + '</option>');

            $.each(result, function () {
                drpVesselType.append('<option value=' + this.VesselTypeID + '>' + this.Description + '</option>');
            });

            GetVesselTypeIDFromShip();
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function GetVesselSubTypeByTypeForDropdown(type) {
    //var vesselTypeId = type.value;
    var vesselTypeId = type;
    var x = $("#urlLoadVesselSubType").val();


    $.ajax({
        url: x,
        type: "POST",
        data: JSON.stringify({ 'vesselTypeID': vesselTypeId }),
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            //debugger;
            var drpVesselSubType = $('#ddlVesselSubType');
            drpVesselSubType.find('option').remove();

            //drpVesselSubType.append('<option value=' + '0' + '>' + 'Select' + '</option>');

            $.each(result, function () {
                drpVesselSubType.append('<option value=' + this.ID + '>' + this.Description + '</option>');
            });
            //GetVesselSubTypeIDFromShip();

        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function GetVesselSubSubTypeByVesselSubTypeForDropdown(VesselSubTypeID) {
    var x = $("#urlLoadVesselSubSubType").val();
    
    $.ajax({
        url: x,
        type: "POST",
        data: JSON.stringify({ 'vesselSubTypeID': VesselSubTypeID }),
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            //debugger;
            var drpVesselSubSubType = $('#ddlVesselSubSubType');
            drpVesselSubSubType.find('option').remove();

            //drpVesselSubSubType.append('<option value=' + '0' + '>' + 'Select' + '</option>');

            $.each(result, function () {
                drpVesselSubSubType.append('<option value=' + this.ID + '>' + this.Description + '</option>');
            });
            GetVesselSubSubTypeIDFromShip();
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}