using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SQLite.ORM.Columns
{
    public class PropertyTypeTableMappingColumn : AbstractTableMappingColumn
    {
        PropertyInfo _prop;

        public string PropertyName { get { return _prop.Name; } }

        public override int? MaxStringLength { get; protected set; }

        public PropertyTypeTableMappingColumn(PropertyInfo prop, CreateFlags createFlags = CreateFlags.None)
        {
            var colAttr = (ColumnAttribute)prop.GetCustomAttributes(typeof(ColumnAttribute), true).FirstOrDefault();

            _prop = prop;
            Name = colAttr == null ? prop.Name : colAttr.Name;
            //If this type is Nullable<T> then Nullable.GetUnderlyingType returns the T, otherwise it returns null, so get the actual type instead
            TargetType = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;
            Collation = Orm.Collation(prop);

            IsPK = Orm.IsPK(prop) ||
                (((createFlags & CreateFlags.ImplicitPK) == CreateFlags.ImplicitPK) &&
                    string.Compare(prop.Name, Orm.ImplicitPkName, StringComparison.OrdinalIgnoreCase) == 0);

            var isAuto = Orm.IsAutoInc(prop) || (IsPK && ((createFlags & CreateFlags.AutoIncPK) == CreateFlags.AutoIncPK));
            IsAutoGuid = isAuto && TargetType == typeof(Guid);
            IsAutoInc = isAuto && !IsAutoGuid;

            Indices = Orm.GetIndices(prop);
            if (!Indices.Any()
                && !IsPK
                && ((createFlags & CreateFlags.ImplicitIndex) == CreateFlags.ImplicitIndex)
                && Name.EndsWith(Orm.ImplicitIndexSuffix, StringComparison.OrdinalIgnoreCase)
                )
            {
                Indices = new IndexedAttribute[] { new IndexedAttribute() };
            }
            IsNullable = !(IsPK || Orm.IsMarkedNotNull(prop));
            MaxStringLength = Orm.MaxStringLength(prop);
        }

        public override void SetValue(object obj, object val)
        {
            _prop.SetValue(obj, val, null);
        }

        public override object GetValue(object obj)
        {
            return _prop.GetValue(obj, null);
        }
    }
}
