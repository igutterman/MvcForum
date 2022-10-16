// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.




function resizeImage(event) { 

    var image = event.target;
    //image.style.opacity = "0.8";

    let isOpPic = false;
    if (image.parentElement.parentElement.parentElement.parentElement.className === "OpPost") {
        isOpPic = true;
    }



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
            //image.style.opacity = "1";

        if (image.getAttribute("data-expanded") === "0") {
            image.dataset.expanded = "1";
        } else {
            image.dataset.expanded = "0";
    
        }
            }
            if (isOpPic) {
                checkOPImageSizes(image)
            }
        }
    };
    request.send(JSON.stringify(ajaxparams));
}






//Takes an OP-image as target. Goes up four levels to get the "OpPost"-class div.
//Checks if any OP-post images in that thread are in expanded state (based on data-expanded property).
//If so, sets OP-post "PostText" to "clear: both". Otherwise, sets to clear: none
function checkOPImageSizes(target) {
    let opPost = target.parentElement.parentElement.parentElement.parentElement;
    let opText = null;
    for (var i = 0; i < opPost.childNodes.length; i++) {
        if (opPost.childNodes[i].className === "PostText") {
            opText = opPost.childNodes[i];
            break;
        }
    }

    let anyExpanded = false;
    let OPpics = null;
    for (var i = 0; i < opPost.childNodes.length; i++) {
        if (opPost.childNodes[i].className === "Files") {
            OPpics = opPost.childNodes[i].getElementsByTagName('img');
            break;
        }
    }
    for (var i = 0; i < OPpics.length; i++) {
        if (OPpics[i].dataset.expanded === "1") {
            anyExpanded = true;
        }
    }

    if (anyExpanded === true) {
        opText.style.clear = "both";
    } else {
        opText.style.clear = "none";
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

    for (let i = 0; i < this.files.length; i++) { 

        let src = URL.createObjectURL(this.files[i]);
        filesArray.push(this.files[i]);

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

        childspan.append(this.files[i].name);

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