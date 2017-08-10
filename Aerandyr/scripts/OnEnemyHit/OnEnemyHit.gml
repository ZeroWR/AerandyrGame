///OnEnemyHit(argument0, argument1);

enemy = argument0;
damage = argument1;

oldHealth = enemy.Health;

enemy.Health -= damage;

objname = object_get_name(enemy);
outMessage = "Enemy " + objname + "hit for " + string(damage) + " damage.  Health was " + string(oldHealth) + " is now " + string(enemy.Health);
show_debug_message(outMessage);

if(enemy.Health <= 0)
{
	OnEnemyKilled(enemy);
}