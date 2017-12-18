using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CustomUtility.EmailSender
{
    public class EmailAddress : IEmailAddress
    {
        private string _name = string.Empty, _emailaddress = string.Empty;

        public EmailAddress(string name, string emailaddress)
        {
            _name = name;
            _emailaddress = emailaddress;
        }

        public System.Net.Mail.MailAddress GetMailAddress()
        {
            return new System.Net.Mail.MailAddress(_emailaddress, _name, Encoding.UTF8);
        }

        #region IEmailAddress Members

        public string Name
        {
            get { return _name; }
        }

        public string Email
        {
            get { return _emailaddress; }
        }

        #endregion
    }
}
