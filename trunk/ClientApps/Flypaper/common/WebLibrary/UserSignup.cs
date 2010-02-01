using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

namespace WebLibrary
{
    /// <summary>
    /// Summary description for UserSignup
    /// </summary>
    public class UserSignup
    {
        private string firstName;
        private string lastName;
        private string email;
        private string company;
        private string phone;
        private string companyURL;
        private string referralNotes;

        /// <summary>
        /// Creates a new object for signing up a new user
        /// </summary>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="email"></param>
        /// <param name="company"></param>
        /// <param name="companyURL"></param>
        /// <param name="referralNotes"></param>
        public UserSignup( string firstName, string lastName, string email, string company, string phone, string companyURL, string referralNotes ) {
            this.firstName = firstName;
            this.lastName = lastName;
            this.email = email;
            this.company = company;
            this.phone = phone;
            this.companyURL = companyURL;
            this.referralNotes = referralNotes;
        }

        /// <summary>
        /// Creates a new object for signing up a new user
        /// </summary>
        /// <param name="request"></param>
        public UserSignup( HttpRequest request )
            : this( request[ "first_name" ], request[ "last_name" ], request[ "email" ],
                request[ "company" ], request[ "phone" ], request[ "source" ], request[ "source2" ] ) {
        }

        /// <summary>
        /// Actually performs the new-user signup process.  Returns the password for the newly-created user.
        /// </summary>
        /// <returns></returns>
        public string DoSignup( out string userName, out string password ) {
            Random r = new Random();
            int rand = r.Next( 100, 999 );
            string pwHead = this.lastName;
            if( pwHead.Length > 3 ) {
                pwHead = pwHead.Substring( 0, 3 );
            }
            password = String.Format( "{0}{1}", pwHead, rand );
            userName = this.email;

            try {
                MembershipUser newUser = Membership.CreateUser( this.email, password, this.email );
            }
            catch( Exception e ) {
                return e.Message;
            }

            if( Membership.ValidateUser( this.email, password ) == true ) {
                return null;     // indicates no error
            }
            else {
                return "post-signup validation failed";
            }

        }
    }
}
