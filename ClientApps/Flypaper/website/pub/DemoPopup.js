// METHODS IN THIS FILE FOR USE WITH POPUP DEMOGRAPHICS DIALOG 
   
// invoke a postback to delete the segment with the given ID
function DoRemovePostback( segmentIdToDelete ){
    document.forms[0].deleteSegmentId.value = segmentIdToDelete;
    __doPostBack( "RemoveLink", 0 );
}

var demoShown = false;

// display the popup demographics dialog
function ShowDemoPopup(){
    document.getElementById( "PopupBackground" ).className = "ShowPopupBackground";
    document.getElementById( "DemoPopupPanel" ).className = "ShowDemoPopupPanel";
    document.getElementById( "DemoPopupShadow" ).className = "ShowDemoPopupShadow";
    demoShown = true;
}

// display the progress indicator for DMA lookup
function ShowRegionUpdate(){
    if( demoShown == true ){
        document.getElementById( "SubregionsList" ).style.visibility = "hidden";
        document.getElementById( "SetStateProgressImage" ).style.visibility = "visible";
        document.getElementById( "SubregionLabel" ).style.visibility = "visible";
    }
}

 // hide the popup demographics dialog after the OK button has been clicked
 function AcceptDemoPopupB(){
    document.forms[ 0 ].demoDataAvailable.value = 'true';
    document.getElementById( "GoalWeightD" ).innerHTML = "Specified";
    HideDemoPopup();
 }

 // hide the popup demographics dialog
 function HideDemoPopup(){
    document.getElementById( "PopupBackground" ).className = "PopupBackground";
    document.getElementById( "DemoPopupPanel" ).className = "DemoPopupPanel";
    document.getElementById( "DemoPopupShadow" ).className = "DemoPopupShadow";
    document.getElementById( "SubregionsList" ).style.visibility = "hidden";
    document.getElementById( "SubSubregionsList" ).style.visibility = "hidden";
    document.getElementById( "SetStateProgressImage" ).style.visibility = "hidden";
    document.getElementById( "SubregionLabel" ).style.visibility = "hidden";
    demoShown = false;
}

// handle a click on the any-gender checkbox
function CheckAnyGender(){
   if( document.forms[ 0 ].GenderAny.checked ){
        document.getElementById( "GenderRow" ).className = "HideRow";
        document.forms[ 0 ].GenderMale.disabled = true;
        document.forms[ 0 ].GenderFemale.disabled = true;
    }
    else {
        document.getElementById( "GenderRow" ).className = "ShowDemoRow";
        document.forms[ 0 ].GenderMale.disabled = false;
        document.forms[ 0 ].GenderFemale.disabled = false;
    }
}

// handle a click on the any-age checkbox
function CheckAnyAge(){
   if( document.forms[ 0 ].AgeAny.checked ){
       // document.getElementById( "AgeRow" ).className = "HideRow2";
        document.forms[ 0 ].Age1.disabled = true;
        document.forms[ 0 ].Age2.disabled = true;
        document.forms[ 0 ].Age3.disabled = true;
        document.forms[ 0 ].Age4.disabled = true;
        document.forms[ 0 ].Age5.disabled = true;
        document.forms[ 0 ].Age6.disabled = true;
        document.forms[ 0 ].Age7.disabled = true;
    }
    else {
        // document.getElementById( "AgeRow" ).className = "ShowDemoRow";
        document.forms[ 0 ].Age1.disabled = false;
        document.forms[ 0 ].Age2.disabled = false;
        document.forms[ 0 ].Age3.disabled = false;
        document.forms[ 0 ].Age4.disabled = false;
        document.forms[ 0 ].Age5.disabled = false;
        document.forms[ 0 ].Age6.disabled = false;
        document.forms[ 0 ].Age7.disabled = false;
    }
}
    
// handle a click on the any-income checkbox
function CheckAnyIncome(){
   if( document.forms[ 0 ].IncomeAny.checked ){
        document.getElementById( "IncomeRow" ).className = "HideRow";
        document.forms[ 0 ].Income1.disabled = true;
        document.forms[ 0 ].Income2.disabled = true;
        document.forms[ 0 ].Income3.disabled = true;
        document.forms[ 0 ].Income4.disabled = true;
        document.forms[ 0 ].Income5.disabled = true;
    }
    else {
        document.getElementById( "IncomeRow" ).className = "ShowDemoRow";
        document.forms[ 0 ].Income1.disabled = false;
        document.forms[ 0 ].Income2.disabled = false;
        document.forms[ 0 ].Income3.disabled = false;
        document.forms[ 0 ].Income4.disabled = false;
        document.forms[ 0 ].Income5.disabled = false;
    }
}
    
// handle a click on the any-kids checkbox
function CheckAnyKids(){
   if( document.forms[ 0 ].KidsAny.checked ){
        document.getElementById( "KidsRow" ).className = "HideRow2";
        document.forms[ 0 ].Kids0.disabled = true;
        document.forms[ 0 ].Kids1.disabled = true;
    }
    else {
        document.getElementById( "KidsRow" ).className = "ShowDemoRow";
        document.forms[ 0 ].Kids0.disabled = false;
        document.forms[ 0 ].Kids1.disabled = false;
    }
}
    
// handle a click on the any-race checkbox
function CheckAnyRace(){
   if( document.forms[ 0 ].RaceAny.checked ){
        document.getElementById( "RaceRow" ).className = "HideRow";
        document.forms[ 0 ].Race1.disabled = true;
        document.forms[ 0 ].Race2.disabled = true;
        document.forms[ 0 ].Race3.disabled = true;
        document.forms[ 0 ].Race4.disabled = true;
    }
    else {
        document.getElementById( "RaceRow" ).className = "ShowDemoRow";
        document.forms[ 0 ].Race1.disabled = false;
        document.forms[ 0 ].Race2.disabled = false;
        document.forms[ 0 ].Race3.disabled = false;
        document.forms[ 0 ].Race4.disabled = false;
    }
}