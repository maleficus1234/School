from Vector import Vector

# An object that updates it's position to a given distance behind a parent entity.
# Used to have one entity follow another.

class BehindTarget(object):

    # Default target sets it's position to 40 pixels behind the given parent
    def __init__(self, parent):
        self.position = Vector()
        self.parent = parent
        self.distance = 20

    # Update to a new position relative to the parent
    def update(self):
        self.position = self.parent.position - (self.parent.forward * self.distance)