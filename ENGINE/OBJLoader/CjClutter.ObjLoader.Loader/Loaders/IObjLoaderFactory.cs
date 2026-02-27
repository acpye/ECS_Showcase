namespace OpenGL.ENGINE.OBJLoader.CjClutter.ObjLoader.Loader.Loaders
{
    public interface IObjLoaderFactory
    {
        IObjLoader Create(IMaterialStreamProvider materialStreamProvider);
        IObjLoader Create();
    }
}