// Admin form fields visibility handler _6tactics plugin

; (function ($) {
    $.fn.adminFormFieldsVisibilityHandler = function (customSettings) {

        // Instance of plugin
        var plugin = this;

        // All default settings are set to empty string
        var settings = $.extend({
            selectedActionPage: "",
            elementColor: "",
            fileColor: "",
            contentItemTitle: "",
            contentType: "",
            displayType: "",
            fileUrl: "",
            fileUrlContent: "",
            summary: "",
            content: "",
            elementVisibility: "",
            sectionVisibility: "",
            linkOption: "",
            isElementTitle: "",
            sectionTitlteVisibilityVisible: ""
        }, customSettings);

        // Elements values
        var contentTypeValue = $(settings.contentType).val();
        var displayTypeValue = $(settings.displayType).val();

        // Elements
        var title = $(settings.contentItemTitle);
        var summary = $(settings.summary);
        var content = $(settings.content);
        var fileUrlInput = $(settings.fileUrl);
        var contentTypeDropdownElement = $(settings.contentType);
        var displayTypeDropdownElement = $(settings.displayType);
        var contentItemPreview = $("#content-item-preview");
        var previewForUploadedImage = $('.preview-for-uploaded-image');

        var linkOptions = $("#ContentItemViewModel_LinkOption");
        var googleMapTitlesDropDown = $("#GoogleMapsList");

        var titleAndLanguageDropdown = $(".title-and-language-dropdown");
        var googleMapTitles = $(".google-maps-titles");

        // Panel wrappers
        var titlePanelWrapper = $('#title-panel-wrapper');
        var routeTitlePanelWrapper = $('#route-title-panel-wrapper');
        var sectionVisibilityPanelWrapper = $('#section-title-visibility-panel-wrapper');
        var elementVisibilityPanelWrapper = $('#element-visibility-panel-wrapper');
        var contentTypePanelWrapper = $('#content-type-panel-wrapper');
        var linkOptionPanelWrapper = $('#link-option-panel-wrapper');
        var linkToWithOptionsPanelWrapper = $('#link-to-with-options-panel-wrapper');
        var displayTypePanelWrapper = $('#display-type-panel-wrapper');
        var elementWidthPanelWrapper = $('#element-width-panel-wrapper');
        var summaryPanelWrapper = $('#summary-panel-wrapper');
        var fileUrlElementPanelWrapper = $('#file-url-panel-wrapper');
        var contentElementPanelWrapper = $('#content-panel-wrapper');

        // Data from create form for current location 
        var titleForParentElementFromCreateForm = $("#title-for-parent-element");
        var contentTypeForParentElementFromCreateForm = $("#content-type-for-parent-element");
        var displayTypeForParentElementFromCreateForm = $("#display-type-for-parent-element");

        // Current location elements
        var currentLocationIntroMessageElement = $("#current-location-intro-message");
        var currentLocationTitle = $("#current-location-title");
        var currentLocationContentType = $("#current-location-content-type");
        var currentLocationDisplayType = $("#current-location-display-type");

        // Current location messages
        var createMessage = "Create child elements for: ";
        var editMessage = "Editing element: ";
        var detailsMessage = "Details for element: ";

        // Bools
        var isContentVisible = true;
        var isSummaryVisible = true;
        var isElementTitle = Boolean(settings.isElementTitle);
        var isFooterElementSelected = false;
        var isParentElementFooter = contentTypeForParentElementFromCreateForm.val() === 'Footer';

        // Check is panel visible
        function isDisplayTypePanelWrapperVisible(elementToCheck) {
            return elementToCheck.css('display') === 'none' ? false : true;
        }

        // Fill current location with data from create form
        function addCurrentLocationDataFromCreateForm() {
            currentLocationTitle.html(titleForParentElementFromCreateForm.val());
            currentLocationContentType.html(contentTypeForParentElementFromCreateForm.val());

            if (displayTypeForParentElementFromCreateForm.length > 0 && displayTypeForParentElementFromCreateForm.val() != "") {
                currentLocationDisplayType.html(" / " + displayTypeForParentElementFromCreateForm.val());
            } else
                currentLocationDisplayType.html("");
        }

        // Fill current location with data from edit and details form
        function addCurrentLocationDataFromEditAndDetailsForm() {
            currentLocationTitle.html(title.val());
            currentLocationContentType.html(contentTypeDropdownElement.val());

            if (displayTypeDropdownElement.length > 0 && displayTypeDropdownElement.val() != "")
                currentLocationDisplayType.html(" / " + displayTypeDropdownElement.val());
            else
                currentLocationDisplayType.html("");

        }

        // Adding panel color
        function addPanelColorForSelectedContentType(color) {
            $(".panel-heading").css("background-color", color);
            $(".btn-primary").css("background-color", color);
            $(".btn-primary").css("border", color);
            $(".panel-custom").css("border-color", color);
        }

        // If element is not title hide file url
        function addRemoveFileUrlElementIfElementIsNotTitle() {
            fileUrlElementPanelWrapper.show();

            if (isElementTitle)
                fileUrlElementPanelWrapper.hide();
        }

        // Remove current and add new bootstrap grid class (col-md-12)
        function resizeElementBySelectedContentType(element, bootstrapClassForRemove, bootstrapClassForAdd) {
            element.removeClass(bootstrapClassForRemove);
            element.addClass(bootstrapClassForAdd);
        }

        // Handle bootstrap grid class when display type panel is added
        function addDisplayTypePanel(contentTypePanelElement) {
            resizeElementBySelectedContentType(contentTypePanelElement, "col-md-12", "col-md-6");
            displayTypePanelWrapper.show();
        }

        // Handle bootstrap grid class when display type panel is removed
        function removeDisplayTypePanel(contentTypePanelElement) {
            resizeElementBySelectedContentType(contentTypePanelElement, "col-md-6", "col-md-12");
            displayTypePanelWrapper.hide();
        }

        // Handle bootstrap grid class when element width panel is added
        function addElementWidthPanel() {
            if (isElementTitle) {
                elementWidthPanelWrapper.show();
                resizeElementBySelectedContentType(contentTypePanelWrapper, "col-md-6", "col-md-5");
                resizeElementBySelectedContentType(displayTypePanelWrapper, "col-md-6", "col-md-4");
            }
        }

        // Handle bootstrap grid class when element width panel is removed
        function removeElementWidthPanel() {
            if (isElementTitle) {
                elementWidthPanelWrapper.hide();
                resizeElementBySelectedContentType(contentTypePanelWrapper, "col-md-5", "col-md-6");
                resizeElementBySelectedContentType(displayTypePanelWrapper, "col-md-4", "col-md-6");
            }
        }

        // Add content item title and removes readonly from input filed
        function addTitle(title) {
            $(settings.contentItemTitle).val(title);
            $(settings.contentItemTitle).removeAttr('readonly');
        }

        // Add content item title on readonly input filed
        function addTitleToReadOnly(title) {
            $(settings.contentItemTitle).val(title);
            $(settings.contentItemTitle).attr('readonly', 'readonly');
        }

        // Handle bootstrap grid class when section visibility panel is added
        function addSectionVisibilityPanel() {
            sectionVisibilityPanelWrapper.show();

            titlePanelWrapper.removeClass("col-md-4");
            titlePanelWrapper.addClass("col-md-6");

            elementVisibilityPanelWrapper.removeClass("col-md-4");
            elementVisibilityPanelWrapper.addClass("col-md-3");
        }

        // Handle bootstrap grid class when section visibility panel is removed
        function removeSectionVisibilityPanel() {
            sectionVisibilityPanelWrapper.hide();

            titlePanelWrapper.removeClass("col-md-6");
            titlePanelWrapper.addClass("col-md-4");

            elementVisibilityPanelWrapper.removeClass("col-md-3");
            elementVisibilityPanelWrapper.addClass("col-md-4");
        }

        function addPageListOnLinkOption() {
            linkOptions.append("<option>PageList</option>");
        }

        function removePageListFromLinkOption() {
            linkOptions.children().each(function () {
                if (this.text === "PageList") {
                    $(this).remove();
                    return false;
                }
            });
        }

        // Define form fields view by selected display type
        function displayItemsByDisplayType(displayType) {

            if (!isDisplayTypePanelWrapperVisible(displayTypePanelWrapper)) return;

            // Empty row
            if (displayType === "EmptyRow") {
                fileUrlElementPanelWrapper.hide();
                summaryPanelWrapper.hide();
                contentElementPanelWrapper.hide();
                //removeSectionVisibilityPanel();
                removeElementWidthPanel();
            }

            // Title with ElementWidthPanel visible
            if (displayType === "Bulletin" ||
                displayType === "YoutubeVideo") {
                fileUrlElementPanelWrapper.hide();
                summaryPanelWrapper.hide();
                contentElementPanelWrapper.hide();
                addElementWidthPanel();
            }

            // Title without ElementWidthPanel visible
            if (displayType === "LinkWithTitle") {
                fileUrlElementPanelWrapper.hide();
                summaryPanelWrapper.hide();
                contentElementPanelWrapper.hide();
                removeElementWidthPanel();
            }

            // Title, fileUrl visible
            if (displayType === "Image" ||
                displayType === "ImageFluid" ||
                displayType === "LinkWithImage" ||
                displayType === "LinkWithTitleAndImage" ||
                displayType === "CubeLink") {
                //fileUrlElementPanelWrapper.show();
                addRemoveFileUrlElementIfElementIsNotTitle();
                summaryPanelWrapper.hide();
                contentElementPanelWrapper.hide();
                removeElementWidthPanel();
            }

            // Title, with ElementWidthPanelfileUrl visible
            if (displayType === "VideoAsPopup") {
                addRemoveFileUrlElementIfElementIsNotTitle();
                summaryPanelWrapper.hide();
                contentElementPanelWrapper.hide();
                addElementWidthPanel();
            }

            if (displayType === "YoutubeVideo" || displayType === "VideoAsPopup") {
                removePageListFromLinkOption();
            }

            // Title, fileUrl visible
            //if (displayType == "LinkWithImage" ||
            //    displayType == "LinkWithTitleAndImage") {
            //    addRemoveFileUrlElementIfElementIsNotTitle();
            //    summaryPanelWrapper.hide();
            //    contentElementPanelWrapper.hide();
            //    removeElementWidthPanel();
            //}

            // Title, summary, fileUrl without ElementWidthPanel visible
            if (displayType === "FilesForDownload") {
                addRemoveFileUrlElementIfElementIsNotTitle();
                contentElementPanelWrapper.hide();
                summaryPanelWrapper.hide();
                removeElementWidthPanel();
                linkOptionPanelWrapper.hide();
                linkToWithOptionsPanelWrapper.hide();
                removePageListFromLinkOption();
            }

            // Title, content, fileUrl visible
            if (displayType === "ContentImageBottom" ||
            displayType === "ContentImageRight" ||
            displayType === "TitleContentImageBottom" ||
            displayType === "TitleImageRightContent" ||
            displayType === "TitleImageTopContent" ||
            displayType === "ImageLeftContent" ||
            displayType === "ImageLeftTitleContent" ||
            displayType === "ImageTopContent" ||
            displayType === "ImageTopTitleContent") {
                addRemoveFileUrlElementIfElementIsNotTitle();
                summaryPanelWrapper.hide();
                addElementWidthPanel();
            }

            // Title, content visible
            if (displayType === "Content" ||
                displayType === "TitleContent") {
                fileUrlElementPanelWrapper.hide();
                summaryPanelWrapper.hide();
                addElementWidthPanel();
            }

            // Title, summary, content visible
            if (displayType === "TitleSummaryContent") {
                fileUrlElementPanelWrapper.hide();
                addElementWidthPanel();
            }

            // Title, summary, content, fileUrl visible
            if (displayType === "TitleImageRightSummaryContent" ||
            displayType === "TitleImageRightSummaryContent" ||
            displayType === "TitleImageRightSummaryContent" ||
            displayType === "TitleImageTopSummaryContent" ||
            displayType === "TitleSummaryContentImageBottom" ||
            displayType === "ImageLeftTitleSummaryContent" ||
            displayType === "ImageTopTitleSummaryContent") {
                addRemoveFileUrlElementIfElementIsNotTitle();
                addElementWidthPanel();
            }

            // Title, summary, fileUrl visible
            if (displayType === "ImageSlider" ||
            displayType === "ImageGallery") {
                addRemoveFileUrlElementIfElementIsNotTitle();
                contentElementPanelWrapper.hide();
                addElementWidthPanel();
            }

            // All element are visible without ElementWidthPanel and with fileUrl on TitleElement!
            if (displayType === "ParallaxSliderFluid") {
                fileUrlElementPanelWrapper.show();
                fileUrlElementPanelWrapper.find("label:first").html("Background image");
                removeElementWidthPanel();
                contentElementPanelWrapper.hide();
                fixFileUrlWidth();
            }

            if (displayType === "GoogleMap" || displayType === "GoogleMapFluid" || displayType === "ContactUsMailForm") {
                fileUrlElementPanelWrapper.hide();
                addElementWidthPanel();
                summaryPanelWrapper.hide();
                contentElementPanelWrapper.hide();
                linkOptionPanelWrapper.hide();
                linkToWithOptionsPanelWrapper.hide();

                isSummaryVisible = false;
                isContentVisible = false;
            }


            if (displayType === "GoogleMapFluid") 
                removeElementWidthPanel();

            if (displayType === "GoogleMapSummary" ||
                displayType === "SummaryGoogleMap" ||
                displayType === "ContactUsMailFormSummary" ||
                displayType === "SummaryContactUsMailForm") {
                fileUrlElementPanelWrapper.hide();
                //addElementWidthPanel();
                removeElementWidthPanel();
                contentElementPanelWrapper.hide();
                linkOptionPanelWrapper.hide();
                linkToWithOptionsPanelWrapper.hide();

                isSummaryVisible = true;
                isContentVisible = false;
            }

            // Show google maps titles as dropdown
            if (displayType === "GoogleMap" ||
                displayType === "GoogleMapFluid" ||
                displayType === "GoogleMapSummary" ||
                displayType === "SummaryGoogleMap") {

                if (isElementTitle) {
                    titleAndLanguageDropdown.show();
                    googleMapTitles.hide();
                } else {
                    titleAndLanguageDropdown.hide();
                    googleMapTitles.show();
                    title.val($('#' + googleMapTitlesDropDown.attr('id') + ' option:selected').text());
                }
            }
        }

        // Defining elements view per selected content type
        function displayItemByContnetType(contentType) {
            switch (contentType) {
                case "Project":
                    addPanelColorForSelectedContentType("tomato");
                    contentElementPanelWrapper.hide();
                    removeSectionVisibilityPanel();
                    elementVisibilityPanelWrapper.hide();
                    resizeElementBySelectedContentType(titlePanelWrapper, "col-md-4", "col-md-12");
                    resizeElementBySelectedContentType(contentTypePanelWrapper, "col-md-6", "col-md-12");
                    break;
                case "Language":
                    addPanelColorForSelectedContentType("blueviolet");
                    fileUrlElementPanelWrapper.hide();
                    summaryPanelWrapper.hide();
                    contentElementPanelWrapper.hide();
                    removeSectionVisibilityPanel();
                    resizeElementBySelectedContentType(titlePanelWrapper, "col-md-4", "col-md-8");
                    resizeElementBySelectedContentType(contentTypePanelWrapper, "col-md-6", "col-md-12");
                    break;
                case "Page":
                    addPanelColorForSelectedContentType("yellowgreen");
                    fileUrlElementPanelWrapper.hide();
                    summaryPanelWrapper.hide();
                    contentElementPanelWrapper.hide();
                    removeSectionVisibilityPanel();

                    routeTitlePanelWrapper.show();
                    //resizeElementBySelectedContentType(titlePanelWrapper, "col-md-4", "col-md-8");

                    if (isFooterElementSelected) {
                        addDisplayTypePanel(contentTypePanelWrapper);
                        isFooterElementSelected = false;
                    }

                    if (settings.selectedActionPage === "Create")
                        removeElementWidthPanel();

                    //if (title.val('Footer'))
                    //    addTitle('');

                    break;
                case "Footer":
                    isFooterElementSelected = true;
                    addPanelColorForSelectedContentType("gray");
                    fileUrlElementPanelWrapper.hide();
                    summaryPanelWrapper.hide();
                    contentElementPanelWrapper.hide();
                    removeSectionVisibilityPanel();
                    removeDisplayTypePanel(contentTypePanelWrapper);
                    routeTitlePanelWrapper.hide();
                    resizeElementBySelectedContentType(titlePanelWrapper, "col-md-4", "col-md-8");

                    removeElementWidthPanel();

                    if (!isDisplayTypePanelWrapperVisible(displayTypePanelWrapper))
                        resizeElementBySelectedContentType(contentTypePanelWrapper, "col-md-6", "col-md-12");

                    addTitleToReadOnly('Footer');

                    break;
                case "ContentElement":
                    routeTitlePanelWrapper.hide();

                    if (settings.sectionTitlteVisibilityVisible === "visible")
                        addSectionVisibilityPanel();

                    addPanelColorForSelectedContentType(settings.elementColor);

                    // Removed 'cause EmptyRow dislay type is first
                    removeElementWidthPanel();
                    //addElementWidthPanel();
                    break;
                case "FileElement":
                    routeTitlePanelWrapper.hide();

                    if (settings.sectionTitlteVisibilityVisible === "visible")
                        addSectionVisibilityPanel();

                    addPanelColorForSelectedContentType(settings.fileColor);
                    summaryPanelWrapper.find("label").html("Description");
                    addElementWidthPanel();
                    fileUrlElementPanelWrapper.hide();
                    break;
                default:
                    break;
            }
        }

        // Create display type dropdown by selected content type
        function getDisplayTypeListItem(selectedContentType) {
            $.get('/Ajax/GetDisplayTypeList', { selectedContentType: selectedContentType }, function (data) {
                $(settings.displayType).html("");

                $.each(data, function (key, value) {
                    $(settings.displayType).append("<option>" + value + "</option>");
                });
            });
        }

        function linkOptionsHanlder(selectedElement) {
            var disabled = $('#LinkDisabled');
            var custom = $('#LinkCustom');
            var pageList = $('#LinkPageList');

            var linkTo = $('#ContentItemViewModel_LinkTo');

            if (selectedElement === 'Disable') {
                disabled.show();
                custom.hide();
                pageList.hide();
                linkTo.val(null);
            }
            else if (selectedElement === 'Custom') {
                disabled.hide();
                custom.show();
                pageList.hide();
                linkTo.val(custom.val());
            }
            else if (selectedElement === 'PageList') {
                disabled.hide();
                custom.hide();
                pageList.show();
                linkTo.val(pageList.val());
            }

        }

        // Constructor
        return plugin.each(function () {
            $(function () {

                //else if (settings.selectedActionPage === "Details") {
                //    summary = $('#ContentItemViewModel_Summary');
                //    var content = $('#ContentItemViewModel_Content');

                //    if (summary.length > 0) {
                //        summary.ckeditor(function () { /*callback*/ }, { customConfig: '/Scripts/ckeditor/customConfig/basic.js' });
                //        summary.ckeditorGet().config.readOnly = true;
                //    }

                //    if (content.length > 0) {
                //        content.ckeditor(function () { /*callback*/ }, { customConfig: '/Scripts/ckeditor/customConfig/full.js' });
                //        content.ckeditorGet().config.readOnly = true;
                //    }
                //}

                if (settings.selectedActionPage === "Edit" || settings.selectedActionPage === "Setup" || settings.selectedActionPage === "Details") {
                    $('.image-link').magnificPopup({ type: 'image' });
                }

                //Show data for current location
                if (settings.selectedActionPage === "Create") {
                    currentLocationIntroMessageElement.html(createMessage);
                    addCurrentLocationDataFromCreateForm();
                }
                else if (settings.selectedActionPage === "Edit") {
                    currentLocationIntroMessageElement.html(editMessage);
                    addCurrentLocationDataFromEditAndDetailsForm();
                }
                else if (settings.selectedActionPage === "Details") {
                    currentLocationIntroMessageElement.html(detailsMessage);
                    addCurrentLocationDataFromEditAndDetailsForm();
                }

                // Show items by selected content type
                displayItemByContnetType(contentTypeValue);

                // Show items by selected display type
                displayItemsByDisplayType(displayTypeValue);

                if (settings.selectedActionPage === "Create" || settings.selectedActionPage === "Edit") {
                    // Fill display type dropdown by changing content type
                    contentTypeDropdownElement.on('change', function () {
                        getDisplayTypeListItem(this.value);
                        displayItemByContnetType(this.value);
                    });

                    // Remove display types from display type list if using Footer content type
                    if (isParentElementFooter)
                        displayTypeDropdownElement.find('option').each(function () {
                            if (this.text === 'ParallaxSliderFluid')
                                this.remove();
                        });

                    // Turn on or off element width by selected display type
                    displayTypeDropdownElement.on('change', function () {
                        displayItemsByDisplayType(this.value);
                    });

                    googleMapTitlesDropDown.on('change', function () {
                        title.val(this.value);
                    });
                }
            });


            if (settings.selectedActionPage === "Create" || settings.selectedActionPage === "Edit") {
                linkOptionsHanlder($(settings.linkOption + ' option:selected').text());

                $('#LinkCustom').on("keyup", function () {
                    linkOptionsHanlder('Custom');
                });

                $('#LinkPageList').on("change", function () {
                    linkOptionsHanlder('PageList');
                });

                $(settings.linkOption).on("change", function () {
                    linkOptionsHanlder(this.value);
                });

                // Create RouteTitle from Title
                $("#ContentItemViewModel_Title").on("click change keyup", function () {
                    $("#ContentItemViewModel_RouteTitle").val(urlDiacriticsFixer($(this).val().toLowerCase()).replace(/[_\s]/g, '-'));
                });

                $('#content-item-preview-wrapper').scrollbar({
                    // CKEDitor init after scroll was loaded
                    onUpdateComplete: function () {
                        //console.log("onUpdateComplete");
                        if (isSummaryVisible && $('#ContentItemViewModel_Summary').length > 0)
                            $('#ContentItemViewModel_Summary').ckeditor(function () {
                                //console.log('Summary on');
                            }, { /*customConfig: '/Scripts/ckeditor/customConfig/basic.js'*/ });

                        if (isContentVisible && $('#ContentItemViewModel_Content').length > 0)
                            $('#ContentItemViewModel_Content').ckeditor(function () {
                                //console.log('Content on');
                            }, { /*customConfig: '/Scripts/ckeditor/customConfig/full.js'*/ });
                    }
                });

                fixFileUrlWidth();
                treeViewSelectedItemHandler.contentItemTreeViewOnCreateAndEdit();
            }

        });

    };

})(jQuery);
