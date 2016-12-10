using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using OpenB.DSL.Expressions;
using OpenB.DSL.Tokenizing;

namespace OpenB.DSL.Test.Parser
{
    [TestFixture]
    public class ParserIfExpressionTest
    {
        public class ExpressionTreeBuilder
        {
            private readonly IEnumerable<Token> tokenList;

            public ExpressionTreeBuilder(IEnumerable<Token> tokenList)
            {
                this.tokenList = tokenList;
            }

            public IExpression Build()
            {
                var result = new ExpressionTree();

                foreach (var token in tokenList)
                {
                    if (token.Groupname == "expr")
                    {
                        var tokenizer = new RegexBasedTokenizer(@"(?<expr>.+?\))((\s+(?<log_op>(AND|OR))(\s+?))|$)");

                        var innnerTokenizer =
                            new RegexBasedTokenizer(
                                @"(\[(?<field>.+?)\])|(?<const>\d+(\.\d{0,5})?)|(?<operator>[\+|\-|\*])+?");
                        tokenizer.Add("expr", innnerTokenizer);

                        var tokens = tokenizer.Tokenize(token.Value);
                    }

                    if (token.Groupname == "operator")
                    {
                    }
                }

                return result;
            }
        }

        public class ConstantExpression : IExpression
        {
            private readonly string value;

            public ConstantExpression(string value)
            {
                if (value == null) throw new ArgumentNullException(nameof(value));

                this.value = value;
            }

            public object Evaluate()
            {
                throw new NotImplementedException();
            }
        }

        public class AddOperatorExpression : IExpression
        {
            private readonly IExpression _left;
            private readonly IExpression _right;

            public AddOperatorExpression(IExpression left, IExpression right)
            {
                if (_left == null) throw new ArgumentNullException(nameof(left));
                if (_right == null) throw new ArgumentNullException(nameof(right));

                _left = left;
                _right = right;
            }

            public object Evaluate()
            {
                var left = double.Parse(_left.Evaluate().ToString());
                var right = double.Parse(_right.Evaluate().ToString());

                return left + right;
            }
        }


        public class FieldExpression : IExpression
        {
            private readonly string value;

            public FieldExpression(string value)
            {
                this.value = value;
            }

            public object Evaluate()
            {
                throw new NotImplementedException();
            }
        }

        [Test]
        public void DoSometing()
        {
            LexicalAnalyser lexicalAnalyser = new LexicalAnalyser("blaat\r\nblaat\r\n\blaat");
            IEnumerable<Token> tokens = lexicalAnalyser.Tokenize();
        }

        [Test]
        public void SimpleCommand_ResultsIn_1_Token_Command()
        {
            var tokenizer = new RegexBasedTokenizer(@"(?<command>\w+$)");


            var tokens = tokenizer.Tokenize("HELLO");

            Assert.That(tokens.Count(), Is.EqualTo(1));
            Assert.That(tokens.ToArray()[0].Value, Is.EqualTo("HELLO"));
            Assert.That(tokens.ToArray()[0].Groupname, Is.EqualTo("command"));
        }

        [Test]
        public void SimpleCommand_ResultsIn_2_Tokens_Command_Parameters()
        {
            var tokenizer = new RegexBasedTokenizer(@"(?<command>(\w+?))\(((?<parameterlist>((\w+,\s*|\w+?))*))\)");

            var tokens = tokenizer.Tokenize("HELLO(a,b,c,d)");

            Assert.That(tokens.Count(), Is.EqualTo(2));
            Assert.That(tokens.ToArray()[0].Value, Is.EqualTo("HELLO"));
            Assert.That(tokens.ToArray()[0].Groupname, Is.EqualTo("command"));
        }

        [Test]
        public void SplitTokenizer_DoSomething()
        {
            var splitTokenizer =
                new SplitTokenizer(new SplitTokenizerOptions
                {
                    IgnoreWhitespace = true,
                    ItemName = "parameter",
                    SplitString = ","
                },
                    new RegexBasedTokenizer(
                        "(\\[(?<field>.+?)\\])|(?<const>(\\d+(\\.\\d{0,5})?)|(\".+?\"))|(?<operator>[\\+|\\-|\\*])+?"));

            var tokens = splitTokenizer.Tokenize("([Model.Field] + 1), \"Hello\", 1 - 3, true");

            Assert.That(tokens.Count(), Is.EqualTo(4));
        }

        [Test]
        public void SplitTokenizer_Result_Command_And_Parameters()
        {
            var mainTokenizer = new RegexBasedTokenizer(@"(?<command>(\w+?))\(((?<parameterlist>((\w+,\s*|\w+?))*))\)");


            var splitTokenizer =
                new SplitTokenizer(new SplitTokenizerOptions
                {
                    IgnoreWhitespace = true,
                    ItemName = "parameter",
                    SplitString = ","
                },
                    new RegexBasedTokenizer(
                        "(\\[(?<field>.+?)\\])|(?<const>(\\d+(\\.\\d{0,5})?)|(\".+?\"))|(?<operator>[\\+|\\-|\\*])+?"));

            mainTokenizer.Add("parameterlist", splitTokenizer);

            var tokens = splitTokenizer.Tokenize("COMMAND([Model.Field] + 1), \"Hello\", 1 - 3, true");

            Assert.That(tokens.Count(), Is.EqualTo(5));
        }
    }

    public class ExpressionTree : IExpression
    {
        public IList<IExpression> ChildExpressions;

        public ExpressionTree()
        {
            ChildExpressions = new List<IExpression>();
        }


        public object Evaluate()
        {
            foreach (var expression in ChildExpressions)
            {
                expression.Evaluate();
            }

            return null;
        }

        public void Add(IExpression expression)
        {
            ChildExpressions.Add(expression);
        }
    }
}