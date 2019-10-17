using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetsHelper.DBHelper
{
    public interface IDBHelper
    {
        T GetById<T>(int id) where T : class, new(); 
    }
}
