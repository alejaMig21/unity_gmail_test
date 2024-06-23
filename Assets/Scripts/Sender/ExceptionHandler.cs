using MailKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using System;
using System.Collections.Generic;
using System.IO;

namespace Emails.Emailer.ExeptionHandler
{
    public static class ExceptionHandler
    {
        public static void ExceptionHandeling(Exception ex)
        {
            switch (ex)
            {
                case ArgumentNullException:
                    SendErrorNotificationInfo("Null Values", ex.Message);
                    break;
                case ServiceNotConnectedException:
                    SendErrorNotificationInfo("SMTP Client not connected", ex.Message);
                    break;
                case InvalidOperationException:
                    SendErrorNotificationInfo("SMTP Client already authenticated", ex.Message);
                    break;
                case NotSupportedException:
                    SendErrorNotificationInfo("Authentication not supported", ex.Message);
                    break;
                case OperationCanceledException:
                    SendErrorNotificationInfo("Operation canceled via token", ex.Message);
                    break;
                case SaslException:
                    SendErrorNotificationInfo("SASL authentication error", ex.Message);
                    break;
                case AuthenticationException:
                    SendErrorNotificationInfo("Authentication error", ex.Message);
                    break;
                case IOException:
                    SendErrorNotificationInfo("I/O error", ex.Message);
                    break;
                case SmtpCommandException smtpEx:
                    if (smtpEx.Message.Contains("5.7.8"))
                    {
                        SendErrorNotificationInfo("Username and Password not accepted", smtpEx.Message);
                    }
                    else
                    {
                        SendErrorNotificationInfo("SMTP command failed", smtpEx.Message);
                    }
                    break;
                case SmtpProtocolException:
                    SendErrorNotificationInfo("SMTP protocol error", ex.Message);
                    break;
                case TimeoutException:
                    SendErrorNotificationInfo("Connection timeout", ex.Message);
                    break;
                case UnauthorizedAccessException:
                    SendErrorNotificationInfo("Unauthorized access", ex.Message);
                    break;
                default:
                    SendErrorNotificationInfo("Unknown error", ex.Message);
                    break;
            }
        }
        private static void SendErrorNotificationInfo(string message, string statusCode)
        {
            List<(string name, string body)> texts = new();
            texts.Add(("Message:", message));
            texts.Add(("StatusCode:", statusCode));
            AnimatedNotificationManager.Send(texts, 1);
        }
    }
}
