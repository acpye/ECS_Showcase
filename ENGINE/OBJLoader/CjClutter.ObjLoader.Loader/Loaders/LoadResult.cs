using System.Collections.Generic;
using OpenGL.ENGINE.OBJLoader.CjClutter.ObjLoader.Loader.Data;
using OpenGL.ENGINE.OBJLoader.CjClutter.ObjLoader.Loader.Data.Elements;
using OpenGL.ENGINE.OBJLoader.CjClutter.ObjLoader.Loader.Data.VertexData;

namespace OpenGL.ENGINE.OBJLoader.CjClutter.ObjLoader.Loader.Loaders
{
    public class LoadResult  
    {
        public IList<Vertex> Vertices { get; set; }
        public IList<Texture> Textures { get; set; }
        public IList<Normal> Normals { get; set; }
        public IList<DataGroup> Groups { get; set; }
        public IList<Material> Materials { get; set; }
    }
}