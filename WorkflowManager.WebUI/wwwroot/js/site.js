// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.



$(document).ready(function () {

});





$("input#residentSign").change(function () {
    // readURL(this);
    if (this.files && this.files[0]) {
        var reader = new FileReader();

        reader.onload = function (e) {
            $('img#residentSign').attr('src', e.target.result);
        }

        reader.readAsDataURL(this.files[0]); // convert to base64 string
    }
});

function alertBlinkOnElementBorder(target, blinkInterval, blinkLength, repeatAmount) {
    var blinkInterval = parseInt(blinkInterval);
    var blinkLength = parseInt(blinkLength);
    var repeatAmount = parseInt(repeatAmount);
    repeatAmount--;

    target.classList.Add("alertBlink");
    setTimeout(() => {
        target.classList.Remove("alertBlink");
        setTimeout(() => {
            if (repeatAmount != 0)
                alertBlink(target, blinkInterval, blinkLength, repeatAmount)
        }, blinkInterval)
    }, blinkLength);
}