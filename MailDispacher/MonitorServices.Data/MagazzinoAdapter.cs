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

        public void FillGIACENZA_BRAND_MANAGER(MagazzinoDS ds)
        {
            string select = @"SELECT * FROM GIACENZA_BRAND_MANAGER ";

            using (DbDataAdapter da = BuildDataAdapter(select))
            {
                da.Fill(ds.GIACENZA_BRAND_MANAGER);
            }
        }

        public void FillUSR_INVENTARIOS(MagazzinoDS ds)
        {
            string select = @"SELECT * FROM DITTA1.USR_INVENTARIOS ";

            using (DbDataAdapter da = BuildDataAdapter(select))
            {
                da.Fill(ds.USR_INVENTARIOS);
            }
        }

        public void CopiaPrelievi()
        {
            string insert = @"INSERT INTO RW_PRELIEVI  
                select SYSDATE DATAOPERAZIONE,PRED.idprelievoD,TMO.CODICEMAG MAG_ORI,PRED.DATAPRELIEVOD,TMD.CODICEMAG MAG_DEST,MAG.MODELLO,PRED.QUANTITA,PRED.QUANTITA_SUCC, 'MaterialiODL' origine,CASE pret.statoprelievot WHEN 0 THEN 'Aperto' when 1 then 'Chiuso' else 'Prelevato' END stato
                from ditta1.usr_prelievod PRED
                INNER JOIN GRUPPO.TABMAG  TMD ON TMD.IDTABMAG = PRED.IDTABMAG_DEST
                INNER JOIN GRUPPO.TABMAG  TMO ON TMO.IDTABMAG = PRED.IDTABMAG_ORI
                inner join ditta1.usr_prd_flusso_movmate moma on pred.idorigine = moma.idprdFLUSSOmovmate AND PRED.ORIGINE = 1 
                inner join ditta1.usr_prd_movmate ma on ma.idprdmovmate = moma.idprdmovmate
                inner join ditta1.usr_prd_fasi fa on fa.idprdfase = ma.idprdfase
                inner join ditta1.usr_prelievot pret on pred.idprelievot = pret.idprelievot
                INNER JOIN GRUPPO.MAGAZZ MAG ON MAG.IDMAGAZZ = PRED.IDMAGAZZ_ORI
                 WHERE fa.codiceclifo not in ('MONT%','SALD%')
                AND pret.statoprelievot = 0
                AND TMD.CODICEMAG NOT IN (select codicemag from gruppo.tabmag 
                where codicemag in ('SEDE','EXIT','LAV','SOS','LSOS','ATTCONF','ATT MONT','ATTSALD','MATL','LSTOCK')
                OR CODICEMAG LIKE 'FINIT%' 
                OR CODICEMAG LIKE 'GRE\SAL%' )
                union all
                select SYSDATE DATAOPERAZIONE,PRED.idprelievoD,TMO.CODICEMAG MAG_ORI,PRED.DATAPRELIEVOD,TMD.CODICEMAG MAG_DEST,MAG.MODELLO,PRED.QUANTITA,PRED.QUANTITA_SUCC, 'OrigineCliente' origine, CASE pret.statoprelievot WHEN 0 THEN 'Aperto' when 1 then 'Chiuso' else 'Prelevato' END stato
                from ditta2.usr_prelievod PRED
                INNER JOIN GRUPPO.TABMAG  TMD ON TMD.IDTABMAG = PRED.IDTABMAG_DEST
                INNER JOIN GRUPPO.TABMAG  TMO ON TMO.IDTABMAG = PRED.IDTABMAG_ORI
                inner join ditta2.usr_VENDITED VD on pred.idorigine = VD.idVENDITED AND PRED.ORIGINE = 0
                inner join ditta2.usr_VENDITET VT on VT.IDVENDITET= VD.IDVENDITET
                INNER JOIN GRUPPO.MAGAZZ MAG ON MAG.IDMAGAZZ = PRED.IDMAGAZZ_ORI
                inner join ditta2.usr_prelievot pret on pred.idprelievot = pret.idprelievot
                 WHERE VT.codiceclifo not in ('MONT%','SALD%')
                AND pret.statoprelievot = 0 
                AND TMD.CODICEMAG NOT IN (select codicemag from gruppo.tabmag 
                where codicemag in ('SEDE','EXIT','LAV','SOS','LSOS','ATTCONF','ATT MONT','ATTSALD','MATL','LSTOCK')
                OR CODICEMAG LIKE 'FINIT%' 
                OR CODICEMAG LIKE 'GRE\SAL%' )";
            using (DbCommand cmd = BuildCommand(insert))
            {
                cmd.ExecuteNonQuery();
            }
        }

        public void CopiaTrasferimentiMP()
        {
            string insert = @"insert into rw_trasferimenti 
                    select 'MP' AZIENDA, SYSDATE DATAOPERAZIONE,ff.IDRICHTRASF,ff.IDRICHTRASFT, ff.IDMAGAZZ, vv.NUMRICHTRASFT, gr.modello, gr.desmagazz,magd.codicemag destinazione, cau.codicecaumgt,mago.codicemag origine,
                    flf.codiceclifor, vv.datarichtrasft, case vv.statorichtrasft when 0 then 'aperto' when 1 then 'chiuso' else 'trasferito' end stato
                    from ditta1.usr_trasf_rich ff
                    inner join ditta1.usr_trasf_richt vv on ff.IDRICHTRASFT = vv.IDRICHTRASFT
                    inner join gruppo.magazz gr on gr.idmagazz = ff.idmagazz
                    inner join gruppo.tabmag magd on magd.idtabmag = ff.idtabmagd
                    inner join gruppo.tabcaumgt cau on cau.idtabcaumgt = ff.idtabcaumgt
                    inner join ditta1.usr_trasf_flusso_rich flf on flf.idrichtrasf = ff.idrichtrasf and flf.sequenza=0
                    inner join gruppo.tabmag mago on mago.idtabmag = flf.idtabmagr
                    where ff.idtabmagd in (0000000137,0000000154,0000000138,0000000139)
                    and vv.statorichtrasft = 0
                    order by vv.NUMRICHTRASFT";
            using (DbCommand cmd = BuildCommand(insert))
            {
                cmd.ExecuteNonQuery();
            }
        }
        public void CopiaTrasferimentiTF()
        {
            string insert = @"insert into rw_trasferimenti 
                    select 'TF' AZIENDA, SYSDATE DATAOPERAZIONE,ff.IDRICHTRASF,ff.IDRICHTRASFT, ff.IDMAGAZZ, vv.NUMRICHTRASFT, gr.modello, gr.desmagazz,magd.codicemag destinazione, cau.codicecaumgt,mago.codicemag origine,
                    flf.codiceclifor, vv.datarichtrasft, case vv.statorichtrasft when 0 then 'aperto' when 1 then 'chiuso' else 'trasferito' end stato
                    from ditta2.usr_trasf_rich ff
                    inner join ditta2.usr_trasf_richt vv on ff.IDRICHTRASFT = vv.IDRICHTRASFT
                    inner join gruppo.magazz gr on gr.idmagazz = ff.idmagazz
                    inner join gruppo.tabmag magd on magd.idtabmag = ff.idtabmagd
                    inner join gruppo.tabcaumgt cau on cau.idtabcaumgt = ff.idtabcaumgt
                    inner join ditta2.usr_trasf_flusso_rich flf on flf.idrichtrasf = ff.idrichtrasf and flf.sequenza=0
                    inner join gruppo.tabmag mago on mago.idtabmag = flf.idtabmagr
                    where ff.idtabmagd in (0000000137,0000000154,0000000138,0000000139)
                    and vv.statorichtrasft = 0
                    order by vv.NUMRICHTRASFT";
            using (DbCommand cmd = BuildCommand(insert))
            {
                cmd.ExecuteNonQuery();
            }
        }

        public void CopiaFattibilitaMP()
        {
            string insert = @"INSERT INTO RW_FATTIBILITA 
                    select 'MP' AS AZIENDA,SYSDATE DATAOPERAZIONE,ld.nomecommessa,tab.idprdfase, min(fatUni) QTA from (
    
                    SELECT IDPRDMATE,
		                    IDPRDFASE,
		                    IDMAGAZZ ,
		                    IDTABMISU ,
		                    IDTABCOMBCOL ,
		                    IDTABVARI ,
		                    IDTABUNIMI ,
		                    UNITRATE ,
		                    FABBIUNI ,
		                    TIPOFABBI ,
		                    QTAOCCORRENZA ,
		                    RILEVAZIONE ,
		                    FABBIACCECOM as FABBIACCE,
		                    FABBINETCOM as FABBINET,0
		                    ACQPRD,
                        QTADAASS,
		                    SUM(SUM_QTOT		 ) as QTOT_DIS_ESI_MAGF ,
		                    SUM(SUM_QTOT_TRASF)   as QTOT_ACC_TRASF,
		                    SUM(CAMPO_VUOTO_PER_LA_VIRGOLA),
                        FABBIACCECOM +SUM(SUM_QTOT_TRASF) fat,
                        (FABBIACCECOM +SUM(SUM_QTOT_TRASF) )/FABBIUNI fatUni
                    FROM 
		                    ( (
	                     SELECT  
			                    USR_PRD_MATE.IDPRDMATE      AS IDPRDMATE,
			                    USR_PRD_MATE.IDPRDFASE      AS IDPRDFASE,
			                    USR_PRD_MATE.IDMAGAZZ       AS IDMAGAZZ      ,
			                    USR_PRD_MATE.IDTABMISU      AS IDTABMISU     ,
			                    USR_PRD_MATE.IDTABCOMBCOL   AS IDTABCOMBCOL  ,
			                    USR_PRD_MATE.IDTABVARI      AS IDTABVARI     ,
			                    USR_PRD_MATE.IDTABUNIMI     AS IDTABUNIMI    ,
			                    USR_PRD_MATE.UNITRATE       AS UNITRATE      ,
			                    USR_PRD_MATE.FABBIUNI       AS FABBIUNI      ,
			                    USR_PRD_MATE.TIPOFABBI      AS TIPOFABBI     ,
			                    USR_PRD_MATE.QTAOCCORRENZA  AS QTAOCCORRENZA ,
			                    USR_PRD_MATE.RILEVAZIONE    AS RILEVAZIONE   ,
			                    USR_PRD_MATE.FABBIACCECOM AS   FABBIACCECOM,
			                    USR_PRD_MATE.FABBINETCOM AS   FABBINETCOM,
			                    MAGAZZ.ACQPRD AS ACQPRD,
			                    COALESCE(SUM (QTOT_DISP_ESI), 0) AS SUM_QTOT,
			                    0 AS SUM_QTOT_TRASF, 
			                    0 AS CAMPO_VUOTO_PER_LA_VIRGOLA ,
                          fase.QTADAASS
		                    FROM DITTA1.USR_PRD_MATE
                        INNER JOIN DITTA1.USR_PRD_FASI FASE ON FASE.IDPRDFASE = USR_PRD_MATE.IDPRDFASE
		                    LEFT JOIN GRUPPO.MAGAZZ ON USR_PRD_MATE.IDMAGAZZ = MAGAZZ.IDMAGAZZ 
		                    LEFT JOIN DITTA1.SALDI_GEN ON (USR_PRD_MATE.IDMAGAZZ = SALDI_GEN.IDMAGAZZ AND COALESCE(USR_PRD_MATE.IDTABMISU, '*') = COALESCE(SALDI_GEN.IDTABMISU, '*') AND COALESCE(USR_PRD_MATE.IDTABCOMBCOL, '*') = COALESCE(SALDI_GEN.IDTABCOMBCOL, '*')  AND COALESCE(USR_PRD_MATE.IDTABVARI, '*') = COALESCE(SALDI_GEN.IDTABVARI, '*') AND COALESCE(USR_PRD_MATE.IDTABUNIMI, '*') = COALESCE(SALDI_GEN.IDTABUNIMI, '*'))
		                    LEFT JOIN GRUPPO.TABMAG ON SALDI_GEN.IDTABMAG=TABMAG.IDTABMAG AND TABMAG.fabbisognisn = 1 
		                    WHERE 1 = 1  
				                    AND USR_PRD_MATE.CACQSN  = 0  
				                    AND USR_PRD_MATE.ANNULLATA_SN = 0 
                            AND FASE.idtabfas IN (0000000203, 0000000202, 0000000077, 0000000058) 
		                    GROUP BY USR_PRD_MATE.IDPRDMATE, USR_PRD_MATE.IDPRDFASE, USR_PRD_MATE.IDMAGAZZ, USR_PRD_MATE.IDTABMISU, USR_PRD_MATE.IDTABCOMBCOL, USR_PRD_MATE.IDTABVARI, 
				                    USR_PRD_MATE.IDTABUNIMI, USR_PRD_MATE.UNITRATE, USR_PRD_MATE.FABBIUNI, USR_PRD_MATE.TIPOFABBI, USR_PRD_MATE.QTAOCCORRENZA, USR_PRD_MATE.RILEVAZIONE, 
				                    USR_PRD_MATE.FABBIACCECOM, USR_PRD_MATE.FABBINETCOM,  MAGAZZ.ACQPRD,fase.QTADAASS
		                    ) UNION (                                 
		                    SELECT  
				                    USR_PRD_MATE.IDPRDMATE      AS IDPRDMATE,
				                    USR_PRD_MATE.IDPRDFASE      AS IDPRDFASE,
				                    USR_PRD_MATE.IDMAGAZZ       AS IDMAGAZZ      ,
				                    USR_PRD_MATE.IDTABMISU      AS IDTABMISU     ,
				                    USR_PRD_MATE.IDTABCOMBCOL   AS IDTABCOMBCOL  ,
				                    USR_PRD_MATE.IDTABVARI      AS IDTABVARI     ,
				                    USR_PRD_MATE.IDTABUNIMI     AS IDTABUNIMI    ,
				                    USR_PRD_MATE.UNITRATE       AS UNITRATE      ,
				                    USR_PRD_MATE.FABBIUNI       AS FABBIUNI      ,
				                    USR_PRD_MATE.TIPOFABBI      AS TIPOFABBI     ,
				                    USR_PRD_MATE.QTAOCCORRENZA  AS QTAOCCORRENZA ,
				                    USR_PRD_MATE.RILEVAZIONE    AS RILEVAZIONE   ,
				                    USR_PRD_MATE.FABBIACCECOM AS   FABBIACCECOM,
				                    USR_PRD_MATE.FABBINETCOM AS   FABBINETCOM,
				                    '' AS ACQPRD ,--  MAGAZZ.ACQPRD AS ACQPRD,
				                    0    AS SUM_QTOT,
				                    COALESCE(SUM (QUANTITA_DOC_ARR), 0) AS SUM_QTOT_TRASF,
				                    0 AS CAMPO_VUOTO_PER_LA_VIRGOLA ,
                            fase.QTADAASS
		                    FROM DITTA1.USR_PRD_MATE
                        INNER JOIN DITTA1.USR_PRD_FASI FASE ON FASE.IDPRDFASE = USR_PRD_MATE.IDPRDFASE
		                    LEFT JOIN DITTA1.USR_ACCTO_CON ON (USR_ACCTO_CON.IDORIGINE = USR_PRD_MATE.IDPRDMATE) AND USR_ACCTO_CON.ORIGINE = 1
		                    LEFT JOIN DITTA1.USR_ACCTO_CON_DOC ON (USR_ACCTO_CON_DOC.IDACCTOCON = USR_ACCTO_CON.IDACCTOCON)  AND USR_ACCTO_CON_DOC.DESTINAZIONE = 3
		                    WHERE 1 = 1  
                            AND FASE.idtabfas IN (0000000203, 0000000202, 0000000077, 0000000058) 
				                    AND USR_PRD_MATE.CACQSN  = 0  
				                    AND USR_PRD_MATE.ANNULLATA_SN = 0 
		                    GROUP BY USR_PRD_MATE.IDPRDMATE, USR_PRD_MATE.IDPRDFASE, USR_PRD_MATE.IDMAGAZZ, USR_PRD_MATE.IDTABMISU, USR_PRD_MATE.IDTABCOMBCOL, USR_PRD_MATE.IDTABVARI, 
				                    USR_PRD_MATE.IDTABUNIMI, USR_PRD_MATE.UNITRATE, USR_PRD_MATE.FABBIUNI, USR_PRD_MATE.TIPOFABBI, USR_PRD_MATE.QTAOCCORRENZA, USR_PRD_MATE.RILEVAZIONE, 
				                    USR_PRD_MATE.FABBIACCECOM, USR_PRD_MATE.FABBINETCOM,fase.QTADAASS
		                    )   
                    )
                    where QTADAASS>0
                    GROUP BY  
                        IDPRDMATE,
                        IDPRDFASE,
                        IDMAGAZZ ,
                        IDTABMISU ,
                        IDTABCOMBCOL ,
                        IDTABVARI ,
                        IDTABUNIMI ,
                        UNITRATE ,
                        FABBIUNI ,
                        TIPOFABBI ,
                        QTAOCCORRENZA ,
                        RILEVAZIONE ,
                        FABBIACCECOM,
                        FABBINETCOM,
                        ACQPRD,QTADAASS) tab
                        inner join ditta1.usr_prd_fasi fa on fa.idprdfase = tab.idprdfase
                        inner join ditta1.usr_prd_lanciod ld on ld.idlanciod = fa.idlanciod
                        having min(fatUni)>0
                        group by ld.nomecommessa,tab.idprdfase";
            using (DbCommand cmd = BuildCommand(insert))
            {
                cmd.ExecuteNonQuery();
            }
        }

        public void CopiaFattibilitaTF()
        {
            string insert = @"INSERT INTO RW_FATTIBILITA 
                    select 'TF' AS AZIENDA,SYSDATE DATAOPERAZIONE,ld.nomecommessa,tab.idprdfase, min(fatUni) QTA from (
    
                    SELECT IDPRDMATE,
		                    IDPRDFASE,
		                    IDMAGAZZ ,
		                    IDTABMISU ,
		                    IDTABCOMBCOL ,
		                    IDTABVARI ,
		                    IDTABUNIMI ,
		                    UNITRATE ,
		                    FABBIUNI ,
		                    TIPOFABBI ,
		                    QTAOCCORRENZA ,
		                    RILEVAZIONE ,
		                    FABBIACCECOM as FABBIACCE,
		                    FABBINETCOM as FABBINET,0
		                    ACQPRD,
                        QTADAASS,
		                    SUM(SUM_QTOT		 ) as QTOT_DIS_ESI_MAGF ,
		                    SUM(SUM_QTOT_TRASF)   as QTOT_ACC_TRASF,
		                    SUM(CAMPO_VUOTO_PER_LA_VIRGOLA),
                        FABBIACCECOM +SUM(SUM_QTOT_TRASF) fat,
                        (FABBIACCECOM +SUM(SUM_QTOT_TRASF) )/FABBIUNI fatUni
                    FROM 
		                    ( (
	                     SELECT  
			                    USR_PRD_MATE.IDPRDMATE      AS IDPRDMATE,
			                    USR_PRD_MATE.IDPRDFASE      AS IDPRDFASE,
			                    USR_PRD_MATE.IDMAGAZZ       AS IDMAGAZZ      ,
			                    USR_PRD_MATE.IDTABMISU      AS IDTABMISU     ,
			                    USR_PRD_MATE.IDTABCOMBCOL   AS IDTABCOMBCOL  ,
			                    USR_PRD_MATE.IDTABVARI      AS IDTABVARI     ,
			                    USR_PRD_MATE.IDTABUNIMI     AS IDTABUNIMI    ,
			                    USR_PRD_MATE.UNITRATE       AS UNITRATE      ,
			                    USR_PRD_MATE.FABBIUNI       AS FABBIUNI      ,
			                    USR_PRD_MATE.TIPOFABBI      AS TIPOFABBI     ,
			                    USR_PRD_MATE.QTAOCCORRENZA  AS QTAOCCORRENZA ,
			                    USR_PRD_MATE.RILEVAZIONE    AS RILEVAZIONE   ,
			                    USR_PRD_MATE.FABBIACCECOM AS   FABBIACCECOM,
			                    USR_PRD_MATE.FABBINETCOM AS   FABBINETCOM,
			                    MAGAZZ.ACQPRD AS ACQPRD,
			                    COALESCE(SUM (QTOT_DISP_ESI), 0) AS SUM_QTOT,
			                    0 AS SUM_QTOT_TRASF, 
			                    0 AS CAMPO_VUOTO_PER_LA_VIRGOLA ,
                          fase.QTADAASS
		                    FROM DITTA2.USR_PRD_MATE
                        INNER JOIN DITTA2.USR_PRD_FASI FASE ON FASE.IDPRDFASE = USR_PRD_MATE.IDPRDFASE
		                    LEFT JOIN GRUPPO.MAGAZZ ON USR_PRD_MATE.IDMAGAZZ = MAGAZZ.IDMAGAZZ 
		                    LEFT JOIN DITTA2.SALDI_GEN ON (USR_PRD_MATE.IDMAGAZZ = SALDI_GEN.IDMAGAZZ AND COALESCE(USR_PRD_MATE.IDTABMISU, '*') = COALESCE(SALDI_GEN.IDTABMISU, '*') AND COALESCE(USR_PRD_MATE.IDTABCOMBCOL, '*') = COALESCE(SALDI_GEN.IDTABCOMBCOL, '*')  AND COALESCE(USR_PRD_MATE.IDTABVARI, '*') = COALESCE(SALDI_GEN.IDTABVARI, '*') AND COALESCE(USR_PRD_MATE.IDTABUNIMI, '*') = COALESCE(SALDI_GEN.IDTABUNIMI, '*'))
		                    LEFT JOIN GRUPPO.TABMAG ON SALDI_GEN.IDTABMAG=TABMAG.IDTABMAG AND TABMAG.fabbisognisn = 1 
		                    WHERE 1 = 1  
				                    AND USR_PRD_MATE.CACQSN  = 0  
				                    AND USR_PRD_MATE.ANNULLATA_SN = 0 
                            AND FASE.idtabfas IN (0000000203, 0000000202, 0000000077, 0000000058) 
		                    GROUP BY USR_PRD_MATE.IDPRDMATE, USR_PRD_MATE.IDPRDFASE, USR_PRD_MATE.IDMAGAZZ, USR_PRD_MATE.IDTABMISU, USR_PRD_MATE.IDTABCOMBCOL, USR_PRD_MATE.IDTABVARI, 
				                    USR_PRD_MATE.IDTABUNIMI, USR_PRD_MATE.UNITRATE, USR_PRD_MATE.FABBIUNI, USR_PRD_MATE.TIPOFABBI, USR_PRD_MATE.QTAOCCORRENZA, USR_PRD_MATE.RILEVAZIONE, 
				                    USR_PRD_MATE.FABBIACCECOM, USR_PRD_MATE.FABBINETCOM,  MAGAZZ.ACQPRD,fase.QTADAASS
		                    ) UNION (                                 
		                    SELECT  
				                    USR_PRD_MATE.IDPRDMATE      AS IDPRDMATE,
				                    USR_PRD_MATE.IDPRDFASE      AS IDPRDFASE,
				                    USR_PRD_MATE.IDMAGAZZ       AS IDMAGAZZ      ,
				                    USR_PRD_MATE.IDTABMISU      AS IDTABMISU     ,
				                    USR_PRD_MATE.IDTABCOMBCOL   AS IDTABCOMBCOL  ,
				                    USR_PRD_MATE.IDTABVARI      AS IDTABVARI     ,
				                    USR_PRD_MATE.IDTABUNIMI     AS IDTABUNIMI    ,
				                    USR_PRD_MATE.UNITRATE       AS UNITRATE      ,
				                    USR_PRD_MATE.FABBIUNI       AS FABBIUNI      ,
				                    USR_PRD_MATE.TIPOFABBI      AS TIPOFABBI     ,
				                    USR_PRD_MATE.QTAOCCORRENZA  AS QTAOCCORRENZA ,
				                    USR_PRD_MATE.RILEVAZIONE    AS RILEVAZIONE   ,
				                    USR_PRD_MATE.FABBIACCECOM AS   FABBIACCECOM,
				                    USR_PRD_MATE.FABBINETCOM AS   FABBINETCOM,
				                    '' AS ACQPRD ,--  MAGAZZ.ACQPRD AS ACQPRD,
				                    0    AS SUM_QTOT,
				                    COALESCE(SUM (QUANTITA_DOC_ARR), 0) AS SUM_QTOT_TRASF,
				                    0 AS CAMPO_VUOTO_PER_LA_VIRGOLA ,
                            fase.QTADAASS
		                    FROM DITTA2.USR_PRD_MATE
                        INNER JOIN DITTA2.USR_PRD_FASI FASE ON FASE.IDPRDFASE = USR_PRD_MATE.IDPRDFASE
		                    LEFT JOIN DITTA2.USR_ACCTO_CON ON (USR_ACCTO_CON.IDORIGINE = USR_PRD_MATE.IDPRDMATE) AND USR_ACCTO_CON.ORIGINE = 1
		                    LEFT JOIN DITTA2.USR_ACCTO_CON_DOC ON (USR_ACCTO_CON_DOC.IDACCTOCON = USR_ACCTO_CON.IDACCTOCON)  AND USR_ACCTO_CON_DOC.DESTINAZIONE = 3
		                    WHERE 1 = 1  
                            AND FASE.idtabfas IN (0000000203, 0000000202, 0000000077, 0000000058) 
				                    AND USR_PRD_MATE.CACQSN  = 0  
				                    AND USR_PRD_MATE.ANNULLATA_SN = 0 
		                    GROUP BY USR_PRD_MATE.IDPRDMATE, USR_PRD_MATE.IDPRDFASE, USR_PRD_MATE.IDMAGAZZ, USR_PRD_MATE.IDTABMISU, USR_PRD_MATE.IDTABCOMBCOL, USR_PRD_MATE.IDTABVARI, 
				                    USR_PRD_MATE.IDTABUNIMI, USR_PRD_MATE.UNITRATE, USR_PRD_MATE.FABBIUNI, USR_PRD_MATE.TIPOFABBI, USR_PRD_MATE.QTAOCCORRENZA, USR_PRD_MATE.RILEVAZIONE, 
				                    USR_PRD_MATE.FABBIACCECOM, USR_PRD_MATE.FABBINETCOM,fase.QTADAASS
		                    )   
                    )
                    where QTADAASS>0
                    GROUP BY  
                        IDPRDMATE,
                        IDPRDFASE,
                        IDMAGAZZ ,
                        IDTABMISU ,
                        IDTABCOMBCOL ,
                        IDTABVARI ,
                        IDTABUNIMI ,
                        UNITRATE ,
                        FABBIUNI ,
                        TIPOFABBI ,
                        QTAOCCORRENZA ,
                        RILEVAZIONE ,
                        FABBIACCECOM,
                        FABBINETCOM,
                        ACQPRD,QTADAASS) tab
                        inner join ditta2.usr_prd_fasi fa on fa.idprdfase = tab.idprdfase
                        inner join ditta2.usr_prd_lanciod ld on ld.idlanciod = fa.idlanciod
                        having min(fatUni)>0
                        group by ld.nomecommessa,tab.idprdfase";
            using (DbCommand cmd = BuildCommand(insert))
            {
                cmd.ExecuteNonQuery();
            }
        }
    }
}
