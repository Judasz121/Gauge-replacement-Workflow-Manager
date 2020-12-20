$(document).ready(function () {
    SetUpScheduleUserEditSortables();
    SetUpScheduleBuildingEditSortables();
    $("#userEditSortable-AssignedJobs").sortable('option', 'update')();
});
/* --== USER EDIT ==-- */
function SetUpScheduleUserEditSortables() {
    var dataPlaceholder = document.getElementById("AssignedJobs");
    var assignedJobsList = $("#userEditSortable-AssignedJobs");
    var allJobsList = $("#userEditSortable-AllJobs");

    assignedJobsList.sortable({
        update: function (e, ui) {
            dataPlaceholder.value = assignedJobsList.sortable("serialize");
        },
        connectWith: ".connectedSortable"
    });
    allJobsList.sortable({
        connectWith: ".connectedSortable"
    });
}


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
