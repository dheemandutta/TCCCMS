function validate() {
    var isValid = true;

    if ($('#ApproverUserId').val().length === 0) {
        $('#ApproverUserId').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#ApproverUserId').css('border-color', 'lightgrey');
    }

    if ($('#SignImagePath').val().length === 0) {
        $('#SignImagePath').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#SignImagePath').css('border-color', 'lightgrey');
    }

    if ($('#Name').val().length === 0) {
        $('#Name').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#Name').css('border-color', 'lightgrey');
    }

    if ($('#Position').val().length === 0) {
        $('#Position').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#Position').css('border-color', 'lightgrey');
    }


    return isValid;
}

function clearTextBox() {
    $('#ApproverUserId').val("");
    $('#SignImagePath').val("");
    $('#Name').val("");
    $('#Position').val("");
}

function SaveApproverSign() {

    //alert($('textarea#Comments').val());
    //debugger;
    var posturl = $('#SaveApproverSign').val();
    var res = validate();
    if (res == false) {
        return false;
    }
    //alert(res);
    if (res) {
        var ApproverSign = {
            ApproverUserId: $('#ApproverUserId').val(),
            SignImagePath: $('#SignImagePath').val(),
            Name: $('#Name').val(),
            Position: $('#Position').val()
        };

        $.ajax({
            url:'/ApproverSign/SaveApproverSign',
            data: JSON.stringify(ApproverSign),
            type: "POST",
            contentType: "application/json;charset=utf-8",
            dataType: "json",

            //success: function (result) {

            //    alert('Added Successfully');

            //    clearTextBox();
            //    }         
            //,



            success: function (result) {
                //loadData();
                //$('#myModal').modal('hide');
                alert('Added Successfully');

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

                //toastr.success("Added Successfully");

                clearTextBox();
            },
            error: function (errormessage) {
                alert(errormessage);
                console.log(errormessage.responseText);
            }
        });
    }
}

function SaveSign() {

    //$.ajax({
    //    url: "/ApproverSign/SaveSign",
    //    data: { user : 'Jac'},
    //    type: "POST",
    //    contentType: "application/json;charset=utf-8",
    //    dataType: "json",

    //    success: function (result) {
    //        //loadData();
    //        //$('#myModal').modal('hide');
    //        alert('Added Successfully');

    //    },
    //    error: function (errormessage) {
    //        alert(errormessage);
    //        console.log(errormessage.responseText);
    //    }
    //});
    var ApproverSign = {
        ApproverUserId: 1,
        SignImagePath: $('#SignImagePath').val(),
        Name:'Bob',
        Position: $('#Position').val()
    };
    $.ajax({
        type: "POST",
        url: '/ApproverSign/SaveSign',
        data: {
            approverUserId: $('#ApproverUserId').val(),
            signImagePath: $('#SignImagePath').val(),
            name: $('#Name').val(),
            position: $('#Position').val()
        },
        success: function (data) {
            if (data) {
                //$('#divViewer_' + id).append(data);
                alert('Added Successfully');
                clearTextBox();
            }
        }
    });
}