using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

namespace Emails.Emailer
{
    public static class AsyncEmailer
    {
        #region FIELDS
        private static string senderEmail;
        private static string receiverEmail;
        private static string senderPassword;
        private static string smtpHost;
        private static int smtpPort;

        private static string messageSubject;
        private static string messageBody;
        #endregion

        #region METHODS
        static AsyncEmailer()
        {
            //LoadEmailSettings();
        }
        //public static void LoadEmailSettings()
        //{
        //    senderEmail = EnvLoader.GetEnvVariable("SENDER_EMAIL");
        //    receiverEmail = EnvLoader.GetEnvVariable("RECEIVER_EMAIL");
        //    senderPassword = EnvLoader.GetEnvVariable("SENDER_PASSWORD");
        //    smtpHost = EnvLoader.GetEnvVariable("SMTP_HOST");
        //    smtpPort = int.Parse(EnvLoader.GetEnvVariable("SMTP_PORT"));
        //}
        public static void LoadEmailSettings(string sender_email, string receiver_email, string sender_password, string smtp_host, string smtp_port)
        {
            senderEmail = sender_email;
            receiverEmail = receiver_email;
            senderPassword = sender_password;
            smtpHost = smtp_host;
            smtpPort = int.Parse(smtp_port);
        }
        public static async Task SendEmailAsync(string subject, string body, List<string> attachments = null)
        {
            var message = CreateEmailMessage(subject, body, attachments);
            await SendEmailMessageAsync(message);
        }
        private static MimeMessage CreateEmailMessage(string subject, string body, List<string> attachments)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Game Character", senderEmail));
            message.To.Add(new MailboxAddress("User Receiver", receiverEmail));
            message.Subject = subject;
            messageSubject = subject;

            var multipartBody = new Multipart("mixed");
            {
                CreateTextPart(body, multipartBody);
                CreateAttachmentsPart(attachments, multipartBody);
            }
            message.Body = multipartBody;

            return message;
        }
        private static void CreateTextPart(string body, Multipart multipartBody)
        {
            var textPart = new TextPart("plain")
            {
                Text = body
            };
            multipartBody.Add(textPart);
            messageBody = body;
        }
        private static void CreateAttachmentsPart(List<string> attachments, Multipart multipartBody)
        {
            if (attachments != null)
            {
                foreach (var attachmentPath in attachments)
                {
                    var attachmentPart = new MimePart(MimeTypes.GetMimeType(attachmentPath))
                    {
                        Content = new MimeContent(File.OpenRead(attachmentPath), ContentEncoding.Default),
                        ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
                        ContentTransferEncoding = ContentEncoding.Base64,
                        FileName = Path.GetFileName(attachmentPath)
                    };
                    multipartBody.Add(attachmentPart);
                }
            }
        }
        private static async Task SendEmailMessageAsync(MimeMessage message)
        {
            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(smtpHost, smtpPort, SecureSocketOptions.StartTls);
                client.AuthenticationMechanisms.Remove("XOAUTH2");
                await client.AuthenticateAsync(senderEmail, senderPassword);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
                Debug.Log("Sent emailToCheck asynchronously!");

                SendNotificationInfo();
            }
        }
        private static void SendNotificationInfo()
        {
            List<(string name, string body, Color color)> texts = new();
            texts.Add(("From:", senderEmail, Color.white));
            texts.Add(("Subject:", messageSubject, Color.white));
            texts.Add(("Body:", messageBody, Color.white));
            AnimatedNotificationManager.Send(texts);
        }
        #endregion
    }
}