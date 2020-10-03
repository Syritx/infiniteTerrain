using System;
using System.Collections.Generic;
using infiniteTerrain.Game.Rendering;
using OpenTK;

namespace infiniteTerrain.Game.Terrain
{
    public class Chunk
    {
        public static int NUM_TILES_LENGTH = 10;
        public static int TILE_SIZE = 10;
        List<Tile> tiles = new List<Tile>();

        float WORLD_X, WORLD_Z;
        Camera camera;

        public Chunk(float rx, float rz, ImprovedNoise noise, int seed, Camera camera) {
            this.camera = camera;

            WORLD_X = rx * NUM_TILES_LENGTH * TILE_SIZE;
            WORLD_Z = rz * NUM_TILES_LENGTH * TILE_SIZE;

            for (int x = -(NUM_TILES_LENGTH/2); x < (NUM_TILES_LENGTH/2); x++) {
                for (int z = -(NUM_TILES_LENGTH/2); z < (NUM_TILES_LENGTH/2); z++) {

                    Vector2 position = new Vector2((-x*TILE_SIZE)+WORLD_X,(-z*TILE_SIZE)+WORLD_Z);
                    Tile tile = new Tile(position, TILE_SIZE, noise, seed);
                    tiles.Add(tile);
                }
            }
        }

        public void update()
        {
            foreach (Tile tile in tiles)
                tile.render();    
        }
    }
}
