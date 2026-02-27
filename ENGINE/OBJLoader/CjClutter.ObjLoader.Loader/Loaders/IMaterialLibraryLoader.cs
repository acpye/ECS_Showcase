using System.IO;

namespace OpenGL.ENGINE.OBJLoader.CjClutter.ObjLoader.Loader.Loaders
{
    public interface IMaterialLibraryLoader
    {
        void Load(Stream lineStream);
    }
}