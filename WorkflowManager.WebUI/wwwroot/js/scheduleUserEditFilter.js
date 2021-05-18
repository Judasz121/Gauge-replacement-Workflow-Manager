var allJobsJSON, inputs;
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
    //console.log(inputs);

    var data = [...AlljobsJSON];
    let i;
    
    for (i = 0; i < data.length; i++) {
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
            let dataAddress = data[i].building.city + " " + data[i].building.street + " " + data[i].building.addressBuildingNum + " " + data[i].building.addressAdditional;
            if (dataAddress.toUpperCase().includes(inputs['filter_building'].value.toUpperCase())) {
                match = true;
            }
        }

        if (inputs[0].value + inputs[1].value + inputs[2].value + inputs[3].value == "")
            match = true;

        if (match == false) {
            delete data[i];
            
        }
    }

    // update front-end
    var allJobsContent = document.getElementById("userEditSortable-AllJobs");
    var newHTML = "";
    //let i; already declared above
    for (i = 0; i < data.length; i++) {
        if (typeof data[i] != 'undefined') {
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
    }
    allJobsContent.innerHTML = "";
    allJobsContent.innerHTML = newHTML;
}