using MonitorServices.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonitorServices.Data
{
    public class MagazzinoAdapter:MonitorAdapterBase
    {
        public MagazzinoAdapter(System.Data.IDbConnection connection, IDbTransaction transaction) :
            base(connection, transaction)
        { }

        public void FillMagazziniNegativi(MagazzinoDS ds)
        {
            string select = @"select  ma.idmagazz,ma.modello, ma.desmagazz,tm.CODICEMAG, tm.destabmag ,sa.qesi
                                from ditta1.SALDI_GEN sa
                                inner join gruppo.magazz ma on ma.idmagazz = sa.idmagazz
                                inner join gruppo.tabmag tm on tm.idtabmag = sa.idtabmag
                                INNER JOIN MONITOR_TABMAG_ABILITATI TA ON TA.idtabmag = TM.idtabmag
                                where sa.qesi <0

                                union all

                                select  ma.idmagazz,ma.modello, ma.desmagazz,tm.CODICEMAG, tm.destabmag ,sa.qesi
                                from ditta2.SALDI_GEN sa
                                inner join gruppo.magazz ma on ma.idmagazz = sa.idmagazz
                                inner join gruppo.tabmag tm on tm.idtabmag = sa.idtabmag
                                INNER JOIN MONITOR_TABMAG_ABILITATI TA ON TA.idtabmag = TM.idtabmag
                                where sa.qesi <0
                                ";

            using (DbDataAdapter da = BuildDataAdapter(select))
            {
                da.Fill(ds.MAGAZZINONEGATIVO);
            }
        }

        public void FillMagazziniGiacenza(MagazzinoDS ds)
        {
            string select = @"  select  ma.idmagazz,ma.modello, ma.desmagazz,tm.CODICEMAG, tm.destabmag ,sa.qesi, SA.QTOT_DISP_ESI,GI.GIACENZA
                                from ditta1.SALDI_GEN sa
                                inner join gruppo.magazz ma on ma.idmagazz = sa.idmagazz
                                inner join gruppo.tabmag tm on tm.idtabmag = sa.idtabmag
                                INNER JOIN MONITOR_GIACENZA GI ON GI.IDMAGAZZ = MA.IDMAGAZZ
                                INNER JOIN MONITOR_TABMAG_ABILITATI TA ON TA.idtabmag = TM.idtabmag
                                where sa.QTOT_DISP_ESI <GI.GIACENZA

                                union all

                                select  ma.idmagazz,ma.modello, ma.desmagazz,tm.CODICEMAG, tm.destabmag ,sa.qesi, SA.QTOT_DISP_ESI,GI.GIACENZA
                                from ditta2.SALDI_GEN sa
                                inner join gruppo.magazz ma on ma.idmagazz = sa.idmagazz
                                inner join gruppo.tabmag tm on tm.idtabmag = sa.idtabmag
                                INNER JOIN MONITOR_GIACENZA GI ON GI.IDMAGAZZ = MA.IDMAGAZZ
                                INNER JOIN MONITOR_TABMAG_ABILITATI TA ON TA.idtabmag = TM.idtabmag
                                where sa.QTOT_DISP_ESI <GI.GIACENZA
                                ";

            using (DbDataAdapter da = BuildDataAdapter(select))
            {
                da.Fill(ds.MAGAZZINOGIACENZA);
            }
        }
    }
}
