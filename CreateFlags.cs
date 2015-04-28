using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLite
{
    [Flags]
    public enum CreateFlags
    {
        None = 0x000,
        ImplicitPK = 0x001,    // create a primary key for field called 'Id' (Orm.ImplicitPkName)
        ImplicitIndex = 0x002,    // create an index for fields ending in 'Id' (Orm.ImplicitIndexSuffix)
        AllImplicit = 0x003,    // do both above
        AutoIncPK = 0x004,    // force PK field to be auto inc
        FullTextSearch3 = 0x100,    // create virtual table using FTS3
        FullTextSearch4 = 0x200     // create virtual table using FTS4
    }
}
