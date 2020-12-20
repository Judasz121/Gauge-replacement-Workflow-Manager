$(document).ready(function () {
    var i;
    var elementsToChosenise = document.querySelectorAll("select[id^=chosen");
    elementsToChosenise.forEach(function (item){
        $(item).chosen({
            no_results_text: "Nie znaleziono użytkownika",
            disable_search_threshold: 5,
        });
    });
});