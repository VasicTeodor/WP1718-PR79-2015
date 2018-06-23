var myLat;
var myLng;
var markers = [];

function initMap() {
    var map = new google.maps.Map(document.getElementById('map'), {
        zoom: 14,
        center: { lat: 45.2671, lng: 19.8335 }
    });

    map.addListener('click', function (e) {
        placeMarkerAndPanTo(e.latLng, map);
        geocodeLatLng();
    });

    var map2 = new google.maps.Map(document.getElementById('mapDestination'), {
        zoom: 14,
        center: { lat: 45.2671, lng: 19.8335 }
    });

    map2.addListener('click', function (e) {
        placeMarkerAndPanTo(e.latLng, map2);
        geocodeLatLngDestination();
    });

    var map3 = new google.maps.Map(document.getElementById('mapDriver'), {
        zoom: 14,
        center: { lat: 45.2671, lng: 19.8335 }
    });

    map3.addListener('click', function (e) {
        placeMarkerAndPanTo(e.latLng, map3);
        geocodeLatLngDriver();
    });
}

function geocodeLatLng() {
    $.ajax({
        type: 'GET',
        url: 'https://nominatim.openstreetmap.org/reverse',
        data: {
            format: 'jsonv2',
            lat: myLat,
            lon: myLng
        },
        dataType: 'json',
        success: function (data) {
            let adresa = data.address.road + ' ' + data.address.house_number + ', ' + data.address.city + ' ' + data.address.postcode;
            $('#driveAddress').attr('readonly', true);
            $('#driveAddressX').attr('readonly', true);
            $('#driveAddressY').attr('readonly', true);
            $('#driveAddress').val(adresa);
            $('#driveAddressX').val(data.lat);
            $('#driveAddressY').val(data.lon);

        },
        error: function () {
            alert("Error while getting address.");
        }
    });
}

function geocodeLatLngDestination() {
    $.ajax({
        type: 'GET',
        url: 'https://nominatim.openstreetmap.org/reverse',
        data: {
            format: 'jsonv2',
            lat: myLat,
            lon: myLng
        },
        dataType: 'json',
        success: function (data) {
            let adresa = data.address.road + ' ' + data.address.house_number + ', ' + data.address.city + ' ' + data.address.postcode;
            $('#driveDestinationAddress').attr('readonly', true);
            $('#driveDestinationAddressX').attr('readonly', true);
            $('#driveDestinationAddressY').attr('readonly', true);
            $('#driveDestinationAddress').val(adresa);
            $('#driveDestinationAddressX').val(data.lat);
            $('#driveDestinationAddressY').val(data.lon);

        },
        error: function () {
            alert("Error while getting address.");
        }
    });
}

function geocodeLatLngDriver() {
    $.ajax({
        type: 'GET',
        url: 'https://nominatim.openstreetmap.org/reverse',
        data: {
            format: 'jsonv2',
            lat: myLat,
            lon: myLng
        },
        dataType: 'json',
        success: function (data) {
            let adresa = data.address.road + ' ' + data.address.house_number + ', ' + data.address.city + ' ' + data.address.postcode;
            $('#driverLocation').attr('readonly', true);
            $('#driverLocationX').attr('readonly', true);
            $('#driverLocationY').attr('readonly', true);
            $('#driverLocation').val(adresa);
            $('#driverLocationX').val(data.lat);
            $('#driverLocationY').val(data.lon);

        },
        error: function () {
            alert("Error while getting address.");
        }
    });
}

function placeMarkerAndPanTo(latLng, map) {
    clearMarkers();
    deleteMarkers();

    if (markers.length < 1) {
        var marker = new google.maps.Marker({
            position: latLng,
            map: map
        });
        map.panTo(latLng);
        markers.push(marker);
        myLat = marker.getPosition().lat();
        myLng = marker.getPosition().lng();
    }
}

function setMapOnAll(map) {
    for (var i = 0; i < markers.length; i++) {
        markers[i].setMap(map);
    }
}

// Removes the markers from the map, but keeps them in the array.
function clearMarkers() {
    setMapOnAll(null);
}

// Deletes all markers in the array by removing references to them.
function deleteMarkers() {
    clearMarkers();
    markers = [];
}