
$(document).ready(() => {
    let CommonUploadDataFromExcelData = '';
    CommonUploadDataFromExcelData = CreateFilePondObj("#UploadDataFromExcelFile", false);


    $("#UploadDataFromExcelForm").on("submit", (e) => {
        e.preventDefault();
        let Form = $(e.currentTarget);
        let IsValidForm = $(Form).valid();
        $(Form).find("[disabled='disabled'][name]").each((i, el) => {
            if ($(el).valid() != true) {
                IsValidForm = false;
            }
        });
        if (IsValidForm) {
            let Url = `/${CurrentPageUrlInfo.Area}/${CurrentPageUrlInfo.Controller}/CreateFromExcele`;
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
                        RefreshPageDatatable();
                        RemoveAllFilesFromFilePond(CommonUploadDataFromExcelData);
                        $(Form).parents(".modal").modal("hide");
                        $("#UploadDataFromExcelModal").modal('hide');
                        if (res.ReturnedFileUrl != undefined && res.ReturnedFileUrl != "" && res.ReturnedFileUrl != null) {
                            window.open(res.ReturnedFileUrl, '_blank');
                        }
                        let MessageToApear = "";

                        if (res.SuccessCount > 0) {
                            MessageToApear += `<li>${getToken("NumberOfSavedRecords")}=${res.SuccessCount} سجل</li>`;
                            //ShowNotification("success", `عدد السجلات التي تم حفظها ${res.SuccessCount}`);
                            
                        }
                        if (res.FailedCount > 0) {
                            MessageToApear += `<li>${getToken("NumberOfInvalidRecords")}=${res.FailedCount} سجل</li>`;

                            //ShowNotification("warning", `عدد السجلات التي تم بها بيانات غير صحيحه ${res.FailedCount}`);
                        }
                        if (MessageToApear != "")
                            ShowNotification("warning", `<ul>${MessageToApear}</ul>`, getToken("DataAboutUploadedExceleFile"));
                    }
                    else {

                        AssignModelResponseErrorsToControllers(res.ModelErrors, Form);
                        ShowNotification("warning", res.Message);
                    }
                 

                    //    todo the logic
                    // remove the files from filepond, etc
                },
                error: function (data) {
                    
                    ShowNotification("error", getToken("خطا في الملف"));
                    RefreshPageDatatable();
                    RemoveAllFilesFromFilePond(CommonUploadDataFromExcelData)
                    //    todo the logic
                }
            }).done(function (data) {
                $(document).trigger("UploadDataFromExcelFormSubmit_Finished");
            });
        }
    });
    $("#UploadDataFromExcelModal").on('hidden.bs.modal', function () {
        $("#UploadDataFromExcelForm").trigger("reset");
        //$("select").change();
        FireDropDownChangeEvent("select");

        RemoveAllFilesFromFilePond(CommonUploadDataFromExcelData);
    });

});