using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CustomUtility.EmailSender
{
    public interface IEmailAddress
    {
        string Name { get; }
        string Email { get; }
    }
}
