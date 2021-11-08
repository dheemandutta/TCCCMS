var tmpApproverList = [];
var approversCount = 0;
function validate() {
    var isValid = true;

    if ($("#fileUpload").get(0).files.length === 0) {
        $('#fileUpload').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#fileUpload').css('border-color', 'lightgrey');
    }
    //if (tmpApproverList.length === 0) {

    //    $('#ddlUser').css('border-color', 'Red');
    //    $('#ddlApproverLevel').css('border-color', 'Red');
    //    isValid = false;
    //}
    //else {
    //    $('#ddlUser').css('border-color', 'lightgrey');
    //    $('#ddlApproverLevel').css('border-color', 'lightgrey');
    //}
    if (tmpApproverList.length === 0) {

        $('#ddlApproverUser').css('border-color', 'Red');
        //$('#ddlApproverLevel').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#ddlApproverUser').css('border-color', 'lightgrey');
        //$('#ddlApproverLevel').css('border-color', 'lightgrey');
    }
        

    return isValid;
}
function validateA() {
    var isValid = true;

    if ($("#aFileUpload").get(0).files.length === 0) {
        $('#aFileUpload').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#aFileUpload').css('border-color', 'lightgrey');
    }
    

    return isValid;
}

function validateR() {
    var isValid = true;

    if ($("#fileUpload").get(0).files.length === 0) {
        $('#fileUpload').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#fileUpload').css('border-color', 'lightgrey');
    }
    

    return isValid;
}

function ClearFields() {
    $('#fileUpload').val('');
    $("#ddlUser").prop('selectedIndex', -1);
    $("#ddlApproverLevel").prop('selectedIndex', -1);
}
function UploadFilledUpForm() {
    
    var url = $('#urlFilledUpForm').val();
    var approverList = [];
    for (var i = 0; i < tmpApproverList.length; i++) {
        //var cur = tmpApproverList[i];
        //if (cur.ID === user.UserId) {
        //    exist = true;
        //    break;
        //}
        approverList.push({
            ApproverId: tmpApproverList[i].ApproverId,
            UserId: tmpApproverList[i].UserId
        });
    }
    //********--------------------------------------------------------------------------
    //Checking whether FormData is available in browser  
    if (window.FormData !== undefined) {

        var fileUpload = $("#fileUpload").get(0);
        var files = fileUpload.files;
        // Create FormData object  
        var fileData = new FormData();

        // Looping over all files and add it to FormData object  
        for (var i = 0; i < files.length; i++) {
            fileData.append(files[i].name, files[i]);
        }

        // Adding one more key to FormData object 
        //fileData.append('formVersion', frmVersion);
        fileData.append('approvers', JSON.stringify(approverList));

        $.ajax({
            url: url,
            type: "POST",
            //datatype: "json",
            //contentType: "application/json; charset=utf-8",
            contentType: false, // Not to set any content header  
            processData: false, // Not to process data  
            data: fileData,
            //data: { categoryId: y},
            success: function (result) {
                alert(result);
                //ClearFields();
                //toastr.options = {
                //    "closeButton": false,
                //    "debug": false,
                //    "newestOnTop": false,
                //    "progressBar": false,
                //    "positionClass": "toast-bottom-full-width",
                //    "preventDuplicates": false,
                //    "onclick": null,
                //    "showDuration": "300",
                //    "hideDuration": "1000",
                //    "timeOut": "5000",
                //    "extendedTimeOut": "1000",
                //    "showEasing": "swing",
                //    "hideEasing": "linear",
                //    "showMethod": "fadeIn",
                //    "hideMethod": "fadeOut"
                //};

                //toastr.success("Form Updated Successfully");
            },
            error: function (err) {
                alert(err.statusText);
            }
        });
    } else {
        alert("FormData is not supported.");
    }

}
///--------below-----UploadFilledUpFormNew() created on 7th APR 2021 due change approval logic that company users which has approval rights can approve froms
function UploadFilledUpFormNew() {
    //$('#fileUpload').val('');
    var url = $('#urlFilledUpForm').val();
    //var url = "/UserMaster/UploadFilledUpForm";
    //********--------------------------------------------------------------------------
    //Checking whether FormData is available in browser  
    if (window.FormData !== undefined) {

        var fileUpload = $("#fileUpload").get(0);
        var files = fileUpload.files;
        //var files = $('#fileUpload')[0];
        // Create FormData object  
        var fileData = new FormData();

        // Looping over all files and add it to FormData object  
        for (var i = 0; i < files.length; i++) {
            fileData.append(files[i].name, files[i]);
        }
        //var e = {
        //    ApproverUserId: '9',
        //    Position: 'AGM'
        //}
        

        // Adding one more key to FormData object 
        //fileData.append('task', '1');
        

        $.ajax({
            url: url,
            type: "POST",
            datatype: "json",
            //contentType: "application/json; charset=utf-8",
            contentType: false, // Not to set any content header  
            processData: false, // Not to process data  
            data: fileData,
            //data: { categoryId: y},
            success: function (result) {
                alert(result);
                $('#fileUpload').val('');
                //ClearFields();
                ClearFields2();
                $('#filledUpFormModal').modal('hide');

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

                toastr.success("Form Updated Successfully");
                $('#filledUpFormModal').modal('hide');
            },
            error: function (err) {
                alert(err.statusText);
            }
        });
    } else {
        alert("FormData is not supported.");
    }

}
function LoadApprover() {
    if ($("#ddlUser").val() === 0 || $("#ddlUser").val() < 0) {
        $('#ddlUser').css('border-color', 'Red');
    }
    else {
        $('#ddlUser').css('border-color', 'lightgrey');
        AddTempApprover();

        if (tmpApproverList.length != 0) {
            SetUpTempApproverGrid();
            $('#divTempApprover').removeClass('displayNone');
        }
    }

}

function AddTempApprover(user) {
    var idx = 0
    //var rank = user.Rank;
    var exist = false;

    if (tmpApproverList.length <= 6) {
        //tmpApproverList.r
        var userId = $('#ddlUser').val();
        idx = tmpApproverList.length + 1;

        for (var i = 0; i < tmpApproverList.length; i++) {
            var cur = tmpApproverList[i];
            if (cur.UserId === userId) {
                exist = true;
                break;
            }
        }

        if (!exist) {
            tmpApproverList.push({
                SL: idx,
                UserId: userId,
                UserName: $("#ddlUser option:selected").text(),
                //UserCode: user.UserCode,
                ApproverId: $('#ddlApproverLevel').val(),
                ApproverLevel: $("#ddlApproverLevel option:selected").text(),
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

        "data": tmpApproverList,
        "columns": [
            {
                "data": "SL", "name": "SL", "autoWidth": true
            },
            {
                "data": "UserName", "name": "UserName", "autoWidth": true
            },
            {
                "data": "ApproverLevel", "name": "ApproverLevel", "autoWidth": true
            },
            {
                "data": "UserId", "width": "50px", "render": function (data) {
                    return '<a href="#" onclick="RemoveTempApprover(' + data + ')">Remove</a>';
                }
            }

        ],
        "rowId": "UserId",
    });
}

function RemoveTempApprover(id) {
    var idx = 1
    var objList = [];
    for (var i = 0; i < tmpApproverList.length; i++) {
        var cur = tmpApproverList[i];
        if (cur.UserId != id) {
            objList.push({
                SL: idx,
                UserId: cur.UserId,
                UserName: cur.UserName,
                //UserCode: cur.Code,
                ApproverId: cur.ApproverId,
                ApproverLevel: cur.ApproverLevel
            });
            idx = idx + 1;
        }
    }

    tmpApproverList = objList;
    if (tmpApproverList.length === 0) {
        $('#divTempApprover').addClass('displayNone');
    }

    SetUpTempApproverGrid();
}

function ApproveFilledUpForm(approverUserId,filledUpFormId,uploadedFormName,catId) {

    var posturl = $('#urlApproveFilledUpForm').val();
    var   Forms = {
        ID: filledUpFormId,
        ApproverUserId: approverUserId,
        FilledUpFormName: uploadedFormName,
        CategoryId: catId
    };
       
    alert("Do You want to Approve this Form..?")
    $.ajax({
        url: posturl,
        data: JSON.stringify(Forms),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",

        success: function (result) {
            //loadData();
            alert("Approved Successfully")
            LoadFormsApprovalList();
            window.location = '/Forms/FormsApprovalList';

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

            toastr.success("Approved Successfully");

            //clearTextBox();
        },
        error: function (errormessage) {
            console.log(errormessage.responseText);
        }
    });
    
}


function LoadFormsApprovalList() {

    var geturl = $('#urlFormsApprovalList').val();
    
    
    $.ajax({
        url: geturl,
        //data: JSON.stringify(Forms),
        type: "GET",
        contentType: "application/json;charset=utf-8",
        dataType: "json",

        success: function (result) {
            
            //clearTextBox();
        },
        error: function (errormessage) {
            console.log(errormessage.responseText);
        }
    });

}



function SendMailForapproval(fromId,uploadedFormName,userId,task) {



    $.ajax({
        url: '/Forms/SendMailForapproval',
        type: "POST",
        datatype: "json",
        contentType: "application/json; charset=utf-8",
        //contentType: false, // Not to set any content header  
        processData: false, // Not to process data  
        data: JSON.stringify({ approvalId: fromId, approverUserId: userId, formName: uploadedFormName, task: task}),
        success: function (result) {
            alert(result);
            //toastr.options = {
            //    "closeButton": false,
            //    "debug": false,
            //    "newestOnTop": false,
            //    "progressBar": false,
            //    "positionClass": "toast-bottom-full-width",
            //    "preventDuplicates": false,
            //    "onclick": null,
            //    "showDuration": "300",
            //    "hideDuration": "1000",
            //    "timeOut": "5000",
            //    "extendedTimeOut": "1000",
            //    "showEasing": "swing",
            //    "hideEasing": "linear",
            //    "showMethod": "fadeIn",
            //    "hideMethod": "fadeOut"
            //};

            //toastr.success(result);
        },
        error: function (err) {
            //alert(err.statusText);
            alert(result);
        }
    });


}


//-------------------------New Upload/Approver Function------Created -on 23rd jul 2021-----approver selection logic has changed----------------------------------------------
function ClearFields2() {
    $('#fileUpload').val('');
    //$("#ddlApproverUser").prop('selectedIndex', -1);
    $("#ddlApproverUser").val('-1');

    $('#ddlApproverUser').css('border-color', 'lightgrey');
    $('#hdnOriginalFormName').val("");
    $('#hdnFormsCategory').val("");
    tmpApproverList = [];
    $('#divTempApprover').addClass('displayNone');

}

function ClearFields3() {
    $("#ddlApproverUser").val('-1');
    $('#fileUpload').val('');
    $('#ddlApproverUser').css('border-color', 'lightgrey');
    $('#hdnOriginalFormName').val("");
    $('#hdnFormsCategory').val("");
    $('#txtReviewedForm').val("");
    $('#lblReviewedForm').text("");
    tmpApproverList = [];
    $('#divTempApprover').addClass('displayNone');
    SetUpTempApproverGridNew();

}
function UploadFilledUpFormWithApprovers() {

    var originalForm = $('#hdnOriginalFormName').val();

    alert("All the fields are correct ..? \n Please Confirm..!");
    var catId = $('#hdnFormsCategory').val();
    var task = $('#taskRadio input:radio:checked').val()
    //$('#fileUpload').val('');
    var url = $('#urlFilledUpForm').val();
    //var url = "/UserMaster/UploadFilledUpFormNew";// Added on 19th Aug 2021
    //********--------------------------------------------------------------------------
    //Checking whether FormData is available in browser  
    if (window.FormData !== undefined) {
        //if ((catId !== "16" || catId !== "17") && task === "R") {
        //    alert("This type of forms are not for Review..!")
        //}
        //else {
            if (validate()) {
                var fileUpload = $("#fileUpload").get(0);
                var files = fileUpload.files;
                var uploadedFormName = '';
                //var files = $('#fileUpload')[0];
                // Create FormData object  
                var fileData = new FormData();

                // Looping over all files and add it to FormData object  
                for (var i = 0; i < files.length; i++) {
                    fileData.append(files[i].name, files[i]);

                    uploadedFormName = files[i].name;
                    //uploadedFormName = uploadedFormName.substr((uploadedFormName.firstIndexOf('.') + 1));
                    uploadedFormName = uploadedFormName.split('.').slice(0, -1).join('.');
                }
                //var e = {
                //    ApproverUserId: '9',
                //    Position: 'AGM'
                //}
                var task = $("#taskRadio input[name='inlineRadioOptions']:checked").val();

                // Adding one more key to FormData object 
                fileData.append('approvers', JSON.stringify(tmpApproverList));
                fileData.append('task', task);
                //for (var i = 0; i < tmpApproverList.length; i++) {
                //    //fileData.append(approvers["ApproverUserId"], tmpApproverList[i].ID);
                //    //fileData.append(approvers["Position"], tmpApproverList[i].Position);
                //    //fileData.append(approvers["UserName"], tmpApproverList[i].Name);

                //    //fileData.append("approvers["+i+"][ApproverUserId]", tmpApproverList[i].ID);
                //    //fileData.append("approvers["+i+"][Position]", tmpApproverList[i].Position);
                //    //fileData.append("approvers[" + i + "][UserName]", tmpApproverList[i].Name);

                //    //fileData.append("approvers[" + i + "]", tmpApproverList[i].ID);

                //}
                ////fileData.append("approvers[ApproverUserId]", tmpApproverList[0].ID);
                ////fileData.append("approvers[Position]", tmpApproverList[0].Position);
                if (uploadedFormName === originalForm) {
                    $.ajax({
                        url: url,
                        type: "POST",
                        datatype: "json",
                        //contentType: "application/json; charset=utf-8",
                        contentType: false, // Not to set any content header  
                        processData: false, // Not to process data  
                        data: fileData,
                        //data: { categoryId: y},
                        success: function (result) {
                            alert(result);
                            $('#fileUpload').val('');
                            //ClearFields();
                            ClearFields2();
                            $('#filledUpFormModal').modal('hide');

                            //toastr.options = {
                            //    "closeButton": false,
                            //    "debug": false,
                            //    "newestOnTop": false,
                            //    "progressBar": false,
                            //    "positionClass": "toast-bottom-full-width",
                            //    "preventDuplicates": false,
                            //    "onclick": null,
                            //    "showDuration": "300",
                            //    "hideDuration": "1000",
                            //    "timeOut": "5000",
                            //    "extendedTimeOut": "1000",
                            //    "showEasing": "swing",
                            //    "hideEasing": "linear",
                            //    "showMethod": "fadeIn",
                            //    "hideMethod": "fadeOut"
                            //};

                            //toastr.success("Form Updated Successfully");
                            $('#filledUpFormModal').modal('hide');
                        },
                        error: function (err) {
                            alert(err.statusText);
                        }
                    });
                }
                else {
                    alert('You are trying to upload another from..! \n Please Choose Exact form..!"')
                }

            }

        //}
    }
    else {
            alert("FormData is not supported.");
    }

 }

//--Added this  fnc on 19th Aug 2021
function UploadApprovedFilledUpForm() {

    var originalForm = $('#hdnAOriginalFormName').val();

    var url = $('#urlApprovedFormsUpload').val();
    //var url = "/UserMaster/UploadFilledUpFormNew";// Added on 19th Aug 2021
    //********--------------------------------------------------------------------------
    //Checking whether FormData is available in browser  
    if (window.FormData !== undefined) {
        
        if (validateA()) {
            var fileUpload = $("#aFileUpload").get(0);
            var files = fileUpload.files;
            var uploadedFormName = '';
            //var files = $('#fileUpload')[0];
            // Create FormData object  
            var fileData = new FormData();

            // Looping over all files and add it to FormData object  
            for (var i = 0; i < files.length; i++) {
                fileData.append(files[i].name, files[i]);

                uploadedFormName = files[i].name;
                //uploadedFormName = uploadedFormName.substr((uploadedFormName.firstIndexOf('.') + 1));
                //uploadedFormName = uploadedFormName.split('.').slice(0, -1).join('.');
            }
           
            var task = $("#taskRadio input[name='inlineRadioOptions']:checked").val();

            // Adding one more key to FormData object 
            fileData.append('formId', $('#hdnAFilledUpFormID').val());
            fileData.append('approverId', $('#hdnAApproverUserID').val());
            fileData.append('formName', $('#hdnAOriginalFormName').val());
            fileData.append('cat', $('#hdnAFormsCategory').val());
            
            if (uploadedFormName === originalForm) {
                $.ajax({
                    url: url,
                    type: "POST",
                    datatype: "json",
                    //contentType: "application/json; charset=utf-8",
                    contentType: false, // Not to set any content header  
                    processData: false, // Not to process data  
                    data: fileData,
                    //data: { categoryId: y},
                    success: function (result) {
                        alert(result);
                        $('#afileUpload').val('');
                        //ClearFields();
                        ClearApprovedFormInfo();
                        $('#approvedFilledUpFormModal').modal('hide');

                        LoadFormsApprovalList();
                        window.location = '/Forms/FormsApprovalList';
                    },
                    error: function (err) {
                        alert(err.statusText);
                    }
                });
            }
            else {
                alert('You are trying to upload another from..! \n Please Choose Exact form..!"')
            }

        }

        
    }
    else {
        alert("FormData is not supported.");
    }

}

function GetApproverUsersPositionFroDropDown() {

    var x = $("#urlApproverUserPosition").val();
    //var x = '/Approver/GetAppr';
    $.ajax({
        url: x,
        type: "GET",
        //data: JSON.stringify({ 'userId': userId }),
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            //debugger;
            console.log(result)
            var ddlApproverUser = $('#ddlApproverUser');
            ddlApproverUser.find('option').remove();

            $.each(result, function () {
                ddlApproverUser.append('<option value=' + this.UserId + '>' + this.Position + '</option>');
            });
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });

}

$('#ddlApproverUser').on('change', function () {
    GetUserByApproverUserId(this.value);
    //$('#ddRank').prop('disabled', true);
});

function GetUserByApproverUserId(id) {
    var userId = id;
    var x = $("#urlApproverUser").val();
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
                    //$('#ddlRank').val(result.RankId);
                    AddTempApproverNew(result);
                    SetUpTempApproverGridNew();
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

function AddTempApproverNew(user) {
    var a = tmpApproverList.length;
    var b = $('#hdnApproversCount').val();
    var c = approversCount;
    var totalCount = eval(a + c);
    var totalApproverOrReviewer = 6;// changed into 0 on 19th Aug 2021
    b = b.replace(/"/g, '\\"')
    //var totalCount = a + b;
    var idx = 0;
    //var rank = user.Rank;
    var exist = false;

    var catId = $('#hdnFormsCategory').val();
    var task = $('#taskRadio input:radio:checked').val()
    var tsk = $('#hdnTask').val();/* for Reviewed from Approval*/
    //if (tmpApproverList.length === 0) {
    //    tmpApproverList.push({
    //        SL: idx,
    //        ID: 100,
    //        Code: COD1234,
    //        Rank: Master,
    //        Name: Name1

    //    });
    //}


    ////Below three condition added on 5th Aug 2021 Due to new logic for form Review and Approved reviewed form
    //if ((catId === '16' || catId === '17') && task === 'R')
    //    totalApproverOrReviewer = 2;
    //else if ((catId === '16' || catId === '17') && task === 'A')
    //    totalApproverOrReviewer = 1;
    //else if ((catId === '16' || catId === '17') && tsk === 'A')/* for Reviewed from Approval*/
    //    totalApproverOrReviewer = 1;
    //else 
    //    totalApproverOrReviewer = 6;
    ////---------above if..else condition commented on 19th Aug 2021; due to approval logic has changed again-------------


    if (totalCount < totalApproverOrReviewer) {
        //tmpApproverList.r
        idx = tmpApproverList.length + 1;

        for (var i = 0; i < tmpApproverList.length; i++) {
            var cur = tmpApproverList[i];
            if (cur.ID === user.ApproverUserId) {
                exist = true;
                break;
            }
        }

        if (!exist) {
            tmpApproverList.push({
                SL: idx,
                ID: user.ApproverUserId,
                Position: user.Position,
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

function SetUpTempApproverGridNew() {
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
                "data": "Position", "name": "Position", "autoWidth": true
            },
            {
                "data": "Name", "name": "Name", "autoWidth": true
            },
            {
                "data": "ID", "width": "50px", "render": function (data) {
                    return '<a href="#" onclick="RemoveTempApproverNew(' + data + ')">Remove</a>';
                }
            }

        ],
        "rowId": "ID",
    });
}


function RemoveTempApproverNew(id) {
    var idx = 1
    var objList = [];
    for (var i = 0; i < tmpApproverList.length; i++) {
        var cur = tmpApproverList[i];
        if (cur.ID != id) {
            objList.push({
                SL: idx,
                ID: cur.ID,
                Position: cur.Position,
                Name: cur.Name

            });
            idx = idx + 1;
        }
    }

    tmpApproverList = objList;
    if (tmpApproverList.length === 0) {
        $('#divTempApprover').addClass('displayNone');
    }

    SetUpTempApproverGridNew();
}



function SetHdnFormName(frm,cat) {
    $('#hdnOriginalFormName').val(frm);
    $('#hdnFormsCategory').val(cat);
}

function SetReviewedFormName(frm,cat) {
    $('#txtReviewedForm').val(frm);
    $('#lblReviewedForm').text(frm);
    $('#hdnFormsCategory').val(cat);
}

function SetApprovedFormInfo(approverId,frmId, frm, cat) {

    $('#hdnAApproverUserID').val(approverId);
    $('#hdnAFilledUpFormID').val(frmId);
    $('#hdnAOriginalFormName').val(frm);
    $('#hdnAFormsCategory').val(cat);
}
function ClearApprovedFormInfo() {
    $('#hdnAApproverUserID').val('');
    $('#hdnAFilledUpFormID').val('');
    $('#hdnAOriginalFormName').val('');
    $('#hdnAFormsCategory').val('');
    $('#aFileUpload').val('');
}

function UploadReviewedFilledUpForm() {

    //This function called when Approver/Reviewer Upload the reviewed form
    var originalForm = $('#hdnOriginalFormName').val();

   // alert("All the fields are correct ..? \n Please Confirm..!");
    //$('#fileUpload').val('');
    var url = $('#urlReviewedFormsUpload').val();
    //var url = "/UserMaster/UploadFilledUpForm";
    //********--------------------------------------------------------------------------
    //Checking whether FormData is available in browser  
    if (window.FormData !== undefined) {
        if (validateR()) {
            var fileUpload = $("#fileUpload").get(0);
            var files = fileUpload.files;
            var uploadReviewedFormName = '';
            //var files = $('#fileUpload')[0];
            // Create FormData object  
            var fileData = new FormData();

            // Looping over all files and add it to FormData object  
            for (var i = 0; i < files.length; i++) {
                fileData.append(files[i].name, files[i]);

                uploadReviewedFormName = files[i].name;
                //uploadedFormName = uploadedFormName.substr((uploadedFormName.firstIndexOf('.') + 1));
                //uploadReviewedFormName = uploadReviewedFormName.split('.').slice(0, -1).join('.');
            }
            
            //var task = $("#taskRadio input[name='inlineRadioOptions']:checked").val();
            var task = 'R';

            // Adding one more key to FormData object 
            //fileData.append('approvers', JSON.stringify(tmpApproverList));
            fileData.append('task', task);
            
            if (uploadReviewedFormName === originalForm) {
                $.ajax({
                    url: url,
                    type: "POST",
                    datatype: "json",
                    //contentType: "application/json; charset=utf-8",
                    contentType: false, // Not to set any content header  
                    processData: false, // Not to process data  
                    data: fileData,
                    //data: { categoryId: y},
                    success: function (result) {
                        alert(result);
                        $('#fileUpload').val('');
                        //ClearFields();
                        //ClearFields2();
                        $('#reviewedFilledUpFormModal').modal('hide');
                        LoadFormsApprovalList();
                        window.location = '/Forms/FormsApprovalList';

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

                        toastr.success("Form Updated Successfully");
                        $('#filledUpFormModal').modal('hide');
                    },
                    error: function (err) {
                        alert(err.statusText);
                    }
                });
            }
            else {
                alert('You are trying to upload another from..! \n Please Choose Exact form..!"')
            }

        }

    }
    else {
        alert("FormData is not supported.");
    }
}

////////Old function and commented on15th Sep 2021
//function UploadReviewedFilledUpFormWithApprovers() {
//    //var url = $('#urlReviewedFormForApproval').val();
//    //var url = "/Forms/UploadFilledUpReviewedFormForApproval";// Commented on 19th Aug 2021
//    var url = "/Forms/UploadFilledUpReviewedFormForApprovalNew"; // Added on 19th Aug 2021
//    if (tmpApproverList.length === 0) {

//        $('#ddlApproverUser').css('border-color', 'Red');

//    }
//    else {
//        var frm = $('#txtReviewedForm').val();

//        $.ajax({
//            url: url,
//            type: "POST",
//            datatype: "json",
//            contentType: "application/json; charset=utf-8",  
//           // processData: false, // Not to process data  
//            data: JSON.stringify({
//                formName: frm,
//               approvers: JSON.stringify(tmpApproverList)
//            }),
//            //data: { categoryId: y},
//            success: function (result) {
//                alert(result);
//                //$('#txtReviewedForm').val('');
//                //$('#lblReviewedForm').text('');
//                //tmpApproverList = [];
//                $('#reviewedFilledUpFormModal').modal('hide');

//                ClearFields3();

//                toastr.options = {
//                    "closeButton": false,
//                    "debug": false,
//                    "newestOnTop": false,
//                    "progressBar": false,
//                    "positionClass": "toast-bottom-full-width",
//                    "preventDuplicates": false,
//                    "onclick": null,
//                    "showDuration": "300",
//                    "hideDuration": "1000",
//                    "timeOut": "5000",
//                    "extendedTimeOut": "1000",
//                    "showEasing": "swing",
//                    "hideEasing": "linear",
//                    "showMethod": "fadeIn",
//                    "hideMethod": "fadeOut"
//                };

//                toastr.success("Form Uploaded Successfully");
//                $('#reviewedFilledUpFormModal').modal('hide');
//            },
//            error: function (err) {
//                alert(err.statusText);
//                $('#reviewedFilledUpFormModal').modal('hide');

//                ClearFields3();
//            }
//        });
//    }

    
//}

/////New function Modified on 15th Sep 2021


function UploadReviewedFilledUpFormWithApprovers() {
    ////////--this function called when ship user upload the Reviewed form for Approval...

    //var url = $('#urlReviewedFormForApproval').val();
    //var url = "/Forms/UploadFilledUpReviewedFormForApproval";// Commented on 19th Aug 2021
    var url = "/Forms/UploadFilledUpReviewedFormForApprovalNew"; // Added on 19th Aug 2021
    if (tmpApproverList.length === 0) {

        $('#ddlApproverUser').css('border-color', 'Red');

    }
    else {
        var frm = $('#txtReviewedForm').val();

        if (window.FormData !== undefined) {

            var fileUpload = $("#fileUpload").get(0);
            var files = fileUpload.files;
            var uploadReviewedFormName = '';
            // Create FormData object  
            var fileData = new FormData();
            // Looping over all files and add it to FormData object  
            for (var i = 0; i < files.length; i++) {
                fileData.append(files[i].name, files[i]);

                uploadReviewedFormName = files[i].name;
            }

            fileData.append('formName', frm);
            fileData.append('approvers', JSON.stringify(tmpApproverList));

            if (files.length === 0) {

                alert("No Selected Form");

                $.ajax({
                    url: url,
                    type: "POST",
                    datatype: "json",
                    //contentType: "application/json; charset=utf-8",
                    // processData: false, // Not to process data  
                    //data: JSON.stringify({
                    //    formName: frm,
                    //    approvers: JSON.stringify(tmpApproverList)
                    //}),
                    contentType: false, // Not to set any content header  
                    processData: false, // Not to process data  
                    data: fileData,
                    success: function (result) {
                        alert(result);
                        $('#reviewedFilledUpFormModal').modal('hide');

                        ClearFields3();

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

                        toastr.success("Form Uploaded Successfully");
                        $('#reviewedFilledUpFormModal').modal('hide');
                    },
                    error: function (err) {
                        alert(err.statusText);
                        $('#reviewedFilledUpFormModal').modal('hide');

                        ClearFields3();
                    }
                });
            }
            //else if (files.length != 0 && uploadReviewedFormName === frm) {
            else if (files.length != 0 ) {
                //alert("Selected Form  and Reviewed form Name are same ..?");
                confirm("Selected Form  and Reviewed form are same ..?")

                $.ajax({
                    url: url,
                    type: "POST",
                    datatype: "json",
                    //contentType: "application/json; charset=utf-8",
                    // processData: false, // Not to process data  
                    //data: JSON.stringify({
                    //    formName: frm,
                    //    approvers: JSON.stringify(tmpApproverList)
                    //}),
                    contentType: false, // Not to set any content header  
                    processData: false, // Not to process data  
                    data: fileData,
                    success: function (result) {
                        alert(result);
                        $('#reviewedFilledUpFormModal').modal('hide');

                        ClearFields3();

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

                        toastr.success("Form Uploaded Successfully");
                        $('#reviewedFilledUpFormModal').modal('hide');
                    },
                    error: function (err) {
                        alert(err.statusText);
                        $('#reviewedFilledUpFormModal').modal('hide');

                        ClearFields3();
                    }
                });
            }
            else {
                alert("Selected Form Name Should be same ..!");
            }
             

            
        }
        else {
            alert("FormData is not supported.");
        }


        
    }


}

function Test() {
    //var url = $('#urlReviewedFormForApproval').val();
    
        var frm = $('#txtReviewedForm').val();

    $.ajax({
        url: '/Forms/UploadFilledUpReviewedFormFor',
        data: JSON.stringify( {
            formName: frm
        }),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",


        success: function (result) {
            //loadData();
            //$('#myModal').modal('hide');
            alert('Added Successfully');

        },
        error: function (errormessage) {
            alert(errormessage);
            console.log(errormessage.responseText);
        }
    });
    
}



