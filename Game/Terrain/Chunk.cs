using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace infiniteTerrain.Game.Terrain
{
    public class Chunk
    {
        public static int NUM_TILES_LENGTH = 2;
        public static int TILE_SIZE = 50;

        bool hasWater;
        float waterLevel;

        List<Tile> tiles = new List<Tile>();
        float WORLD_X, WORLD_Z;

        public Chunk(float rx, float rz, ImprovedNoise noise, List<int> seeds, bool hasWater, float waterLevel) {

            this.hasWater = hasWater;
            this.waterLevel = waterLevel;

            WORLD_X = rx * NUM_TILES_LENGTH * TILE_SIZE;
            WORLD_Z = rz * NUM_TILES_LENGTH * TILE_SIZE;

            for (int x = -(NUM_TILES_LENGTH/2); x < (NUM_TILES_LENGTH/2); x++) {
                for (int z = -(NUM_TILES_LENGTH/2); z < (NUM_TILES_LENGTH/2); z++) {

                    Vector2 position = new Vector2((-x*TILE_SIZE)+WORLD_X,(-z*TILE_SIZE)+WORLD_Z);
                    Tile tile = new Tile(position, TILE_SIZE, noise, seeds);
                    tiles.Add(tile);
                }
            }
        }

        public void update()
        {
            if (hasWater) {
                GL.Begin(BeginMode.Quads);
                GL.Color4((double)5 / 255, (double)94 / 255, (double)227 / 255, 0.6);

                GL.Normal3(0, 1, 0);
                GL.Vertex3(WORLD_X, waterLevel, WORLD_Z);
                GL.Vertex3(WORLD_X-(TILE_SIZE*NUM_TILES_LENGTH), waterLevel, WORLD_Z);
                GL.Vertex3(WORLD_X-(TILE_SIZE*NUM_TILES_LENGTH), waterLevel, WORLD_Z-(TILE_SIZE*NUM_TILES_LENGTH));
                GL.Vertex3(WORLD_X, waterLevel, WORLD_Z-(TILE_SIZE*NUM_TILES_LENGTH));

                GL.End();
            }

            foreach (Tile tile in tiles) { 
                tile.render();
            }
        }
    }
}
