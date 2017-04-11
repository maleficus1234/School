from Tile import Tile

# Tile that kills if entered. In TME2 it was a high cost, as far too many ants
# died from random wandering. I made it lower cost so ants can path through it:
# there's more population growth to cull.
class Poison(Tile):
    """description of class"""

    def __init__(self):
        super(Poison, self).__init__("Content/poison.png", 1)
