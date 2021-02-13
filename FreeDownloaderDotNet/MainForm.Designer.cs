namespace FreeDownloaderDotNet
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.TreeNode treeNode6 = new System.Windows.Forms.TreeNode("All Downloads (0/0)");
            System.Windows.Forms.TreeNode treeNode7 = new System.Windows.Forms.TreeNode("Videos (0)");
            System.Windows.Forms.TreeNode treeNode8 = new System.Windows.Forms.TreeNode("Music (0)");
            System.Windows.Forms.TreeNode treeNode9 = new System.Windows.Forms.TreeNode("Other (0)");
            System.Windows.Forms.TreeNode treeNode10 = new System.Windows.Forms.TreeNode("Completed (0)");
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.ribbonPanel2 = new System.Windows.Forms.RibbonPanel();
            this.ribbonPanel3 = new System.Windows.Forms.RibbonPanel();
            this.treeViewLeftPanel = new System.Windows.Forms.TreeView();
            this.kryptonDataGridView1 = new ComponentFactory.Krypton.Toolkit.KryptonDataGridView();
            this.State = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StartDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colFile = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colUrl = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPercentage = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.contextMenuGridView = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ribbonPanel4 = new System.Windows.Forms.RibbonPanel();
            this.dataGridViewImageColumn1 = new System.Windows.Forms.DataGridViewImageColumn();
            this.colState = new System.Windows.Forms.DataGridViewImageColumn();
            this.startToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.stopToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyUrlToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showInExplorerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ribbon1 = new System.Windows.Forms.Ribbon();
            this.ribbonOrbMenuOptions = new System.Windows.Forms.RibbonOrbMenuItem();
            this.ribbonOrbMenuLogs = new System.Windows.Forms.RibbonOrbMenuItem();
            this.ribbonOrbMenuExit = new System.Windows.Forms.RibbonOrbMenuItem();
            this.ribbonTab1 = new System.Windows.Forms.RibbonTab();
            this.ribbonPanel1 = new System.Windows.Forms.RibbonPanel();
            this.rbtnAddUrl = new System.Windows.Forms.RibbonButton();
            this.rbtnStartDownload = new System.Windows.Forms.RibbonButton();
            this.rbtnStopDownload = new System.Windows.Forms.RibbonButton();
            this.rbtnDeleteDownload = new System.Windows.Forms.RibbonButton();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonDataGridView1)).BeginInit();
            this.contextMenuGridView.SuspendLayout();
            this.SuspendLayout();
            // 
            // ribbonPanel2
            // 
            this.ribbonPanel2.ButtonMoreVisible = false;
            this.ribbonPanel2.Items.Add(this.rbtnStartDownload);
            this.ribbonPanel2.Name = "ribbonPanel2";
            this.ribbonPanel2.Text = "Start";
            // 
            // ribbonPanel3
            // 
            this.ribbonPanel3.ButtonMoreVisible = false;
            this.ribbonPanel3.Items.Add(this.rbtnStopDownload);
            this.ribbonPanel3.Name = "ribbonPanel3";
            this.ribbonPanel3.Text = "Stop";
            // 
            // treeViewLeftPanel
            // 
            this.treeViewLeftPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.treeViewLeftPanel.Location = new System.Drawing.Point(0, 114);
            this.treeViewLeftPanel.Name = "treeViewLeftPanel";
            treeNode6.Name = "trNodeAllDownloads";
            treeNode6.Tag = "All";
            treeNode6.Text = "All Downloads (0/0)";
            treeNode7.Name = "trNodeVideos";
            treeNode7.Tag = "Videos";
            treeNode7.Text = "Videos (0)";
            treeNode8.Name = "trNodeMusic";
            treeNode8.Tag = "Music";
            treeNode8.Text = "Music (0)";
            treeNode9.Name = "trNodeOther";
            treeNode9.Tag = "Other";
            treeNode9.Text = "Other (0)";
            treeNode10.Name = "trNodeCompleted";
            treeNode10.Tag = "Completed";
            treeNode10.Text = "Completed (0)";
            this.treeViewLeftPanel.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode6,
            treeNode7,
            treeNode8,
            treeNode9,
            treeNode10});
            this.treeViewLeftPanel.Size = new System.Drawing.Size(207, 336);
            this.treeViewLeftPanel.TabIndex = 5;
            this.treeViewLeftPanel.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeViewLeftPanel_AfterSelect);
            // 
            // kryptonDataGridView1
            // 
            this.kryptonDataGridView1.AllowUserToAddRows = false;
            this.kryptonDataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.kryptonDataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.kryptonDataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colState,
            this.State,
            this.StartDate,
            this.Id,
            this.colFile,
            this.colUrl,
            this.colPercentage});
            this.kryptonDataGridView1.ContextMenuStrip = this.contextMenuGridView;
            this.kryptonDataGridView1.Location = new System.Drawing.Point(210, 114);
            this.kryptonDataGridView1.MultiSelect = false;
            this.kryptonDataGridView1.Name = "kryptonDataGridView1";
            this.kryptonDataGridView1.RowHeadersVisible = false;
            this.kryptonDataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.kryptonDataGridView1.Size = new System.Drawing.Size(589, 336);
            this.kryptonDataGridView1.TabIndex = 6;
            this.kryptonDataGridView1.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.kryptonDataGridView1_CellMouseDown);
            // 
            // State
            // 
            this.State.DataPropertyName = "State";
            this.State.HeaderText = "State";
            this.State.Name = "State";
            this.State.Visible = false;
            // 
            // StartDate
            // 
            this.StartDate.DataPropertyName = "StartDate";
            this.StartDate.HeaderText = "StartDate";
            this.StartDate.Name = "StartDate";
            this.StartDate.Visible = false;
            // 
            // Id
            // 
            this.Id.DataPropertyName = "Id";
            this.Id.HeaderText = "Id";
            this.Id.Name = "Id";
            this.Id.ReadOnly = true;
            this.Id.Visible = false;
            // 
            // colFile
            // 
            this.colFile.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCellsExceptHeader;
            this.colFile.DataPropertyName = "Filename";
            this.colFile.HeaderText = "File";
            this.colFile.Name = "colFile";
            this.colFile.Width = 5;
            // 
            // colUrl
            // 
            this.colUrl.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colUrl.DataPropertyName = "Url";
            this.colUrl.HeaderText = "Url";
            this.colUrl.Name = "colUrl";
            // 
            // colPercentage
            // 
            this.colPercentage.DataPropertyName = "Percentage";
            this.colPercentage.HeaderText = "%";
            this.colPercentage.Name = "colPercentage";
            this.colPercentage.ReadOnly = true;
            // 
            // contextMenuGridView
            // 
            this.contextMenuGridView.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.contextMenuGridView.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.startToolStripMenuItem,
            this.stopToolStripMenuItem,
            this.deleteToolStripMenuItem,
            this.copyUrlToolStripMenuItem,
            this.showInExplorerToolStripMenuItem});
            this.contextMenuGridView.Name = "contextMenuGridView";
            this.contextMenuGridView.Size = new System.Drawing.Size(162, 114);
            this.contextMenuGridView.Opened += new System.EventHandler(this.contextMenuGridView_Opened);
            // 
            // ribbonPanel4
            // 
            this.ribbonPanel4.Items.Add(this.rbtnDeleteDownload);
            this.ribbonPanel4.Name = "ribbonPanel4";
            this.ribbonPanel4.Text = "Delete";
            // 
            // dataGridViewImageColumn1
            // 
            this.dataGridViewImageColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewImageColumn1.DataPropertyName = "stateImg";
            this.dataGridViewImageColumn1.FillWeight = 16F;
            this.dataGridViewImageColumn1.HeaderText = "";
            this.dataGridViewImageColumn1.Image = global::FreeDownloaderDotNet.Properties.Resources.grid_stopped;
            this.dataGridViewImageColumn1.Name = "dataGridViewImageColumn1";
            this.dataGridViewImageColumn1.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewImageColumn1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // colState
            // 
            this.colState.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colState.DataPropertyName = "StateImg";
            this.colState.FillWeight = 11F;
            this.colState.HeaderText = "";
            this.colState.Image = global::FreeDownloaderDotNet.Properties.Resources.grid_stopped;
            this.colState.Name = "colState";
            this.colState.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colState.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // startToolStripMenuItem
            // 
            this.startToolStripMenuItem.Image = global::FreeDownloaderDotNet.Properties.Resources.play_button_28;
            this.startToolStripMenuItem.Name = "startToolStripMenuItem";
            this.startToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.startToolStripMenuItem.Text = "Start";
            this.startToolStripMenuItem.Click += new System.EventHandler(this.startToolStripMenuItem_Click);
            // 
            // stopToolStripMenuItem
            // 
            this.stopToolStripMenuItem.Image = global::FreeDownloaderDotNet.Properties.Resources.stop_button_28;
            this.stopToolStripMenuItem.Name = "stopToolStripMenuItem";
            this.stopToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.stopToolStripMenuItem.Text = "Stop";
            this.stopToolStripMenuItem.Click += new System.EventHandler(this.stopToolStripMenuItem_Click);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Image = global::FreeDownloaderDotNet.Properties.Resources.delete_btn2_28;
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.deleteToolStripMenuItem.Text = "Delete";
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
            // 
            // copyUrlToolStripMenuItem
            // 
            this.copyUrlToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("copyUrlToolStripMenuItem.Image")));
            this.copyUrlToolStripMenuItem.Name = "copyUrlToolStripMenuItem";
            this.copyUrlToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.copyUrlToolStripMenuItem.Text = "Copy Url";
            this.copyUrlToolStripMenuItem.Click += new System.EventHandler(this.copyUrlToolStripMenuItem_Click);
            // 
            // showInExplorerToolStripMenuItem
            // 
            this.showInExplorerToolStripMenuItem.Image = global::FreeDownloaderDotNet.Properties.Resources.show_in_explorer_28;
            this.showInExplorerToolStripMenuItem.Name = "showInExplorerToolStripMenuItem";
            this.showInExplorerToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.showInExplorerToolStripMenuItem.Text = "Show in Explorer";
            this.showInExplorerToolStripMenuItem.Click += new System.EventHandler(this.showInExplorerToolStripMenuItem_Click);
            // 
            // ribbon1
            // 
            this.ribbon1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.ribbon1.Location = new System.Drawing.Point(0, 0);
            this.ribbon1.Minimized = false;
            this.ribbon1.Name = "ribbon1";
            // 
            // 
            // 
            this.ribbon1.OrbDropDown.BorderRoundness = 8;
            this.ribbon1.OrbDropDown.Location = new System.Drawing.Point(0, 0);
            this.ribbon1.OrbDropDown.MenuItems.Add(this.ribbonOrbMenuOptions);
            this.ribbon1.OrbDropDown.MenuItems.Add(this.ribbonOrbMenuLogs);
            this.ribbon1.OrbDropDown.MenuItems.Add(this.ribbonOrbMenuExit);
            this.ribbon1.OrbDropDown.Name = "";
            this.ribbon1.OrbDropDown.Size = new System.Drawing.Size(527, 204);
            this.ribbon1.OrbDropDown.TabIndex = 0;
            this.ribbon1.OrbImage = global::FreeDownloaderDotNet.Properties.Resources.file_menu_16;
            this.ribbon1.OrbText = "";
            // 
            // 
            // 
            this.ribbon1.QuickAccessToolbar.Visible = false;
            this.ribbon1.RibbonTabFont = new System.Drawing.Font("Trebuchet MS", 9F);
            this.ribbon1.Size = new System.Drawing.Size(800, 114);
            this.ribbon1.TabIndex = 0;
            this.ribbon1.Tabs.Add(this.ribbonTab1);
            this.ribbon1.Text = "ribbon1";
            this.ribbon1.ThemeColor = System.Windows.Forms.RibbonTheme.Green;
            // 
            // ribbonOrbMenuOptions
            // 
            this.ribbonOrbMenuOptions.DropDownArrowDirection = System.Windows.Forms.RibbonArrowDirection.Left;
            this.ribbonOrbMenuOptions.Image = ((System.Drawing.Image)(resources.GetObject("ribbonOrbMenuOptions.Image")));
            this.ribbonOrbMenuOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("ribbonOrbMenuOptions.LargeImage")));
            this.ribbonOrbMenuOptions.Name = "ribbonOrbMenuOptions";
            this.ribbonOrbMenuOptions.SmallImage = ((System.Drawing.Image)(resources.GetObject("ribbonOrbMenuOptions.SmallImage")));
            this.ribbonOrbMenuOptions.Text = "Options";
            this.ribbonOrbMenuOptions.Click += new System.EventHandler(this.ribbonOrbMenuOptions_Click);
            // 
            // ribbonOrbMenuLogs
            // 
            this.ribbonOrbMenuLogs.DropDownArrowDirection = System.Windows.Forms.RibbonArrowDirection.Left;
            this.ribbonOrbMenuLogs.Image = ((System.Drawing.Image)(resources.GetObject("ribbonOrbMenuLogs.Image")));
            this.ribbonOrbMenuLogs.LargeImage = ((System.Drawing.Image)(resources.GetObject("ribbonOrbMenuLogs.LargeImage")));
            this.ribbonOrbMenuLogs.Name = "ribbonOrbMenuLogs";
            this.ribbonOrbMenuLogs.SmallImage = ((System.Drawing.Image)(resources.GetObject("ribbonOrbMenuLogs.SmallImage")));
            this.ribbonOrbMenuLogs.Text = "Logs";
            this.ribbonOrbMenuLogs.Click += new System.EventHandler(this.ribbonOrbMenuLogs_Click);
            // 
            // ribbonOrbMenuExit
            // 
            this.ribbonOrbMenuExit.DropDownArrowDirection = System.Windows.Forms.RibbonArrowDirection.Left;
            this.ribbonOrbMenuExit.Image = ((System.Drawing.Image)(resources.GetObject("ribbonOrbMenuExit.Image")));
            this.ribbonOrbMenuExit.LargeImage = ((System.Drawing.Image)(resources.GetObject("ribbonOrbMenuExit.LargeImage")));
            this.ribbonOrbMenuExit.Name = "ribbonOrbMenuExit";
            this.ribbonOrbMenuExit.SmallImage = ((System.Drawing.Image)(resources.GetObject("ribbonOrbMenuExit.SmallImage")));
            this.ribbonOrbMenuExit.Text = "Exit";
            this.ribbonOrbMenuExit.Click += new System.EventHandler(this.ribbonOrbMenuExit_Click);
            // 
            // ribbonTab1
            // 
            this.ribbonTab1.Name = "ribbonTab1";
            this.ribbonTab1.Panels.Add(this.ribbonPanel1);
            this.ribbonTab1.Panels.Add(this.ribbonPanel2);
            this.ribbonTab1.Panels.Add(this.ribbonPanel3);
            this.ribbonTab1.Panels.Add(this.ribbonPanel4);
            this.ribbonTab1.Text = "Home";
            // 
            // ribbonPanel1
            // 
            this.ribbonPanel1.ButtonMoreVisible = false;
            this.ribbonPanel1.Items.Add(this.rbtnAddUrl);
            this.ribbonPanel1.Name = "ribbonPanel1";
            this.ribbonPanel1.Text = "Add Url";
            // 
            // rbtnAddUrl
            // 
            this.rbtnAddUrl.Image = global::FreeDownloaderDotNet.Properties.Resources.add_url_28;
            this.rbtnAddUrl.LargeImage = global::FreeDownloaderDotNet.Properties.Resources.add_url_28;
            this.rbtnAddUrl.Name = "rbtnAddUrl";
            this.rbtnAddUrl.SmallImage = ((System.Drawing.Image)(resources.GetObject("rbtnAddUrl.SmallImage")));
            this.rbtnAddUrl.Text = "";
            this.rbtnAddUrl.TextAlignment = System.Windows.Forms.RibbonItem.RibbonItemTextAlignment.Center;
            this.rbtnAddUrl.Click += new System.EventHandler(this.rbtnAddUrl_Click);
            // 
            // rbtnStartDownload
            // 
            this.rbtnStartDownload.Image = global::FreeDownloaderDotNet.Properties.Resources.play_button_28;
            this.rbtnStartDownload.LargeImage = global::FreeDownloaderDotNet.Properties.Resources.play_button_28;
            this.rbtnStartDownload.Name = "rbtnStartDownload";
            this.rbtnStartDownload.SmallImage = ((System.Drawing.Image)(resources.GetObject("rbtnStartDownload.SmallImage")));
            this.rbtnStartDownload.Text = "";
            this.rbtnStartDownload.Click += new System.EventHandler(this.rbtnStartDownload_Click);
            // 
            // rbtnStopDownload
            // 
            this.rbtnStopDownload.Image = global::FreeDownloaderDotNet.Properties.Resources.stop_button_28;
            this.rbtnStopDownload.LargeImage = global::FreeDownloaderDotNet.Properties.Resources.stop_button_28;
            this.rbtnStopDownload.Name = "rbtnStopDownload";
            this.rbtnStopDownload.SmallImage = ((System.Drawing.Image)(resources.GetObject("rbtnStopDownload.SmallImage")));
            this.rbtnStopDownload.Text = "";
            this.rbtnStopDownload.Click += new System.EventHandler(this.rbtnStopDownload_Click);
            // 
            // rbtnDeleteDownload
            // 
            this.rbtnDeleteDownload.Image = global::FreeDownloaderDotNet.Properties.Resources.delete_btn2_28;
            this.rbtnDeleteDownload.LargeImage = global::FreeDownloaderDotNet.Properties.Resources.delete_btn2_28;
            this.rbtnDeleteDownload.Name = "rbtnDeleteDownload";
            this.rbtnDeleteDownload.SmallImage = ((System.Drawing.Image)(resources.GetObject("rbtnDeleteDownload.SmallImage")));
            this.rbtnDeleteDownload.Click += new System.EventHandler(this.rbtnDeleteDownload_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.kryptonDataGridView1);
            this.Controls.Add(this.treeViewLeftPanel);
            this.Controls.Add(this.ribbon1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Free Downloader Dot Net";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.MainForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonDataGridView1)).EndInit();
            this.contextMenuGridView.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Ribbon ribbon1;
        private System.Windows.Forms.RibbonTab ribbonTab1;
        private System.Windows.Forms.RibbonPanel ribbonPanel1;
        private System.Windows.Forms.TreeView treeViewLeftPanel;
        private ComponentFactory.Krypton.Toolkit.KryptonDataGridView kryptonDataGridView1;
        private System.Windows.Forms.RibbonButton rbtnAddUrl;
        private System.Windows.Forms.RibbonPanel ribbonPanel2;
        private System.Windows.Forms.RibbonButton rbtnStartDownload;
        private System.Windows.Forms.RibbonPanel ribbonPanel3;
        private System.Windows.Forms.RibbonButton rbtnStopDownload;
        private System.Windows.Forms.DataGridViewImageColumn dataGridViewImageColumn1;
        private System.Windows.Forms.RibbonOrbMenuItem ribbonOrbMenuOptions;
        private System.Windows.Forms.RibbonOrbMenuItem ribbonOrbMenuLogs;
        private System.Windows.Forms.RibbonOrbMenuItem ribbonOrbMenuExit;
        private System.Windows.Forms.DataGridViewImageColumn colState;
        private System.Windows.Forms.DataGridViewTextBoxColumn State;
        private System.Windows.Forms.DataGridViewTextBoxColumn StartDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn Id;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFile;
        private System.Windows.Forms.DataGridViewTextBoxColumn colUrl;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPercentage;
        private System.Windows.Forms.ContextMenuStrip contextMenuGridView;
        private System.Windows.Forms.ToolStripMenuItem startToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem stopToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.RibbonPanel ribbonPanel4;
        private System.Windows.Forms.RibbonButton rbtnDeleteDownload;
        private System.Windows.Forms.ToolStripMenuItem copyUrlToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showInExplorerToolStripMenuItem;
    }
}

