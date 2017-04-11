import random
from AStar import AStar

# Action that pathfinds to a water tile randomly chosen from the ones
# the ant knows about.
class AStarToWater:


    def act(self, ant):
        # Select a random water tile from the ones it knows about
        ran = random.randint(0, len(ant.knownWater)-1)
        randomWater = ant.knownWater[ran]

        # Create a path to that tile.
        aStar = AStar(ant.tiles, ant.tiles[ant.x][ant.y], randomWater)
        path = aStar.takeStep()
        if path != None:
            while aStar.done == False:
                path = aStar.takeStep()
        ant.path = path