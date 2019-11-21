using MailDispatcher.Common;
using MonitorServices.Data;
using MonitorServices.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonitorServices.Services
{
    public class ValorizzazioneMagazziniService
    {
        public void ValorizzaMagazzini()
        {
            StringBuilder Messaggi = new StringBuilder();
            ValorizzaMagazziniDS ds = new ValorizzaMagazziniDS();
            using (ValorizzazioneMagazziniBusiness bMagazzino = new ValorizzazioneMagazziniBusiness())
            {
                try
                {
                    bMagazzino.FillSALDI_GEN(ds);
                    bMagazzino.FillUSR_LIS_VEN(ds);

                    foreach (ValorizzaMagazziniDS.SALDI_GENRow saldo in ds.SALDI_GEN)
                    {
                        decimal prezzo = 0;
                        ValorizzaMagazziniDS.USR_LIS_VENRow listino = ds.USR_LIS_VEN.Where(x => !x.IsIDMAGAZZNull() && x.IDMAGAZZ == saldo.IDMAGAZZ && x.VALIDITA <= DateTime.Today && x.FINEVALIDITA >= DateTime.Today).FirstOrDefault();
                        if (listino != null)
                        {
                            prezzo = listino.PREZZOUNI;
                        }

                        decimal valore = 0;
                        if (!saldo.IsQESINull())
                        {
                            valore = saldo.QESI * prezzo;
                        }

                        ValorizzaMagazziniDS.MONITOR_SALDI_GENRow nuovaRiga = ds.MONITOR_SALDI_GEN.NewMONITOR_SALDI_GENRow();
                        object[] array = nuovaRiga.ItemArray.Clone() as object[];
                        saldo.ItemArray.CopyTo(array, 4);
                        array[1] = DateTime.Today;
                        array[2] = prezzo;
                        array[3] = valore;
                        nuovaRiga.ItemArray = array;

                        ds.MONITOR_SALDI_GEN.AddMONITOR_SALDI_GENRow(nuovaRiga);
                    }

                    bMagazzino.UpdateMonitorDSTable(ds.MONITOR_SALDI_GEN.TableName, ds);
                }
                catch (Exception ex)
                {
                    bMagazzino.Rollback();
                    string messaggio = string.Format("ERRORE - in ValorizzazioneMagazziniService.ValorizzaMagazzini ");
                    Messaggi.AppendLine(messaggio);
                    messaggio = string.Format("ERRORE - Eccezione {0} ", ex.Message);
                    Messaggi.AppendLine(messaggio);

                }
            }
            if (Messaggi.Length > 0)
            {
                string oggetto = string.Format("Errore in valorizza magazzini");
                string corpo = Messaggi.ToString();

                decimal IDMAIL = MailDispatcherService.CreaEmail("NOTIFICHE", oggetto, corpo);
                MailDispatcherService.SottomettiEmail(IDMAIL);

            }
        }
    }
}
