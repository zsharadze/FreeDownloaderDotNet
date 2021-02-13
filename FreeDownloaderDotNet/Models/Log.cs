using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeDownloaderDotNet.Models
{
    [Table("Logs")]
    public class Log
    {
        [Key]
        public long Id { get; set; }
        public string Message { get; set; }
        public bool IsError { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
