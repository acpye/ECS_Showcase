using System;
using OpenGL.ENGINE.OBJLoader.CjClutter.ObjLoader.Loader.Common;
using OpenGL.ENGINE.OBJLoader.CjClutter.ObjLoader.Loader.Data.DataStore;
using OpenGL.ENGINE.OBJLoader.CjClutter.ObjLoader.Loader.Data.VertexData;
using OpenGL.ENGINE.OBJLoader.CjClutter.ObjLoader.Loader.TypeParsers.Interfaces;

namespace OpenGL.ENGINE.OBJLoader.CjClutter.ObjLoader.Loader.TypeParsers
{
    public class VertexParser : TypeParserBase, IVertexParser
    {
        private readonly IVertexDataStore _vertexDataStore;

        public VertexParser(IVertexDataStore vertexDataStore)
        {
            _vertexDataStore = vertexDataStore;
        }

        protected override string Keyword
        {
            get { return "v"; }
        }

        public override void Parse(string line)
        {
            string[] parts = line.Split(new[]{' '}, StringSplitOptions.RemoveEmptyEntries);

            float x = parts[0].ParseInvariantFloat();
            float y = parts[1].ParseInvariantFloat();
            float z = parts[2].ParseInvariantFloat();

            Vertex vertex = new Vertex(x, y, z);
            _vertexDataStore.AddVertex(vertex);
        }
    }
}