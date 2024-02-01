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
    public partial class CetakLaporanPenjualan : Form
    {
        private DataSet ds;
        private SqlDataAdapter da;
        Koneksi konn = new Koneksi();

        void cetak()
        {
            SqlConnection conn = konn.GetConn();
            {
                conn.Open();
                da = new SqlDataAdapter("select * from TBL_DETAILJUAL order by DetailId asc", conn);
                ds = new DataSet();
                da.Fill(ds, "TBL_DETAILJUAL");
                reportPenjualan myreport = new reportPenjualan();
                myreport.SetDataSource(ds);
                crystalReportViewer1.ReportSource = myreport;
                crystalReportViewer1.Refresh();
            }
        }
        public CetakLaporanPenjualan()
        {
            InitializeComponent();
            cetak();
        }

        private void CetakLaporanPenjualan_Load(object sender, EventArgs e)
        {

        }
    }
}
