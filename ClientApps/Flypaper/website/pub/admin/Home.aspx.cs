using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

public partial class admin_Home : System.Web.UI.Page
{
    static RoleProvider rol = null;

    protected void Page_Load( object sender, EventArgs e )
    {
        if( ConfigurationManager.AppSettings["AdPlanIt.loginConnectionString"] != null )
        {
            this.UserDb.ConnectionString = ConfigurationManager.AppSettings["AdPlanIt.loginConnectionString"];
            this.LoginStats.ConnectionString = ConfigurationManager.AppSettings["AdPlanIt.loginConnectionString"];
        }

        if( !IsPostBack )
        {
            rol = Roles.Providers["SqlProvider"];

            foreach( string name in rol.GetAllRoles() )
            {
                ListItem item = new ListItem( name );
                this.RoleList.Items.Add( item );
            }
        }
    }

    protected void DeleteUserButton_Click( object sender, EventArgs e )
    {
        if( this.UserNameBox.Text != null || this.UserNameBox.Text != "" )
        {
            MembershipUser user = Membership.GetUser( this.UserNameBox.Text, false );

            if( user != null )
            {
                Membership.DeleteUser( user.UserName );

                this.UserNameBox.Text = null;

                UserDb.DataBind();

                this.UserView.DataBind();
            }
        }
    }

    protected void SubmitUserChangesButton_Click( object sender, EventArgs e )
    {
        if( this.UserView.SelectedRow != null )
        {
              TableCell cell = UserView.SelectedRow.Cells[0];

            MembershipUser user = Membership.GetUser( cell.Text, false );

            

            if( user != null )
            {
                // we need to remember this as accessing the profile changes it!
                // definately a kludge
                DateTime lad = user.LastActivityDate;

                ProfileCommon profile = Profile.GetProfile( user.UserName );

                // reset last activity date
                user.LastActivityDate = lad;
               
                if( FirstNameBox.Text != profile.UserMediaItems.FirstName )
                {
                    profile.UserMediaItems.FirstName =  FirstNameBox.Text;
                }

                if( LastNameBox.Text != profile.UserMediaItems.LastName )
                {
                    profile.UserMediaItems.LastName = LastNameBox.Text;
                }

                if( CompanyNameBox.Text != profile.UserMediaItems.CompanyName )
                {
                }

                if( CompanyURLBox.Text != profile.UserMediaItems.CompanyURL )
                {
                    profile.UserMediaItems.CompanyURL =  CompanyURLBox.Text;
                }

                if( PhoneBox.Text != profile.UserMediaItems.Phone )
                {
                    profile.UserMediaItems.Phone = PhoneBox.Text;
                }

                if( ReferralinfoBox.Text != profile.UserMediaItems.Referralnfo )
                {
                    profile.UserMediaItems.Referralnfo = ReferralinfoBox.Text;
                }

                if( PasswordBox.Text != user.GetPassword() )
                {
                    user.ChangePassword( user.GetPassword(), PasswordBox.Text );
                }

                profile.Save();
                Membership.UpdateUser( user );
            }
        }

    }

    protected void UserView_SelectedIndexChanged( object sender, EventArgs e )
    {
        if( this.UserView.SelectedRow != null )
        {
            TableCell cell = UserView.SelectedRow.Cells[0];

            this.UserNameBox.Text = cell.Text;

            MembershipUser user = Membership.GetUser( cell.Text, false );

            if( user != null )
            {
                this.UserNameBox.ForeColor = System.Drawing.Color.Black;

                // we need to remember this as accessing the profile changes it!
                // definately a kludge
                DateTime lad = user.LastActivityDate;

                ProfileCommon profile = Profile.GetProfile( user.UserName );

                // reset last activity date
                user.LastActivityDate = lad;
               

                FirstNameBox.Text = profile.UserMediaItems.FirstName;
                LastNameBox.Text = profile.UserMediaItems.LastName;
                CompanyNameBox.Text = profile.UserMediaItems.CompanyName;
                CompanyURLBox.Text = profile.UserMediaItems.CompanyURL;
                PhoneBox.Text = profile.UserMediaItems.Phone;
                ReferralinfoBox.Text = profile.UserMediaItems.Referralnfo;
                PasswordBox.Text = user.GetPassword();

                Membership.UpdateUser( user );

                FirstNameBox.ReadOnly = true;
                LastNameBox.ReadOnly = true;
                CompanyNameBox.ReadOnly = true;
                CompanyURLBox.ReadOnly = true;
                PhoneBox.ReadOnly = true;
                ReferralinfoBox.ReadOnly = true;
                PasswordBox.ReadOnly = true;

                SignUpBox.Text = user.CreationDate.ToShortDateString();

                SubmitUserChangesButton.Enabled = false;

                string[] roles =  rol.GetRolesForUser( user.UserName );

                if( roles.Length > 0 )
                {
                    ListItem item = RoleList.Items.FindByText( roles[0] );

                    RoleList.SelectedIndex = RoleList.Items.IndexOf( item );
                }
            }
            else
            {
                this.UserNameBox.ForeColor = System.Drawing.Color.Red;
            }
        }
    }

    protected void EditPasswordButton_Click( object sender, EventArgs e )
    {
        PasswordBox.ReadOnly = false;
        SubmitUserChangesButton.Enabled = true;
    }
    protected void EditInfoButton_Click( object sender, EventArgs e )
    {  
        CompanyNameBox.ReadOnly = false;
        CompanyURLBox.ReadOnly = false;
        PhoneBox.ReadOnly = false;
        ReferralinfoBox.ReadOnly = false;
        SubmitUserChangesButton.Enabled = true;

    }
    protected void editNameButton_Click( object sender, EventArgs e )
    {
        FirstNameBox.ReadOnly = false;
        LastNameBox.ReadOnly = false;
        SubmitUserChangesButton.Enabled = true;
    }
    protected void UserLogButton_Click( object sender, EventArgs e )
    {
        string username = "";

        if( this.UserView.SelectedRow != null )
        {
            TableCell cell = UserView.SelectedRow.Cells[0];

            username = cell.Text;
        }

        Response.Redirect( "LogCheck.aspx?user=" + username );

    }
    protected void ChangeType_Click( object sender, EventArgs e )
    {
        if( this.UserNameBox.Text != null || this.UserNameBox.Text != "" )
        {
            MembershipUser user = Membership.GetUser( this.UserNameBox.Text, false );

            if( user != null )
            {
                string[] userNames = new string[] { user.UserName };
                string[] newRoles = new string[] { RoleList.SelectedValue };

                string[] currentRoles = rol.GetRolesForUser( user.UserName );

                if( currentRoles.Length > 0 )
                {
                    rol.RemoveUsersFromRoles( userNames, currentRoles );
                }

                rol.AddUsersToRoles( userNames, newRoles );
            }
        }
    }
}
