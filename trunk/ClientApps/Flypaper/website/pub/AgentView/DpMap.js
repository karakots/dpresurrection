// DpMap.js
// Author: Steve Noble
// 1-22-09
// basic google map functionality

var map;

//  Only needed this for Asp Update Panel
// but other problems insued with animation
// turns out did not need to embed  map in UpdatePanel

// Add this line to the load function for Asp AJAX
// but be sure to if Asp is available
// Sys.WebForms.PageRequestManager.getInstance().add_endRequest(DpUpdateMap);
    
//function DpUpdateMap(sender, args) {
//    if (GBrowserIsCompatible()) {
//        map = new GMap2(document.getElementById("DPEZGmap"));

//        // default location & settings
//        // view world
//        map.setCenter(new GLatLng(37.4419, -100), 1);
//        map.enableScrollWheelZoom();
//        map.addControl(new GSmallMapControl());

//        if (initMap) {
//            alert("calling init");
//            initMap();
//        }

//        // AnimateMap();
//    }
//}

var desired = new Object();


function DpLoadMap() {

    
    if (GBrowserIsCompatible()) {
        map = new GMap2(document.getElementById("DPEZGmap"));

        // default location & settings
        // view world
        map.setCenter(new GLatLng(37.4419, -100), 1);
        map.enableScrollWheelZoom();
        map.addControl(new GSmallMapControl());

        desired.zoom = -1;

        if (initMap) {
            initMap();
        }

        window.setTimeout(AnimateMap, 1000);
        // AnimateMap();
    }
}

function AnimateMap() {
    if (desired.zoom > 0) {
        var currentCenter = map.getCenter();
        var currentZoomLevel = map.getZoom();
//        var box = map.getBounds();
//        var newLat = currentCenter.center.lat() + 0.5 * (currentCenter.lat() + desired.center.lat());
//        var newLng = desired.center.lng() + 0.5 * (currentCenter.lng() - desired.center.lng());
//        var newPt = new GLatLng(newLat.valueOf(), newLng.valueOf());

        if (currentCenter.distanceFrom(desired.center) > 10000) {
            map.panTo(desired.center);
            
             window.setTimeout(AnimateMap, 1000);
            return;
         }
         
         if (currentZoomLevel < desired.zoom) {
            map.zoomIn();

            window.setTimeout(AnimateMap, 1000);
            return;
         }
         
//        if (box.contains(desired.center)) {
//            map.zoomIn();

//            window.setTimeout(AnimateMap, 1000);
//            return;
//        }
//        else if (currentCenter.distanceFrom(desired.center) > 50000) {
//        
//            map.panTo(newPt);
//            window.setTimeout(AnimateMap, 500);
//            return;
//        }
    }
}

// ensure load an unload are defined correctly
var loadFunc = window["onload"];
if (loadFunc) {
    window["onload"] = function() { loadFunc(), DpLoadMap(); };
}
else {
    window["onload"] = DpLoadMap;
}

var unloadFunc = window["onunload"]
if (unloadFunc) {
    window["onunload"] = function() { unloadFunc(), GUnload(); };
}
else {
    window["onunload"] = GUnload;
}
