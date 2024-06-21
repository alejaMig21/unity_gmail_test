using TMPro;
using UnityEngine;

namespace Emails.Emailer.Sender.UI
{
    public class UIEmailSender : EmailSender
    {
        #region FIELDS
        [SerializeField]
        private TMP_InputField tmp_host = null;
        [SerializeField]
        private TMP_InputField tmp_port = null;
        [SerializeField]
        private TMP_InputField tmp_sender_email = null;
        [SerializeField]
        private TMP_InputField tmp_password = null;
        [SerializeField]
        private TMP_InputField tmp_subject = null;
        [SerializeField]
        private TMP_InputField tmp_body = null;
        [SerializeField]
        private TMP_InputField tmp_receiver_email = null;
        #endregion

        #region METHODS
        public void SetUpValues(TMP_InputField tmp_host, TMP_InputField tmp_port, TMP_InputField tmp_sender_email, TMP_InputField tmp_password, TMP_InputField tmp_subject, TMP_InputField tmp_body, TMP_InputField tmp_receiver_email)
        {
            this.tmp_host = tmp_host;
            this.tmp_port = tmp_port;
            this.tmp_sender_email = tmp_sender_email;
            this.tmp_password = tmp_password;
            this.tmp_subject = tmp_subject;
            this.tmp_body = tmp_body;
            this.tmp_receiver_email = tmp_receiver_email;

            base.SetUpValues(tmp_host.text, tmp_port.text, tmp_sender_email.text, tmp_password.text, tmp_subject.text, tmp_body.text, tmp_receiver_email.text);
        }
        public override void SendEmail()
        {
            SetUpValues(tmp_host, tmp_port, tmp_sender_email, tmp_password, tmp_subject, tmp_body, tmp_receiver_email);

            base.SendEmail();
        }
        #endregion
    }
}