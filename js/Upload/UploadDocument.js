    function EnableFileControl(x) {
    //var catId = x.value;
    var catId = x
    if (catId > 0) {
        $("#fileUpload").prop('disabled', false);
    }
    else {
        $("#fileUpload").prop('disabled', true);
    }
}

function UploadFiles() {
    var res = validate();
    if (res == false) {
        return false;
    }
    //var catId = $("#drpCategory").value;
    //var fileUpload = $("#fileUpload").get(0);
    //var files = fileUpload.files;
    //var fileData = new FormData();
    //for (var i = 0; i < files.length; i++) {
    //       fileData.append(files[i].name, files[i]);
    // }

    //$.ajax({
    //    type: 'POST',
    //    url: '/UploadDocument/UploadFiles',
    //    data: JSON.stringify({ categoryId: '1', fileData: fileData }),
    //    datatype: "json",
    //    contentType: "application/json; charset=utf-8",
    //    processData: false,
    //    success: function (data) {
    //        alert('success');
    //    },
    //    error: function () {
    //        alert('error');
    //    }
    //});

    //********--------------------------------------------------------------------------
    if (res) {
        //Checking whether FormData is available in browser  
        if (window.FormData !== undefined) {
            var catId = $("#ddlCategory").val();
            var catName = $("#ddlCategory option:selected").text();
            var fileUpload = $("#fileUpload").get(0);
            var files = fileUpload.files;
            // Create FormData object  
            var fileData = new FormData();
            var cate = {//--Not required
                ID: '2',
                CatecoryName: 'Admin'

            };

            // Looping over all files and add it to FormData object  
            for (var i = 0; i < files.length; i++) {
                fileData.append(files[i].name, files[i]);
            }

            // Adding one more key to FormData object  
            fileData.append('category', cate);//--Not required
            fileData.append('categoryId', catId);
            fileData.append('categoryName', catName);

            $.ajax({
                url: '/Document/UploadFiles',
                type: "POST",
                //datatype: "json",
                //contentType: "application/json; charset=utf-8",
                contentType: false, // Not to set any content header  
                processData: false, // Not to process data  
                data: fileData,
                //data: { categoryId: y},
                success: function (result) {
                    //alert(result);
                    ClearFields();
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
                },
                error: function (err) {
                    alert(err.statusText);
                }
            });
        } else {
            alert("FormData is not supported.");
        }  

    }
     
}
function UploadAndUpdateForm() {
    var res = validateForUpdateForm();
    if (res == false) {
        return false;
    }
    
    //********--------------------------------------------------------------------------
    if (res) {
        //Checking whether FormData is available in browser  
        if (window.FormData !== undefined) {
            var catId = $("#ddlCategory").val();
            var catName = $("#ddlCategory option:selected").text();
            var frmName = $("#ddlForms option:selected").text();
            var frmVersion = $("#txtVersion").val();
            var fileUpload = $("#fileUpload").get(0);
            var files = fileUpload.files;
            // Create FormData object  
            var fileData = new FormData();
            
            // Looping over all files and add it to FormData object  
            for (var i = 0; i < files.length; i++) {
                fileData.append(files[i].name, files[i]);
            }

            // Adding one more key to FormData object  
            //fileData.append('category', cate);//--Not required
            fileData.append('categoryId', catId);
            fileData.append('categoryName', catName);
            fileData.append('formName', frmName);
            fileData.append('formVersion', frmVersion);

            $.ajax({
                url: '/Document/UploadAndUpdateForm',
                type: "POST",
                //datatype: "json",
                //contentType: "application/json; charset=utf-8",
                contentType: false, // Not to set any content header  
                processData: false, // Not to process data  
                data: fileData,
                //data: { categoryId: y},
                success: function (result) {
                    //alert(result);
                    //ClearFields();
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
                },
                error: function (err) {
                    alert(err.statusText);
                }
            });
        } else {
            alert("FormData is not supported.");
        }

    }

}

function validate() {
    var isValid = true;

    if ($("#ddlCategory").val() === 0 || $("#ddlCategory").val() < 0) {
        $('#ddlCategory').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#ddlCategory').css('border-color', 'lightgrey');
    }
    if ($("#fileUpload").is('[disabled=disabled]')) {
        $('#ddlCategory').css('border-color', 'Red');
        isValid = false;
    }
    else {
        if ($("#fileUpload").get(0).files.length === 0) {
            $('#fileUpload').css('border-color', 'Red');
            isValid = false;
        }
        else
            $('#fileUpload').css('border-color', 'lightgrey');
    }

    return isValid;
}
function validateForUpdateForm() {
    var isValid = true;

    if ($("#ddlCategory").val() === 0 || $("#ddlCategory").val() < 0) {
        $('#ddlCategory').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#ddlCategory').css('border-color', 'lightgrey');
        if ($("#ddlForms").val() === 0 || $("#ddlForms").val() < 0) {
            $('#ddlForms').css('border-color', 'Red');
            isValid = false;
        }
        else {
            $('#ddlForms').css('border-color', 'lightgrey');
            
        }
    }
    if ($("#fileUpload").is('[disabled=disabled]')) {
        $('#ddlCategory').css('border-color', 'Red');
        isValid = false;
    }
    else {
        if ($("#fileUpload").get(0).files.length === 0) {
            $('#fileUpload').css('border-color', 'Red');
            isValid = false;
        }
        else {
            if ($("#ddlForms option:selected").text() === $("#fileUpload").get(0).files[0].name) {
                $('#fileUpload').css('border-color', 'lightgrey');
            }
            else {
                $('#fileUpload').css('border-color', 'Red');
                $('#ddlForms').css('border-color', 'Red');
                isValid = false;
            }
                
        } 
    }
    if ($('#txtVersion').val().length === 0) {
        $('#txtVersion').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#txtVersion').css('border-color', 'lightgrey');
    }
    

    return isValid;
}

function ClearFields() {
    $('#fileUpload').val('');
    $("#fileUpload").prop('disabled', true);
    $("#ddlCategory").prop('selectedIndex', 0);
}

function LoadFormsList(x) {
    var catId = x.value;
    var catName = $("#ddlCategory option:selected").text();;
    if (catId > 0) {
        SetUpGrid(catId, catName);
    }

}

function SetUpGrid(catId,catName) {
    var loadposturl = $('#loaddata').val();

    //do not throw error
    $.fn.dataTable.ext.errMode = 'none';

    //check if datatable is already created then destroy iy and then create it
    if ($.fn.dataTable.isDataTable('#formsTable')) {
        table = $('#formsTable').DataTable();
        table.destroy();
    }

    // alert('hh');
    var table = $("#formsTable").DataTable({
        "dom": 'Bfrtip',
        "rowReorder": false,
        "ordering": false,
        "filter": false, // this is for disable filter (search box)
        "bPaginate": false, //this is for hide pagination
        "bInfo": false, // hide showing entries
        "buttons": [
            //{ extend: "excel", className: "buttonsToHide" },
            //{ extend: "pdf", className: "buttonsToHide" },
            //{ extend: "print", className: "buttonsToHide" }
        ],

        "ajax": {
            "url": loadposturl,
            "type": "POST",
            "datatype": "json",
            "data": { catId : catId }
        },
        "columns": [
            {
                "data": "RowNumber", "name": "RowNumber", "autoWidth": true
            },
            {
                "data": "FormName", "name": "FormsName", "autoWidth": true
            },
            

            {
                "data": "FormName", "width": "150px", "render": function (data) {
                    var fName = "'" + data + "'";
                    var str = '<div class="col-sm-12"><div class="row"><div class="col-sm-6"><a href="/Document/Download?catName=' + catName + '&formName=' + data + '" class="btn btn-info btn-sm" style="background-color: #e90000;" >Download</a></div>';
                    str = str + '<div class="col-sm-6"><a href="#" class="btn btn-info btn-sm" style="background-color: #e90000;" onclick="DeleteForm(' + fName +')">Delete</a></div></div></div>';
                    return str;
                },
                
            }
            //{
            //    "data": "ID", "width": "50px", "render": function (d) {
            //        //debugger;
            //        return '<a href="#" class="btn btn-info btn-sm" style="background-color: #e90000;" onclick="DeleteForm(' + d + ')">Delete</a>';
            //    }
            //}

        ],
        "rowId": "ID",
        "dom": "Bfrtip"
    });
    //table.buttons('.buttonsToHide').nodes().addClass('hidden');
}

function DeleteForm(name) {
    var e = $('#urlDeletForm').val();
    var catId = $("#ddlCategory option:selected").val();
    var catName = $("#ddlCategory option:selected").text();
    var ans = confirm("Are you sure you want to delete this Record?");
    if (ans) {
        // debugger;
        $.ajax({
            url: e,
            data: JSON.stringify({ formName: name, catName: catName}),
            type: "POST",
            contentType: "application/json;charset=UTF-8",
            dataType: "json",
            success: function (result) {
                // debugger;

                if (result == -1) {
                    alert("Form cannot be deleted as this is already used.");
                }
                else if (result == 0) {

                        alert("Form cannot be deleted as this is already used.");
                }
                else {
                    
                    SetUpGrid(catId, catName);
                }
            },
            error: function () {
                //to do something
            }
        });
    }
}

function LoadFormsByCategoryForDropdown(catId) {
    var categoryId = catId;
    var x = $("#urlFormsDropDown").val();
    if (catId > 0) {
        $.ajax({
            url: x,
            type: "POST",
            data: JSON.stringify({ 'categoryId': categoryId }),
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {
                //debugger;
                var drpForm = $('#ddlForms');
                drpForm.find('option').remove();

                drpForm.append('<option value=' + '-1' + '>' + 'Select' + '</option>');

                $.each(result, function () {
                    drpForm.append('<option value=' + this.ID + '>' + this.FormName + '</option>');
                });
            },
            error: function (errormessage) {
                alert(errormessage.responseText);
            }
        });

    }

    
}

