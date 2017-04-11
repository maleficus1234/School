from Entity import Entity
import pygame

# Store the sprite here so it is only loaded once.
masterSprite = None

# An explosion entity. Doesn't do much other than live for a very short time.
class Splosion(Entity):

    # Constructor
    def __init__(self):
        # Call the entity constructor
        super(Splosion, self).__init__()

        # Load the sprite if it hasn't already been.
        global masterSprite
        if masterSprite == None:
            masterSprite = pygame.image.load("Content/splosion.png")

        self.sprite = masterSprite

        # A countdown until the explosion finishes
        self.ticks = 0

        self.boomSound.stop()
        self.boomSound.play()

    # Update the explosion entity
    def update(self, entities, center ):
        # Increase counter to the explosion completion
        self.ticks += 1
        # If the counter hits 5, delete the explosion
        if self.ticks >= 5:
            self.alive = False