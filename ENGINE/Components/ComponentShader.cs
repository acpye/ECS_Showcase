using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenGL.ENGINE.Managers;
using OpenGL.ENGINE.Scenes;
using OpenGL.ENGINE.OBJLoader;

namespace OpenGL.ENGINE.Components
{
    abstract class ComponentShader : IComponent
    {
        public int pgmID;

        public ComponentShader(string vertexShaderName, string fragmentShaderName)
        {
            pgmID = GL.CreateProgram();
            GL.AttachShader(pgmID, ResourceManager.LoadShader(vertexShaderName, ShaderType.VertexShader));
            GL.AttachShader(pgmID, ResourceManager.LoadShader(fragmentShaderName, ShaderType.FragmentShader));

            GL.BindAttribLocation(pgmID, 0, "a_Position");
            GL.BindAttribLocation(pgmID, 1, "a_TexCoord");
            GL.BindAttribLocation(pgmID, 2, "a_Normal");

            GL.LinkProgram(pgmID);
            Console.WriteLine(GL.GetProgramInfoLog(pgmID));
        }

        public abstract void ApplyShader(Scene scene, Matrix4 model, Geometry geometry);

        public ComponentTypes ComponentType
        {
            get { return ComponentTypes.COMPONENT_SHADER; }
        }

        public void Close() { }
    }
}
