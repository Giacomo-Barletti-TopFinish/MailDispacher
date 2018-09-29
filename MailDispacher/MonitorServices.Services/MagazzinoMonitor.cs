﻿using MailDispatcher.Common;
using MonitorServices.Data;
using MonitorServices.Entities;
using MonitorServices.Helpers;
using System;
using System.Collections.Generic;
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
                bMagazzino.FillMagazziniGiacenza(ds);

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
    }
}