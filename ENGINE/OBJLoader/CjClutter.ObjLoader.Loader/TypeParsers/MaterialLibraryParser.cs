using OpenGL.ENGINE.OBJLoader.CjClutter.ObjLoader.Loader.Loaders;
using OpenGL.ENGINE.OBJLoader.CjClutter.ObjLoader.Loader.TypeParsers.Interfaces;

namespace OpenGL.ENGINE.OBJLoader.CjClutter.ObjLoader.Loader.TypeParsers
{
    public class MaterialLibraryParser : TypeParserBase, IMaterialLibraryParser
    {
        private readonly IMaterialLibraryLoaderFacade _libraryLoaderFacade;

        public MaterialLibraryParser(IMaterialLibraryLoaderFacade libraryLoaderFacade)
        {
            _libraryLoaderFacade = libraryLoaderFacade;
        }

        protected override string Keyword
        {
            get { return "mtllib"; }
        }

        public override void Parse(string line)
        {
            _libraryLoaderFacade.Load(line);
        }
    }
}