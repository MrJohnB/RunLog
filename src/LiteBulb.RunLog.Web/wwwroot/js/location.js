// Note: This example requires that you consent to location sharing when prompted by your browser.
// If you see the error "The Geolocation service failed.", it means you probably
// did not give permission for the browser to locate you.
var positionOptions = {
	enableHighAccuracy: true,
	timeout: 5000,
	maximumAge: 0
};

var appAssemblyName = "";
var positionCallbackName = "";

function getPosition(appAssemblyName, positionCallbackName) {
	this.appAssemblyName = appAssemblyName;
	this.positionCallbackName = positionCallbackName;

	if (navigator.geolocation)
		return navigator.geolocation.getCurrentPosition(positionCallback, positionErrorCallback, positionOptions);
	else
		console.log("Geolocation is not supported by this browser.");
}

function positionCallback(geolocationPosition) {
	var positionViewModel = {
		timeStamp: new Date(geolocationPosition.timestamp),
		latitude: geolocationPosition.coords.latitude,
		longitude: geolocationPosition.coords.longitude,
		altitude: geolocationPosition.coords.altitude,
		accuracy: geolocationPosition.coords.accuracy,
		altitudeAccuracy: geolocationPosition.coords.altitudeAccuracy,
		heading: geolocationPosition.coords.heading,
		speed: geolocationPosition.coords.speed,
		satelliteCount: null
	};

	console.log(positionViewModel);

	DotNet.invokeMethodAsync(this.appAssemblyName, this.positionCallbackName, positionViewModel);
}

function positionErrorCallback(error) {
	switch (error.code) {
		case error.PERMISSION_DENIED:
			console.warn("User denied the request for Geolocation.");
			break;
		case error.POSITION_UNAVAILABLE:
			console.warn("Location information is unavailable.");
			break;
		case error.TIMEOUT:
			console.warn("The request to get user location timed out.");
			break;
		case error.UNKNOWN_ERROR:
			console.warn("An unknown error occurred.");
			break;
		default:
			console.warn("Error code: '${error.code}'.  Error message: '${error.message}'.");
	}
}

var watchPositionId; //TODO: what if there's multiple users of the system?
//var watchPositionList = [];

function startWatchPosition(appAssemblyName, positionCallbackName) {
	this.appAssemblyName = appAssemblyName;
	this.positionCallbackName = positionCallbackName;

	if (navigator.geolocation) {
		watchPositionId = navigator.geolocation.watchPosition(watchPositionCallback, positionErrorCallback, positionOptions);
		console.log("Geolocation method watchPosition() started.");
	}
	else
		console.log("Geolocation is not supported by this browser.");
}

function watchPositionCallback(geolocationPosition) {
	var positionViewModel = {
		timeStamp: new Date(geolocationPosition.timestamp),
		latitude: geolocationPosition.coords.latitude,
		longitude: geolocationPosition.coords.longitude,
		altitude: geolocationPosition.coords.altitude,
		accuracy: geolocationPosition.coords.accuracy,
		altitudeAccuracy: geolocationPosition.coords.altitudeAccuracy,
		heading: geolocationPosition.coords.heading,
		speed: geolocationPosition.coords.speed,
		satelliteCount: null
	};

	console.log(positionViewModel);

	DotNet.invokeMethodAsync(this.appAssemblyName, this.positionCallbackName, positionViewModel);
}

function stopWatchPosition(appAssemblyName, stopWatchPositionCallbackName) {
	if (navigator.geolocation) {
		navigator.geolocation.clearWatch(watchPositionId);
		console.log("Geolocation method watchPosition() stopped.");
	}
	else
		console.log("Geolocation is not supported by this browser.  Geolocation method watchPosition() cannot be cleared.");
}