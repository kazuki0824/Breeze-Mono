using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Xml;
public static class n_getflv
{
	#region "internal"
	public enum authflagkind
	{
		fail = 0,
		Authenticated = 1,
		Authenticated_As_Premium = 3
	}
	public static int x_niconico_authflag { get; set; }

	private static Cookie setWatchTicket(string watchId, ref string thumbplaykey, ref string response, ref string p)
	{
		HttpWebRequest req = (HttpWebRequest)WebRequest.Create("http://ext.nicovideo.jp/thumb_watch/" + watchId);
		HttpWebResponse res = null;

		req.Referer = "http://www.kazukikuroda.co.cc/";
		CookieContainer ctor = new CookieContainer();
		req.CookieContainer = ctor;
		res = (HttpWebResponse)req.GetResponse();
		string responsedData = null;
		using (System.IO.StreamReader strm = new System.IO.StreamReader(res.GetResponseStream())) {
			responsedData = strm.ReadToEnd();
			p = responsedData;
		}
		//Regexオブジェクトを作成 
		System.Text.RegularExpressions.Regex r = new System.Text.RegularExpressions.Regex("(?<=\\'thumbPlayKey\\'\\: )\\'.+?\\'", System.Text.RegularExpressions.RegexOptions.IgnoreCase);

		System.Text.RegularExpressions.MatchCollection mc = r.Matches(responsedData);
		thumbplaykey = mc[0].Value.Replace("'", "");
		r = new System.Text.RegularExpressions.Regex("(?<=Nicovideo\\.playerUrl = )\\'.+?\\'");
		mc = r.Matches(responsedData);
		string referer = mc[0].Value.Replace("'", "");

		req = (HttpWebRequest)WebRequest.Create("http://ext.nicovideo.jp/thumb_watch");
		req.CookieContainer = ctor;
		req.Headers.Add(System.Net.HttpRequestHeader.AcceptLanguage, "ja,en-US;q=0.8,en;q=0.6");
		req.ContentType = "application/x-www-form-urlencoded";
		req.UserAgent = "Mozilla/5.0 (Windows NT 6.1) AppleWebKit/537.1 (KHTML, like Gecko) Chrome/21.0.1180.79 Safari/537.1";
		req.Method = "post";
		req.KeepAlive = true;
		req.Referer = referer;
		var _with1 = new System.IO.StreamWriter(req.GetRequestStream());
		string post = Uri.EscapeUriString(string.Format("as3=1&k={0}&v={1}", thumbplaykey, watchId));
		_with1.Write(post);
		_with1.Close();
		res = (HttpWebResponse)req.GetResponse();
		using (System.IO.StreamReader strm = new System.IO.StreamReader(res.GetResponseStream())) {
			response = strm.ReadToEnd();
		}

		return res.Cookies["nicohistory"];
	}
	public static string getflv(string watchId, ref System.Net.Cookie nicohistory, ref string player)
	{
		string thumbplaykey = null;
		string resbody = null;
		nicohistory = n_getflv.setWatchTicket(watchId, ref thumbplaykey, ref resbody, ref player);
		return resbody;
	}
	public static Dictionary<string, string> getflvParse(string arg)
	{

		Dictionary<string, string> d = new Dictionary<string, string>();
		foreach (string x in arg.Split('&')) {
			string[] kv = null;
			kv = x.Split('=');
			d.Add(kv[0], Uri.UnescapeDataString(kv[1]));
			kv = null;
		}
		return d;
	}
	public static XmlDocument getthumbinfo(string watchId)
	{
		System.Xml.XmlDocument d = new System.Xml.XmlDocument();
		d.Load(string.Format("http://ext.nicovideo.jp/api/getthumbinfo/{0}", watchId));
		return d;
	}
	#endregion

}


