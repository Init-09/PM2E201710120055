using PM2E201710120055.Modelos;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PM2E201710120055.Controles
{
   public class DataBaseSQLite
    {
        readonly SQLiteAsyncConnection db;
        public DataBaseSQLite(string pathdb)
        {
            db = new SQLiteAsyncConnection(pathdb);
            db.CreateTableAsync<MisSitios>().Wait();
        }
        public Task<List<MisSitios>> ObtenerListaSitios()
        {
            return db.Table<MisSitios>().ToListAsync();
        }
        public Task<MisSitios> ObtenerSitios(int pcodigo)
        {
            return db.Table<MisSitios>()
                .Where(i => i.id == pcodigo)
                .FirstOrDefaultAsync();
        }

        public Task<int> GrabarSitios(MisSitios s)
        {
            if (s.id != 0)
            {
                return db.UpdateAsync(s);
            }
            else
            {
                return db.InsertAsync(s);
            }

        }

        public Task<int> EliminarSitios(MisSitios s)
        {
            return db.DeleteAsync(s);
        }


    }
}
