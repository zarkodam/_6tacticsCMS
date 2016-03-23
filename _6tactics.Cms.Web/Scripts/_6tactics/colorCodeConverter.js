colorCodeConverter = {
    convertRgbToHex: function (rgb) {
        rgb = rgb.match(/^rgb\((\d+),\s*(\d+),\s*(\d+)\)$/);
        function hex(x) {
            return ("0" + parseInt(x).toString(16)).slice(-2);
        }
        return "#" + hex(rgb[1]) + hex(rgb[2]) + hex(rgb[3]);
    },

    convertRgbToRgba: function (rgb, opacity) {
        rgb = rgb.match(/^rgb\((\d+),\s*(\d+),\s*(\d+)\)$/);
        return 'rgba(' + rgb[1] + ',' + rgb[2] + ',' + rgb[3] + ',' + opacity + ')';
    },

    convertRgbaToRgb: function (rgba) {
        rgba = rgba.match(/^rgba\((\d+),\s*(\d+),\s*(\d+)/);
        return 'rgb(' + rgba[1] + ',' + rgba[2] + ',' + rgba[3] + ')';
    },

    changeOpacityOnRgba: function (rgba, opacity) {
        rgba = rgba.match(/^rgba\((\d+),\s*(\d+),\s*(\d+)/);
        return 'rgba(' + rgba[1] + ', ' + rgba[2] + ', ' + rgba[3] + ', ' + opacity + ')';
    }
}