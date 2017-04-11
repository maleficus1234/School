from AI import Example
from AI import MultiDecision
from AI import DebugAction

from MoveRandomly import MoveRandomly
from AStarToHome import AStarToHome
from Ant import Ant
from TileMap import TileMap
from AStarToFood import AStarToFood
from AStarToWater import AStarToWater

from AI import ID3

# "Trains" an ant by building a decision tree with knowledge of examples
# Ants will wander randomly if thirsty or hungry, if it know where to find water or food.
# Ants path to home if they find food
# Once the ant finds food or water, it will remember them and path to them next time it's hungry or thirsty.
def train(ant):
    # The attributes found in the examples
    attributes = ["knows food", "hungry", "carrying food", "thirsty", "knows water"]

    examples = []

    # Actions to take when the current state matches ones found while developing this program.
    # This was almost trial-and-error: any time the ant encountered a state it couldn't account for, I had to inspect
    # the state and add an appropriate example so that it would know how to handle it in the future.
    examples.append(Example(MoveRandomly(), {"hungry":True, "knows food":False, "carrying food": False, "thirsty": False, "knows water":False}))
    examples.append(Example(MoveRandomly(), {"hungry":True, "knows food":False, "carrying food": False, "thirsty": False, "knows water":True}))
    examples.append(Example(MoveRandomly(), {"hungry":False, "knows food":False, "carrying food": False, "thirsty": True, "knows water":False}))
    examples.append(Example(MoveRandomly(), {"hungry":False, "knows food":True, "carrying food": False, "thirsty": True, "knows water":False}))
    examples.append(Example(AStarToFood(), {"hungry":True, "knows food":True, "carrying food": False, "thirsty": False, "knows water":False}))
    examples.append(Example(AStarToFood(), {"hungry":True, "knows food":True, "carrying food": True, "thirsty": False, "knows water":False}))
    examples.append(Example(AStarToFood(), {"hungry":True, "knows food":True, "carrying food": False, "thirsty": False, "knows water":True}))
    examples.append(Example(AStarToHome(), {"hungry":False, "knows food":True, "carrying food": True, "thirsty": False, "knows water":False}))
    examples.append(Example(AStarToHome(), {"hungry":False, "knows food":False, "carrying food": True, "thirsty": False, "knows water":False}))
    examples.append(Example(AStarToHome(), {"hungry":False, "knows food":True, "carrying food": True, "thirsty": False, "knows water":True}))
    examples.append(Example(AStarToHome(), {"hungry":False, "knows food":False, "carrying food": True, "thirsty": False, "knows water":True}))
    examples.append(Example(AStarToWater(), {"hungry":False, "knows food":True, "carrying food": False, "thirsty": True, "knows water":True}))
    examples.append(Example(AStarToWater(), {"hungry":False, "knows food":False, "carrying food": False, "thirsty": True, "knows water":True}))

    # Build the tree
    ID3.makeTree(examples, attributes, ant.brain)


# Test code used while debugging
import pygame

pygame.mixer.init()

tileMap = TileMap()
ant = Ant(tileMap.tiles)
ant.x = 1
ant.y = 1
train(ant)

ant.status["hungry"] = False
ant.status["knows food"] = False
ant.status["carrying food"] = False
ant.status["thirsty"] = True
ant.status["knows water"] = False

ant.brain.makeDecision(ant.status).action.act(ant)

'''

examples = []

attributes = ["knows food", "hungry", "carrying food"]

# Ant is hungry
e1 = Example()
e1.action = DebugAction("Move randomly")
e1.attributes["hungry"] = "true"
e1.attributes["knows food"] = "false"
examples.append(e1)

e5 = Example()
e5.action = DebugAction("AStar to food")
e5.attributes["hungry"] = "true"
e5.attributes["knows food"] = "true"
examples.append(e5)


# Ant is not hungry, food is in tile
e2 = Example()
e2.action = DebugAction("Move to random tile")
e2.attributes["hungry"] = "false"
#e2.attributes["knows food"] = "true"
e2.attributes["carrying food"] = "false"
examples.append(e2)

# Ant is carrying food
e3 = Example()
e3.action = DebugAction("Return home")
e3.attributes["hungry"] = "false"
#e3.attributes["knows food"] = "true"
e3.attributes["carrying food"] = "true"
examples.append(e3)

status = {}
status["knows food"] = "true"
status["hungry"] = "true"
status["carrying food"] = "false"

treeRoot = MultiDecision()
ID3.makeTree(examples, attributes, treeRoot)

decision = treeRoot.makeDecision(status)
decision.action.act()'''