using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using Pervasive.Data.SqlClient;

namespace RocedesAPI.Conex_Pervasive
{
    public class Cls_ConexionPervasive
    {


        private PsqlConnection _Cnx = null;

        public Cls_ConexionPervasive()
        {
            _Cnx = new PsqlConnection("Server DSN=INTELSERVER;Host=192.168.14.50");
        }


        public DataTable GetDatos(string Query, out string Mensaje )
        {
            Mensaje = string.Empty;

            try
            {
                DataTable dt = new DataTable();

                PsqlDataAdapter _Adp = new PsqlDataAdapter();
                _Adp.SelectCommand = new PsqlCommand(Query, _Cnx);
                _Cnx.Open();
                _Adp.Fill(dt);
                _Cnx.Close();

                return dt;
            }
            catch(Exception ex)
            {
                _Cnx.Close();
                Mensaje = Cls.Cls_Mensaje.Tojson(null, 0, "1", ex.Message, 1);
                return null;
            }

        }
        /*var cnn = ConfigurationManager.AppSettings["PervasiveSQLClient"];
        PsqlConnection DBConn = new PsqlConnection(cnn.ToString());
            try
            {
                DataTable dt = new DataTable();
        // Open the connection
        DBConn.Open();
                PsqlDataAdapter da = new PsqlDataAdapter();
        da.SelectCommand = new PsqlCommand(query, DBConn);
        da.Fill(dt);
                //Console.WriteLine("Connection Successful!");

                return dt;

            }
            catch (PsqlException ex)
            {
                // Connection failed
                Console.WriteLine(ex.Message);
                return null;
            }
            finally
{
    DBConn.Close();*/
 
    }
}