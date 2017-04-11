
# A Dumb-fire steering behavior that just keeps the entity moving in a straight line.
class Dumb(object):

    # Constructor: entity is entity to which the behavior applies
    def __init__(self, entity):
        self.entity = entity

    # Accept an output behavior and update it
    def getOutput(self, output):
        # Just go in the entity's forward direction
        output.velocity = self.entity.forward * self.entity.maxSpeed
