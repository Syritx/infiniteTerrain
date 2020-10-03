using System.Collections.Generic;
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

        float amplitude = 12.8f;
        float frequency = 1.5f;
        float offset = Chunk.TILE_SIZE * Chunk.NUM_TILES_LENGTH;

        public Tile(Vector2 position, int TILE_SIZE, ImprovedNoise noise, List<int> seed)
        {
            vertices[0] = new Vector3(position.X, DEFAULT_Y, position.Y);
            vertices[1] = new Vector3(vertices[0].X + TILE_SIZE, DEFAULT_Y, position.Y);
            vertices[2] = new Vector3(vertices[0].X + TILE_SIZE, DEFAULT_Y, position.Y + TILE_SIZE);
            vertices[3] = new Vector3(vertices[0].X, DEFAULT_Y, position.Y + TILE_SIZE);

            normalA = tMath.calculateNormal(vertices[0], vertices[1], vertices[2]);
            normalB = tMath.calculateNormal(vertices[2], vertices[3], vertices[0]);

            for (int i = 0; i < 4; i++) {
                for (int j = 1; j < seed.Count; j++) {
                    vertices[i].Y += (float)noise.noise(((vertices[i].X/offset)*(frequency/(3*j))), ((vertices[i].Z / offset)*(frequency/(3*j))), seed[j]) * (amplitude+(13.5f*j));
                }
            }
        }

        public void render()
        {
            GL.Begin(BeginMode.Triangles);
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
