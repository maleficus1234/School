import random

# The AI opponent for a Connect4 game
class AI:

    # Create an AI for the given connect4 game
    def __init__(self, connect4):
        self.connect4 = connect4

    # Play the AI turn
    def play(self):
        # Create a copy of the current status of the game, to use as the root of a move predicion tree.
        root = self.connect4.copy()
        
        # Create a movement prediction tree of depth 4 (3 plus the root) and for the black, AI, player
        self.createMoves(root, True, 3)

        # Run the alpha-beta pruned minimax algorithm to find the best predicted state
        a = -float("inf")
        b = float("inf")
        bestState = self.minimax(root, 3, a, b, True)

        # Determine the move that would potentially lead to that state
        bestMove = self.getMove(root, bestState)

        # Make the move
        self.connect4.addChip(bestMove.move, True)

    # Creates a tree of game states with predicted moves, with the first move indicated by isBlack
    def createMoves(self, root, isBlack, depth):
        # The tree is the desired depth: exit
        if depth == 0:
            return
        
        # Add all possible moves to the current node of the tree
        root.children = []
        for i in range(0, 7):
            child = root.copy()
            
            child.parent = root
            if child.addChip(i, isBlack):
                root.children.append(child)
                # Record some information both for debugging and for best move evaluation
                child.depth = depth
                child.isBlack = isBlack
                child.move = i
                # Estimate the score
                child.heuristicScore()
                # Recursively find moves for all child states
                self.createMoves(child, not isBlack, depth -1)
      
    # Alpha-beta pruned minimax to find best game state      
    def minimax(self, current, depth, a, b, maximize):

        # if the current game state is a win, or we've hit the end of the tree, return
        if depth == 0 or current.win:
            return current

        # If maximize is true, we're evaluating the AI move
        if maximize:
            v = -float("inf")
            bestMove = None
            for child in current.children:
                node = self.minimax(child, depth - 1, a, b, False)
                # After a painful amount of experimentation, I found I got best results
                # by minimaxing on the difference between the black and white scores.
                # In other words, we want to maximize the amount that the AI's score is
                # greater than that of the player's
                score = node.blackScore - node.whiteScore
                if score > v:
                    v = score
                    bestMove = node
                # Alpha-beta pruning: skip any moves that can't possibly be better
                if v > a:
                    a = v
                if b <= a:
                    break
            return bestMove
        # If maximize if false, we're evaluating the player move
        else:
            v = float("inf")
            bestMove = None
            for child in current.children:
                node = self.minimax(child, depth - 1, a, b, True)
                # As with maximize, use the difference between black and white scores.
                # In this case the player would want to be the difference to be as small as possible:
                # whether the lowest positive number if the AI is leading, or lowest negative number if 
                # the player is winning.
                score = node.blackScore - node.whiteScore
                if score < v:
                    v = score
                    bestMove = node
                # Alpha-beta pruning: skip any moves that can't possibly be better
                if v < b:
                    b = v
                if b <= a:
                    break
            return bestMove

    # Walk up through the tree from the given node to find the move that leads to it from the root.
    def getMove(self, root, leaf):
        current = leaf
        while current.parent != root:
            current = current.parent
        return current