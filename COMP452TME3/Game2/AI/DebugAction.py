
# An action that outputs text when triggered: I used this during early testing
class DebugAction:

    def __init__(self, text):
        self.text = text

    def act(self, text):
        print self.text