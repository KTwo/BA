using BiometricsAnalysis.Tables;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace BiometricsAnalysis
{
    public class Database_Utils
    {
        private string _host;
        private string _port;
        private string _db_user_name;
        private string _db_name;
        private string _db_password;
        private SqlConnection _db_conn;

        public Database_Utils()
        {
            host = "188.26.151.237\\K2_BA_RT_AS_ES";
            db_user_name = "administrator";
            db_password = "nicusor";
            db_name = "K2_BA";
            db_conn = new SqlConnection(
                "user id=" + db_user_name + ";" +
                "password=" + db_password + ";" + 
                "server=" + host + ";" + 
                "database=" + db_name + ";" + 
                "connection timeout=5"
                );
            try
            {
                db_conn.Open();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        public Database_Utils(string host, string db_name, string db_user_name, string db_password)
        {
            this.host = host;
            this.db_name = db_name;
            this.db_user_name = db_user_name;
            this.db_password = db_password;
            db_conn = new SqlConnection(
                "user id=" + db_user_name + ";" +
                "password=" + db_password + ";" +
                "server=" + host + ";" +
                "database=" + db_name + ";" +
                "connection timeout=5"
                );
            try
            {
                db_conn.Open();
            }
            catch (Exception e)
            {
               Console.WriteLine(e.Message);
            }
        }

        public void close()
        {
            try
            {
                db_conn.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
        public int query_user(string user_name, string password)
        {
            string query = "SELECT * FROM users WHERE user_name =@User AND password =@Pass;";

            SqlCommand comm = new SqlCommand(query, db_conn);
            comm.Parameters.AddWithValue("@User", user_name);
            comm.Parameters.AddWithValue("@Pass", password);

            int id_user;
            try
            {
                // executa query-ul si daca gaseste user-ul in baza de date, ii intoarce id-ul
                id_user = Int32.Parse(comm.ExecuteScalar().ToString());
                
            }
            catch (Exception e)
            {
                // nu a gasit user-ul in baza de date
                id_user = 0;
            }

            if (id_user != 0)
            {
                comm.Dispose();
                return id_user;
            }
            else
            {
                comm.Dispose();
                return -1;
            }
        }

        public int insert_log(Log log)
        {
            string query = "INSERT INTO logs (id_user, login_date, ip) VALUES (@id_user, @login_date, @ip)";
            SqlDataReader reader = null;
            SqlCommand comm = new SqlCommand(query, db_conn);
            comm.Parameters.AddWithValue("@id_user", log.id_user);
            comm.Parameters.AddWithValue("@login_date", log.login_date);
            comm.Parameters.AddWithValue("@ip", log.ip);
            reader = comm.ExecuteReader();
            reader.Close();
            comm.Dispose();

            query = "SELECT * FROM logs WHERE id_user = @id_user AND logout_date IS NULL";
            int id_log;
            comm = new SqlCommand(query, db_conn);
            comm.Parameters.AddWithValue("@id_user", log.id_user);

            try
            {
                id_log = Int32.Parse(comm.ExecuteScalar().ToString());

                Audit audit = new Audit(id_log, "Login", "The user has logged in");
                this.insert_audit(audit);
            }
            catch (Exception e)
            {
                id_log = 0;
            }

            reader.Close();
            comm.Dispose();

            return id_log;
        }
        /// <summary>
        /// Adauga un nou element in Entries
        /// </summary>
        /// <author>GABRIEL</author>
        /// <param name="firstName">Prenume</param>
        /// <param name="lastName">Nume</param>
        /// <returns>Id-ul elementului adaugat</returns>
        public int insert_entry(string firstName, string lastName)
        {
            string query = "INSERT INTO entries (FirstName, LastName) VALUES (@FirstName, @LastName)";
            SqlDataReader reader = null;
            SqlCommand comm = new SqlCommand(query, db_conn);
            comm.Parameters.AddWithValue("@FirstName", firstName);
            comm.Parameters.AddWithValue("@LastName", lastName);
            reader = comm.ExecuteReader();
            reader.Close();
            comm.Dispose();

            string select = "SELECT * FROM entries WHERE FirstName = @FirstName AND LastName = @LastName";
            int EntryId = -1;
            comm = new SqlCommand(select, db_conn);
            comm.Parameters.AddWithValue("@FirstName", firstName);
            comm.Parameters.AddWithValue("@LastName", lastName);
            try
            {
                String response = comm.ExecuteScalar().ToString();
                EntryId = Int32.Parse(response);
                if (EntryId != -1)
                {
                    reader = comm.ExecuteReader();
                    reader.Close();
                }
                else
                {
                    Console.WriteLine("User {0} {1} deja exista in tabelul Entries", firstName, lastName);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Eroare SQL la {0} {1} in tabelul Entries", firstName, lastName);
            }
            reader.Close();
            comm.Dispose();
            return EntryId;
        }
        /// <summary>
        /// Insert a face element in FACES
        /// </summary>
        /// <author>GABRIEL</author>>
        /// <param name="face">Face object containing all the info</param>
        /// <returns>the inserted elements FaceId</returns>
        public int insert_face(Face face)
        {

            String bites = ImageToBinary(face.img.ToString());
            BCM encoder = new BCM();
            // encrypt image
            String encrypted = BinaryStringToHexString(encoder.crypt_binary(bites, 123, 456));

/*
            // get from bits to bytes 
            var byteArray = BinaryToByte(hex2binary(temp));
            // save encrypted image 
            File.WriteAllBytes("nicusor2.bmp", byteArray);

            // decode entrypted text 
            encoder.decrypt_text(encrypted, 123, 456);
            String decripted = encoder.getDecryptes_binary;
            // get from bits to bytes 
            var byteArray2 = BinaryToByte(decripted);
            // save decripted image 
            File.WriteAllBytes("nicusor3.bmp", byteArray2);
            //BitmapImage image = BinaryToImage(bites);
*/
            string query = "INSERT INTO faces (EntryId, Percentage, IsSample, Comment, Img)" + 
                " VALUES (@EntryId, @Percentage, @IsSample, @Comment, @Img)";
            SqlDataReader reader = null;
            SqlCommand comm = new SqlCommand(query, db_conn);
            comm.Parameters.AddWithValue("@EntryId", face.entryId);
            comm.Parameters.AddWithValue("@Percentage", face.percentage);
            comm.Parameters.AddWithValue("@IsSample", face.isSample);
            comm.Parameters.AddWithValue("@Comment", face.comment);
            comm.Parameters.AddWithValue("@Img", encrypted);
            reader = comm.ExecuteReader();
            
            reader.Close();
            comm.Dispose();

            string select = "SELECT * FROM faces WHERE EntryId = @EntryId AND Percentage = @Percentage " + 
                "AND IsSample = @IsSample AND Comment = @Comment AND Img = @Img";
            int FaceId = -1;
            comm = new SqlCommand(select, db_conn);
            comm.Parameters.AddWithValue("@EntryId", face.entryId);
            comm.Parameters.AddWithValue("@Percentage", face.percentage);
            comm.Parameters.AddWithValue("@IsSample", face.isSample);
            comm.Parameters.AddWithValue("@Comment", face.comment);
            comm.Parameters.AddWithValue("@Img", encrypted);
            try
            {
                FaceId = Int32.Parse(comm.ExecuteScalar().ToString());
                Console.WriteLine(FaceId);
            }
            catch (Exception e)
            {
                Console.WriteLine("Eroare SQL la {0} {1} in tabelul Entries", face.entryId, face.percentage);
                Console.WriteLine(e.Message);
            }
            reader.Close();
            comm.Dispose();
            return FaceId;
        }

        public static string BinaryStringToHexString(string binary)
        {
            StringBuilder result = new StringBuilder(binary.Length / 8 + 1);
            
            int mod4Len = binary.Length % 8;
            if (mod4Len != 0)
            {
                // pad to length multiple of 8
                binary = binary.PadLeft(((binary.Length / 8) + 1) * 8, '0');
            }

            for (int i = 0; i < binary.Length; i += 8)
            {
                string eightBits = binary.Substring(i, 8);
                result.AppendFormat("{0:X2}", Convert.ToByte(eightBits, 2));
            }

            return result.ToString();
        }

        private string hex2binary(string hexvalue)
        {
            return String.Join(String.Empty, hexvalue.Select(c => Convert.ToString(Convert.ToInt32(c.ToString(), 16), 2).PadLeft(4, '0')));
        }
        
        private byte[] BinaryToByte(String bites)
        {
            return Enumerable.Range(0, int.MaxValue / 8)
                          .Select(i => i * 8)
                          .TakeWhile(i => i < bites.Length)
                          .Select(i => bites.Substring(i, 8))
                          .Select(s => Convert.ToByte(s, 2))
                          .ToArray();
        }

        public static String ImageToBinary(string imagePath)
        {
            FileStream fS = new FileStream(imagePath, FileMode.Open, FileAccess.Read);
            byte[] b = new byte[fS.Length];
            fS.Read(b, 0, (int)fS.Length);
            fS.Close();

            String[] biteString = new String[b.Length];
            for (int counter = 0; counter < b.Length; counter++)
            {
                StringBuilder out_string = new StringBuilder();
                byte mask = 128;
                for (int i = 7; i >= 0; --i)
                {
                    out_string.Append((b[counter] & mask) != 0 ? "1" : "0");
                    mask >>= 1;
                }
                biteString[counter] = out_string.ToString();
            }

            String bits = string.Join("", biteString);
            return bits;
        }
        
        public static BitmapImage BinaryToImage(String bites)
        {
            var byteArray = Enumerable.Range(0, int.MaxValue / 8)
                          .Select(i => i * 8)
                          .TakeWhile(i => i < bites.Length)
                          .Select(i => bites.Substring(i, 8))
                          .Select(s => Convert.ToByte(s, 2))
                          .ToArray();
            //File.WriteAllBytes(string path, byte[] bytes);
            TypeConverter tc = TypeDescriptor.GetConverter(typeof(Bitmap));
            BitmapImage temp = (BitmapImage)tc.ConvertFrom(byteArray);
            return temp;
        }
        public void close_log(Log log)
        {
            log.logout_date = Convert.ToDateTime(DateTime.Now.ToString("HH:mm:ss.fff"));

            string query = "UPDATE logs SET logout_date = @logout_date WHERE id_log = @id_log";

            SqlDataReader reader = null;
            SqlCommand comm = new SqlCommand(query, db_conn);
            comm.Parameters.AddWithValue("@id_log", log.id_log);
            comm.Parameters.AddWithValue("@logout_date", log.logout_date);
            
            reader = comm.ExecuteReader();
           
            comm.Dispose();
            reader.Close();

            Audit audit = new Audit(log.id_log, "Logout", "The user has logged out");
            this.insert_audit(audit);
        }

        public void insert_audit(Audit audit)
        {
            string query = "INSERT INTO audits (id_log, name_audit, info_audit, date_audit) VALUES (@id_log, @name_audit, @info_audit, @date_audit)";
            SqlDataReader reader = null;
            SqlCommand comm = new SqlCommand(query, db_conn);
            comm.Parameters.AddWithValue("@id_log", audit.id_log);
            comm.Parameters.AddWithValue("@name_audit", audit.name_audit);
            comm.Parameters.AddWithValue("@info_audit", audit.info_audit);
            comm.Parameters.AddWithValue("@date_audit", audit.date_audit);
            reader = comm.ExecuteReader();
            reader.Close();
            comm.Dispose();
        }

        public List<string> get_Entries()
        {
            List<string> entries = new List<string>();
            string query = "SELECT * FROM entries";
            int id_log;
            SqlCommand comm = new SqlCommand(query, db_conn);
            SqlDataReader reader = null;
            reader = comm.ExecuteReader();
             
            while (reader.Read())
            {
                string firstName = (string)reader["FirstName"];
                string lastName = (string)reader["LastName"];
                entries.Add(firstName + " " + lastName);
            }

            reader.Close();
            comm.Dispose();
            return entries;
        }

        public List<string> get_Faces(String lastName, String firstName)
        {
            List<string> faces = new List<string>();
            string select = "SELECT FaceId FROM entries e JOIN faces f ON e.EntryId = f.EntryId WHERE FirstName = @FirstName AND LastName = @LastName";

            SqlCommand comm = new SqlCommand(select, db_conn);
            comm.Parameters.AddWithValue("@FirstName", firstName);
            comm.Parameters.AddWithValue("@LastName", lastName);
            SqlDataReader reader = null;
            try
            {
                reader = comm.ExecuteReader();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

            while (reader.Read())
            {
                int faceId = (int)reader["FaceId"];
                faces.Add(faceId.ToString());
            }

            reader.Close();
            comm.Dispose();
            return faces;
        }

        public string host { get { return _host; } set { _host = value; } }
        public string port { get { return _port; } set { _port = value; } }
        public string db_user_name { get { return _db_user_name; } set { _db_user_name = value; } }
        public string db_password { get { return _db_password; } set { _db_password = value; } }
        public string db_name { get { return _db_name; } set { _db_name = value; } }
        public SqlConnection db_conn { get { return _db_conn; } set { _db_conn = value; } }

    }
}
