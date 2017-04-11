from Slug import Slug
import pygame

# A global copy of the railgun sound for reuse
sound = None

# A railgun: handles firing of slugs and audio
class Railgun(object):
    """description of class"""

    # Constructor: Railgun needs to know about it's owner so it knows the location and
    # orientation of slugs that it spawns
    def __init__(self, owner):
        self.owner = owner

        # Load the railgun sound if it hasn't already been
        global sound
        if sound == None:
            sound = pygame.mixer.Sound("Content/Railgun.wav")
            sound.set_volume(0.15)

        self.sound = sound

    # Fire the railgun: we need the entity list so that slug entities can be added
    def fire(self, entities):
        # Create a slug and set it to the position and orientation of the parent
        slug = Slug()
        slug.forward =self.owner.forward.copy()
        slug.position = self.owner.position.copy()
        entities.append(slug)

        # Play the railgun sound
        self.sound.stop()
        self.sound.play()
        