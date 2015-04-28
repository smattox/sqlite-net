using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SQLite
{
    public partial class SQLiteAsyncConnection
    {
        SQLiteConnectionString _connectionString;
        SQLiteOpenFlags _openFlags;

        public SQLiteAsyncConnection(string databasePath, bool storeDateTimeAsTicks = true)
            : this(databasePath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create, storeDateTimeAsTicks)
        {
        }

        public SQLiteAsyncConnection(string databasePath, SQLiteOpenFlags openFlags, bool storeDateTimeAsTicks = true)
        {
            _openFlags = openFlags;
            _connectionString = new SQLiteConnectionString(databasePath, storeDateTimeAsTicks);
        }

        SQLiteConnectionWithLock GetConnection()
        {
            return SQLiteConnectionPool.Shared.GetConnection(_connectionString, _openFlags);
        }

        public Task<CreateTablesResult> CreateTableAsync<T>(CreateFlags createFlags = CreateFlags.None)
            where T : new()
        {
            return CreateTablesAsync(createFlags, typeof(T));
        }

        public Task<CreateTablesResult> CreateTablesAsync<T, T2>(CreateFlags createFlags = CreateFlags.None)
            where T : new()
            where T2 : new()
        {
            return CreateTablesAsync(createFlags, typeof(T), typeof(T2));
        }

        public Task<CreateTablesResult> CreateTablesAsync<T, T2, T3>(CreateFlags createFlags = CreateFlags.None)
            where T : new()
            where T2 : new()
            where T3 : new()
        {
            return CreateTablesAsync(createFlags, typeof(T2), typeof(T3));
        }

        public Task<CreateTablesResult> CreateTablesAsync<T, T2, T3, T4>(CreateFlags createFlags = CreateFlags.None)
            where T : new()
            where T2 : new()
            where T3 : new()
            where T4 : new()
        {
            return CreateTablesAsync(createFlags, typeof(T2), typeof(T3), typeof(T4));
        }

        public Task<CreateTablesResult> CreateTablesAsync<T, T2, T3, T4, T5>(CreateFlags createFlags = CreateFlags.None)
            where T : new()
            where T2 : new()
            where T3 : new()
            where T4 : new()
            where T5 : new()
        {
            return CreateTablesAsync(createFlags, typeof(T), typeof(T2), typeof(T3), typeof(T4), typeof(T5));
        }

        public Task<CreateTablesResult> CreateTablesAsync(CreateFlags createFlags = CreateFlags.None, params Type[] types)
        {
            return Task.Factory.StartNew(() =>
            {
                CreateTablesResult result = new CreateTablesResult();
                var conn = GetConnection();
                using (conn.Lock())
                {
                    foreach (Type type in types)
                    {
                        int aResult = conn.CreateTable(type, createFlags);
                        result.Results[type] = aResult;
                    }
                }
                return result;
            });
        }

        public Task<int> DropTableAsync<T>()
            where T : new()
        {
            return Task.Factory.StartNew(() =>
            {
                var conn = GetConnection();
                using (conn.Lock())
                {
                    return conn.DropTable<T>();
                }
            });
        }

        public Task<int> InsertAsync(object item)
        {
            return Task.Factory.StartNew(() =>
            {
                var conn = GetConnection();
                using (conn.Lock())
                {
                    return conn.Insert(item);
                }
            });
        }

        public Task<int> InsertOrReplaceAsync(object item)
        {
            return Task.Factory.StartNew(() =>
            {
                var conn = GetConnection();
                using (conn.Lock())
                {
                    return conn.InsertOrReplace(item);
                }
            });
        }

        public Task<int> UpdateAsync(object item)
        {
            return Task.Factory.StartNew(() =>
            {
                var conn = GetConnection();
                using (conn.Lock())
                {
                    return conn.Update(item);
                }
            });
        }

        public Task<int> DeleteAsync(object item)
        {
            return Task.Factory.StartNew(() =>
            {
                var conn = GetConnection();
                using (conn.Lock())
                {
                    return conn.Delete(item);
                }
            });
        }

        public Task<T> GetAsync<T>(object pk)
            where T : new()
        {
            return Task.Factory.StartNew(() =>
            {
                var conn = GetConnection();
                using (conn.Lock())
                {
                    return conn.Get<T>(pk);
                }
            });
        }

        public Task<T> FindAsync<T>(object pk)
            where T : new()
        {
            return Task.Factory.StartNew(() =>
            {
                var conn = GetConnection();
                using (conn.Lock())
                {
                    return conn.Find<T>(pk);
                }
            });
        }

        public Task<T> GetAsync<T>(Expression<Func<T, bool>> predicate)
            where T : new()
        {
            return Task.Factory.StartNew(() =>
            {
                var conn = GetConnection();
                using (conn.Lock())
                {
                    return conn.Get<T>(predicate);
                }
            });
        }

        public Task<T> FindAsync<T>(Expression<Func<T, bool>> predicate)
            where T : new()
        {
            return Task.Factory.StartNew(() =>
            {
                var conn = GetConnection();
                using (conn.Lock())
                {
                    return conn.Find<T>(predicate);
                }
            });
        }

        public Task<int> ExecuteAsync(string query, params object[] args)
        {
            return Task<int>.Factory.StartNew(() =>
            {
                var conn = GetConnection();
                using (conn.Lock())
                {
                    return conn.Execute(query, args);
                }
            });
        }

        public Task<int> InsertAllAsync(IEnumerable items)
        {
            return Task.Factory.StartNew(() =>
            {
                var conn = GetConnection();
                using (conn.Lock())
                {
                    return conn.InsertAll(items);
                }
            });
        }

        public Task<int> UpdateAllAsync(IEnumerable items)
        {
            return Task.Factory.StartNew(() =>
            {
                var conn = GetConnection();
                using (conn.Lock())
                {
                    return conn.UpdateAll(items);
                }
            });
        }

        [Obsolete("Will cause a deadlock if any call in action ends up in a different thread. Use RunInTransactionAsync(Action<SQLiteConnection>) instead.")]
        public Task RunInTransactionAsync(Action<SQLiteAsyncConnection> action)
        {
            return Task.Factory.StartNew(() =>
            {
                var conn = this.GetConnection();
                using (conn.Lock())
                {
                    conn.BeginTransaction();
                    try
                    {
                        action(this);
                        conn.Commit();
                    }
                    catch (Exception)
                    {
                        conn.Rollback();
                        throw;
                    }
                }
            });
        }

        public Task RunInTransactionAsync(Action<SQLiteConnection> action)
        {
            return Task.Factory.StartNew(() =>
            {
                var conn = this.GetConnection();
                using (conn.Lock())
                {
                    conn.BeginTransaction();
                    try
                    {
                        action(conn);
                        conn.Commit();
                    }
                    catch (Exception)
                    {
                        conn.Rollback();
                        throw;
                    }
                }
            });
        }

        public AsyncTableQuery<T> Table<T>()
            where T : new()
        {
            //
            // This isn't async as the underlying connection doesn't go out to the database
            // until the query is performed. The Async methods are on the query iteself.
            //
            var conn = GetConnection();
            return new AsyncTableQuery<T>(conn.Table<T>());
        }

        public Task<T> ExecuteScalarAsync<T>(string sql, params object[] args)
        {
            return Task<T>.Factory.StartNew(() =>
            {
                var conn = GetConnection();
                using (conn.Lock())
                {
                    var command = conn.CreateCommand(sql, args);
                    return command.ExecuteScalar<T>();
                }
            });
        }

        public Task<List<T>> QueryAsync<T>(string sql, params object[] args)
            where T : new()
        {
            return Task<List<T>>.Factory.StartNew(() =>
            {
                var conn = GetConnection();
                using (conn.Lock())
                {
                    return conn.Query<T>(sql, args);
                }
            });
        }
    }

}
