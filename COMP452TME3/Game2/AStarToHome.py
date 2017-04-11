from AStar import AStar

# Action that creates a path to the home tile.
class AStarToHome:

    def act(self, ant):
        # Create a path to the home node at 0,0
        aStar = AStar(ant.tiles, ant.tiles[ant.x][ant.y], ant.tiles[0][0])
        path = aStar.takeStep()
        if path != None:
            while aStar.done == False:
                path = aStar.takeStep()
        ant.path = path