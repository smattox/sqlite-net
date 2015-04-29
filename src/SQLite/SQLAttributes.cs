using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLite
{
    [AttributeUsage(AttributeTargets.Class)]
    public class TableAttribute : Attribute
    {
        public string Name { get; set; }

        public TableAttribute(string name)
        {
            Name = name;
        }
    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class ColumnAttribute : Attribute
    {
        public string Name { get; set; }

        public ColumnAttribute(string name)
        {
            Name = name;
        }
    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class PrimaryKeyAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class AutoIncrementAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class IndexedAttribute : Attribute
    {
        public string Name { get; set; }
        public int Order { get; set; }
        public virtual bool Unique { get; set; }

        public IndexedAttribute()
        {
        }

        public IndexedAttribute(string name, int order)
        {
            Name = name;
            Order = order;
        }
    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class IgnoreAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class UniqueAttribute : IndexedAttribute
    {
        public override bool Unique
        {
            get { return true; }
            set { /* throw?  */ }
        }
    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class MaxLengthAttribute : Attribute
    {
        public int Value { get; private set; }

        public MaxLengthAttribute(int length)
        {
            Value = length;
        }
    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class CollationAttribute : Attribute
    {
        public string Value { get; private set; }

        public CollationAttribute(string collation)
        {
            Value = collation;
        }
    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class NotNullAttribute : Attribute
    {
    }
}
