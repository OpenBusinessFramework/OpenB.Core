namespace OpenB.DSL.Tokenizing
{
    public class LineTokenizer : SplitTokenizer
    {
        public LineTokenizer()
            : base(new SplitTokenizerOptions {IgnoreWhitespace = true, ItemName = "Line", SplitString = "\r\n"}, new RegexBasedTokenizer("blip"))
        {
        }
    }
}