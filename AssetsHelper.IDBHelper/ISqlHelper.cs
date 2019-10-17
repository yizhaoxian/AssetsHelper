using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetsHelper.IDBHelper
{
    public interface ISqlHelper
    {
        IEnumerable<T> GetTest<T>();
    }
}
