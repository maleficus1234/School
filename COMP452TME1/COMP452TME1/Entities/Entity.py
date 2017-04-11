import pygame
from pygame import mouse

import operator

from Kinematic import Kinematic
from pygame import draw
from Vector import Vector
from MouseTarget import MouseTarget
from Steering import Seek
from Steering import Output

import math

# Sounds potentially used by all entities: railgun slug impact, and explosion
impactSound = None
boomSound = None

# Base class for ingame entities: basically anything requiring special behavior such as
# steering. Derived from Kinematic, which has position and orientation information. 
class Entity(Kinematic):

    # Create a new entity instance
    def __init__(self):
        super(Entity, self).__init__()

        # Load the impact and boom sounds if they haven't already been.
        global impactSound
        if impactSound == None:
            impactSound = pygame.mixer.Sound("Content/impact.wav")
            impactSound.set_volume(0.15)
        self.impactSound = impactSound

        global boomSound
        if boomSound == None:
            boomSound = pygame.mixer.Sound("Content/boom.wav")
            boomSound.set_volume(0.15)
        self.boomSound = boomSound

        # As a superclass, do not specify a sprite yet. That is the job of subclasses.
        self.sprite = None

        # Set a default forward direction (the orientation of the kinematic)
        self.forward = Vector(1,0)

        # Set a default maximum rotation per update.
        self.maxRotation = 0.1

        # Set a default position
        self.position = Vector(500,500)

        # Set a default maximum speed
        self.maxSpeed = 1

        # Don't set an actuator (movement constraint): that is the job of subclasses
        self.actuator = None

        # Create a list to hold multiple steering behaviors
        self.steerings = []

        # Set a default health for the entity
        self.health = 50

        # Alive by default
        self.alive = True

    # Update entity kinematic info
    def update(self, entities, center):
        # Create a steering output holder
        output = Output()
        
        # Accumulate the output of each steering behavior, if any
        for steering in self.steerings:
            steering.getOutput(output)

        # Apply the constraints provided by an actuator, if there is one
        if self.actuator != None:
            self.actuator.constrain(self, output)

        # Apply the final output to the kinematic information
        self.forward.rotate(output.rotation)
        self.position += output.velocity

    # Draw the entity on the screen, with the screen centered at center (so that the player
    # ship is always at the center of the screen
    def draw(self, screen, center):
        # Get the angle of the forward vector relative to a vector pointing up
        angle = Vector.angle(self.forward, Vector(0,-1))
        # Check if the angle to the up vector is left or right of the forward vector, and
        # change it's sign to reflect that
        dir = Vector.cross(self.forward, Vector(0,-1))
        if dir < 0:
            angle = -angle

        # Create a rotated version of the entity's sprite, for rendering
        renderSprite = pygame.transform.rotate(self.sprite, math.degrees(angle))
        # Get the pygame rect of the sprite to adjust where it's rendered
        rect = renderSprite.get_rect()

        # Set the center of the sprite's rect to the entity's object relative to the center of the screen
        rect.center = [self.position.x - center.x, self.position.y - center.y]

        # Draw the sprite!
        screen.blit(renderSprite, rect)

