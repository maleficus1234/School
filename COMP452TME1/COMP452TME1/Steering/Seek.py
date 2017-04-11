from Output import Output
from Vector import Vector

# Steering behavior that rotates the entity towards the target
# and moves towards it.
class Seek(object):

    # Constructor: entity is the entity to which the behavior is applied.
    def __init__(self, entity):
        self.entity = entity

        # If True, only use this behavior if we're outside a given range.
        # This is only used by saucers: outside this radius, seek will override
        # the saucer's wander behavior.
        self.seekIfFurther = False
        self.seekIfFurtherRange = 50

    # Modify the given steering output with this behavior.
    def getOutput(self, output):
        # Get a vector from the parent entity to the target
        direction = self.entity.target.position - self.entity.position

        # If seekIfFurther is set, and we're within range, just return the output
        # set by the wander behavior.
        if self.seekIfFurther:
            if direction.length() < self.seekIfFurtherRange:
                return output

        # Calculate the velocity from the forwrad vector and maximum speed.
        direction = direction.normalized()
        output.velocity = direction * self.entity.maxSpeed

        # Determine whether to turn right or left.
        rotationDir = Vector.cross(self.entity.forward, direction)
        # Get the angle between the forward vector and the vector to the target
        angleTarget = Vector.angle(self.entity.forward, direction)
        # Change the sign of the angle to match turning right or left.
        if(rotationDir > 0):
            toRotate = angleTarget
        else:
            toRotate = -angleTarget

        # Cap the rotation angle to the maximum possible
        if abs(toRotate) > self.entity.maxRotation:
            if toRotate > 0:
                toRotate = self.entity.maxRotation
            else:
                toRotate = -self.entity.maxRotation

        # apply the rotation to the output.
        output.rotation = toRotate