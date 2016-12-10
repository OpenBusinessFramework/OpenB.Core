using System;
using System.Collections.Generic;

namespace OpenB.DSL.Tokenizing
{
    public abstract class BaseTokenizer
    {
        protected readonly IDictionary<string, ITokenizer> ChildTokenizers = new Dictionary<string, ITokenizer>();

        public void Add(string groupName, ITokenizer tokenizer)
        {
            if (tokenizer == null) throw new ArgumentNullException(nameof(tokenizer));
            if (string.IsNullOrEmpty(groupName)) throw new ArgumentNullException(nameof(groupName));


            ChildTokenizers.Add(groupName, tokenizer);
        }
    }
}