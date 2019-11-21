using MonitorServices.Data.Core;
using MonitorServices.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonitorServices.Data
{
    public class ValorizzazioneMagazziniBusiness : MonitorBusinessBase
    {
        public ValorizzazioneMagazziniBusiness() : base() { }

        [DataContext]
        public void FillSALDI_GEN(ValorizzaMagazziniDS ds)
        {
            ValorizzazioneMagazziniAdapter a = new ValorizzazioneMagazziniAdapter(DbConnection, DbTransaction);
            a.FillSALDI_GEN(ds);
        }

        [DataContext]
        public void FillUSR_LIS_VEN(ValorizzaMagazziniDS ds)
        {
            ValorizzazioneMagazziniAdapter a = new ValorizzazioneMagazziniAdapter(DbConnection, DbTransaction);
            a.FillUSR_LIS_VEN(ds);
        }

        [DataContext(true)]
        public void UpdateMonitorDSTable(string tablename, ValorizzaMagazziniDS ds)
        {
            ValorizzazioneMagazziniAdapter a = new ValorizzazioneMagazziniAdapter(DbConnection, DbTransaction);
            a.UpdateMonitorDSTable(tablename, ds);
        }
    }
}
