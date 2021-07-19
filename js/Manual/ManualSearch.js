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
    if (searchword.length > 0) {

        $.each(searchword, function (index, value) {
            var sword = value;
            var custfilter = new RegExp(sword, "ig");
            var repstr = "<span class='highlight'>" + sword + "</span>";

            if (sword != "") {

                if ($('.searchdContent .card-body').length > 0) {
                    $('.card-body').each(function () {
                        //var x = $(this).html($(this).html());
                        //console.log(x);
                        $(this).html($(this).html().replace(custfilter, repstr));
                    })
                }
                else {
                    $('.searchdContent').each(function () {
                        //var x = $(this).html($(this).html());
                        //console.log(x);
                        $(this).html($(this).html().replace(custfilter, repstr));
						
						//**End****** automatically expand  word containing accordion panel*************
						var paren =  getStringParent(sword);
						//console.log(paren);
						var prv = paren.prev();
						paren.prev().addClass("active");
						paren.css("display","block");
						//**End****** automatically expand  word containing accordion panel*************
						
                    })
                }

            }
        })
        
    }
    
}
function getStringParent(str) { 
  return $(".panel:contains('"+ str +"')");
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

function PrintHtml(data) {
    var mywindow = window.open('', '', 'left=0,top=0,width=1600,height=1400');

    var is_chrome = Boolean(mywindow.chrome);

    mywindow.document.write('<html><head><title></title>');
    mywindow.document.write('</head><body >');
    mywindow.document.write($('#ParentBody').html());
    mywindow.document.write('</body></html>');
    mywindow.document.close(); // necessary for IE >= 10 and necessary before onload for chrome
    is_chrome = false;
    //alert(is_chrome);
    if (is_chrome) {
        mywindow.onload = function () { // wait until all resources loaded 
            mywindow.focus(); // necessary for IE >= 10
            mywindow.print();  // change window to mywindow
            mywindow.close();// change window to mywindow
        };
    }
    else {
        mywindow.document.close(); // necessary for IE >= 10
        mywindow.focus(); // necessary for IE >= 10
        mywindow.print();
        mywindow.close();
    }

    return true;
}