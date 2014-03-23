using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Reflection;
using rslib_StringEvaluator;

namespace rslib_Ctrl {
public class myOrbit
{
	/// <summary>
	/// Parser
	/// </summary>
	public enum vServiceKind
	{
		No,
		Niconico,
		Niconama,
		Youtube,
		Ustream,
		Anitube,
		Youku,
		Pandoratv,
		Dailymotion
	}
	/// <summary>
	/// The reflection target
	/// </summary>
	static Hashtable reflectionTarget = new Hashtable {
		{
			vServiceKind.Youtube,
			"GetYoutube"
		},
		{
			vServiceKind.Niconico,
			"Getniconico"
		},
		{
			vServiceKind.Dailymotion,
			"Dailymotion"
		}
	};
	public static vServiceKind UsingParserCk(string u)
	{
		vServiceKind attribute = default(vServiceKind);
		if ((Evaltool.Evaluation(u, "http(s)?://(www\\.)?youtube\\.com/watch\\?.*", Evaltool.evalStrategy.RegularExpression)) || Evaltool.Evaluation(u, "http://youtu\\.be/\\w+")) {
			attribute = vServiceKind.Youtube;
		} else if (Evaltool.Evaluation(u, "http://(www\\.)?nicovideo\\.jp/watch/[sn][mo]\\d+", Evaltool.evalStrategy.RegularExpression)) {
			attribute = vServiceKind.Niconico;
		} else if (Evaltool.Evaluation(u, "(?<=http://v.youku.com/v_show/id_)\\w+(?=\\.html)", Evaltool.evalStrategy.RegularExpression)) {
			attribute = vServiceKind.Youku;
		} else if (Evaltool.Evaluation(u, "http://www.dailymotion.com/*ideo/*", Evaltool.evalStrategy.WildCard)) {
			attribute = vServiceKind.Dailymotion;
		} else {
			attribute = vServiceKind.No;
		}
		return attribute;
	}

	/// <summary>
	/// Usings the parser main.
	/// </summary>
	/// <param name="u">動画のURL.</param>
	/// <returns>
	/// 0から始まるインデックス.(有効な場合は画質番号.) 順に[ファイル名],[URI],[Cookie].
	/// </returns>
	public static SortedDictionary<int, Tuple<string, Uri, string>> UsingParserMain(string u)
	{
		string invoke = Convert.ToString(reflectionTarget[UsingParserCk(u)]);
		if ((invoke == null))
			return null;
		Type t = typeof(OrbitV);
		object returnValue = t.InvokeMember(invoke, BindingFlags.InvokeMethod | BindingFlags.OptionalParamBinding, null, null, new object[] { u });
		if (returnValue.GetType().Name.Contains(@"Dictionary")) {
			return (new SortedDictionary<int, Tuple<string, Uri, string>>((Dictionary<int, Tuple<string, Uri, string>>)returnValue));
        } else {
            return null;
        }
	}
}
}