from Common import Tile

# Tile that kills if entered. It has a high pathfinding cost to minimize the
# change of ants pathing onto it while returning home: it seems enough of them
# already die while moving randomly.
class Poison(Tile):
    """description of class"""

    def __init__(self):
        super(Poison, self).__init__("Content/poison.png", 500)
