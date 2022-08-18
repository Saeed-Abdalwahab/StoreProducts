toastr.options = {
    "closeButton": true,
    "debug": false,
    "newestOnTop": true,
    "progressBar": true,
    "positionClass": "toastr-top-right",
    //'close-button': true,
     //"closeHtml": '<button><i class="icon-off"></i></button>',
    "preventDuplicates": false,
    "onclick": null,
    "showDuration": "300",
    "hideDuration": "100",
    "timeOut": "5000",
    "extendedTimeOut": "100",
    "showEasing": "swing",
    "hideEasing": "linear",
    "showMethod": "fadeIn",
    "hideMethod": "fadeOut"
};

var NotificationType = {
    Success:1,
    Error:2,
    Warning:3,
    Info:4
}

function ShowNotification(NotificationType, Message, Title, options) {
    toastr.options = {
        "closeButton": true,
        "debug": false,
        "newestOnTop": true,
        "progressBar": true,
        "positionClass": "toastr-top-right",
        //'close-button': true,
        //"closeHtml": '<button><i class="icon-off"></i></button>',
        "preventDuplicates": false,
        "onclick": null,
        "showDuration": "300",
        "hideDuration": "100",
        "timeOut": "5000",
        "extendedTimeOut": "100",
        "showEasing": "swing",
        "hideEasing": "linear",
        "showMethod": "fadeIn",
        "hideMethod": "fadeOut"
    };
    toastr.options = JsonConcat(toastr.options, options)
    Title = Title == undefined||Title=="" ? getToken("Notification") : Title;
    NotificationType = NotificationType.toLowerCase();
    switch (NotificationType) {
        case "success":
            toastr.success(Message,Title);
            break;
        case "info":
            toastr.info(Message, Title);
            break;
        case "warning":
            toastr.warning(Message, Title);
            break;
        case "error":
            toastr.error(Message, Title);
            break;
        default:
            break;
    }
}