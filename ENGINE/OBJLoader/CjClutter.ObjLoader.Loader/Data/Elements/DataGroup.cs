using System.Collections.Generic;
using OpenGL.ENGINE.OBJLoader.CjClutter.ObjLoader.Loader.Data;
using OpenGL.ENGINE.OBJLoader.CjClutter.ObjLoader.Loader.Data.DataStore;

namespace OpenGL.ENGINE.OBJLoader.CjClutter.ObjLoader.Loader.Data.Elements
{
    public class DataGroup : IFaceGroup
    {
        private readonly List<Face> _faces = new List<Face>();
        
        public DataGroup(string name)
        {
            Name = name;
        }

        public string Name { get; private set; }
        public Material Material { get; set; }

        public IList<Face> Faces { get { return _faces; } }

        public void AddFace(Face face)
        {
            _faces.Add(face);
        }
    }
}