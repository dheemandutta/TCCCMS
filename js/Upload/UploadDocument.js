function EnableFileControl(x) {
    var catId = x.value;
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
                url: '/UploadDocument/UploadFiles',
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

function ClearFields() {
    $('#fileUpload').val('');
    $("#fileUpload").prop('disabled', true);
    $("#ddlCategory").prop('selectedIndex', 0);
}