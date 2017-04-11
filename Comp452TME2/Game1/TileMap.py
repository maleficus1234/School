import pygame

from Common import Tile

from Tiles import Open

# Container class to create, store, and manage tiles
class TileMap(object):
    """description of class"""

    # Create a 16x16 set of tiles, all defaulting to Open.
    def __init__(self):
        self.tiles = [[0 for x in range(16)] for y in range(16)] 

        for x in range(0,16):
            for y in range(0,16):
                self.tiles[x][y] = Open()
                self.tiles[x][y].x = x * 32
                self.tiles[x][y].y = y * 32

    # Render the tiles and a grid to help mouse clicking
    def draw(self, screen, font):
        # Render the tiles, showing their recorded path info.
        for x in range(0, 16):
            for y in range(0, 16):
                self.tiles[x][y].draw(screen, font, True)
        # Render a grid over the tiles.
        for x in range(0, 16):
            for y in range(0, 16):
                pygame.draw.line(screen, [255,255,255], [x * 32, y*32], [x*32+32, y * 32])
                pygame.draw.line(screen, [255,255,255], [x * 32, y*32], [x*32, y * 32+32])
        pygame.draw.line(screen, [255,255,255], [0, 16*32], [16*32, 16*32])
        pygame.draw.line(screen, [255,255,255], [16*32, 0], [16*32, 16*32])

    # Clear all recorded pathfinding info from every tile.
    def clearKnowledge(self):
        for x in range(0, 16):
            for y in range(0,16):
                self.tiles[x][y].visited = False
                self.tiles[x][y].processed = False
                self.tiles[x][y].step = -1