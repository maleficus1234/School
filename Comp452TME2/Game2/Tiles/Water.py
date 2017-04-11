from Common import Tile

# A high cost water tile
class Water(Tile):
    """description of class"""

    def __init__(self):
        super(Water, self).__init__("Content/water.png", 10)
