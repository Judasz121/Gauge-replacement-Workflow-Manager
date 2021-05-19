

// --== FILTER ==--

var allJobsJSON, inputs, filterResult;
var filter = document.getElementById("Alljobs-filter-content");
if (filter != null) {

    // download data
    let url = window.location.href.split('/');
    var userId = url[url.length - 1];
    $.ajax({
        type: "POST",
        url: '/Schedule/GetAllJobs',
        data: { id: userId },
        dataType: "json",
        success: function (response) {
            allJobsJSON = response;
        },
        error: function (req, status, error) {
            console.log("req:");
            console.log(req);
            console.log("status:");
            console.log(status);
            console.log("error:");
            console.log(error);
        }
    })

    // setup input events
    inputs = filter.getElementsByTagName('input');
    let i;
    for (i = 0; i < inputs.length; i++) {
        inputs[i].addEventListener("keyup", function () {
            FilterUpdate(inputs, allJobsJSON);
        });
    }
    

}


function FilterUpdate(inputs, AlljobsJSON) {
    var data = [...AlljobsJSON];

    let i;
    for (i = 0; i < data.length; i++) {
        if (inputs[0].value + inputs[1].value + inputs[2].value + inputs[3].value == "")
            break;
        if (typeof data[i] == 'undefined')
            continue;

        let match = false;

        if (inputs['filter_asignedName'].value != "") {
            let j;
            for (j = 0; j < data[i].userJobs.length; j++) {
                if (data[i].userJobs[j].user.firstName.toUpperCase().includes(inputs['filter_asignedName'].value.toUpperCase())) {
                    match = true;
                }
            }
        }
        if (!match && inputs['filter_asignedSurname'].value != "") {
            let j;
            for (j = 0; j < data[i].userJobs.length; j++) {
                if (data[i].userJobs[j].user.lastName.toUpperCase().includes(inputs['filter_asignedSurname'].value.toUpperCase())) {
                    match = true;
                }
            }
        }
        if (!match && inputs['filter_jobName'].value != "") {
            if (data[i].name.toUpperCase().includes(inputs['filter_jobName'].value.toUpperCase())) {
                match = true;
            }
        }
        if (!match && inputs['filter_building'].value != "") {
            if (data[i].building.fullAddress.toUpperCase().includes(inputs['filter_building'].value.toUpperCase())) {
                match = true;
            }
        }

        if (match == false) {
            delete data[i];
        }
    }
    filterResult = data;

    // update front-end
    var allJobsContent = document.getElementById("userEditSortable-AllJobs");
    var newHTML = "";
    //let i; already declared above
    for (i = 0; i < data.length; i++) {
        if (typeof data[i] == 'undefined')
            continue;

        let currJob = data[i];
        let jobHTML = `<li id="job_` + currJob.id + `" class="ui-sortable-handle">
						<div class="card job-card">
							<div class="card-header">
								<a class="object-link" href="/Job/Details/` + currJob.id + `">` + currJob.name + `</a>
							</div>
							<div class="card-body">
								<div class="row">
									<div class="col">
										Budynek
										<hr>
										<a class="object-link" href="/Building/Details/` + currJob.building.id + `">` + currJob.building.fullAddress + `</a>
									</div>
									<div class="col">
										Przypisani
										<hr>`;
        let j;
        for (j = 0; j < currJob.userJobs.length; j++) {
            let currUser = currJob.userJobs[j].user;
            jobHTML = jobHTML + `
                                        <a class="object-link" href="/User/Details/` + currUser.id + `">` + currUser.fullName + `</a>
                                        <br>`;
        }
        jobHTML = jobHTML + `
									</div>
								</div>
							</div>
						</div>
					</li>`;
        newHTML = newHTML.concat(jobHTML);
        
    }
    allJobsContent.innerHTML = "";
    allJobsContent.innerHTML = newHTML;
}

//sortable integration

function AddItemToFilterData(itemArray) {
    let item = itemArray[0];
    let jobId = parseInt(item.id.split('_')[1]);
    let jobName = item.querySelector('.card.job-card .card-header a').innerHTML;
    let buildingAddress = item.querySelector('.card.job-card .card-body .row .col a').innerHTML;

    let asigned = item.querySelectorAll('.card.job-card .card-body .row .col');
    asigned = asigned[asigned.length - 1];
    asigned = asigned.querySelectorAll('a');
    let building = {fullAddress: buildingAddress}
    let job = { id: jobId, name: jobName, building: building, userJobs: null };
    let i;
    let userJobs = new Array();
    for (i = 0; i < asigned.length; i++) {
        let userId = asigned[i].href.split('/');
        userId = userId[userId.length - 1];
        let userFullName = asigned[i].innerHTML;
        let userFirstname = userFullName.split(' ')[0];
        let userSurname = userFullName.split(' ')[1];

        let user = { id: userId, firstName: userFirstname, lastName: userSurname, fullName: userFullName };
        let userJob = { jobId: jobId, user: user, userId: userId };
        userJobs.push(userJob);
    }
    job.userJobs = userJobs;

    allJobsJSON.push(job);
}
function RemoveItemFromFilterData(itemArray) {
    let item = itemArray[0];
    let itemId = parseInt(item.id.split('_')[1]);
    let i;
    for (i = 0; i < allJobsJSON.length; i++) {
        if (allJobsJSON[i].id == itemId) {
            delete allJobsJSON[i];
            break;
        }
    }
}


// --== SORTABLE ==--

$(document).ready(function () {
    SetUpScheduleUserEditSortables();
    $("#userEditSortable-AssignedJobs").sortable('option', 'update')();
});

function SetUpScheduleUserEditSortables() {
    var dataPlaceholder = document.getElementById("AssignedJobs");
    var assignedJobsList = $("#userEditSortable-AssignedJobs");
    var allJobsList = $("#userEditSortable-AllJobs");

    assignedJobsList.sortable({
        update: function (e, ui) {
            dataPlaceholder.value = assignedJobsList.sortable("serialize");
            if (typeof ui != 'undefined' && ui.sender != null && ui.sender[0].id == "userEditSortable-AllJobs")
                RemoveItemFromFilterData(ui.item)
        },
        connectWith: ".connectedSortable"
    });
    allJobsList.sortable({
        update: function (e, ui) {
            if (typeof ui != 'undefined' && ui.sender != null && ui.sender[0].id == "userEditSortable-AssignedJobs")
                AddItemToFilterData(ui.item)
        },
        connectWith: ".connectedSortable"
    });
}