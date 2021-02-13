using FreeDownloaderDotNet.Data;
using FreeDownloaderDotNet.Helpers;
using FreeDownloaderDotNet.Models;
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
    public partial class OptionsForm : Form
    {
        ApplicationDbContext _context;

        public OptionsForm(ApplicationDbContext context)
        {
            InitializeComponent();
            _context = context;
        }

        private void OptionsForm_Load(object sender, EventArgs e)
        {            
            listMenu.SelectedIndex = 0;
            if (true)
            {
                txtDownloadFolder.Text = _context.Settings.SingleOrDefault(a => a.Name == "DownloadFolder").Value;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
