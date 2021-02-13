using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ComponentFactory.Krypton.Toolkit;
using FreeDownloaderDotNet.Data;
using FreeDownloaderDotNet.Helpers;
using FreeDownloaderDotNet.Models;
using FreeDownloaderDotNet.Models.Enums;
using Microsoft.VisualBasic;

namespace FreeDownloaderDotNet
{
    public partial class MainForm : Form
    {
        #region Private Fields
        private readonly ApplicationDbContext _context;
        private List<GridItem> _gridSourceAll = new List<GridItem>();
        private List<GridItem> _gridSourceVideos = new List<GridItem>();
        private List<GridItem> _gridSourceMusic = new List<GridItem>();
        private List<GridItem> _gridSourceOther = new List<GridItem>();
        private List<GridItem> _gridSourceCompleted = new List<GridItem>();
        private BindingList<GridItem> _bindingListAll = null;
        private BindingList<GridItem> _bindingListVideos = null;
        private BindingList<GridItem> _bindingListMusic = null;
        private BindingList<GridItem> _bindingListOther = null;
        private BindingList<GridItem> _bindingListCompleted = null;
        private BindingSource _bindingSource = null;
        private readonly GetFileTypeFromName _getFileTypeFromName;
        private readonly OptionsForm _optionsForm;
        private readonly LogsForm _logsForm;
        private Dictionary<int, BackgroundWorker> _backgroundWorkers3 = new Dictionary<int, BackgroundWorker>();
        private const bool _logErrorsSetting = false;
        private string _downloadFolder = "";
        #endregion

        #region Constructor
        public MainForm(ApplicationDbContext context, GetFileTypeFromName getFileTypeFromName, OptionsForm optionsForm, LogsForm logsForm)
        {
            InitializeComponent();
            _context = context;
            _getFileTypeFromName = getFileTypeFromName;
            _optionsForm = optionsForm;
            _logsForm = logsForm;
            bool saveInitial = false;
            Setting downloadSetting = GetDownloadSetting();
            if (downloadSetting == null)
            {
                _downloadFolder = KnownFolders.GetPath(KnownFolder.Downloads);
                _context.Settings.Add(new Models.Setting() { Name = "DownloadFolder", Value = _downloadFolder });
                saveInitial = true;
                _context.SaveChanges();
            }
            else
            {
                _downloadFolder = downloadSetting.Value;
            }
            if (_context.Settings.SingleOrDefault(a => a.Name == "AudioFileTypes") == null)
            {
                _context.Settings.Add(new Models.Setting() { Name = "AudioFileTypes", Value = ".wav|.mp3|.wma|.flac" });
                saveInitial = true;
            }
            if (_context.Settings.SingleOrDefault(a => a.Name == "VideoFileTypes") == null)
            {
                _context.Settings.Add(new Models.Setting() { Name = "VideoFileTypes", Value = ".mp4|.avi|.wmv|.mov|.mkv|.mpeg|.flv" });
                saveInitial = true;
            }
            if (saveInitial)
            {
                _context.SaveChanges();
            }

            for (int i = 0; i < 3; i++)
            {
                BackgroundWorker backgroundWorker = new BackgroundWorker();
                InitializeNewBgWorker(backgroundWorker);
                _backgroundWorkers3.Add(i - 2, backgroundWorker);
            }

            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, errors) =>
            { return true; };
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;

            RemoveDuplicatesFromDB();
        }
        #endregion

        #region Background Worker
        private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            Download download = (Download)e.Argument;
            bool? retVal = DownloadFile(download);
            if (retVal == null)
            {
                //file is corrupted.
                download.FileIsCorrupted = true;
            }
            e.Result = download;
        }

        private void BackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            GridItem gridItem = _bindingListAll.FirstOrDefault(a => a.Id == (int)e.UserState);
            if (gridItem != null)
            {
                gridItem.Percentage = e.ProgressPercentage;
                Download download = _context.Downloads.Find(gridItem.Id);

                try
                {
                    if (download != null)
                    {
                        download.Percent = gridItem.Percentage;
                        _context.SaveChanges();
                    }
                }
                catch (Exception ex)
                {

                }
            }
            else
            {
                LogMessage("_bindingList.FirstOrDefaul returned null in ProgressChanged", true);
            }
        }

        private void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Download downloadResult = (Download)e.Result;
            if (downloadResult.DownloadCanceled)
            {
                downloadResult.DownloadCanceled = false;
                return;
            }


            if (!downloadResult.FileIsCorrupted)
            {
                ApplicationDbContext context = new ApplicationDbContext(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);

                Download downloadInDb = context.Downloads.Find(downloadResult.Id);
                if (downloadInDb != null && downloadInDb.FinishDate == null)
                {
                    downloadInDb.FinishDate = DateTime.Now;
                    downloadInDb.Percent = 100;
                    downloadInDb.State = FileState.Finished;
                    context.SaveChanges();
                }
                GridItem gridItem = _bindingListAll.FirstOrDefault(a => a.Id == downloadResult.Id);
                if (gridItem != null)
                {
                    gridItem.State = FileState.Finished;
                    gridItem.StateImg = GetGridStateImageFromState(gridItem.State);
                    gridItem.Percentage = 100;
                }
            }
            else
            {
                GridItem gridItem = _bindingListAll.FirstOrDefault(a => a.Id == downloadResult.Id);
                Download downloadInDb = _context.Downloads.Find(downloadResult.Id);
                if (downloadInDb != null)//downloadInDb is null when downloading file and clicking delete
                {
                    downloadInDb.FinishDate = DateTime.Now;
                    downloadInDb.State = FileState.Corrupted;
                    gridItem.Filename = downloadInDb.Filename = "FILE CORRUPTED, DELETE ME!. " + downloadInDb.Filename;
                    _context.SaveChanges();
                    gridItem.State = FileState.Corrupted;
                    gridItem.StateImg = GetGridStateImageFromState(gridItem.State);
                }
            }

            RenderTreeViewAndGetDownloads(false);

            //start queued downloads
            foreach (var item in _bindingListAll)
            {
                if (item.State == FileState.InQueue)
                {
                    foreach (var item2 in _backgroundWorkers3)
                    {
                        if (!item2.Value.IsBusy)
                        {
                            _backgroundWorkers3.Remove(item2.Key);

                            BackgroundWorker _bgWorker = new BackgroundWorker();
                            InitializeNewBgWorker(_bgWorker);

                            _backgroundWorkers3.Add(item.Id, _bgWorker);
                            Download downloadToStart = _context.Downloads.Find(item.Id);
                            if (downloadToStart != null)
                            {
                                item.State = FileState.Playing;
                                item.StateImg = GetGridStateImageFromState(item.State);
                                _backgroundWorkers3[item.Id].RunWorkerAsync(downloadToStart);

                                downloadToStart.State = FileState.Playing;
                                if (downloadToStart.StartDate == null)
                                    downloadToStart.StartDate = DateTime.Now;
                                _context.SaveChanges();
                            }
                            break;
                        }
                    }
                }
            }
        }
        #endregion

        #region Methods
        private bool? DownloadFile(Download download)
        {
            BackgroundWorker bg = _backgroundWorkers3[download.Id];
            string localFilePath = Path.Combine(_downloadFolder, download.Filename);

            long Content_Length = -1;

            System.Net.HttpWebRequest req = (HttpWebRequest)System.Net.WebRequest.Create(download.Url);

            string localFileName = Path.GetFileName(localFilePath);

            bool result = false;
            try
            {
                req.Method = "HEAD";
                req.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)";
                using (System.Net.WebResponse resp = req.GetResponse())
                {
                    if (long.TryParse(resp.Headers.Get("Content-Length"), out long ContentLength))
                    {
                        Content_Length = ContentLength;
                    }
                }

                FileInfo finfo = null;

                if (File.Exists(localFilePath))
                {
                    finfo = new FileInfo(localFilePath);

                    if (Content_Length != -1 && finfo.Length == Content_Length)
                    {
                        result = true;
                        //"File with name " + localFileName + " already downloaded! Skipping...");
                    }
                    else if (bg.CancellationPending)
                    {
                        download.DownloadCanceled = true;
                        result = true;
                        //"Resuming download of file named: " + localFileName + " ...");
                    }
                }



                if (result == false)
                {
                    long resContentLength = DownloadFileWithResume(download.Url, localFilePath, download.Id, ref bg);
                    if (resContentLength == -1000)
                    {
                        //file is corrupted.
                        return null;
                    }
                }

                finfo = finfo ?? new FileInfo(localFilePath);

                if (result == false)
                {
                    if (finfo.Length == Content_Length)
                    {
                        result = true;
                        LogMessage("File named " + localFileName + " successfully downloaded!", false);
                    }
                    else
                    {
                        result = false;
                        LogMessage("Downloading file " + localFileName + " interrupted Retrying download...", true);
                        DownloadFile(download);
                    }
                }
            }
            catch (System.Net.WebException webEx)
            {
                if (webEx.Message.ToString() == "The remote server returned an error: (416) Requested Range Not Satisfiable.")
                {
                    LogMessage(download.Url + " The remote server returned an error: (416) Requested Range Not Satisfiable.", true);
                    result = true;
                }
            }
            catch (Exception ex)
            {
                LogMessage("Error: " + ex.Message + " Retrying download file " + localFileName, true);
                DownloadFile(download);
            }

            return result;
        }

        private long DownloadFileWithResume(string sourceUrl, string destinationPath, int id, ref BackgroundWorker backgroundWorker)
        {
            long existLen = 0;
            int progress = 0;
            long contentLength = 0;
            System.IO.FileStream saveFileStream;
            if (System.IO.File.Exists(destinationPath))
            {
                System.IO.FileInfo fINfo =
                    new System.IO.FileInfo(destinationPath);
                existLen = fINfo.Length;
            }

            bool append = false;
            if (existLen > 0)
            {
                saveFileStream = new System.IO.FileStream(destinationPath,
                                                          System.IO.FileMode.Append, System.IO.FileAccess.Write,
                                                          System.IO.FileShare.ReadWrite);
                append = true;
            }
            else
            {
                saveFileStream = new System.IO.FileStream(destinationPath,
                                                          System.IO.FileMode.Create, System.IO.FileAccess.Write,
                                                          System.IO.FileShare.ReadWrite);
                append = false;
            }

            //get correct content length for progress percentage on append
            long lenFull = -1;
            if (append)
            {

                var requestTmp = (HttpWebRequest)HttpWebRequest.Create(sourceUrl);
                requestTmp.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)";
                using (var responseTmp = (HttpWebResponse)requestTmp.GetResponse())
                {
                    lenFull = responseTmp.ContentLength;
                }
            }

            var request = (HttpWebRequest)HttpWebRequest.Create(sourceUrl);
            request.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)";
            request.AddRange((int)existLen);
            using (var response = (HttpWebResponse)request.GetResponse())
            {
                contentLength = response.ContentLength;
                byte[] buffer = new byte[1024]; 
                using (var stream = response.GetResponseStream())
                {
                    int read;
                    while ((read = stream.Read(buffer, 0, buffer.Length)) != 0)
                    {
                        if (backgroundWorker.CancellationPending)
                        {
                            break;
                        }
                        saveFileStream.Write(buffer, 0, read);
                        if (progress != (int)(saveFileStream.Length * 100 / (!append ? contentLength : lenFull)))
                        {
                            progress = (int)(saveFileStream.Length * 100 / (!append ? contentLength : lenFull));

                            backgroundWorker.ReportProgress(progress, id);
                        }
                        if (saveFileStream.Length == contentLength)
                            break;
                    }

                    if (lenFull != -1 && !backgroundWorker.CancellationPending && saveFileStream.Length != lenFull)
                    {
                        LogMessage(Path.GetFileName(destinationPath) + " File content is corrupted.", true);
                        return -1000;
                    }

                    saveFileStream.Flush();
                    saveFileStream.Close();
                }
            }

            return contentLength;
        }

        private void StartDownloadMenuClicked()
        {
            GridItem selectedItem = GetSelectedItemInGrid();
            if (selectedItem == null)
                return;

            int idToRemove = -100;
            BackgroundWorker bgToRun = null;
            foreach (var bgKey in _backgroundWorkers3.Keys)
            {
                if (!_backgroundWorkers3[bgKey].IsBusy)
                {
                    idToRemove = bgKey;
                    bgToRun = _backgroundWorkers3[bgKey];
                    break;
                }
            }

            if (bgToRun != null)
            {
                selectedItem.State = Models.Enums.FileState.Playing;
                selectedItem.StateImg = GetGridStateImageFromState(selectedItem.State);
                _backgroundWorkers3.Remove(idToRemove);
                if (!_backgroundWorkers3.ContainsKey(selectedItem.Id))
                {
                    _backgroundWorkers3.Add(selectedItem.Id, bgToRun);
                    bgToRun.RunWorkerAsync(_context.Downloads.Find(selectedItem.Id));
                    ChangeGridItemStateInDb(selectedItem);
                }
                else
                {
                    BackgroundWorker bgToRunExisting = _backgroundWorkers3[selectedItem.Id];
                    if (!bgToRunExisting.IsBusy)
                    {
                        bgToRunExisting.RunWorkerAsync(_context.Downloads.Find(selectedItem.Id));
                        ChangeGridItemStateInDb(selectedItem);
                    }
                }
            }
            else
            {
                selectedItem.State = Models.Enums.FileState.InQueue;
                selectedItem.StateImg = GetGridStateImageFromState(selectedItem.State);
            }
        }

        private void StopDownloadMenuClicked()
        {
            GridItem selectedItem = GetSelectedItemInGrid();
            if (selectedItem == null)
                return;
            BackgroundWorker selectedbgWorker = _backgroundWorkers3[selectedItem.Id];
            if (selectedbgWorker != null)
            {
                selectedItem.State = FileState.Stopped;
                selectedItem.StateImg = GetGridStateImageFromState(selectedItem.State);
                selectedbgWorker.CancelAsync();
                ChangeGridItemStateInDb(selectedItem);
            }
        }
        #endregion

        #region Event Handlers
        private void MainForm_Load(object sender, EventArgs e)
        {
            RenderTreeViewAndGetDownloads(true);
            BindGrid();

            Dictionary<GridItem, BackgroundWorker> bgListToRun = new Dictionary<GridItem, BackgroundWorker>();

            foreach (var item in _bindingListAll)
            {
                if (item.State != FileState.Playing)
                    continue;

                int idToRemove = -100;
                BackgroundWorker bgToRun = null;
                foreach (var bgKey in _backgroundWorkers3.Keys)
                {
                    if (!_backgroundWorkers3[bgKey].IsBusy)
                    {
                        idToRemove = bgKey;
                        bgToRun = _backgroundWorkers3[bgKey];
                        break;
                    }
                }

                if (bgToRun != null)
                {
                    item.State = Models.Enums.FileState.Playing;
                    _backgroundWorkers3.Remove(idToRemove);
                    _backgroundWorkers3.Add(item.Id, bgToRun);
                    bgListToRun.Add(item, bgToRun);
                }
                else
                {
                    item.State = Models.Enums.FileState.InQueue;
                    item.StateImg = GetGridStateImageFromState(item.State);
                }
            }

            foreach (var item in _bindingListAll)
            {
                if (item.State != FileState.InQueue)
                    continue;

                int idToRemove = -100;
                BackgroundWorker bgToRun = null;
                foreach (var bgKey in _backgroundWorkers3.Keys)
                {
                    if (!_backgroundWorkers3[bgKey].IsBusy)
                    {
                        idToRemove = bgKey;
                        bgToRun = _backgroundWorkers3[bgKey];
                        break;
                    }
                }

                if (bgToRun != null)
                {
                    item.State = Models.Enums.FileState.Playing;
                    item.StateImg = GetGridStateImageFromState(item.State);
                    _backgroundWorkers3.Remove(idToRemove);
                    _backgroundWorkers3.Add(item.Id, bgToRun);
                    bgListToRun.Add(item, bgToRun);
                    Download downloadInDb = _context.Downloads.SingleOrDefault(a => a.Id == item.Id);
                    downloadInDb.State = FileState.Playing;
                    if (downloadInDb.StartDate == null)
                        downloadInDb.StartDate = DateTime.Now;
                    _context.SaveChanges();
                }
                else
                {
                    item.State = Models.Enums.FileState.InQueue;
                    item.StateImg = GetGridStateImageFromState(item.State);
                    Download downloadInDb = _context.Downloads.SingleOrDefault(a => a.Id == item.Id);
                    downloadInDb.State = FileState.InQueue;
                    _context.SaveChanges();
                }
            }

            foreach (var item in bgListToRun)
            {
                if (_backgroundWorkers3.ContainsKey(item.Key.Id))
                    _backgroundWorkers3[item.Key.Id].RunWorkerAsync(_context.Downloads.Find(item.Key.Id));
            }
        }

        private void RenderTreeViewAndGetDownloads(bool getDownloads)
        {
            int countStartedQueuedStopped = 0;
            int countCompleted = 0;
            int countVideos = 0;
            int countMusic = 0;
            int countOther = 0;
            string allDownloads = "All Downloads";
            string videos = "Videos";
            string music = "Music";
            string other = "Other";
            string completed = "Completed";

            try
            {
                foreach (var item in _context.Downloads)
                {
                    GridItem gridItem = new GridItem();
                    gridItem.Filename = item.Filename;
                    gridItem.Id = item.Id;
                    gridItem.State = item.State;
                    gridItem.StateImg = GetGridStateImageFromState(item.State);
                    gridItem.Percentage = item.Percent.HasValue ? item.Percent.Value : 0;
                    gridItem.Url = item.Url;
                    gridItem.StartDate = item.StartDate;
                    if (item.State == FileState.Playing || item.State == FileState.InQueue || item.State == FileState.Stopped)
                        countStartedQueuedStopped++;
                    else
                        countCompleted++;

                    if (item.Type == FileType.Video)
                        countVideos++;
                    else if (item.Type == FileType.Music)
                        countMusic++;
                    else
                        countOther++;

                    allDownloads += " (" + countStartedQueuedStopped + "/" + countCompleted + ")";
                    videos += " (" + countVideos + ")";
                    music += " (" + countMusic + ")";
                    other += " (" + countOther + ")";
                    completed += " (" + countCompleted + ")";

                    treeViewLeftPanel.Nodes["trNodeAllDownloads"].Text = allDownloads;
                    treeViewLeftPanel.Nodes["trNodeVideos"].Text = videos;
                    treeViewLeftPanel.Nodes["trNodeMusic"].Text = music;
                    treeViewLeftPanel.Nodes["trNodeOther"].Text = other;
                    treeViewLeftPanel.Nodes["trNodeCompleted"].Text = completed;

                    if (getDownloads)
                    {
                        AddGridItemToSource(gridItem, item);
                    }
                }
            }
            catch (Exception ex)
            {
                LogMessage(ex.Message, true);
            }

            if (_context.Downloads.Count() == 0)
            {
                treeViewLeftPanel.Nodes["trNodeAllDownloads"].Text = allDownloads + " (0/0)";
                treeViewLeftPanel.Nodes["trNodeVideos"].Text = videos + " (0/0)";
                treeViewLeftPanel.Nodes["trNodeMusic"].Text = music + " (0/0)";
                treeViewLeftPanel.Nodes["trNodeOther"].Text = other + " (0/0)";
                treeViewLeftPanel.Nodes["trNodeCompleted"].Text = completed + " (0/0)";
            }
        }

        private void AddGridItemToSource(GridItem gridItem, Download item)
        {
            _gridSourceAll.Add(gridItem);
            if (item.Type == FileType.Video)
                _gridSourceVideos.Add(gridItem);
            else if (item.Type == FileType.Music)
                _gridSourceMusic.Add(gridItem);
            else if (item.Type == FileType.Other)
                _gridSourceOther.Add(gridItem);
            if (item.FinishDate != null)
                _gridSourceCompleted.Add(gridItem);
        }

        private void BindGrid()
        {
            _bindingListAll = new BindingList<GridItem>(_gridSourceAll);
            _bindingListVideos = new BindingList<GridItem>(_gridSourceVideos);
            _bindingListMusic = new BindingList<GridItem>(_gridSourceMusic);
            _bindingListOther = new BindingList<GridItem>(_gridSourceOther);
            _bindingListCompleted = new BindingList<GridItem>(_gridSourceCompleted);
            _bindingSource = new BindingSource(_bindingListAll, null);

            kryptonDataGridView1.DataSource = _bindingSource;
        }

        private void menuBtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void menuBtnMinimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void rbtnAddUrl_Click(object sender, EventArgs e)
        {
            int x = this.Left + (this.Width / 2) - 150;
            int y = this.Top + (this.Height / 2) - 100;
            string inputUrl = Interaction.InputBox("Add url to start downloading file:", "Add url", "", x, y);
            if (!string.IsNullOrEmpty(inputUrl))
            {
                string fileName = Path.GetFileName(inputUrl);
                string path = Path.Combine(_downloadFolder, fileName);
                string url = inputUrl;

                //check if download already exists
                Download download = _context.Downloads.FirstOrDefault(a => a.Path == path && a.Filename == fileName && a.Url == url);

                if (download == null)
                {
                    download = new Download();
                    download.Filename = fileName;
                    download.Path = path;
                    download.Type = _getFileTypeFromName.GetFileType(download.Filename);
                    download.Url = url;

                    int idToRemove = -100;
                    BackgroundWorker bgToRun = null;
                    foreach (var item in _backgroundWorkers3.Keys)
                    {
                        if (!_backgroundWorkers3[item].IsBusy)
                        {
                            idToRemove = item;
                            bgToRun = _backgroundWorkers3[item];
                            break;
                        }
                    }


                    GridItem gridItem = new GridItem();
                    gridItem.Filename = download.Filename;
                    gridItem.Url = download.Url;
                    gridItem.StateImg = null;
                    gridItem.Percentage = 0;


                    if (bgToRun != null)
                    {
                        download.State = Models.Enums.FileState.Playing;
                        download.StartDate = DateTime.Now;
                        gridItem.State = download.State;
                        gridItem.StateImg = GetGridStateImageFromState(gridItem.State);
                        _context.Downloads.Add(download);
                        try
                        {
                            _context.SaveChanges();
                        }
                        catch (Exception ex)
                        {
                            
                        }
                        gridItem.Id = download.Id;
                        _bindingListAll.Add(gridItem);
                        _backgroundWorkers3.Remove(idToRemove);
                        _backgroundWorkers3.Add(download.Id, bgToRun);
                        bgToRun.RunWorkerAsync(download);
                    }
                    else
                    {
                        download.State = Models.Enums.FileState.InQueue;
                        gridItem.State = FileState.InQueue;
                        gridItem.StateImg = GetGridStateImageFromState(download.State);
                        _context.Downloads.Add(download);
                        _context.SaveChanges();
                        gridItem.Id = download.Id;
                        _bindingListAll.Add(gridItem);
                    }
                }
                else
                {
                    MessageBox.Show("This file is already in downloads list", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            RenderTreeViewAndGetDownloads(false);
        }

        private void ribbonOrbMenuOptions_Click(object sender, EventArgs e)
        {
            _optionsForm.ShowDialog();
            _downloadFolder = GetDownloadSetting().Value;
        }

        private void ribbonOrbMenuLogs_Click(object sender, EventArgs e)
        {
            _logsForm.ShowDialog();
        }

        private void ribbonOrbMenuExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void rbtnStartDownload_Click(object sender, EventArgs e)
        {
            StartDownloadMenuClicked();
        }

        private void rbtnStopDownload_Click(object sender, EventArgs e)
        {
            StopDownloadMenuClicked();
        }

        private void rbtnDeleteDownload_Click(object sender, EventArgs e)
        {
            DeleteDownloadMenuClicked();
        }

        private void DeleteDownloadMenuClicked()
        {
            GridItem selectedItem = GetSelectedItemInGrid();
            if (selectedItem == null)
                return;

            BackgroundWorker selectedbgWorker = null;

            _backgroundWorkers3.TryGetValue(selectedItem.Id, out selectedbgWorker);
            if (selectedbgWorker != null)
            {
                selectedbgWorker.CancelAsync();

                _backgroundWorkers3[selectedItem.Id] = new BackgroundWorker();
                InitializeNewBgWorker(_backgroundWorkers3[selectedItem.Id]);
            }

            Download downloadToDelete = _context.Downloads.Find(selectedItem.Id);
            if (downloadToDelete != null)
            {
                _context.Downloads.Remove(downloadToDelete);
                try
                {
                    _context.SaveChanges();
                }
                catch (Exception ex)
                {

                }
            }

            RenderTreeViewAndGetDownloads(false);

            if (kryptonDataGridView1.SelectedRows[0] != null)
            {
                kryptonDataGridView1.Rows.Remove(kryptonDataGridView1.SelectedRows[0]);
            }
        }

        private void startToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StartDownloadMenuClicked();
        }

        private void stopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StopDownloadMenuClicked();
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DeleteDownloadMenuClicked();
        }

        private void copyUrlToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GridItem selectedItem = GetSelectedItemInGrid();
            if (selectedItem == null)
                return;

            System.Windows.Forms.Clipboard.SetText(selectedItem.Url);
        }

        private void showInExplorerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GridItem selectedItem = GetSelectedItemInGrid();
            if (selectedItem == null)
                return;


            try
            {
                string argument = @"/select, " + Path.Combine(_downloadFolder, selectedItem.Filename);
                System.Diagnostics.Process.Start("explorer.exe", argument);
                return;
            }
            catch (Exception ex)
            {

            }
        }


        #endregion

        #region Helper Methods
        private Setting GetDownloadSetting()
        {
            return _context.Settings.SingleOrDefault(a => a.Name == "DownloadFolder");
        }
     
        private void LogMessage(string message, bool isError)
        {
            if (!_logErrorsSetting)
                return;
            Log log = new Log();
            log.Message = message;
            log.IsError = isError;
            log.CreatedDate = DateTime.Now;

            _context.Logs.Add(log);
            try
            {
                _context.SaveChanges();
            }
            catch (Exception ex)
            {

            }
        }

        private void RemoveDuplicatesFromDB()
        {
            var duplicateIds = new List<int>();
            var checkedIds = new List<int>();
            foreach (var item1 in _context.Downloads)
            {
                checkedIds.Add(item1.Id);
                foreach (var item2 in _context.Downloads)
                {
                    if (item1.Id != item2.Id && !checkedIds.Contains(item2.Id)
                        && item1.Filename == item2.Filename && item1.Url == item2.Url && item1.Path == item2.Path && item1.FinishDate != null && item2.FinishDate != null
                        && !duplicateIds.Contains(item2.Id))
                    {
                        duplicateIds.Add(item2.Id);
                    }
                }
            }

            foreach (var item in duplicateIds)
            {
                _context.Downloads.Remove(_context.Downloads.Find(item));
            }

            _context.SaveChanges();


            duplicateIds = new List<int>();
            checkedIds = new List<int>();

            foreach (var item1 in _context.Downloads)
            {
                checkedIds.Add(item1.Id);
                foreach (var item2 in _context.Downloads)
                {
                    if (item1.Id != item2.Id && !checkedIds.Contains(item2.Id)
                        && item1.Filename == item2.Filename && item1.Url == item2.Url && item1.Path == item2.Path && item1.FinishDate == null && item2.FinishDate == null
                        && !duplicateIds.Contains(item2.Id))
                    {
                        duplicateIds.Add(item2.Id);
                    }
                }
            }
            foreach (var item in duplicateIds)
            {
                _context.Downloads.Remove(_context.Downloads.Find(item));
            }

            _context.SaveChanges();
        }

        private Image GetGridStateImageFromState(FileState state)
        {
            switch (state)
            {
                case FileState.Finished:
                    return Properties.Resources.grid_completed;
                case FileState.InQueue:
                    return Properties.Resources.grid_queued;
                case FileState.Playing:
                    return Properties.Resources.grid_playing;
                case FileState.Stopped:
                    return Properties.Resources.grid_stopped;
                case FileState.Corrupted:
                    return Properties.Resources.grid_corrupted;
                default:
                    return null;
            }
        }

        private void ChangeGridItemStateInDb(GridItem gridItem)
        {
            Download downloadToUpdateState = _context.Downloads.Find(gridItem.Id);
            if (downloadToUpdateState == null)
                ShowMessageBoxError("gridItem can't be found in db in ChangeGridItemStateInDb method");
            else
            {
                downloadToUpdateState.State = gridItem.State;
                _context.SaveChanges();
            }
        }
        private void treeViewLeftPanel_AfterSelect(object sender, TreeViewEventArgs e)
        {

            switch ((TreeViewType)Enum.Parse(typeof(TreeViewType), e.Node.Tag.ToString()))
            {
                case TreeViewType.All:
                    _bindingSource = new BindingSource(_bindingListAll, null);
                    break;
                case TreeViewType.Videos:
                    _bindingSource = new BindingSource(_bindingListVideos, null);
                    break;
                case TreeViewType.Music:
                    _bindingSource = new BindingSource(_bindingListMusic, null);
                    break;
                case TreeViewType.Other:
                    _bindingSource = new BindingSource(_bindingListOther, null);
                    break;
                case TreeViewType.Completed:
                    _bindingSource = new BindingSource(_bindingListCompleted, null);
                    break;
                default:
                    {
                        string error = "Invalid tree tag";
                        LogMessage(error, true);
                        throw new Exception(error);
                    }
            }
            kryptonDataGridView1.DataSource = _bindingSource;
        }

        private void ShowMessageBoxError(string errorMessage)
        {
            LogMessage(errorMessage, true);
            MessageBox.Show(errorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private GridItem GetSelectedItemInGrid()
        {
            GridItem selectedItem = null;
            if (kryptonDataGridView1.SelectedRows.Count > 0)
            {

                selectedItem = (GridItem)kryptonDataGridView1.SelectedRows[0].DataBoundItem;

                if (selectedItem == null)
                {
                    ShowMessageBoxError("Invalid selected item");
                }
            }

            return selectedItem;
        }

        private void contextMenuGridView_Opened(object sender, EventArgs e)
        {
            startToolStripMenuItem.Enabled = stopToolStripMenuItem.Enabled = deleteToolStripMenuItem.Enabled = copyUrlToolStripMenuItem.Enabled = showInExplorerToolStripMenuItem.Enabled = true;

            GridItem selectedItem = GetSelectedItemInGrid();
            if (selectedItem == null || selectedItem.State == FileState.Finished)
            {
                startToolStripMenuItem.Enabled = false;
                stopToolStripMenuItem.Enabled = false;
            }
            else if (selectedItem != null && (selectedItem.State == FileState.InQueue || selectedItem.State == FileState.Stopped))
            {
                stopToolStripMenuItem.Enabled = false;
            }
            else if (selectedItem != null && selectedItem.State == FileState.Playing)
            {
                startToolStripMenuItem.Enabled = false;
            }
            if (selectedItem == null)
            {
                deleteToolStripMenuItem.Enabled = copyUrlToolStripMenuItem.Enabled = showInExplorerToolStripMenuItem.Enabled = false;
            }
        }

        private void kryptonDataGridView1_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                int rowSelected = e.RowIndex;
                if (e.RowIndex != -1)
                {
                    kryptonDataGridView1.ClearSelection();
                    kryptonDataGridView1.Rows[rowSelected].Selected = true;
                }
            }
        }

        private void InitializeNewBgWorker(BackgroundWorker bgWorker)
        {
            bgWorker.WorkerReportsProgress = true;
            bgWorker.WorkerSupportsCancellation = true;
            bgWorker.DoWork += BackgroundWorker_DoWork;
            bgWorker.ProgressChanged += BackgroundWorker_ProgressChanged;
            bgWorker.RunWorkerCompleted += BackgroundWorker_RunWorkerCompleted;
        }
        #endregion
    }
}
