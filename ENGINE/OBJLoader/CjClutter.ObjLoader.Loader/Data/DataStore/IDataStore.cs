using System.Collections.Generic;
using OpenGL.ENGINE.OBJLoader.CjClutter.ObjLoader.Loader.Data;
using OpenGL.ENGINE.OBJLoader.CjClutter.ObjLoader.Loader.Data.Elements;
using OpenGL.ENGINE.OBJLoader.CjClutter.ObjLoader.Loader.Data.VertexData;

namespace OpenGL.ENGINE.OBJLoader.CjClutter.ObjLoader.Loader.Data.DataStore
{
    public interface IDataStore 
    {
        IList<Vertex> Vertices { get; }
        IList<Texture> Textures { get; }
        IList<Normal> Normals { get; }
        IList<Material> Materials { get; }
        IList<DataGroup> Groups { get; }
    }
}