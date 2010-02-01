//  -------------------------------General AdPlanIt Scripts ------------------------------
function test() {
    alert("gotcha");
    return true;
}

function ShowMainWaitCursor()
{
    document.body.style.cursor = "progress";
}

function ConfirmEditLoss(){
    var m0 = "      Warning: Plan is edited.  Changes may be lost.  Proceed?    ";
    return confirm( m0 );
}

/* Campaign Page */
function ShowSubcategoryProgress()
{
    document.getElementById( "SetCategoryProgressImage" ).style.visibility = "visible";
}

function CheckGoalCheckboxes(){
    var c1 = document.getElementById( "GoalSelR" ).checked;
    if( c1 == true ){
        document.getElementById( "GoalWeightR" ).className = "ShowGoalWeight";
    }
    else {
        document.getElementById( "GoalWeightR" ).className = "HideGoalWeight";
        document.getElementById( "GoalWeightR" ).value = "";
    }
    
    var c2 = document.getElementById( "GoalSelG" ).checked;
    if( c2 == true ){
        document.getElementById( "GoalWeightG" ).className = "ShowGoalWeight";
    }
    else {
        document.getElementById( "GoalWeightG" ).className = "HideGoalWeight";
        document.getElementById( "GoalWeightG" ).value = "";
    }
      
    var c3 = document.getElementById( "GoalSelP" ).checked;
    if( c3 == true ){
        document.getElementById( "GoalWeightP" ).className = "ShowGoalWeight";
    }
    else {
        document.getElementById( "GoalWeightP" ).className = "HideGoalWeight";
        document.getElementById( "GoalWeightP" ).value = "";
    }
    
    var c4 = document.getElementById( "GoalSelD" ).checked;
    if( c4 == true ){
        document.getElementById( "GoalWeightD" ).className = "ShowGoalWeight";
    }
    else {
        document.getElementById( "GoalWeightD" ).className = "HideGoalWeight";
        document.getElementById( "GoalWeightD" ).value = "";
    }
       
    var c5 = document.getElementById( "GoalSelT" ).checked;
    if( c5 == true ){
        document.getElementById( "GoalWeightT" ).className = "ShowGoalWeight";
    }
    else {
        document.getElementById( "GoalWeightT" ).className = "HideGoalWeight";
        document.getElementById( "GoalWeightT" ).value = "";
    }
    
    
    CheckWeightSum();
}

function VerifyGoalWeightsOK(){
    CheckWeightSum();
    if( goalWeigntsOK == false ){
          alert( "\r\n\r\n         Error: Goal Weights must total 100%     \r\n\r\n" );
          return false;
    }
    else {
        return true;
    }
}

function GetPlanName( defaultName ){
    CheckWeightSum();
    if( goalWeigntsOK == false ){
          alert( "\r\n\r\n         Error: Goal Weights must total 100%     \r\n\r\n" );
          return false;
    }
    GetNewPlanName( defaultName, "      Enter a name for your Media Plan:    " );
    return false;
}

function AllowOnlyNumbers( event ){
    var charCode = ( event.which ) ? event.which : event.keyCode;
    if( (charCode >= 48 && charCode <= 57) || charCode == 8 || charCode == 46 ){
        status = "";
        return true;
    }
    status = "Entries must be numbers only";
    return false;
}

function GetNameForNextVersion( currentVersionName ){
    GetNewPlanName( currentVersionName, "      A NEW VERSION of this plan will be created to preserve existing results. New Plan Name:      " );
    return false;
}

var changingPlanName = false;
function ChangePlanName( currentName ){
    GetNewPlanName( currentName, "      Enter new name for this Media Plan:    " );
    changingPlanName = true;
    return false;
}

function ShowModalPopup( caption, suggestedName ) {
   document.getElementById( "PopupCaptionTextP" ).innerHTML = caption;
   if( suggestedName != null ){
        document.getElementById( "NewNameBox" ).value = suggestedName;
        document.getElementById( "NewNameBox" ).className = "NewNameBoxShown";
        document.getElementById( "PopupCaptionTextP" ).className = "PopupCaptionText";
        document.getElementById( "PopupButtonsTbl" ).className = "PopupButtonsTable";
        document.getElementById( "CancelButton" ).className = "CancelButtonShown";
    }
    else {
        document.getElementById( "NewNameBox" ).className = "NewNameBoxHidden";
        document.getElementById( "PopupCaptionTextP" ).className = "PopupCaptionTextOnly";
        document.getElementById( "PopupButtonsTbl" ).className = "PopupButtonsTableOKOnly";
        document.getElementById( "CancelButton" ).className = "CancelButtonHidden";
    }
       
    var modalPopupBehavior = $find('programmaticModalPopupBehavior');
    modalPopupBehavior.show();
    document.getElementById( "NewNameBox" ).select();
    document.getElementById( "NewNameBox" ).focus();

   return false;
}
        
function OnGetNameOK(){
    var newName = document.getElementById( "NewNameBox" ).value;
    HideModalPopup();
    return false;
}

function OnGetNameCancel(){
    HideModalPopup();
    return false;
}

function HideModalPopup() {
    var modalPopupBehavior = $find('programmaticModalPopupBehavior');
    modalPopupBehavior.hide();
    status = "";
    return false;
}

function GetNewPlanName( currentName, promptStr, showAutoCheckbox ){
    ShowModalPopup( promptStr, currentName, showAutoCheckbox );
    return false;
}

function CheckForEnter( event )
{
    if( event.keyCode == 13 ){
        document.getElementById( "hideModalPopupViaClientButton" ).click();
        status = "";
        return false;
    }
    if( event.keyCode == 222 ){
        status = "Quotes not allowed in names";
        return false;    // disallow single and double quotes
    }    
    if( event.keyCode == 34 || event.keyCode == 39 ){
        status = "Quotes not allowed in names";
        return false;    // disallow single and double quotes
    }
    status = "";
    return true;
}

var planIsAutogenerated = false;

function ReceiveNewPlanName( existingNames ){
    var oldNames = existingNames.split( '^' );
    var nameOK = false;
    do {
        var newName = document.getElementById( "NewNameBox" ).value;
        if( newName != null ){
            var nameInUse = false;
           for( i = 0; i < oldNames.length; i++ ){
                if( newName == oldNames[ i ] ){
                    nameInUse = true;
                    break;
                }
            }
       
           if( nameInUse == true ){
              alert( "\r\n\r\n         Error: Plan name already in use.     \r\n\r\n" );
              currentName = newName;
              return false;
            }
            else {
              nameOK = true;
            }
       }
       else {
           // the user hit cancel
           return false;
        }          
    } while( nameOK == false );
  
   document.getElementById( "newPlanName" ).value = newName;

    HideModalPopup();
    if( changingPlanName == false ){
        __doPostBack( "ChangePlanName", planIsAutogenerated );
    }
    else {
         __doPostBack( "ChangePlanNameOnly", planIsAutogenerated );
    }
    return false;
}

var goalWeigntsOK = false;

function CheckWeightSum(){
    var w1 = document.getElementById( "GoalWeightR" ).value;
    var w1n = parseInt( w1 );
    if( w1 == "" ){
        w1n = 0;
    }
    
    var w2 = document.getElementById( "GoalWeightG" ).value;
    var w2n = parseInt( w2 );
     if( w2 == "" ){
        w2n = 0;
    }
   
    var w3 = document.getElementById( "GoalWeightP" ).value;
    var w3n = parseInt( w3 );
     if( w3 == "" ){
        w3n = 0;
    }
   
    var w4 = document.getElementById( "GoalWeightD" ).value;
    var w4n = parseInt( w4 );
    if( w4 == "" ){
        w4n = 0;
    }
    
    var w5 = document.getElementById( "GoalWeightT" ).value;
    var w5n = parseInt( w5 );
     if( w5 == "" ){
        w5n = 0;
    }
   
    var tot = w1n + w2n + w3n + w4n + w5n;
        
    if( tot == 100 ){
        goalWeigntsOK = true;
        document.getElementById( "TotalGoalWeightSpan" ).innerHTML = "Total: " + tot + "%";
     }
     else {
        goalWeigntsOK = false;
        document.getElementById( "TotalGoalWeightSpan" ).innerHTML = "<font color='Red'><b>Total: " + tot + "%</b></font>";
     }
}

function SaveProfile(){
    __doPostBack( "SaveProfile", 0 );
}

function ShowNoRegionWarning(){
    alert( '\r\n\r\n                You must specify at least one geographic region before creating a plan.                 \r\n\r\n' );
}

function CampaignNameIsLegal(){
    var tst = document.getElementById( "CampaignNameTextBox" ).value;
    var nameok = true;
    if( tst.indexOf( "/" ) != -1 ){
        nameok = false;
    }   
    if( tst.indexOf( "\\" ) != -1 ){
        nameok = false;
    }
    if( tst.indexOf( ":" ) != -1 ){
        nameok = false;
    }
    if( tst.indexOf( "*" ) != -1 ){
        nameok = false;
    }
    if( tst.indexOf( "?" ) != -1 ){
        nameok = false;
    }
    if( tst.indexOf( "\"" ) != -1 ){
        nameok = false;
    }
    if( tst.indexOf( "<" ) != -1 ){
        nameok = false;
    }
    if( tst.indexOf( ">" ) != -1 ){
        nameok = false;
    }
    if( tst.indexOf( "|" ) != -1 ){
        nameok = false;
    }    
    if( tst.indexOf( "'" ) != -1 ){
        nameok = false;
    }
    
    if( nameok == false ){
        alert( '\r\n\r\n                The campaign name contains one or more illegal characters, which are: / \\ : * ? \" < > |  \'               \r\n\r\n' );
    } 
    return nameok;
}

// invoke a postback to delete the segment with the given ID
function DoRemovePostback( segmentIdToDelete ){
    document.forms[0].deleteSegmentId.value = segmentIdToDelete;
    __doPostBack( "RemoveLink", 0 );
}

// invoke a postback to delete the segment with the given ID
function DoRemoveRegionPostback( regionNumToDelete ){
    document.forms[0].deleteRegionNum.value = regionNumToDelete;
    __doPostBack( "RemoveRegionLink", 0 );
}

// show the bottom of the scrollable transcript window
function ScrollTranscriptToBottom(){
    document.getElementById( "StoryBodyDiv" ).scrollTop = 50000; 
}

/* Home Page */
function OpenDemoWindow(){
    window.open( "Tutorial/Tutorial_1.html", "__blank", "menubar=no,scrollbars=no,toolbar=no,titlebar=no,height=800", false );
}

function OpenDemo2Window(){
    window.open( "Demo1/demo1.htm", "__blank", "menubar=no,scrollbars=no,toolbar=no,titlebar=no", false );
}

function ShowPrintView(){
    window.open( window.location + "?print=1", "__blank", "menubar=false", true );
    return false;
}

function DoCreatRFP(){
    alert( '\r\n\r\n       Create RFP -- Coming soon!        \r\n\r\n' );
    return false;
}

function UpdatePrice( rowNumber, countInputName ){
    UpdatePPrice( rowNumber, countInputName, null );
}

function SetTargetingPriceRatio(){
    var sliderVal = document.getElementById( "TargetingLevelTextbox" ).value;
    var mult = (sliderVal / 10) + 1;
    var multstr = " " + mult;
    if( multstr.indexOf( "." ) == -1 ){
        multstr = multstr + ".0";
    }
    
    document.getElementById( "adPriceMultiplier" ).innerHTML = multstr;
}

function UpdatePPrice( rowNumber, countInputName, pulseCountInputName ){
    var pr1 = document.getElementById( countInputName ).value;
    var costInput = "RowPrice" + rowNumber;
    var costOutput = "RowTotal" + rowNumber;
    var costDispOutput = "RowTotalDisp" + rowNumber;
    var costper = document.getElementById( costInput ).value;
    
    var pulsecnt = 1;
    
    if( pulseCountInputName != null ){
        pulsecnt = document.getElementById( pulseCountInputName ).value;
        pulsecnt = (pulsecnt * 1) + 1;
    }
    
    document.getElementById( costOutput ).value = ptot;
    
    var ptot = new Number();
    ptot = pr1 * costper * pulsecnt;
    
    document.getElementById( costDispOutput ).innerHTML = forrmatDollars( ptot );
    
    UpdateTotalPrice();
}

function UpdateTotalPrice(){
    var expr = document.getElementById( "totalExpression" ).value;

    var gtot = new Number();
    gtot = eval( expr );
       
    document.getElementById( 'GrandTotal' ).innerHTML ="Total:&nbsp;<b>" + forrmatDollars( gtot ) + "</b>";
}

function forrmatDollars( dtot ){
    var dval = dtot;
    var endLen = 3;
    if( dtot >= 10 ){
        dval = dtot.toFixed( 2 );
    }
    if( dtot >= 1000 ){
        dval = dtot.toFixed( 0 );
        endLen = 0;
    }
     
    var dvalstr = "$&nbsp;" + dval;
    if( dtot >= 1000 ){
        var s1 = dvalstr.substr( 0, dvalstr.length - 3 - endLen );
        var s2 = dvalstr.substring( dvalstr.length - 3 - endLen, dvalstr.length );
        dvalstr = s1 + "," + s2;
    }
    
    if( dtot >= 1000000 ){
        var s1 = dvalstr.substr( 0, dvalstr.length - 7 - endLen );
        var s2 = dvalstr.substring( dvalstr.length - 7 - endLen, dvalstr.length );
        dvalstr = s1 + "," + s2;
    }
    return dvalstr;
}

/* Analysis page (improvement suggestions) */
function ShowSuggestionProgress(){


    if( document.getElementById( "WaitMessage" ) != null ){
        document.getElementById( "WaitMessage" ).className = "WaitMessage";
    }

    if (document.getElementById("SuggestionCell") != null) {
        document.getElementById("SuggestionCell").innerHTML = "&nbsp";
    }

    window.setTimeout(WaitError, 310000);

    return true;
}


function WaitError() {

 if( document.getElementById( "WaitMessage" ) != null ){
        document.getElementById( "WaitMessage" ).className = "WaitMessageHidden";
    }
    if (document.getElementById("SuggestionCell") != null) {
        document.getElementById("SuggestionCell").innerHTML = "<div style=\"color: Red; font-size: 9pt; font-weight: bold;\">The requested operation timed out</div>";
    }
    
}


function ClearWaitCursor(){
    document.body.style.cursor = "default";
}

function VerifyImprovedPlanNameIsNew( existingNames ){
    var oldNames = existingNames.split( '^' );
    var nameOK = false;
    do {
        var newName = document.getElementById( "SuggestedPlanName" ).value;
        if( newName != null ){
            var nameInUse = false;
           for( i = 0; i < oldNames.length; i++ ){
                if( newName == oldNames[ i ] ){
                    nameInUse = true;
                    break;
                }
            }
       
           if( nameInUse == true ){
              alert( "\r\n\r\n          Error: Plan name is already in use.          \r\n         Please enter a different name.        \r\n\r\n" );
              currentName = newName;
              return false;
            }
            else {
              nameOK = true;
            }
       }
       else {
           alert( 'Error: did not get new name' );
           return false;
        }          
    } while( nameOK == false );
    
    return true;
}

function UpdateSuggestedPlanTotal( numRows, currentTotal ){
    var i = 0;
    var tot = new Number();
    tot = currentTotal;
    for( i = 0; i < numRows; i++ ){
        var addID = "PriceAdd" + i;
        var addSelID = "Add" + i;
        var rmvID = "PriceRemove" + i;
        var rmvSelID = "Remove" + i;
        
        var t1 = document.getElementById( addSelID ).checked;
        var t2 = document.getElementById( addID ).value;
        
        if( document.getElementById( addID ) != null ){
           var v1 = document.getElementById( addSelID ).checked;
            if( v1 == true ){
                var vv2 = document.getElementById( addID ).value;
                var v2 = new Number();
                v2 = eval( vv2 );
                tot = tot + v2;
            }
        }
        if( document.getElementById( rmvID ) != null ){
            var v1x = document.getElementById( rmvSelID ).checked;
            if( v1x == true ){
                var vv2x = document.getElementById( rmvID ).value;
                var v2x = new Number();
                v2x = eval( vv2x );
                tot = tot - v2x;
            }
        }

    }
    //alert( 'fooo2d: ' + tot );
    document.getElementById( 'SuggPlanTotal' ).innerHTML = "Total with Changes: <b>" + forrmatDollars( tot ) + "<b>";
}
  
// Timing.aspx page
function CheckEnteredDates(){
    var nr1 = document.getElementById( "TimingItemCount" ).value;
    
    var psy = document.getElementById( "PlanStartYear" ).value;
    var psm = document.getElementById( "PlanStartMonth" ).value;
    var psd = document.getElementById( "PlanStartDay" ).value;
    var planStart = new Date();
    planStart.setFullYear( psy, psm, psd );
   
    var pey = document.getElementById( "PlanEndYear" ).value;
    var pem = document.getElementById( "PlanEndMonth" ).value;
    var ped = document.getElementById( "PlanEndDay" ).value;
    var planEnd = new Date();
    planEnd.setFullYear( pey, pem, ped );
      
    var ldy = document.getElementById( "LeadDateYear" ).value;
    var ldm = document.getElementById( "LeadDateMonth" ).value;
    var ldd = document.getElementById( "LeadDateDay" ).value;
    var leadDate = new Date();
    leadDate.setFullYear( ldy, ldm, ldd );
    
    var early = false;
    var late = false;
    var needLead = false;
    
    for( i = 0; i < nr1; i++ ){
        var oidn = "RowOpt" + i;
        var opt = document.getElementById( oidn ).value;
        var yid = "Year-" + i + "-" + opt;
        var yr = document.getElementById( yid ).value;
        var mid = "Month-" + i + "-" + opt;
        var mo = document.getElementById( mid ).value;
        var did = "Day-" + i + "-" + opt;
        var dy = document.getElementById( did ).value;

        var myDate = new Date();
        myDate.setFullYear( yr, mo, dy );

        if( myDate < planStart ){
           early = true;
        }
        else if( myDate < leadDate ){
           needLead = true;
        }
        
        if( myDate > planEnd ){
           late = true;
        }    
    }
    
    if( late == false && needLead == false && late == false ){
        return true;
    }
 
    if( early == true && late == false ){
        alert( "      Error: EARLY ADS.  Some ad dates are before the beginning of the campaign.      \r\n\r\n      To be analyzed, all ad dates must be within the duration of the campaign.      " );
        return false;
    }
    else if( early == false && late == true ){
        alert( "      Error: LATE ADS.  Some ad dates are after the end of the campaign.      \r\n\r\n      To be analyzed, all ad dates must be within the duration of the campaign.      " );
        return false;
    }
    else if( early == true && late == true ){
         alert( "      Error: EARLY/LATE ADS.  Some ad dates are out of range of the campaign.      \r\n\r\n      To be analyzed, all ad dates must be within the campaign duration.      " );
        return false;
    }
    else if( needLead == true ){
        var typ = document.getElementById( "TypeSelection" ).value;
        var mm = "";
        if( typ == "Radio" ){
            mm = "      New Radio ads typically require about two weeks to prepare and get scheduled.    \r\n\r\n";
        }
        else if( typ == "Newspaper" ){
            mm = "      New Newspaper ads typically require about one week to prepare and get published.    \r\n\r\n";
        }
        else if( typ == "Magazine" ){
            mm = "      New Magazine ads typically require a minimum of 6-8 weeks to prepare and get published.    \r\n\r\n";
        }
        else if( typ == "Yellowpages" ){
            mm = "      Since Yellow Pages are publlished yearly, you must check with the media vendor for cutoff dates.    \r\n\r\n";
        }
    
        var msg = "      Warning: SHORT LEAD TIME.   \r\n\r\n      Some ad dates are earler than would be reasonable considering the expected lead time "+
         " for starting new ads in this media.     \r\n\r\n" + mm + "      Use these dates anyway?.      ";
         return confirm( msg  );
    } 
    else {
        alert( "      Error: Unexpected error!      " );
        return false;
    }
}

// media info popup dialog
var t;
var vname_v;
var vtype_v;
var vsub_v;
var vurl_v;
var vgeo_v;

function PrepInfoPopup( vname, vtype, vsub, vurl, vgeo, doNow ){
    vname_v = vname;
    vtype_v = vtype;
    vsub_v = vsub;
    vurl_v = vurl;
    vgeo_v = vgeo;
    
    if( doNow ){
        ShowInfoPopup();
    }
    else {
        //t = setTimeout( "ShowInfoPopup();", 700 );
    }
    status = "Click for more information on this media";
}

function ShowInfoPopup(){
    document.getElementById( "InfoPopTitle" ).innerHTML = vtype_v + ' Media Details';
    document.getElementById( "InfoPopName" ).innerHTML = vname_v;
    document.getElementById( "InfoPopGeo" ).innerHTML = vgeo_v;
    document.getElementById( "InfoPopType" ).innerHTML = vsub_v;
    if( vurl_v != "" ){
        var itemSite = "http://" + vurl_v;
        document.getElementById( "InfoPopURL" ).innerHTML = "<a href='#' onclick='window.open( " + '"' + itemSite + '"' + " );' >" + vurl_v + "</a>";
    }
    else {
        document.getElementById( "InfoPopURL" ).innerHTML = 'not available';
    }
   
    var modalPopupBehavior2 = $find('programmaticModalPopupBehavior2');
    modalPopupBehavior2.show();
}

function DismissInfoPopup(){
    clearTimeout( t );
    status = "";
}

function HideInfoPopup() {
    var modalPopupBehavior2 = $find('programmaticModalPopupBehavior2');
    modalPopupBehavior2.hide();
    status = "";
    return false;
}

function ShowCompetitionControls(){
    var x = document.getElementById( "CompetitionSelection" ).checked;
    document.getElementById( "CompetitionMode" ).value = x;
    if( x == true ){
        document.getElementById( "CompNameLabel" ).className = "CompVis";
        document.getElementById( "CompetitionName" ).className = "CompVis";
    }
    else {
        document.getElementById( "CompNameLabel" ).className = "CompVisHidden";
        document.getElementById( "CompetitionName" ).className = "CompVisHidden";
    }
}