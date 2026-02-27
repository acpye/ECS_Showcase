using OpenGL.ENGINE.OBJLoader.CjClutter.ObjLoader.Loader.Common;
using OpenGL.ENGINE.OBJLoader.CjClutter.ObjLoader.Loader.Data.DataStore;
using OpenGL.ENGINE.OBJLoader.CjClutter.ObjLoader.Loader.Data.VertexData;
using OpenGL.ENGINE.OBJLoader.CjClutter.ObjLoader.Loader.TypeParsers.Interfaces;

namespace OpenGL.ENGINE.OBJLoader.CjClutter.ObjLoader.Loader.TypeParsers
{
    public class TextureParser : TypeParserBase, ITextureParser
    {
        private readonly ITextureDataStore _textureDataStore;

        public TextureParser(ITextureDataStore textureDataStore)
        {
            _textureDataStore = textureDataStore;
        }

        protected override string Keyword
        {
            get { return "vt"; }
        }

        public override void Parse(string line)
        {
            string[] parts = line.Split(' ');

            float x = parts[0].ParseInvariantFloat();
            float y = parts[1].ParseInvariantFloat();

            Texture texture = new Texture(x, y);
            _textureDataStore.AddTexture(texture);
        }
    }
}