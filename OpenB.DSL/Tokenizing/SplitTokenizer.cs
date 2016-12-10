using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace OpenB.DSL.Tokenizing
{
    public class SplitTokenizer : BaseTokenizer, ITokenizer
    {
        private readonly ITokenizer childTokenizer;
        private readonly SplitTokenizerOptions splitTokenizerOptions;

        public SplitTokenizer(SplitTokenizerOptions splitTokenizerOptions, ITokenizer childTokenizer)
        {
            if (splitTokenizerOptions == null) throw new ArgumentNullException(nameof(splitTokenizerOptions));
            if (childTokenizer == null) throw new ArgumentNullException(nameof(childTokenizer));

            this.splitTokenizerOptions = splitTokenizerOptions;
            this.childTokenizer = childTokenizer;
        }

        public IEnumerable<Token> Tokenize(string expression)
        {
            var items = Regex.Split(expression, splitTokenizerOptions.SplitString, RegexOptions.Multiline);
            return TokenizeChildren(items);
        }

        private IList<Token> TokenizeChildren(string[] expressions)
        {
            var tokenList = new List<Token>();

            foreach (var expression in expressions)
            {
                Token token = new Token(splitTokenizerOptions.ItemName, childTokenizer.Tokenize(expression), expression);
                tokenList.Add(token);

            }

            return tokenList;
        }
    }
}