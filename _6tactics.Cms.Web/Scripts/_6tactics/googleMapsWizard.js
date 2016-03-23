// _6tactics GoogleMaps Wizard (GoogleMaps required)  _6tactics plugin

; (function ($) {
	$.fn.googleMapsWizard = function (options, callback) {

		// Instance
		var plugin = this;


		// Default settings
		var settings = $.extend({
			// Common settings
			action: null, // searchAction, zoomAndCenterAction, previewAction
			mapResizeEventActivatorId: null, // when id is clicked map position is set to center
			defaultAddress: "europe", // continent country town street street number
			zoom: 2, // default zoom
			scrollwheel: true, // enable or disable scroll wheel

			markerVisibility: true, // is marker visible
			markerImageUrl: null, // marker image url

			infowindowVisibility: null, // is info window visible
			infowindowMaxWidth: "330px", // info window max width
			infowindowMaxHeight: "auto", // info window max height
			infowindowImageUrl: null, // info windows image url

			// searchAction
			formattedAddressId: null, // id for input field where the searched(formatted) address will be shown 
			addressLatitudeId: null, // id for input field where the searched latitude will be shown 
			addressLongitudeId: null, // id for input field where the searched latitude will be shown 
			addresszIndexId: "0", // id for input field where the zIndex will be set 
			searchSubmitElementId: null, // id for search input field 

			addLocationFormCallback: null, // function(){}

			// zoomAndCenterAction
			mapZoomId: null, // id for input field where the current zoom will be shown 
			mapLatitudeId: null, // input id where the current latitude will be shown
			mapLongitudeId: null, // input id where the current longitude will be shown
			savedLocations: null, // get saved locations from json (look at the savedLocationsExample.json file)

			// previewAction
			automaticallyCenterLocations: true, // sets added locations to center of the map (avoids mapLatitudeId and mapLongitudeId)

			// customMapStyles
			mapStyles: {},

			// onMapReizeCallback
			onResizeCallback: null,
			timeOutInMsOnResize: 20

		}, options);

		// Globals
		var map = null;

		var infowindow = null;
		var infowindows = [];

		var marker = null;
		var markers = [];

		var geocoderPosition = null;
		var searchedPosition = null;
		var searchedPlace = null;

		var locations = [];
		var locationsLatLngs = [];

		var placedMapZoom = settings.zoom;
		var placedLatLng = null;

		// UTILITIES

		// Check if element is valid
		var isValid = function (data) {
			if (typeof data != "undefined" && data != null)
				return true;
			return false;
		};

		// Check if marker is enabled
		var isMarkerOn = function () {
			if (isValid(settings.markerVisibility) && settings.markerVisibility) {
				//alert("marker is on fnc")
				return true;
			}
			return false;
		};

		// Check if infowindow is enabled
		var isInfowindowOn = function () {
			if (isValid(settings.infowindowVisibility) && settings.infowindowVisibility)
				return true;
			return false;
		};

		// Check if marker is enabled
		var isSearchSubmitElementIdSet = function () {
			if (isValid(settings.searchSubmitElementId))
				return true;
			return false;
		};

		// Creating marker
		var addMarker = function (markerObject, mapObject, position, formattedAddress, zindex) {
			markerObject.setPosition(position);
			markerObject.setTitle(formattedAddress.toString());

			var icon = {
				path: "M0-165c-27.618 0-50 21.966-50 49.054C-50-88.849 0 0 0 0s50-88.849 50-115.946C50-143.034 27.605-165 0-165z",
				//fillColor: '#d9534f',
				//fillColor: '#428bca',
				fillColor: '#5EBEEF',
				fillOpacity: 1,
				strokeColor: "white",
				strokeWeight: 2,
				scale: 1 / 4
			};

			if (isValid(settings.markerImageUrl))
				markerObject.setIcon(settings.markerImageUrl);
			else
				markerObject.setIcon(icon);

			markerObject.setZIndex(zindex);
			markerObject.setMap(mapObject);
			markerObject.setVisible(true);
		};

		// Creating infoWindow
		var addInfowindow = function (infowindowObject, mapObject, position, formattedAddress, markerObject) {
			var imageElement = isValid(settings.infowindowImageUrl) ? "<img src=\"" + settings.infowindowImageUrl + "\">" : "";

			var infoWindowContent = "<div class=\"info-window\" style=\"text-align: center; max-width: " + settings.infowindowMaxWidth + "; height: "
				+ settings.infowindowMaxHeight + "\">" + imageElement + "<b>" + formattedAddress + "</b></div>";

			infowindowObject.setContent(infoWindowContent);

			if (isValid(markerObject))
				infowindowObject.open(mapObject, markerObject);
			else {
				infowindowObject.setPosition(position);
				infowindowObject.open(mapObject);
			}
		};


		// Adding location to form
		var addLocationToForm = function (formattedAddress, latitude, longitude, addLocationFormCallback) {
			if (isValid(settings.formattedAddressId))
				$(settings.formattedAddressId).val(formattedAddress);

			if (isValid(settings.addressLatitudeId))
				$(settings.addressLatitudeId).val(latitude);

			if (isValid(settings.addressLongitudeId))
				$(settings.addressLongitudeId).val(longitude);

			if (typeof addLocationFormCallback == "function") {
				addLocationFormCallback();
			}
		};

		// Is infowindow opened
		var isInfowindowOpened = function (infowindowObject) {
			return isValid(infowindowObject.getMap());
		};

		var addEventOnMarkerToOpenInfoWindow = function (marker, infowindow) {
			// Open info window on click on marker
			window.google.maps.event.addListener(marker, "click", function () {
				infowindow.open(map, marker);
			});
		}

		var addSavedLocationsToPlacedLatLng = function () {
			for (var i = 0; i < locations.length; i++) {

				// Adding instances of Latitudes and Longitutes in global array
				locationsLatLngs[i] = new window.google.maps.LatLng(locations[i].latitude, locations[i].longitude);

				// Adding instances of infowindows in global array
				infowindows[i] = new window.google.maps.InfoWindow();

				// Adding instances of markers in global array
				markers[i] = new window.google.maps.Marker({ anchorPoint: new window.google.maps.Point(0, -25) });

				// Show marker if is enabled
				if (isMarkerOn()) {
					addMarker(markers[i], map, locationsLatLngs[i], locations[i].formattedAddress, locations[i].zIndex);

					// Show infowindow with marker if is enabled
					if (isInfowindowOn()) {
						addInfowindow(infowindows[i], map, locationsLatLngs[i], locations[i].formattedAddress, markers[i]);

						// Marker event for opening infoWindow
						addEventOnMarkerToOpenInfoWindow(markers[i], infowindows[i]);
					}
				}

				// Show infoWindow if is enabled
				if (isInfowindowOn() && !isMarkerOn())
					addInfowindow(infowindows[i], map, locationsLatLngs[i], locations[i].formattedAddress);
			}

			// Set lat and lng for all locations
			placedLatLng = new window.google.maps.LatLngBounds();
			for (var j = 0; j < locationsLatLngs.length; j++)
				placedLatLng.extend(locationsLatLngs[j]);

			map.setCenter(placedLatLng.getCenter());
		};


		// PLUGIN ROLES

		var searchAction = function () {

			// If infowindow is enabled initialize it
			if (isInfowindowOn())
				infowindow = new window.google.maps.InfoWindow();

			// If marker is enabled initialize it
			if (isMarkerOn())
				marker = new window.google.maps.Marker({ anchorPoint: new window.google.maps.Point(0, -25) });

			// Show search input
			$(settings.inputId).attr("style", "display: block!important");

			// Searched value
			var input = $(settings.inputId)[0];

			map.controls[window.google.maps.ControlPosition.TOP_LEFT].push(input);

			var autocomplete = new window.google.maps.places.Autocomplete(input);
			autocomplete.bindTo("bounds", map);

			// Search event
			window.google.maps.event.addListener(autocomplete, "place_changed", function () {
				// Hide marker if isInfoWindow is enabled
				if (isInfowindowOn())
					infowindow.close();

				// Hide marker if marker is enabled
				if (isMarkerOn())
					marker.setVisible(false);

				// Get searched place
				searchedPlace = autocomplete.getPlace();
				if (!searchedPlace.geometry) return;

				searchedPosition = searchedPlace.geometry.location;

				// Set coordinates to hidden fields after search
				addLocationToForm(searchedPlace.formatted_address, searchedPosition.lat(), searchedPosition.lng(), settings.addLocationFormCallback);

				// Set searched map
				map.setCenter(searchedPosition);

				// TODO: Set through settings (defaul will be 17)
				// Set zoom after search
				map.setZoom(17);

				// Show marker if marker is enabled
				if (isMarkerOn()) {
					addMarker(marker, map, searchedPosition, searchedPlace.formatted_address, 0);

					// Show infoWindow with marker if is enabled
					if (isInfowindowOn()) {
						addInfowindow(infowindow, map, searchedPosition, searchedPlace.formatted_address, marker);

						// Marker event for opening infoWindow
						addEventOnMarkerToOpenInfoWindow(marker, infowindow);
					}
				}

				// Show infoWindow if is enabled
				if (isInfowindowOn() && !isMarkerOn())
					addInfowindow(infowindow, map, searchedPosition, searchedPlace.formatted_address);

			});

			// Add location button
			var onLocationAddButton = null;
			if (isSearchSubmitElementIdSet())
				onLocationAddButton = $(settings.searchSubmitElementId)[0];

			// Event for AddingNewLocation button
			if (onLocationAddButton != null)
				window.google.maps.event.addDomListener(onLocationAddButton, "click", function () {
					if (isInfowindowOn())
						infowindow.close();
					if (isMarkerOn())
						marker.setVisible(false);

					$(settings.inputElementId).val("");
				});
		};

		var zoomAndCenterAction = function () {

			// Get locations from db through settings
			locations = settings.savedLocations;

			if (isValid(locations)) {
				// Continue if locations exists
				if (locations.length < 1) return;

				addSavedLocationsToPlacedLatLng();

				// Latitude and longitude coordinate values
				var setLatLng = function (lat, lng) {
					placedLatLng = new window.google.maps.LatLngBounds();
					placedLatLng.extend(new window.google.maps.LatLng(lat, lng));
				};

				var latLngRefrash = function (mapObject) {
					setLatLng(mapObject.getCenter().lat().toFixed(13), mapObject.getCenter().lng().toFixed(13));
				};

				// Latitude and longitude inputs
				var mapCenterLatitudeInput = null;
				var mapCenterLongitudeInput = null;

				var setValuesToLatLngInputs = function (lat, lng) {
					mapCenterLatitudeInput.val(lat);
					mapCenterLongitudeInput.val(lng);
				};

				var latLngInputsRefrash = function (mapObject) {
					setValuesToLatLngInputs(mapObject.getCenter().lat().toFixed(13), mapObject.getCenter().lng().toFixed(13));
				};

				var isMapLatLngIdValid = function () {
					return isValid(settings.mapLatitudeId) && isValid(settings.mapLongitudeId);
				}


				// Check and set lat lng input fields for dragging and zooming
				if (isMapLatLngIdValid()) {
					mapCenterLatitudeInput = $(settings.mapLatitudeId);
					mapCenterLongitudeInput = $(settings.mapLongitudeId);
				}

				// Set custom defined latitudes and longitudes for locations
				if (!settings.automaticallyCenterLocations && isMapLatLngIdValid()) {
					// Refrash mapLatitudeId and mapLongitudeId input fieds
					window.google.maps.event.addListener(map, "drag", function () {
						latLngRefrash(map);
						latLngInputsRefrash(map);
					});

					if ((mapCenterLatitudeInput != null && mapCenterLongitudeInput != null) && (mapCenterLatitudeInput.val().length > 3 && mapCenterLongitudeInput.val().length > 3))
						setLatLng($(settings.mapLatitudeId).val(), $(settings.mapLongitudeId).val());
					else
						setValuesToLatLngInputs(placedLatLng.getCenter().lat(), placedLatLng.getCenter().lng());
				}

				//Check if zoom element id exist
				if (isValid(settings.mapZoomId)) {
					var mapZoom = $(settings.mapZoomId);

					// Check if...
					if (mapZoom.val() === 0)
						mapZoom.val(map.getZoom());
					else {
						var valueFromZoomInput = parseInt(mapZoom.val());

						if (!isNaN(valueFromZoomInput))
							placedMapZoom = valueFromZoomInput;
					}

					map.setZoom(placedMapZoom);

					window.google.maps.event.addListener(map, "zoom_changed", function () {
						mapZoom.val(map.getZoom());
						placedMapZoom = parseInt(mapZoom.val());

						// Automatically zoom fixing 
						if (settings.automaticallyCenterLocations)
							map.setCenter(placedLatLng.getCenter());

						if (isMapLatLngIdValid())
							latLngInputsRefrash(map);

						latLngRefrash(map);
					});
				}

			}
		};

		var previewAction = function () {
			// Get locations from db through settings
			locations = settings.savedLocations;
			if (isValid(locations)) {
				// Continue if locations exists
				if (locations.length < 1) return;

				addSavedLocationsToPlacedLatLng();
				placedMapZoom = settings.zoom;
			}

		};

		// Map setup (...when to use one of roles)
		var mapSetup = function () {
			// Run selected action (serch, multiple locations preview, set zoom and center)
			var setProperAction = function () {
				if (settings.action === "search")
					searchAction();
				else if (settings.action === "zoomAndCenter")
					zoomAndCenterAction();
				else
					previewAction();
			};

			// Geocoder initialization
			var geocoder = new window.google.maps.Geocoder();

			// Getting coordinations by geocoder from address name
			geocoder.geocode({ "address": settings.defaultAddress }, function (results, status) {
				if (status === window.google.maps.GeocoderStatus.OK) {
					geocoderPosition = results[0].geometry.location;

					// Setting properties for map
					var location = new window.google.maps.LatLng(results[0].geometry.location.lat(), results[0].geometry.location.lng());
					var mapProp = { center: location, zoom: settings.zoom, mapTypeId: window.google.maps.MapTypeId.ROADMAP, scrollwheel: settings.scrollwheel, styles: settings.mapStyles };

					// Initialize map with properties
					map = new window.google.maps.Map($(plugin)[0], mapProp);

					setProperAction();
				} else
					alert("Geocode was not successful for the following reason: " + status);
			});
		};

		var mapLocationsOnEventRefrash = function () {
			window.google.maps.event.trigger(map, "resize");

			if (searchedPosition != null) {
				//console.log("searchedPosition: " + searchedPosition);
				if (isInfowindowOn() && isMarkerOn() && isInfowindowOpened(infowindow))
					addInfowindow(infowindow, map, searchedPosition, searchedPlace.formatted_address, marker);
				map.setCenter(searchedPosition);
			}
			else if (placedLatLng != null) {
				//console.log("placedLatLng: " + placedLatLng);
				map.setCenter(placedLatLng.getCenter());
				map.setZoom(placedMapZoom);

				if (isInfowindowOn() && !isMarkerOn()) {
					for (var i = 0; i < locations.length; i++)
						addInfowindow(infowindows[i], map, locationsLatLngs[i], locations[i].formattedAddress);
				}

				if (isInfowindowOn() && isMarkerOn()) {
					for (var j = 0; j < locations.length; j++)
						addInfowindow(infowindows[j], map, locationsLatLngs[j], locations[j].formattedAddress, markers[j]);
				}

			} else {
				//console.log("geocoderPosition: " + geocoderPosition);
				map.setCenter(geocoderPosition);
			}

		}

		// Constructor
		return plugin.each(function () {
			mapSetup();

			$(settings.mapResizeEventActivatorId).on("click", function () {
				mapLocationsOnEventRefrash();
			});

			window.google.maps.event.addDomListener(window, 'resize', function () {
				setTimeout(function() {
					mapLocationsOnEventRefrash();
				}, settings.timeOutInMsOnResize);
			});

			if (typeof settings.callback == "function") {
				settings.callback();
			}
		});
	};

})(jQuery);
