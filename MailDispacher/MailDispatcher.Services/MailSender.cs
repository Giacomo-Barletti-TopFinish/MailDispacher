using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Net;
using System.IO;

namespace MailDispatcher.Services
{
    public class MailSender
    {
        private string _from;
        private string _host;
        private int _port;
        private string _senderCredential;
        private string _senderPassword;
        private MailMessage _mail;
        public MailSender()
        {
            _from = ConfigurationManager.AppSettings["FromMailAddress"];
            _host = ConfigurationManager.AppSettings["Host"];
            _port = Int32.Parse(ConfigurationManager.AppSettings["Port"]);
            _senderCredential = ConfigurationManager.AppSettings["SenderCredential"];
            _senderPassword = ConfigurationManager.AppSettings["SenderPassword"];

        }

        public void CreaEmail(List<string> Destinatari, string Oggetto)
        {

            _mail = new MailMessage();
            _mail.From = new MailAddress(_from);

            foreach (string destinatario in Destinatari)
                _mail.To.Add(new MailAddress(destinatario));

            _mail.Subject = Oggetto;
        }

        public void AggiungiCorpoAllaMail(string corpo)
        {
            _mail.Body = corpo;
        }

        public void AggiungiAllegato(Stream stream, string filename)
        {
            Attachment at = new Attachment(stream, filename);
            _mail.Attachments.Add(at);
        }

        public void InviaMail()
        {
            SmtpClient smtp = new SmtpClient();
            smtp.Host = _host;
            smtp.Port = _port;

            smtp.Credentials = new NetworkCredential(_senderCredential, _senderPassword);
            smtp.EnableSsl = true;

            smtp.Send(_mail);
        }

        public bool VerificaInvioMail()
        {
            if (_mail.DeliveryNotificationOptions == DeliveryNotificationOptions.OnSuccess)
                return true;
            return false;

        }
    }
}
