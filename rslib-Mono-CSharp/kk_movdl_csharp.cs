using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;
public static class kk_movDlForUser
{
    public static SortedDictionary<int, string> yt(string targetUri)
    {
        CookieContainer cc = new CookieContainer();
        HttpWebRequest req = (HttpWebRequest)WebRequest.Create(targetUri);
        req.CookieContainer = cc;
        req.Timeout = 5000;
        req.GetResponse().Close();
        req = (HttpWebRequest)WebRequest.Create("http://www.youtube.com/get_video_info?video_id=" + Regex.Match(targetUri, "(?<=v=)[\\w-]+").Value);
        req.CookieContainer = cc;
        string _info = null;
        System.Net.WebResponse res = req.GetResponse();
        System.IO.StreamReader sr = new System.IO.StreamReader(res.GetResponseStream());
        _info = sr.ReadToEnd();
        sr.Close();
        res.Close();
        Hashtable info = new Hashtable();
        Dictionary<string, string> _tmp = new Dictionary<string, string>();
        SortedDictionary<int, string> fmtmap = new SortedDictionary<int, string>();

        foreach (string item in _info.Split('&'))
        {
            info.Add(item.Split('=')[0], Uri.UnescapeDataString(item.Split('=')[1]));
        }
        if (Convert.ToString(info["status"]) == "fail")
        {
            throw new UnauthorizedAccessException();
        }
        foreach (string item in Convert.ToString(info["url_encoded_fmt_stream_map"]).Split(','))
        {
            foreach (string a in item.Split('&'))
            {
                _tmp.Add(a.Split('=')[0], Uri.UnescapeDataString(a.Split('=')[1]));
            }
            fmtmap.Add(Convert.ToInt32(_tmp["itag"]), (_tmp["url"]) + "&signature=");
            _tmp.Clear();
        }

        req = (HttpWebRequest)WebRequest.Create("http://www.youtube.com/get_video_info?video_id=" + Regex.Match(targetUri, "(?<=v=)\\w+").Value + "&t=" + Convert.ToString(info["token"]));
        req.CookieContainer = cc;
        req.Timeout = 1500;
        req.GetResponse().Close();

        fmtmap[-2] = Convert.ToString(info["title"]);
        fmtmap[-1] = cc.GetCookieHeader(new Uri("http://www.youtube.com"));
        info.Clear();
        return fmtmap;
    }
}