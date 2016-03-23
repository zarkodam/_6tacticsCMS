function rgb2hex(rgb) {
    if (typeof rgb === 'undefined')
        return '#3851BC';
    else {
        rgb = rgb.match(/^rgb\((\d+),\s*(\d+),\s*(\d+)\)$/);
        function hex(x) {
            return ("0" + parseInt(x).toString(16)).slice(-2);
        }
        return "#" + hex(rgb[1]) + hex(rgb[2]) + hex(rgb[3]);
    }
}

googleMapsWizardCustomStyles = {
    customStyle: [
      {
          stylers: [
            { hue: rgb2hex($('.web-current-location-wrapper').css('background-color')) },
            { saturation: -20 }
          ]
      }, {
          featureType: "road",
          elementType: "geometry",
          stylers: [
            { lightness: -10 }
            //{ visibility: "simplified" }
          ]
      }, {
          featureType: "road",
          elementType: "labels"
          //stylers: [
          //  { visibility: "off" }
          //]
      }
    ]
}

// https://developers.google.com/maps/documentation/javascript/styling?hl=en