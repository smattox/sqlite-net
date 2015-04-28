using SQLite.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLite.Query
{
    public class NotifyTableChangedEventArgs : EventArgs
    {
        public TableMapping Table { get; private set; }
        public NotifyTableChangedAction Action { get; private set; }

        public NotifyTableChangedEventArgs(TableMapping table, NotifyTableChangedAction action)
        {
            Table = table;
            Action = action;
        }

        public enum NotifyTableChangedAction
        {
            Insert,
            Update,
            Delete,
        }
    }
}
