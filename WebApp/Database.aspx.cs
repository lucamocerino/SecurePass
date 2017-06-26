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
    public partial class Database : System.Web.UI.Page
    {
        public string constr = "server=localhost;user id=root;password=lucamocerino;database=admindb;persistsecurityinfo=True";
        
            protected void Page_Load(object sender, EventArgs e)
            {
                if (!this.IsPostBack)
                {
                    this.BindGrid();
                    this.BindGrid2();
                }
            }

            private void BindGrid()
            {
                //string constr = ConfigurationManager.ConnectionStrings["admindbConnectionString"].ConnectionString;
                
                using (MySqlConnection con = new MySqlConnection(constr))
                {
                    using (MySqlCommand cmd = new MySqlCommand("SELECT * FROM user"))
                    {
                        using (MySqlDataAdapter sda = new MySqlDataAdapter())
                        {
                            cmd.Connection = con;
                            sda.SelectCommand = cmd;
                            using (DataTable dt = new DataTable())
                            {
                                sda.Fill(dt);
                                GridView1.DataSource = dt;
                                GridView1.DataBind();
                            }
                        }
                    }
                }
            }

            private void BindGrid2()
            {
                //string constr = ConfigurationManager.ConnectionStrings["admindbConnectionString"].ConnectionString;
                using (MySqlConnection con = new MySqlConnection(constr))
                {
                    using (MySqlCommand cmd = new MySqlCommand("SELECT * FROM timetable"))
                    {
                        using (MySqlDataAdapter sda = new MySqlDataAdapter())
                        {
                            cmd.Connection = con;
                            sda.SelectCommand = cmd;
                            using (DataTable dt = new DataTable())
                            {
                                sda.Fill(dt);
                                GridView2.DataSource = dt;
                                GridView2.DataBind();
                            }
                        }
                    }
                }
            }

            protected void Insert(object sender, EventArgs e)
            {

                string iduser = txtUserId.Text;
                string name = txtname.Text;
                string surname = txtSurname.Text;
                string role= txtRole.Text;
                string path = txtPath.Text;

               // string constr = ConfigurationManager.ConnectionStrings["admindbConnectionString"].ConnectionString;
                using (MySqlConnection con = new MySqlConnection(constr))
                {
                    using (MySqlCommand cmd = new MySqlCommand("INSERT INTO user (iduser, name, surname, role, path) VALUES (@iduser, @name, @surname, @role, @path)"))
                    {
                        using (MySqlDataAdapter sda = new MySqlDataAdapter())
                        {
                            cmd.Parameters.AddWithValue("@iduser", iduser);
                            cmd.Parameters.AddWithValue("@name", name);
                            cmd.Parameters.AddWithValue("@surname", surname);
                            cmd.Parameters.AddWithValue("@role", role);
                            cmd.Parameters.AddWithValue("@path", path);
                            cmd.Connection = con;
                            
                                con.Open();
                                cmd.ExecuteNonQuery();
                                con.Close();

                        }
                    }
                }
                this.BindGrid();
            }

            protected void OnRowEditing(object sender, GridViewEditEventArgs e)
            {
                GridView1.EditIndex = e.NewEditIndex;
                this.BindGrid();
            }

            protected void OnRowCancelingEdit(object sender, EventArgs e)
            {
                GridView1.EditIndex = -1;
                this.BindGrid();
            }

            protected void OnRowUpdating(object sender, GridViewUpdateEventArgs e)
            {

                GridViewRow row = GridView1.Rows[e.RowIndex];
                string iduser = (row.FindControl("txtUserId") as TextBox).Text;
                string name = (row.FindControl("txtname") as TextBox).Text;
                string surname = (row.FindControl("txtSurname") as TextBox).Text;
                string role = (row.FindControl("txtRole") as TextBox).Text;
                string path = (row.FindControl("txtPath") as TextBox).Text;

               // string constr = ConfigurationManager.ConnectionStrings["admindbConnectionString"].ConnectionString;
                using (MySqlConnection con = new MySqlConnection(constr))
                {
                    using (MySqlCommand cmd = new MySqlCommand("UPDATE admindb.user SET name= @name, surname= @surname, role= @role, path= @path WHERE iduser=@iduser"))
                    {
                        using (MySqlDataAdapter sda = new MySqlDataAdapter())
                        {
                            cmd.Parameters.AddWithValue("@iduser", iduser);
                            cmd.Parameters.AddWithValue("@name", name);
                            cmd.Parameters.AddWithValue("@surname", surname);
                            cmd.Parameters.AddWithValue("@role", role);
                            cmd.Parameters.AddWithValue("@path", path);

                            cmd.Connection = con;
                            con.Open();
                            cmd.ExecuteNonQuery();

                            
                            
                    
                
                
                
                            con.Close();
                        }
                    }
                }
                GridView1.EditIndex = -1;
                this.BindGrid();
            }

        
            
        
            protected void OnRowDeleting(object sender, GridViewDeleteEventArgs e)
            {
                


                string iduser = GridView1.DataKeys[e.RowIndex].Value.ToString();
               // string constr = ConfigurationManager.ConnectionStrings["admindbConnectionString"].ConnectionString;
                using (MySqlConnection con = new MySqlConnection(constr))
                {
                    using (MySqlCommand cmd = new MySqlCommand("DELETE FROM user WHERE iduser = @iduser"))
                    {
                        using (MySqlDataAdapter sda = new MySqlDataAdapter())
                        {
                            cmd.Parameters.AddWithValue("@iduser", iduser);
                            cmd.Connection = con;
                            con.Open();
                            cmd.ExecuteNonQuery();

                           

                            con.Close();
                        }
                    }   
                }
                
                this.BindGrid();
            }

            protected void ButtonSearch_Click(object sender, EventArgs e)
            {
               string iduser = TextBox1.Text;
            
               //string constr = ConfigurationManager.ConnectionStrings["admindbConnectionString"].ConnectionString;
                using (MySqlConnection con = new MySqlConnection(constr))
                {
                    using (MySqlCommand cmd = new MySqlCommand("SELECT iduser,entrytime, exitime, currDate FROM admindb.timetable WHERE iduser=@iduser"))
                    {
                        using (MySqlDataAdapter sda = new MySqlDataAdapter())
                        {
                            cmd.Parameters.AddWithValue("@iduser", iduser);
                            cmd.Connection = con;
                            sda.SelectCommand = cmd;

                            using (DataTable dt = new DataTable())
                            {
                                sda.Fill(dt);
                                GridView3.DataSource = dt;
                                GridView3.DataBind();
                            }
                        }
                    }
                }
          

            }
    

          
        
        }
    }
