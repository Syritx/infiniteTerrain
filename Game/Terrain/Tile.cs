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

        float amplitude = 19.8f;
        float frequency = 1;
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


            vertices[0].Y = (float)noise.noise((vertices[0].X / offset)*frequency + offsetx, (vertices[0].Z / offset)*frequency + offsety, seed)* amplitude;
            vertices[1].Y = (float)noise.noise((vertices[1].X / offset)*frequency + offsetx, (vertices[1].Z / offset)*frequency + offsety, seed)* amplitude;
            vertices[2].Y = (float)noise.noise((vertices[2].X / offset)*frequency + offsetx, (vertices[2].Z / offset)*frequency + offsety, seed)* amplitude;
            vertices[3].Y = (float)noise.noise((vertices[3].X / offset)*frequency + offsetx, (vertices[3].Z / offset)*frequency + offsety, seed)* amplitude;

            normalA = tMath.calculateNormal(vertices[0], vertices[1], vertices[2]);
            normalB = tMath.calculateNormal(vertices[2], vertices[3], vertices[0]);
        }

        public void render(float offsetx, float offsety)
        {
            vertices[0].Y = (float)noise.noise((vertices[0].X / offset) * frequency + offsetx, (vertices[0].Z / offset) * frequency + offsety, seed) * amplitude;
            vertices[1].Y = (float)noise.noise((vertices[1].X / offset) * frequency + offsetx, (vertices[1].Z / offset) * frequency + offsety, seed) * amplitude;
            vertices[2].Y = (float)noise.noise((vertices[2].X / offset) * frequency + offsetx, (vertices[2].Z / offset) * frequency + offsety, seed) * amplitude;
            vertices[3].Y = (float)noise.noise((vertices[3].X / offset) * frequency + offsetx, (vertices[3].Z / offset) * frequency + offsety, seed) * amplitude;

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
