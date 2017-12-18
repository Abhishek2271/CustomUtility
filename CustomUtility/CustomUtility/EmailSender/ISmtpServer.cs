using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CustomUtility.EmailSender
{
    public interface ISmtpServer
    {
        string Server { get; }
        int Port { get; }
        bool RequireAuthentication { get; }
        bool RequireSSL { get; }
        string Username { get; }
        string Password { get; }
    }
}
