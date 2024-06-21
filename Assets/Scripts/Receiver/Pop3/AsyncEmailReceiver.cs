using MailKit.Net.Pop3;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace Emails.Pop3.Receiver
{
    public static class AsyncEmailReceiver
    {
        #region FIELDS
        private static string pop3Host;
        private static int pop3Port;
        private static string receiverEmail;
        private static string receiverPassword;
        private static string senderEmail;
        #endregion

        #region METHODS
        static AsyncEmailReceiver()
        {
            //LoadEmailSettings();
        }
        //public static void LoadEmailSettings()
        //{
        //    pop3Host = EnvLoader.GetEnvVariable("POP3_HOST");
        //    pop3Port = int.Parse(EnvLoader.GetEnvVariable("POP3_PORT"));
        //    receiverEmail = EnvLoader.GetEnvVariable("SENDER_EMAIL");
        //    receiverPassword = EnvLoader.GetEnvVariable("SENDER_PASSWORD");
        //    senderEmail = EnvLoader.GetEnvVariable("RECEIVER_EMAIL");
        //}
        public static void LoadEmailSettings(string sender_email, string receiver_email, string receiver_password, string pop3_host, string pop3_port)
        {
            senderEmail = sender_email;
            receiverEmail = receiver_email;
            receiverPassword = receiver_password;
            pop3Host = pop3_host;
            pop3Port = int.Parse(pop3_port);
        }
        public static async Task<List<MimeMessage>> GetAllMessagesAsync()
        {
            List<MimeMessage> messages = new();

            try
            {
                using var client = new Pop3Client();
                await client.ConnectAsync(pop3Host, pop3Port, true);
                client.AuthenticationMechanisms.Remove("XOAUTH2");
                await client.AuthenticateAsync(receiverEmail, receiverPassword);

                for (int i = 0; i < client.Count; i++)
                {
                    var message = await client.GetMessageAsync(i);

                    if (senderEmail != string.Empty)
                    {
                        if (!IsEmailFrom(message, senderEmail))
                        {
                            continue;
                        }
                    }

                    messages.Add(message);
                }

                client.Disconnect(true);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }

            return messages;
        }
        public static async Task<MimeMessage> ReceiveLatestEmailAsync(string sender = "")
        {
            MimeMessage latestMessage = null;

            try
            {
                using (var client = new Pop3Client())
                {
                    await client.ConnectAsync(pop3Host, pop3Port, true);
                    client.AuthenticationMechanisms.Remove("XOAUTH2");
                    await client.AuthenticateAsync(receiverEmail, receiverPassword);

                    int availableMessageCount = client.Count;

                    if (availableMessageCount > 0)
                    {
                        latestMessage = await client.GetMessageAsync(availableMessageCount - 1);

                        if (sender != string.Empty)
                        {
                            IsEmailFrom(latestMessage, senderEmail);
                        }
                    }
                    else
                    {
                        Debug.Log("No emails found.");
                    }

                    client.Disconnect(true);
                }
            }
            catch (Exception ex)
            {
                Debug.LogError("An error occurred while checking for emails: " + ex.Message);
            }

            return latestMessage;
        }
        private static bool IsEmailFrom(MimeMessage message, string specificSender)
        {
            if (message.From.Mailboxes.Any(m => m.Address == specificSender))
            {
                Debug.Log("Current emailToCheck from " + specificSender + " found: Subject: " + message.Subject + ", Body: " + message.TextBody);
                return true;
            }

            Debug.Log("No new emailToCheck from specific sender.");
            return false;
        }
        #endregion
    }
}