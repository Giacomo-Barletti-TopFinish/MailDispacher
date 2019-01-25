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
    public class MagazzinoAdapter : MonitorAdapterBase
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
            string select = @"   
                            select  'METAL-PLUS' AS AZIENDA ,ma.idmagazz,ma.modello, ma.desmagazz, sum(SA.QTOT_DISP_ESI) QTOT_DISP_ESI,GI.GIACENZA
                                from ditta1.SALDI_GEN sa
                                inner join gruppo.magazz ma on ma.idmagazz = sa.idmagazz
                                inner join gruppo.tabmag tm on tm.idtabmag = sa.idtabmag
                                INNER JOIN MONITOR_GIACENZA GI ON GI.IDMAGAZZ = MA.IDMAGAZZ
                                INNER JOIN MONITOR_TABMAG_ABILITATI TA ON TA.idtabmag = TM.idtabmag
                                having sum(SA.QTOT_DISP_ESI) <GI.GIACENZA
                                group by ma.idmagazz,ma.modello, ma.desmagazz,GI.GIACENZA
                                union all
                                
                 select 'METAL-PLUS' AS AZIENDA ,ma.idmagazz,'*'||ma.modello, ma.desmagazz, 0 QTOT_DISP_ESI,GI.GIACENZA from MONITOR_GIACENZA gi 
                 inner join gruppo.magazz ma on ma.idmagazz = gi.idmagazz
                 where gi.idmagazz not in (
                 select   distinct ma.idmagazz
                                from ditta1.SALDI_GEN sa
                                inner join gruppo.magazz ma on ma.idmagazz = sa.idmagazz
                                inner join gruppo.tabmag tm on tm.idtabmag = sa.idtabmag
                                INNER JOIN MONITOR_GIACENZA GI ON GI.IDMAGAZZ = MA.IDMAGAZZ
                                INNER JOIN MONITOR_TABMAG_ABILITATI TA ON TA.idtabmag = TM.idtabmag)                               
                                ";

            using (DbDataAdapter da = BuildDataAdapter(select))
            {
                da.Fill(ds.MAGAZZINOGIACENZA);
            }
        }

        public void FillMONITOR_APPROVVIGIONAMENTO(MagazzinoDS ds)
        {
            string select = @"SELECT * FROM MONITOR_APPROVVIGIONAMENTO ";

            using (DbDataAdapter da = BuildDataAdapter(select))
            {
                da.Fill(ds.MONITOR_APPROVVIGIONAMENTO);
            }
        }

    }
}
