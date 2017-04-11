from Vector import Vector

class Output(object):
    """description of class"""

    def __init__(self):
        self.velocity = Vector()
        self.rotation = 0

    def __add__(self, n):
        output = Output()
        output.velocity = self.velocity + n.velocity
        output.rotation = self.rotation + n.rotation
        return output