using SQLite.ORM.Columns.PropertyCollection;
using SQLite.ORM.Columns.PropertyAttributeChecker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SQLite.ORM.Columns
{
    public class StandardWrappedPublicPropertyCollector : PropertyCollector
    {
        private PropertyCollector innerCollector;
        private IPropertyAttributeChecker checker;

        public StandardWrappedPublicPropertyCollector() :
            this(PropertyCollectorFactory.Create(),
                 PropertyAttributeCheckerFactory.Create()) { }

        public StandardWrappedPublicPropertyCollector(PropertyCollector collector,
            IPropertyAttributeChecker checker)
        {
            this.innerCollector = collector;
            this.checker = checker;
        }

        public PropertyInfo[] Collect(Type type)
        {
            return innerCollector.Collect(type).Where(property => property.CanWrite &&
                !checker.PropertyHasAttribute(property, typeof(IgnoreAttribute))).ToArray();
        }
    }
}
