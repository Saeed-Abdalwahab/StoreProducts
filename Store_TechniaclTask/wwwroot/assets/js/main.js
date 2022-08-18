$(document).ready(function () {
	'use strict';
	
	// Datatable
	$(".kt_datatable").DataTable({
		"language": {
			//"lengthMenu": "Show _MENU_",
			'url': DataTableLanguageUrl(),

		},
		"dom": "<'row'" +
			"<'col-sm-6 d-flex align-items-center justify-conten-start'l>" +
			"<'col-sm-6 d-flex align-items-center justify-content-end'f>" +
			">" +

			"<'table-responsive'tr>" +

			"<'row'" +
			"<'col-sm-12 col-md-5 d-flex align-items-center justify-content-center justify-content-md-start'i>" +
			"<'col-sm-12 col-md-7 d-flex align-items-center justify-content-center justify-content-md-end'p>" +
			">"
	});
	
	// flatpicker "DatePicker"
	$(".kt_calendar_datepicker").flatpickr();
	
	//**** Begin Form Validation  ****//
	
	// Define form element
	const form = document.getElementById('kt_modal_new_target_form');

	// Init form validation rules. For more info check the FormValidation plugin's official documentation:https://formvalidation.io/
	var validator = FormValidation.formValidation();

	//// Submit button handler
	//const submitButton = document.getElementById('kt_modal_new_target_submit');
	//submitButton.addEventListener('click', function (e) {
	//	// Prevent default button action
	//	e.preventDefault();

	//	// Validate form before submit
	//	if (validator) {
	//		validator.validate().then(function (status) {

	//			if (status == 'Valid') {
	//				// Show loading indication
	//				submitButton.setAttribute('data-kt-indicator', 'on');

	//				// Disable button to avoid multiple click
	//				submitButton.disabled = true;

	//				// Simulate form submission. For more info check the plugin's official documentation: https://sweetalert2.github.io/
	//				setTimeout(function () {
	//					// Remove loading indication
	//					submitButton.removeAttribute('data-kt-indicator');

	//					// Enable button
	//					submitButton.disabled = false;

	//					// Show popup confirmation
	//					Swal.fire({
	//						text: "Form has been successfully submitted!",
	//						icon: "success",
	//						buttonsStyling: false,
	//						confirmButtonText: "Ok, got it!",
	//						customClass: {
	//							confirmButton: "btn btn-primary"
	//						}
	//					});

	//					//form.submit(); // Submit form
	//				}, 2000);
	//			}
	//		});
	//	}
	//});
	
	//** End Form Validation  **//
	
	//// Sweet Alert
	//const button = document.getElementById('kt_sweetalert');

	//button.addEventListener('click', e =>{
	//	e.preventDefault();

	//	Swal.fire({
	//		html: `Confirm Delete ?`,
	//		icon: "info",
	//		buttonsStyling: false,
	//		showCancelButton: true,
	//		confirmButtonText: "Ok, got it!",
	//		cancelButtonText: 'Nope, cancel it',
	//		customClass: {
	//			confirmButton: "btn btn-primary",
	//			cancelButton: 'btn btn-danger'
	//		}
	//	});
	//});
	
	
});
