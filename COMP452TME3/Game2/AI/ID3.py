import math

from MultiDecision import MultiDecision

# Splits a set of examples by the given attribute, and returns the new sets
def splitByAttribute(examples, attribute):
    sets = {}

    for example in examples:
        # Move the example into the set map by it's attribute value
        # (so, an example where hungry = true will go into a different set
        # that one where hungry = false
        if not sets.has_key(example.getValue(attribute)):
            sets[example.getValue(attribute)] = []
        sets[example.getValue(attribute)].append(example)
    return sets

# Return the value of base 2 log x
def log2(x):
    if x == 0:
        return 0
    return math.log(x) / math.log(2)

# Get the entropy (amount of knowledge) in a set of examples
def entropy(examples):
    exampleCount = len(examples)

    # If there are none in the set, exit.
    if exampleCount == 0:
        return 0

    # Store action counts here
    actionTallies = {}

    for example in examples:
        # Add this example's action to the action tally
        if actionTallies.has_key(example.action):
            actionTallies[example.action] += 1
        else:
            actionTallies[example.action] = 1

    # Count the number of actions
    actionCount = len(actionTallies.keys())

    # Exit if there aren't any
    if actionCount == 0:
        return 0

    entropy = 0

    for actionTally in actionTallies.values():
        # lower the entropy for each action found
        proportion = float(actionTally) / float(exampleCount)
        entropy -= proportion * log2(proportion)

    return entropy

# Return the accumulated entropy of a set of examples
def entropyOfSets(sets, exampleCount):
    e = 0

    for set in sets.values():
        # Lower the total entropy by the entropy of this set
        proportion = len(set) / exampleCount
        e -= proportion * entropy(set)
    return e

# Build a decision tree from the given examples
def makeTree(examples, attributes, decisionNode):

    # Initial entropy
    initialEntropy = entropy(examples)

    # No entropy, so stop
    if initialEntropy <= 0: 
        return

    exampleCount = len(examples)

    # Store information on which attribute is best to split on
    bestInformationGain = 0
    bestSplitAttribute = None
    bestSets = None

    # Test each attribute to find the best to split on
    for attribute in attributes:
        # Try to split on that attribute
        sets = splitByAttribute(examples, attribute)
        # Get the total entropy of the split sets
        overallEntropy = entropyOfSets(sets, exampleCount)
        # Found out how much info we gained by comparing the split sets entropy
        # to the original
        informationGain = initialEntropy - overallEntropy

        # If we gained more info than the best, this is now the new best
        if informationGain > bestInformationGain:
            bestInformationGain = informationGain
            bestSplitAttribute = attribute
            bestSets = sets

    # This decision tree node will now use the best split attribute that we found.
    # (The value of this attribute is what will determine which child to use)
    decisionNode.testAttribute = bestSplitAttribute

    # Remove the best attribute from the set
    newAttributes = [ v for v in attributes if not v == bestSplitAttribute ]

    # Create daughter nodes for this decision
    if bestSets != None:
        for set in bestSets.values():
            # Get the value to give this daughter node, for testing that it is the one to use for a decision
            attributeValue = set[0].getValue(bestSplitAttribute)

            # Create the daughter. Store the example for lookup of the action
            daughter = MultiDecision()
            daughter.examples = set
        
            # Attach the daughter node to the parent, then recursively continue building the tree for each daughter
            decisionNode.daughterNodes[attributeValue] = daughter
            makeTree(set, newAttributes, daughter)