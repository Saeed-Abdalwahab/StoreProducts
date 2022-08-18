
$("body").append('<div id="DynamicLG"></div>');
const $dynamicGallery = document.getElementById('DynamicLG');

function OpenFsLightbox(Settings) {
    
    SharedlightboxObj.props.sources = Settings.sources;
    //SharedlightboxObj.props.source = Settings.source;
    //lightbox.props.onInit = () => console.log('Lightbox initialized!');
    SharedlightboxObj.open();
}
function LightGalaryclick(Source) {
    let dynamicGallery = lightGallery($dynamicGallery, {
    //    dynamic: true,
    //    dynamicEl: [
    //        {
    //            src: Source,
    //            thumb: '',
    //            subHtml: '',
    //            subHtmlUrl: Source
    //        }
          
    //    ],
    });
    dynamicGallery.openGallery(2);
}

$(document).on("click", ".lightGalary", (el) => {

    //let Source = $(el.currentTarget).attr("data-source");
    //let Source = $(el.currentTarget).find("a:first-child > img").attr("")
    //let dynamicGallery = lightGallery(el.currentTarget,
    //    {
    //    dynamic: true,
    //    dynamicEl: [
    //    {
    //        src: Source,
    //        thumb: '',
    //        subHtml: '',
    //        subHtmlUrl: Source
    //    }

    //]}   );
    //$(el.currentTarget).find("a:first-child > img").trigger("click");

})

