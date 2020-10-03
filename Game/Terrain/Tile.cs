using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace infiniteTerrain.Game.Terrain
{
    public class Tile
    {
        private int DEFAULT_Y = -2;

        Vector3[] vertices = {
            new Vector3(0,0,0),
            new Vector3(0,0,0),
            new Vector3(0,0,0),
            new Vector3(0,0,0),
        };

        Vector3 normalA, normalB;
        ImprovedNoise noise;

        float amplitude = 49.8f;
        float frequency = .5f;
        float offset = Chunk.TILE_SIZE * Chunk.NUM_TILES_LENGTH;
        int seed;

        public Tile(Vector2 position, int TILE_SIZE, ImprovedNoise noise, int seed, Chunk chunk, float offsetx, float offsety)
        {
            this.seed = seed;
            this.noise = noise;

            vertices[0] = new Vector3(position.X, DEFAULT_Y, position.Y);
            vertices[1] = new Vector3(vertices[0].X + TILE_SIZE, DEFAULT_Y, position.Y);
            vertices[2] = new Vector3(vertices[0].X + TILE_SIZE, DEFAULT_Y, position.Y + TILE_SIZE);
            vertices[3] = new Vector3(vertices[0].X, DEFAULT_Y, position.Y + TILE_SIZE);


            for (int i = 0; i < 4; i++) {
                for (int j = 0; j < 2; j++) {
                    vertices[i].Y += (float)noise.noise((vertices[i].X / offset) * frequency + offsetx, (vertices[i].Z / offset) * frequency + offsety, seed) * amplitude;
                }
            }

            normalA = tMath.calculateNormal(vertices[0], vertices[1], vertices[2]);
            normalB = tMath.calculateNormal(vertices[2], vertices[3], vertices[0]);
        }

        public void render(float offsetx, float offsety)
        {
            for (int i = 0; i < 4; i++) {
                vertices[i].Y = (float)noise.noise((vertices[i].X / offset) * frequency + offsetx, (vertices[i].Z / offset) * frequency + offsety, seed) * amplitude;
            }

            GL.Begin(PrimitiveType.Triangles);
            GL.Color3((double)114 / 255, (double)179 / 255, (double)29 / 255);

            GL.Normal3(normalA);
            GL.Vertex3(vertices[0]);
            GL.Vertex3(vertices[1]);
            GL.Vertex3(vertices[2]);

            GL.Normal3(normalB);
            GL.Vertex3(vertices[2]);
            GL.Vertex3(vertices[3]);
            GL.Vertex3(vertices[0]);
            GL.End();
        }
    }
}
