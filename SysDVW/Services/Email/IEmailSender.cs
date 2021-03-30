﻿using System.Threading.Tasks;

namespace SysDVW.Services.Email
{
    public interface IEmailSender
    {
        void SendEmail(Message message);
        Task SendEmailAsync(Message message);
    }
}
