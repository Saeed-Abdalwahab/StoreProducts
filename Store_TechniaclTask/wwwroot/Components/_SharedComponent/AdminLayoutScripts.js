
var DataTableLengthMenuCommon = [[10, 50, 100, -1], [10, 50, 100, getToken("All")]];
function ShowSuccessCopidNotification(Message, OnclickFunc) {
    let Temptoastr = toastr;
    Temptoastr.options = {
        "closeButton": true,
        "debug": false,
        "newestOnTop": true,
        "progressBar": true,
        "positionClass": "toastr-top-right",
        "preventDuplicates": false,
        "onclick": OnclickFunc,
        //"showDuration": "3000",
        "showDuration": "0",
        "hideDuration": "0",
        "timeOut": "0",
        "extendedTimeOut": "0",
        "showEasing": "swing",
        "hideEasing": "linear",
        "showMethod": "fadeIn",
        "hideMethod": "fadeOut"
    };
    Temptoastr.success(Message, getToken("Notification") + '<i class="far fa-clone text-info m-3"></i>');
}

function IntialFlatPickrInPut(Parent) {
    $(".flatpickr_Calander").each((i, e) => {
        let parent = $(e).parent(Parent ?? "div")[0];
        var taskFlatpickrConfig = {
            "locale": CurrentPageUrlInfo.dir == "rtl" ? "ar" : "en", // locale for this instance only
            "firstDayOfWeek": 6,// start week on Monday
            //appendTo: document.getElementById('PageModal')
            //enableTime: true,
            //altInput: true,
            appendTo: parent
        };
        flatpickr(e, taskFlatpickrConfig);
    })
}

function RemoveUnassignedPolicy() {
    //if (IsSuperAdmin.toLowerCase() != "true".toLowerCase()) {

        //$("[data-PolicyType]").each((i, elment) => {
        //    let Permission = $(elment).attr("data-PolicyType");
        //    if (IsInPolicy(Permission) == false) {
        //        $(elment).remove();
        //    }
        //});
    //}
}
function IsInPolicy(PolicyName) {
    return true;
    //return UserCurrentPagePermissions.includes(PolicyName.toLowerCase().trim()) == true || UserCurrentPagePermissions.includes(SystemPermissions.find(x => x.Value == PolicyName)?.key);
}
function AssignUserLanguage() {
    if (CurrentUserLanguage.toLocaleLowerCase() == "ar-eg") {
        $("#SpanLanguage").text("العربية");
        $("#ImageLanguage").attr("src", "/assets/media/flags/egypt.svg");
    }
    else {
        $("#SpanLanguage").text("Language");
        $("#ImageLanguage").attr("src", "/assets/media/flags/united-states.svg");
    }
}
function ChangeUserLanguage(Language) {
    $.post("/home/SetLanguage?language=" + Language, (re) => {
        location.reload();
    })
}
$(function () {
    $.fn.filepond.registerPlugin(
        FilePondPluginFileEncode,
        FilePondPluginFileValidateSize,
        FilePondPluginImageExifOrientation,
        FilePondPluginImagePreview,
        FilePondPluginFileValidateType
    );
});
function ActiveItemInMenu() {
    $("#kt_aside").find("a.menu-link").each((i, el) => {
        let url = `/${CurrentPageUrlInfo.Area}/${CurrentPageUrlInfo.Controller}/${CurrentPageUrlInfo.Action}`.toLowerCase();
        url=  url.replace("//","/")
        let menulink = $(el).attr("href")?.toLowerCase();
        if (url.includes(menulink) == true) {
            $(el).addClass("active");
            $(el).parents('div[data-kt-menu-trigger="click"]').addClass("hover").addClass("show");;
        }
    })
    document.querySelector("#kt_aside .menu-item .active").scrollIntoView({ behavior: 'smooth' })

}

function CreateSelect2DropDown(Selectselector, parentselector = ".modal-body") {
    $(Selectselector).each((i, e) => {
        //let parent = $("#PageModal");
        let parent = $(e).parents(parentselector);
        //let parent = $(e).parents("form");
        $(e).select2({
            dropdownParent: parent,
            dir: CurrentPageUrlInfo.dir,
        }).on('select2:open', (e) => {
            
            //document.querySelector('#PageModal .select2-search__field').focus();
        });
    })
}
function Select2matchCustom(params, data) {
    var Result = null;
    // If there are no search terms, return all of the data
    if ($.trim(params.term) === '') {
        return data;
    }
    // Do not display the item if there is no 'text' property
    if (typeof data.text === 'undefined') {
        return null;
    }

    // `params.term` should be the term that is used for searching
    // `data.text` is the text that is displayed for the data object
    if (data.text.toLocaleLowerCase().trim().indexOf(params.term.toLocaleLowerCase().trim()) > -1) {
        var modifiedData = $.extend({}, data, true);
        modifiedData.text += '  (matched)';
        // You can return modified objects from here
        // This includes matching the `children` how you want in nested data sets
        return modifiedData;
    }
    $.each(data.element.attributes, (i, attribute) => {
        if (attribute.name.toLowerCase().indexOf("data-search") > -1 || attribute.name.toLowerCase().indexOf("data_search") > -1) {
            let attrValue = attribute.value;
            if (attrValue.toLowerCase().trim().indexOf(params.term.toLocaleLowerCase().trim()) > -1) {
                var modifiedData = $.extend({}, data, true);
                modifiedData.text += '  (matched)';
                // You can return modified objects from here
                // This includes matching the `children` how you want in nested data sets
                Result = modifiedData;
                return;
            }
        }
    });
    // Return `null` if the term should not be displayed
    return Result;
}


function CreateSelect2DropDownWithCustomSearch(Selectselector, parentselector =".modal-body") {
    $(Selectselector).each((i, e) => {
        //let parent = $("#PageModal");
        let parent = $(e).parents(parentselector);
        //let parent = $(e).parents("form");
        $(e).select2({
            dropdownParent: parent,
            dir: CurrentPageUrlInfo.dir,
            matcher: Select2matchCustom
        }).on('select2:open', (e) => {
            //document.querySelector('#PageModal .select2-search__field').focus();
        });
    })
}
$(document).ready(function () {
    $(".model.show").keypress(function (event) {
        var keycode = (event.keyCode ? event.keyCode : event.which);
        if (keycode == '13') {
            $(".model.show form").submit();
        }
    });
    $(document).ajaxSend(function (event, jqxhr, settings) {

        let indicator_label = '';
        let indicator_progress = '';
        var el = $(event.target.activeElement);
        if ($(el).prop('nodeName') != undefined) {
            var elementType = $(el).prop('nodeName').toLowerCase();
            switch (elementType) {
                case "input":
                    break;
                case "button":
                    indicator_progress = $(el).find("span.indicator-progress");
                    indicator_label = $(el).find("span.indicator-label");
                    if (indicator_progress.length > 0) {
                        $(indicator_progress).addClass("d-inline");
                        $(indicator_progress).removeClass("d-none");
                        $(indicator_label).addClass("d-none");
                        $(indicator_label).removeClass("d-inline");
                        $(el).prop('disabled', true);
                        settings.selector = $(el);
                    }
                    break;
                case "a":
                    indicator_progress = $(el).find("span.indicator-progress");
                    indicator_label = $(el).find("span.indicator-label");
                    if (indicator_progress.length > 0) {
                        $(indicator_progress).addClass("d-inline");
                        $(indicator_progress).removeClass("d-none");
                        $(indicator_label).addClass("d-none");
                        $(indicator_label).removeClass("d-inline");
                        $(el).prop('disabled', true);
                        settings.selector = $(el);
                    }
                    break;
                case "label":
                    indicator_progress = $(el).find("span.indicator-progress");
                    indicator_label = $(el).find("span.indicator-label");
                    if (indicator_progress.length > 0) {
                        $(indicator_progress).addClass("d-inline");
                        $(indicator_progress).removeClass("d-none");
                        $(indicator_label).addClass("d-none");
                        $(indicator_label).removeClass("d-inline");
                        $(el).prop('disabled', true);
                        settings.selector = $(el);
                    }
                    break;
                default:
            }
        }
    });
    $(document).ajaxComplete(function (event, xhr, settings) {
        if (settings.selector !== undefined) {
            let el = $(settings.selector);
            let indicator_progress = $(el).find("span.indicator-progress");
            let indicator_label = $(el).find("span.indicator-label");
            if (indicator_progress.length > 0) {
                $(indicator_progress).removeClass("d-inline");
                $(indicator_progress).addClass("d-none");
                $(indicator_label).removeClass("d-none");
                $(indicator_label).addClass("d-inline");
                $(el).prop('disabled', false);
            }
        }
    });
    CreateSelect2DropDown(".Common_Select2");
    CreateSelect2DropDownWithCustomSearch(".Select2_CustomSearch");
    RemoveUnassignedPolicy();
    AssignUserLanguage();
    ActiveItemInMenu();
    $('form [data-val-required]').each(function () {
        if ($(this).attr("data-val-required") && $(this).attr("type") != "checkbox") {
            $(this).prev('label[for]').addClass('required');
        }
        else {
            $(this).prev('label[for]').removeClass('required');
        }
    })
})