function CreateCommonDataTableAjax_ServerSide(DatatableConfigration, IntialCompleteFunc = "") {
    
    let DatatableObj = "";
    if (typeof (IntialCompleteFunc) != "function") {
        IntialCompleteFunc = (settings, json) => {
        }
    }
    if (DatatableConfigration.UseTableCounter == true || DatatableConfigration.UseTableCounter == undefined) {
        DatatableConfigration.Columns = [{
            //"data": "ID",
            "render": function (d, t, f, m) {
                return m.row + 1
            }
        }].concat(DatatableConfigration.Columns);
    }
    if (DatatableConfigration.HasActiveToggle == true) {
        DatatableConfigration.Columns.push({
            "data": "IsActive",
            "className": "HideFromExport",
            "render": function (data, ypet, full, meta) {
                if (IsInPolicy("CreatePolicy") == true) {
                    let isChecked = "";
                    if (data == true) {
                        isChecked = "checked";
                    }
                    return `<div class="form-check form-switch form-check-custom form-check-solid me-10">
                                                            <input onchange="toogleActive(this,'${full.ID}')" class="form-check-input h-30px w-50px" type="checkbox" ${isChecked} value="" >
                                                        </div>`;
                }
                else if (data == true) {
                    return `<i class="far fa-check-circle text-success"></i>`;
                }
                else {
                    return `<i class="far fa-times-circle text-danger"></i>`;

                }
            }

        }
        )

    }
    if (DatatableConfigration.UseDefaultAction == true || DatatableConfigration.UseDefaultAction == undefined) {
        DatatableConfigration.Columns.push({
            "data": "ID",
            "className": "HideFromExport text-center",
            "render": function (data, ypet, full, meta) {
                let Edit = DataTableRowEditBtn(data);
                let Remove = DataTableRowDeleteBtn(data);
                let Details = DataTableRowDetailsBtn(data);
                return Edit + Remove + Details;
            },
        }
        )
    }
    if (!$.fn.DataTable.isDataTable(DatatableConfigration.TableID)) {
        let DatatableConfigJson = {
            scrollCollapse: true,
            searching: true,
            buttons: [
                GetDatatablePrintConfig(DatatableConfigration.TableID), GetDatatableExcelConfig(DatatableConfigration.TableID), GetDatatablePdfConfig(DatatableConfigration.TableID)// col visibility
            ],
            "dom": DatatableCommonDomHtml(),
            "bServerSide": true,
            "bProcessing": true,

            "sAjaxSource": DatatableConfigration.Url,
            "lengthMenu": DataTableLengthMenuCommon,
            'language': {
                'url': DataTableLanguageUrl(),
            },
            "columns": DatatableConfigration.Columns,
            "columnDefs": DatatableConfigration.columnDefs ?? [],
            "initComplete": function (settings, json) {

                if (typeof (IntialCompleteFunc) == "function") {
                    IntialCompleteFunc(json, json);
                }
            }


        };
        DatatableConfigJson = JsonConcat(DatatableConfigJson, DatatableConfigration.AdditionalDatatbleConfig);

        DatatableObj = $(DatatableConfigration.TableID).DataTable(DatatableConfigJson).on('init.dt', function () {
            //RemoveUnassignedPolicy();
            //show nothing
            //console.log('no access to: ' + $('.dataTables_scroll'));
            //setTimeout(function () {
            //    //show element
            //    console.log('access to: ' + $('.dataTables_scroll'));
            //}, 0);
        });
    }
    else {
        DatatableObj = new $.fn.dataTable.Api(DatatableConfigration.TableID)
        DatatableObj.ajax.url(DatatableConfigration.Url).load(IntialCompleteFunc, (callback) => {
        }, (rest) => {
        });
    }
    return DatatableObj;
}

function CreateCommonDatatableAjax(DatatableConfigration, IntialCompleteFunc = "", UseTableCounter = true, UseDefaultAction = true, HasActiveToggle = false) {
    let DatatableObj = "";
    if (typeof (IntialCompleteFunc) != "function") {
        IntialCompleteFunc = (settings, json) => {
        }
    }
    if (UseTableCounter == true) {
        DatatableConfigration.Columns = [{
            //"data": "ID",
            "render": function (d, t, f, m) {
                return m.row + 1
            }
        }].concat(DatatableConfigration.Columns);
    }
    if (HasActiveToggle == true) {
        DatatableConfigration.Columns.push({
            "data": "IsActive",
            "className": "HideFromExport",
            "render": function (data, ypet, full, meta) {
                if (IsInPolicy("CreatePolicy") == true) {
                    let isChecked = "";
                    if (data == true) {
                        isChecked = "checked";
                    }
                    return `<div class="form-check form-switch form-check-custom form-check-solid me-10">
                                                            <input onchange="toogleActive(this,'${full.ID}')" class="form-check-input h-30px w-50px" type="checkbox" ${isChecked} value="" >
                                                        </div>`;
                }
                else if (data == true) {
                    return `<i class="far fa-check-circle text-success"></i>`;
                }
                else {
                    return `<i class="far fa-times-circle text-danger"></i>`;

                }
            }
        }
        )

    }
    if (UseDefaultAction == true) {
        DatatableConfigration.Columns.push({
            "data": "ID",
            "className": "HideFromExport text-center",
            "render": function (data, ypet, full, meta) {
                let Edit = DataTableRowEditBtn(data);
                let Remove = DataTableRowDeleteBtn(data);
                let Details = DataTableRowDetailsBtn(data);
                return Edit + Remove + Details;
            },
        }
        )
    }
    if (!$.fn.DataTable.isDataTable(DatatableConfigration.TableID)) {
        let DatatableConfigJson = {
            //scrollY: 500,
            scrollCollapse: true,
            searching: true,
            buttons: [
                GetDatatablePrintConfig(DatatableConfigration.TableID), GetDatatableExcelConfig(DatatableConfigration.TableID), GetDatatablePdfConfig(DatatableConfigration.TableID)// col visibility
            ],
            //lengthChangke: true,
            //dom: 'Blfrtip',
            "dom": DatatableCommonDomHtml(),
            "processing": true,
            "ajax": {
                "url": DatatableConfigration.Url /*`/Admission/EmployeeAllowance/GetEmployeeAllowances?EmployeeID=${$('input[name="EmployeeVM_STEP1.ID"]').val()}`*/,
                "type": "Get",
                "datatype": "json",
                "dataSrc": DatatableConfigration.DataSrc ?? "data"
            },
            "lengthMenu": DataTableLengthMenuCommon,
            'language': {
                'url': DataTableLanguageUrl(),
            },
            "columns": DatatableConfigration.Columns,
            "columnDefs": DatatableConfigration.columnDefs ?? [],
            "initComplete": function (settings, json) {

                if (typeof (IntialCompleteFunc) == "function") {
                    IntialCompleteFunc(json, json);
                }
            }


        };
        DatatableConfigJson = JsonConcat(DatatableConfigJson, DatatableConfigration.AdditionalDatatbleConfig);

        DatatableObj = $(DatatableConfigration.TableID).DataTable(DatatableConfigJson).on('init.dt', function () {
            //RemoveUnassignedPolicy();
            //show nothing
            //console.log('no access to: ' + $('.dataTables_scroll'));
            //setTimeout(function () {
            //    //show element
            //    console.log('access to: ' + $('.dataTables_scroll'));
            //}, 0);
        });
    }
    else {
        DatatableObj = new $.fn.dataTable.Api(DatatableConfigration.TableID)
        DatatableObj.ajax.url(DatatableConfigration.Url).load(IntialCompleteFunc, (callback) => {
        }, (rest) => {
        });
    }
    return DatatableObj;
}
function CreateCommonSelectableDatatableAjax(DatatableConfigration, IntialCompleteFunc = "") {
    let DatatableObj = "";
    if (typeof (IntialCompleteFunc) != "function") {
        IntialCompleteFunc = (settings, json) => {
        }
    }
    if (DatatableConfigration.UseTableCounter == undefined || DatatableConfigration.UseTableCounter == true) {
        DatatableConfigration.Columns = Prepend({
            "data": "ID",
            "render": function (d, t, f, m) {
                return m.row + 1
            }
        }, DatatableConfigration.Columns)
    }
    DatatableConfigration.Columns = Prepend({
        "data": "ID",
        "render": function (d, t, f, m) {
            return "";
        }
    }, DatatableConfigration.Columns)
    if (!$.fn.DataTable.isDataTable(DatatableConfigration.TableID)) {
        let DatatableConfigJson = {
            searching: true,
            columnDefs: [{
                orderable: false,
                className: 'select-checkbox',
                targets: 0
            }],
            select: {
                //style: 'os',
                style: 'multi',
                selector: 'td:first-child'
            },
            order: [[1, 'asc']],
            scrollCollapse: true,
            "dom": DatatableCommonDomHtml(),
            "processing": false,
            "ajax": {
                "url": DatatableConfigration.Url,
                "type": "Get",
                "datatype": "json",
            },
            "lengthMenu": DataTableLengthMenuCommon,
            'language': {
                'url': DataTableLanguageUrl(),
            },
            "columns": DatatableConfigration.Columns,
            "initComplete": IntialCompleteFunc,
        };
        DatatableConfigJson = JsonConcat(DatatableConfigJson, DatatableConfigration.AdditionalDatatbleConfig);
        DatatableObj = $(DatatableConfigration.TableID).DataTable(DatatableConfigJson);
        $.each(DatatableConfigration.ArrayOfEvents ?? [], (i, OBJ) => {
            DatatableObj.on(OBJ.EventName, OBJ.EventFunction);
        })
    }
    else {
        DatatableObj = new $.fn.dataTable.Api(DatatableConfigration.TableID)
        DatatableObj.ajax.url(DatatableConfigration.Url).load(IntialCompleteFunc, (rest) => {
        });
    }
    return DatatableObj;
}
function CreateCommonSelectableDatatableJson(DatatableConfigration, IntialCompleteFunc = "") {
    let DatatableObj = "";
    if (typeof (IntialCompleteFunc) != "function") {
        IntialCompleteFunc = (settings, json) => {
        }
    }
    if (DatatableConfigration.UseTableCounter == undefined || DatatableConfigration.UseTableCounter == true) {
        DatatableConfigration.Columns = Prepend({
            "data": "ID",
            "render": function (d, t, f, m) {
                return m.row + 1
            }
        }, DatatableConfigration.Columns)
    }
    DatatableConfigration.Columns = Prepend({
        "data": "ID",
        "render": function (d, t, f, m) {
            return "";
        }
    }, DatatableConfigration.Columns)
    if (!$.fn.DataTable.isDataTable(DatatableConfigration.TableID)) {
        let DatatableConfigJson = {
            searching: true,
            columnDefs: [{
                orderable: false,
                className: 'select-checkbox',
                targets: 0
            }],
            select: {
                //style: 'os',
                style: 'multi',
                selector: 'td:first-child'
            },
            order: [[1, 'asc']],
            scrollCollapse: true,
            "dom": DatatableCommonDomHtml(),
            "data": DatatableConfigration.DataJson,
            "lengthMenu": DataTableLengthMenuCommon,
            'language': {
                'url': DataTableLanguageUrl(),
            },
            "columns": DatatableConfigration.Columns,
            "initComplete": IntialCompleteFunc,
        };
        DatatableConfigJson = JsonConcat(DatatableConfigJson, DatatableConfigration.AdditionalDatatbleConfig);
        DatatableObj = $(DatatableConfigration.TableID).DataTable(DatatableConfigJson);
        $.each(DatatableConfigration.ArrayOfEvents ?? [], (i, OBJ) => {
            DatatableObj.on(OBJ.EventName, OBJ.EventFunction);
        })
    }
    else {
        DatatableObj = new $.fn.dataTable.Api(DatatableConfigration.TableID)
        DatatableObj = $(DatatableConfigration.TableID).DataTable().clear().rows.add(DatatableConfigration.DataJson).draw();
    }
    return DatatableObj;
}
function CreateCommonDatatableJson(DatatableConfigration, IntialCompleteFunc = "") {
    let DatatableObj = "";
    if (typeof (IntialCompleteFunc) != "function") {
        IntialCompleteFunc = (settings, json) => {
        }
    }
    if (DatatableConfigration.UseTableCounter == undefined || DatatableConfigration.UseTableCounter == true) {
        DatatableConfigration.Columns = Prepend({
            "data": "ID",
            "render": function (d, t, f, m) {
                return m.row + 1
            }
        }, DatatableConfigration.Columns)
    }
    if (!$.fn.DataTable.isDataTable(DatatableConfigration.TableID)) {
        let DatatableConfigJson = {
            scrollCollapse: true,
            searching: true,
            buttons: [
                GetDatatablePrintConfig(DatatableConfigration.TableID), GetDatatableExcelConfig(DatatableConfigration.TableID), GetDatatablePdfConfig(DatatableConfigration.TableID)// col visibility
            ],
            //lengthChangke: true,
            //dom: 'Blfrtip',
            "dom": DatatableCommonDomHtml(),
            "data": DatatableConfigration.DataJson,
            "lengthMenu": DatatableConfigration.DataTableLengthMenuCommon?? DataTableLengthMenuCommon,
            'language': {
                'url': DataTableLanguageUrl(),
            },
            "columns": DatatableConfigration.Columns,
            "initComplete": IntialCompleteFunc,
        };
        DatatableConfigJson = JsonConcat(DatatableConfigJson, DatatableConfigration.AdditionalDatatbleConfig);
        DatatableObj = $(DatatableConfigration.TableID).DataTable(DatatableConfigJson);
        $.each(DatatableConfigration.ArrayOfEvents ?? [], (i, OBJ) => {
            debugger
            DatatableObj.on(OBJ.EventName, OBJ.EventFunction);
        })
    }
    else {
        DatatableObj = new $.fn.dataTable.Api(DatatableConfigration.TableID)
        DatatableObj = $(DatatableConfigration.TableID).DataTable().clear().rows.add(DatatableConfigration.DataJson).draw();
    }
    return DatatableObj;
}


function DataTableRowEditBtn(ID) {
    return IsInPolicy("EditPolicy") == true ? `<a href="javascript:OpenPageModal('${ID}')"  data-PolicyType="EditPolicy" class="btn btn-icon btn-bg-light btn-active-color-primary btn-sm me-1">
                                    <span class="indicator-label" data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" title="${getToken("Edit")}"><i class="bi bi-pencil-fill fs-5 text-primary"></i></span>
<span class="indicator-progress">
            <span class="spinner-border spinner-border-sm align-middle ms-2"></span>
        </span>
</a>`: '';
}
function DataTableRowDeleteBtn(ID, RemoveFunc ='RemovePageObject') {
    return IsInPolicy("DeletePolicy") == true ? `  <a href="javascript:OpenSweetAlertConfirmModal('${ID}','${RemoveFunc}');" data-PolicyType="DeletePolicy" class="btn btn-icon btn-bg-light btn-active-color-primary btn-sm me-1" >
                                    <span  data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" title="${getToken("Delete")}"><i class="bi bi-trash fs-4 text-danger"></i></span>

</a>`: ''
}
function DataTableRowDetailsBtn(ID) {
    return IsInPolicy("BrowsPolicy") == true ? `<a href="javascript:OpenPageModal_Detailes('${ID}','true')" data-PolicyType="BrowsPolicy" class="btn btn-icon btn-bg-light btn-active-color-primary btn-sm me-1">
                                    <span class="indicator-label" data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" title="${getToken("Details")}"><i class="bi bi-exclamation-lg fs-3"></i></span>
<span class="indicator-progress">
           
            <span class="spinner-border spinner-border-sm align-middle ms-2"></span>
        </span>
</a>`: ''
}

function CustomSettingForDatatablePDF_Salary(doc, TableSelector) {
    let Language = getCookieValue("Usre_Culture");
    Language = (Language == undefined || Language.toLowerCase().indexOf("ar") >= 0) ? "ar" : "en";
    if (Language == "ar") {
        //We Revers Column Order 
        $.each(doc.content[1].table.body, (i, e) => {
            e.reverse();
            $.each(e, (ii, ee) => {
                if (isArabic(ee.text)) {
                    ee.text = ee.text.split(' ').reverse().join(' ');
                }
            })
        });
        doc.styles.tableBodyEven.alignment = "right";
        doc.styles.tableBodyOdd.alignment = "right";
        doc.content[0].alignment = 'right';
        doc.content[1].alignment = 'right';

    }

    else {
        doc.styles.tableBodyEven.alignment = "left";
        doc.styles.tableBodyOdd.alignment = "left";
        doc.content[0].alignment = 'left';
        doc.content[1].alignment = 'left';

    }

    var tblBody = doc.content[1].table.body;
    // ***
    //This section creates a grid border layout
    // ***
    doc.content[1].layout = {
        hLineWidth: function (i, node) {
            return (i === 0 || i === node.table.body.length) ? 2 : 1;
        },
        vLineWidth: function (i, node) {
            return (i === 0 || i === node.table.widths.length) ? 2 : 1;
        },

        layout: {
            paddingLeft: (i, node) => 0,
            paddingRight: (i, node) => 0,
            paddingTop: (i, node) => 10,
            paddingBottom: (i, node) => 10
        },

    };
    //Wirdth 

    var colCount = new Array();
    $(TableSelector).find('tbody tr:first-child td:visible:not(.select-checkbox):not(.HideFromExport)').each(function () {

        if ($(this).attr('colspan')) {
            for (var i = 1; i <= $(this).attr('colspan'); $i++) {
                colCount.push('auto');
                //colCount.push('*');
            }
        }
        else {
            colCount.push('auto');
            //colCount.push('*');
        }

    });

    colCount[($(TableSelector).find('tbody tr:first-child td:visible:not(.select-checkbox):not(.HideFromExport)').length - 1)] = "*";//Change Employee Name Witds
    doc.content[1].table.widths = colCount;
    doc.info = {
        title: 'awesome Document testttt',
        author: 'john doe testtt',
        subject: 'subject of document testtt',
        keywords: 'keywords for document testttt',
    };
    doc.watermark = { text: 'MountainKW ', angle: 70, opacity: 0.3, bold: true, italics: true, fontSize: 40 };
    doc.defaultStyle =
    {
        font: 'ArabicFont',
        fontSize: 11,
        bold: false

    };
    doc.pageMargins = [10, 10, 10, 10];

}
function Create_DatatablePDF_Salary(Selector) {
    var DatatableInstance = new $.fn.dataTable.Api(Selector)
    DatatableInstance.button('.buttons-pdf').trigger();
}
function GetDatatablePdfConfig_Salary(TableSelector) {
    return {
        extend: 'pdfHtml5',
        text: 'PDF',
        customize: function (doc) {
            CustomSettingForDatatablePDF_Salary(doc, TableSelector);
        },
        titleAttr: 'Generate PDF',
        className: 'btn-outline-danger btn-sm d-none',

        exportOptions: {
            columns: ':visible:not(.select-checkbox):not(.HideFromExport)',
            orientation: 'landscape',
            pageSize: 'LEGAL'
        },
        orientation: 'landscape',
        pageSize: 'LEGAL'



    }
}
function SearchInDatatable(DataTableID, Text) {
    Text.replace('#', '');
    DatatableObj = new $.fn.dataTable.Api("#" + DataTableID);
    DatatableObj.search(Text).draw();

}
function SelectAllInDatatable(bool, TableSelector) {
    var DatatableInstance = new $.fn.dataTable.Api(TableSelector)
    if ($.fn.DataTable.isDataTable(TableSelector)) {
        if (bool) {
            DatatableInstance.rows(":not('.DisableChecked')").select();
        } else {
            DatatableInstance.rows(":not('.DisableChecked')").deselect();
        }
    }
}
function HandlDatatbleSelectAllCheckBoxChangg(element) {
    let TableID = $(element).parents("table").attr("id") ?? "";
    //let DatatableObj = new $.fn.dataTable.Api(TableID)
    SelectAllInDatatable($(element).is(":checked"), "#" + TableID)
    //if ($(element).is(":checked")) {
    //    DatatableObj.rows().select();
    //}
    //else {
    //    DatatableObj.rows().deselect();
    //}
}
function DataTableLanguageUrl() {
    return  '/Components/_SharedComponent/JsonData/DatatableLanguage_En.json';
    //return CurrentUserLanguage === 'ar-EG' ? '/Components/_SharedComponent/JsonData/DatatableLanguage_Ar.json' : '/Components/_SharedComponent/JsonData/DatatableLanguage_En.json';
}
function CustomSettingForDatatablePDF(doc, TableSelector) {

    let Language = getCookieValue("Usre_Culture");
    Language = (Language == undefined || Language.toLowerCase().indexOf("ar") >= 0) ? "ar" : "en";

    if (Language == "ar") {
        //We Revers Column Order 
        $.each(doc.content[1].table.body, (i, e) => {
            e.reverse();
            $.each(e, (ii, ee) => {
                if (isArabic(ee.text)) {

                    ee.text = ee.text.split(' ').reverse().join(' ');
                }
            })
        });
        doc.styles.tableBodyEven.alignment = "right";
        doc.styles.tableBodyOdd.alignment = "right";
        doc.content[0].alignment = 'right';
        doc.content[1].alignment = 'right';
        //This section loops thru each row in table looking for where either
        //the second or third cell is empty.
        //If both cells empty changes rows background color to '#FFF9C4'
        //if only the third cell is empty changes background color to '#FFFDE7'
        // ***
        //$('#EmployeesTable').find('tr').each(function (ix, row) {
        //    var index = ix;
        //    var rowElt = row;
        //    $(row).find('td').each(function (ind, elt) {
        //        if (tblBody[index][1].text == '' && tblBody[index][2].text == '' && tblBody[index][ind] != undefined) {
        //            delete tblBody[index][ind].style;
        //            tblBody[index][ind].fillColor = '#FFF9C4';
        //        }
        //        else {
        //            if (tblBody[index][2].text == '' && tblBody[index][ind] != undefined) {
        //                delete tblBody[index][ind].style;
        //                tblBody[index][ind].fillColor = '#FFFDE7';
        //            }
        //        }
        //    });
        //});
    }

    else {
        doc.styles.tableBodyEven.alignment = "left";
        doc.styles.tableBodyOdd.alignment = "left";
        doc.content[0].alignment = 'left';
        doc.content[1].alignment = 'left';

    }

    var tblBody = doc.content[1].table.body;
    // ***
    //This section creates a grid border layout
    // ***
    doc.content[1].layout = {
        hLineWidth: function (i, node) {
            return (i === 0 || i === node.table.body.length) ? 2 : 1;
        },
        vLineWidth: function (i, node) {
            return (i === 0 || i === node.table.widths.length) ? 2 : 1;
        },
        hLineColor: function (i, node) {
            return (i === 0 || i === node.table.body.length) ? 'black' : 'gray';
        },
        vLineColor: function (i, node) {
            return (i === 0 || i === node.table.widths.length) ? 'black' : 'gray';
        }
    };
    //Wirdth 
    var colCount = new Array();
    $(TableSelector).find('tbody tr:first-child td:visible:not(.select-checkbox):not(.HideFromExport)').each(function () {

        if ($(this).attr('colspan')) {
            for (var i = 1; i <= $(this).attr('colspan'); $i++) {
                //colCount.push('auto');
                colCount.push('*');

            }
        }
        else {
            //colCount.push('auto');
            colCount.push('*');
        }

    });
    doc.content[1].table.widths = colCount;
    doc.info = {
        title: 'awesome Document testttt',
        author: 'john doe testtt',
        subject: 'subject of document testtt',
        keywords: 'keywords for document testttt',
    };
    doc.watermark = { text: 'MountainKW ', angle: 70, opacity: 0.3, bold: true, italics: true, fontSize: 40 };
    doc.defaultStyle =
    {
        font: 'ArabicFont',
        fontSize: 12,
        bold: true

    };

}
function CustomSettingForDatatableExcel(doc, TableSelector) {

    let Language = "";
    Language = (CurrentUserLanguage.toLowerCase().indexOf("ar") >= 0) ? "ar" : "en";

    if (Language == "ar") {
        //We Revers Column Order 
        $.each(doc.content[1].table.body, (i, e) => {
            e.reverse();
            $.each(e, (ii, ee) => {
                if (isArabic(ee.text)) {

                    ee.text = ee.text.split(' ').reverse().join(' ');
                }
            })
        });
        doc.styles.tableBodyEven.alignment = "right";
        doc.styles.tableBodyOdd.alignment = "right";
        doc.content[0].alignment = 'right';
        doc.content[1].alignment = 'right';
        //This section loops thru each row in table looking for where either
        //the second or third cell is empty.
        //If both cells empty changes rows background color to '#FFF9C4'
        //if only the third cell is empty changes background color to '#FFFDE7'
        // ***
        //$('#EmployeesTable').find('tr').each(function (ix, row) {
        //    var index = ix;
        //    var rowElt = row;
        //    $(row).find('td').each(function (ind, elt) {
        //        if (tblBody[index][1].text == '' && tblBody[index][2].text == '' && tblBody[index][ind] != undefined) {
        //            delete tblBody[index][ind].style;
        //            tblBody[index][ind].fillColor = '#FFF9C4';
        //        }
        //        else {
        //            if (tblBody[index][2].text == '' && tblBody[index][ind] != undefined) {
        //                delete tblBody[index][ind].style;
        //                tblBody[index][ind].fillColor = '#FFFDE7';
        //            }
        //        }
        //    });
        //});
    }

    else {
        doc.styles.tableBodyEven.alignment = "left";
        doc.styles.tableBodyOdd.alignment = "left";
        doc.content[0].alignment = 'left';
        doc.content[1].alignment = 'left';

    }

    var tblBody = doc.content[1].table.body;
    // ***
    //This section creates a grid border layout
    // ***
    doc.content[1].layout = {
        hLineWidth: function (i, node) {
            return (i === 0 || i === node.table.body.length) ? 2 : 1;
        },
        vLineWidth: function (i, node) {
            return (i === 0 || i === node.table.widths.length) ? 2 : 1;
        },
        hLineColor: function (i, node) {
            return (i === 0 || i === node.table.body.length) ? 'black' : 'gray';
        },
        vLineColor: function (i, node) {
            return (i === 0 || i === node.table.widths.length) ? 'black' : 'gray';
        }
    };
    //Wirdth 
    var colCount = new Array();
    $(TableSelector).find('tbody tr:first-child td:visible:not(.select-checkbox):not(.HideFromExport)').each(function () {

        if ($(this).attr('colspan')) {
            for (var i = 1; i <= $(this).attr('colspan'); $i++) {
                //colCount.push('auto');
                colCount.push('*');

            }
        }
        else {
            //colCount.push('auto');
            colCount.push('*');
        }

    });
    doc.content[1].table.widths = colCount;
    doc.info = {
        title: 'awesome Document testttt',
        author: 'john doe testtt',
        subject: 'subject of document testtt',
        keywords: 'keywords for document testttt',
    };
    doc.watermark = { text: 'MountainKW ', angle: 70, opacity: 0.3, bold: true, italics: true, fontSize: 40 };
    doc.defaultStyle =
    {
        font: 'ArabicFont',
        fontSize: 12,
        bold: true

    };

}
function Create_DatatablePDF(Selector) {
    var DatatableInstance = new $.fn.dataTable.Api(Selector)
    DatatableInstance.button('.buttons-pdf').trigger();
}
function Create_DatatableExcele(Selector) {
    var DatatableInstance = new $.fn.dataTable.Api(Selector)
    DatatableInstance.button('.buttons-excel').trigger();
}
function Create_DatatablePrint(Selector) {
    var DatatableInstance = new $.fn.dataTable.Api(Selector)
    DatatableInstance.button('.buttons-print').trigger();
}
function GetDatatablePdfConfig(TableSelector) {
    return {
        extend: 'pdfHtml5',
        text: 'PDF',
        customize: function (doc) {
            CustomSettingForDatatablePDF(doc, TableSelector);
        },
        titleAttr: 'Generate PDF',
        className: 'btn-outline-danger btn-sm d-none',
        exportOptions: {
            columns: ':visible:not(.select-checkbox):not(.HideFromExport)',
            orthogonal: "portrait",
        },



    }
}
function GetDatatableExcelConfig(TableSelector) {
    return {
        //extend: 'excel',
        //customize: function (doc) {
        //    CustomSettingForDatatableExcel(doc, TableSelector);
        //},
        extend: 'excelHtml5',
        orientation: 'landscape',
        customizeData: function (data) {
            for (var i = 0; i < data.body.length; i++) {
                for (var j = 0; j < data.body[i].length; j++) {
                    data.body[i][j] = '\u200C' + data.body[i][j];
                }
            }
        },
        customize: function (xlsx) {

            //var sheet = xlsx.xl.worksheets['sheet1.xml'];
            //$('c[r=A1] t', sheet).text('Custom text');

        },
        //titleAttr: 'Generate PDF',
        className: 'btn-outline-danger btn-sm d-none',
        exportOptions: {
            columns: ':visible:not(.select-checkbox):not(.HideFromExport)',
            //orthogonal: "portrait",
        },



    }
}
function GetDatatablePrintConfig(TableSelector) {
    return {
        extend: 'print',
        //customize: function (doc) {
        //    CustomSettingForDatatableExcel(doc, TableSelector);
        //},
        //titleAttr: 'Generate PDF',
        className: 'btn-outline-danger btn-sm d-none',
        exportOptions: {
            columns: ':visible:not(.select-checkbox):not(.HideFromExport)',
            //orthogonal: "portrait",
        },



    }
}
function isArabic(text) {
    var pattern = /[\u0600-\u06FF\u0750-\u077F]/;
    result = pattern.test(text);
    return result;
}
function DatatableCommonDomHtml() {
    return "<'row'" +
        "<'col-sm-6 d-flex align-items-center justify-content-start'l>" +
        "<'col-sm-6 d-flex align-items-center justify-content-end'f>" +
        ">" +
        "<'table-responsive'tr>" +
        "<'row'" +
        "<'col-sm-12 col-md-5 d-flex align-items-center justify-content-center justify-content-md-start'i>" +
        "<'col-sm-12 col-md-7 d-flex align-items-center justify-content-center justify-content-md-end'p>" +
        ">"
}