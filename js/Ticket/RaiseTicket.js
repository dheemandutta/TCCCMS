function UploadFilesAndSendTicket() {
    //var res = validate();
    //if (res == false) {
    //    return false;
    //}
    //********--------------------------------------------------------------------------
    //if (res) {
        //Checking whether FormData is available in browser  
        if (window.FormData !== undefined) {
            var error = $("#txtError").val();
            var desc = $("#txtDescription").val();
            var fileUpload = $("#fileUpload").get(0);
            var files = fileUpload.files;
            // Create FormData object  
            var fileData = new FormData();
           

            // Looping over all files and add it to FormData object  
            for (var i = 0; i < files.length; i++) {
                fileData.append(files[i].name, files[i]);
            }

            // Adding one more key to FormData object 
            fileData.append('error', error);
            fileData.append('description', desc);

            $.ajax({
                url: '/Ticket/SendTicket',
                type: "POST",
                //datatype: "json",
                //contentType: "application/json; charset=utf-8",
                contentType: false, // Not to set any content header  
                processData: false, // Not to process data  
                data: fileData,
                //data: { categoryId: y},
                success: function (result) {
                    alert("Your Ticket Number : "+result);
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

    //}

}