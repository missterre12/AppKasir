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
using System.Drawing.Printing;

namespace AppKasir
{
    public partial class FormTransJual : Form
    {
        Koneksi konn = new Koneksi();
        private SqlCommand cmd;
        private DataSet ds;
        private SqlDataAdapter da;
        private SqlDataReader rd;

        public FormTransJual()
        {
            InitializeComponent();
            printDocument1.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(this.printDocument1_PrintPage);
        }

        public static class LoggedInUser
        {
            public static string NamaKasir { get; set; }
        }

        void KondisiAwal()
        {
            LBLHargaBarang.Text = "";
            LBLNamaKasir.Text = LoggedInUser.NamaKasir; // Perbarui nama kasir
            LBLNamaBarang.Text = "";
            LBLItem.Text = "";
            LBLKembali.Text = "";
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            LBLTotal.Text = "";
            LBLTanggal.Text = DateTime.Now.ToString("dd-MM-yyyy");
        }

        void BuatKolom()
        {
            dataGridView1.Columns.Clear();
            dataGridView1.Columns.Add("KodeBarang", "Kode Barang");
            dataGridView1.Columns.Add("NamaBarang", "Nama Barang");
            dataGridView1.Columns.Add("HargaBarang", "Harga Barang");
            dataGridView1.Columns.Add("JumlahJual", "Jumlah Barang");
            dataGridView1.Columns.Add("SubTotal", "Sub Total");
            dataGridView1.Columns[1].Width = 393;
            dataGridView1.Refresh();
        }
        private void FormTransJual_Load(object sender, EventArgs e)
        {
            KondisiAwal();
            BuatKolom();

            // Ambil nomor penjualan terakhir sebelum mengatur LBLNoJual
            int nomorPenjualanTerakhir = GetNomorPenjualanTerakhir();
            LBLNoJual.Text = (nomorPenjualanTerakhir + 1).ToString("D4");

            LoggedInUser.NamaKasir = "KSR001";
            LBLNamaKasir.Text = LoggedInUser.NamaKasir;

            button2.Click += button2_Click;
            LBLKembali.KeyPress += LBLKembali_KeyPress;

        }

        private System.Drawing.Printing.PrintDocument printDocument1 = new System.Drawing.Printing.PrintDocument();

        private int GetNomorPenjualanTerakhir()
        {
            int nomorPenjualanTerakhir = 0;
            try
            {
                using (SqlConnection conn = konn.GetConn())
                {
                    konn.OpenConnection(conn);

                    string query = "SELECT MAX(NoJual) FROM TBL_JUAL";
                    cmd = new SqlCommand(query, conn);
                    object result = cmd.ExecuteScalar();

                    if (result != DBNull.Value)
                    {
                        nomorPenjualanTerakhir = Convert.ToInt32(result);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            return nomorPenjualanTerakhir;
        }

        private void SimpanTransaksi()
        {
            try
            {
                using (SqlConnection conn = konn.GetConn())
                {
                    konn.OpenConnection(conn);

                    // Tambahkan data ke TBL_JUAL
                    string insertJualQuery = "INSERT INTO TBL_JUAL (TglJual, ItemJual, TotalJual, DiBayar, Kembali, KodeKasir) VALUES (@TglJual, @ItemJual, @TotalJual, @DiBayar, @Kembali, @KodeKasir); SELECT SCOPE_IDENTITY();";
                    cmd = new SqlCommand(insertJualQuery, conn);
                    cmd.Parameters.AddWithValue("@TglJual", DateTime.Now);
                    cmd.Parameters.AddWithValue("@ItemJual", LBLItem.Text);
                    cmd.Parameters.AddWithValue("@TotalJual", LBLTotal.Text);
                    cmd.Parameters.AddWithValue("@DiBayar", textBox3.Text);
                    cmd.Parameters.AddWithValue("@Kembali", LBLKembali.Text);
                    cmd.Parameters.AddWithValue("@KodeKasir", LoggedInUser.NamaKasir);

                    // Execute the insert command and retrieve the newly inserted NoJual
                    int noJual = Convert.ToInt32(cmd.ExecuteScalar());

                    // Tambahkan data ke TBL_DETAILJUAL
                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        string kodeBarang = Convert.ToString(row.Cells["KodeBarang"].Value);
                        string namaBarang = Convert.ToString(row.Cells["NamaBarang"].Value);
                        decimal hargaBarang = Convert.ToDecimal(row.Cells["HargaBarang"].Value);
                        int jumlahJual = Convert.ToInt32(row.Cells["JumlahJual"].Value);
                        decimal subTotal = Convert.ToDecimal(row.Cells["SubTotal"].Value);

                        string insertDetailQuery = "INSERT INTO TBL_DETAILJUAL (NoJual, KodeBarang, NamaBarang, HargaBarang, JumlahJual, SubTotal) VALUES (@NoJual, @KodeBarang, @NamaBarang, @HargaBarang, @JumlahJual, @SubTotal)";
                        cmd = new SqlCommand(insertDetailQuery, conn);
                        cmd.Parameters.AddWithValue("@NoJual", noJual);
                        cmd.Parameters.AddWithValue("@KodeBarang", kodeBarang);
                        cmd.Parameters.AddWithValue("@NamaBarang", namaBarang);
                        cmd.Parameters.AddWithValue("@HargaBarang", hargaBarang);
                        cmd.Parameters.AddWithValue("@JumlahJual", jumlahJual);
                        cmd.Parameters.AddWithValue("@SubTotal", subTotal);

                        cmd.ExecuteNonQuery();
                    }

                    // Update LBLNoJual only if the transaction is successfully saved
                    LBLNoJual.Text = noJual.ToString("D4");

                    // Bersihkan formulir setelah simpan
                    KondisiAwal();

                    // Kosongkan dataGridView1
                    dataGridView1.Rows.Clear();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void LBLKembali_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                // When Enter is pressed, save the transaction to the database and print receipt
                SimpanTransaksi();
                CetakStruk();
            }
        }

        private void CetakStruk()
        {
            PrintDocument pd = new PrintDocument();
            pd.PrintPage += new PrintPageEventHandler(PrintReceipt);

            try
            {
                pd.Print();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error printing receipt: " + ex.Message);
            }
        }

        private void PrintReceipt(object sender, PrintPageEventArgs e)
        {
            // Define the content to be printed on the receipt
            string content = $"Transaction Receipt\n\n" +
                             $"Date: {DateTime.Now}\n" +
                             $"Total: {LBLTotal.Text}\n" +
                             $"Amount Paid: {textBox3.Text}\n" +
                             $"Change: {LBLKembali.Text}\n\n" +
                             "Thank you for your purchase!";

            // Set the font and brush for printing
            Font font = new Font("Arial", 12);
            SolidBrush brush = new SolidBrush(Color.Black);

            // Set the position to start printing
            float yPos = 10f;
            float leftMargin = 10f;
            float topMargin = 10f;

            // Create a rectangle for the content
            RectangleF rect = new RectangleF(leftMargin, topMargin, e.PageBounds.Width - 2 * leftMargin, e.PageBounds.Height - 2 * topMargin);

            // Draw the content on the page
            e.Graphics.DrawString(content, font, brush, rect, new StringFormat());

            // Clean up resources
            font.Dispose();
            brush.Dispose();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            LBLJam.Text = DateTime.Now.ToString("hh:mm:ss tt");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SimpanTransaksi();
        }

        private void UpdateTotal()
        {
            // Calculate the total from the "SubTotal" column in dataGridView1
            int total = dataGridView1.Rows.Cast<DataGridViewRow>().Sum(row => Convert.ToInt32(row.Cells["SubTotal"].Value));
            LBLTotal.Text = total.ToString();
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                // Ketika tombol Enter pada textBox2 ditekan
                // Ambil data barang dari TBL_BARANG berdasarkan KodeBarang

                string kodeBarang = textBox2.Text.Trim();

                try
                {
                    using (SqlConnection conn = konn.GetConn())
                    {
                        konn.OpenConnection(conn);

                        string query = "SELECT NamaBarang, HargaJual, JumlahBarang FROM TBL_BARANG WHERE KodeBarang = @KodeBarang";
                        cmd = new SqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@KodeBarang", kodeBarang);

                        SqlDataReader reader = cmd.ExecuteReader();

                        if (reader.Read())
                        {
                            // Data ditemukan, isi informasi barang ke label dan textbox
                            LBLNamaBarang.Text = reader["NamaBarang"].ToString();
                            LBLHargaBarang.Text = reader["HargaJual"].ToString();
                            LBLItem.Text = reader["JumlahBarang"].ToString();
                            textBox1.Focus(); // Fokus ke textBox1 untuk memasukkan jumlah
                        }
                        else
                        {
                            MessageBox.Show("Kode barang tidak ditemukan.");
                            // Bersihkan textBox2 jika KodeBarang tidak ditemukan
                            textBox2.Clear();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                // Ketika tombol Enter pada textBox1 ditekan
                // Tambahkan data ke dataGridView1
                string kodeBarang = textBox2.Text.Trim(); // Ambil kode barang dari textBox2
                string jumlahJualStr = textBox1.Text.Trim(); // Ambil jumlah jual dari textBox1
                int jumlahJual;

                if (int.TryParse(jumlahJualStr, out jumlahJual))
                {
                    // Validasi berhasil, cek apakah stok mencukupi
                    int stok;

                    if (int.TryParse(LBLItem.Text, out stok))
                    {
                        if (jumlahJual <= stok)
                        {
                            // Stok mencukupi, tambahkan data ke dataGridView1
                            string namaBarang = LBLNamaBarang.Text;
                            string hargaBarang = LBLHargaBarang.Text;
                            int subtotal = jumlahJual * Convert.ToInt32(hargaBarang);

                            dataGridView1.Rows.Add(kodeBarang, namaBarang, hargaBarang, jumlahJual, subtotal);

                            // Update stok di label
                            LBLItem.Text = (stok - jumlahJual).ToString();

                            // Update total items in LBLItem by summing up the quantities in dataGridView1
                            int totalItems = dataGridView1.Rows.Cast<DataGridViewRow>().Sum(row => Convert.ToInt32(row.Cells["JumlahJual"].Value));
                            LBLItem.Text = totalItems.ToString();

                            // Bersihkan textBox2, LBLNamaBarang, LBLHargaBarang, dan textBox1 setelah memasukkan data
                            textBox2.Clear();
                            LBLNamaBarang.Text = "";
                            LBLHargaBarang.Text = "";
                            textBox1.Clear();

                            // Fokuskan kembali ke textBox2 untuk memasukkan kode barang baru
                            textBox2.Focus();

                            // Update the total
                            UpdateTotal();
                        }
                        else
                        {
                            MessageBox.Show("Stok tidak mencukupi.");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Stok tidak valid.");
                    }
                }
                else
                {
                    MessageBox.Show("Jumlah jual harus berupa angka.");
                }
            }
        }

        private void GetBarangInfo(string kodeBarang)
        {
            try
            {
                using (SqlConnection conn = konn.GetConn())
                {
                    konn.OpenConnection(conn);

                    string query = "SELECT NamaBarang, HargaBarang FROM TBL_BARANG WHERE KodeBarang = @KodeBarang";
                    cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@KodeBarang", kodeBarang);
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        LBLNamaBarang.Text = reader["NamaBarang"].ToString();
                        LBLHargaBarang.Text = reader["HargaBarang"].ToString();
                    }
                    else
                    {
                        MessageBox.Show("Barang tidak ditemukan.");
                        // Bersihkan LBLNamaBarang dan LBLHargaBarang jika tidak ada data ditemukan
                        LBLNamaBarang.Text = "";
                        LBLHargaBarang.Text = "";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            // Ketika isi textBox3 berubah, hitung kembalian dan tampilkan di LBLKembali
            CalculateKembalian();
        }

        private void CalculateKembalian()
        {
            // Cek apakah textBox3 berisi angka yang valid
            if (decimal.TryParse(textBox3.Text.Trim(), out decimal uangDibayar))
            {
                // Hitung kembalian
                decimal totalBelanja = decimal.Parse(LBLTotal.Text);
                decimal kembalian = uangDibayar - totalBelanja;

                // Tampilkan kembalian di LBLKembali
                LBLKembali.Text = kembalian.ToString();
            }
            else
            {
                // Jika input pada textBox3 tidak valid, kosongkan LBLKembali
                LBLKembali.Text = "";
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            PrintPreviewDialog printPreviewDialog = new PrintPreviewDialog();
            printPreviewDialog.Document = printDocument1;
            printPreviewDialog.ShowDialog();
        }

        private void printDocument1_PrintPage(object sender, PrintPageEventArgs e)
        {
            // Contoh: Gambar isi struk
            e.Graphics.DrawString("Struk Penjualan", new Font("Arial", 12, FontStyle.Bold), Brushes.Black, new PointF(10, 10));

            // Informasi transaksi
            e.Graphics.DrawString("Nomor Penjualan: " + LBLNoJual.Text, new Font("Arial", 10), Brushes.Black, new PointF(10, 40));

            // Header Tabel Barang
            e.Graphics.DrawString("Kode Barang", new Font("Arial", 10, FontStyle.Bold), Brushes.Black, new PointF(10, 70));
            e.Graphics.DrawString("Nama Barang", new Font("Arial", 10, FontStyle.Bold), Brushes.Black, new PointF(120, 70));
            e.Graphics.DrawString("Harga", new Font("Arial", 10, FontStyle.Bold), Brushes.Black, new PointF(300, 70));
            e.Graphics.DrawString("Jumlah", new Font("Arial", 10, FontStyle.Bold), Brushes.Black, new PointF(400, 70));
            e.Graphics.DrawString("Subtotal", new Font("Arial", 10, FontStyle.Bold), Brushes.Black, new PointF(500, 70));

            // Informasi Barang
            int yPos = 100;
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Cells["KodeBarang"].Value != null && row.Cells["NamaBarang"].Value != null &&
                    row.Cells["HargaBarang"].Value != null && row.Cells["JumlahJual"].Value != null &&
                    row.Cells["SubTotal"].Value != null)
                {
                    e.Graphics.DrawString(row.Cells["KodeBarang"].Value.ToString(), new Font("Arial", 10), Brushes.Black, new PointF(10, yPos));
                    e.Graphics.DrawString(row.Cells["NamaBarang"].Value.ToString(), new Font("Arial", 10), Brushes.Black, new PointF(120, yPos));
                    e.Graphics.DrawString(row.Cells["HargaBarang"].Value.ToString(), new Font("Arial", 10), Brushes.Black, new PointF(300, yPos));
                    e.Graphics.DrawString(row.Cells["JumlahJual"].Value.ToString(), new Font("Arial", 10), Brushes.Black, new PointF(400, yPos));
                    e.Graphics.DrawString(row.Cells["SubTotal"].Value.ToString(), new Font("Arial", 10), Brushes.Black, new PointF(500, yPos));
                }

                yPos += 20;
            }
        }
    }
}
