# An example of a set of states and the action to take
class Example:

    # Create an example with the given action from the given map of states
    def __init__(self, action = None, attributes = {}):
        self.action = action
        self.attributes = attributes

    # Get the value of a state attribute for this example
    def getValue(self, attribute):
        if self.attributes.has_key(attribute):
            return self.attributes[attribute]
        else:
            return None