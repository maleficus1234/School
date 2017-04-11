import pygame

from Common import AStar
import Common
import random

from Tiles import Food, Grass, Poison, Water

# Audio and sprites for ants
masterSprite = None
deadSound = None

# The four possible states of ant-dom.
SEEKINGFOOD = 0
RETURNINGFOOD = 1
SEEKINGWATER = 2
DEAD = 3

# Yup, it's an Ant.
# Seriously though, it's also a finite state machine.
class Ant(object):
    """description of class"""

    # Initialize the ant with a starting state to seek food, and load resources if necessary
    def __init__(self, tiles):
        self.tiles = tiles

        self.x = 0
        self.y = 0

        self.state = SEEKINGFOOD

        global masterSprite
        if masterSprite == None:
            masterSprite = pygame.image.load("Content/ant.png")
        self.texture = masterSprite

        global deadSound
        if deadSound == None:
            deadSound = pygame.mixer.Sound("Content/Wilhelm_Scream.ogg")
            deadSound.set_volume(0.15)

        self.path = None

        # If true, the game knows to spawn a new ant at this ant's location.
        self.spawnNew = False

    # Update the ant every frame.
    def update(self):

        # Is the ant alive and on a poison tile? Kill it.
        if type(self.tiles[self.x][self.y]) is Poison and self.state != DEAD:
            self.state = DEAD
            global deadSound
            deadSound.play()

        # Check the state of the ant ant handle accordingly

        # If the ant is dead, do nothing. Because it's dead :)
        # I don't actually delete the ant, so that the user can see where it died.
        if self.state == DEAD:
            return

        # The ant is currently looking for food.
        if self.state == SEEKINGFOOD:
            # Move to a random adjacent tile, with potentially fatal consequences.
            self.moveRandom()

            # Is the ant on a food tile?
            if type(self.tiles[self.x][self.y]) is Food:
                # Change state to returning food
                self.state = RETURNINGFOOD
                # Change the tile to grass, the food is gone.
                # (The assignment didn't seem clear on whether food should disappear, so
                # figured I'd do that)
                self.tiles[self.x][self.y] = Grass()
                self.tiles[self.x][self.y].x = self.x * 32
                self.tiles[self.x][self.y].y = self.y * 32
                # Create a path to the home node at 0,0
                aStar = AStar(self.tiles, self.tiles[self.x][self.y], self.tiles[0][0])
                path = aStar.takeStep()
                if path != None:
                    while aStar.done == False:
                        path = aStar.takeStep()
                self.path = path
            return

        # The ant is returning food. Don't bother checking for poison tiles:
        # the poison tile cost is high so the ant will never path over one.
        if self.state == RETURNINGFOOD:
            # Are we home?
            if self.x == 0 and self.y == 0:
                # Create a new ant
                self.spawnNew = True
                # Start looking for water
                self.state = SEEKINGWATER
            else:
                # We're not home, so follow the path
                self.x = self.path[-1].x/32
                self.y = self.path[-1].y/32
                self.path.remove(self.path[-1])
            return

        # The ant is looking for water.
        if self.state == SEEKINGWATER:
            # Move to a random adjacent tile
            self.moveRandom()
            # If we found water, start seeking food.
            if type(self.tiles[self.x][self.y]) is Water:
                self.state = SEEKINGFOOD
            return

    # Draw the ant, with some debug info showing the state of the ant.
    def draw(self, screen, font):
        rect = self.texture.get_rect()
        rect = rect.move(self.x * 32, self.y*32)
        screen.blit(self.texture, rect)

        # Indicate the current state of the ant
        s = ""
        if self.state == SEEKINGFOOD:
            s = "F"
        elif self.state == SEEKINGWATER:
            s = "W"
        elif self.state == DEAD:
            s = "X"
        elif self.state == RETURNINGFOOD:
            s = "R"
        label = font.render(s, 1, (255,255,255))
        screen.blit(label, (self.x*32+2, self.y*32))

    # Return a random tile from the set of neighbouring tiles.
    def moveRandom(self):
        neighbours = Common.getNeighbours(self.tiles, self.tiles[self.x][self.y])
        r = random.randint(0, len(neighbours)-1)
        self.x = neighbours[r].x/32
        self.y = neighbours[r].y/32

