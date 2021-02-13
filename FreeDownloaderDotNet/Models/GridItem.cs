using FreeDownloaderDotNet.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeDownloaderDotNet
{
    public class GridItem : INotifyPropertyChanged
    {
        public int Id { get; set; }
        
        private string _filename;
        public string Filename
        {
            get { return _filename; }
            set { _filename = value; NotifyPropertyChanged("StaFilenamete"); }
        }
        public string Url { get; set; }
        private FileState _state;

        public FileState State
        {
            get { return _state; }
            set { _state = value; NotifyPropertyChanged("State"); }
        }

        public DateTime? StartDate { get; set; }
        private int _percentage;

        public event PropertyChangedEventHandler PropertyChanged;

        private Image _stateImg;

        public Image StateImg
        {
            get { return _stateImg; }
            set { _stateImg = value; NotifyPropertyChanged("StateImg"); }
        }

        public int Percentage
        {
            get { return _percentage; }
            set { _percentage = value; NotifyPropertyChanged("Percentage"); }
        }

        private void NotifyPropertyChanged(string p)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(p));
        }
    }
}
