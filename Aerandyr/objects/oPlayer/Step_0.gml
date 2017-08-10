/// @description Player Movement and Collisions

// Get Player Input
key_left = keyboard_check(ord("A"));
key_right = keyboard_check(ord("D"));
key_jump = keyboard_check_pressed(vk_space);

//We don't want to move if we're attacking.
if(is_attacking)
{
	//show_debug_message("Step returning because we're attacking");
	return;
}

// Calculate Movement
var move = key_right - key_left;

if move != 0 {
	hsp += move*accel;
	hsp = clamp(hsp, -walksp, walksp);
} else {
	hsp = lerp(hsp, 0, frict);
}

vsp = vsp + ((is_wall_sliding) ? sliding_gravity : grv);

// Jumping
if 
(
	(key_jump) &&
	(
		place_meeting(x,y+1,oWall) ||
		(is_wall_sliding)
	)
)
{
	vsp = -15;
	if(is_wall_sliding)
	{
		//Jump away from the wall
		hsp -= (15 * move);
	}
}

horizontal_collision = false;
// Horizontal Collision
if (place_meeting(x+hsp,y,oWall))
{
	while (!place_meeting(x+sign(hsp),y,oWall))
	{
		x = x + sign(hsp);
	}
	hsp = 0;
	horizontal_collision = true;
}

x = x + hsp;

vertical_collision = false;
// Vertical Collision
if (place_meeting(x,y+vsp,oWall))
{
	while (!place_meeting(x,y+sign(vsp),oWall))
	{
		y = y + sign(vsp);
	}
	vsp = 0;
	vertical_collision = true;
}

is_wall_sliding = (horizontal_collision == true && vertical_collision == false && vsp > 0.0);

y = y + vsp;

// Player Animation
if (!place_meeting(x,y+1,oWall))
{
	sprite_index = sPlayerJ;
	if (sign(vsp) > 0) sprite_index = sPlayerF; else sprite_index = sPlayerJ; 
} 
else
{

	//If we pressed attack and our current sprite isn't the attack sprite, OR
	//the attack is done playing: play it again.
	if 
	(
		keyboard_check_pressed(ord("J")) && 
		(
			sprite_index != sPlayerA || 
			image_index > (image_number - 1)
		)	
	)
	{
		show_debug_message("Attack pressed.  Current animation frame: " + string(image_index) + " out of " + string(image_number - 1));
		image_index = 0; //Reset the animation frames, so we're sure to start the animation from the beginning.
	    sprite_index = sPlayerA;
		is_attacking = true;
	}
	else
	{
		if (hsp == 0)
		{
			sprite_index = sPlayer;
		}
		else
		{
			sprite_index = sPlayer;
		}
	}
}

if (hsp != 0) image_xscale = sign(hsp);

if ((abs(hsp) > 0.5) && (vsp == 0) && (is_attacking == false))
{
	sprite_index = sPlayerR;
}


