function googleMapWizardInit(encodedMapJsonString, encodedPreviewSetupJsonString, mapName) {
    // Fixed json string from model
    var mapData = encodedMapJsonString;
    var mapDataParsed = JSON.parse(mapData.replace(/"/g, '"'));

    // Fixed json string from model
    var mapSettings = encodedPreviewSetupJsonString;
    var mapSettingsParsed = JSON.parse(mapSettings.replace(/"/g, '"'));

    //mapSettingsParsed.mapResizeEventActivatorId = '#wizard-t-3, #wizard-t-2, #wizard-t-1, #wizard-t-0';
    mapSettingsParsed.zoom = mapDataParsed.zoom;
    mapSettingsParsed.savedLocations = mapDataParsed.locations;
    mapSettingsParsed.mapStyles = mapSettingsParsed.isGoogleMapStyleEnabled ? googleMapsWizardCustomStyles.customStyle : {};

    // Generate id to avoid id repeating
    var generatedId = '#' + $.fixDuplicatedIdAndReturnFixedId('.google-map', mapName);

    // Add full height on map's wrapper
    if (mapName.toLowerCase().indexOf('fullpage') >= 0) {
        $(generatedId).addClass('google-map-fluid');
        $(generatedId).layoutFixxxer({ elementsAroundTheBody: ['.navbar', '.selected-page-panel', 'footer'] });
    }

    // Map init
    $(generatedId).googleMapsWizard(mapSettingsParsed);
}