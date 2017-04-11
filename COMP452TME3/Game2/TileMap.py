import pygame
import random

from Tiles import Grass, Poison, Water, Food, Tile

# Container to manage the creation, storage, and management of tiles
class TileMap(object):
    """description of class"""

    # Construct a new tile map
    def __init__(self):
        self.tiles = [[0 for x in range(16)] for y in range(16)] 

        # Fill the map with random food, water, and grass tiles.
        # I generate poison separately so I can manage their number
        # more closely.
        for x in range(0,16):
            for y in range(0,16):
                # Make the tiles closest to home grass
                if x < 8 and y < 8:
                    self.tiles[x][y] = Grass()
                else:
                    self.tiles[x][y] = self.randomTile()
                self.tiles[x][y].x = x * 32
                self.tiles[x][y].y = y * 32

        # Create three randomly positioned poison tiles
        for i in range(0, 6):
            x = random.randint(0, 15)
            y = random.randint(0, 15)
            self.tiles[x][y] = Poison()
            self.tiles[x][y].x = x * 32
            self.tiles[x][y].y = y * 32

        # Make sure that the home tile is grass
        self.tiles[0][0] = Grass()

    # Render each tile, without pathfinding info
    def draw(self, screen, font):
        for x in range(0, 16):
            for y in range(0, 16):
                self.tiles[x][y].draw(screen, font, False)

    # Return a random Food, Water, or Grass tile
    def randomTile(self):
        r = random.uniform(0, 1.0)
        if r > 0.90:
            return Food()
        elif r > 0.80:
            return Water()
        else:
            return Grass()