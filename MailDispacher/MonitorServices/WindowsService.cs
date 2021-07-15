using MonitorServices.Entities;
using MonitorServices.Properties;
using MonitorServices.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Topshelf.Logging;

namespace MonitorServices
{
    public class WindowsService
    {
        private LogWriter _log = HostLogger.Get<WindowsService>();
        private Timer _timer;
        object lockObject = new object();
        public void Start()
        {
            try
            {
                string messaggio = string.Format("Timer impostato a {0} minuti", Settings.Default.TimerPeriod);
                _timer = new Timer(TimerCallBack, null, 1L, Settings.Default.TimerPeriod * 60 * 1000);
                _log.Info("Servizio Monitor avviato");

            }
            catch (Exception ex)
            {
                _log.Error("Errore in fase di start del servizio", ex);

            }
        }

        public void Stop()
        {
            try
            {
                _timer.Dispose();
                _log.Info("Servizio Monitor fermato");
            }
            catch (Exception ex)
            {
                _log.Error("Errore in fase di stop del servizio", ex);
            }
        }

        private void TimerCallBack(Object stateInfo)
        {
            if (Monitor.TryEnter(lockObject))
            {
                try
                {
                    MonitorService sMonitor = new MonitorService();
                    MonitorDS.MONITOR_SCHEDULERRow schedulazione;


                    if (sMonitor.VerificaEsecuzione("MAGAZZININEGATIVI", out schedulazione))
                    {
                        MagazzinoMonitor mMagazzino = new MagazzinoMonitor();
                        mMagazzino.VerificaSaldiNegativi();
                        sMonitor.AggiornaSchedulazione(schedulazione);
                    }

                    if (sMonitor.VerificaEsecuzione("MAGAZZINIGIACENZE", out schedulazione))
                    {
                        MagazzinoMonitor mMagazzino = new MagazzinoMonitor();
                        mMagazzino.VerificaGiacenze();
                        sMonitor.AggiornaSchedulazione(schedulazione);
                    }

                    if (sMonitor.VerificaEsecuzione("SALDIUBICAZIONI", out schedulazione))
                    {
                        MagazzinoMonitor mMagazzino = new MagazzinoMonitor();
                        mMagazzino.SaldiUbicazioni();
                        sMonitor.AggiornaSchedulazione(schedulazione);
                    }

                    if (sMonitor.VerificaEsecuzione("PRELIEVI", out schedulazione))
                    {
                        PrelieviService prelievi = new PrelieviService();
                        prelievi.CreaCopiaPrelievi();
                        prelievi.CreaCopiaTrasferimenti();
                        prelievi.CreaCopiaFattibilita();
                        sMonitor.AggiornaSchedulazione(schedulazione);
                    }

                    if (sMonitor.VerificaEsecuzione("VERIFICAREPLICHE", out schedulazione))
                    {
                        VerificaRepliche mRepliche = new VerificaRepliche();
                        mRepliche.VerificaReplicheCartelleServer(@"\\DC01\IMMAGINI", @"\\DC02\IMMAGINI", "GIACOMO.BARLETTI", "topPasqua#1", "viamattei");
                        mRepliche.VerificaReplicheCartelleServer(@"\\DC02\IMMAGINI", @"\\DC01\IMMAGINI", "GIACOMO.BARLETTI", "topPasqua#1", "viamattei");
                        mRepliche.VerificaReplicheCartelleServer(@"\\DC01\DATI DISTRIBUITI", @"\\DC02\DATI DISTRIBUITI", "GIACOMO.BARLETTI", "topPasqua#1", "viamattei");
                        mRepliche.VerificaReplicheCartelleServer(@"\\DC02\DATI DISTRIBUITI", @"\\DC01\DATI DISTRIBUITI", "GIACOMO.BARLETTI", "topPasqua#1", "viamattei");
                        sMonitor.AggiornaSchedulazione(schedulazione);
                    }

                    if (sMonitor.VerificaEsecuzione("MAGAZZINIGIACENZEBRANDMANAGER", out schedulazione))
                    {
                        MagazzinoMonitor mMagazzino = new MagazzinoMonitor();
                        mMagazzino.VerificaGiacenzeBrandManager();
                        sMonitor.AggiornaSchedulazione(schedulazione);
                    }

                    if (sMonitor.VerificaEsecuzione("SCARTIDIFETTOSI", out schedulazione))
                    {
                        MagazzinoMonitor mMagazzino = new MagazzinoMonitor();
                        mMagazzino.ScartiDifettosi();
                        sMonitor.AggiornaSchedulazione(schedulazione);
                    }

                    if (sMonitor.VerificaEsecuzione("SCARTIGRECO", out schedulazione))
                    {
                        MagazzinoMonitor mMagazzino = new MagazzinoMonitor();
                        mMagazzino.ScartiGreco();
                        sMonitor.AggiornaSchedulazione(schedulazione);
                    }

                    if (sMonitor.VerificaEsecuzione("ESTRAZIONE_OC", out schedulazione))
                    {
                        MagazzinoMonitor mMagazzino = new MagazzinoMonitor();
                        mMagazzino.EstrazioneOC();
                        sMonitor.AggiornaSchedulazione(schedulazione);
                    }
                }
                catch (Exception ex)
                {
                    _log.Error("Errore Monitor Service", ex);
                    while (ex.InnerException != null)
                    {
                        _log.Error("--- INNER EXCEPTION", ex);
                        ex = ex.InnerException;
                    }
                }
                finally
                {
                    Monitor.Exit(lockObject);
                }
            }
        }
    }
}
