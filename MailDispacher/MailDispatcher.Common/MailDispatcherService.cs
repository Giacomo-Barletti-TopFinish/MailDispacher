using MailDispacher.Entities;
using MailDispatcher.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MailDispatcher.Common
{
    public class MailDispatcherService
    {
        public static decimal CreaEmail(string Richiedente, string oggetto, string corpo)
        {
            MailDispatcherDS ds = new MailDispatcherDS();
            using (MailDispatcherBusiness bMD = new MailDispatcherBusiness())
            {
                bMD.FillMD_RICHIEDENTI(ds);
                MailDispatcherDS.MD_RICHIEDENTIRow richiedente = ds.MD_RICHIEDENTI.Where(x => x.RICHIEDENTE.Trim().ToUpper() == Richiedente.Trim().ToUpper()).FirstOrDefault();
                if (richiedente == null)
                    return -1;

                decimal IDMAIL = bMD.CreaMail(richiedente.IDRICHIEDENTE, oggetto, corpo);

                return IDMAIL;
            }
        }

        public static void AggiungiAllegato(decimal IDMAIL, string filename, MemoryStream ms)
        {
            using (MailDispatcherBusiness bMD = new MailDispatcherBusiness())
            {
                bMD.AggiungiAllegato(IDMAIL, filename, ms);
            }
        }

        public static void SottomettiEmail(decimal IDMAIL)
        {
            using (MailDispatcherBusiness bMD = new MailDispatcherBusiness())
            {
                bMD.SottomettiEmail(IDMAIL);
            }
        }
    }
}
