//  -------------------------------Campaigns-list Page Scripts ------------------------------

function ShowMainWaitCursor()
{
    document.body.style.cursor = "progress";
}

function OpenDemoWindow(){
    window.open( "Tutorial/Tutorial_1.html", "__blank", "menubar=no,scrollbars=no,toolbar=no,titlebar=no,height=800", false );
}

var campaignNames = "?";
function SetCampaignNames( cnames ){
    campaignNames = cnames;
}

var nextPlanNames = "?";
function SetNextPlanNames( npnames ){
    nextPlanNames = npnames;
}

var campaignPlanNames = [];
function SetCampaignPlanNames( cpnames, indx ){
    campaignPlanNames[ indx ] = cpnames;
}

var actionCode;
var actionCampaignIndex;
var actionCurrentPlanName;

function GetNewPlanName( suggestedNewName, promptStr, showAutoCheckbox, campaignIndx ){
    document.getElementById( "PopupTitleText" ).innerHTML = "Create New Media Plan";   
    ShowModalPopup( promptStr, suggestedNewName, showAutoCheckbox );
    actionCode = "new";
    actionCampaignIndex = campaignIndx;
    return false;
}

function GetCopiedCampaignName( suggestedNewName, promptStr, showAutoCheckbox, campaignIndx ){
    document.getElementById( "PopupTitleText" ).innerHTML = "Duplicate Campaign";   
    ShowModalPopup( promptStr, suggestedNewName, showAutoCheckbox );
    actionCode = "ccopy";
    actionCampaignIndex = campaignIndx;
    return false;
}

function GetCopiedPlanName( currentName, suggestedNewName,  promptStr, showAutoCheckbox, campaignIndx ){
    actionCurrentPlanName = currentName;
    document.getElementById( "PopupTitleText" ).innerHTML = "Duplicate Media Plan";   
    ShowModalPopup( promptStr, suggestedNewName, showAutoCheckbox );
    actionCode = "pcopy";
    actionCampaignIndex = campaignIndx;
    return false;
}

function OnGetNameCancel(){
    HideModalPopup();
    return false;
}

function HideModalPopup() {
    var modalPopupBehavior = $find('programmaticModalPopupBehavior');
    modalPopupBehavior.hide();
    return false;
}

// handles a change in selected value of a plan-action dropdown on the campaigns page
function FireActionPostback( controlID, campaignIndex, copiedPlanName ){

    var ctlVal = document.getElementById( controlID ).value;
    var indx1 = ctlVal.indexOf( "::" );
    var indx2 = ctlVal.lastIndexOf( "::", ctlVal.length );
    var cnamestr = ctlVal.substring( 0, indx1 );
    var pname = ctlVal.substring( indx1 + 2, indx2 );
    if( pname.length == 0 ){
        pname = "---";
    }
    
    var indx3 = cnamestr.indexOf( "-" );
    var cname = cnamestr.substr( indx3 + 1 );
    var act = cnamestr.substring( 0, indx3 );
    
    if( act == "" ){
        return;
    }
    
    var ml = "             ";
    var mr = "                        ";
    
    var msg = ml + "OK to " + act + " Plan?" + mr + "\r\n\r\n" + ml + "Plan: " + pname + mr + "\r\n\r\n" + ml + "Campaign: " + cname + mr;
    
    if( act == "Delete" ){
        if( confirm( msg ) == false ){
            document.getElementById( controlID ).selectedIndex = 0;
            return;
        }
    }    
    
    if( act == "Duplicate" ){
        var pnam = GetCopiedPlanName( pname, copiedPlanName,"      Name for the new Media Plan:    ", false, campaignIndex );
        document.getElementById( controlID ).selectedIndex = 0;        
        return;  
    }
    
    document.getElementById( controlID ).selectedIndex = 0;
    __doPostBack( "DoAction", ctlVal );
}

function OkToCopyPlan(){
    var pnam;
    if( pnam == null ){
        document.getElementById( controlID ).selectedIndex = 0;
        return;
    }
    document.getElementById( "newPlanName" ).value = pnam;     
}


// handles a change in selected value of a campaign-action dropdown on the campaigns page
function FireCampaignActionPostback( controlID, campaignIndex, suggestedCopyName ){

    var ctlVal = document.getElementById( controlID ).value;
    
    var indx = ctlVal.indexOf( "-" );
    var cname = ctlVal.substr( indx + 1 );
    var act = ctlVal.substring( 0, indx );
    
    if( act == "" ){
        return;
    }
    
    var ml = "             ";
    var mr = "                        ";
    
    var msg = ml + "OK to " + act + " Campaign?" + mr + "\r\n\r\n" + ml + "Campaign: " + cname + mr;
    
    if( act == "Delete" ){
        if( confirm( msg ) == false ){
            document.getElementById( controlID ).selectedIndex = 0;
            return;
        }
    }    
    
    if( act == "Copy" ){
        GetCopiedCampaignName( suggestedCopyName,"      Name for the new Campaign:    ", false, campaignIndex );
        document.getElementById( controlID ).selectedIndex = 0;
        return;
    }
    
    if( act == "Add" ){
        var defaultNames = nextPlanNames.split( "^" );
        var dnSel = defaultNames[ campaignIndex - 1 ];
        GetNewPlanName( dnSel,"      Name for the new Media Plan:    ", true, campaignIndex );
        document.getElementById( controlID ).selectedIndex = 0;
        return; 
    }
    
    document.getElementById( controlID ).selectedIndex = 0;
    __doPostBack( "DoCampaignAction", ctlVal );
}

function OkToAddPlan(){
    var pnam;
    if( pnam == null ){
        document.getElementById( controlID ).selectedIndex = 0;
        return;
    }
    document.getElementById( "newPlanName" ).value = pnam;     
}


function ShowModalPopup( caption, suggestedName, showAutogenCheckbox ) {
   document.getElementById( "PopupCaptionTextP" ).innerHTML = caption;
//   if( suggestedName != null ){
        document.getElementById( "NewNameBox" ).value = suggestedName;
        document.getElementById( "NewNameBox" ).className = "NewNameBoxShown";
        document.getElementById( "PopupCaptionTextP" ).className = "PopupCaptionText";
        document.getElementById( "PopupButtonsTbl" ).className = "PopupButtonsTable";
        document.getElementById( "CancelButton" ).className = "CancelButtonShown";
//    }
//    else {
//        document.getElementById( "NewNameBox" ).className = "NewNameBoxHidden";
//        document.getElementById( "PopupCaptionTextP" ).className = "PopupCaptionTextOnly";
//        document.getElementById( "PopupButtonsTbl" ).className = "PopupButtonsTableOKOnly";
//        document.getElementById( "CancelButton" ).className = "CancelButtonHidden";
//    }
    
    if( showAutogenCheckbox == true ){
        document.getElementById( "AutoGenSpan" ).className = "AutogenCheckbox";
    }
    else {
        document.getElementById( "AutoGenSpan" ).className = "AutogenCheckboxHidden";
    }
   
    var modalPopupBehavior = $find('programmaticModalPopupBehavior');
    modalPopupBehavior.show();
    document.getElementById( "NewNameBox" ).select();
    document.getElementById( "NewNameBox" ).focus();

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


function ReceiveNewName(){
    var existingNames;
    if( actionCode == "new" || actionCode == "pcopy" ){
        existingNames = campaignPlanNames[ actionCampaignIndex - 1 ];
    }
    else if( actionCode == "ccopy" ){
        existingNames = campaignNames;
    }
   
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
              if( actionCode != "ccopy" ){
                  alert( "\r\n\r\n         Error: Plan name already in use.     \r\n\r\n" );
              }
              else {
                  alert( "\r\n\r\n         Error: Campaign name already in use.     \r\n\r\n" );
              }
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
    
    HideModalPopup();
    
    if( actionCode == "new" ){
       document.getElementById( "newPlanName" ).value = newName;
        __doPostBack( "CreateNewPlan", actionCampaignIndex );
    }
    else if( actionCode == "ccopy" ){
        document.getElementById( "newCampaignName" ).value = newName;
        __doPostBack( "CopyCampaign", actionCampaignIndex );
    }
    else if( actionCode == "pcopy" ){
       document.getElementById( "newPlanName" ).value = newName;
       var acionCode = actionCampaignIndex + "::" + actionCurrentPlanName;
        __doPostBack( "CopyPlan", acionCode );
    }

    return false;
}
