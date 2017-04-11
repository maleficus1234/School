import random

# A wander steering behavior that turns randomly for a wobbling movement.
class Wander(object):

    # Constructor: entity is the entity to which the behavior is applied.
    def __init__(self, entity):
        self.entity = entity

    # Modify the given steering output
    def getOutput(self, output):
        # Set the velocity using the entity's forward vector and maximum speed
        output.velocity = self.entity.forward * self.entity.maxSpeed
        # Rotate the entity by a random amount.
        output.rotation = random.uniform(-1.0, 1.0) * self.entity.maxRotation