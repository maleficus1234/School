import pygame
import sys
from Connect4 import Connect4
from AI import AI

pygame.init()

# Set the window size
size = width, height = 64*7, 64*7

# Create the pygame window
screen = pygame.display.set_mode(size)

pygame.display.set_caption("Connect4")

# Create the game and AI player
connect4 = Connect4()
ai = AI(connect4)

while True:

    # Quit if the user hits the X button
    for event in pygame.event.get():
        if event.type == pygame.QUIT: sys.exit()

        # Keep going as long as there's no winner
        if not connect4.win:
            # Mouse button event handling
            if event.type == pygame.MOUSEBUTTONUP:
                pos = event.pos
                button = event.button

                # Determine which column to drop a chip in
                tilePos = (pos[0]/64, pos[1]/64)
        
                # If a move is possible, drop the chip there
                if connect4.spots[tilePos[0]][0] == 0:
                    connect4.addChip(tilePos[0], False)

                    # Score the game to check for win, or so it's scored
                    # for the AI
                    connect4.heuristicScore()

                    # Did we win?
                    if connect4.win:
                        pygame.display.set_caption("Game won by white")
                    # No? Ok, AI's turn.
                    else:
                        # Make the move
                        ai.play()
                        # Score the game, check for a win
                        connect4.heuristicScore()
                        if connect4.win:
                            pygame.display.set_caption("Game won by black")


    # Clear the screen to black
    screen.fill([125,125,125])

    connect4.draw(screen)

    pygame.display.flip()  
    pygame.time.wait(100)