from Common import Tile

# A low cost tile with food
class Food(Tile):

    def __init__(self):
        super(Food, self).__init__("Content/food.png", 1)
