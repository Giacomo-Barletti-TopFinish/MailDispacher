using MailDispacher.Entities;
using MailDispatcher.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace MailDispatcher.Services
{
    public class MailDispatcherService
    {
        public void SendMail()
        {
            MailDispatcherDS ds = new MailDispatcherDS();
            using (MailDispatcherBusiness bMD = new MailDispatcherBusiness())
            {
                bMD.FillMD_EMAILByStato(MD_EMAIL_STATO.DA_INVIARE, ds);
                bMD.InsertMD_LOG(string.Format("Ci sono {0} mail da inviare", ds.MD_EMAIL.Rows.Count));

                if (ds.MD_EMAIL.Rows.Count == 0) return;

                bMD.FillMD_GRUPPI(ds);
                bMD.FillMD_RICHIEDENTI(ds);
                bMD.FillMD_GRUPPI_RICHIEDENTI(ds);
                bMD.FillMD_GRUPPI_DESTINATARI(ds);


                List<decimal> idmail = ds.MD_EMAIL.Select(x => x.IDMAIL).Distinct().ToList();

                bMD.FillMD_ALLEGATI(ds, idmail);


                foreach (MailDispatcherDS.MD_EMAILRow mail in ds.MD_EMAIL)
                {
                    try
                    {
                        decimal richiedente = mail.IDRICHIEDENTE;

                        List<string> destintinatariMail = (from destinatari in ds.MD_GRUPPI_DESTINATARI
                                                           join gruppi in ds.MD_GRUPPI_DESTINATARI on destinatari.IDGRUPPO equals gruppi.IDGRUPPO
                                                           join richiedenti in ds.MD_GRUPPI_RICHIEDENTI on gruppi.IDGRUPPO equals richiedenti.IDGRUPPO
                                                           where richiedenti.IDRICHIEDENTE == richiedente
                                                           select destinatari.DESTINATARIO).ToList();

                        MailSender sender = new MailSender();
                        string oggetto = string.Format("MAIL:{0} - {1}", mail.IDMAIL, mail.OGGETTO);
                        sender.CreaEmail(destintinatariMail, oggetto);

                        StringBuilder sb = new StringBuilder();
                        sb.AppendLine("MAIL AUTOMATICA DA MAIL DISPATCHER - NON RISPONDERE");
                        sb.AppendLine(string.Format("MAIL: {0}", mail.IDMAIL));
                        sb.AppendLine();
                        sb.AppendLine(mail.CORPO);

                        sender.AggiungiCorpoAllaMail(sb.ToString());

                        string corpo = string.Format("MAIL AUTOMAT");

                        sender.InviaMail();

                        mail.STATO = MD_EMAIL_STATO.INVIATA;
                        mail.DATAINVIO = DateTime.Now;
                        bMD.UpdateMailDispatcherDSTable(ds.MD_EMAIL.TableName, ds);
                        bMD.InsertMD_LOG(mail.IDMAIL, "INVIATA", string.Empty);
                    }
                    catch (Exception ex)
                    {
                        decimal tentativo = mail.IsTENTATIVONull() ? 0 : mail.TENTATIVO;
                        if (tentativo < 3)
                        {
                            mail.STATO = MD_EMAIL_STATO.DA_INVIARE;
                            mail.TENTATIVO = tentativo + 1;
                            bMD.UpdateMailDispatcherDSTable(ds.MD_EMAIL.TableName, ds);
                        }
                        else
                        {
                            mail.STATO = MD_EMAIL_STATO.DA_INVIARE;
                            mail.TENTATIVO = tentativo + 1;
                            bMD.UpdateMailDispatcherDSTable(ds.MD_EMAIL.TableName, ds);
                        }

                        StringBuilder exstr = new StringBuilder();
                        exstr.AppendLine(string.Format("Eccezione {0}", ex.Message));
                        exstr.AppendLine(string.Format("Stack {0}", ex.StackTrace));
                        while (ex.InnerException != null)
                        {
                            exstr.AppendLine(string.Format("Eccezione {0}", ex.InnerException.Message));
                            exstr.AppendLine(string.Format("Stack {0}", ex.InnerException.StackTrace));
                            ex = ex.InnerException;
                        }
                        if (exstr.Length > 200)
                            exstr.Remove(199, (exstr.Length - 200));

                        bMD.InsertMD_LOG(mail.IDMAIL, "ERRORE", exstr.ToString());
                    }
                }

            }
        }
    }
}
