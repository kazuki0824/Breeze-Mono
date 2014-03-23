
namespace rslibMonoCSharp.Utils
{
    public static class AdjustToFileNameFormat
    {
        public static string RemoveInvalidPathChar(string src)
        {
            int s = src.IndexOfAny(System.IO.Path.GetInvalidFileNameChars());
            while (s!=-1)
            {
                src = src.Remove(s, 1);
                s = src.IndexOfAny(System.IO.Path.GetInvalidFileNameChars());
            }
            return src;
        }
    }
}