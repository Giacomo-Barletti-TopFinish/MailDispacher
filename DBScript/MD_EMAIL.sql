DROP TABLE MD_EMAIL;
DROP TABLE MD_EMAIL_LOG;
DROP TABLE MD_EMAIL_STATO;
drop table MD_EMAIL_DESTINATARI;
drop table MD_allegati;
drop table MD_LOG;
DROP TABLE MD_GRUPPI_APP;
DROP TABLE MD_GRUPPI_DESTINATARI;
DROP TABLE MD_GRUPPI;

CREATE TABLE MD_EMAIL_STATO
(
  CODICE VARCHAR2(3) NOT NULL, 
  DESCRIZIONE VARCHAR2(30)NOT NULL,
   CONSTRAINT PK_CODICE PRIMARY KEY (CODICE)
);

INSERT INTO MD_EMAIL_STATO (CODICE, DESCRIZIONE) VALUES ('CRT','CREAZIONE');
INSERT INTO MD_EMAIL_STATO (CODICE, DESCRIZIONE) VALUES ('DIN','DA INVIARE');
INSERT INTO MD_EMAIL_STATO (CODICE, DESCRIZIONE) VALUES ('ERR','ERRORE');
INSERT INTO MD_EMAIL_STATO (CODICE, DESCRIZIONE) VALUES ('INV','INVIATA');
INSERT INTO MD_EMAIL_STATO (CODICE, DESCRIZIONE) VALUES ('BLK','BLOCCATA');

CREATE TABLE MD_RICHIEDENTI
(
  IDRICHIEDENTE NUMBER GENERATED BY DEFAULT ON NULL AS IDENTITY,
  RICHIEDENTE VARCHAR2(50) NOT NULL,
   CONSTRAINT IDRICHIEDENTE PRIMARY KEY (IDRICHIEDENTE)
);

CREATE UNIQUE INDEX IDX_RICHIEDENTE_1 ON MD_RICHIEDENTI(RICHIEDENTE);

CREATE TABLE MD_EMAIL
(
  IDMAIL NUMBER GENERATED BY DEFAULT ON NULL AS IDENTITY,   
  TENTATIVO NUMBER(1 )NULL,
  IDRICHIEDENTE NUMBER NOT NULL,
  DATACREAZIONE DATE NOT NULL,
  DATAINVIO DATE NULL,
  STATO VARCHAR(3) NOT NULL,
  OGGETTO VARCHAR2(50) NOT NULL,
  CORPO VARCHAR2(4000) NOT NULL,
   CONSTRAINT PK_IDMAIL PRIMARY KEY (IDMAIL),
    FOREIGN KEY (STATO)REFERENCES MD_EMAIL_STATO (CODICE) ENABLE,
    FOREIGN KEY (IDRICHIEDENTE)REFERENCES MD_RICHIEDENTI (IDRICHIEDENTE) ENABLE
);

CREATE INDEX IDX_MD_EMAIL_2 ON MD_EMAIL (STATO);

CREATE TABLE MD_GRUPPI
(
  IDGRUPPO NUMBER GENERATED BY DEFAULT ON NULL AS IDENTITY ,   
  NOME VARCHAR2(30) NOT NULL,
  CONSTRAINT PK_IDGRUPPO PRIMARY KEY (IDGRUPPO)
);
CREATE TABLE MD_GRUPPI_DESTINATARI
(
  IDDESTINATARIO NUMBER GENERATED BY DEFAULT ON NULL AS IDENTITY,
  IDGRUPPO NUMBER ,   
  DESTINATARIO VARCHAR2(100 )NOT NULL,
  CONSTRAINT PK_IDDESTINATARIO PRIMARY KEY (IDDESTINATARIO),
   FOREIGN KEY (IDGRUPPO)REFERENCES MD_GRUPPI (IDGRUPPO) ENABLE
);

CREATE INDEX idx_MD_GRUPPI_DESTINATARI_1 ON MD_GRUPPI_DESTINATARI(IDGRUPPO);

DROP TABLE MD_GRUPPI_RICHIEDENTI;
CREATE TABLE MD_GRUPPI_RICHIEDENTI
(
  IDGRRICH NUMBER GENERATED BY DEFAULT ON NULL AS IDENTITY,
  IDRICHIEDENTE NUMBER NOT NULL,
  IDGRUPPO NUMBER NOT NULL,
  A_CC VARCHAR(1) NOT NULL,
CONSTRAINT PK_IDGRRICH PRIMARY KEY (IDGRRICH),
FOREIGN KEY (IDGRUPPO)REFERENCES MD_GRUPPI (IDGRUPPO) ENABLE,
   FOREIGN KEY (IDRICHIEDENTE)REFERENCES MD_RICHIEDENTI (IDRICHIEDENTE) ENABLE
);

CREATE INDEX IDX_MD_GRUPPI_RICHIEDENTI_1 ON MD_GRUPPI_RICHIEDENTI(IDRICHIEDENTE);
CREATE INDEX IDX_MD_GRUPPI_RICHIEDENTI_2 ON MD_GRUPPI_RICHIEDENTI(IDGRUPPO);

DROP TABLE MD_ALLEGATI;
CREATE TABLE MD_ALLEGATI
(
  IDEMAIL_ALL NUMBER GENERATED BY DEFAULT ON NULL AS IDENTITY,
  IDMAIL NUMBER NOT NULL,
  FILECONTENT BLOB NOT NULL,
  FILENAME VARCHAR2(50) NOT NULL,
  CONSTRAINT PK_IDEMAIL_ALL PRIMARY KEY (IDEMAIL_ALL),
    FOREIGN KEY (IDMAIL)REFERENCES MD_EMAIL (IDMAIL) ENABLE
);

CREATE INDEX IDX_MD_ALLEGATI_1 ON MD_ALLEGATI(IDMAIL);

drop table md_log;
CREATE TABLE MD_LOG
(
IDMAIL_LOG NUMBER GENERATED BY DEFAULT ON NULL AS IDENTITY,
IDMAIL NUMBER NULL,
DATAOPERAZIONE DATE NOT NULL,
TIPOOPERAZIONE VARCHAR2(30) NULL,
NOTA VARCHAR2(200) ,
CONSTRAINT PK_IDMAIL_LOG PRIMARY KEY (IDMAIL_LOG),
    FOREIGN KEY (IDMAIL)REFERENCES MD_EMAIL (IDMAIL) ENABLE

);

CREATE INDEX IDX_MD_LOG_1 ON MD_LOG(IDMAIL);
