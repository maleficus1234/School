import sys, pygame, pygame._view
from Entities import Fighter
from Entities import Transport
from Entities import Slug
from Vector import Vector
from Entities import Saucer
from Background import Background
from Entities import Splosion
import random

# Start up pygame and it's audio system.
pygame.init()
pygame.mixer.init()
pygame.font.init()

# Set the window size
size = width, height = 1024, 768

# Create the pygame window
screen = pygame.display.set_mode(size)

# Font for rendering text
font = pygame.font.SysFont("consolas", 50)

# Create a starscape background surface
background = Background(size)

# The list of all entities currently in the game.
entities = []

transports = []

# Create the player fighter entity
fighter = Fighter()
fighter.position = Vector(200,200)
entities.append(fighter)

# Create transports randomly positioned around the player
t = Transport(fighter)
t.position = Vector(fighter.position.x + random.randint(-400, 400), fighter.position.y + random.randint(-400, 400))
entities.append(t)
transports.append(t)
for i in range(0,8):
    t = Transport(t)
    t.position = Vector(fighter.position.x + random.randint(-400, 400), fighter.position.y + random.randint(-400, 400))
    entities.append(t)
    transports.append(t)


clock = pygame.time.Clock()
remaining = 100000
pygame.display.set_caption("Time remaining: " + str(remaining))
sinceLastSaucer = 5000

while 1:

    # Sleep for 30 milliseconds
    clock.tick(30)
    remaining -= 30
    sinceLastSaucer -= 30
    

    # Poll for pygame events
    for event in pygame.event.get():
        if event.type == pygame.QUIT: sys.exit()

    # Check for entities that need to be removed
    toRemove = []
    for entity in entities:
        if entity != fighter:
            # If entity is flagged as dead, remove it.
            if not entity.alive:
                if not entity in toRemove:
                    toRemove.append(entity)
                # Create an explosion if a transport
                if type(entity) is Transport:
                    splode = Splosion()
                    splode.position = entity.position
                    entities.append(splode)
            # If entity is more than a given distance from the player, remove it.
            if (entity.position - fighter.position).length() > 2000:
                if not entity in toRemove:
                    toRemove.append(entity)

        # If the entity is a slug, see if it hits a saucer
        if type(entity) is Slug:
            for other in entities:
                if type(other) is Saucer:
                    # Check if the slug is within the radius of the saucer
                    if (entity.position - other.position).length() < 16:
                        # Inform the saucer of the bad news.
                        other.railgunHit()
                        # If the saucer is now dead, spawn an explosion entity
                        if other.alive == False:
                            splode = Splosion()
                            splode.position = other.position
                            entities.append(splode)
                        # Delete the slug
                        if not entity in toRemove:
                            toRemove.append(entity)

    # Remove all entities in the removal list.
    for entity in toRemove:
        entities.remove(entity)
        if type(entity) is Transport:
            transports.remove(entity)

    # Update transport targets to whatever is above it in the list.
    # Thus, tranports will get a new target (whatever is above in the list)
    # when it's target is destroyed
    for t in transports:
        # First transport follows the player ship
        if transports.index(t) == 0:
            t.behindTarget.parent = fighter
        else:
            t.behindTarget.parent = transports[transports.index(t) - 1]

    # Spawn a randomly positioned saucer at intervals
    # Subsequent saucers should spawn at random length intervals
    # Saucers should target a random transport
    if sinceLastSaucer <= 0 and len(transports) > 0:
        saucer = Saucer(transports[random.randint(0, len(transports)-1)])
        saucer.position = Vector(fighter.position.x + random.randint(-1000, 1000), fighter.position.y + random.randint(-1000, 1000))
        entities.append(saucer)
        sinceLastSaucer = random.randint(500, 4000)

    
    

    # Update all entities: make sure to take into account the player position
    # Though, only if the game is still on!
    if remaining > 0:
        center = fighter.position - Vector(512, 384)
        for e in entities:
            # If the saucer's target has been destroyed, get a new target
            if type(e) is Saucer:
                if e.target.alive == False:
                    if len(transports) > 0:
                        if len(transports) > 1:
                            e.target = transports[random.randint(0, len(transports)-1)]
                        else:
                            e.target = transports[0]
            e.update(entities, center)

    # Draw the background
    background.blit(screen)

    # Draw each entity, taking into account the player position, such that the player
    # is always at the center of the screen.
    for e in entities:
        e.draw(screen, center)

    # Render game status info
    if len(transports) > 0:
        if remaining > 0:
            pygame.display.set_caption("Time remaining: " + str(remaining/1000))
            label = font.render("Time remaining: " + str(remaining/1000), 1, (255,255,0))
            screen.blit(label, (20, 20))

        else:
            pygame.display.set_caption("Game won! :)")
            label = font.render("Game won! :)", 1, (255,255,0))
            screen.blit(label, (20, 20))

    else:
        pygame.display.set_caption("Game lost! :(")
        label = font.render("Game lost! :(", 1, (255,255,0))
        screen.blit(label, (20, 20))

        remaining = 0
    
    pygame.display.flip()