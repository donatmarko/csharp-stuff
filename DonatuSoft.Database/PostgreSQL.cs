using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DonatuSoft.Database
{
    /// <summary>
    /// PostgreSQL adatbázisok kezelésére alkalmas osztály.
    /// ODBC interfészt használ.
    /// Elengedhetetlen a számítógépre telepített psqlodbc driver.
    /// A Tools könyvtárban mellékelve mindkét platformhoz.
    /// </summary>
    public class PostgreSQL
    {
        public static bool StringToBool(string szöveg)
        {
            return szöveg == "1";
        }

        enum PostgresDriver { x86, x64 }
        OdbcConnection connection;

        string server = "localhost";
        int port = 5432;
        string username;
        string password;
        string database;

        /// <summary>
        /// PostgreSQL ODBC connection stringet ad vissza.
        /// </summary>
        /// <param name="driver">x64-es, vagy x86-os driverre van szükség?</param>
        /// <returns>PostgreSQL ODBC connection stringet ad vissza.</returns>
        string getConnectionString(PostgresDriver driver)
        {
            if (driver == PostgresDriver.x64)
                return string.Format("Driver={{PostgreSQL Unicode(x64)}};Server={0};Port={1};Database={2};Uid={3};Pwd={4};", server, port, database, username, password);
            else
                if (driver == PostgresDriver.x86)
                return string.Format("Driver={{PostgreSQL Unicode}};Server={0};Port={1};Database={2};Uid={3};Pwd={4};", server, port, database, username, password);
            else
                return "";
        }

        public PostgreSQL(string server, string username, string password, string database)
        {
            this.server = server;
            this.username = username;
            this.password = password;
            this.database = database;
        }

        public PostgreSQL(string server, int port, string username, string password, string database)
        {
            this.server = server;
            this.port = port;
            this.username = username;
            this.password = password;
            this.database = database;
        }

        public PostgreSQL(string username, string password, string database)
        {
            this.username = username;
            this.password = password;
            this.database = database;
        }

        /// <summary>
        /// Kapcsolódik a PostgreSQL adatbázishoz.
        /// Eldönti, hogy x64-es vagy x86-os driverre van-e szükség, ellenkező esetben hibát dob.
        /// </summary>
        public void Connect()
        {
            try
            {
                // x64 driver
                connection = new OdbcConnection(getConnectionString(PostgresDriver.x64));
                connection.Open();
            }
            catch
            {
                // x86 driver
                connection = new OdbcConnection(getConnectionString(PostgresDriver.x86));
                connection.Open();
            }
        }

        /// <summary>
        /// Lekapcsolódik a PostgreSQL adatbázisról.
        /// </summary>
        public void Disconnect()
        {
            connection.Close();
            connection.Dispose();
        }

        /// <summary>
        /// Olyan SQL műveletek futtatására alkalmas, amelyeknek nincs eredményhalmaza.
        /// </summary>
        /// <param name="SQL">az SQL query</param>
        public void Execute(string SQL)
        {
            OdbcCommand cmd = connection.CreateCommand();
            cmd.CommandText = SQL;
            cmd.ExecuteNonQuery();
        }

        public OdbcDataReader GetReader(string SQL)
        {
            OdbcCommand cmd = connection.CreateCommand();
            cmd.CommandText = SQL;
            return cmd.ExecuteReader();
        }

        public object GetScalar(string SQL)
        {
            OdbcCommand cmd = connection.CreateCommand();
            cmd.CommandText = SQL;
            return cmd.ExecuteScalar();
        }

        public DataSet GetDataSet(string SQL)
        {
            OdbcDataAdapter da = new OdbcDataAdapter(SQL, connection);
            var ds = new DataSet();
            da.Fill(ds);
            return ds;
        }

        public DataTable GetDataTable(string SQL)
        {
            OdbcDataAdapter da = new OdbcDataAdapter(SQL, connection);
            var dt = new DataTable();
            da.Fill(dt);
            return dt;
        }
    }
}
