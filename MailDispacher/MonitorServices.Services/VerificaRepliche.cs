using MailDispatcher.Common;
using MonitorServices.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;


namespace MonitorServices.Services
{
    public class VerificaRepliche
    {
        StringBuilder Messaggi = new StringBuilder();
        public void VerificaReplicheCartelleServer(string sorgente, string destinazione, string user, string password, string dominio)
        {
            try
            {
                sorgente = sorgente.ToUpper();
                destinazione = destinazione.ToUpper();

                System.Net.NetworkCredential readCredentials = new NetworkCredential(user, password, dominio);
                using (new NetworkConnection(sorgente, readCredentials))
                using (new NetworkConnection(destinazione, readCredentials))
                {
                    DirectoryInfo diSorgente = new DirectoryInfo(sorgente);
                    VerificaCartella(diSorgente, sorgente, destinazione);
                }
            }
            catch (Exception ex)
            {
                string messaggio = string.Format("ERRORE - in VerificaReplicheCartelleServer ");
                Messaggi.AppendLine(messaggio);
                messaggio = string.Format("ERRORE - Eccezione {0} SORGENTE:{1} DESTINAZIONE:{2}", ex.Message, sorgente, destinazione);
                Messaggi.AppendLine(messaggio);

            }

            if (Messaggi.Length > 0)
            {
                string oggetto = string.Format("Verifica repliche {0} Sorgente:{1}", DateTime.Today.ToShortDateString(), sorgente);
                string corpo = Messaggi.ToString();

                decimal IDMAIL = MailDispatcherService.CreaEmail("VERIFICA REPLICHE", oggetto, corpo);
                MailDispatcherService.SottomettiEmail(IDMAIL);

            }
        }

        public void VerificaCartella(DirectoryInfo cartella, string sorgente, string destinazione)
        {
            try
            {
                FileInfo[] filesDaVerificare = cartella.GetFiles();

                foreach (FileInfo file in filesDaVerificare)
                {
                    try
                    {
                        bool esito = VerificaFile(file, sorgente, destinazione);
                        if (!esito)
                        {
                            string messaggio = string.Format("ATTENZIONE - File {0} non trovato in {1}", file, destinazione);
                            Messaggi.AppendLine(messaggio);
                        }
                    }
                    catch (Exception ex1)
                    {
                        string messaggio = string.Format("ERRORE VERIFICA FILE - Cartella {0} File {1}", cartella.FullName, file.FullName);
                        Messaggi.AppendLine(messaggio);
                        messaggio = string.Format("ERRORE - Eccezione {0} ", ex1.Message);
                        Messaggi.AppendLine(messaggio);
                    }
                }

                DirectoryInfo[] cartelleDaVerificare = cartella.GetDirectories();
                foreach (DirectoryInfo cartellaDaVerificare in cartelleDaVerificare)
                {
                    VerificaCartella(cartellaDaVerificare, sorgente, destinazione);
                }
            }
            catch (Exception ex)
            {
                string messaggio = string.Format("ERRORE VERIFICA CARTELLE - Cartella {0} ", cartella.FullName);
                Messaggi.Append(messaggio);
                messaggio = string.Format("ERRORE - Eccezione {0} ", ex.Message);
                Messaggi.Append(messaggio);

            }
        }


        public bool VerificaFile(FileInfo file, string sorgente, string destinazione)
        {
            string fileDaVerificare = file.FullName.Replace(sorgente, destinazione);
            bool esito = File.Exists(fileDaVerificare);
            if (!esito)
                file.CopyTo(fileDaVerificare);

            return esito;

        }
    }
}
