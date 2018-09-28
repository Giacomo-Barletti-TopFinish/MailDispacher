using MailDispacher.Entities;
using MailDispatcher.Data.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MailDispatcher.Data
{
    public class MailDispatcherBusiness : MDBusinessBase
    {
        public MailDispatcherBusiness() : base() { }

        [DataContext]
        public void FillMD_GRUPPI(MailDispatcherDS ds)
        {
            MailDispatcherAdapter a = new MailDispatcherAdapter(DbConnection, DbTransaction);
            a.FillMD_GRUPPI(ds);
        }

        [DataContext]
        public void FillMD_GRUPPI_DESTINATARI(MailDispatcherDS ds)
        {
            MailDispatcherAdapter a = new MailDispatcherAdapter(DbConnection, DbTransaction);
            a.FillMD_GRUPPI_DESTINATARI(ds);
        }

        [DataContext]
        public void FillMD_GRUPPI_RICHIEDENTI(MailDispatcherDS ds)
        {
            MailDispatcherAdapter a = new MailDispatcherAdapter(DbConnection, DbTransaction);
            a.FillMD_GRUPPI_RICHIEDENTI(ds);
        }

        [DataContext]
        public void FillMD_RICHIEDENTI(MailDispatcherDS ds)
        {
            MailDispatcherAdapter a = new MailDispatcherAdapter(DbConnection, DbTransaction);
            a.FillMD_RICHIEDENTI(ds);
        }


        [DataContext(true)]
        public void UpdateMailDispatcherDSTable(string Tablename, MailDispatcherDS ds)
        {
            MailDispatcherAdapter a = new MailDispatcherAdapter(DbConnection, DbTransaction);
            a.UpdateMailDispatcherDSTable(Tablename, ds);
        }

        [DataContext]
        public void FillMD_EMAILByStato(string Stato, MailDispatcherDS ds)
        {
            MailDispatcherAdapter a = new MailDispatcherAdapter(DbConnection, DbTransaction);
            a.FillMD_EMAILByStato(Stato, ds);
        }

        [DataContext]
        public void FillMD_ALLEGATI(MailDispatcherDS ds, List<decimal> IDMAIL)
        {
            MailDispatcherAdapter a = new MailDispatcherAdapter(DbConnection, DbTransaction);
            a.FillMD_ALLEGATI(ds, IDMAIL);
        }

        [DataContext]
        public void FillMD_ALLEGATI(MailDispatcherDS ds, decimal IDMAIL)
        {
            MailDispatcherAdapter a = new MailDispatcherAdapter(DbConnection, DbTransaction);
            a.FillMD_ALLEGATI(ds, IDMAIL);
        }


        [DataContext]
        public void InsertMD_LOG(decimal IdMail, string TipoOperazione, string Nota)
        {
            MailDispatcherAdapter a = new MailDispatcherAdapter(DbConnection, DbTransaction);
            a.InsertMD_LOG(IdMail, TipoOperazione.ToUpper(), Nota.ToUpper());
        }

        [DataContext]
        public void InsertMD_LOG(string Nota)
        {
            MailDispatcherAdapter a = new MailDispatcherAdapter(DbConnection, DbTransaction);
            a.InsertMD_LOG(Nota);
        }

        [DataContext(true)]
        public decimal CreaMail(decimal idRichiedente, string oggetto, string corpo)
        {
            MailDispatcherAdapter a = new MailDispatcherAdapter(DbConnection, DbTransaction);
            return a.CreaMail(idRichiedente, oggetto, corpo);
        }

        [DataContext(true)]
        public void SottomettiEmail(decimal IDMAIL)
        {
            MailDispatcherAdapter a = new MailDispatcherAdapter(DbConnection, DbTransaction);
            a.SottomettiEmail(IDMAIL);
        }

        [DataContext(true)]
        public void AggiungiAllegato(decimal IDMAIL, string filename, MemoryStream ms)
        {            
            MailDispatcherAdapter a = new MailDispatcherAdapter(DbConnection, DbTransaction);
            a.AggiungiAllegato(IDMAIL, filename, ms);
        }
    }
}
