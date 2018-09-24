using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using MailDispacher.Entities;

namespace MailDispatcher.Data
{

    public class MailDispatcherAdapter : MDAdapterBase
    {
        public MailDispatcherAdapter(System.Data.IDbConnection connection, IDbTransaction transaction) :
            base(connection, transaction)
        { }

        public void FillMD_GRUPPI(MailDispatcherDS ds)
        {
            string select = @"SELECT * FROM MD_GRUPPI ";

            using (DbDataAdapter da = BuildDataAdapter(select))
            {
                da.Fill(ds.MD_GRUPPI);
            }
        }

        public void FillMD_GRUPPI_DESTINATARI(MailDispatcherDS ds)
        {
            string select = @"SELECT * FROM MD_GRUPPI_DESTINATARI ";

            using (DbDataAdapter da = BuildDataAdapter(select))
            {
                da.Fill(ds.MD_GRUPPI_DESTINATARI);
            }
        }

        public void FillMD_RICHIEDENTI(MailDispatcherDS ds)
        {
            string select = @"SELECT * FROM MD_RICHIEDENTI ";

            using (DbDataAdapter da = BuildDataAdapter(select))
            {
                da.Fill(ds.MD_RICHIEDENTI);
            }
        }

        public void FillMD_GRUPPI_RICHIEDENTI(MailDispatcherDS ds)
        {
            string select = @"SELECT * FROM MD_GRUPPI_RICHIEDENTI ";

            using (DbDataAdapter da = BuildDataAdapter(select))
            {
                da.Fill(ds.MD_GRUPPI_RICHIEDENTI);
            }
        }

        public void UpdateMailDispatcherDSTable(string tablename, MailDispatcherDS ds)
        {
            string query = string.Format(CultureInfo.InvariantCulture, "SELECT * FROM {0}", tablename);

            using (DbDataAdapter a = BuildDataAdapter(query))
            {
                a.ContinueUpdateOnError = false;
                DataTable dt = ds.Tables[tablename];
                DbCommandBuilder cmd = BuildCommandBuilder(a);
                a.UpdateCommand = cmd.GetUpdateCommand();
                a.DeleteCommand = cmd.GetDeleteCommand();
                a.InsertCommand = cmd.GetInsertCommand();
                a.Update(dt);
            }
        }

        public void FillMD_EMAILByStato(string Stato, MailDispatcherDS ds)
        {
            string select = @"select * from MD_EMAIL where STATO=$P{Stato}";

            ParamSet ps = new ParamSet();
            ps.AddParam("Stato", DbType.String, Stato);

            using (DbDataAdapter da = BuildDataAdapter(select, ps))
            {
                da.Fill(ds.MD_EMAIL);
            }
        }

        public void FillMD_ALLEGATI(MailDispatcherDS ds, List<decimal> IDMAIL)
        {

            if (IDMAIL.Count == 0) return;

            string selezione = ConvertToStringForInCondition(IDMAIL);
            string select = @"SELECT * FROM MD_ALLEGATI WHERE IDMAIL IN ({0})";

            select = string.Format(select, selezione);

            using (DbDataAdapter da = BuildDataAdapter(select))
            {
                da.Fill(ds.MD_ALLEGATI);
            }
        }

        public void InsertMD_LOG(string Nota)
        {

            string select = @"INSERT INTO MD_LOG (DATAOPERAZIONE,TIPOOPERAZIONE, NOTA) VALUES ($P{Data},'INFO',$P{Nota})";

            ParamSet ps = new ParamSet();
            ps.AddParam("Nota", DbType.String, Nota);
            ps.AddParam("Data", DbType.DateTime, DateTime.Now);


            using (DbCommand cmd = BuildCommand(select, ps))
            {
                cmd.ExecuteNonQuery();
            }
        }

        public void InsertMD_LOG(decimal IdMail, string TipoOperazione, string Nota)
        {

            string select = @"INSERT INTO MD_LOG (IDMAIL,DATAOPERAZIONE,TIPOOPERAZIONE, NOTA) VALUES ($P{IDMAIL},$P{Data},$P{TipoOperazione},$P{Nota})";

            ParamSet ps = new ParamSet();
            ps.AddParam("IDMAIL", DbType.Decimal, IdMail);
            ps.AddParam("Data", DbType.DateTime, DateTime.Now);
            ps.AddParam("Nota", DbType.String, Nota);
            ps.AddParam("TipoOperazione", DbType.String, TipoOperazione);

            using (DbCommand cmd = BuildCommand(select, ps))
            {
                cmd.ExecuteNonQuery();
            }
        }
    }
}
