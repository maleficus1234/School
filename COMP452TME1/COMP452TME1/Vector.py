import math

# A simple 2D vector class: I whipped this up because pygame's vector class is surprisingly
# buggy. Fortunately I had some vector code kicking around that I was able to port to Python
# in short order.
class Vector(object):

    # The x and y components
    x = 0
    y = 0

    # Construct a vector that defaults to 0,0
    def __init__(self, x = 0, y = 0):
        self.x = x
        self.y = y

    # Returns the dot product of two vectors
    @staticmethod
    def dot(vector1, vector2):
        return (vector1.x * vector2.x) + (vector1.y * vector2.y)

    # Returns the angle between two vectors.
    @staticmethod
    def angle(vector1, vector2):
        sqr1 = math.sqrt((vector1.x * vector1.x) + (vector1.y * vector1.y))
        sqr2 = math.sqrt((vector2.x * vector2.x) + (vector2.y * vector2.y))
        dot = Vector.dot(vector1, vector2)
        cosA = dot/(sqr1 * sqr2)
        cosA = min(cosA, 1)
        cosA = max(cosA, -1)
        return math.acos(cosA)

    # Returns the cross product of two vectors.
    @staticmethod
    def cross(vector1, vector2):
        return (vector1.x*vector2.y)-(vector1.y*vector2.x)

    # Rotate the vector by the given angle, in radians.
    def rotate(self, angle):
        x = self.x * math.cos(angle) - self.y * math.sin(angle)
        y = self.x * math.sin(angle) + self.y * math.cos(angle)
        self.x = x
        self.y = y

    # Override the multiply operator to scale a vector with a float
    def __mul__(self, n):
        return Vector(self.x * n, self.y * n)

    # Override the divide operator to scale a vector with a float
    def __div__(self, n):
        return Vector(self.x / n, self.y / n)

    # Override the add operator to add two vectors
    def __add__(self, n):
        return Vector(self.x + n.x, self.y + n.y)

    # Override the sub operator to subtract two vectors
    def __sub__(self, n):
        return Vector(self.x - n.x, self.y - n.y)

    # Return the length of the vector
    def length(self):
        return math.sqrt(self.x * self.x + self.y * self.y)

    # Return a normalized version of the vector
    def normalized(self):
        length = self.length()
        x = self.x / length
        y = self.y / length
        return Vector(x,y)

    # Return a copy of the vector
    def copy(self):
        return Vector(self.x, self.y)