using MimeKit;
using System.Collections.Generic;
using UnityEngine;

namespace Emails.Imap.Receiver.Checker
{
    public class EmailChecker : MonoBehaviour
    {
        [SerializeField]
        private string host = string.Empty;
        [SerializeField]
        private string port = string.Empty;
        [SerializeField]
        private string emailToCheck = string.Empty;
        [SerializeField]
        private string password = string.Empty;
        [SerializeField]
        private string remitent = string.Empty;

        public void SetUpValues(string host, string port, string emailToCheck, string password, string remitent)
        {
            this.host = host;
            this.port = port;
            this.emailToCheck = emailToCheck;
            this.password = password;
            this.remitent = remitent;
        }
        //private void SetUpEnvVars(string email, string password)
        //{
        //    EnvLoader.SetEnvVariable("SENDER_EMAIL", email);
        //    EnvLoader.SetEnvVariable("SENDER_PASSWORD", password);
        //}
        public async virtual void CheckLatestEmail()
        {
            //SetUpEnvVars(emailToCheck, password);

            AsyncEmailReceiver.LoadEmailSettings(remitent, emailToCheck, password, host, port);

            // Verificar correos nuevos
            MimeMessage newMessage = await AsyncEmailReceiver.ReceiveLatestEmailAsync(remitent);

            if (newMessage != null)
            {
                Debug.Log("From: " + newMessage.From);
                Debug.Log("MessageID: " + newMessage.MessageId);
                Debug.Log("Subject: " + newMessage.Subject);
                Debug.Log("Body: " + newMessage.TextBody);
            }
        }
        public virtual async void CheckAllEmails()
        {
            //SetUpEnvVars(emailToCheck, password);

            AsyncEmailReceiver.LoadEmailSettings(remitent, emailToCheck, password, host, port);

            List<MimeMessage> messages = await AsyncEmailReceiver.GetAllMessagesAsync();

            for (int i = 0; i < messages.Count; i++)
            {
                MimeMessage message = messages[i];
                Debug.Log("Message No." + (i + 1));
                Debug.Log("From: " + message.From);
                Debug.Log("MessageID: " + message.MessageId);
                Debug.Log("Subject: " + message.Subject);
                Debug.Log("Body: " + message.TextBody);
            }
        }
    }
}