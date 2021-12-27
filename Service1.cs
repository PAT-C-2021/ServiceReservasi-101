using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace ServiceReservasi
{
    
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in both code and config file together.
    public class Service1 : IService1
    {
        string koneksi = "Data Source = RADITYA-PRATAMA;Initial Catalog=WCFReservasi;Persist Security Info = True;User ID=sa;Password=pratamaputra1";
        SqlConnection conn;
        SqlCommand com;

        public string Register(string username, string password, string kategori)
        {
            try
            {
                string sql = "INSERT into dbo.Login VALUES('" + username + "', '" + password + "', '" + kategori + "')";
                conn = new SqlConnection(koneksi);
                com = new SqlCommand(sql, conn);
                conn.Open();
                com.ExecuteNonQuery();
                conn.Close();

                return "Sukses";
            }
            catch (Exception e)
            {
                return e.ToString();
            }
        }


        public string Login(string username, string password)
        {
            string kategori = "";

            string sql = "SELECT Kategori from dbo.Login where Username='" + username + "' and Password='" + password + "'";
            conn = new SqlConnection(koneksi);
            com = new SqlCommand(sql, conn);
            conn.Open();

            SqlDataReader reader = com.ExecuteReader();
            while (reader.Read())
            {
                kategori = reader.GetString(0);
            }
            conn.Close();
            return kategori;
        }
        public List<DataRegister> DataRegist()
        {
            List<DataRegister> list = new List<DataRegister>();
            try
            {
                string sql = "SELECT ID_login, Username, Password, Kategori from Login";
                conn = new SqlConnection(koneksi);
                com = new SqlCommand(sql, conn);
                conn.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    DataRegister data = new DataRegister();
                    data.Id = reader.GetInt32(0);
                    data.Username = reader.GetString(1);
                    data.Password = reader.GetString(2);
                    data.Kategori = reader.GetString(3);
                    list.Add(data);

                }
                conn.Close();
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return list;
        }
        public string DeleteRegister(string username)
        {
            try
            {
                int id = 0;
                string sql = "SELECT ID_login from dbo.Login where Username='" + username + "'";
                conn = new SqlConnection(koneksi);
                com = new SqlCommand(sql, conn);
                conn.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    id = reader.GetInt32(0);
                }
                conn.Close();

                string sql2 = "DELETE from dbo.Login where ID_login=" + id + "";
                conn = new SqlConnection(koneksi);
                com = new SqlCommand(sql2, conn);
                conn.Open();
                com.ExecuteNonQuery();
                conn.Close();

                return "Berhasil";
            }
            catch (Exception e)
            {
                return e.ToString();
            }
        }
        public string UpdateRegister(string username, string password, string kategori, int id)
        {
            try
            {
                string sql2 = "UPDATE dbo.Login set Username='" + username + "', Password='" + password + "',Kategori='" + kategori + "'" +
                    "where ID_login=" + id + "";
                conn = new SqlConnection(koneksi);
                com = new SqlCommand(sql2, conn);
                conn.Open();
                com.ExecuteNonQuery();
                conn.Close();

                return "Sukses";
            }
            catch (Exception e)
            {
                return e.ToString();
            }
        }
        public List<DetailLokasi> DetailLokasi()
        {
            List<DetailLokasi> LokasiFull = new List<DetailLokasi>();
            try
            {
                string sql = "SELECT ID_lokasi, Nama_lokasi, Deskripsi_full, Kuota from dbo.Lokasi";
                conn = new SqlConnection(koneksi);
                com = new SqlCommand(sql, conn);
                conn.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    DetailLokasi data = new DetailLokasi();
                    data.IDLokasi = reader.GetString(0);
                    data.NamaLokasi = reader.GetString(1);
                    data.DeskripsiFull = reader.GetString(2);
                    data.Kuota = reader.GetInt32(3);
                    LokasiFull.Add(data);

                }
                conn.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return LokasiFull;
        }

        public List<Pemesanan> Pemesanan()
        {
            List<Pemesanan> pemesanans = new List<Pemesanan>();
            try
            {
                string sql = "SELECT ID_reservasi, Nama_customer, No_telpon, Jumlah_pemesanan, Nama_lokasi FROM dbo.Pemesanan p JOIN dbo.Lokasi l on p.ID_lokasi = l.ID_lokasi";
                conn = new SqlConnection(koneksi);
                com = new SqlCommand(sql, conn);
                conn.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    Pemesanan data = new Pemesanan();
                    data.IDPemesanan = reader.GetString(0);
                    data.NamaCustomer = reader.GetString(1);
                    data.NoTelpon = reader.GetString(2);
                    data.JumlahPemesanan = reader.GetInt32(3);
                    data.IDLokasi = reader.GetString(4);
                    pemesanans.Add(data);

                }
                conn.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return pemesanans;
        }

        public string pemesanan(string IDPemesanan, string NamaCustomer, string NoTelpon, int JumlahPemesanan, string IDLokasi)
        {
            string n = "gagal";
            try
            {
                string sql = "INSERT INTO dbo.Pemesanan VALUES('" + IDPemesanan + "','" + NamaCustomer + "','" + NoTelpon + "'," + JumlahPemesanan + ",'" + IDLokasi + "')";
                conn = new SqlConnection(koneksi);
                com = new SqlCommand(sql, conn);
                conn.Open();
                com.ExecuteNonQuery();
                conn.Close();

                string sql2 = "UPDATE dbo.Lokasi set Kuota = Kuota - " + JumlahPemesanan + " WHERE ID_lokasi = '" + IDLokasi + "' ";
                conn = new SqlConnection(koneksi);
                com = new SqlCommand(sql2, conn);
                conn.Open();
                com.ExecuteNonQuery();
                conn.Close();

                n = "Berhasil";

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return n;
        }

        public string editPemesanan(string IDPemesanan, string NamaCustomer, string No_telpon)
        {
            string a = "gagal";
            try
            {
                string sql = "UPDATE dbo.Pemesanan SET Nama_customer = '" + NamaCustomer + "', No_telpon = '" + No_telpon + "' WHERE ID_reservasi = '" + IDPemesanan + "' ";
                conn = new SqlConnection(koneksi);
                com = new SqlCommand(sql, conn);
                conn.Open();
                com.ExecuteNonQuery();
                conn.Close();

                a = "Berhasil";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return a;
        }

        public string deletePemesanan(string IDPemesanan)
        {
            string a = "gagal";
            try
            {
                string sql = "DELETE FROM dbo.Pemesanan WHERE ID_reservasi = '" + IDPemesanan + "' ";
                conn = new SqlConnection(koneksi);
                com = new SqlCommand(sql, conn);
                conn.Open();
                com.ExecuteNonQuery();
                conn.Close();

                a = "Berhasil";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return a;
        }

    }
}
