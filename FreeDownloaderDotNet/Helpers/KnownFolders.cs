using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace FreeDownloaderDotNet.Helpers
{
    public static class KnownFolders
    {
        private static string[] _knownFolderGuids = new string[]
        {
        "{374DE290-123F-4565-9164-39C4925E467B}", // Downloads
        };

        public static string GetPath(KnownFolder knownFolder)
        {
            return GetPath(knownFolder, false);
        }

        public static string GetPath(KnownFolder knownFolder, bool defaultUser)
        {
            return GetPath(knownFolder, KnownFolderFlags.DontVerify, defaultUser);
        }

        private static string GetPath(KnownFolder knownFolder, KnownFolderFlags flags,
            bool defaultUser)
        {
            int result = SHGetKnownFolderPath(new Guid(_knownFolderGuids[(int)knownFolder]),
                (uint)flags, new IntPtr(defaultUser ? -1 : 0), out IntPtr outPath);
            if (result >= 0)
            {
                string path = Marshal.PtrToStringUni(outPath);
                Marshal.FreeCoTaskMem(outPath);
                return path;
            }
            else
            {
                throw new ExternalException("Unable to retrieve the known folder path. It may not "
                    + "be available on this system.", result);
            }
        }

        [DllImport("Shell32.dll")]
        private static extern int SHGetKnownFolderPath(
            [MarshalAs(UnmanagedType.LPStruct)] Guid rfid, uint dwFlags, IntPtr hToken,
            out IntPtr ppszPath);

        [Flags]
        private enum KnownFolderFlags : uint
        {
            SimpleIDList = 0x00000100,
            NotParentRelative = 0x00000200,
            DefaultPath = 0x00000400,
            Init = 0x00000800,
            NoAlias = 0x00001000,
            DontUnexpand = 0x00002000,
            DontVerify = 0x00004000,
            Create = 0x00008000,
            NoAppcontainerRedirection = 0x00010000,
            AliasOnly = 0x80000000
        }
    }

    public enum KnownFolder
    {
        Downloads
    }
}
