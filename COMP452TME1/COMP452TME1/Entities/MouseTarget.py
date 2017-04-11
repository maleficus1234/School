from pygame import mouse

from Vector import Vector

# Acts a steering behavior target that positions itself using the mouse cursor position
class MouseTarget(object):
 
    # Constructor
    def __init__(self):
        # Just set an initial value for the target position
        self.position = Vector()

    # Update the target using current mouse cursor coordinates
    def update(self, center):
        self.position = center + Vector(mouse.get_pos()[0], mouse.get_pos()[1])