
# A node in a decision tree that chooses between multiple options
# It selects the daughter appropriat for the given state
class MultiDecision:

    # Create the decision node
    def __init__(self):
        # Dictionary of daughter nodes, which are keyed on their attribute value (True or False)
        self.daughterNodes = {}
        # The attribute used in this decision: "hungry", "knows food", etc
        self.testAttribute = 0
        # The set of examples involved in forming this decision node
        self.examples = {}

    # Get the daughter matching the given attribute value.
    def getBranch(self, value):
        return self.daughterNodes[value]

    # Make a decision: returns a decision node, or the action to take if it's terminal.
    def makeDecision(self, knowledge):
        if knowledge.has_key(self.testAttribute):
            branch = self.getBranch(knowledge[self.testAttribute])
            return branch.makeDecision(knowledge)
        return self.examples[0]