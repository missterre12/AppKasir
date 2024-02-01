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
using CrystalDecisions.CrystalReports.Engine;

namespace AppKasir.Laporan
{
    public partial class CetakLaporanBarang : Form
    {
        private DataSet ds;
        private SqlDataAdapter da;
        Koneksi konn = new Koneksi();

        void cetak()
        {
            SqlConnection conn = konn.GetConn();
            {
                conn.Open();
                da = new SqlDataAdapter("select * from TBL_BARANG order by KodeBarang asc", conn);
                ds = new DataSet();
                da.Fill(ds, "TBL_BARANG");
                reportBarang myreport = new reportBarang();
                myreport.SetDataSource(ds);
                crystalReportViewer1.ReportSource = myreport;
                crystalReportViewer1.Refresh();
            }
        }
        public CetakLaporanBarang()
        {
            InitializeComponent();
            cetak();
        }

        private void CetakLaporanBarang_Load(object sender, EventArgs e)
        {

        }
    }
}
