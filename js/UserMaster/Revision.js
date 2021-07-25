function GetViewersByRevision(id) {
    $('#divViewer_' + id).html("");
    var x = id;
    //@* $.post("@Url.Action("GetViewerByRevision", "RevisionHistory",new { id =x})", function (data) {
    //    if (data) {
    //        $('#divViewer_' + id).append(data);

    //    }

    //});*@

    $.ajax({
        type: "POST",
        url: '@Url.Action("GetViewerByRevision", "RevisionHistory")',
        data: { Id: id },
        success: function (data) {
            if (data) {
                $('#divViewer_' + id).append(data);
                LoadAccordian();

            }
        }
    });
}

function SaveRevisionViewer(id) {


    $.ajax({
        type: "POST",
        url: '@Url.Action("SaveRevisionViewer", "RevisionHistory")',
        data: { revisionId: id },
        success: function (data) {
            if (data) {
                //$('#divViewer_' + id).append(data);

            }
        }
    });
}

function LoadAccordian() {
    var acc = document.getElementsByClassName("accordion");
    var i;

    for (i = 0; i < acc.length; i++) {
        acc[i].addEventListener("click", function () {
            this.classList.toggle("active");
            var panel = this.nextElementSibling;
            if (panel.style.display === "block") {
                panel.style.display = "none";
            } else {
                panel.style.display = "block";
            }
        });
    }
}

function PrintRevisionDetailsHtml(data, id) {
    var mywindow = window.open('', '', 'left=0,top=0,width=1600,height=1400');

    var is_chrome = Boolean(mywindow.chrome);

    mywindow.document.write('<html><head><title></title>');
    mywindow.document.write('</head><body >');
    mywindow.document.write($('#RevisionDetails_' + id).html());
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
