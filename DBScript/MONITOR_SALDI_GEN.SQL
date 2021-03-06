
  CREATE TABLE MONITOR_SALDI_GEN 
   (	
   AZIENDA VARCHAR(2) NOT NULL,
   DATAELABORAZIONE DATE NOT NULL,
   PREZZO FLOAT NOT NULL,
   VALORE FLOAT NOT NULL,
   "IDSALDIGEN" VARCHAR2(36 BYTE) NOT NULL , 
	"IDTABMAG" VARCHAR2(10 BYTE) NOT NULL , 
	"CODICECLIFO" CHAR(10 BYTE), 
	"IDMAGAZZ" VARCHAR2(10 BYTE) NOT NULL , 
	"IDTABMISU" VARCHAR2(10 BYTE), 
	"IDTABCOMBCOL" VARCHAR2(10 BYTE), 
	"IDTABVARI" VARCHAR2(10 BYTE), 
	"IDTABUNIMI" VARCHAR2(10 BYTE) NOT NULL  , 
	"QRIM" FLOAT(126) DEFAULT 0 NOT NULL  , 
	"QCARPROG" FLOAT(126) DEFAULT 0 NOT NULL  , 
	"QSCAPROG" FLOAT(126) DEFAULT 0 NOT NULL  , 
	"DATAGIORNALE" DATE, 
	"QESI" FLOAT(126) DEFAULT 0 NOT NULL  , 
	"QDDE_ACE_OCP" FLOAT(126) DEFAULT 0 NOT NULL  , 
	"QDDE_ACE_OCD" FLOAT(126) DEFAULT 0 NOT NULL  , 
	"QDDE_ACE_MAT_COP" FLOAT(126) DEFAULT 0 NOT NULL  , 
	"QDDE_ACE_MAT_COD" FLOAT(126) DEFAULT 0 NOT NULL  , 
	"QDDE_ACE_MAT_OLP" FLOAT(126) DEFAULT 0 NOT NULL  , 
	"QDDE_ACE_MAT_OLD" FLOAT(126) DEFAULT 0 NOT NULL  , 
	"QDDE_ACE_TRA" FLOAT(126) DEFAULT 0 NOT NULL  , 
	"QDDE_INP_OCP" FLOAT(126) DEFAULT 0 NOT NULL  , 
	"QDDE_INP_OLD" FLOAT(126) DEFAULT 0 NOT NULL  , 
	"QDDE_INP_TRA" FLOAT(126) DEFAULT 0 NOT NULL  , 
	"QDDE_EST_OCP" FLOAT(126) DEFAULT 0 NOT NULL  , 
	"QDDE_EST_OLD" FLOAT(126) DEFAULT 0 NOT NULL  , 
	"QDDE_EST_TRA" FLOAT(126) DEFAULT 0 NOT NULL  , 
	"QDDE_PRB_OCP" FLOAT(126) DEFAULT 0 NOT NULL  , 
	"QDDE_PRB_OLD" FLOAT(126) DEFAULT 0 NOT NULL  , 
	"QDDE_PRB_TRA" FLOAT(126) DEFAULT 0 NOT NULL  , 
	"QIND_IMB_OCP" FLOAT(126) DEFAULT 0 NOT NULL  , 
	"QIND_IMB_OLD" FLOAT(126) DEFAULT 0 NOT NULL  , 
	"QIND_IMB_TRA" FLOAT(126) DEFAULT 0 NOT NULL  , 
	"QTOT_DISP_ESI" FLOAT(126) DEFAULT 0 NOT NULL  , 
	"QDDT_OCP_DAC" FLOAT(126) DEFAULT 0 NOT NULL  , 
	"QDDT_OCD_DAC" FLOAT(126) DEFAULT 0 NOT NULL  , 
	"QIND_OCP_TOT" FLOAT(126) DEFAULT 0 NOT NULL  , 
	"QIND_OCD_TOT" FLOAT(126) DEFAULT 0 NOT NULL  , 
	"QADT_OFP_DAC" FLOAT(126) DEFAULT 0 NOT NULL  , 
	"QADT_OFD_DAC" FLOAT(126) DEFAULT 0 NOT NULL  , 
	"QIND_ATT_CAR" FLOAT(126) DEFAULT 0 NOT NULL  , 
	"QADT_COP_DAC" FLOAT(126) DEFAULT 0 NOT NULL  , 
	"QADT_COD_DAC" FLOAT(126) DEFAULT 0 NOT NULL  , 
	"QADT_OLP_DAC" FLOAT(126) DEFAULT 0 NOT NULL  , 
	"QADT_OLD_DAC" FLOAT(126) DEFAULT 0 NOT NULL  , 
	"QADT_OLL_DAC" FLOAT(126) DEFAULT 0 NOT NULL  , 
	"QADT_PCA_TRA" FLOAT(126) DEFAULT 0 NOT NULL  , 
	"QDDT_FAB_MAT_COP" FLOAT(126) DEFAULT 0 NOT NULL  , 
	"QDDT_FAB_MAT_COD" FLOAT(126) DEFAULT 0 NOT NULL  , 
	"QDDT_FAB_MAT_OLP" FLOAT(126) DEFAULT 0 NOT NULL  , 
	"QDDT_FAB_MAT_OLD" FLOAT(126) DEFAULT 0 NOT NULL  , 
	"QDDT_FAB_TRA" FLOAT(126) DEFAULT 0 NOT NULL  , 
	"QDDT_ACO_OCP" FLOAT(126) DEFAULT 0 NOT NULL  , 
	"QDDT_ACO_OCD" FLOAT(126) DEFAULT 0 NOT NULL  , 
	"QDDT_ACO_MAT_COP" FLOAT(126) DEFAULT 0 NOT NULL  , 
	"QDDT_ACO_MAT_COD" FLOAT(126) DEFAULT 0 NOT NULL  , 
	"QDDT_ACO_MAT_OLP" FLOAT(126) DEFAULT 0 NOT NULL  , 
	"QDDT_ACO_MAT_OLD" FLOAT(126) DEFAULT 0 NOT NULL  , 
	"QDDT_ACO_TRA" FLOAT(126) DEFAULT 0 NOT NULL  , 
	"QTOT_DISP_DEFI" FLOAT(126) DEFAULT 0 NOT NULL  , 
	"QTOT_DISP_TOT" FLOAT(126) DEFAULT 0 NOT NULL  , 
	"DATACR" DATE, 
	"DATAVR" DATE, 
	"UTENTE" VARCHAR2(10 BYTE), 
	"QDDE_ACE_MAT_REP" FLOAT(126) DEFAULT 0 NOT NULL  , 
	"QIND_MAT_RIL_REP" FLOAT(126) DEFAULT 0 NOT NULL  , 
	"QDDT_FAB_MAT_REP" FLOAT(126) DEFAULT 0 NOT NULL  , 
	"QIND_RCH_APP" FLOAT(126) DEFAULT 0 NOT NULL  , 
	"QADT_ORD_CHQ" FLOAT(126) DEFAULT 0 NOT NULL  , 
	"QDDE_IN_CHQ" FLOAT(126) DEFAULT 0 NOT NULL  );

  CREATE INDEX IDX_MONITOR_SALDI_GEN ON MONITOR_SALDI_GEN(DATAELABORAZIONE,IDTABMAG); 
  