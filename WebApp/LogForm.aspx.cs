using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using MySql.Data.MySqlClient;

namespace WebApplication3
{
    public partial class LogForm : System.Web.UI.Page
    {
        private MySqlConnection con = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Admin"] != null)
            {
                Response.Redirect("Database.aspx");
            }
        }
        protected void Button_Click(object sender, EventArgs e)
        {
            //string username = UserName.Text;
            //string password = Password.Text;
           // string username = ((TextBox)Login1.FindControl("UserName")).Text;
           // string password = ((TextBox)Login1.FindControl("Password")).Text;


            string username = ((TextBox)Login1.FindControl("UserName")).Text;
            string password = ((TextBox)Login1.FindControl("Password")).Text;
            con = new MySqlConnection(ConfigurationManager.ConnectionStrings["admindbConnectionString"].ConnectionString);

            if (!username.Equals("Admin"))
            {
                ScriptManager.RegisterStartupScript(this, typeof(Page), "MsgUpdate",
                                                               "alert('Wrong administrator username');", true);
                return;
            }

            try
            {
                // Open the connection
                con.Open();
            }
            catch (Exception er)
            {
                ScriptManager.RegisterStartupScript(this, typeof(Page), "MsgUpdate",
                                                               "alert('Connection NOT valid');", true);
                return;
            }
            try
            {
                string passwordDb = null;

                // Sql query that retrieves the password associated to a well defined username
                string query = @"SELECT * FROM admintab WHERE username = ?User";
                MySqlCommand comm = con.CreateCommand();
                comm.CommandText = query;
                comm.Parameters.Add("?User", MySqlDbType.VarChar).Value = "Admin";
                // Use a prepared statement
                comm.Prepare();
                comm.ExecuteNonQuery();
                MySqlDataReader reader = comm.ExecuteReader();
                while (reader.Read())
                {
                    passwordDb = reader.GetString(1);
                }
                reader.Close();
                // If the retrieved password is equal to the input password, OK
                if (passwordDb.Equals(password))
                {
                    con.Close();
                    Session["Admin"] = username;
                    Response.Redirect("~/Database.aspx");
                }
                // Otherwise, there is an error
                else
                {
                    con.Close();
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "MsgUpdate",
                                                                   "alert('Incorrect password');", true);
                    return;
                }
            }
            catch (Exception er)
            {
                con.Close();
                ScriptManager.RegisterStartupScript(this, typeof(Page), "MsgUpdate",
                                                               "alert('" + er.Message + "');", true);
                return;
            }
        }
            /*
            string constr = ConfigurationManager.ConnectionStrings["admindbConnectionString"].ConnectionString;
            using (MySqlConnection con = new MySqlConnection(constr))
            {
                using (MySqlCommand cmd = new MySqlCommand("SELECT * FROM admintab WHERE username=@username AND password=@password"))
                {
                    using (MySqlDataAdapter sda = new MySqlDataAdapter())
                    {
                        cmd.Parameters.AddWithValue("@username", username);
                        cmd.Parameters.AddWithValue("@password", password);

                        cmd.Connection = con;

                        con.Open();
                        string output = cmd.ExecuteScalar().ToString();

                        if (output != null)
                        {
                            Session["admin"] = username;
                            Response.Redirect("Database.aspx");
                        }
                        else
                        {
                            Response.Write("Error");
                        }
                        con.Close();

                    }
                }
            }
             */
        
    }
}
