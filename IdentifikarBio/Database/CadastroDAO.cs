using IdentifikarBio.Database.Objetos;
using IdentifikarBio.Suporte;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace IdentifikarBio.Database
{
    public static class CadastroDAO
    {
        

        public static bool InserirCadastro(ObCadastro cad, LogFile log)
        {

            StringBuilder sql = new StringBuilder();
            StringBuilder valores = new StringBuilder();
            try
            {
                using (var gerDB = new FactoryDAO())
                {
                    sql.Clear();
                    sql.AppendLine("INSERT INTO cadastro_tbl (cpf, renach, nome");

                    valores.Append("'").Append(cad.cpf).AppendLine("' ");
                    valores.Append(", '").Append(cad.renach).AppendLine("' ");
                    valores.Append(", '").Append(cad.nome).AppendLine("' ");

                    if (!string.IsNullOrEmpty(cad.sexo))
                    {
                        sql.Append(", sexo");
                        valores.Append(", '").Append(cad.sexo).AppendLine("' ");
                    }

                    if (!string.IsNullOrEmpty(cad.endereco))
                    {
                        sql.Append(", endereco");
                        valores.Append(", '").Append(cad.endereco).AppendLine("' ");
                    }

                    if (!string.IsNullOrEmpty(cad.numero))
                    {
                        sql.Append(", numero");
                        valores.Append(", '").Append(cad.numero).AppendLine("' ");
                    }

                    if (!string.IsNullOrEmpty(cad.bairro))
                    {
                        sql.Append(", bairro");
                        valores.Append(", '").Append(cad.bairro).AppendLine("' ");
                    }

                    if (!string.IsNullOrEmpty(cad.cep))
                    {
                        sql.Append(", cep");
                        valores.Append(", '").Append(cad.cep).AppendLine("' ");
                    }

                    if (!string.IsNullOrEmpty(cad.Municipio))
                    {
                        sql.Append(", municipio");
                        valores.Append(", '").Append(cad.Municipio).AppendLine("' ");
                    }

                    if (!string.IsNullOrEmpty(cad.uf))
                    {
                        sql.Append(", uf");
                        valores.Append(", '").Append(cad.uf).AppendLine("' ");
                    }

                    if (!string.IsNullOrEmpty(cad.DDDTelefone))
                    {
                        sql.Append(", ddd_telefone");
                        valores.Append(", '").Append(cad.DDDTelefone).AppendLine("' ");
                    }

                    if (!string.IsNullOrEmpty(cad.telefone))
                    {
                        sql.Append(", telefone");
                        valores.Append(", '").Append(cad.telefone).AppendLine("' ");
                    }

                    if (!string.IsNullOrEmpty(cad.DDDCelular))
                    {
                        sql.Append(", ddd_celular");
                        valores.Append(", '").Append(cad.DDDCelular).AppendLine("' ");
                    }

                    if (!string.IsNullOrEmpty(cad.telefone2))
                    {
                        sql.Append(", celular");
                        valores.Append(", '").Append(cad.telefone2).AppendLine("' ");
                    }

                    if (!string.IsNullOrEmpty(cad.email))
                    {
                        sql.Append(", email");
                        valores.Append(", '").Append(cad.email).AppendLine("' ");
                    }

                    if (!string.IsNullOrEmpty(cad.registro))
                    {
                        sql.Append(", rg");
                        valores.Append(", '").Append(cad.registro).AppendLine("' ");
                    }

                    if (!string.IsNullOrEmpty(cad.registro_emissor))
                    {
                        sql.Append(", expedido");
                        valores.Append(", '").Append(cad.registro_emissor).AppendLine("' ");
                    }

                    if (!string.IsNullOrEmpty(cad.registro_uf))
                    {
                        sql.Append(", expedido_uf");
                        valores.Append(", '").Append(cad.registro_uf).AppendLine("' ");
                    }

                    if (cad.nascimento != null)
                    {
                        sql.Append(", nascimento");
                        valores.Append(", '").Append(cad.nascimento.ToString()).AppendLine("' ");
                    }

                    if (!string.IsNullOrEmpty(cad.nome_pai))
                    {
                        sql.Append(", filiacao_pai");
                        valores.Append(", '").Append(cad.nome_pai).AppendLine("' ");
                    }

                    if (!string.IsNullOrEmpty(cad.nome_mae))
                    {
                        sql.Append(", filiacao_mae");
                        valores.Append(", '").Append(cad.nome_mae).AppendLine("' ");
                    }

                    if (!string.IsNullOrEmpty(cad.complemento))
                    {
                        sql.Append(", complemento");
                        valores.Append(", '").Append(cad.complemento).AppendLine("' ");
                    }

                    sql.Append(") VALUES (").Append(valores).AppendLine(" )");

                    int resp = gerDB.executeNonQuery(sql.ToString(), log);
                }

            }
            catch (Exception e)
            {
                if (e.Message.Contains("Duplicate entry") && e.Message.Contains("cpfcol_UNIQUE"))
                {
                    MessageBox.Show("Este CPF já esta cadastrado em nosso base de dados", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else if (e.Message.Contains("Duplicate entry") && e.Message.Contains("renach_UNIQUE"))
                {

                    MessageBox.Show("Este RENACH já esta cadastrado em nosso base de dados", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                log.write("FALHA InserirCadastro", e);
            }

            return false;
        }

        public static ObCadastro GetCadastroCpf(string cpf, LogFile log)
        {
            try
            {
                using (var gerDB = new FactoryDAO())
                {
                    StringBuilder sql = new StringBuilder();
                    sql.AppendLine("SELECT * FROM cadastro_tbl ");
                    sql.AppendLine("WHERE cpf='" + cpf + "' limit 1 ");

                    DataTable dt = gerDB.executeQuery(sql.ToString(), log);

                    if (dt.Rows.Count > 0)
                    {
                        DataRow row = dt.Rows[0];

                        ObCadastro a = new ObCadastro();
                        if (row["id_cadastro"] != DBNull.Value)
                            a.id_candidatos = Convert.ToInt32(row["id_cadastro"]);

                        if (row["cpf"] != DBNull.Value)
                            a.cpf = Convert.ToString(row["cpf"]);

                        if (row["renach"] != DBNull.Value)
                            a.renach = Convert.ToString(row["renach"]);

                        if (row["nome"] != DBNull.Value)
                            a.nome = Convert.ToString(row["nome"]);

                        if (row["sexo"] != DBNull.Value)
                            a.sexo = Convert.ToString(row["sexo"]);

                        if (row["endereco"] != DBNull.Value)
                            a.endereco = Convert.ToString(row["endereco"]);

                        if (row["numero"] != DBNull.Value)
                            a.numero = Convert.ToString(row["numero"]);

                        if (row["bairro"] != DBNull.Value)
                            a.bairro = Convert.ToString(row["bairro"]);

                        if (row["cep"] != DBNull.Value)
                            a.cep = Convert.ToString(row["cep"]);

                        if (row["municipio"] != DBNull.Value)
                            a.Municipio = Convert.ToString(row["municipio"]);

                        if (row["ddd_telefone"] != DBNull.Value)
                            a.DDDTelefone = Convert.ToString(row["ddd_telefone"]);

                        if (row["telefone"] != DBNull.Value)
                            a.telefone = Convert.ToString(row["telefone"]);

                        if (row["ddd_celular"] != DBNull.Value)
                            a.DDDCelular = Convert.ToString(row["ddd_celular"]);

                        if (row["celular"] != DBNull.Value)
                            a.telefone2 = Convert.ToString(row["celular"]);

                        if (row["email"] != DBNull.Value)
                            a.email = Convert.ToString(row["email"]);

                        if (row["rg"] != DBNull.Value)
                            a.registro = Convert.ToString(row["rg"]);

                        if (row["expedido"] != DBNull.Value)
                            a.registro_emissor = Convert.ToString(row["expedido"]);

                        if (row["expedido_uf"] != DBNull.Value)
                            a.registro_uf = Convert.ToString(row["expedido_uf"]);

                        if (row["nascimento"] != DBNull.Value)
                            a.nascimento = Convert.ToDateTime(row["nascimento"]);

                        if (row["filiacao_pai"] != DBNull.Value)
                            a.nome_pai = Convert.ToString(row["filiacao_pai"]);

                        if (row["filiacao_mae"] != DBNull.Value)
                            a.nome_mae = Convert.ToString(row["filiacao_mae"]);

                        if (row["complemento"] != DBNull.Value)
                            a.complemento = Convert.ToString(row["complemento"]);

                        return a;
                    }
                    else {
                        ObCadastro a = new ObCadastro();
                        a.cpf = "";
                        return a;
                    }
                    
                        
                    
                }
            }

            catch (Exception e)
            {
                log.write("Falha GetCadastroCpf: ", e);
            }
            return null;
        }


public static ObCadastro GetCadastroRenach(string renach, LogFile log)
        {
            try
            {
                using (var gerDB = new FactoryDAO())
                {
                    StringBuilder sql = new StringBuilder();
                    sql.AppendLine("SELECT * FROM cadastro_tbl ");
                    sql.AppendLine("WHERE renach='" + renach + "' limit 1 ");

                    DataTable dt = gerDB.executeQuery(sql.ToString(), log);

                    if (dt.Rows.Count > 0)
                    {
                        DataRow row = dt.Rows[0];

                        ObCadastro a = new ObCadastro();
                        if (row["id_cadastro"] != DBNull.Value)
                            a.id_candidatos = Convert.ToInt32(row["id_cadastro"]);

                        if (row["cpf"] != DBNull.Value)
                            a.cpf = Convert.ToString(row["cpf"]);

                        if (row["renach"] != DBNull.Value)
                            a.renach = Convert.ToString(row["renach"]);

                        if (row["nome"] != DBNull.Value)
                            a.nome = Convert.ToString(row["nome"]);

                        if (row["sexo"] != DBNull.Value)
                            a.sexo = Convert.ToString(row["sexo"]);

                        if (row["endereco"] != DBNull.Value)
                            a.endereco = Convert.ToString(row["endereco"]);

                        if (row["numero"] != DBNull.Value)
                            a.numero = Convert.ToString(row["numero"]);

                        if (row["bairro"] != DBNull.Value)
                            a.bairro = Convert.ToString(row["bairro"]);

                        if (row["cep"] != DBNull.Value)
                            a.cep = Convert.ToString(row["cep"]);

                        if (row["municipio"] != DBNull.Value)
                            a.Municipio = Convert.ToString(row["municipio"]);

                        if (row["ddd_telefone"] != DBNull.Value)
                            a.DDDTelefone = Convert.ToString(row["ddd_telefone"]);

                        if (row["telefone"] != DBNull.Value)
                            a.telefone = Convert.ToString(row["telefone"]);

                        if (row["ddd_celular"] != DBNull.Value)
                            a.DDDCelular = Convert.ToString(row["ddd_celular"]);

                        if (row["celular"] != DBNull.Value)
                            a.telefone2 = Convert.ToString(row["celular"]);

                        if (row["email"] != DBNull.Value)
                            a.email = Convert.ToString(row["email"]);

                        if (row["rg"] != DBNull.Value)
                            a.registro = Convert.ToString(row["rg"]);

                        if (row["expedido"] != DBNull.Value)
                            a.registro_emissor = Convert.ToString(row["expedido"]);

                        if (row["expedido_uf"] != DBNull.Value)
                            a.registro_uf = Convert.ToString(row["expedido_uf"]);

                        if (row["nascimento"] != DBNull.Value)
                            a.nascimento = Convert.ToDateTime(row["nascimento"]);

                        if (row["filiacao_pai"] != DBNull.Value)
                            a.nome_pai = Convert.ToString(row["filiacao_pai"]);

                        if (row["filiacao_mae"] != DBNull.Value)
                            a.nome_mae = Convert.ToString(row["filiacao_mae"]);

                        if (row["complemento"] != DBNull.Value)
                            a.complemento = Convert.ToString(row["complemento"]);

                        return a;
                    }
                    else
                    {
                        ObCadastro a = new ObCadastro();
                        a.renach = "";
                        return a;
                    }

                    }
            }
            catch (Exception e)
            {
                log.write("Falha GetCadastroRenach: ", e);
            }
            return null;
        }

        public static ObCadastro ConsultaCPF(string cpf, LogFile log) {

            StringBuilder sql = new StringBuilder();

            sql.AppendLine("SELECT * FROM cadastro_tbl ");
            sql.AppendLine("WHERE cpf='" + cpf + "' limit 1 ");
            try
            {

                using (var gerDB = new FactoryDAO())
                {

                    DataTable dt = gerDB.executeQuery(sql.ToString(), log);
                    if (dt.Rows.Count > 0)
                    {
                        DataRow row = dt.Rows[0];
                        ObCadastro a = new ObCadastro();
                        if (row["cpf"] != DBNull.Value )
                        {
                            a.cpf = Convert.ToString(row["cpf"] );
                            
                        }
                       
                    }
                }

            }
            catch (Exception e)
            {
                log.write("falha consultando renach", e);
            }
            return null;
        }
        public static ObCadastro ConsultaRENACH(string renach, LogFile log)
        {

            StringBuilder sql = new StringBuilder();

            sql.AppendLine("SELECT * FROM cadastro_tbl ");
            sql.AppendLine("WHERE renach='" + renach + "' limit 1 ");
            try
            {

                using (var gerDB = new FactoryDAO())
                {

                    DataTable dt = gerDB.executeQuery(sql.ToString(), log);
                    if (dt.Rows.Count > 0)
                    {
                        DataRow row = dt.Rows[0];
                        ObCadastro a = new ObCadastro();
                        if (row["renach"] != DBNull.Value)
                        {
                            a.renach = Convert.ToString(row["renach"]);
                           
                        }
                    }
                    
                }
            }
            catch (Exception e)
            {
                log.write("falha consultando renach", e);

            }
            return null;
        }




private static readonly IDictionary<Type, ICollection<PropertyInfo>> _Properties =
       new Dictionary<Type, ICollection<PropertyInfo>>();
        public static IEnumerable<T> DataTableToList<T>(this DataTable table) where T : class, new()
        {
            try
            {
                var objType = typeof(T);
                ICollection<PropertyInfo> properties;

                lock (_Properties)
                {
                    if (!_Properties.TryGetValue(objType, out properties))
                    {
                        properties = objType.GetProperties().Where(property => property.CanWrite).ToList();
                        _Properties.Add(objType, properties);
                    }
                }

                var list = new List<T>(table.Rows.Count);

                foreach (var row in table.AsEnumerable())
                {
                    var obj = new T();

                    foreach (var prop in properties)
                    {
                        try
                        {
                            var propType = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;
                            var safeValue = row[prop.Name] == null ? null : Convert.ChangeType(row[prop.Name], propType);

                            prop.SetValue(obj, safeValue, null);
                        }
                        catch
                        {
                            // ignored
                        }
                    }

                    list.Add(obj);
                }

                return list;
            }
            catch
            {
                return Enumerable.Empty<T>();
            }
        }
        public static string FormataDataMysql(DateTime dt)
        {
            return dt.Year.ToString() + "-" + dt.Month.ToString() + "-" + dt.Day.ToString() + " " + dt.Hour + ":" + dt.Minute + ":" + dt.Second;
        }

        public static string FormataDecimal(decimal dt)
        {
            var a = Convert.ToString(dt);

            if (a.Contains(","))
                return a.Replace(".", "").Replace(",", ".");
            else
                return a;
        }

        public static string FormataDecimal(string a)
        {
            if (a.Contains(","))
                return a.Replace(".", "").Replace(",", ".").Replace("\r\n", "");
            else
                return a;
        }

        
    }
}
