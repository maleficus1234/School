from Entity import Entity
import pygame
from Steering import Seek
from Steering import Arrive
from BehindTarget import BehindTarget
from Steering import RocketConstraint

# Store a copy of the sprite here for reuse
masterSprite = None

# A civilian transport entity
class Transport(Entity):

    # Constructor: parent is the entity it is to follow
    def __init__(self, parent):
        self.parent = parent
        # Call the entity constructor
        super(Transport, self).__init__()

        # Load the transport sprite if it hasn't already been.
        global masterSprite
        if masterSprite == None:
            masterSprite = pygame.image.load("Content/transport.png")
        self.sprite = masterSprite

        # Create a target that positions itself behind the parent, so it follows
        # behind the parent.
        self.behindTarget = BehindTarget(parent)
        self.target = self.behindTarget

        # Create a seek behavior that uses the position behind the parent as it's target
        self.targetSeek = Seek(self)
        self.steerings.append(self.targetSeek)

        # Create an arrive behavior using the posiion behind the parent as it's target
        self.targetArrive = Arrive(self)
        self.steerings.append(self.targetArrive)

        # We want to constrain the transport to only move forward, otherwise you get strange
        # looking sliding behavior
        self.actuator = RocketConstraint()

        # Set the maximum speed and rotation of the transport
        self.maxSpeed = 3
        self.maxRotation = 0.05

        

    # Update the transport
    def update(self, entities, center ):
        # Update the target relative to the parent entity
        self.behindTarget.update()
        # Call Entity.update()
        super(Transport, self).update(entities, center)

        if self.health <= 0:
            self.alive = False