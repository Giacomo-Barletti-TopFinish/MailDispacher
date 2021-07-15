using MailDispatcher.Common;
using MonitorServices.Data;
using MonitorServices.Entities;
using MonitorServices.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonitorServices.Services
{
    public class MagazzinoMonitor
    {
        public void VerificaSaldiNegativi()
        {
            MagazzinoDS ds = new MagazzinoDS();

            using (MagazzinoBusiness bMagazzino = new MagazzinoBusiness())
            {
                bMagazzino.FillMagazziniNegativi(ds);

                if (ds.MAGAZZINONEGATIVO.Count == 0) return;

                ExcelHelper excel = new ExcelHelper();
                byte[] file = excel.CreaExcelMagazziniNegativi(ds);

                string oggetto = string.Format("Magazzini negativi al giorno {0}", DateTime.Today.ToShortDateString());
                string corpo = "Dati in allegato";

                decimal IDMAIL = MailDispatcherService.CreaEmail("MONITOR MAGAZZINI NEGATIVI", oggetto, corpo);
                MailDispatcherService.AggiungiAllegato(IDMAIL, "MagazziniNegativi.xlsx", new System.IO.MemoryStream(file));
                MailDispatcherService.SottomettiEmail(IDMAIL);

            }
        }

        public void VerificaGiacenze()
        {
            MagazzinoDS ds = new MagazzinoDS();

            using (MagazzinoBusiness bMagazzino = new MagazzinoBusiness())
            {
                bMagazzino.FillMONITOR_APPROVVIGIONAMENTO(ds);
                bMagazzino.FillMagazziniGiacenza(ds);

                foreach (MagazzinoDS.MONITOR_APPROVVIGIONAMENTORow approvvigionamento in ds.MONITOR_APPROVVIGIONAMENTO.Where(x => x.DATASCADENZA > DateTime.Now))
                {
                    foreach (MagazzinoDS.MAGAZZINOGIACENZARow giacenza in ds.MAGAZZINOGIACENZA.Where(x => x.RowState != System.Data.DataRowState.Deleted && x.IDMAGAZZ == approvvigionamento.IDMAGAZZ))
                        giacenza.Delete();
                }

                ds.MAGAZZINOGIACENZA.AcceptChanges();

                if (ds.MAGAZZINOGIACENZA.Count == 0) return;

                ExcelHelper excel = new ExcelHelper();
                byte[] file = excel.CreaExcelMagazziniGiacenze(ds);

                string oggetto = string.Format("Giacenze magazzino al giorno {0}", DateTime.Today.ToShortDateString());
                string corpo = "Dati in allegato";

                decimal IDMAIL = MailDispatcherService.CreaEmail("MONITOR GIACENZE MAGAZZINI", oggetto, corpo);
                MailDispatcherService.AggiungiAllegato(IDMAIL, "GiacenzeMagazzini.xlsx", new System.IO.MemoryStream(file));
                MailDispatcherService.SottomettiEmail(IDMAIL);

            }
        }

        public void SaldiUbicazioni()
        {
            MagazzinoDS ds = new MagazzinoDS();

            using (MagazzinoBusiness bMagazzino = new MagazzinoBusiness())
            {
                bMagazzino.FillSALDIUBICAZIONI(ds);

                foreach (MagazzinoDS.SALDIUBICAZIONIRow saldo in ds.SALDIUBICAZIONI)
                {
                    if (saldo.IsCATEGORIANull())
                    {
                        saldo.Delete();
                        continue;
                    }
                    switch (saldo.CATEGORIA)
                    {
                        case "Lavorazione Esterna":
                        case "Prodotti Di Terzi":
                        case "PL45":
                            saldo.Delete();
                            break;

                    }
                }

                ds.SALDIUBICAZIONI.AcceptChanges();

                if (ds.SALDIUBICAZIONI.Count == 0) return;

                ExcelHelper excel = new ExcelHelper();
                byte[] file = excel.CreaExcelSaldiUbicazioni(ds);
                byte[] fileMovimenti = excel.CreaExcelMovimentiFiltrati(ds);

                string oggetto = string.Format("Saldi ubicazioni al giorno {0}", DateTime.Today.ToShortDateString());
                string corpo = "Dati in allegato";

                decimal IDMAIL = MailDispatcherService.CreaEmail("SALDI UBICAZIONI", oggetto, corpo);
                MailDispatcherService.AggiungiAllegato(IDMAIL, "SaldiUbicazioni.xlsx", new System.IO.MemoryStream(file));
                MailDispatcherService.AggiungiAllegato(IDMAIL, "Movimenti.xlsx", new System.IO.MemoryStream(fileMovimenti));
                MailDispatcherService.SottomettiEmail(IDMAIL);

            }
        }
        public void VerificaGiacenzeBrandManager()
        {
            MagazzinoDS ds = new MagazzinoDS();

            using (MagazzinoBusiness bMagazzino = new MagazzinoBusiness())
            {
                bMagazzino.FillGIACENZA_BRAND_MANAGER(ds);
                bMagazzino.FillUSR_INVENTARIOS(ds);

                foreach (MagazzinoDS.GIACENZA_BRAND_MANAGERRow elemento in ds.GIACENZA_BRAND_MANAGER)
                {
                    MagazzinoDS.USR_INVENTARIOSRow costo = ds.USR_INVENTARIOS.Where(x => x.METODOCOSTO1 != 0 && x.IDMAGAZZ == elemento.IDMAGAZZ).OrderBy(x => x.DATACR).FirstOrDefault();
                    if (costo != null)
                    {
                        elemento.COSTO = costo.COSTO1;
                        elemento.VALORE = costo.COSTO1 * elemento.QESI;
                        elemento.VALORE_DISP = costo.COSTO1 * elemento.QTOT_DISP_ESI;
                    }
                }


                ExcelHelper excel = new ExcelHelper();
                byte[] file = excel.CreaExcelMagazziniGiacenzeBrandManager(ds);

                string oggetto = string.Format("Giacenze Brand Manager al giorno {0}", DateTime.Today.ToShortDateString());
                string corpo = "Dati in allegato";

                decimal IDMAIL = MailDispatcherService.CreaEmail("MONITOR GIACENZE BRAND", oggetto, corpo);
                MailDispatcherService.AggiungiAllegato(IDMAIL, "GiacenzeBrandManager.xlsx", new System.IO.MemoryStream(file));
                MailDispatcherService.SottomettiEmail(IDMAIL);

            }
        }

        public void ScartiDifettosi()
        {
            MagazzinoDS ds = new MagazzinoDS();
            DateTime dataTermini = DateTime.Today.AddDays(-1);
            using (MagazzinoBusiness bMagazzino = new MagazzinoBusiness())
            {
                bMagazzino.FillSCARTIDIFETTOSI(dataTermini, ds);

                ExcelHelper excel = new ExcelHelper();
                byte[] file = excel.CreaExcelScartiDifettosi(ds);
                FileStream fs = new FileStream(@"c:\temp\ttt.xlsx", FileMode.Create);
                fs.Write(file, 0, file.Length);
                fs.Flush();
                fs.Close();
                string oggetto = string.Format("Scarti difettosi al giorno {0}", dataTermini.ToShortDateString());
                string corpo = "Dati in allegato";

                decimal IDMAIL = MailDispatcherService.CreaEmail("SCARTI DIFETTOSI", oggetto, corpo);
                MailDispatcherService.AggiungiAllegato(IDMAIL, "SCARTI DIFETTOSI.xlsx", new System.IO.MemoryStream(file));
                MailDispatcherService.SottomettiEmail(IDMAIL);

            }
        }

        public void ScartiGreco()
        {
            MagazzinoDS ds = new MagazzinoDS();
            DateTime dataTermini = DateTime.Today.AddDays(-1);
            using (MagazzinoBusiness bMagazzino = new MagazzinoBusiness())
            {
                bMagazzino.FillSCARTIGRECO(dataTermini, ds);

                ExcelHelper excel = new ExcelHelper();
                byte[] file = excel.CreaExcelScartiGreco(ds);
                string filename = string.Format(@"c:\temp\Scarti del {0}.{1}.{2}.xlsx", dataTermini.Day, dataTermini.Month, dataTermini.Year);
                FileStream fs = new FileStream(filename, FileMode.Create);
                fs.Write(file, 0, file.Length);
                fs.Flush();
                fs.Close();
                string oggetto = string.Format("Scarti difettosi al giorno {0}", dataTermini.ToShortDateString());
                string corpo = "Dati in allegato";

                decimal IDMAIL = MailDispatcherService.CreaEmail("SCARTIGRECO", oggetto, corpo);
                MailDispatcherService.AggiungiAllegato(IDMAIL, "SCARTI.xlsx", new System.IO.MemoryStream(file));
                MailDispatcherService.SottomettiEmail(IDMAIL);
                if (File.Exists(filename))
                    File.Delete(filename);
            }
        }

        public void EstrazioneOC()
        {
            MagazzinoDS ds = new MagazzinoDS();
            using (MagazzinoBusiness bMagazzino = new MagazzinoBusiness())
            {
                bMagazzino.FillESTRAZIONE_OC( ds);

                ExcelHelper excel = new ExcelHelper();
                byte[] file = excel.CreaExcelEstrazioneOC(ds);
                string filename = string.Format(@"c:\temp\Estrazione_OC del {0}.{1}.{2}.xlsx", DateTime.Today.Day, DateTime.Today.Month, DateTime.Today.Year);
                FileStream fs = new FileStream(filename, FileMode.Create);
                fs.Write(file, 0, file.Length);
                fs.Flush();
                fs.Close();
                string oggetto = string.Format("Estrazione OC al giorno {0}", DateTime.Today.ToShortDateString());
                string corpo = "Dati in allegato";

                decimal IDMAIL = MailDispatcherService.CreaEmail("ESTRAZIONE_OC", oggetto, corpo);
                MailDispatcherService.AggiungiAllegato(IDMAIL, "ESTRAZIONE_OC.xlsx", new System.IO.MemoryStream(file));
                MailDispatcherService.SottomettiEmail(IDMAIL);
                if (File.Exists(filename))
                    File.Delete(filename);
            }
        }

    }
}
