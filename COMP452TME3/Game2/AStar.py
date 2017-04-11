from Tiles import Tile
import math

# Performs interuptable A* pathfinding. Because the path is build in steps,
# with separate invocations of takeStep, this pathfinder could be used across
# multiple frames.
class AStar:

    # Setup the A* algorithm using the given tiles, and start/end positions
    def __init__(self, tiles, start, end):
        # The closed node list
        self.closed = []
        # The open node list
        self.open = []
        # The starting tile
        self.start = start
        # The ending tile
        self.end = end

        # The set of tiles to search though
        self.tiles = tiles

        # Map storage for costs from the start to a given node
        self.startToNodeCosts = {}
        # The cost to get to the start node from the start node is zero
        self.startToNodeCosts[self.start] = 0
        # Map storage for costs to get from start to end via the given node
        self.startToEndCosts = {}
        # The starting cost to the end from the start node is purely a guess: heuristic
        self.startToEndCosts[self.start] = heuristicCost(self.start, self.end)

        # Map storage to look up where we came from, for each node
        self.nodesCameFrom = {}

        # Put the start node in the open list to get things going
        self.open.append(self.start)

        # Informs the user of this class whether the path has been completed or not. This
        # is necessary since the path building is interruptable: the path is built in steps.
        # Note that "done" being true doesn't necessary mean that a path was found: "done" states
        # are either a path found, or no possible path.
        self.done = False

        # The current step in the pathfinding: keep track of this so we can store that info in a tile
        # for display to the user
        self.step = 0

    # Performs another round of pathfinding. Each invocation returns the path built so far, or none
    # if there's no possible path. "done" gets set to true once there is a compete path, or no path
    # is found to be possible
    def takeStep(self):
        # Only proceed if there's a tile in the open list. If not, there's no possible path.
        if len(self.open) > 0:
            # Get the node with the lowest start-to-end path going through it.
            current = getLowestScore(self.startToEndCosts, self.open)
            # Flag it as processed, for the user to see.
            current.processed = True
            # Record what step it was processed in, for the user to see.
            current.step = self.step

            # Build the path-so-far by starting from the current node and walking
            # backwards using our record of where we came from to get to that node.
            path = [current]
            step = current
            while step in self.nodesCameFrom.keys():
                step = self.nodesCameFrom[step]
                path.append(step)
                # If the path so far ends at the end node, we're done!
                if current == self.end:
                    self.done = True

            # Move the current node from open to closed
            self.open.remove(current)
            self.closed.append(current)

            # Get the list of neighbours to which we can move from the current node
            neighbours = AStar.getNeighbours(self.tiles, current)
            for neighbour in neighbours:
                # Flag the neighbour as "visited" or inspected, for the user's info.
                neighbour.visited = True
                # Record the step at which the neighbour was visited, for the user's info.
                neighbour.step = self.step
                # If the neighbour is already in the closed list, skip it.
                if neighbour in self.closed:
                    continue

                # Get a score for the cost from the start to this neighbour, through the current node.
                testScore = self.startToNodeCosts[current] + neighbour.cost
                # Put the neighbour in the open list
                if not neighbour in self.open:
                    self.open.append(neighbour)

                # Record how we came to this neighbour node (from the current node)
                self.nodesCameFrom[neighbour] = current
                # Record the cost from the start to the neighbour, via the current
                self.startToNodeCosts[neighbour] = testScore
                # Record an estimate of the cost from the start to end, via the neighbour, using the heuristic.
                self.startToEndCosts[neighbour] = self.startToNodeCosts[neighbour] + heuristicCost(neighbour, self.end)

            self.step += 1
            return path

        # There's no possible path!
        self.done = True
        return []

    # Return a list of neighbours to the given tile. Inaccessible tiles are not included.
    @staticmethod
    def getNeighbours(tiles, tile):
        neighbours = []
        x = tile.x/32
        y = tile.y/32
        
        # Left
        if x > 0:
            if tiles[x-1][y].cost != -1: neighbours.append(tiles[x-1][y])
        # Right:
        if x < 15:
            if tiles[x+1][y].cost != -1: neighbours.append(tiles[x+1][y])
        # Above:
        if y > 0:
            if tiles[x][y-1].cost != -1: neighbours.append(tiles[x][y-1])
        # Below:
        if y < 15:
            if tiles[x][y+1].cost != -1: neighbours.append(tiles[x][y+1])
        # Above left:
        if x > 0 and y > 0:
            if tiles[x-1][y-1].cost != -1: neighbours.append(tiles[x-1][y-1])

        # Above right:
        if x < 15 and y > 0:
            if tiles[x+1][y-1].cost != -1: neighbours.append(tiles[x+1][y-1])

        # Below left:
        if x > 0 and y < 15:
            if tiles[x-1][y+1].cost != -1: neighbours.append(tiles[x-1][y+1])

        # Below right:
        if x < 15 and y < 15:
            if tiles[x+1][y+1].cost != -1: neighbours.append(tiles[x+1][y+1])

        return neighbours



# Returns the lowest cost tile in the list of tiles, using the given map of costs.
def getLowestScore(costs, tiles):
    lowScore = 10000
    lowTile = None
    for tile in tiles:
        if costs[tile] < lowScore:
            lowScore = costs[tile]
            lowTile = tile
    return lowTile

# Return an estimate of the cost to move between two nodes. For this assignment,
# I just used the distance between them multiplied by a cost of 3.
def heuristicCost(start, end):
    x = end.x/32 - start.x/32
    y = end.y/32 - start.y/32
    return math.sqrt(x*x + y*y) * 3

