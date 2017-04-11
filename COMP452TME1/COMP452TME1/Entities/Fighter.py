from Entity import Entity
from Railgun import Railgun
from Steering import Seek
from pygame import mouse
from Vector import Vector
import pygame
from MouseTarget import MouseTarget
from Steering import Arrive
from Steering import RocketConstraint

# The player ship sprite: keep a single copy around for reuse
masterSprite = None

# The player ship entity
class Fighter(Entity):

    # Constructor
    def __init__(self):
        # Call the Entity constructor
        super(Fighter, self).__init__()

        # Load the fighter sprite if it hasn't been already
        global masterSprite
        if masterSprite == None:
            masterSprite = pygame.image.load("Content/fighter.png")

        # Set the entity's sprite
        self.sprite = masterSprite

        # Create a railgun: handles spawning of slugs and audio
        self.railgun = Railgun(self)

        # Set the maximum speed of the player ship
        self.maxSpeed = 3

        # Set a target or the entity to follow: in this case the mouse cursor, and have Seek
        # and Arrive steering behaviors use it.
        self.target = MouseTarget()
        self.mouseSeek = Seek(self)
        self.steerings.append(self.mouseSeek)
        self.arrive = Arrive(self)
        # Radius of acceptance for arrival
        self.arrive.radius = 10
        self.steerings.append(self.arrive)

        # We want to constrain the player ship to only move forward: otherwise you get
        # strange looking sideways sliding effects
        self.actuator = RocketConstraint()

    # Update the player ship
    def update(self, entities, center):
        # Update the mouse cursor target to the current mouse position
        self.target.update(center)

        # Call Entity.update()
        super(Fighter, self).update(entities, center)

        # If the player hit the left mouse button, fire the railgun
        if mouse.get_pressed()[0]:
            self.railgun.fire(entities)