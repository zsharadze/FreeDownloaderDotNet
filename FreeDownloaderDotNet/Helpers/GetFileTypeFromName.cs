using FreeDownloaderDotNet.Data;
using FreeDownloaderDotNet.Models.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeDownloaderDotNet.Helpers
{
    public class GetFileTypeFromName
    {
       private readonly ApplicationDbContext _context;

        public GetFileTypeFromName(ApplicationDbContext context)
        {
            _context = context;
        }

        public FileType GetFileType(string Filename)
        {
            List<string> AudioFileTypes = _context.Settings.SingleOrDefault(a => a.Name == "AudioFileTypes").Value.Split('|').ToList();
            List<string> VideoFileTypes = _context.Settings.SingleOrDefault(a => a.Name == "VideoFileTypes").Value.Split('|').ToList();

            if (AudioFileTypes.Contains(Path.GetExtension(Filename).ToLower()))
                return FileType.Music;
            else if (VideoFileTypes.Contains(Path.GetExtension(Filename).ToLower()))
                return FileType.Video;
            else
                return FileType.Other;
        }
    }
}
