/// @description Create Player Variables

hsp = 0;
vsp = 0;
grv = 0.9;
accel = 1.2;
frict = .2;
walksp = 7;

is_attacking = false;
is_wall_sliding = false;

//Constants
sliding_gravity = grv / 3.0;

//Debug variables
last_weapon_coll_x1 = undefined;
last_weapon_coll_y1 = undefined;
last_weapon_coll_x2 = undefined;
last_weapon_coll_y2 = undefined;
