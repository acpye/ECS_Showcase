using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenGL.ENGINE.Scenes;
using OpenGL.GAME.Scenes;
using OpenGL.ENGINE.OBJLoader;

namespace OpenGL.ENGINE.Components
{
    class ComponentShaderDefault : ComponentShader
    {
        public int uniform_stex;
        public int uniform_mmodelviewproj;
        public int uniform_mmodel;
        public int uniform_diffuse;

        public ComponentShaderDefault() : base("ENGINE/Shaders/single-light.vert", "ENGINE/Shaders/single-light.frag")
        {
            uniform_stex = GL.GetUniformLocation(pgmID, "s_texture");
            uniform_mmodelviewproj = GL.GetUniformLocation(pgmID, "ModelViewProjMat");
            uniform_mmodel = GL.GetUniformLocation(pgmID, "ModelMat");
            uniform_diffuse = GL.GetUniformLocation(pgmID, "v_diffuse");
        }

        public override void ApplyShader(Scene scene, Matrix4 model, Geometry geometry)
        {
            GL.UseProgram(pgmID);

            GL.Uniform1(uniform_stex, 0);
            GL.ActiveTexture(TextureUnit.Texture0);

            GL.UniformMatrix4(uniform_mmodel, false, ref model);
            Matrix4 modelViewProjection = model * GameScene.gameInstance.camera.view * GameScene.gameInstance.camera.projection;
            GL.UniformMatrix4(uniform_mmodelviewproj, false, ref modelViewProjection);

            geometry.Render(uniform_diffuse);

            GL.UseProgram(0);
        }
    }
}