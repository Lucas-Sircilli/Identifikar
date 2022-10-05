using IdentifikarBio.Suporte;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IdentifikarBio.Database
{
    public class FactoryDAO : IDisposable
    {
        public MySqlConnection cn;
        public String codigoInserido;
        public FactoryDAO()
        {
            cn = abrirConexao();
        }

        public FactoryDAO(int t)
        {
            cn = abrirConexao();
        }



        public MySqlConnection conexao
        {
            get
            {
                string connectionString =
                  "Server=localhost;" +
                  "Database=indetifikar_db;" +
                  "User ID=root;" +
                  "Password=suportegvd249;" +
                  "Pooling=false; convert zero datetime=True";
                if (cn == null)
                    cn = new MySqlConnection(connectionString);

                return cn;
            }
        }

        public MySqlConnection conexaoExterna
        {
            get
            {
                string connectionString =
                  "Server=localhost;" +
                  "Database=indetifikar_db;" +
                  "User ID=root;" +
                  "Password=suportegvd249;" +
                  "Pooling=false; convert zero datetime=True";
                MySqlConnection cn = new MySqlConnection(connectionString);

                return cn;
            }
        }


        public MySqlConnection abrirConexao()
        {
            MySqlConnection cnx = conexao;

            try
            {
                if (cnx.State != ConnectionState.Open)
                    cnx.Open();
                return cnx;
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        public MySqlConnection abrirConexao2()
        {
            MySqlConnection cn = conexaoExterna;

            try
            {
                if (cn.State != ConnectionState.Open)
                    cn.Open();
                return cn;
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        public void fecharConexao(LogFile log)
        {
            try
            {
                cn.Close();
            }
            catch (Exception e)
            {
                log.write("FALHA NA TENTATIVA DE FECHAR CONEXAO COM O BANCO", e);
            }
        }

        public DataTable executeQuery(string sql, LogFile log)
        {
            try
            {
                var contador = 0;
                while (cn.State != ConnectionState.Open && contador < 3)
                {
                    contador++;
                    Thread.Sleep(1000);
                    cn = abrirConexao();
                }

                if (contador >= 3) { log.write("Estourou tentativas de conexao [executeQuery]"); }

                //log.write("SQL: " + sql);
                IDbCommand sqlComm = cn.CreateCommand();
                sqlComm.CommandText = sql;

                //sqlComm.ExecuteNonQuery();
                IDataReader reader = sqlComm.ExecuteReader();

                DataTable dt = new DataTable();
                dt.Load(reader);
                reader.Close();
                return dt;
            }
            catch (Exception e)
            {
                log.write("executeQuery: " + sql, e);
            }
            return null;
        }

        public int executeNonQuery(string sql, LogFile log)
        {            
                var contador = 0;
                while (cn.State != ConnectionState.Open && contador < 3)
                {
                    contador++;
                    Thread.Sleep(1000);
                    cn = abrirConexao();
                }

                if (contador >= 3) { log.write("Estourou tentativas de conexao [executeNonQuery]"); }

                //log.write("SQL: " + sql);
                IDbCommand sqlComm = cn.CreateCommand();
                sqlComm.CommandText = sql;

                //sqlComm.ExecuteNonQuery();
                return sqlComm.ExecuteNonQuery();
         }



        public string executeQueryComStringRetorno(string sql, LogFile log)
        {
            try
            {
                var contador = 0;
                while (cn.State != ConnectionState.Open && contador < 3)
                {
                    contador++;
                    Thread.Sleep(1000);
                    cn = abrirConexao();
                }

                if (contador >= 5) { log.write("Estourou tentativas de conexao [executeQueryComStringRetorno]"); }

                string dado;

                IDbCommand sqlComm = cn.CreateCommand();
                sqlComm.CommandText = sql;

                //sqlComm.ExecuteNonQuery();
                IDataReader reader = sqlComm.ExecuteReader();

                DataTable dt = new DataTable();
                dt.Load(reader);
                reader.Close();
                dado = dt.Rows[0][0].ToString();

                return dado;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        

        public void Dispose()
        {
            try
            {
                cn.Close();
            }
            catch (Exception e)
            {

            }
        }
    }
}
