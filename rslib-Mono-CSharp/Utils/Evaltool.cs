namespace rslib_StringEvaluator
{
public class Evaltool
{
	public enum evalStrategy
	{
		WildCard = 0,
		RegularExpression = 1
	}
	public static bool Evaluation(string src, string pattern, evalStrategy strategy = evalStrategy.WildCard)
	{
        if (pattern.StartsWith("|") && pattern.EndsWith("|")) { strategy = evalStrategy.RegularExpression; }
		if ((strategy == evalStrategy.RegularExpression)) {
			return System.Text.RegularExpressions.Regex.IsMatch(src, pattern.Trim('|'));
		} else {
			return like_equivalent (src, pattern);
		}
	}
	private static bool like_equivalent(string src,string pattern)
	{
		pattern = pattern.Replace("*",".*").Replace("?",".").Replace("#","\\d");
	    return	System.Text.RegularExpressions.Regex.IsMatch(src,pattern,System.Text.RegularExpressions.RegexOptions.Singleline);
	}
}
}