// Predefined setting for all fileManager plugins

fileManagerCommonSettings = {
    isImagePreviewSupported: true,
    useThumbnailsInsteadTitles: true
}


fileManagerReadSettings = $.extend({
    urlToLoad: null,
    fileStatus: 'uploaded',
    previewAreaId: '#files-preview-wrapper',
    isEditingAllowed: true
}, fileManagerCommonSettings);


fileManagerUploadSettings = $.extend({
    fileStatus: 'readyForUpload',
    dropAreaId: '#files-preview-wrapper',
    filePathInputId: '#SelectedLocation',
    newFilenameInputId: '#file-rename-input-on-create',
    postActionPage: '/FileManager/DoPostingFilesToServer',

    aspNetMvcAntiforgeryToken: null,

    fileSizeLimitInMb: 4, // maximum allowed on server side by default (ASP.NET) 

    // Callbacks
    fileAlreadyExistCallback: null,
    fileExtensionIsNotAllowedCallback: null,
    onUploadCompleteCallback: null,
    fileSizeLimitExceededCallback: null,
    uploadingAbortCallback: null,

    supportedExtensions: [
        ".bmp", ".jpg", ".jpeg", ".png", ".gif", ".svg", ".ico", // images
        ".txt", ".rtf", ".csv", ".doc", ".docx", ".xls", ".xlsx", ".ppt", ".pptx", ".pdf", ".epub", // documents
        ".7z", ".zip", ".rar" // compressed
    ]
}, fileManagerCommonSettings);


fileManagerEditSettings = {
    newFilenameInputId: '#file-rename-input-on-edit',
    postActionPage: '/FileManager/DoRenamingFiles',

    aspNetMvcAntiforgeryToken: null,

    // Callbacks
    onFilenameChange: null,
    onEditCompleteCallback: null
};


fileManagerRemoveSettings = {
    postActionPage: '/FileManager/DoDeletingFile',
    aspNetMvcAntiforgeryToken: null,
    removeConfirmationCallback: null,
    removeDoneCallback: null
};