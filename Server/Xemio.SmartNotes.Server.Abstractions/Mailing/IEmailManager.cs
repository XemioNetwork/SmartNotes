﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xemio.SmartNotes.Server.Abstractions.Mailing
{
    public interface IEmailManager
    {
        /// <summary>
        /// Ensures the email templates exist.
        /// </summary>
        void EnsureTemplatesExist();
        /// <summary>
        /// Starts to send emails.
        /// </summary>
        void StartSendingEmails();
        /// <summary>
        /// Stops sending the emails.
        /// </summary>
        void Stop();
    }
}
