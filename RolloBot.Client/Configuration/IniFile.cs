using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace RolloBot.Client.Configuration
{
    internal static class NativeMethods
    {
        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        internal static extern int WritePrivateProfileString(string Section, string Key, string Value, string FilePath);

        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        internal static extern int GetPrivateProfileString(string Section, string Key, string Default, StringBuilder RetVal, int Size, string FilePath);
    }

    public class IniFile
    {
        private readonly string filePath;
        private readonly string fileName;
        
        public IniFile(string filePath, string fileName = "Config.ini")
        {
            this.fileName = fileName;

            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }

            this.filePath = filePath + @"\" + fileName;
        }

        public string Read(string Key, string Section = null)
        {
            var RetVal = new StringBuilder(255);
            NativeMethods.GetPrivateProfileString(Section ?? fileName, Key, "", RetVal, 255, filePath);
            return RetVal.ToString();
        }

        public void Write(string Key, string Value, string Section = null)
        {
            NativeMethods.WritePrivateProfileString(Section ?? fileName, Key, Value, filePath);
        }

        public void DeleteKey(string Key, string Section = null)
        {
            Write(Key, null, Section ?? fileName);
        }

        public void DeleteSection(string Section = null)
        {
            Write(null, null, Section ?? fileName);
        }

        public bool KeyExists(string Key, string Section = null)
        {
            return Read(Key, Section).Length > 0;
        }
    }
}
