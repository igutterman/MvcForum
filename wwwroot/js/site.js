// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.




function resizeImage(event) { 

var image = event.target;

var size = "thumb";

if (image.getAttribute("data-expanded") === "0") {
    size = "full";
    }

var request;
if (window.XMLHttpRequest) {
    //New browsers
    request = new XMLHttpRequest();
    }
else if(window.ActiveXObject) {
    //Old IE browsers
    request = new ActiveXObject("Microsoft.XMLHTTP");
    }

var ajaxparams = {FileSize: size, FileName: image.id};


//console.log(value);


if (request != null) {
    var url = "/LoadImage";
    request.open("POST", url, true);
    request.setRequestHeader("Content-Type", "application/json");
    request.onreadystatechange = function() {
    if (request.readyState == 4 && request.status == 200) {
        var response = JSON.parse(request.responseText);

        image.src = '/images/' + response;

    if (image.getAttribute("data-expanded") === "0") {
        image.dataset.expanded = "1";
    } else {
        image.dataset.expanded = "0";
            }   
        }
    };
    request.send(JSON.stringify(ajaxparams));
    }

}


