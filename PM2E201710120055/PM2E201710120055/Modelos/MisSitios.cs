using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace PM2E201710120055.Modelos
{
    public class MisSitios
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }
        public string descripcion { get; set; }
        public string latitud { get; set; }
        public string longitud { get; set; }
        public string fotografia { get; set; }

    }
}
