using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebLibrary
{
    public class EmailSender
    {
        private const string sendAgent = "support@adplanit.com";
        private const string sendAgentPassword = "supp0708";
        private const string sendServer = "mail.adplanit.com";
        private const int sendServerPort = 25;

        public EmailSender() {
        }

        /// <summary>
        /// Sends an email to a single recipient.
        /// </summary>
        /// <param name="toAddress"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        public string SendMessage( string toAddress, string subject, string body ) {
            System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage( sendAgent, toAddress );

            return DoSendMessage( message, subject, body );
        }

        /// <summary>
        /// Sends an email to a list of recipients.
        /// </summary>
        /// <param name="toAddresses"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        public string SendMessage( List<string> toAddresses, string subject, string body ) {
            System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage( sendAgent, "dummy@dummy.com" );
            message.To.Clear();
            for( int i = 0; i < toAddresses.Count; i++ ) {
                message.To.Add( toAddresses[ i ] );
            }
            
            return DoSendMessage( message, subject, body );
        }

        private string DoSendMessage(  System.Net.Mail.MailMessage message, string subject, string body ){

            string rval = null;

            message.Subject = subject;
            message.Body = body;

            // do the criitcal setup stuff
            System.Net.Mail.SmtpClient mailClient = new System.Net.Mail.SmtpClient( sendServer, sendServerPort );
            // mailClient.EnableSsl = true;
            mailClient.Credentials = new System.Net.NetworkCredential( sendAgent, sendAgentPassword );    // set user credentials!

            // send the message!
            try
            {
                mailClient.Send( message );
            }
            catch( Exception e )
            {
                rval = e.Message;
            }

            return rval; ;
        }
    }
}
