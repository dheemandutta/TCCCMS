
function validate() {
    var isValid = true;
    
    if ($('#RankId').val().length === 0) {
        $('#RankId').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#RankId').css('border-color', 'lightgrey');
    }

    if ($('#ShipId').val().length === 0) {
        $('#ShipId').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#ShipId').css('border-color', 'lightgrey');
    }

    if ($('#UserName').val().length === 0) {
        $('#UserName').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#UserName').css('border-color', 'lightgrey');
    }

    if ($('#Password').val().length === 0) {
        $('#Password').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#Password').css('border-color', 'lightgrey');
    }

    //if ($('#Email').val().length === 0) {
    //    $('#Email').css('border-color', 'Red');
    //    isValid = false;
    //}
    //else {
    //    $('#Email').css('border-color', 'lightgrey');
    //}

    if ($('#Gender').val().length === 0) {
        $('#Gender').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#Gender').css('border-color', 'lightgrey');
    }

    if ($('#VesselIMO').val().length === 0) {
        $('#VesselIMO').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#VesselIMO').css('border-color', 'lightgrey');
    }


    return isValid;
}

function validateCUser() {
    var isValid = true;

    

    if ($('#cUserName').val().length === 0) {
        $('#cUserName').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#cUserName').css('border-color', 'lightgrey');
    }

    if ($('#cPassword').val().length === 0) {
        $('#cPassword').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#cPassword').css('border-color', 'lightgrey');
    }

    if ($('#cEmail').val().length === 0) {
        $('#cEmail').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#cEmail').css('border-color', 'lightgrey');
    }

    if ($('#cGender').val().length === 0) {
        $('#cGender').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#cGender').css('border-color', 'lightgrey');
    }



    return isValid;
}

function clearTextBox() {
    $('#UserId').val("");
    $('#RankId').val("");
    $('#ShipId').val("");
    $('#UserName').val("");
    $('#UserCode').val(""); //--Added on 30th Jan 2021 @Bk
    $('#Password').val("");
    $('#ConfirmPassword').val("");
    $('#Email').val("");
   // $('#CreatedBy').val("");
   // $('#ModifiedBy').val("");
    $('#Gender').val("");
    $('#VesselIMO').val("");

    $('#IsAdmin').val("");

    //------Company User Added on 30th Jan 2021 @Bk
    $('#cUserName').val("");
    $('#cUserCode').val("");
    $('#cPassword').val("");
    $('#cConfirmPassword').val("");
    $('#cEmail').val("");
    // $('#CreatedBy').val("");
    // $('#ModifiedBy').val("");
    $('#cGender').val("");
    $('#cIsAdmin').val("");

    $('#btnUpdate').hide();
    $('#btnAdd').show();

    LoadTab();
}

function SaveUpdateUser() {

    //alert($('textarea#Comments').val());
    //debugger;
    var UserMaster="";
    var posturl = $('#SaveUpdateUser').val();
    var res = false;
    if (userType === 1) {
        res = validate();
    }
    else if (userType === 2) {
        res = validateCUser();
    }
    if (res == false) {
        return false;
    }
    //alert(res);
    if (res) {
        if (userType === 1) {
            UserMaster = {
                UserId:     $('#UserId').val(),
                RankId:     $('#RankId').val(),
                ShipId:     $('#ShipId').val(),
                UserCode:   $('#UserCode').val(),
                UserName:   $('#UserName').val(),
                Password:   $('#Password').val(),
                Gender:     $('#Gender').val(),
                VesselIMO:  $('#VesselIMO').val(),

                IsAdmin:    document.getElementById("IsAdmin").checked,
                UserType : 1
            };
        }
        else if (userType === 2) {
            UserMaster = {
                UserId:     $('#UserId').val(),
                UserName:   $('#cUserName').val(),
                UserCode:   $('#cUserCode').val(),
                Password:   $('#cPassword').val(),
                Email:      $('#cEmail').val(),
                Gender:     $('#cGender').val(),

                IsAdmin:    document.getElementById("cIsAdmin").checked,
                UserType: 2
            };
        }
         

        $.ajax({
            url: posturl,
            data: JSON.stringify(UserMaster),
            type: "POST",
            contentType: "application/json;charset=utf-8",
            dataType: "json",

            success: function (result) {
                //loadData();

                CreateTableHeader(userType)
                $('#myModal').modal('hide');

               

                

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
    CreateTableHeader(1);

    $.ajax({
        url: loadposturl,
        type: "GET",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            SetUpGridShipUser('1');
            //SetUpGridSupportUser();
            //SetUpGridCompanyUser();
        },
        error: function (errormessage) {
            console.log(errormessage.responseText);
        }
    });
}

function CreateTableHeader(utyp) {
    
    if (utyp === 2) {
        $('#UserMasterTable thead tr').append('<th>User Name</th><th>User Code</th><th>Email</th><th>Gender</th><th>Edit</th><th> Delete</th>');
        SetUpGridCompanyUser(2);
    }
    else if (utyp === 1) {
        $('#UserMasterTable thead tr').append('<th>User Name</th><th>Created On</th><th>Email</th><th>Gender</th><th>Vessel IMO</th><th>Rank Name</th><th>Ship Name</th><th>Edit</th><th> Delete</th>');
        SetUpGridShipUser(utyp);
    }
        
}

function SetUpGridShipUser(UserType) {
    var loadposturl = $('#loaddata').val();

    //do not throw error
    $.fn.dataTable.ext.errMode = 'none';

   // var UserType = 1; //////////////////////////////////////////////////////////////////////// SupportUser = 0, ShipUser(Index) = 1, CompanyUser = 2.

    //check if datatable is already created then destroy iy and then create it
    if ($.fn.dataTable.isDataTable('#UserMasterTable')) {
        table = $('#UserMasterTable').DataTable();
        table.destroy();
    }

    // alert('hh');
    var table = $("#UserMasterTable").DataTable({
        //"dom": 'Bfrtip',
        //"rowReorder": false,
        "ordering": false,
        "filter": false, // this is for disable filter (search box)

        "ajax": {
            "url": loadposturl,
            "type": "POST",
            "datatype": "json",
            "data": { UserType: UserType }
        },
        "columns": [
            //{
            //    "data": "Order", "name": "Order", "autoWidth": true, "className": 'reorder'
            //},
            {
                "data": "UserName", "name": "UserName", "autoWidth": true
            },
            {
                "data": "CreatedOn1", "name": "CreatedOn1", "autoWidth": true
            },
            {
                "data": "Email", "name": "Email", "autoWidth": true
            },
            //{
            //    "data": "CreatedBy", "name": "CreatedBy", "autoWidth": true
            //},
            //{
            //    "data": "ModifiedBy", "name": "ModifiedBy", "autoWidth": true
            //},
            {
                "data": "Gender", "name": "Gender", "autoWidth": true
            },
            {
                "data": "VesselIMO", "name": "VesselIMO", "autoWidth": true
            },
            {
                "data": "RankName", "name": "RankName", "autoWidth": true
            },
            {
                "data": "ShipName", "name": "ShipName", "autoWidth": true
            },

            {
                "data": "UserId", "width": "50px", "render": function (data) {
                    return '<a href="#" class="btn btn-info btn-sm" style="background-color: #e90000;" onclick="GetUserByUserId(' + data + ')">Edit</a>';
                }
            },
            {
                "data": "UserId", "width": "50px", "render": function (d) {
                    //debugger;
                    return '<a href="#" class="btn btn-info btn-sm" style="background-color: #e90000;" onclick="DeleteUserMaster(' + d + ')">Delete</a>';


                }
            }

        ],
        "rowId": "UserId"
        //"dom": "Bfrtip"
    });
}

//function SetUpGridSupportUser(UserType) {
//    var loadposturl = $('#loaddata').val();

//    //do not throw error
//    $.fn.dataTable.ext.errMode = 'none';

//    var UserType = 0; //////////////////////////////////////////////////////////////////////// SupportUser = 0, ShipUser(Index) = 1, CompanyUser = 2.

//    //check if datatable is already created then destroy iy and then create it
//    if ($.fn.dataTable.isDataTable('#UserMasterTable')) {
//        table = $('#UserMasterTable').DataTable();
//        table.destroy();
//    }

//    // alert('hh');
//    var table = $("#UserMasterTable").DataTable({
//        "dom": 'Bfrtip',
//        "rowReorder": false,
//        "ordering": false,
//        "filter": false, // this is for disable filter (search box)

//        "ajax": {
//            "url": loadposturl,
//            "type": "POST",
//            "datatype": "json",
//            "data": { UserType: UserType },
//        },
//        "columns": [
//            //{
//            //    "data": "Order", "name": "Order", "autoWidth": true, "className": 'reorder'
//            //},
//            {
//                "data": "UserName", "name": "UserName", "autoWidth": true
//            },
//            {
//                "data": "CreatedOn1", "name": "CreatedOn1", "autoWidth": true
//            },
//            {
//                "data": "Email", "name": "Email", "autoWidth": true
//            },
//            //{
//            //    "data": "CreatedBy", "name": "CreatedBy", "autoWidth": true
//            //},
//            //{
//            //    "data": "ModifiedBy", "name": "ModifiedBy", "autoWidth": true
//            //},
//            {
//                "data": "Gender", "name": "Gender", "autoWidth": true
//            },
//            {
//                "data": "VesselIMO", "name": "VesselIMO", "autoWidth": true
//            },
//            {
//                "data": "RankName", "name": "RankName", "autoWidth": true
//            },
//            {
//                "data": "ShipName", "name": "ShipName", "autoWidth": true
//            },
     
//            {
//                "data": "UserId", "width": "50px", "render": function (data) {
//                    return '<a href="#" class="btn btn-info btn-sm" style="background-color: #e90000;" onclick="GetUserByUserId(' + data + ')">Edit</a>';
//                }
//            },
//            {
//                "data": "UserId", "width": "50px", "render": function (d) {
//                    //debugger;
//                    return '<a href="#" class="btn btn-info btn-sm" style="background-color: #e90000;" onclick="DeleteUserMaster(' + d + ')">Delete</a>';


//                }
//            }

//        ],
//        "rowId": "UserId",
//        "dom": "Bfrtip"
//    });
//}

function SetUpGridCompanyUser(UserType) {
    var loadposturl = $('#loaddata').val();

    //do not throw error
    $.fn.dataTable.ext.errMode = 'none';

    //var UserType = 2; //////////////////////////////////////////////////////////////////////// SupportUser = 0, ShipUser(Index) = 1, CompanyUser = 2.

    //check if datatable is already created then destroy iy and then create it
    if ($.fn.dataTable.isDataTable('#UserMasterTable')) {
        table = $('#UserMasterTable').DataTable();
        table.destroy();
    }

    // alert('hh');
    var table = $("#UserMasterTable").DataTable({
        //"dom": 'Bfrtip',
        //"rowReorder": false,
        "ordering": false,
        "filter": false, // this is for disable filter (search box)

        "ajax": {
            "url": loadposturl,
            "type": "POST",
            "datatype": "json",
            "data": { UserType: UserType }
        },
        "columns": [
            //{
            //    "data": "Order", "name": "Order", "autoWidth": true, "className": 'reorder'
            //},
            {
                "data": "UserName", "name": "UserName", "autoWidth": true
            },
            {
                "data": "UserCode", "name": "UserCode", "autoWidth": true
            },
            //{
            //    "data": "CreatedOn1", "name": "CreatedOn1", "autoWidth": true
            //},
            {
                "data": "Email", "name": "Email", "autoWidth": true
            },
            //{
            //    "data": "CreatedBy", "name": "CreatedBy", "autoWidth": true
            //},
            //{
            //    "data": "ModifiedBy", "name": "ModifiedBy", "autoWidth": true
            //},
            //{
            //    "data": "Gender", "name": "Gender", "autoWidth": true
            //},
            //{
            //    "data": "VesselIMO", "name": "VesselIMO", "autoWidth": true
            //},
            //{
            //    "data": "RankName", "name": "RankName", "autoWidth": true
            //},
            //{
            //    "data": "ShipName", "name": "ShipName", "autoWidth": true
            //},

            {
                "data": "UserId", "width": "50px", "render": function (data) {
                    return '<a href="#" class="btn btn-info btn-sm" style="background-color: #e90000;" onclick="GetUserByUserId(' + data + ')">Edit</a>';
                }
            },
            {
                "data": "UserId", "width": "50px", "render": function (d) {
                    //debugger;
                    return '<a href="#" class="btn btn-info btn-sm" style="background-color: #e90000;" onclick="DeleteUserMaster(' + d + ')">Delete</a>';


                }
            }

        ],
        "rowId": "UserId"
       // "dom": "Bfrtip"
    });
}




function DeleteUserMaster(UserId) {
    var e = $('#DeleteUserMaster').val();
    var ans = confirm("Are you sure you want to delete this Record?");
    if (ans) {
        // debugger;
        $.ajax({
            url: e,
            data: JSON.stringify({ UserId: UserId }),
            type: "POST",
            contentType: "application/json;charset=UTF-8",
            dataType: "json",
            success: function (result) {
                // debugger;

                if (result == -1) {
                    alert(" cannot be deleted as this is already used.");
                }
                else if (result == 0) {
                    alert(" cannot be deleted as this is already used.");
                }
                else {
                    loadData();
                }
            },
            error: function () {
                alert(" cannot be deleted as this is already used");
            }
        });
    }
}




function GetUserByUserId(UserId) {
    $('#UserName').css('border-color', 'lightgrey');
    var x = $("#GetUserByUserId").val();
    //alert(x);
    //debugger;
    $.ajax({
        url: x,
        data:
        {
            UserId: UserId
        },
        type: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            $('#UserId').val(result.UserId);
            if (userType === 1) {
                //debugger;
               
                $('#UserName').val(result.UserName);
                $('#Password').val(result.Password);
                //$('#CreatedOn').val(result.CreatedOn);
                //$('#Email').val(result.Email);
                //$('#CreatedBy').val(result.CreatedBy);
                //$('#ModifiedBy').val(result.ModifiedBy);
                $('#UserCode').val(result.UserCode);
                $('#Gender').val(result.Gender);
                $('#VesselIMO').val(result.VesselIMO);
                $('#RankId').val(result.RankId);
                $('#ShipId').val(result.ShipId);

                $('#IsAdmin').val(result.IsAdmin);
            }
            else if (userType === 2) {
                $('#cUserName').val(result.UserName);
                $('#cUserCode').val(result.UserCode);
                $('#cPassword').val(result.Password);
                $('#CreatedOn').val(result.CreatedOn);
                $('#Email').val(result.Email);
                //$('#CreatedBy').val(result.CreatedBy);
                //$('#ModifiedBy').val(result.ModifiedBy);
                $('#cGender').val(result.Gender);

                $('#cIsAdmin').val(result.IsAdmin);
            }
            LoadTab();

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

function confirmPass() {
    var pass = "";
    var confPass = "";
    if (selected_tab === 0) {
        pass = document.getElementById("Password").value
        confPass = document.getElementById("ConfirmPassword").value
        if (pass != confPass) {
            alert('Wrong confirm password !');
        }
    }
    else if (selected_tab === 1) {
        pass = document.getElementById("cPassword").value
        confPass = document.getElementById("cConfirmPassword").value
        if (pass != confPass) {
            alert('Wrong confirm password !');
        }
    }

}

///---------------Tabs---------------------
var selected_tab = 0;
var userType = 2;
function LoadTab() {
    //var tab = $('#tab').val();
    //if (tab === "Edit") {
    //    selected_tab = 2;
    //}
    //else if (tab === "Delete") {
    //    selected_tab = 3;
    //}
    
    if (userType === 1) {
        selected_tab = 0;
        $('#liTab2').prop('hidden', true);
    }
    else if (userType === 2) {
        selected_tab = 1;
        $('#liTab2').prop('hidden', false);
    }

    var tabs = $("#tabs").tabs({
        active: selected_tab,
        //select: function (e, i) {
        //    selected_tab = i.index;
        //}
    });
    if (userType === 1) {
        $('#liTab2').prop('disabled', true);
    }
    else {
        $('#liTab2').prop('disabled', false);
    }
}

$('#tabs').click('tabsselect', function (event, ui) {

    selected_tab = $("#tabs").tabs('option', 'active');
    var id = $('#Id').val();
    if (selected_tab === 2 && id === "0") {
        //$('#TestCode1').prop('disabled', true);
        //$('#TestName1').prop('disabled', true);
        //$('#Result1').prop('disabled', true);
        //$('#Formula1').prop('disabled', true);
        //$('#btnSubmit').prop('disabled', true);
    }
    else {
        //$('#TestCode1').prop('disabled', false);
        //$('#TestName1').prop('disabled', false);
        //$('#Result1').prop('disabled', false);
        //$('#Formula1').prop('disabled', false);
        //$('#btnSubmit').prop('disabled', false);
    }
});

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
                $('#VesselIMO').val(result.VesselIMONumber);
                
            },
            error: function (errormessage) {
                alert(errormessage.responseText);
            }
        });
    }
}

function CheckEmail(email) {
    var regex = /^([a-zA-Z0-9_\.\-\+])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;
    var regex1 = /^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$/;
    if (!regex1.test(email)) {
        alert('Wrong formated emailID!');
        $('#Email').css('border-color', 'Red');
        return false;
    } else {
        $('#Email').css('border-color', 'lightgrey');
        return true;
    }
}

function GetuUserCode() {
    var chk = true;
    var shipId = '0';
    var userTyp = '0';
    var rank = '0';
    var usrName = '';
    if (selected_tab === 0) {
        shipId = $('#ShipId').val();
        userTyp = 1;
        rank = $('#RankId').val();
    }
    else if (selected_tab === 1) {
        userTyp = 2;
        if ($('#cUserName').val().length <= 5) {
            $('#cUserName').css('border-color', 'Red');
            chk = false;
        } else {
            $('#cUserName').css('border-color', 'lightgrey');
            usrName = $('#cUserName').val();
        }
    }
    
    
    var URL = $("#urlGenerateCode").val();
    if (chk) {
        $.ajax({
            url: URL,
            type: "POST",
            data: JSON.stringify({ 'userType': userTyp, 'shipId': shipId, 'rankId': rank, 'userName': usrName }),
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {
                //debugger;
                if (selected_tab === 0) {
                    $('#UserCode').val(result);
                }
                else if (selected_tab === 1) {
                    $('#cUserCode').val(result);
                }
            },
            error: function (errormessage) {
                alert(errormessage.responseText);
            }
        });
    }
}
//-------------End--Tabs----------------------


