import pygame

from AStar import AStar
import random

from Tiles import Food, Grass, Poison, Water

from AI import MultiDecision

# Audio and sprites for ants
masterSprite = None
deadSound = None

# Yup, it's an Ant.
# No longer a finite state machine: it now uses a decision tree to decide actions
class Ant(object):
    """description of class"""

    # Initialize the ant with a starting state to seek food, and load resources if necessary
    def __init__(self, tiles):
        self.tiles = tiles

        self.x = 0
        self.y = 0

        global masterSprite
        if masterSprite == None:
            masterSprite = pygame.image.load("Content/ant.png")
        self.texture = masterSprite

        global deadSound
        if deadSound == None:
            deadSound = pygame.mixer.Sound("Content/Wilhelm_Scream.ogg")
            deadSound.set_volume(0.15)

        # The path that the ant is currently following, if any
        self.path = []

        self.alive = True

        # If true, the game knows to spawn a new ant at this ant's location.
        self.spawnNew = False

        # The ant's "brain" is the root of the decision tree
        self.brain = MultiDecision()

        # The current state of the ant. These are what allow the decision tree to
        # choose an appropriate course of action.
        self.status = {}
        self.status["hungry"] = True            # Ant is hungry
        self.status["knows food"] = False       # Ant doesn't know where food is to be found
        self.status["carrying food"] = False    # Ant is not currently carrying food
        self.status["thirsty"] = False          # Ant is not thirsty
        self.status["knows water"] = False      # And doesn't know where water is to be found

        # List of water and food tiles that the ant has found
        self.knownFood = []
        self.knownWater = []

    # Update the ant every frame.
    def update(self):

        # If the ant is in a poison tile and alive, kill it
        if type(self.tiles[self.x][self.y]) is Poison and self.alive == True:
            self.alive = False
            global deadSound
            deadSound.play()

        # If the ant is dead, do nothing.
        if not self.alive:
            return

        # Start by assessing the ant's situation to adjust it's status before making a decision

        # If the ant is in a tile with food, it is no longer hungry, and starts
        # carrying food. Remember this food file.
        if type(self.tiles[self.x][self.y]) is Food:
            if self.knownFood.count(self.tiles[self.x][self.y]) == 0:
                self.knownFood.append(self.tiles[self.x][self.y])
                self.status["knows food"] = True
            if self.status["hungry"] == True:
                self.status["carrying food"] = True
                self.status["hungry"] = False
         
        # If the ant is in a tile with water, it is no longer thirsty, but becomes hungry
        # Remember this water tile
        if type(self.tiles[self.x][self.y]) is Water:
            if self.knownWater.count(self.tiles[self.x][self.y]) == 0:
                self.knownWater.append(self.tiles[self.x][self.y])
                self.status["knows water"] = True
            if self.status["thirsty"] == True:
                self.status["thirsty"] = False 
                self.status["hungry"] = True 
        
        # Are we home and carrying food?
        if self.x == 0 and self.y == 0 and self.status["carrying food"] == True:
            # Create a new ant
            self.spawnNew = True
            # Start looking for water
            self.status["carrying food"] = False
            self.status["thirsty"] = True

        # If there are still nodes in the ant's path, keep following it.
        if len(self.path) > 0:
            self.x = self.path[-1].x/32
            self.y = self.path[-1].y/32
            self.path.remove(self.path[-1])
        else:
            # Otherwise, make a decision by passing the ant's state to the decision tree.
            self.brain.makeDecision(self.status).action.act(self)

    # Draw the ant, with some debug info showing the state of the ant.
    def draw(self, screen, font):
        rect = self.texture.get_rect()
        rect = rect.move(self.x * 32, self.y*32)
        screen.blit(self.texture, rect)

    # Return a random tile from the set of neighbouring tiles.
    def moveRandom(self):
        neighbours = AStar.getNeighbours(self.tiles, self.tiles[self.x][self.y])
        r = random.randint(0, len(neighbours)-1)
        self.x = neighbours[r].x/32
        self.y = neighbours[r].y/32

