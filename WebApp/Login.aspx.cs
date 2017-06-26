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
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }



        protected void Login_Click(object sender, EventArgs e)
        {
            string username = textUsername.Text;
            string password = textPassword.Text;
            string error = "";
            string output = "";

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

                        if (cmd.ExecuteScalar() == null){
                            output = null;
                            //Response.Write("Error");
                            error = "Error: invalid username or password";
                            Label1.Text = error;
                            
                         }
                        else
                        {
                            output = cmd.ExecuteScalar().ToString();
                        }
                   

                        if (output !=null)
                        {
                            Session["admin"] = textUsername.Text;
                            Response.Redirect("Database.aspx");
                        }
                        else
                        
                            
                        
                        con.Close();

                    }
                }
            }
        }

    }
}