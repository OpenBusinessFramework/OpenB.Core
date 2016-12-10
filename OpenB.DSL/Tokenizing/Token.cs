using System.Collections.Generic;

namespace OpenB.DSL.Tokenizing
{
    public class Token
    {
        public Token(string groupname, string value)
        {
            Groupname = groupname;
            Value = value;
        }

        public Token(string groupname, IEnumerable<Token> childTokens, string value)
            : this(groupname, value)
        {
            ChildTokens = childTokens;
        }

        public string Value { get; private set; }
        public string Groupname { get; private set; }

        public IEnumerable<Token> ChildTokens { get; private set; }

        public override string ToString()
        {
            return string.Format("{0}:{1}", Groupname, Value);
        }
    }
}