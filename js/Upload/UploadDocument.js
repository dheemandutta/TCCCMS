function EnableFileControl(x) {
    var catId = x.value;
    if (catId !== "" | catId != "undefined" | catId != null) {
        $("#fileUpload").prop('disabled', false);
    }
    else {
        $("#fileUpload").prop('disabled', true);
    }
}

function UploadFiles() {
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
     //Checking whether FormData is available in browser  
    if (window.FormData !== undefined) {
        var catId = $("#drpCategory").val();
        var catName = $("#drpCategory option:selected").text();
        var fileUpload = $("#fileUpload").get(0);
        var files = fileUpload.files;
        // Create FormData object  
        var fileData = new FormData();
        var cate = {
            ID: '2',
            CatecoryName:'Admin'

        };

        // Looping over all files and add it to FormData object  
        for (var i = 0; i < files.length; i++) {
            fileData.append(files[i].name, files[i]);
        }

        // Adding one more key to FormData object  
        fileData.append('category', cate);
        fileData.append('categoryId', catId);
        fileData.append('categoryName', catName);

        $.ajax({
            url: '/UploadDocument/UploadFiles',
            type: "POST",
            //datatype: "json",
            //contentType: "application/json; charset=utf-8",
            contentType: false, // Not to set any content header  
            processData: false, // Not to process data  
            data:  fileData ,
            //data: { categoryId: y},
            success: function (result) {
                alert(result);
            },
            error: function (err) {
                alert(err.statusText);
            }
        });
    } else {
        alert("FormData is not supported.");
    }  
}