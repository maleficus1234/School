from Entity import Entity
import pygame
from Steering import Seek, Arrive, Wander

# Keep a copy of the saucer sprite here so it is only loaded once
masterSprite = None
laserSound = None

# An alien flying saucer entity: uses the wander behavior for random UFO-ish
# movement unless it's outside a range of it's target, in which case it seeks.
# Does not use a constraint: it can move in any direction. It's a UFO after all :)
class Saucer(Entity):

    # Constructor: accepts the UFO's target
    def __init__(self, target):
        # Call the entity constructor
        super(Saucer, self).__init__()

        self.target = target

        # Load the saucer sprite if it hasn't already been
        global masterSprite
        if masterSprite == None:
            masterSprite = pygame.image.load("Content/saucer.png")

        global laserSound
        if laserSound == None:
            laserSound = pygame.mixer.Sound("Content/laser.wav")
            laserSound.set_volume(0.15)
        self.laserSound = laserSound

        self.sprite = masterSprite

        # Set the maxSpeed and maxRotation to a high value: UFOs are fast!
        self.maxSpeed = 10
        self.maxRotation = 1

        # Add a wander steering behavior
        self.wander = Wander(self)
        self.steerings.append(self.wander)

        # Add a seek behavior: in this case it only works when greater than a distance
        # from the target. While outside that range it will override the wander behavior
        self.seek = Seek(self)
        self.seek.seekIfFurther = True
        self.seek.seekIfFurtherRange = 300
        self.steerings.append(self.seek)

    def update(self, entities, center):
        # Call Entity.update()
        super(Saucer, self).update(entities, center)

        # If we're in range of the target, reduce it's health
        if (self.target.position - self.position).length() < 150:
            self.target.health -= 1
            self.laserSound.play()

    # An override of draw: we want to draw a laser when in range
    def draw(self, screen, center):
        super(Saucer, self).draw(screen,center)

        start = [self.position.x - center.x, self.position.y - center.y]
        end =  [self.target.position.x - center.x, self.target.position.y - center.y]
        if (self.target.position - self.position).length() < 150:
            pygame.draw.line(screen, [255,0,0], start, end)

    # Called if a railgun slug hits the saucer
    def railgunHit(self):
        # Play the sound of the slug impact
        self.impactSound.stop()
        self.impactSound.play()
        
        # Reduce the health of the saucer
        self.health -= 10

        # If health hits zero, set the entity to dead and play an explosion sound
        if self.health <= 0:
            self.alive = False
            