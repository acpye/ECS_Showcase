using System.IO;

namespace OpenGL.ENGINE.OBJLoader.CjClutter.ObjLoader.Loader.Loaders
{
    public abstract class LoaderBase
    {
        private StreamReader _lineStreamReader;

        protected void StartLoad(Stream lineStream)
        {
            _lineStreamReader = new StreamReader(lineStream);

            while (!_lineStreamReader.EndOfStream)
            {
                ParseLine();
            }
        }

        private void ParseLine()
        {
            string currentLine = _lineStreamReader.ReadLine();

            if (string.IsNullOrWhiteSpace(currentLine) || currentLine[0] == '#')
            {
                return;
            }

            string[] fields = currentLine.Trim().Split(null, 2);
            string keyword = fields[0].Trim();
            string data = fields[1].Trim();

            ParseLine(keyword, data);
        }

        protected abstract void ParseLine(string keyword, string data);
    }
}