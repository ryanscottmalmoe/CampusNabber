
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


$(window).resize(function() {
    if ($(this).width() <= 636) 
        document.getElementById("logoImage").style.display = "none";
    else 
        document.getElementById("logoImage").style.display = "inline";
});
