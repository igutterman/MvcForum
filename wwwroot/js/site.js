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



//variables and functions for file previews
var counter = 0;
var filesArray = new Array();

document.getElementById('FileSelector').onchange = function () {

    if (counter === 4) {
        alert("Can't post more than 4 files.");
        setInputFileList();
        console.log(this.files);
        return;
    }


    let src = URL.createObjectURL(this.files[0]);
    filesArray.push(this.files[0]);

    let child = document.createElement("div");
    child.id = "previews" + counter;
    child.style.display = "flex";
    child.style.flexDirection = "column";



    document.getElementById('FilePreviews').appendChild(child);

    let childspan = document.createElement("span");

    child.appendChild(childspan);

    let button = document.createElement("button");
    button.type = "button";
    button.textContent = "X";
    button.classList.add("removeFileButton");
    button.id = "button" + counter;
    button.setAttribute('onclick', 'removeFile(event)');
    childspan.append(button);

    childspan.append(this.files[0].name);

    let childimg = document.createElement("img");

    childimg.style.maxWidth = 200 + "px";
    childimg.style.maxHeight = 200 + "px";
    childimg.style.objectFit = "contain";

    child.appendChild(childimg);


    childimg.src = src;

    counter++;
    console.log(filesArray);

    console.log("filesarray length:");
    console.log(filesArray.length);
}

function setInputFileList() {
    let fileInput = document.getElementById('FileSelector');

    if (filesArray.length === 0) {
        console.log("here");
        fileInput.value = null;

    } else {
        console.log("here2");
        let dT = new DataTransfer();
        for (const file of filesArray)
            dT.items.add(file);
        //dT.items.add(...filesArray);
        fileInput.value = null;
        fileInput.files = dT.files;
    }
    console.log("filesarray length:");
    console.log(filesArray.length);
    console.log(fileInput.files);
}

function removeFile(event) {
    //var target = event.target;
    //console.log(target.id);
    let index = event.target.parentElement.id.charAt(event.target.id.length - 1);


    //change this to just the splice line
    if (filesArray.length === 1) {
        filesArray.length = 0;
    } else {
        filesArray.splice(parseInt(index), 1);
    }

    event.target.parentElement.parentElement.remove();


    //decrement id's of subsequent divs, buttons
    for (let i = index + 1; i < 4; i++) {
        let ele = document.getElementById("previews" + i);
        let ele2 = document.getElementById("button" + i);
        if (ele) {
            ele.id = "previews" + (i - 1);
            ele2.id = "button" + (i - 1);
        }
    }

    counter--;
    console.log(filesArray);
    setInputFileList();
}

//end of variables and functions for file previews