import random
from AStar import AStar

# Action that pathfinds to a food tile randomly chosen from the ones
# the ant knows about.
class AStarToFood:

    def act(self, ant):

        # Select a random food tile from the ones it knows about.
        ran = random.randint(0, len(ant.knownFood)-1)
        randomFood = ant.knownFood[ran]

        # Build a path to that tile
        aStar = AStar(ant.tiles, ant.tiles[ant.x][ant.y], randomFood)
        path = aStar.takeStep()
        if path != None:
            while aStar.done == False:
                path = aStar.takeStep()
        ant.path = path