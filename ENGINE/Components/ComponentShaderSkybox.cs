using OpenGL.ENGINE.Components;
using OpenGL.ENGINE.Managers;
using OpenGL.ENGINE.OBJLoader;
using OpenGL.ENGINE.Scenes;
using OpenGL.GAME.Scenes;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace OpenGL.GAME.Components
{
    class ComponentShaderSkybox : ComponentShader
    {
        private readonly int uniform_stex;
        private readonly int uniform_mmodelviewproj;
        private readonly int uniform_diffuse;

        public ComponentShaderSkybox() : base("ENGINE/Shaders/skybox.vert", "ENGINE/Shaders/skybox.frag")
        {
            uniform_stex = GL.GetUniformLocation(pgmID, "s_texture");
            uniform_mmodelviewproj = GL.GetUniformLocation(pgmID, "ModelViewProjMat");
            uniform_diffuse = GL.GetUniformLocation(pgmID, "v_diffuse");
        }

        public override void ApplyShader(Scene scene, Matrix4 model, Geometry geometry)
        {
            GL.DepthMask(false);
            GL.DepthFunc(DepthFunction.Lequal);

            GL.UseProgram(pgmID);

            GL.Uniform1(uniform_stex, 0);
            GL.ActiveTexture(TextureUnit.Texture0);

            Matrix4 modelViewProjection = model * GameScene.gameInstance.camera.view * GameScene.gameInstance.camera.projection;
            GL.UniformMatrix4(uniform_mmodelviewproj, false, ref modelViewProjection);

            geometry.Render(uniform_diffuse);

            GL.UseProgram(0);

            GL.DepthMask(true);
            GL.DepthFunc(DepthFunction.Less);
        }
    }
}