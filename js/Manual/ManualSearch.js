var stxt = "Company";
$(document).ready(function () {
    
    console.log(stxt + "  after");
    //HighlightText(stxt);

    GetSearchText();
});

function HighlightText(x) {
    $('.searchdContent').highlight(x);
    console.log('from highlight');
    console.log(x);
}

function replaceText(searchword) {

    $(".searchdContent").find(".highlight").removeClass("highlight");

    //var searchword = $("#searchtxt").val();

    var custfilter = new RegExp(searchword, "ig");
    var repstr = "<span class='highlight'>" + searchword + "</span>";

    if (searchword != "") {
        $('.searchdContent').each(function () {
            $(this).html($(this).html().replace(custfilter, repstr));
        })
    }
}

function GetSearchText() {
    $.ajax({
        //url: '@Url.Action("SearchPage", "Home")',
        url: '/Manual/GetSearchText',
        type: 'GET',
        dataType: 'json',
        cache: false,
        success: function (data) {
            stxt = data;
            console.log(stxt);
            console.log("b4hightlite")
            //stxt = ['company', 'business'];
            //HighlightText(stxt);
            console.log("after highlite--" + stxt);
            //$('.searchdContent').highlight(stxt);
            replaceText(stxt);
        },
        //error: function (data) {
        //    console.log(data);
        //    if (code == 500) {
        //        alert('500 status code! server error');
        //    }
        //}
        error: function (jqXHR, textStatus, errorThrown) {
            console.log(textStatus + ": " + jqXHR.status + " " + errorThrown);
        }
        
    });

}