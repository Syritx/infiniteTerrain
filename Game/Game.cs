using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Graphics;
using System.Collections.Generic;

using infiniteTerrain.Game.Terrain;
using infiniteTerrain.Game.Rendering;

namespace infiniteTerrain.Game
{
    public class Game : GameWindow
    {
        public float worldX, worldY;

        float[] fogColor = { 230, 230, 230, .1f };

        float[] lightPos = { 100f, 100, 100f };
        float[] lightDiffuse = { .15f, .16f, .32f };
        float[] lightAmbient = { .4f, .4f, .6f };

        List<Chunk> chunks = new List<Chunk>();
        public static int chunkDistance = 10;
        int seed = new Random().Next(1, 100000000);

        Camera camera;
        ImprovedNoise noise;

        public Game(int width, int height, string title) : base(width,height,GraphicsMode.Default,title) {

            camera = new Camera(this);
            noise = new ImprovedNoise();
            createChunks();

            Run(60);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            var view = Matrix4.LookAt(camera.position, camera.position + camera.front, camera.up);
            GL.LoadMatrix(ref view);
            GL.MatrixMode(MatrixMode.Modelview);

            GL.Fog(FogParameter.FogMode, (int)FogMode.Linear);
            GL.Hint(HintTarget.FogHint, HintMode.Nicest);
            GL.Fog(FogParameter.FogColor, fogColor);

            GL.Fog(FogParameter.FogDensity, (float).01f);
            GL.Fog(FogParameter.FogStart, (float)1000/15);
            GL.Fog(FogParameter.FogEnd, 200);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            for (int i = 0; i < chunks.Count; i++) {
                chunks[i].update();
            }

            GL.Light(LightName.Light0, LightParameter.Position, lightPos);
            GL.Light(LightName.Light0, LightParameter.Diffuse, lightDiffuse);
            GL.Light(LightName.Light0, LightParameter.Ambient, lightAmbient);

            SwapBuffers();
            base.OnRenderFrame(e);
        }

        protected override void OnResize(EventArgs e)
        {
            GL.Viewport(0, 0, Width, Height);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();

            Matrix4 perspectiveMatrix =
                Matrix4.CreatePerspectiveFieldOfView(1, Width/Height, 1.0f, 2000.0f);
            GL.LoadMatrix(ref perspectiveMatrix);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.End();
            base.OnResize(e);
        }

        public void updateAllChunks()
        {
            chunks = new List<Chunk>();
            createChunks();
        }

        void createChunks()
        {
            for (int x = (int)(camera.relative.X-chunkDistance); x < (int)(camera.relative.X+chunkDistance); x++) {
                for (int z = (int)(camera.relative.Z-chunkDistance); z < (int)(camera.relative.Z+chunkDistance); z++) {
                    Console.WriteLine(x + " " + z);
                    chunks.Add(new Chunk(x, z, noise, seed,camera));
                }
            }
            return;
        }

        protected override void OnLoad(EventArgs e)
        {
            GL.ClearColor(0, 0, 0, 0);
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.Fog);

            GL.Enable(EnableCap.Lighting);
            GL.Enable(EnableCap.ColorMaterial);
            GL.Enable(EnableCap.Light0);

            base.OnLoad(e);
        }
    }
}
