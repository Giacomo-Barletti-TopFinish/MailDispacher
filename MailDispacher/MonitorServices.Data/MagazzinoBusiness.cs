using MonitorServices.Data.Core;
using MonitorServices.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonitorServices.Data
{
    public class MagazzinoBusiness : MonitorBusinessBase
    {
        public MagazzinoBusiness() : base() { }

        [DataContext]
        public void FillMagazziniNegativi(MagazzinoDS ds)
        {
            MagazzinoAdapter a = new MagazzinoAdapter(DbConnection, DbTransaction);
            a.FillMagazziniNegativi(ds);
        }

        [DataContext]
        public void FillMagazziniGiacenza(MagazzinoDS ds)
        {
            MagazzinoAdapter a = new MagazzinoAdapter(DbConnection, DbTransaction);
            a.FillMagazziniGiacenza(ds);
        }

        [DataContext]
        public void FillMONITOR_APPROVVIGIONAMENTO(MagazzinoDS ds)
        {
            MagazzinoAdapter a = new MagazzinoAdapter(DbConnection, DbTransaction);
            a.FillMONITOR_APPROVVIGIONAMENTO(ds);
        }

        [DataContext]
        public void FillGIACENZA_BRAND_MANAGER(MagazzinoDS ds)
        {
            MagazzinoAdapter a = new MagazzinoAdapter(DbConnection, DbTransaction);
            a.FillGIACENZA_BRAND_MANAGER(ds);
        }

        [DataContext]
        public void FillUSR_INVENTARIOS(MagazzinoDS ds)
        {
            MagazzinoAdapter a = new MagazzinoAdapter(DbConnection, DbTransaction);
            a.FillUSR_INVENTARIOS(ds);
        }

        [DataContext(true)]
        public void CopiaPrelievi()
        {
            MagazzinoAdapter a = new MagazzinoAdapter(DbConnection, DbTransaction);
            a.CopiaPrelievi();
        }

        [DataContext(true)]
        public void CopiaTrasferimenti()
        {
            MagazzinoAdapter a = new MagazzinoAdapter(DbConnection, DbTransaction);
            a.CopiaTrasferimentiTF();
            a.CopiaTrasferimentiMP();
        }

        [DataContext(true)]
        public void CopiaFattibilita()
        {
            MagazzinoAdapter a = new MagazzinoAdapter(DbConnection, DbTransaction);
            a.CopiaFattibilitaMP();
            a.CopiaFattibilitaTF();
        }
    }
}
