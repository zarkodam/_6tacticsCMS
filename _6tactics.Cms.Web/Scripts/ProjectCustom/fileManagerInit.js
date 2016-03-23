function openPoup(id, beforeOpenAction, afterCloseAction) {
    $.magnificPopup.open({
        items: {
            src: $(id),
            type: 'inline'
        },
        callbacks: {
            beforeOpen: function () {
                if (typeof beforeOpenAction == 'function')
                    beforeOpenAction();
            },
            afterClose: function() {
                if (typeof afterCloseAction == 'function')
                    afterCloseAction();
            }
        }
    });
}

// fileManagerUpload popups
function fileAlreadyExistPopup() {
    openPoup('#file-already-exist');
}

function fileExtensionIsNotAllowedPopup() {
    openPoup('#file-extension-is-not-allowed');
}


function onUploadCompletePopup() {
    openPoup('#upload-complete');
}

function fileSizeLimitExceededPopup() {
    openPoup('#file-limit-exceeded');
}

function uploadingAbortPopup() {
    openPoup('#upload-aborted');
}


// fileManagerEdit popups
function renamePopup() {
    openPoup('#rename-file-on-edit-popup');
}

function onEditComplete() {
    openPoup('#edit-applied');
}

var removePopupElementCache = $('#remove-confirmation').html();

// fileManagerRemove popups
function removeConfirmation() {
    var deferred = $.Deferred();

    var popupId = '#remove-confirmation';
    var popupContentClass = '.popup-content';

    openPoup(popupId, function() {
        $(popupId).html(removePopupElementCache);
    }, function() {
        $(popupId).removeClass('popup-success').removeClass('popup-warning').addClass('popup-error');
    });

    $('#remove-confirm').on('click', function () {
        $(popupId).find(popupContentClass).html('File is successfully removed!');
        $(popupId).removeClass('popup-error').addClass('popup-success');
        deferred.resolve();
    });

    $('#remove-cancel').on('click', function () {
        $(popupId).find(popupContentClass).html('File removement canceled!');
        $(popupId).removeClass('popup-error').addClass('popup-warning');
        deferred.reject();
    });

    return deferred.promise();
}

//function loadingStart() {
//    $('.gears-wrapper').show();
//}

//function loadingStop() {
//    $('.gears-wrapper').hide();
//}

function fileManagerReadAndEditInit(webFolderPath) {
    // FileManagerRead
    fileManagerReadSettings.urlToLoad = '/filemanager/getfilesfromserver?webfilepath=' + webFolderPath;
    fileManagerReadSettings.fileStatus = 'uploaded';

    loadingStart();
    $(document.body).fileManagerRead(fileManagerReadSettings, loadingStop);
}

// Select folder on treeview element click 
$(document.body).on('click', '.folder-tree-node', function (e) {
    e.stopPropagation();

    var webFolderPath = $(this).find('.folder-path').html();
    $('#current-location-path').html(webFolderPath);
    $('#SelectedLocation').val(webFolderPath);

    // Load files on selected folder
    fileManagerReadAndEditInit(webFolderPath);
});

function fileManagerUploadInit(antiForgeryToken) {
    // FileManagerUpload
    fileManagerUploadSettings.fileStatus = 'readyForUpload';
    fileManagerUploadSettings.aspNetMvcAntiforgeryToken = antiForgeryToken;
    fileManagerUploadSettings.fileAlreadyExistCallback = fileAlreadyExistPopup;
    fileManagerUploadSettings.fileExtensionIsNotAllowedCallback = fileExtensionIsNotAllowedPopup;
    fileManagerUploadSettings.onUploadCompleteCallback = onUploadCompletePopup;
    fileManagerUploadSettings.fileSizeLimitExceededCallback = fileSizeLimitExceededPopup;
    fileManagerUploadSettings.uploadingAbortCallback = uploadingAbortPopup;
    $("#upload-edit-files").fileManagerUpload(fileManagerUploadSettings);
}

function fileManagerEditInit(antiForgeryToken) {
    // FileManagerEdit
    fileManagerEditSettings.aspNetMvcAntiforgeryToken = antiForgeryToken;
    fileManagerEditSettings.onFilenameChange = renamePopup;
    fileManagerEditSettings.onEditCompleteCallback = onEditComplete;
    $(document.body).fileManagerEdit(fileManagerEditSettings, '#upload-edit-files');
}

function fileManagerRemoveInit(antiForgeryToken) {
    // FileManagerRemove
    fileManagerRemoveSettings.aspNetMvcAntiforgeryToken = antiForgeryToken;
    fileManagerRemoveSettings.removeConfirmationCallback = removeConfirmation;
    $(document.body).fileManagerRemove(fileManagerRemoveSettings);
}

function treeViewActionsPopupsInit(callback) {
    // POPUPs
    $('.create-folder-popup').fileManagerPopupHandler({
        actionType: 'create',
        popupId: '#create-folder-popup',
        classToRemove: 'popup-success',
        classToAdd: 'popup-info',
        elementIdPrefix: 'create--',
        urlToLoad: '/filemanager/createfolder?wheretocreate=',
        isQueryOnLinkEnabled: true,
        onLoadStartCallback: loadingStart,
        onLoadCompleteCallback: loadingStop
    });

    $('.delete-folder-popup').fileManagerPopupHandler({
        actionType: 'delete',
        popupId: '#delete-folder-popup',
        classToRemove: 'popup-success',
        classToAdd: 'popup-warning',
        elementIdPrefix: 'remove--',
        urlToLoad: '/filemanager/deletefolder?folderpath=',
        isQueryOnLinkEnabled: true,
        onLoadStartCallback: loadingStart,
        onLoadCompleteCallback: loadingStop
    }, function () {
        //fileManagerReadAndEditInit("/uploads");
    });
}

// Initializtion
function fileManagerIndexInit(antiForgeryToken) {
    // Load files for default location
    fileManagerReadAndEditInit('/uploads');

    fileManagerUploadInit(antiForgeryToken);

    fileManagerEditInit(antiForgeryToken);

    fileManagerRemoveInit(antiForgeryToken);
}

function fileManagerFolderTreeViewInit(callback) {
    treeViewActionsPopupsInit(callback);
}
