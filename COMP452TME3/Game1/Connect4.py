import pygame

import math

# A Connect 4 game: holds and stores game state
class Connect4:

    # Create an empty connect 4 game
    def __init__(self):
        # Nobody has won yet!
        self.win = False
        # No parent game (will only have a parent if the game is in a tree)
        self.parent = None
        # Create all chip spots: 7x6
        self.spots = [[None for x in range(6)] for y in range(7)] 
        # All chip spots start empty
        for x in range(0, 7):
            for y in range(0, 6):
                self.spots[x][y] = 0

    # Draw the game
    def draw(self, screen):
        # Draw lines to show the columns
        for i in range(1, 8):
            pygame.draw.line(screen, (255,255,255), (i * 64, 64), (i*64, 7*64))

        # Draw the chips, if any
        for x in range(0, 7):
            for y in range(0, 6):
                if self.spots[x][y] == 1:
                    pygame.draw.circle(screen, (0,0,0), (x*64+32, y*64+32+64), 32)
                elif self.spots[x][y] == 2:
                    pygame.draw.circle(screen, (255,255,255), (x*64+32, y*64+32+64), 32)

    # Make a move on the given column: isBlack indicates black or white. Returns False if the move
    # is invalid
    def addChip(self, column, isBlack):
        # Check each spot in the column for a free spot, starting from the bottom. Because gravity.
        for i in range(5, -1, -1):
            if self.spots[column][i] == 0:
                # We found a free spot, so add the chip and return True
                if isBlack:
                    self.spots[column][i] = 1
                else:
                    self.spots[column][i] = 2
                return True
        # We didn't find a free spot, so return false
        return False

    # Returns a copy of the game for use in creating a tree of states
    def copy(self):
        copy = Connect4()
        for x in range(0, 7):
            for y in range(0, 6):
                copy.spots[x][y] = self.spots[x][y]

        return copy

    # Estimate the current scores of the game.
    # This was probably the hardest part of the assignment: first determining how to estimate
    # scores, but then deciding what scores were appropriate.
    def heuristicScore(self):
        self.blackScore = 0.0
        self.whiteScore = 0.0
        
        for color in range(1, 3):

            # Check for horizontal 4-in-a-rows, including potential ones
            for row in range(0, 6):
                for column in range(0, 4):
                    batch = []
                    batch.append(self.spots[column][row])
                    batch.append(self.spots[column+1][row])
                    batch.append(self.spots[column+2][row])
                    batch.append(self.spots[column+3][row])
                    self.scoreChips(batch, color)
            
            # Check for vertical 4-in-a-rows, including potential ones
            for column in range(0, 7):
                for row in range(0, 3):
                    batch = []
                    batch.append(self.spots[column][row])
                    batch.append(self.spots[column][row+1])
                    batch.append(self.spots[column][row+2])
                    batch.append(self.spots[column][row+3])
                    self.scoreChips(batch, color)
                 
            # Check for right diagonal 4-in-a-rows, including potential ones
            self.scoreRightDiagonal((0,2), color)    
            self.scoreRightDiagonal((0,1), color)     
            self.scoreRightDiagonal((0,0), color)  
            self.scoreRightDiagonal((1,0), color)    
            self.scoreRightDiagonal((2,0), color)   

            # Check for left diagonal 4-in-a-rows, including potential ones
            self.scoreLeftDiagonal((6,0), color)   
            self.scoreLeftDiagonal((5,0), color)   
            self.scoreLeftDiagonal((4,0), color)  
            self.scoreLeftDiagonal((6,1), color)  
            self.scoreLeftDiagonal((6,2), color)  


    # Check for right diagonal 4-in-a-rows, including potential ones
    def scoreRightDiagonal(self, spot, color):
        x = spot[0]
        y = spot[1]
        
        # Find each set of 4 spots along this diagonal, and score
        while y < 3 and x < 4:
            batch = []
            batch.append(self.spots[x][y])
            batch.append(self.spots[x+1][y+1])
            batch.append(self.spots[x+2][y+2])
            batch.append(self.spots[x+3][y+3])

            self.scoreChips(batch, color)
            y+=1
            x+=1

    # Check for left diagonal 4-in-a-rows, including potential ones
    def scoreLeftDiagonal(self, spot, color):
        x = spot[0]
        y = spot[1]
        
        # Find each set of 4 spots along this diagonal, and score
        while y < 3 and x > 2:
            batch = []
            batch.append(self.spots[x][y])
            batch.append(self.spots[x-1][y+1])
            batch.append(self.spots[x-2][y+2])
            batch.append(self.spots[x-3][y+3])

            self.scoreChips(batch, color)
            y+=1
            x-=1

    # Score a set of four chips. Scores are given for:
    # 1) 4 in a row
    # 2) 3 chips and an empty spot - next move could be a 4 in a row!
    # 3) 2 chips and two empty spots - could lead to #2
    # Trying to find scores for these that worked well was not fun.
    def scoreChips(self, batch, color):
        numFound = 0

        # Count the number of chips of the given color in this batch
        for chip in batch:
            if chip == color:
                numFound +=1
            elif chip != 0:
                numFound = 0
                break

        # It's a four-in-a-row. Score high and as a win
        if numFound == 4:
            if color == 1:
                self.blackScore += 100000
            else:
                self.whiteScore += 100000
            self.win = True

        # Give a game with 3s a modest score
        if numFound == 3:
            if color == 1:
                self.blackScore += 500
            else:
                self.whiteScore += 500

        # Give games with 2s a low score
        if numFound == 2:
            if color == 1:
                self.blackScore += 150
            else:
                self.whiteScore += 150

