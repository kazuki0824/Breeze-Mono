using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
static class Evaltool
{
	public enum evalStrategy
	{
		WildCard = 0,
		RegularExpression = 1
	}
	public static bool Evaluation(string src, string pattern, evalStrategy strategy = evalStrategy.WildCard)
	{
		if ((strategy == evalStrategy.RegularExpression)) {
			return System.Text.RegularExpressions.Regex.IsMatch(src, pattern.Trim('|'));
		} else {
			return like (src, pattern);
				;
		}
	}
	private static bool like(string src,string pattern)
	{
		pattern = pattern.Replace("*",".*").Replace("?",".").Replace("#","\\d");
	    return	System.Text.RegularExpressions.Regex.IsMatch(src,pattern,System.Text.RegularExpressions.RegexOptions.Singleline);
	}
}