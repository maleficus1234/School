
# An actuator that constrains the steering output to only move forward: in this
# case like there's a big rocket behind it. Without this constraint, you get strange
# looking sliding behaviors.
class RocketConstraint(object):

    # Constrain to only go in the direction of the entity's forward vector
    def constrain(self, entity, output):
        speed = output.velocity.length()
        output.velocity = entity.forward * speed
