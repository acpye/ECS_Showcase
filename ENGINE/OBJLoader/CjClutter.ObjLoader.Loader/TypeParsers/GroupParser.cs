using OpenGL.ENGINE.OBJLoader.CjClutter.ObjLoader.Loader.Data.DataStore;
using OpenGL.ENGINE.OBJLoader.CjClutter.ObjLoader.Loader.TypeParsers.Interfaces;

namespace OpenGL.ENGINE.OBJLoader.CjClutter.ObjLoader.Loader.TypeParsers
{
    public class GroupParser : TypeParserBase, IGroupParser
    {
        private readonly IGroupDataStore _groupDataStore;

        public GroupParser(IGroupDataStore groupDataStore)
        {
            _groupDataStore = groupDataStore;
        }

        protected override string Keyword
        {
            get { return "g"; }
        }

        public override void Parse(string line)
        {
            _groupDataStore.PushGroup(line);
        }
    }
}