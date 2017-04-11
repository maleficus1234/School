from Output import Output
from Vector import Vector

# Arrive steering behavior: comes to a stop within a radius of the target
class Arrive(object):

    # Constructor: entity is entity to which the behavior applies
    def __init__(self, entity):
        self.entity = entity

        # No target at first
        self.target = None

        # Set a default arrival radius
        self.radius = 30

        # Set a default desired time to arrival
        self.timeToTarget = 0.25

    # Accepts a steering behavior output and updates it.
    def getOutput(self, output):

        # Get a vector from the parent entity to the target
        toTarget = self.entity.target.position - self.entity.position

        # If we're already in the arrival radius, set velocity to zero and return
        if toTarget.length() < self.radius:
            output.velocity = Vector()
            return output

        # Set the velocity to that needed to arrive in the desired time
        output.velocity = toTarget / self.timeToTarget

        # Constrain to the max speed, if necessary
        if output.velocity.length() > self.entity.maxSpeed:
            output.velocity = output.velocity.normalized()
            output.velocity *= self.entity.maxSpeed

        return Output