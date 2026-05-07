
var uploadManagement = (function () {
    var selectedFiles = [];


    function openModelUpload(e) {
        var currentFiles = getCurrentFiles();

        if (!window.isInvalidCharacter && currentFiles < maxFileInputsCount) {
            $("#" + asynUpload).find("input:file").eq(0).click();
        }
    }

    function onFileUploaded(sender, args) {
        AllowModalAction(true);
    }

    function onFileUploadFailed(sender, args) {
        AllowModalAction(true);
    }

    function submitUploadFile(sender, args) {
        isUpload = false;
        var info = args.get_fileInfo();
        if (info.ContentLength > 0) {
            var dataUpload = $("#list-white [data-upload='" + info.FileName.toLowerCase() + "']");
            if (dataUpload.length > 0) {
                $(".uxProgress").hide();
                dataUpload.each(function (item) {
                    $($(this).parent().find(".progress-bar")[0]).css("width", "100%");
                    var seft = this;
                    setTimeout(function () {
                        $($(seft).parent().find(".progress-bar")[0]).hide();
                        $(seft).attr("data-upload", "");
                        var item = $("#list-white [data-upload]:not([data-upload=''])");
                        if (item === null || item.length === 0) {
                            document.getElementById(submitUpload).click();
                            sender.set_enabled(true);
                        }
                    }, 500);
                    $($(this).parent().find(".uploading")[0]).addClass("hidden");
                    $($(this).parent().find(".fileSize")[0]).removeClass("hidden");
                });
            }
            else {
                setTimeout(function () { uploadManagement.submitUploadFile(sender, args) }, 500);
            }
        }
    }

    function onProgressUpdating(sender, args) {
        var data = args.get_data();
        $("#list-white [data-upload='" + data.fileName.toLowerCase() + "']").each(function (item) {
            $($(this).parent().find(".progress-bar")[0]).css("width", data.percent + "%");
        });
    }

    function onFileUploading(sender, args) {
        $(".uxProgress").show();
        if (nOofFiles > 0 && !window.isInvalid) {
            nOofFiles -= 1;
            currentFilesTemp += 1;
        } else {
            args.set_cancel(true);
            AllowModalAction(true);
            return;
        }
    }

    function onFileSelected(sender, args) {
        // Not allow to upload when reaching to the maximum file count
        var currentFiles = getCurrentFiles();
        if (currentFiles >= maxFileInputsCount) {
            EnableControl(false);
            return;
        }

        var file = sender._fileInput.files;
        if (!isUpload && file.length > 0) {
            isUpload = true;
            $(".uxProgress").show();
            selectedFiles = [];
            for (var i = 0; i < file.length; i++) {
                if (maxFileInputsCount > selectedFiles.length + currentFiles) {
                    selectedFiles.push({
                        FileName: file[i].name,
                        ContentLength: file[i].size,
                        DocumentTypeValue: file[i].type
                    });
                }
            }

            if (currentFiles + selectedFiles.length >= maxFileInputsCount) {
                EnableControl(false);
            }

            if (selectedFiles.length > 0) {
                $get(ux_upload_uxhiddenFields).value = JSON.stringify(selectedFiles);
                document.getElementById(btnSubmit_Upload).click();
            }
        }
    }

    window.isInvalid = false;

    function OnClientFileDropped(sender, args) {

        var isValid = true;
        // Not allow to upload when reaching to the maximum file count
        var currentFiles = getCurrentFiles();
        if (currentFiles >= maxFileInputsCount || window.isInvalidCharacter) {
            isValid = false;
        }

        window.isInvalid = false;
        if (!isValid) {
            window.isInvalid = true;
            EnableControl(false);
            return;
        }

        var original = args.get_originalDropEvent();
        if (!isUpload && original !== null && original.originalEvent.dataTransfer.files.length > 0) {
            isUpload = true;
            $(".uxProgress").show();
            selectedFiles = [];
            var file = original.originalEvent.dataTransfer.files;
            for (var i = 0; i < file.length; i++) {
                if (maxFileInputsCount > selectedFiles.length + currentFiles) {
                    selectedFiles.push({
                        FileName: file[i].name,
                        ContentLength: file[i].size,
                        DocumentTypeValue: file[i].type
                    });
                }
            }

            if (currentFiles + selectedFiles.length >= maxFileInputsCount) {
                EnableControl(false);
            }
            if (selectedFiles.length > 0) {
                $get(ux_upload_uxhiddenFields).value = JSON.stringify(selectedFiles);
                document.getElementById(btnSubmit_Upload).click();
            }
        }
    }

    function OnClientValidationFailed(sender, args) {
        nOofFiles -= 1;
        currentFilesTemp += 1;
        isUpload = false;
        AllowModalAction(true);
    }

    function clearAllAttachFile() {
        document.getElementById(ux_upload_btnClearAllUpload).click();
    }

    function getCurrentFiles() {
        return $('.list-white .file-info').length;
    }

    function EnableUploadControl() {
        if (maxFileInputsCount > currentFilesTemp) {
            EnableControl(true);
        } else {
            EnableControl(false);
        }
    }

    function EnableControl(enable) {
        $find(asynUpload).set_enabled(enable);
        if (enable) {
            $('.drap-drop-file').removeClass('disable-session-max-file');
        } else {
            $('.drap-drop-file').addClass('disable-session-max-file');
        }
    }

    function AllowModalAction(enable) {
        if (enable) {
            EnableUploadControl();
        }
        else {
            EnableControl(false);
        }
        $(".uxProgress").hide();
    }

    return {
        openModelUpload: openModelUpload,
        submitUploadFile: submitUploadFile,
        onFileSelected: onFileSelected,
        onFileUploading: onFileUploading,
        OnClientFileDropped: OnClientFileDropped,
        OnClientValidationFailed: OnClientValidationFailed,
        clearAllAttachFile: clearAllAttachFile,
        onFileUploaded: onFileUploaded,
        onFileUploadFailed: onFileUploadFailed,
        onProgressUpdating: onProgressUpdating,
        EnableControl: EnableControl
    };
})();

function deleteFileClick(btnDelete) {
    $get(btnDelete).click();
    nOofFiles += 1;
    currentFilesTemp -= 1;
    uploadManagement.EnableControl(true);
}

var nOofFiles = 0;
var maxFileInputsCount = 0;
var currentFilesTemp = 0;
var isUpload = false;

$(document).ready(function () {
    nOofFiles = parseInt(maxFiles);
    maxFileInputsCount = parseInt(maxFiles);
})