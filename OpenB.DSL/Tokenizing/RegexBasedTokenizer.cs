using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace OpenB.DSL.Tokenizing
{
    public class RegexBasedTokenizer : BaseTokenizer, ITokenizer
    {
        
        private readonly Regex _regex;

        public RegexBasedTokenizer(string regularExpression)
        {
            try
            {
                _regex = new Regex(regularExpression, RegexOptions.Multiline);
            }
            catch (ArgumentException argumentException)
            {
                throw new NotSupportedException(
                    string.Format("The given regular expression is not supported ({0}).", regularExpression),
                    argumentException);
            }
        }

       

        public IEnumerable<Token> Tokenize(string expression)
        {
            if (expression == null) throw new ArgumentNullException(nameof(expression));

            IList<Token> tokens = new List<Token>();

            if (_regex.Match(expression).Success)
            {
                foreach (Match m in _regex.Matches(expression))
                {
                    foreach (var groupName in _regex.GetGroupNames())
                    {
                        int groupNumber;

                        if (int.TryParse(groupName, out groupNumber))
                        {
                            continue;
                        }

                        var g = m.Groups[groupName];

                        if (g.Success)
                        {
                            foreach (Capture capture in g.Captures)
                            {
                                var canChildTokenizersBeApplied = ChildTokenizers.Any(c => c.Key.Equals(groupName));

                                var token = canChildTokenizersBeApplied
                                    ? new Token(groupName, TokenizeChildren(groupName, capture.Value), capture.Value)
                                    : new Token(groupName, capture.Value);
                                tokens.Add(token);
                            }
                        }
                    }
                }
            }

            return tokens;
        }

        private IList<Token> TokenizeChildren(string groupName, string value)
        {
            var tokens = new List<Token>();

            foreach (RegexBasedTokenizer tokenizer in ChildTokenizers.Where(c => c.Key.Equals(groupName)).Select(c => c.Value))
            {
                tokens.AddRange(tokenizer.Tokenize(value));
            }
            return tokens;
        }
    }

    public class SplitTokenizerOptions
    {
        public string SplitString { get; set; }
        public bool IgnoreWhitespace { get; set; }
        public string ItemName { get; set; }
    }
}