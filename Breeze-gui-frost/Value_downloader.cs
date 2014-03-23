using System;
using System.Collections.Generic;
namespace Breeze_gui_frost
{
    static internal class Value_downloader
    {
        public static readonly List<string> VideoMimelist = new List<string> { "|[A-F0-9]{8}|", "video/mp4", "video/mp2t" };
        public static string DefaultPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
    }
}
