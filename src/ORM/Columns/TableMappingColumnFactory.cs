﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SQLite.ORM.Columns
{
    public interface TableMappingColumnFactory
    {
        TableMappingColumn[] CreateColumnsOnProperty(PropertyInfo property, CreateFlags flags);

        TableMappingColumn[] CreateColumnsOnField(FieldInfo field, CreateFlags flags);
    }
}