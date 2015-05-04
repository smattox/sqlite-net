using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SQLite.ORM.Columns
{
    public abstract class AbstractTableMappingColumn : TableMappingColumn
    {
        public string Name { get; protected set; }

        public Type TargetType { get; protected set; }

        public string TargetName { get; protected set; }

        public IEnumerable<IndexedAttribute> Indices { get; set; }

        protected string Path { get; set; }

        protected AbstractTableMappingColumn(MemberInfo info, string path, CreateFlags createFlags = CreateFlags.None)
        {
            Name = path + ORMUtilities.GetColumnName(info);
            Path = path;
            
            Indices = ORMUtilities.GetIndices(info);
        }

        protected object GetTargetObject(object source)
        {
            // hackish
            string cleanedPath = Regex.Replace(Path, "\\[.+\\]\\.", "");
            string[] references = cleanedPath.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i != references.Length; i++)
            {
                object newSource = ORMUtilities.GetMemberValue(source, references[i]);
                if (newSource == null)
                {
                    newSource = ORMUtilities.InstantiateMember(source, references[i]);
                }
                source = newSource;
            }
            return source;
        }
    }
}
