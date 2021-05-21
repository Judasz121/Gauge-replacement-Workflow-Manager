$(document).ready(function () {
    SetUpScheduleBuildingEditSortables();
});

    /* --== BUILDING EDIT ==-- */
    function SetUpScheduleBuildingEditSortables() {
        var dataPlaceholder = document.getElementById("JobsOrder");
        var targetList = $("#buildingEditSortable");
        targetList.sortable({
            update: function (e, ui) {
                dataPlaceholder.value = targetList.sortable("serialize");
            },
            disableSelection: true
        });
    }
