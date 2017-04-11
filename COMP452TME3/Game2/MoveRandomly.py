from AStar import AStar
import random

# Action that moves to a random tile
class MoveRandomly:

    def act(self, ant):
        neighbours = AStar.getNeighbours(ant.tiles, ant.tiles[ant.x][ant.y])
        r = random.randint(0, len(neighbours)-1)
        ant.x = neighbours[r].x/32
        ant.y = neighbours[r].y/32