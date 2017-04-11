import pygame
import sys

from Tiles import Swamp, Grass, Open, Block
from TileMap import TileMap
from Common import AStar

# Initialize pygame
pygame.init()
pygame.font.init()

# Set the window size
size = width, height = 32*16, 32*16

# Create the pygame window
screen = pygame.display.set_mode(size)

pygame.display.set_caption("Path finder")

# Font for rendering text
font = pygame.font.SysFont("consolas", 10)

# Create the tiles
tileMap = TileMap()

# Set a default start and end to 0,0 and 15,15
start = tileMap.tiles[0][0]
end = tileMap.tiles[15][15]

path = None

# Draw the tiles and path, if there is one
def drawTiles(path):
    tileMap.draw(screen, font)

    if path != None:
        last = path[0]
        for tile in path:
            pygame.draw.line(screen, (255,0,0), (last.x+16,last.y+16), (tile.x+16, tile.y+16))
            last = tile 
    pygame.display.flip()  

while True:
    # Clear screen to black
    screen.fill([0,0,0])

    # Poll for pygame events
    for event in pygame.event.get():
        if event.type == pygame.QUIT: sys.exit()

        # Mouse button event handling
        if event.type == pygame.MOUSEBUTTONUP:
            pos = event.pos
            button = event.button
            tilePos = (pos[0]/32, pos[1]/32)
            # Check if click is inside the map
            if pos[0] < 32 * 16 and pos[1] < 32 * 16:

                # Left mouse button rotates through tile types
                if button == 1:
                    tile = None
                    if type(tileMap.tiles[tilePos[0]][tilePos[1]]) is Open:
                        tile = Grass()
                    if type(tileMap.tiles[tilePos[0]][tilePos[1]]) is Grass:
                        tile = Swamp()
                    if type(tileMap.tiles[tilePos[0]][tilePos[1]]) is Swamp:
                        tile = Block()
                    if type(tileMap.tiles[tilePos[0]][tilePos[1]]) is Block:
                        tile = Open()
                    tile.x = (pos[0]/32) * 32
                    tile.y = (pos[1]/32) * 32
                    tileMap.tiles[pos[0]/32][pos[1]/32] = tile

                # Middle mouse button sets the start position
                if button == 2:
                    start = tileMap.tiles[tilePos[0]][tilePos[1]]

                # Right mouse button sets the end position
                if button == 3:
                    end = tileMap.tiles[tilePos[0]][tilePos[1]]

            # Clear all pathing knowledge recorded in the tiles
            tileMap.clearKnowledge()
            # Setup the path finder and take the first step.
            aStar = AStar(tileMap.tiles, start, end)
            path = aStar.takeStep()
            # Draw the path so far
            drawTiles(path)

            # If there's more pathing to be done, keep stepping and build the path
            # until it's done: whether a path is found or not.
            # Setting up the pathfinder this way means that it's processing could
            # be spread across multiple frames.
            if path != None:
                while aStar.done == False:
                    pygame.time.wait(10)
                    path = aStar.takeStep()
                    drawTiles(path)
            if path == None:
                pygame.display.set_caption("No path found")
            else:
                pygame.display.set_caption("Path found")
                 
    drawTiles(path)
    

    

