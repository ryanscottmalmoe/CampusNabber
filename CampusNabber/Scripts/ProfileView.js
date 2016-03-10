
function posts() {
     document.getElementById("postItemTable").hidden = false;
     document.getElementById("settingsSection").style.display = "none";
     document.getElementById("deactivateButton").style.display = "none";
     editButtonsOff();
}


function settings() {
    document.getElementById("postItemTable").hidden = true;
    document.getElementById("settingsSection").style.display = "inline";
    document.getElementById("deactivateButton").style.display = "inline";
}

function editButtonsOff() {
    document.getElementById("editUsername").style.display = "none";
    document.getElementById("editEmail").style.display = "none";
    document.getElementById("editSchoolName").style.display = "none";
    document.getElementById("saveButton").style.display = "none";

}

function username() {
    document.getElementById("editUsername").style.display = "inline";
    document.getElementById("editEmail").style.display = "none";
    document.getElementById("editSchoolName").style.display = "none";
    document.getElementById("saveButton").style.display = "inline";

}
function email() {
    document.getElementById("editUsername").style.display = "none";
    document.getElementById("editEmail").style.display = "inline";
    document.getElementById("editSchoolName").style.display = "none";
    document.getElementById("saveButton").style.display = "inline";
}
function school() {
    document.getElementById("editUsername").style.display = "none";
    document.getElementById("editEmail").style.display = "none";
    document.getElementById("editSchoolName").style.display = "inline";
    document.getElementById("saveButton").style.display = "inline";

}
function password() {
    document.getElementById("editUsername").style.display = "none";
    document.getElementById("editEmail").style.display = "none";
    document.getElementById("editSchoolName").style.display = "none";
    document.getElementById("saveButton").style.display = "none";

}
