var stxt = "Company";
$(document).ready(function () {
    GetSearchText();
    

    console.log(stxt + "  after");
    //HighlightText(stxt);
});


function HighlightText(x) {
    $('.searchdContent').highlight(x);
    console.log(x);
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
            console.log("b4hilt")
            //stxt = ['company', 'business'];
            HighlightText(stxt); console.log("after hglt");
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