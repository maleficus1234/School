import pygame
import random

from pygame import Rect

# Creates and renders the starscape background.
class Background(object):

    # Constructor: draws the background surface with the given dimensions.
    def __init__(self, size):
        # Create the pygame surface.
        self.rect = Rect(0, 0, size[0], size[1])
        self.surface = pygame.Surface(size)

        # Fill in with black.
        self.surface.fill([0,0,0])

        # Draw 1000 randomly positioned stars
        for i in range(0,1000):
            x = random.randint(0, size[0])
            y = random.randint(0, size[1])
            self.surface.set_at([x, y], [255,255,255,255])

    # Blit the surface to the screen.
    def blit(self, screen):
        screen.blit(self.surface, self.rect)