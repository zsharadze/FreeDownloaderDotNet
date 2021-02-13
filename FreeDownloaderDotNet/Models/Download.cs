using FreeDownloaderDotNet.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeDownloaderDotNet.Models
{
    [Table("Downloads")]
    public class Download
    {
        [Key]
        public int Id { get; set; }
        public string Filename { get; set; }
        public string Url { get; set; }
        public string Path { get; set; }
        public FileType Type { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? FinishDate { get; set; }
        public int? Percent { get; set; }
        public FileState State { get; set; }
        [NotMapped]
        public bool FileIsCorrupted { get; set; }
        [NotMapped]
        public bool DownloadCanceled { get; set; }
    }
}
