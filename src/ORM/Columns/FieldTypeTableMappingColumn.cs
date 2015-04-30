using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SQLite.ORM.Columns
{
    public class FieldTypeTableMappingColumn : AbstractTableMappingColumn
    {
        private FieldInfo _field;

        public FieldTypeTableMappingColumn(FieldInfo field, string path, CreateFlags createFlags = CreateFlags.None)
            : base(field, path, createFlags)
        {
            _field = field;

            //If this type is Nullable<T> then Nullable.GetUnderlyingType returns the T, otherwise it returns null, so get the actual type instead
            TargetType = Nullable.GetUnderlyingType(field.FieldType) ?? field.FieldType;
        }

        public override void SetValue(object obj, object val)
        {
            obj = GetTargetObject(obj);
            _field.SetValue(obj, val);
        }

        public override object GetValue(object obj)
        {
            obj = GetTargetObject(obj);
            return _field.GetValue(obj);
        }
    }
}
