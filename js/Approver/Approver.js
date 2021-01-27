var tmpApproverList = [];
var approversCount = 0;
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

    tmpApproverList = [];
    $('#divTempApprover').addClass('displayNone');

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
    if ($("#ddlUser").val() === 0 || $("#ddlUser").val() < 0) {
        $('#ddlUser').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#ddlUser').css('border-color', 'lightgrey');

    }
    if ($("#ddlRank").val() === 0 || $("#ddlRank").val() < 0) {
        $('#ddlRank').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#ddlRank').css('border-color', 'lightgrey');
       
    }
    //if ($("#ddlApproverGrade").val() === 0 || $("#ddlApproverGrade").val() < 0) {
    //    $('#ddlApproverGrade').css('border-color', 'Red');
    //    isValid = false;
    //}
    //else {
    //    $('#ddlApproverGrade').css('border-color', 'lightgrey');

    //}
    if (tmpApproverList.length === 0) {

        $('#ddlUser').css('border-color', 'Red');
        isValid = false;
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
            },
            //{
            //    "data": "ID", "width": "50px", "render": function (data) {
            //        return '<a href="#" onclick="GetShipByID(' + data + ')">Edit</a>';
            //    }
            //},
            {
                "data": "ID", "width": "50px", "render": function (data) {
                    return '<a href="#" onclick="DeleteApprover(' + data + ')">Delete</a>';
                }
            }

        ],
        "rowId": "ID",
    });
}

function SetUpTempApproverGrid() {
    //var loadposturl = $('#loaddata').val();

    //do not throw error
    $.fn.dataTable.ext.errMode = 'none';

    //check if datatable is already created then destroy iy and then create it
    if ($.fn.dataTable.isDataTable('#tempApproverTable')) {
        table = $('#tempApproverTable').DataTable();
        table.destroy();
    }

    var table = $("#tempApproverTable").DataTable({
        "processing": false, // for show progress bar
        "rowReorder": false,
        "ordering": false,
        "filter": false, // this is for disable filter (search box)
        "orderMulti": false, // for disable multiple column at once
        "bLengthChange": false, //disable entries dropdown
        "bPaginate": false, //this is for hide pagination
        "bInfo": false, // hide showing entries
        //"ajax": {
        //    "url": loadposturl,
        //    "type": "POST",
        //    "datatype": "json"
        //},
        //"ajax": { "data": tmpApproverList },
        "data": tmpApproverList,
        "columns": [
            {
                "data": "SL", "name": "SL", "autoWidth": true
            },
            //{
            //    "data": "ID", "name": "ID", "autoWidth": true
            //},
            {
                "data": "Code", "name": "Code", "autoWidth": true
            },
            //{
            //    "data": "RankId", "name": "RankId", "autoWidth": true
            //},
            {
                "data": "Rank", "name": "Rank", "autoWidth": true
            },
            {
                "data": "Name", "name": "Name", "autoWidth": true
            },
            //{
            //    "data": "ID", "width": "50px", "render": function (data) {
            //        return '<a href="#" onclick="GetShipByID(' + data + ')">Edit</a>';
            //    }
            //},
            {
                "data": "ID", "width": "50px", "render": function (data) {
                    return '<a href="#" onclick="RemoveTempApprover(' + data + ')">Remove</a>';
                }
            }

        ],
        "rowId": "ID",
    });
}


function SaveApprover() {
    // alert();
    var URL = $('#urlSaveDetails2').val();
    // debugger;
    var res = validate();
    if (res === false) {
        return false;
    }
    var approverList = [];
    //var approverObj = {

    //    ID: $('#ID').val(),
    //    ShipId: $('#ddlShip').val(),
    //    VesselIMONumber: $('#IMONumber').val(),

    //    RankId: $('#ddlRank').val(),
    //    UserId: $('#ddlUser').val(),
    //    //ApproverId: $('#ddlApproverGrade').val(),

    //};
    for (var i = 0; i < tmpApproverList.length; i++) {
        //var cur = tmpApproverList[i];
        //if (cur.ID === user.UserId) {
        //    exist = true;
        //    break;
        //}
        approverList.push({
            ID: $('#ID').val(),
            ShipId: $('#ddlShip').val(),
            VesselIMONumber: $('#IMONumber').val(),

            RankId: tmpApproverList[i].RankId,
            UserId: tmpApproverList[i].ID
        });
    }
    //debugger;
    $.ajax({

        url: URL,
        //data: { approver: JSON.stringify(approverObj), userList: JSON.stringify(tmpApproverList)},
        data: JSON.stringify(approverList) ,
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
                $('#hdnApproversCount').val(result.Ship.ApproversCount);
                approversCount = result.Ship.ApproversCount;
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
    if (tmpApproverList.length <= 6) {
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
                    AddTempApprover(result.User);
                    SetUpTempApproverGrid();
                    $('#divTempApprover').removeClass('displayNone');
                },
                error: function (errormessage) {
                    alert(errormessage.responseText);
                }
            });
        }
    }
    else {
        alert("You are not allowed add more Approver");
    }
    
}

function DeleteApprover(id) {
    var URL = $('#urlDeleteApprover').val();
    var ans = confirm("Are you sure you want to delete this Record?");
    if (ans) {
        // debugger;
        $.ajax({
            url: URL,
            data: JSON.stringify({ approverMasterId: id }),
            type: "POST",
            contentType: "application/json;charset=UTF-8",
            dataType: "json",
            success: function (result) {
                // debugger;

                if (result == -1) {
                    alert("Approver cannot be deleted as this is already used.");
                }
                else if (result == 0) {

                    alert("Approver cannot be deleted as this is already used.");
                }
                else {

                    SetUpGrid();
                }
            },
            error: function () {
                //to do something
            }
        });
    }
}

function AddTempApprover(user) {
    var a = tmpApproverList.length;
    var b = $('#hdnApproversCount').val();
    var c = approversCount;
    var totalCount = eval( a + c);
    b = b.replace(/"/g, '\\"')
    //var totalCount = a + b;
    var idx = 0
    var rank = user.Rank;
    var exist = false;
    //if (tmpApproverList.length === 0) {
    //    tmpApproverList.push({
    //        SL: idx,
    //        ID: 100,
    //        Code: COD1234,
    //        Rank: Master,
    //        Name: Name1

    //    });
    //}
    if (totalCount < 6) {
        //tmpApproverList.r
        idx = tmpApproverList.length + 1;

        for (var i = 0; i < tmpApproverList.length; i++) {
            var cur = tmpApproverList[i];
            if (cur.ID === user.UserId) {
                exist = true;
                break;
            }
        }

        if (!exist) {
            tmpApproverList.push({
                SL: idx,
                ID: user.UserId,
                Code: user.UserCode,
                RankId: rank.RankId,
                Rank: rank.RankName,
                Name: user.UserName

            });
        }
        else {
            alert("This user Already added..!")
        }
        

    }
    else {
        alert("You are not allowed add more Approver");
    }
    
}

function RemoveTempApprover(id) {
    var idx = 1
    var objList = []; 
    for (var i = 0; i < tmpApproverList.length; i++) {
        var cur = tmpApproverList[i];
        if (cur.ID != id) {
            objList.push({
                SL: idx,
                ID: cur.ID,
                Code: cur.Code,
                RankId: cur.RankId,
                Rank: cur.Rank,
                Name: cur.Name
            });
            idx = idx + 1;
        }
    }
    
    tmpApproverList = objList;

    SetUpTempApproverGrid();
}



    
