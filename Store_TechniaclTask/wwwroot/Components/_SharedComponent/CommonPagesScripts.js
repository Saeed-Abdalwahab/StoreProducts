
async function ResetPageForm(ID = "") {
    $("#PageForm").trigger("reset");
    $("#ID").val(ID);
    $("#PageForm select").val("");
    clearValidation("#PageForm");
    //$('[data-control="select2"]').trigger('change.select2');
    //FireDropDownChangeEvent("select");
    //FireResetDropDownEvent("select");
    FireDropDownChangeEvent("#PageForm input[type='radio']");
    $("#PageForm select").trigger("change", { IsCodeTrigger: true, IsReset: true });
    return LoadAllCascadedDropdownInFormPage("#PageForm");
    //$("select").change();
}
async function LoadAllCascadedDropdownInFormPage(FormParent = "#PageForm") {
    //Here We Make Shure To Load All DropDownDataAfter Form Reset Fire
}
function GetPageFormObj(ID) {
    $.get(`/${CurrentPageUrlInfo.Area}/${CurrentPageUrlInfo.Controller}/GetObj?ID=${ID}`.replace('//','/'), (res) => {
        populateJsonObj_ToForm($("#PageForm").first(), res.Data ?? res);
        // We load All DropDown Again to hide data based on selected options
        LoadAllCascadedDropdownInFormPage(); //Comment1 show Hide Data Come From triiger event change 
        $("#PageModal").modal("show");
    }).fail((xhr, textStatus, errorThrown) => {
        ShowNotification("error", "");
    });
}
function OpenUploadDataFromExcelModal() {
    LoadAllCascadedDropdownInFormPage("#UploadDataFromExcelModal")

    $('#UploadDataFromExcelModal').modal('show');
}
function toogleActive(element, ID) {
    let isActive = $(element).is(":checked");
    new swal({
        title: getToken("alert"),
        text: isActive == true ? getToken("ConfirmActive") : getToken("ConfirmDeActive"),
        icon: "info",
        confirmButtonText: getToken("Confirm"),
        cancelButtonText: getToken("Cancele"),
        customClass: {
            confirmButton: "btn btn-primary",
            cancelButton: 'btn btn-info'
        },
        buttonsStyling: false,
        showCancelButton: true,
        dangerMode: true,
    })
        .then((willDelete) => {
            if (willDelete.value == true) {
                $.post(`/${CurrentPageUrlInfo.Area}/${CurrentPageUrlInfo.Controller}/toogleActive?ID=${ID}&IsActive=${isActive}`.replace('//', '/'), (res) => {
                    if (res.Status = true) {
                        ShowNotification("success", res.Message);
                    }
                    else {
                        ShowNotification("error", res.Message);
                    }
                    RefreshPageDatatable();
                })
            }
            else {
                //swal("Your imaginary file is safe!");
                $(element).prop("checked", !isActive)
                RefreshPageDatatable();
            }
        });
}
function OpenPageModal(ID) {
    if (ID == 0 || ID == '' || ID == undefined || ID == null) {
        ResetPageForm("0");
        $("#PageModal .PageModalAction").text((getToken("Add")));
        $("#PageModal").modal("show");
    }
    else {
        $("#PageModal .PageModalAction").text((getToken("Edit")));
        ResetPageForm(ID).then((a, b, c) => {
            GetPageFormObj(ID);
        });
    }
}


function OpenPageModal_Detailes(ID) {
    $("#PageModal").modal("hide");
    ResetPageForm(ID).then((a, b, c) => {
        if (ID > 0 || ID?.length > 0) {
            $.get(`/${CurrentPageUrlInfo.Area}/${CurrentPageUrlInfo.Controller}/GetObj?ID=${ID}`.replace('//', '/'), (res) => {
                populateJsonObj_ToForm($("#PageForm").first(), res.Data ?? res);
                $("#PageModal .PageModalAction").text((getToken("Details")));
                $("#PageForm").addClass("pe-none");
                $('#PageModal [type="submit"]').addClass("d-none");
                SetElmentReadolny($("#PageModal [class*='form-']"));
                $("#PageModal").modal("show");
            }).fail((xhr, textStatus, errorThrown) => {
                ShowNotification("error", "");
            });
        }
    });

}
$("#PageForm").on("submit", (e) => {
    clearValidation("#PageForm");
    e.preventDefault();
    let Form = $(e.currentTarget);
    let ID = $(Form).find("[name='ID']").val();
    let Action = ID == undefined || ID == '' || ID == null || ID == "0" || ID == 0 ? "/Create" : "/Edit";
    let Controller = "/" + CurrentPageUrlInfo.Controller;
    let Area = (CurrentPageUrlInfo.Area == null || CurrentPageUrlInfo.Area == undefined || CurrentPageUrlInfo.Area == "") ? "" : "/" + CurrentPageUrlInfo.Area;
    let Url = Area + Controller + Action;
    let IsValidForm = $(Form).valid();
    $(Form).find("[disabled='disabled'][name]").each((i, el) => {
        if ($(el).valid() != true) {
            IsValidForm = false;
        }
    })
    if (IsValidForm) {
        if ($(Form).find("input[type='file']").length == 0) {
            let DataToSent = $(Form).serialize();
            $.post(Url, DataToSent, (res) => {
                if (res.Status == true) {

                    ShowNotification("success", res.Message);
                    RefreshPageDatatable();
                    ResetPageForm();
                    $(Form).parents(".modal").modal("hide");
                    //if (Action != "/Create") {
                    //}
                }
                else {
                    AssignModelResponseErrorsToControllers(res.ModelErrors, Form);
                    ShowNotification("warning", res.Message);
                }
            }).fail((xhr, textStatus, errorThrown) => {
                ShowNotification("error", "");

            }).done(function (data) {

            });
        }
        else {
            let DataToSent = new FormData($(Form).get(0));
            $(Form).find(".filepond--root").each((i, elment) => {
                let filepondivID = "#" + $(elment).attr("id");
                let InputFileDefaultName = $(elment).find("input[name][type='hidden']").attr("name") ?? "Attachment";
                DataToSent = GetFilePondFilesInFormData(filepondivID, DataToSent, InputFileDefaultName)
            });
            $.ajax({
                "url": Url,
                "method": "Post",
                "datatype": "json",
                processData: false,
                contentType: false,
                //"Content-Type": "application/json; charset=utf8",
                data: DataToSent,
                success: function (res) {
                    if (res.Status == true) {
                        $(document).trigger("PageFormSubmit_Finished");
                        ShowNotification("success", res.Message);
                        RefreshPageDatatable();
                        ResetPageForm();
                        $(Form).parents(".modal").modal("hide");
                        //if (Action != "/Create") {
                        //}
                    }
                    else {
                        AssignModelResponseErrorsToControllers(res.ModelErrors, Form);
                        ShowNotification("warning", res.Message);
                    }
                },
                error: function (res) {
                    ShowNotification("error", res.Message);
                },

            }).done(function (data) {
            });
        }
    }
    else {
    }
});
$("#PageModal").on('hidden.bs.modal', function () {
    //ResetPageForm()
    $("#PageForm").removeClass("pe-none");
    SetElmentReadolny($("#PageModal [class*='form-']"), false);
    $('#PageModal [type="submit"]').removeClass("d-none");
});
$("#PageModal").on('shown.bs.modal', function () {
    if ($("#PageModal #PageForm").hasClass("pe-none") == false) {
        $("#PageModal input:not([type='hidden']):not(.flatpickr_Calander)").eq(0).focus();
    }
});
$("#PageModal").on('show.bs.modal', function () {
    IntialFlatPickrInPut();
});

$(document).on("RemovePageObject", function (event, ID, callBack) {
    $.post(`/${CurrentPageUrlInfo.Area}/${CurrentPageUrlInfo.Controller}/RemoveObj?ID=${ID}`.replace('//', '/'), (res) => {
        if (res.Status == true) {
            ShowNotification("success", res.Message);
            RefreshPageDatatable();
        }
        else {
            if (res.ListOfMessages?.length > 0) {
                let head = `<h5>${getToken("NeedToRemoveData")}</h5>`
                let body = `<ul>${res.ListOfMessages.map(x => `<li>${getToken(x)}</li>`)}</ul>`
                let foot = `<h6>${getToken("RelatedToComleteRemove")}</h6>`
                ShowNotification("error", head + body + foot, "");
            }
            else {
                ShowNotification("error", res.Message);
            }
        }

    }).fail((xhr, textStatus, errorThrown) => {
        ShowNotification("error", "");
    });
});
//$(document).on('keyup', 'span.select2-container--open input.select2-search__field', function (e) {
//    if (e.which == 40 || e.which == 38) {
//        var idx = $(this).closest('.select2-dropdown').find('li.select2-results__option--highlighted').index();
//        var myurl = $('#mycombo option').eq(idx).attr('url');
//        $("#myimage").attr("src", myurl);
//    }
//})
/*$("#PageForm ")*/