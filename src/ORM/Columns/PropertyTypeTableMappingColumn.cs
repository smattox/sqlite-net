using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SQLite.ORM.Columns
{
    public class PropertyTypeTableMappingColumn : AbstractDirectTableMappingColumn
    {
        private PropertyInfo _prop;

        public PropertyTypeTableMappingColumn(PropertyInfo prop, string path, CreateFlags createFlags = CreateFlags.None)
            : base(prop, path, createFlags)
        {
            _prop = prop;

            //If this type is Nullable<T> then Nullable.GetUnderlyingType returns the T, otherwise it returns null, so get the actual type instead
            TargetType = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;
        }

        public override void SetValue(object obj, object val)
        {
            obj = GetTargetObject(obj);
            _prop.SetValue(obj, val, null);
        }

        public override object GetValue(object obj)
        {
            obj = GetTargetObject(obj);
            return _prop.GetValue(obj, null);
        }
    }
}
