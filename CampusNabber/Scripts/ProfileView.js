
function posts() {
     document.getElementById("postItemTable").hidden = false;
     document.getElementById("settingsSection").style.display = "none";
     document.getElementById("deactivateButton").style.display = "none";
}


function settings() {
    document.getElementById("postItemTable").hidden = true;
    document.getElementById("settingsSection").style.display = "inline";
    document.getElementById("deactivateButton").style.display = "inline";
}

