using System.Collections.Generic;
using UnityEngine;

namespace Emails.Emailer.Sender
{
    public class EmailSender : MonoBehaviour
    {
        #region FIELDS
        [SerializeField]
        private string host = string.Empty;
        [SerializeField]
        private string port = string.Empty;
        [SerializeField]
        private string sender_email = string.Empty;
        [SerializeField]
        private string password = string.Empty;
        [SerializeField]
        private string subject = string.Empty;
        [SerializeField]
        private string body = string.Empty;
        [SerializeField]
        private string receiver_email = string.Empty;
        #endregion

        #region METHODS
        public void SetUpValues(string host, string port, string sender_email, string password, string subject, string body, string receiver_email)
        {
            this.host = host;
            this.port = port;
            this.sender_email = sender_email;
            this.password = password;
            this.subject = subject;
            this.body = body;
            this.receiver_email = receiver_email;
        }
        //private void SetUpEnvVars(string sender_email, string password, string receiver_email)
        //{
        //    EnvLoader.SetEnvVariable("SENDER_EMAIL", sender_email);
        //    EnvLoader.SetEnvVariable("SENDER_PASSWORD", password);
        //    EnvLoader.SetEnvVariable("RECEIVER_EMAIL", receiver_email);
        //}
        public async virtual void SendEmail()
        {
            // Cargar el .env si no se ha cargado
            //EnvLoader.GetEnvVariable("SENDER_EMAIL"); // Esto asegura que el archivo .env esté cargado
            //SetUpEnvVars(sender_email, password, receiver_email);

            AsyncEmailer.LoadEmailSettings(sender_email, receiver_email, password, host, port);

            // Enviar un correo electrónico
            List<string> attachments = new()
            {
                "C:/Users/aleja/OneDrive/Pictures/neuron_activation.jpg"
            };

            Debug.Log(attachments[0]);

            //Emailer.SendEmail(subject, body, new());
            await AsyncEmailer.SendEmailAsync(subject, body, attachments);
        }
        #endregion
    }
}