using MonitorServices.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonitorServices.Data
{
    public class ValorizzazioneMagazziniAdapter : MonitorAdapterBase
    {
        public ValorizzazioneMagazziniAdapter(System.Data.IDbConnection connection, IDbTransaction transaction) :
           base(connection, transaction)
        { }

        public void FillSALDI_GEN(ValorizzaMagazziniDS ds)
        {
            string select = @"select * from saldi_gen";

            using (DbDataAdapter da = BuildDataAdapter(select))
            {
                da.Fill(ds.SALDI_GEN);
            }
        }

        public void FillUSR_LIS_VEN(ValorizzaMagazziniDS ds)
        {
            string select = @"select * from USR_LIS_VEN";

            using (DbDataAdapter da = BuildDataAdapter(select))
            {
                da.Fill(ds.USR_LIS_VEN);
            }
        }

        public void UpdateMonitorDSTable(string tablename, ValorizzaMagazziniDS ds)
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
    }
}
