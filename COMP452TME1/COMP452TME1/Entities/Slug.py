from Entity import Entity
import pygame
from Steering import Dumb

# Keep a copy of the sprite here so it is only loaded once
masterSprite = None

# A railgun slug/bullet entity. Just fires in a straight line with no steering
class Slug(Entity):

    # Constructor
    def __init__(self):
        # Call the entity constructor
        super(Slug, self).__init__()

        # Load the sprite if it hasn't already been.
        global masterSprite
        if masterSprite == None:
            masterSprite = pygame.image.load("Content/slug.png")
        self.sprite = masterSprite

        # Set the speed of the slug to a high bullet-y speed
        self.maxSpeed = 20

        # Add a "dumbfire" behavior so that the slug keeps going in the direction
        # in which it is fired.
        self.steerings.append(Dumb(self))