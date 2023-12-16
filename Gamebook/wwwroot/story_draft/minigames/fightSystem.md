### Weapons
- knife - damage: 10, energy: 15
- baseball - damage: 25, energy: 20
- handgun - damage: 25, energy: 5
- gun - damage: 40, energy: 10

player: health, energy
enemy: health

### adjustable variables:

- playerEnergyCost
- playerDamage
- playerMaxHealth
- playerMaxEnergy
- enemyMaxHealth

**PLAYER attack:**

- text: you hit enemy / you missed
- player energy- (playerEnergyCost)
- enemy health- (playerDamage)

**PLAYER defend:**

- text: your defence has held / your defence collapsed
- energy+
- health+

**ENEMY attack:**

- text: opponent hit you
- player heath- (random 1 to 10)

**ENEMY defend:**

- text: opponent has dodget the attack
- block incoming damage

examples:
you hit enemy -10hp -5energy
you missed - 5energy
Not enough energy to attack
your defence has held +5hp +5energy
your defence collapsed
opponent hit you -10hp
opponent has dodget the attack
