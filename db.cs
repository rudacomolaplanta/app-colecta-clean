using desafiocoaniquem.Models;
using System.Data;
using System.Collections;
using MySqlConnector;

namespace desafiocoaniquem
{
    public class db
    {
        public static int insertTransaction(string monto, string canal, string referencia, string token, string RequestDATA, string e)
        {
            //MySQL
            int ret = 0;
            MySqlConnection databaseConnection = new MySqlConnection(Configuration.getConnectionString());
            MySqlCommand cmd = new MySqlCommand("INSERT INTO transactions (RequestTimestamp,RequestAmount,RequestChannel,RequestPaymentReference,RequestToken,RequestDATA, Email) VALUES(CURRENT_TIMESTAMP(3),@RequestAmount,@RequestChannel,@RequestPaymentReference,@RequestToken,@RequestDATA, @Email)", databaseConnection);
            cmd.Parameters.AddWithValue("@RequestAmount", monto);
            cmd.Parameters.AddWithValue("@RequestChannel", canal);
            cmd.Parameters.AddWithValue("@RequestPaymentReference", referencia);
            cmd.Parameters.AddWithValue("@RequestToken", token);
            cmd.Parameters.AddWithValue("@RequestDATA", RequestDATA);
            cmd.Parameters.AddWithValue("@Email", e);
            cmd.CommandTimeout = 60;
            databaseConnection.Open();
            cmd.ExecuteNonQuery();
            ret = (int)cmd.LastInsertedId;
            databaseConnection.Close();
            return ret;
        }

        public static void updateTransactionToken(string id, string token)
        {
            //MySQL
            MySqlConnection databaseConnection = new MySqlConnection(Configuration.getConnectionString());
            MySqlCommand cmd = new MySqlCommand("UPDATE transactions SET RequestToken = @RequestToken WHERE IdTransaction = @IdTransaction", databaseConnection);
            cmd.Parameters.AddWithValue("@RequestToken", token);
            cmd.Parameters.AddWithValue("@IdTransaction", id);
            cmd.CommandTimeout = 60;
            databaseConnection.Open();
            cmd.ExecuteNonQuery();
            databaseConnection.Close();
        }

        public static void updateTransactionRequestDATA(int id, string requestDATA)
        {
            //MySQL
            MySqlConnection databaseConnection = new MySqlConnection(Configuration.getConnectionString());
            MySqlCommand cmd = new MySqlCommand("UPDATE transactions SET RequestDATA = @RequestDATA WHERE IdTransaction = @IdTransaction", databaseConnection);
            cmd.Parameters.AddWithValue("@RequestDATA", requestDATA);
            cmd.Parameters.AddWithValue("@IdTransaction", id);
            cmd.CommandTimeout = 60;
            databaseConnection.Open();
            cmd.ExecuteNonQuery();
            databaseConnection.Close();
        }

        public static Transaction getRequestTransaction(string id)
        {
            DataSet ds = new DataSet();
            //SqlConnection connection = new SqlConnection(Configuration.getConnectionString());
            MySqlConnection databaseConnection = new MySqlConnection(Configuration.getConnectionString());
            Transaction transaction = null;
            using (MySqlCommand cmd = new MySqlCommand("SELECT * FROM transactions WHERE IdTransaction = @IdTransaction", databaseConnection))
            {
                cmd.Parameters.AddWithValue("@IdTransaction", id);
                databaseConnection.Open();
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                da.Fill(ds);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    transaction = new Transaction();
                    transaction.Email = ds.Tables[0].Rows[0]["Email"] != DBNull.Value ? Convert.ToString(ds.Tables[0].Rows[0]["Email"]) : "";
                    transaction.IdTransaction = Convert.ToInt32(ds.Tables[0].Rows[0]["IdTransaction"]);
                    transaction.RequestTimestamp = Convert.ToDateTime(ds.Tables[0].Rows[0]["RequestTimestamp"]);
                    transaction.RequestAmount = Convert.ToInt32(ds.Tables[0].Rows[0]["RequestAmount"]);
                    transaction.RequestChannel = Convert.ToString(ds.Tables[0].Rows[0]["RequestChannel"]);
                    transaction.RequestPaymentReference = Convert.ToString(ds.Tables[0].Rows[0]["RequestPaymentReference"]);
                    transaction.RequestToken = Convert.ToString(ds.Tables[0].Rows[0]["RequestToken"]);
                    transaction.RequestDATA = ds.Tables[0].Rows[0]["RequestDATA"] != DBNull.Value ? Convert.ToString(ds.Tables[0].Rows[0]["RequestDATA"]) : "";
                }
                databaseConnection.Close();
            }
            return transaction;
        }

        public static Transaction getTransactionByToken(string t)
        {
            DataSet ds = new DataSet();
            //SqlConnection connection = new SqlConnection(Configuration.getConnectionString());
            MySqlConnection databaseConnection = new MySqlConnection(Configuration.getConnectionString());
            Transaction transaction = null;
            using (MySqlCommand cmd = new MySqlCommand("SELECT * FROM transactions WHERE RequestToken = @Token", databaseConnection))
            {
                cmd.Parameters.AddWithValue("@Token", t);
                databaseConnection.Open();
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                da.Fill(ds);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    transaction = new Transaction();
                    //transaction.Email = ds.Tables[0].Rows[0]["Email"] != DBNull.Value ? Convert.ToString(ds.Tables[0].Rows[0]["Email"]) : "";
                    //transaction.IdTransaction = Convert.ToInt32(ds.Tables[0].Rows[0]["IdTransaction"]);
                    transaction.RequestTimestamp = Convert.ToDateTime(ds.Tables[0].Rows[0]["RequestTimestamp"]);
                    transaction.RequestAmount = Convert.ToInt32(ds.Tables[0].Rows[0]["RequestAmount"]);
                    //transaction.RequestChannel = Convert.ToString(ds.Tables[0].Rows[0]["RequestChannel"]);
                    //transaction.RequestPaymentReference = Convert.ToString(ds.Tables[0].Rows[0]["RequestPaymentReference"]);
                    transaction.RequestToken = Convert.ToString(ds.Tables[0].Rows[0]["RequestToken"]);
                    if (ds.Tables[0].Rows[0]["ResponseResponseCode"] != DBNull.Value)
                        transaction.ResponseResponseCode = Convert.ToString(ds.Tables[0].Rows[0]["ResponseResponseCode"]);
                    //transaction.RequestDATA = ds.Tables[0].Rows[0]["RequestDATA"] != DBNull.Value ? Convert.ToString(ds.Tables[0].Rows[0]["RequestDATA"]) : "";
                }
                databaseConnection.Close();
            }
            return transaction;
        }


        public static void updateTransactionResponse(string id, string responseResponseCode, string responseAuthorizationCode, string responseDATA)
        {
            //MySQL
            MySqlConnection databaseConnection = new MySqlConnection(Configuration.getConnectionString());
            MySqlCommand cmd = new MySqlCommand("UPDATE transactions SET ResponseTimestamp = CURRENT_TIMESTAMP(3), ResponseAuthorizationCode = @ResponseAuthorizationCode, ResponseResponseCode = @ResponseResponseCode, ResponseDATA = @ResponseDATA WHERE IdTransaction = @IdTransaction", databaseConnection);
            cmd.Parameters.AddWithValue("@ResponseResponseCode", responseResponseCode);
            cmd.Parameters.AddWithValue("@ResponseAuthorizationCode", responseAuthorizationCode);
            cmd.Parameters.AddWithValue("@ResponseDATA", responseDATA);
            cmd.Parameters.AddWithValue("@IdTransaction", id);
            cmd.CommandTimeout = 60;
            databaseConnection.Open();
            cmd.ExecuteNonQuery();
            databaseConnection.Close();

        }

        public static List<ConsultaAlcancia> getTotalAlcancia(string id, DateTime fechaInicioColecta)
        {
            DataSet ds = new DataSet();
            //SqlConnection connection = new SqlConnection(Configuration.getConnectionString());
            MySqlConnection databaseConnection = new MySqlConnection(Configuration.getConnectionString());
            ConsultaAlcancia consulta = null;
            List<ConsultaAlcancia> lista = new List<ConsultaAlcancia>();
            using (MySqlCommand cmd = new MySqlCommand("select sum(RequestAmount) as total from transactions t where t.RequestPaymentReference = @ref and ResponseResponseCode = '0' and RequestTimestamp >= @fechainiciocolecta", databaseConnection))
            {
                cmd.Parameters.AddWithValue("@ref", id);
                cmd.Parameters.AddWithValue("@fechainiciocolecta", fechaInicioColecta);
                databaseConnection.Open();
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                da.Fill(ds);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    consulta = new ConsultaAlcancia();
                    consulta.Total = ds.Tables[0].Rows[0]["total"] != DBNull.Value ? Convert.ToInt32(ds.Tables[0].Rows[0]["total"]) : 0;
                    lista.Add(consulta);
                }
                databaseConnection.Close();
            }
            return lista;
        }

        public static int getTotalAmount(DateTime fchaInicioColecta)
        {
            int ret = 0;
            DataSet ds = new DataSet();
            MySqlConnection databaseConnection = new MySqlConnection(Configuration.getConnectionString());
            using (MySqlCommand cmd = new MySqlCommand("select sum(RequestAmount) + (select Valor from properties where Id='monto_manual') as total from transactions t where ResponseResponseCode = '0' and RequestTimestamp >= @fechainiciocolecta", databaseConnection))
            {
                cmd.Parameters.AddWithValue("@fechainiciocolecta", fchaInicioColecta);
                databaseConnection.Open();
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                da.Fill(ds);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ret = ds.Tables[0].Rows[0]["total"] != DBNull.Value ? Convert.ToInt32(ds.Tables[0].Rows[0]["total"]) : 0;
                }
                databaseConnection.Close();
            }
            return ret;
        }

        public static ArrayList getProperties()
        {
            DataSet ds = new DataSet();
            //SqlConnection connection = new SqlConnection(Configuration.getConnectionString());
            MySqlConnection databaseConnection = new MySqlConnection(Configuration.getConnectionString());
            ArrayList list = new ArrayList();
            using (MySqlCommand cmd = new MySqlCommand("select * from properties", databaseConnection))
            {
                databaseConnection.Open();
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                da.Fill(ds);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        string id = ds.Tables[0].Rows[i]["Id"] != DBNull.Value ? Convert.ToString(ds.Tables[0].Rows[i]["Id"]) : "";
                        string value = ds.Tables[0].Rows[i]["Valor"] != DBNull.Value ? Convert.ToString(ds.Tables[0].Rows[i]["Valor"]) : "";
                        list.Add(new string[] { id, value });
                    }
                }
                databaseConnection.Close();
            }
            return list;
        }

        public static void updateMonto(int monto)
        {
            //MySQL
            MySqlConnection databaseConnection = new MySqlConnection(Configuration.getConnectionString());
            MySqlCommand cmd = new MySqlCommand("UPDATE properties SET Valor = @Monto WHERE Id = 'monto_manual'", databaseConnection);
            cmd.Parameters.AddWithValue("@Monto", monto);
            cmd.CommandTimeout = 60;
            databaseConnection.Open();
            cmd.ExecuteNonQuery();
            databaseConnection.Close();
        }

        public static DataTable getAllColectaTrxs()
        {
            DataTable dt = null;
            DataSet ds = new DataSet();
            MySqlConnection databaseConnection = new MySqlConnection(Configuration.getConnectionString());
            Transaction transaction = null;
            using (MySqlCommand cmd = new MySqlCommand("SELECT * FROM transactions where RequestTimestamp >= @FechaInicio", databaseConnection))
            {
                cmd.Parameters.AddWithValue("@FechaInicio", GlobalConfiguration.colectaConfig.Fecha);
                databaseConnection.Open();
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                da.Fill(ds);
                dt = ds.Tables[0];
            }
            return dt;
        }
    }
}