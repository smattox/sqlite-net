using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLite.ORM
{
    public interface TableMappingFactory
    {
        TableMapping Create(Type type, CreateFlags flags = CreateFlags.None);
    }
}
