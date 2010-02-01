<%@ Page Language="C#" AutoEventWireup="true" CodeFile="About.aspx.cs" Inherits="About" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>About AdPlanit</title>
    <link rel="stylesheet" type="text/css" href="~/css/About.css" />     
    <script type="text/javascript" src="AdPlanIt.js"></script>
</head>
<body>
    <form id="HomeForm1" runat="server">
    <input type="hidden" ID="ExpandedItemTypes" name="ExpandedItemTypes" runat="server" />
    <asp:ScriptManager ID="ScriptManager1" runat="server"  />    

    <div id="MainDIv" >    
      <%-- Start of standard header --%>

       <asp:Panel ID="NavBar" CssClass="NavBar" runat="server"  >
      </asp:Panel>       

<asp:Image runat="server" ID="BetaMarker" ImageUrl="images/Beta-Green.gif" />
<div style="height:29px;">&nbsp;</div>
        <ul class="nav">
            <li></li>
            <li><a href="Support.aspx" id="A2" class="nav_help" onclick="ShowMainWaitCursor(); return true;" ></a></li>
            <li><a href="Campaigns.aspx" id="A1" class="nav_mycampaigns" onclick="ShowMainWaitCursor(); return true;" ></a></li>
            <li><a href="Home.aspx" id="ctl00_NavHome" class="nav_home active" onclick="ShowMainWaitCursor(); return true;" ></a></li>
        </ul>
              <img class="Logo" src="images/Logo_v2-2.gif" />
       <%-- End of standard header --%>
       
       <div ID="ContentDiv" runat="server" visible="true">    
        <table ID="ContentTable" cellpadding="0" cellspacing="0" width="100%">
           <tr>
               <td ID="HoomeInfoDiv" runat="server" >

              </td>
               <td id="RHCol" runat="server" rowspan="3" style="z-index:1000">
               <div id="HomeRHDiv">
               <div style="position:relative; text-align:center; top: 2px; margin-bottom: 18px; margin-right: 10px; color: White; font-weight: bold;" > Our Management</div>



		   


<div id="HomeRHBottomDiv"> 

<img src="images/WhiteDot.gif" class="WhiteDot" />
Doug Chalmers, CEO<br /><br />
<img src="images/WhiteDot.gif" class="WhiteDot"  />
Ken Karakotsios, Founder & President<br /><br />
<img src="images/WhiteDot.gif" class="WhiteDot"  />
Steve Noble, VP Engineering<br /><br />
<img src="images/WhiteDot.gif" class="WhiteDot"  />
Dean Dubbe, VP Finance & Operations<br /><br />

<p class="advisors"><b>Our Advisors</b></p>



<img src="images/WhiteDot.gif" class="WhiteDot" />
Steve Gertz, President, Sage Associates<br /><br />
<img src="images/WhiteDot.gif" class="WhiteDot" />
Michaela Draganska, Assoc. Professor of Marketing, Stanford University<br /><br />
<img src="images/WhiteDot.gif" class="WhiteDot" />
Jack Koch, former sales & marketing executive at several large technology companies.


</div>


               </div>
               </td>
               
               <%-- End of Right-hand Column --%>
           </tr>
           <tr>
               <td ID="PageDataCell" runat="server">
                <table id="HomePageInfoTable" cellpadding="0" cellspacing="0">
                <tr>
                <td id="Column1">
                <h1>About Us</h1><br />
                <p class="aboutTxt">AdPlanit is the creation of a team of scientists and marketing executives whose goal is to help small and mid-sized businesses advertise more effectively and confidently.   </p>

<p class="aboutTxt">We have been providing advanced marketing and advertising tools to the biggest brands and advertising companies in the world.</p>

<p class="aboutTxt">We’ve used our proven technology and experience to create the world’s first environment to test advertising with a virtual population of consumers.</p>

<p class="aboutTxt">AdPlanit brings the power of these “big company” tools and technology to all businesses.</p>

<p class="aboutTxt">At AdPlanit we are committed to respecting your time, resources and privacy. Our goal is to provide results that are easy to understand, implement, and maximize the return on your advertising spending. </p>

<p class="aboutTxt">Our revenue is derived from arrangements with media providers, but the choice of who to use and how to buy is yours.</p>
<br /><br />

                </td>
                <td id="Column2">
                <div id="Col2TopTopDiv">
                </div>
                <div id="TopImageDiv">
                    <img src="images/People-About.gif" />
                </div>
                <div id="Col2TopDiv">
                <p class="quotehdr">About the Company</p>

<p class="quote">AdPlanit is a product of <a href="http://www.decisionpower.com" target="_blank" >DecisionPower, Inc.</a> , a privately held company located in Campbell, CA.</p>

<p class="quote">DecisionPower applies advanced modeling technology to decision making in marketing and advertising.</p>


                </div>
                <div id="Col2BottomDiv">
                <p class="subhd"><br /></p>


                </div>
                </td>
                </tr>
                </table>
                </td>
           </tr>
           

           
       </table>
       <table width="100%" cellpadding="0" cellspacing="0" id="FooterTable"><tr align="center"><td id="Footer"><%= WebLibrary.Utils.Footer %></td></tr></table>


       </div>      

    </div>


    </form>
</body>
</html>
