(function () {
    "use strict";

    let postImageForm = document.querySelector("#postImage");
    let widthField = document.querySelector("#width");
    let heightField = document.querySelector("#height");
    let statusField = document.querySelector(".uploadStatus");
    let fileField = document.querySelector("#image");

    if (!postImageForm)
        return;

    function submitImage(event) {
        event.preventDefault();
        
        if (!postImageForm.checkValidity())
            return false;

        let formData = new FormData();

        if (widthField && heightField) {
            formData.append("width", widthField.value);
            formData.append("height", heightField.value);
        }

        formData.append("image", fileField.files[0]);

        var request = new XMLHttpRequest();

        request.onloadstart = function () {
            statusField.innerText = "Putting huge megacorp to work...";
        }

        request.open("POST", postImageForm.action);
        request.send(formData);

        request.onload = function() {
            if (this.status !== 200) {
                statusField.innerText = "Ouch, something went terribly wrong. Not logged, and nobody is notified.";
                return;
            }
            statusField.innerText = null;
            document.querySelector("#imageResult").innerHTML = this.response;
        }
        
        return false;
    }

    postImageForm.onsubmit = submitImage;
})();