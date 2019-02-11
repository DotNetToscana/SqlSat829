using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace DataSample.DataAccessLayer.Dapper
{
    public class DapperContext : IDapperContext, IDisposable
    {
        private IDbConnection connection;
        private IDbConnection Connection
        {
            get
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }

                return connection;
            }
        }

        public DapperContext(string connectionString)
        {
            connection = new SqlConnection(connectionString);
        }

        public Task<IEnumerable<T>> GetDataAsync<T>(string sql, object param = null, CommandType? commandType = null) where T : class
            => Connection.QueryAsync<T>(sql, param, commandType: commandType);

        public Task<IEnumerable<TReturn>> GetDataAsync<TFirst, TSecond, TReturn>(string sql, Func<TFirst, TSecond, TReturn> map, object param = null, string splitOn = "Id", CommandType? commandType = null) where TReturn : class
            => Connection.QueryAsync(sql, map, param, splitOn: splitOn, commandType: commandType);

        public Task<IEnumerable<TReturn>> GetDataAsync<TFirst, TSecond, TThird, TReturn>(string sql, Func<TFirst, TSecond, TThird, TReturn> map, object param = null, string splitOn = "Id", CommandType? commandType = null) where TReturn : class
            => Connection.QueryAsync(sql, map, param, splitOn: splitOn, commandType: commandType);
            
        public Task<T> GetObjectAsync<T>(string sql, object param = null, CommandType? commandType = null) where T : class 
            => Connection.QueryFirstOrDefaultAsync<T>(sql, param, commandType: commandType);

        public async Task<TReturn> GetObjectAsync<TFirst, TSecond, TReturn>(string sql, Func<TFirst, TSecond, TReturn> map, object param = null, string splitOn = "Id", CommandType? commandType = null) where TReturn : class
        {
            var result = await Connection.QueryAsync(sql, map, param, splitOn: splitOn, commandType: commandType);
            return result.FirstOrDefault();
        }

        public async Task<TReturn> GetObjectAsync<TFirst, TSecond, TThird, TReturn>(string sql, Func<TFirst, TSecond, TThird, TReturn> map, object param = null, string splitOn = "Id", CommandType? commandType = null) where TReturn : class
        {
            var result = await Connection.QueryAsync(sql, map, param, splitOn: splitOn, commandType: commandType);
            return result.FirstOrDefault();
        }

        public Task<T> GetSingleValueAsync<T>(string sql, object param = null, CommandType? commandType = null) 
            => Connection.ExecuteScalarAsync<T>(sql, param, commandType: commandType);

        public Task InsertAsync(string sql, object param = null, CommandType? commandType = null) 
            => Connection.ExecuteAsync(sql, param, commandType: commandType);

        public Task<T> InsertAsync<T>(string sql, object param = null, CommandType? commandType = null) where T : class
        {
            // Aggiunge il comando SQL per recuperare la chiave del record appena inserito.
            sql += "; SELECT CAST(SCOPE_IDENTITY() AS INT); ";
            return Connection.QuerySingleAsync<T>(sql, param, commandType: commandType);
        }

        public Task UpdateAsync(string sql, object param = null, CommandType? commandType = null) 
            => Connection.ExecuteAsync(sql, param, commandType: commandType);

        public Task DeleteAsync(string sql, object param = null, CommandType? commandType = null) 
            => Connection.ExecuteAsync(sql, param, commandType: commandType);

        public Task ExecuteAsync(string sql, object param = null, CommandType? commandType = null) 
            => Connection.ExecuteAsync(sql, param, commandType: commandType);

        public IDbTransaction BeginTransaction() => Connection.BeginTransaction();

        /// <summary>
        /// Close and dispose of the database connection
        /// </summary>
        public void Dispose()
        {
            if (connection.State == ConnectionState.Open)
            {
                connection.Close();
            }

            connection.Dispose();
            connection = null;
        }
    }
}
