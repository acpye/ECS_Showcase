using System;
using OpenGL.ENGINE.OBJLoader.CjClutter.ObjLoader.Loader.Common;
using OpenGL.ENGINE.OBJLoader.CjClutter.ObjLoader.Loader.Data.DataStore;
using OpenGL.ENGINE.OBJLoader.CjClutter.ObjLoader.Loader.Data.Elements;
using OpenGL.ENGINE.OBJLoader.CjClutter.ObjLoader.Loader.TypeParsers.Interfaces;

namespace OpenGL.ENGINE.OBJLoader.CjClutter.ObjLoader.Loader.TypeParsers
{
    public class FaceParser : TypeParserBase, IFaceParser
    {
        private readonly IFaceGroup _faceGroup;

        public FaceParser(IFaceGroup faceGroup)
        {
            _faceGroup = faceGroup;
        }

        protected override string Keyword
        {
            get { return "f"; }
        }

        public override void Parse(string line)
        {
            string[] vertices = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            Face face = new Face();

            foreach (string vertexString in vertices)
            {
                FaceVertex faceVertex = ParseFaceVertex(vertexString);
                face.AddVertex(faceVertex);
            }

            _faceGroup.AddFace(face);
        }

        private FaceVertex ParseFaceVertex(string vertexString)
        {
            string[] fields = vertexString.Split(new[]{'/'}, StringSplitOptions.None);

            int vertexIndex = fields[0].ParseInvariantInt();
            FaceVertex faceVertex = new FaceVertex(vertexIndex, 0, 0);

            if(fields.Length > 1)
            {
                int textureIndex = fields[1].Length == 0 ? 0 : fields[1].ParseInvariantInt();
                faceVertex.TextureIndex = textureIndex;
            }

            if(fields.Length > 2)
            {
                int normalIndex = fields.Length > 2 && fields[2].Length == 0 ? 0 : fields[2].ParseInvariantInt();
                faceVertex.NormalIndex = normalIndex;
            }

            return faceVertex;
        }
    }
}