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
        bool didTrue = false;

        float[] fogColor = { 230, 230, 230, .1f };
        float[] lightPos = { 100f, 100, 100f };
        float[] lightDiffuse = { .15f, .16f, .32f };
        float[] lightAmbient = { .4f, .4f, .6f };

        List<Chunk> chunks = new List<Chunk>();
        public static int chunkDistance = 16;
        int seed = new Random().Next(1, 100000000);

        int lastX = 0,
            lastZ = 0;

        List<int> seeds = new List<int>();

        Camera camera;
        ImprovedNoise noise;

        public Game(int width, int height, string title) : base(width,height,GraphicsMode.Default,title) {

            for (int i = 0; i < 20; i++) {
                seeds.Add(new Random().Next(1, 100000000));
            }

            camera = new Camera(this);
            noise = new ImprovedNoise();
            createChunks();

            Run(1/60);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            var view = Matrix4.LookAt(camera.position, camera.position + camera.front, camera.up);
            GL.LoadMatrix(ref view);
            GL.MatrixMode(MatrixMode.Modelview);

            GL.Light(LightName.Light0, LightParameter.Position, lightPos);
            GL.Light(LightName.Light0, LightParameter.Diffuse, lightDiffuse);
            GL.Light(LightName.Light0, LightParameter.Ambient, lightAmbient);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            for (int i = 0; i < chunks.Count; i++) {
                chunks[i].update();
            }

            SwapBuffers();
            base.OnRenderFrame(e);
        }

        protected override void OnResize(EventArgs e)
        {
            GL.Viewport(0, 0, Width, Height);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();

            Matrix4 perspectiveMatrix =
                Matrix4.CreatePerspectiveFieldOfView(1, 16/9, 1.0f, 2000.0f);
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
            bool startedLoadingChunks = true;
            int startX = (int)camera.position.X / (Chunk.NUM_TILES_LENGTH * Chunk.TILE_SIZE);
            int startZ = (int)camera.position.Z / (Chunk.NUM_TILES_LENGTH * Chunk.TILE_SIZE);

            for (int x = (int)(startX - chunkDistance); x < (int)(startX + chunkDistance); x++) {
                for (int z = (int)(startZ - chunkDistance); z < (int)(startZ + chunkDistance); z++) {

                    if (startedLoadingChunks) {
                        startedLoadingChunks = false;

                        lightPos[0] = x;
                        lightPos[1] = z;
                    }
                    chunks.Add(new Chunk(x +((float)chunkDistance/Chunk.TILE_SIZE), z+((float)chunkDistance/Chunk.TILE_SIZE), noise, seeds, true, 10.8f));
                }
            }
            return;
        }

        protected override void OnLoad(EventArgs e)
        {
            GL.ClearColor(0, 0, 0, 0);
            GL.Enable(EnableCap.DepthTest);

            GL.Enable(EnableCap.Blend);
            //GL.BlendFunc(BlendingFactor.Src1Alpha, BlendingFactor.OneMinusSrcAlpha);

            GL.Enable(EnableCap.Lighting);
            GL.Enable(EnableCap.ColorMaterial);
            GL.Enable(EnableCap.Light0);
            GL.Enable(EnableCap.Fog);

            GL.Enable(EnableCap.CullFace);
            GL.CullFace(CullFaceMode.Front);

            GL.Fog(FogParameter.FogMode, (int)FogMode.Linear);
            GL.Hint(HintTarget.FogHint, HintMode.Nicest);
            GL.Fog(FogParameter.FogColor, fogColor);

            GL.Fog(FogParameter.FogStart, (float)1000 / 1005);
            GL.Fog(FogParameter.FogEnd, 2050);

            base.OnLoad(e);
        }
    }
}
