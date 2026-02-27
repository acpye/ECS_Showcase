using OpenGL.ENGINE.OBJLoader.CjClutter.ObjLoader.Loader.Common;
using OpenGL.ENGINE.OBJLoader.CjClutter.ObjLoader.Loader.Data.DataStore;
using OpenGL.ENGINE.OBJLoader.CjClutter.ObjLoader.Loader.Data.VertexData;
using OpenGL.ENGINE.OBJLoader.CjClutter.ObjLoader.Loader.TypeParsers.Interfaces;

namespace OpenGL.ENGINE.OBJLoader.CjClutter.ObjLoader.Loader.TypeParsers
{
    public class NormalParser : TypeParserBase, INormalParser
    {
        private readonly INormalDataStore _normalDataStore;

        public NormalParser(INormalDataStore normalDataStore)
        {
            _normalDataStore = normalDataStore;
        }

        protected override string Keyword
        {
            get { return "vn"; }
        }

        public override void Parse(string line)
        {
            string[] parts = line.Split(' ');

            float x = parts[0].ParseInvariantFloat();
            float y = parts[1].ParseInvariantFloat();
            float z = parts[2].ParseInvariantFloat();

            Normal normal = new Normal(x, y, z);
            _normalDataStore.AddNormal(normal);
        }
    }
}