using TMPro;
using UnityEngine;

namespace Emails.Imap.Receiver.Checker.UI
{
    public class UIEmailChecker : EmailChecker
    {
        [SerializeField]
        private TMP_InputField tmp_host = null;
        [SerializeField]
        private TMP_InputField tmp_port = null;
        [SerializeField]
        private TMP_InputField tmp_email_to_check = null;
        [SerializeField]
        private TMP_InputField tmp_password = null;
        [SerializeField]
        private TMP_InputField tmp_remitent = null;

        public void SetUpValues(TMP_InputField tmp_host, TMP_InputField tmp_port, TMP_InputField tmp_email, TMP_InputField tmp_password, TMP_InputField tmp_remitent)
        {
            base.SetUpValues(tmp_host.text, tmp_port.text, tmp_email.text, tmp_password.text, tmp_remitent.text);
        }
        public override void CheckLatestEmail()
        {
            SetUpValues(tmp_host, tmp_port, tmp_email_to_check, tmp_password, tmp_remitent);

            base.CheckLatestEmail();
        }
        public override void CheckAllEmails()
        {
            SetUpValues(tmp_host, tmp_port, tmp_email_to_check, tmp_password, tmp_remitent);

            base.CheckAllEmails();
        }
    }
}