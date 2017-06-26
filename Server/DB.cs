using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Configuration;

namespace ServerSide
{
    class DB
    {
        private MySqlConnection con = null;
       

        // Constructor
        public DB(string connectionString)
        {

            con = new MySqlConnection(connectionString);
        }


        // Check if iduser is valid
        public bool CheckID(string IdUser)
        {
            try
            {
                // Open the connection
                con.Open();
                Console.WriteLine("You are connected to the db!");
            }
            catch (Exception er)
            {
                Console.WriteLine("Connection error: " + er.Message);
                return false;
            }

                string ID = null;
                MySqlCommand comm = con.CreateCommand();
                MySqlDataReader reader = null;
                // Sql query that retrieves the iduser
                string query = @"SELECT iduser FROM user WHERE iduser = ?User";
              
                comm.CommandText = query;
                comm.Parameters.Add("?User", MySqlDbType.VarChar).Value = IdUser;
                comm.Prepare();
                comm.ExecuteNonQuery();
                reader = comm.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ID = reader.GetString(0);

                        if (ID == null)
                        {
                            Console.WriteLine("No user found");
                            con.Close();
                            reader.Close();
                            return false;
                        }
                        else
                        {
                            Console.WriteLine("User: "+ID+" found");
                            con.Close();
                            reader.Close();
                            return true;
                        }
                    }
                }
                else
                {
                    Console.WriteLine("No user found.");
                    con.Close();
                    reader.Close();
                    return false;
                }
                return false;
            
        }

        // Return the path of the image
        public string FindPath(string IdUser)
        {
            try
            {
                // Open the connection
                con.Open();
                Console.WriteLine("You are connected to the db!");
            }
            catch (Exception er)
            {
                Console.WriteLine("Connection error: " + er.Message);
                return null;
            }

            string path = null;
            MySqlCommand comm = con.CreateCommand();
            MySqlDataReader reader = null;
            // Sql query that retrieves the iduser
            string query = @"SELECT path FROM user WHERE iduser = ?User";

            comm.CommandText = query;
            comm.Parameters.Add("?User", MySqlDbType.VarChar).Value = IdUser;
            comm.Prepare();
            comm.ExecuteNonQuery();
            reader = comm.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    path = reader.GetString(0);

                    if (path == null)
                    {
                        Console.WriteLine("No path found");
                        con.Close();
                        reader.Close();
                        return null;
                    }
                    else
                    {
                        Console.WriteLine("Path: " + path + " found");
                        con.Close();
                        reader.Close();
                        return path;
                    }
                }
            }
            else
            {
                Console.WriteLine("No path found.");
                con.Close();
                reader.Close();
                return null;
            }
            return path;
        }


        // Returns the credentials (name@surname@role)
        public string FindCredetials(string IdUser)
        {
            try
            {
                // Open the connection
                con.Open();
                Console.WriteLine("You are connected to the db!");
            }
            catch (Exception er)
            {
                Console.WriteLine("Connection error: " + er.Message);
                return null;
            }

            string name = null;
            string surname = null;
            string role = null;
            string credential= null;

            MySqlCommand comm = con.CreateCommand();
            MySqlDataReader reader = null;
            string query = @"SELECT  name,surname,role FROM user WHERE iduser = ?User";

            comm.CommandText = query;
            comm.Parameters.Add("?User", MySqlDbType.VarChar).Value = IdUser;
            comm.Prepare();
            comm.ExecuteNonQuery();
            reader = comm.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    name = reader.GetString(0);
                    surname = reader.GetString(1);
                    role = reader.GetString(2);

                    if (name == null || surname==null || role==null)
                    {
                        Console.WriteLine("No credetial found");
                        con.Close();
                        reader.Close();
                        return null;
                    }
                    else
                    {
                        Console.WriteLine("Name: " + name + ","+surname+","+role+" found");

                        con.Close();
                        reader.Close();
                        credential=name+"@"+surname+"@"+role;
                        Console.WriteLine(credential);

                        return credential;
                    }
                }
            }
            else
            {
                Console.WriteLine("No credential found.");
                con.Close();
                reader.Close();
                return null;
            }
            return credential;
        }

        // Chech there's an entry 
        public bool CheckIdFirst(string IdUser)
        {
            try
            {
                // Open the connection
                con.Open();
                Console.WriteLine("You are connected to the db!");
            }
            catch (Exception er)
            {
                Console.WriteLine("Connection error: " + er.Message);
                return false;
            }

            //string entryTime = null;
            string currDate = DateTime.Now.ToString("MM/dd/yyyy");
            MySqlCommand comm = con.CreateCommand();
            MySqlDataReader reader = null;
            // Sql query that retrieves the iduser
            string query = @"SELECT iduser FROM timetable WHERE iduser = ?User and currDate=?currDate";

            comm.CommandText = query;
            comm.Parameters.Add("?User", MySqlDbType.VarChar).Value = IdUser;
            comm.Parameters.Add("?currDate", MySqlDbType.VarChar).Value = currDate;



            comm.Prepare();
            comm.ExecuteNonQuery();
            reader = comm.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {


                    if (reader.IsDBNull(0))
                    {
                        Console.WriteLine("No iduser found, first access");
                        con.Close();
                        reader.Close();
                        return false;
                    }
                    else
                    {
                        //entryTime = reader.GetString(0);
                        Console.WriteLine("Id already exist!!!");
                        con.Close();
                        reader.Close();
                        return true;
                    }
                }
            }
            else
            {
                Console.WriteLine("No id found found");
                reader.Close();
                con.Close();
                return false;
            }


            return false;
        }



        // Return the path of the image
        public int FindLastID(string IdUser)
        {
            try
            {
                // Open the connection
                con.Open();
                Console.WriteLine("You are connected to the db!");
            }
            catch (Exception er)
            {
                Console.WriteLine("Connection error: " + er.Message);
                return -1;
            }

            int max_id = 0;
            string currDate = DateTime.Now.ToString("MM/dd/yyyy");
            MySqlCommand comm = con.CreateCommand();
            MySqlDataReader reader = null;
            int i = 0;
            var list = new List<Int32>();
            // Sql query that retrieves the iduser
            string query = @"SELECT id FROM timetable WHERE iduser = ?User and currDate=?currDate";

            comm.CommandText = query;
            comm.Parameters.Add("?User", MySqlDbType.VarChar).Value = IdUser;
            comm.Parameters.Add("?currDate", MySqlDbType.VarChar).Value = currDate;
            comm.Prepare();
            comm.ExecuteNonQuery();
            reader = comm.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    
                    list.Add(Int32.Parse(reader.GetString(0)));
                    
                    
                }
                var allRecords = list.ToArray();
                max_id=allRecords.Max();
                con.Close();
                reader.Close();
                return max_id;
                   
            }
            else
            {
                Console.WriteLine("No max id found.");
                con.Close();
                reader.Close();
                return -1;
            }

            
        }





        // Check the ID from iduser and current date
        public int FindID(string IdUser)
        {
            try
            {
                // Open the connection
                con.Open();
                Console.WriteLine("You are connected to the db!");
            }
            catch (Exception er)
            {
                Console.WriteLine("Connection error: " + er.Message);
                return -1;
            }

            int ID = -1;
            string currDate = DateTime.Now.ToString("MM/dd/yyyy");
            MySqlCommand comm = con.CreateCommand();
            MySqlDataReader reader = null;
            // Sql query that retrieves the iduser
            string query = @"SELECT id FROM timetable WHERE iduser = ?User and currDate=currDate";

            comm.CommandText = query;
            comm.Parameters.Add("?User", MySqlDbType.VarChar).Value = IdUser;
            comm.Parameters.Add("?currDate", MySqlDbType.VarChar).Value = currDate;

            comm.Prepare();
            comm.ExecuteNonQuery();
            reader = comm.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    ID = Int32.Parse(reader.GetString(0));

                    if (ID == -1)
                    {
                        Console.WriteLine("No id found");
                        con.Close();
                        reader.Close();
                        return -1;
                    }
                    else
                    {
                        Console.WriteLine("ID: " + ID + " found");
                        con.Close();
                        reader.Close();
                        return ID;
                    }
                }
            }
            else
            {
                Console.WriteLine("No ID found.");
                reader.Close();
                con.Close();
                return -1;
            }
            return ID;
        }

        // Check the if is the entry time
        public bool CheckEntryTime(string IdUser,int id)
        {
            try
            {
                // Open the connection
                con.Open();
                Console.WriteLine("You are connected to the db!");
            }
            catch (Exception er)
            {
                Console.WriteLine("Connection error: " + er.Message);
                return false;
            }

            string entryTime = null;
            string currDate = DateTime.Now.ToString("MM/dd/yyyy");
            MySqlCommand comm = con.CreateCommand();
            MySqlDataReader reader = null;
            // Sql query that retrieves the iduser
            string query = @"SELECT entrytime FROM timetable WHERE iduser = ?User and currDate=?currDate and id=?id";

            comm.CommandText = query;
            comm.Parameters.Add("?User", MySqlDbType.VarChar).Value = IdUser;
            comm.Parameters.Add("?currDate", MySqlDbType.VarChar).Value = currDate;
            comm.Parameters.Add("?id", MySqlDbType.Int32).Value = id;
          

                comm.Prepare();
                comm.ExecuteNonQuery();
                reader = comm.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        
                    
                        if (reader.IsDBNull(0))
                        {
                            Console.WriteLine("No entryTime found, first access");
                            con.Close();
                            reader.Close();
                            return false;
                        }
                        else
                        {
                            entryTime = reader.GetString(0);
                            Console.WriteLine("Entry time: " + entryTime + " found");
                            con.Close();
                            reader.Close();
                            return true;
                        }
                    }
                }
                else
                {
                    Console.WriteLine("No Entry time found");
                    reader.Close();
                    con.Close();
                    return false;
                }
            

            return false;
        }


        //Check if exit time exist
        public bool CheckExitTime(string IdUser,int id)
        {
            try
            {
                // Open the connection
                con.Open();
                Console.WriteLine("You are connected to the db!");
            }
            catch (Exception er)
            {
                Console.WriteLine("Connection error: " + er.Message);
                return false;
            }

            string entryTime = null;
            string currDate = DateTime.Now.ToString("MM/dd/yyyy");
            MySqlCommand comm = con.CreateCommand();
            MySqlDataReader reader = null;
            // Sql query that retrieves the iduser
            string query = @"SELECT exitime FROM timetable WHERE iduser = ?User and currDate=?currDate and id=?id";

            comm.CommandText = query;
            comm.Parameters.Add("?User", MySqlDbType.VarChar).Value = IdUser;
            comm.Parameters.Add("?currDate", MySqlDbType.VarChar).Value = currDate;
            comm.Parameters.Add("?id", MySqlDbType.Int32).Value = id;


            comm.Prepare();
            comm.ExecuteNonQuery();
            reader = comm.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {


                    if (reader.IsDBNull(0))
                    {
                        Console.WriteLine("No exit time found, first access");
                        con.Close();
                        reader.Close();
                        return false;
                    }
                    else
                    {
                        entryTime = reader.GetString(0);
                        Console.WriteLine("Exit time: " + entryTime + " found");
                        con.Close();
                        reader.Close();
                        return true;
                    }
                }
            }
            else
            {
                Console.WriteLine("No Exit time found");
                reader.Close();
                con.Close();
                return false;
            }


            return false;
        }



        //Return true if person is found
        public bool CheckPerson(string IdUser)
        {
            try
            {
                // Open the connection
                con.Open();
                Console.WriteLine("You are connected to the db!");
            }
            catch (Exception er)
            {
                Console.WriteLine("Connection error: " + er.Message);
                return false;
            }

            string entryTime = null;
            string currDate = DateTime.Now.ToString("MM/dd/yyyy");
            MySqlCommand comm = con.CreateCommand();
            MySqlDataReader reader = null;
            // Sql query that retrieves the iduser
            string query = @"SELECT iduser,id FROM timetable WHERE iduser = ?User and currDate=?currDate";

            comm.CommandText = query;
            comm.Parameters.Add("?User", MySqlDbType.VarChar).Value = IdUser;
            comm.Parameters.Add("?currDate", MySqlDbType.VarChar).Value = currDate;



            comm.Prepare();
            comm.ExecuteNonQuery();
            reader = comm.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {


                    if (reader.IsDBNull(0))
                    {
                        Console.WriteLine("No person found, first access");
                        con.Close();
                        reader.Close();
                        return false;
                    }
                    else
                    {
                        entryTime = reader.GetString(0);
                        Console.WriteLine("Person: " + IdUser + " found");
                        con.Close();
                        reader.Close();
                        return true;
                    }
                }
            }
            else
            {
                Console.WriteLine("No person time found");
                reader.Close();
                con.Close();
                return false;
            }


            return false;
        }



        // Check the if is the entry time
        public string GetEntryOrExitTime(string IdUser,int en_ex,int id)
        {
            try
            {
                // Open the connection
                con.Open();
                Console.WriteLine("You are connected to the db!");
            }
            catch (Exception er)
            {
                Console.WriteLine("Connection error: " + er.Message);
                return null;
            }

            string entryTime = null;
            string query = null;
            string currDate = DateTime.Now.ToString("MM/dd/yyyy");
            MySqlCommand comm = con.CreateCommand();
            MySqlDataReader reader = null;
            
            if (en_ex==1) //EntryTime
            {
                query = @"SELECT entrytime FROM timetable WHERE iduser = ?User and currDate=?currDate  and id=?id";
            }
            else
            {
                query = @"SELECT exitime FROM timetable WHERE iduser = ?User and currDate=?currDate and id=?id";
            }
            comm.CommandText = query;
            comm.Parameters.Add("?User", MySqlDbType.VarChar).Value = IdUser;
            comm.Parameters.Add("?currDate", MySqlDbType.VarChar).Value = currDate;
            comm.Parameters.Add("?id", MySqlDbType.VarChar).Value = id;


            comm.Prepare();
            comm.ExecuteNonQuery();
            reader = comm.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {


                    if (reader.IsDBNull(0))
                    {
                        Console.WriteLine("No time found");
                        con.Close();
                        reader.Close();
                        return null;
                    }
                    else
                    {
                        entryTime = reader.GetString(0);
                        Console.WriteLine("Time: " + entryTime + " found");
                        con.Close();
                        reader.Close();
                        return entryTime;
                    }
                }
            }
            else
            {
                Console.WriteLine("No  time found");
                reader.Close();
                con.Close();
                return null;
            }


            return entryTime;
        }





        // Insert the entryTime credential
        public bool InserFirstCredential(string IdUser,int id_seq)
        {
            try
            {
                // Open the connection
                con.Open();
                Console.WriteLine("You are connected to the db!");
            }
            catch (Exception er)
            {
                Console.WriteLine("Connection error: " + er.Message);
                con.Close();
                return false;
            }

          
                Random rnd = new Random();
               
               
                string currDate = DateTime.Now.ToString("MM/dd/yyyy");
                MySqlCommand comm = con.CreateCommand();

                // Sql query that retrieves the iduser
                string query = @"INSERT INTO timetable (id, iduser, currDate, entrytime) VALUES (?id, ?iduser,?currDate,now())";

                try
                {
                    comm.CommandText = query;
                    comm.Parameters.Add("?id", MySqlDbType.Int32).Value = id_seq;
                    comm.Parameters.Add("?iduser", MySqlDbType.VarChar).Value = IdUser;
                    comm.Parameters.Add("?currDate", MySqlDbType.VarChar).Value = currDate;
                    comm.Prepare();
                    comm.ExecuteNonQuery();
                  
                }
                catch (Exception er)
                {
                    Console.WriteLine("Insert entrytime error: " + er.Message);
                    con.Close();
                    return false;
                }
               
            con.Close();
            return true;
        }

        //Insert exit time credential
        public bool InserExitCredential(string IdUser, int id_seq)
        {
            try
            {
                // Open the connection
                con.Open();
                Console.WriteLine("You are connected to the db!");
            }
            catch (Exception er)
            {
                Console.WriteLine("Connection error: " + er.Message);
                con.Close();
                return false;
            }


            Random rnd = new Random();


            string currDate = DateTime.Now.ToString("MM/dd/yyyy");
            MySqlCommand comm = con.CreateCommand();

            // Sql query that retrieves the iduser
            string query = @"INSERT INTO timetable (id, iduser, currDate,exitime) VALUES (?id, ?iduser,?currDate,now())";

            try
            {
                comm.CommandText = query;
                comm.Parameters.Add("?id", MySqlDbType.Int32).Value = id_seq;
                comm.Parameters.Add("?iduser", MySqlDbType.VarChar).Value = IdUser;
                comm.Parameters.Add("?currDate", MySqlDbType.VarChar).Value = currDate;
                comm.Prepare();
                comm.ExecuteNonQuery();

            }
            catch (Exception er)
            {
                Console.WriteLine("Insert entrytime error: " + er.Message);
                con.Close();
                return false;
            }

            con.Close();
            return true;
        }

        // Insert the entryTime
        public bool InsertExitTime(string IdUser, int id_seq)
        {
            try
            {
                // Open the connection
                con.Open();
                Console.WriteLine("You are connected to the db!");
            }
            catch (Exception er)
            {
                Console.WriteLine("Connection error: " + er.Message);
                return false;
            }

            MySqlCommand comm = con.CreateCommand();

            // Sql query that retrieves the iduser
            string query = @"UPDATE timetable set exitime=now() where iduser=?iduser and id=?id";

            try
            {
                comm.CommandText = query;
                comm.Parameters.Add("?id", MySqlDbType.Int32).Value = id_seq;
                comm.Parameters.Add("?iduser", MySqlDbType.VarChar).Value = IdUser;
                comm.Prepare();
                comm.ExecuteNonQuery();
                con.Close();
            }
            catch (Exception er)
            {
                Console.WriteLine("Insert exit time error: " + er.Message);
                con.Close();
                return false;
            }


            return true;
        }

        // Insert the entryTime
        public bool InsertEntryTime(string IdUser)
        {
            try
            {
                // Open the connection
                con.Open();
                Console.WriteLine("You are connected to the db!");
            }
            catch (Exception er)
            {
                Console.WriteLine("Connection error: " + er.Message);
                return false;
            }

            MySqlCommand comm = con.CreateCommand();
            string currDate = DateTime.Now.ToString("MM/dd/yyyy");
            // Sql query that retrieves the iduser
            string query = @"UPDATE timetable set entrytime=now() where iduser=?iduser and currDate=?currDate";

            try
            {
                comm.CommandText = query;
                comm.Parameters.Add("?iduser", MySqlDbType.VarChar).Value = IdUser;
                comm.Parameters.Add("?currDate", MySqlDbType.VarChar).Value = currDate;
                comm.Prepare();
                comm.ExecuteNonQuery();
                con.Close();
                return true;
            }
            catch (Exception er)
            {
                Console.WriteLine("Insert entry time error: " + er.Message);
                con.Close();
                return false;
            }

        }

        
        /*
        public static int Main(String[] args)
        {
            string constr = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
            DB datab = new DB(constr);
           // bool res = datab.CheckID("A1");
            // string path = datab.FindPath("luca");
          //  bool ins = datab.InsertEntryTime("A1");
           //bool ins = datab.InserExitTime("A1", 2);
            //int id = datab.FindID("A1");
            //bool e = datab.CheckEntryTime("A1");
            //string s = datab.FindCredetials("A2");
            //string s = datab.GetEntryOrExitTime("A1",0);
            //bool s = datab.CheckPerson("A1");
            datab.InserFirstCredential("A1", 5);
            //bool s1 = datab.CheckPerson("A1");
           // bool s=datab.CheckEntryTime("A1",datab.FindLastID("A1"));
            //Console.WriteLine(s.ToString());
            //int s= datab.FindLastID("A1");
            Console.WriteLine("Press enter to close...");
            Console.ReadLine();
            return 0;
        }
       */
    
    
    }



}
