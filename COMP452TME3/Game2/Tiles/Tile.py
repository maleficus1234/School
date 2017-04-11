import pygame

# Map for storage of tile sprites
tileTextures = {}

# Base class for tiles
class Tile(object):
    
    # Create a tile using the given texture, with the given pathfinding cost.
    # -1 cost indicates inaccessibility
    def __init__(self, textureFile, cost):
        # Load the sprite if it hasn't already been.
        if not tileTextures.has_key(textureFile):
            tileTextures[textureFile] = pygame.image.load(textureFile)
        self.texture = tileTextures[textureFile]
        self.cost = cost
        self.x = 0
        self.y = 0

        # Set default values for the user info
        # Whether this tile was visited at any point during pathfinding
        self.visited = False
        # Whether this node was considered as part of a path
        self.processed = False
        # At which step in the pathfinding this node was considered.
        self.step = -1

    # Render the tile. If debug is true, the tile will be rendered with pathfinding data.
    def draw(self, screen, font, debug = False):
        rect = self.texture.get_rect()
        rect = rect.move(self.x, self.y)
        screen.blit(self.texture, rect)

        if debug == True:
            if self.processed:
                label = font.render("p", 1, (0,0,0))
                screen.blit(label, (self.x+2, self.y))
            else:
                if self.visited:
                    label = font.render("v", 1, (0,0,0))
                    screen.blit(label, (self.x+2, self.y))

            if self.step != -1:
                label = font.render(str(self.step), 1, (0,0,0))
                screen.blit(label, (self.x+16, self.y))