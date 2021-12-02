using Data.Models.Services;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Data.Services.Account
{
    /// <summary>
    /// Сервис для отправки Email сообщений
    /// </summary>
    public class EmailService
    {
        private static readonly Dictionary<ServiceMail, EmailAuth> EmailBoxes = new Dictionary<ServiceMail, EmailAuth>()
        {
            {
                ServiceMail.Yandex,
                new EmailAuth(){  Name= "INNO-ТИМ",  Email = "bilabteam@yandex.ru", Host = "smtp.yandex.ru" , 
                    Password = "672412AaAa" , Port= 465 }
            },
        };

        /// <summary>
        /// Отправка сообщения на Email
        /// </summary>
        /// <param name="email">Email адрес получателя</param>
        /// <param name="subject">Тема сообщения</param>
        /// <param name="message">Текст сообщения</param>
        /// <param name="box">Тип почтового ящика, который необходимо использовать для отправки Email</param>
        /// <returns></returns>
        public static async Task<ResultEmailService> SendEmailAsync(string email, string subject, string message, 
            ServiceMail box = ServiceMail.Yandex)
        {
            try
            {
                EmailAuth mail = EmailBoxes[box];

                // Создаем объект сообщения
                var emailMessage = new MimeMessage();

                // Отправитель
                emailMessage.From.Add(new MailboxAddress(mail.Name, mail.Email));

                // Кому отправляем
                emailMessage.To.Add(new MailboxAddress("", email));

                // Тема письма
                emailMessage.Subject = subject;

                // Текст письма
                emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
                {
                    Text = message
                };

                using var client = new SmtpClient();

                await client.ConnectAsync(mail.Host, mail.Port, mail.UseSSL);

                await client.AuthenticateAsync(mail.Email, mail.Password);

                await client.SendAsync(emailMessage);

                await client.DisconnectAsync(true);

                return new ResultEmailService(true);

            }
            catch (Exception ex)
            {
                return new ResultEmailService(false) { Exception = ex };
            }
        }

        /// <summary>
        /// Информация о почтовом ящике
        /// </summary>`
        private class EmailAuth
        {
            /// <summary>
            /// Имя отправителя
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// Почтв
            /// </summary>
            public string Email { get; set; }

            /// <summary>
            /// Пароль
            /// </summary>
            public string Password { get; set; }

            /// <summary>
            /// Хост
            /// </summary>
            public string Host { get; set; }

            /// <summary>
            /// Порт
            /// </summary>
            public int Port { get; set; }


            /// <summary>
            /// Использование SSL 
            /// </summary>
            public SecureSocketOptions UseSSL { get; set; } = SecureSocketOptions.Auto;
        }
    }

    /// <summary>
    /// Типы почтовых ящиков
    /// </summary>
    public enum ServiceMail
    {
        /// <summary>
        /// Почтовый ящик - Яндекс
        /// </summary>
        Yandex,
    }
}