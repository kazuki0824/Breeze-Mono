using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using rslib_StringEvaluator;

static class OrbitV
{
	#region "ReflectionBlock"
	public static Dictionary<int, Tuple<string, Uri, string>> Getniconico(string url, out Dictionary<string, string> videoinfo)
	{
		Uri u = new Uri(url);
		System.Net.Cookie nchs = null;
		string player_raw = "";
		string watchId = u.AbsolutePath.Replace("/watch/", "");
		string getflv_param = n_getflv.getflv(watchId, ref nchs,ref player_raw);
		videoinfo = n_getflv.getflvParse(getflv_param);
		string downloadUri = videoinfo["url"];
		Regex r = new Regex("(?<=movieType\\: )\\'.+?\\'", System.Text.RegularExpressions.RegexOptions.ECMAScript | System.Text.RegularExpressions.RegexOptions.Compiled);
		string ext = r.Matches(player_raw)[0].Value.Replace("'", "");
		Dictionary<int, Tuple<string, Uri, string>> returnvalue = new Dictionary<int, Tuple<string, Uri, string>> { {
				0,
				new Tuple<string, Uri, string>("*." + ext, new Uri(downloadUri), nchs.ToString())
			} };
		getflv_param = null;
		return returnvalue;
	}
	public static Dictionary<int, Tuple<string, Uri, string>> GetYoutube(string url)
	{
		string yt_ck = "";
		string yt_title = "";
		SortedDictionary<int, string> dic =kk_movDlForUser.yt(url);
		yt_ck = dic[-1];
		dic.Remove(-1);
		yt_title = dic[-2];
		dic.Remove(-2);

		Dictionary<int, Tuple<string, Uri, string>> newdict = new Dictionary<int, Tuple<string, Uri, string>>();
		foreach (object t_loopVariable in dic) {
            KeyValuePair<int, string> t = (KeyValuePair<int, string>)t_loopVariable;
			newdict.Add(t.Key, new Tuple<string, Uri, string>(yt_title + ".flv", new Uri(t.Value), yt_ck));
		}
		return newdict;
	}
	private const Int32 _dmCountLimit = 300;
	private const string _dmUriPattern = "";
	public static Dictionary<int, Tuple<string, Uri, string>> Dailymotion(string url)
	{
		//Dim fObj As IEnumerable(Of Fiddler.Session)
		//With myOrbit.getFiddlerLog()
		//    fObj = .Reverse().
		//        TakeWhile(Function(item, i) i < _dmCountLimit AndAlso item.fullUrl Like _dmUriPattern).
		//        Reverse
		//End With
		string m = url;

		if (Evaltool.Evaluation(m, "http://www.dailymotion.com/video/???????_*")) {
			m = m.Substring(0, m.IndexOf('_'));
		}
		m.Remove(m.IndexOf('_'));
		HttpWebRequest req = WebRequest.CreateHttp(m.Replace("/video/", "/embed/video/"));
		WebResponse _with1 = req.GetResponse();
		using (StreamReader r = new StreamReader( _with1.GetResponseStream())) {
			m = r.ReadToEnd();
		}
		_with1.Close();
		m = m.Substring(m.IndexOf("var info = ") + "var info = ".Length);
		Debug.Print(m);
		m = m.Remove(m.IndexOf("," + Environment.NewLine));
		Newtonsoft.Json.Linq.JObject j = Newtonsoft.Json.Linq.JObject.Parse(m);
		qList.Reverse();
		List<Tuple<string, Uri, string>> rList = new List<Tuple<string, Uri, string>>();
		qList.AsParallel().AsOrdered().ForAll((s) =>
			{
				string realUrl = j[s].ToString();
				if (string.IsNullOrEmpty(realUrl))
					return;
				string savename = realUrl.Remove(realUrl.IndexOf('?'));
				savename = savename.Substring(savename.LastIndexOf('/') + 1);
				rList.Add(Tuple.Create<string, Uri, string>(savename, new Uri(realUrl, UriKind.Absolute), ""));
			});
		SortedDictionary<int, string> d = new SortedDictionary<int, string>();
		int ind = -1;
		return rList.ToDictionary<Tuple<string,Uri,string>,int>((item) =>
			{
				ind += 1;
				return ind;
			});
	#endregion
	}
    static readonly List<string> qList = new List<string> {
		"stream_h264_hd1080_url",
		"stream_h264_hq_url",
		"stream_h264_hd_url",
		"stream_h264_url",
		"stream_h264_ld_url"
	};
    }
