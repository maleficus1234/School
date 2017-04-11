from Common import Tile

# A blocking, inaccessible, tile
class Block(Tile):
    """description of class"""

    def __init__(self):
        super(Block, self).__init__("Content/block.png", -1)
