using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace infiniteTerrain.Game.Terrain
{
    public class Chunk
    {
        public static int NUM_TILES_LENGTH = 2;
        public static int TILE_SIZE = 50;

        List<Tile> tiles = new List<Tile>();
        float WORLD_X, WORLD_Z;

        public Chunk(float rx, float rz, ImprovedNoise noise, List<int> seeds, bool hasWater, float waterLevel) {

            WORLD_X = rx * NUM_TILES_LENGTH * TILE_SIZE;
            WORLD_Z = rz * NUM_TILES_LENGTH * TILE_SIZE;

            for (int x = -(NUM_TILES_LENGTH/2); x < (NUM_TILES_LENGTH/2); x++) {
                for (int z = -(NUM_TILES_LENGTH/2); z < (NUM_TILES_LENGTH/2); z++) {

                    Vector2 position = new Vector2((-x*TILE_SIZE)+WORLD_X,(-z*TILE_SIZE)+WORLD_Z);
                    Tile tile = new Tile(position, TILE_SIZE, noise, seeds, hasWater, waterLevel);
                    tiles.Add(tile);
                }
            }
        }

        public void update()
        {
            foreach (Tile tile in tiles)
            {
                tile.render();
            }
        }
    }
}
