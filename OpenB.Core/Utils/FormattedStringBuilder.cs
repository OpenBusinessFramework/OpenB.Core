using System;
using System.Text;

namespace OpenB.Core.Utils
{
    public class FormattedStringBuilder
    {
        private readonly StringBuilder _stringBuilder;
        private int _currentLevel;
        private bool _lastActionWasLine;

        public FormattedStringBuilder()
        {
            _stringBuilder = new StringBuilder();
            _currentLevel = 0;
        }

        public void LevelUp()
        {
            _currentLevel = Math.Max(0, _currentLevel - 1);
        }

        public void LevelDown()
        {
            _currentLevel++;
        }

        public void AppendLine()
        {
            _lastActionWasLine = true;
            _stringBuilder.AppendLine();
        }

        public void Append(string value)
        {
            string correctedValue = _lastActionWasLine ? string.Concat(GetIndentation(), value) : value;

            _stringBuilder.Append(correctedValue);
            _lastActionWasLine = false;
        }

        public void AppendLine(string value)
        {
            _lastActionWasLine = true;
            _stringBuilder.AppendLine(string.Concat(GetIndentation(), value));
        }

        private string GetIndentation()
        {
            var indentedStringBuilder = new StringBuilder();
            for (int x = 0; x < _currentLevel; x++)
            {
                indentedStringBuilder.Append("\t");
            }

            return indentedStringBuilder.ToString();
        }

        public override string ToString()
        {
            return _stringBuilder.ToString();
        }
    }
}