using MonitorServices.Data.Core;
using MonitorServices.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonitorServices.Data
{
    public class MonitorBusiness : MonitorBusinessBase
    {
        public MonitorBusiness() : base() { }

        [DataContext]
        public void FillMONITOR_SCHEDULER(MonitorDS ds)
        {
            MonitorAdapter a = new MonitorAdapter(DbConnection, DbTransaction);
            a.FillMONITOR_SCHEDULER(ds);
        }

        [DataContext(true)]
        public void UpdateMONITOR_SCHEDULER(MonitorDS ds)
        {
            MonitorAdapter a = new MonitorAdapter(DbConnection, DbTransaction);
            a.UpdateMonitorDSTable(ds.MONITOR_SCHEDULER.TableName, ds);
        }        
    }
}
