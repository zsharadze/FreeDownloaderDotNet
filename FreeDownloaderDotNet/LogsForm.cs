using FreeDownloaderDotNet.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FreeDownloaderDotNet
{
    public partial class LogsForm : Form
    {
        private readonly ApplicationDbContext _context;

        public LogsForm(ApplicationDbContext context)
        {
            InitializeComponent();
            _context = context;
        }

        private void LogsForm_Load(object sender, EventArgs e)
        {
            foreach (var item in _context.Logs)
            {
                txtLogs.Text += item.CreatedDate.ToString("dd-MM-yyyy HH:mm:ss") + ": " + (item.IsError ? "Error: " : "") + item.Message + " " + Environment.NewLine;
            }

            if (txtLogs.Text != "")
                txtLogs.Select(0, 0);
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            _context.Logs.RemoveRange(_context.Logs);
            _context.SaveChanges();
            txtLogs.Text = "";
        }
    }
}
