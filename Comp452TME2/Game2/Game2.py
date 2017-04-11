import pygame
import sys
from TileMap import TileMap
from Ant import Ant

# Initialize pygame
pygame.init()
pygame.mixer.init()
pygame.font.init()

# Query the user as to the number of ants to start with.
# Constrain it to 1-100 to prevent ridiculousness.
numAnts = -1
while numAnts == -1:
    try:
        i=int(raw_input('Enter starting number of ants:'))
        if i > 0 and i <= 100:
            numAnts = i
        else:
            print "Number is limited to 1-100 to prevent overloading"
    except ValueError:
        print "Not a number"

# Set the window size
size = width, height = 32*16, 32*16

# Create the pygame window
screen = pygame.display.set_mode(size)

pygame.display.set_caption("Ants!")

# Font for rendering text
font = pygame.font.SysFont("consolas", 10)

# Create the map of tiles
tileMap = TileMap()

# Store all ants, living or dead, here.
ants = []

# Create the user-requested number of ants.
for i in range(0, numAnts):
    ants.append(Ant(tileMap.tiles))

while True:
    alive = 0
    dead = 0
    for ant in ants:
        if ant.state == 3:
            dead += 1
        else:
            alive += 1
    # Show the number of living ants in the window caption
    pygame.display.set_caption(str(len(ants)) + " Ants! " + str(alive) + " alive " + str(dead) + " dead ")

    # Clear the screen to black
    screen.fill([0,0,0])

    # Quit if the user hits the X button
    for event in pygame.event.get():
        if event.type == pygame.QUIT: sys.exit()

    # Render all tiles
    tileMap.draw(screen, font)

    # update each ant
    for ant in ants:
        # If the ant is telling u to spawn a new ant, do so, and set spawnNew to False again.
        if ant.spawnNew:
            ant.spawnNew = False
            ants.append(Ant(tileMap.tiles))
        ant.update()

    # Draw the ants
    for ant in ants:
        ant.draw(screen, font)

    pygame.display.flip()  

    pygame.time.wait(300)