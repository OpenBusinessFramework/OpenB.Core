using System.Collections.Generic;

namespace OpenB.DSL.Tokenizing
{
    public class LexicalAnalyser
    {
        private readonly string script;

        public LexicalAnalyser(string script)
        {
            this.script = script;
        }

        public IEnumerable<Token> Tokenize()
        {
            LineTokenizer lineTokenizer = new LineTokenizer();
            return lineTokenizer.Tokenize(script);
        }
    }
}