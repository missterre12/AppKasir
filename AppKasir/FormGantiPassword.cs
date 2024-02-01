using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace AppKasir
{
    public partial class FormGantiPassword : Form
    {
        private bool userNotLoggedIn;

        Koneksi konn = new Koneksi();
        private SqlCommand cmd;
        private DataSet ds;
        private SqlDataAdapter da;
        private SqlDataReader rd;

        public FormGantiPassword(bool userNotLoggedIn)
        {
            InitializeComponent();
            this.userNotLoggedIn = userNotLoggedIn;

            if (userNotLoggedIn)
            {
                EnableFormControls();
            }
            else
            {
                MessageBox.Show("You must log out to access the password change feature.");
                this.Close();
            }
        }

        private void EnableFormControls()
        {
            LBLUsername.Enabled = true;
            LBLPasswordBaru.Enabled = true;
            LBLKonfPasswordBaru.Enabled = true;
            btn_simpan.Enabled = true;
        }

        private void FormGantiPassword_Load(object sender, EventArgs e)
        {
            if (userNotLoggedIn)
            {
                EnableFormControls();
            }
            else
            {
                MessageBox.Show("You must log out to access the password change feature.");
                this.Close();
            }
        }

        private void btn_simpan_Click(object sender, EventArgs e)
        {
            if (userNotLoggedIn)
            {
                string connectionString = "Data Source=LAPTOP-1080RJAM\\SQLEXPRESS;Initial Catalog=DB_KASIR;Integrated Security=True"; 
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                        string updateQuery = "UPDATE TBL_KASIR SET PasswordKasir = @PasswordKasir WHERE NamaKasir = @NamaKasir";

                        using (SqlCommand cmd = new SqlCommand(updateQuery, connection))
                        {
                            cmd.Parameters.AddWithValue("@PasswordKasir", LBLPasswordBaru.Text);
                            cmd.Parameters.AddWithValue("@NamaKasir", LBLUsername.Text);

                            int rowsAffected = cmd.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Password berhasil diubah!");
                                this.Close();
                            }
                            else
                            {
                                MessageBox.Show("Gagal mengganti password!");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: " + ex.Message);
                    }
                }
            }
        }
    }
}