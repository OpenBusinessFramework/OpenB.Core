using System.Collections.Generic;

namespace OpenB.DSL.Tokenizing
{
    public interface ITokenizer
    {
        void Add(string groupName, ITokenizer tokenizer);
        IEnumerable<Token> Tokenize(string expression);
    }
}