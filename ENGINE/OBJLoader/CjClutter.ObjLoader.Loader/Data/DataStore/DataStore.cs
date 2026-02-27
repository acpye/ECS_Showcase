using System.Collections.Generic;
using System.Linq;
using OpenGL.ENGINE.OBJLoader.CjClutter.ObjLoader.Loader.Common;
using OpenGL.ENGINE.OBJLoader.CjClutter.ObjLoader.Loader.Data;
using OpenGL.ENGINE.OBJLoader.CjClutter.ObjLoader.Loader.Data.Elements;
using OpenGL.ENGINE.OBJLoader.CjClutter.ObjLoader.Loader.Data.VertexData;

namespace OpenGL.ENGINE.OBJLoader.CjClutter.ObjLoader.Loader.Data.DataStore
{
    public class DataStore : IDataStore, IGroupDataStore, IVertexDataStore, ITextureDataStore, INormalDataStore,
                             IFaceGroup, IMaterialLibrary, IElementGroup
    {
        private DataGroup _currentGroup;

        private readonly List<DataGroup> _groups = new List<DataGroup>();
        private readonly List<Material> _materials = new List<Material>();

        private readonly List<Vertex> _vertices = new List<Vertex>();
        private readonly List<Texture> _textures = new List<Texture>();
        private readonly List<Normal> _normals = new List<Normal>();

        public IList<Vertex> Vertices
        {
            get { return _vertices; }
        }

        public IList<Texture> Textures
        {
            get { return _textures; }
        }

        public IList<Normal> Normals
        {
            get { return _normals; }
        }

        public IList<Material> Materials
        {
            get { return _materials; }
        }

        public IList<DataGroup> Groups
        {
            get { return _groups; }
        }

        public void AddFace(Face face)
        {
            PushGroupIfNeeded();

            _currentGroup.AddFace(face);
        }

        public void PushGroup(string groupName)
        {
            _currentGroup = new DataGroup(groupName);
            _groups.Add(_currentGroup);
        }

        private void PushGroupIfNeeded()
        {
            if (_currentGroup == null)
            {
                PushGroup("default");
            }
        }

        public void AddVertex(Vertex vertex)
        {
            _vertices.Add(vertex);
        }

        public void AddTexture(Texture texture)
        {
            _textures.Add(texture);
        }

        public void AddNormal(Normal normal)
        {
            _normals.Add(normal);
        }

        public void Push(Material material)
        {
            _materials.Add(material);
        }

        public void SetMaterial(string materialName)
        {
            Material material = _materials.SingleOrDefault(x => x.Name.EqualsOrdinalIgnoreCase(materialName));
            PushGroupIfNeeded();
            _currentGroup.Material = material;
        }
    }
}