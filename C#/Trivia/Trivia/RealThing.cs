using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Trivia
{
    public interface IWriter
    {
        void WriteLine(string line);
    }

    class ConsoleWriter : IWriter
    {
        public void WriteLine(string line)
        {
            Console.WriteLine(line);
        }
    }

    class MockWriter : IWriter
    {
        private readonly StringBuilder _sb = new StringBuilder();

        public string OutputSoFar => _sb.ToString();

        public void WriteLine(string line)
        {
            _sb.AppendLine(line);
        }
    }
}
