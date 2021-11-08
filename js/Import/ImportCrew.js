function ExportToTable() {
    var regex = /^([a-zA-Z0-9\s_\\.\-:])+(.xlsx|.xls)$/;
    /*Checks whether the file is a valid excel file*/
    if (regex.test($("#excelfile").val().toLowerCase())) {
        var xlsxflag = false; /*Flag for checking whether excel is .xls format or .xlsx format*/
        if ($("#excelfile").val().toLowerCase().indexOf(".xlsx") > 0) {
            xlsxflag = true;
        }
        /*Checks whether the browser supports HTML5*/
        if (typeof (FileReader) != "undefined") {
            var reader = new FileReader();
            reader.onload = function (e) {
                var data = e.target.result;
                /*Converts the excel data in to object*/
                if (xlsxflag) {
                    var workbook = XLSX.read(data, { type: 'binary' });
                }
                else {
                    var workbook = XLS.read(data, { type: 'binary' });
                }
                /*Gets all the sheetnames of excel in to a variable*/
                var sheet_name_list = workbook.SheetNames;

                var cnt = 0; /*This is used for restricting the script to consider only first sheet of excel*/
                sheet_name_list.forEach(function (y) { /*Iterate through all sheets*/
                    /*Convert the cell value to Json*/
                    if (xlsxflag) {
                        var exceljson = XLSX.utils.sheet_to_json(workbook.Sheets[y]);
                    }
                    else {
                        var exceljson = XLS.utils.sheet_to_row_object_array(workbook.Sheets[y]);
                    }
                    if (exceljson.length > 0 && cnt == 0) {
                        BindTable(exceljson, '#exceltable');
                        cnt++;
                    }
                });
                $('#exceltable').show();
            }
            if (xlsxflag) {/*If excel file is .xlsx extension than creates a Array Buffer from excel*/
                reader.readAsArrayBuffer($("#excelfile")[0].files[0]);
            }
            else {
                reader.readAsBinaryString($("#excelfile")[0].files[0]);
            }
        }
        else {
            alert("Sorry! Your browser does not support HTML5!");
        }
    }
    else {
        alert("Please upload a valid Excel file!");
    }
}

function BindTable(jsondata, tableid) {/*Function used to convert the JSON array to Html Table*/
    var columns = BindTableHeader(jsondata, tableid); /*Gets all the column headings of Excel*/
    for (var i = 0; i < jsondata.length; i++) {
        var row$ = $('<tr/>');
        for (var colIndex = 0; colIndex < columns.length; colIndex++) {
            var cellValue = jsondata[i][columns[colIndex]];
            if (cellValue == null)
                cellValue = "";
            row$.append($('<td/>').html(cellValue));
        }
        $(tableid).append(row$);
    }
}
function BindTableHeader(jsondata, tableid) {/*Function used to get all column names from JSON and bind the html table header*/
    var columnSet = [];
    var headerTr$ = $('<tr/>');
    for (var i = 0; i < jsondata.length; i++) {
        var rowHash = jsondata[i];
        for (var key in rowHash) {
            if (rowHash.hasOwnProperty(key)) {
                if ($.inArray(key, columnSet) == -1) {/*Adding each unique column names to a variable array*/
                    columnSet.push(key);
                    headerTr$.append($('<th/>').html(key));
                }
            }
        }
    }
    $(tableid).append(headerTr$);
    return columnSet;
}


function GetTemporaryCrewFromGrid() {
    //***************************
    //Get all Grews from the grid and Send to ImportCrewList Action to insert into Database
    //***************************
    var tmpCrewList = [];
    var url = $('#importCrewList').val();
    var idx = 0;
    var crewName = "";
    var rankName = "";
    var email = "";
    var replacedCrewName = "";
    var replacedCrewId = 0;
    var shipId = 0;
    var tdCount = 0;


    var html_table_data = "";
    var bRowStarted = true;
    $('#tblImportCrewList tbody>tr').each(function () {
        idx += 1;
        tdCount = 0;
        $('td', this).each(function () {

            tdCount += 1;
            //if (html_table_data.length == 0 || bRowStarted == true) {
            //    html_table_data += $(this).text();
            //    bRowStarted = false;
            //}
            //else
            //    html_table_data += " | " + $(this).text();
            if (tdCount == 2)
                shipId = $(this).text();
            if (tdCount == 3)
                crewName = $.trim($(this).text());
            if (tdCount == 4)
                rankName = $.trim($(this).text());
            if (tdCount == 5)
                email = $.trim($(this).text());
            if (tdCount == 6) {
                replacedCrewName = $(this).text();
                replacedCrewId = $(this).find('select').val();
                replacedCrewName = $(this).find('select :selected').text();
               // replacedCrewName = $(this).find('select').find('option:selected').text();
            }
               
        });
        //html_table_data += "\n";
        //bRowStarted = true;

        //----------------
        tmpCrewList.push({
            SL:                 idx,
            CrewName:           crewName,
            RankName:           rankName,
            Email:              email,
            ReplacedCrewId:     replacedCrewId,
            ReplacedCrewName:   replacedCrewName,
            ShipId:             shipId

        });

    });

    //alert(html_table_data);
    //alert(tmpCrewList);
    var crews = JSON.stringify(tmpCrewList);
    var crews = tmpCrewList;
   // var crews = "m";
    //var name = "Bingshu";
    if (window.FormData !== undefined) {
        var fileData = new FormData();

        fileData.append('crews', JSON.stringify(tmpCrewList));

        $.ajax({
            url: url,
            type: "POST",
            datatype: "json",
            //contentType: false,
            //contentType: 'application/json; charset=utf-8',
            /*data: JSON.stringify(tmpCrewList).toString(),*/
            //data: JSON.stringify(tmpCrewList.toString()),
            //data: { crews: name},
            //data: { id: 1, name:"BB" },
            contentType: false, // Not to set any content header  
            processData: false, // Not to process data  
            data: fileData,
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

    }
}