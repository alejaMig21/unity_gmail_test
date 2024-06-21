using MailKit;
using MailKit.Net.Imap;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace Emails.Imap.Receiver
{
    public static class AsyncEmailReceiver
    {
        #region FIELDS
        private static string imapHost;
        private static int imapPort;
        private static string receiverEmail;
        private static string receiverPassword;
        private static string senderEmail;
        #endregion

        #region METHODS
        static AsyncEmailReceiver() { }
        public static void LoadEmailSettings(string sender_email, string receiver_email, string receiver_password, string imap_host, string imap_port)
        {
            senderEmail = sender_email;
            receiverEmail = receiver_email;
            receiverPassword = receiver_password;
            imapHost = imap_host;
            imapPort = int.Parse(imap_port);
        }
        public static async Task<List<MimeMessage>> GetAllMessagesAsync()
        {
            List<MimeMessage> messages = new();

            try
            {
                using var client = new ImapClient();
                await client.ConnectAsync(imapHost, imapPort, true);
                client.AuthenticationMechanisms.Remove("XOAUTH2");
                await client.AuthenticateAsync(receiverEmail, receiverPassword);

                var inbox = client.Inbox;
                await inbox.OpenAsync(MailKit.FolderAccess.ReadOnly);

                for (int i = 0; i < inbox.Count; i++)
                {
                    var message = await inbox.GetMessageAsync(i);

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
                using (var client = new ImapClient())
                {
                    await client.ConnectAsync(imapHost, imapPort, true);
                    client.AuthenticationMechanisms.Remove("XOAUTH2");
                    await client.AuthenticateAsync(receiverEmail, receiverPassword);

                    var inbox = client.Inbox;
                    await inbox.OpenAsync(MailKit.FolderAccess.ReadOnly);

                    if (inbox.Count > 0)
                    {
                        if (string.IsNullOrEmpty(sender))
                        {
                            // No sender specified, return the latest message
                            latestMessage = await inbox.GetMessageAsync(inbox.Count - 1);
                        }
                        else
                        {
                            latestMessage = await LatestMessageFromSender(sender, inbox);
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
        private static async Task<MimeMessage> LatestMessageFromSender(string sender, IMailFolder inbox)
        {
            MimeMessage currentMessage = null;
            // Sender specified, search for the latest message from that sender
            for (int i = inbox.Count - 1; i >= 0; i--)
            {
                currentMessage = await inbox.GetMessageAsync(i);
                if (IsEmailFrom(currentMessage, sender))
                {
                    break;
                }
            }

            if (currentMessage == null)
            {
                Debug.Log($"No emails found from sender: {sender}");
            }

            return currentMessage;
        }
        private static bool IsEmailFrom(MimeMessage message, string sender)
        {
            if (message.From.Mailboxes.Any(m => m.Address == sender))
            {
                return true;
            }
            return false;
        }
        #endregion
    }
}