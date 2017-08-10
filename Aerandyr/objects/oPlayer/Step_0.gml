/// @description Player Movement and Collisions

// Get Player Input
key_left = keyboard_check(ord("A"));
key_right = keyboard_check(ord("D"));
key_jump = keyboard_check_pressed(vk_space);

//We don't want to move if we're attacking.
if(is_attacking)
{
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

next_animation = undefined;

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
	next_animation = sPlayerToJ;
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

if(next_animation == undefined)
{
	// Player Animation
	if (!place_meeting(x,y+1,oWall))
	{
		next_animation = sPlayerJ;
		if (sign(vsp) > 0)
		{
			if(sprite_index == sPlayerFromJ && image_index >= (image_number - 1)) 
				next_animation = sPlayerF; 
			else if(sprite_index != sPlayerF)
				next_animation = sPlayerFromJ;
			else
				next_animation = sPlayerF; //This should technically never happen, but I'd rather it go straight to this than...wtf ever else.
		}
		else 
			next_animation = sPlayerJ; 
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
			next_animation = sPlayerA;
			is_attacking = true;
			show_debug_message("Doing enemy collision check");
			//Do enemy collision check
			halfwidth = sprite_width / 2;
			halfheight = sprite_height / 2;
			coll_x1 = x;
			coll_y1 = y - halfheight;
			coll_x2 = x + halfwidth;
			coll_y2 = coll_y1 + sprite_height;
			//Not going to lie:  I have no idea how the fuck this works, because I never take into account which way the player is facing. :|
			inst = collision_rectangle(coll_x1, coll_y1, coll_x2, coll_y2, oTestEnemy, false, true);
			if(inst != noone)
			{
				OnEnemyHit(inst, 25);
			}
			//DEBUG: Uncomment these if you need to debug the weapon collision
			//last_weapon_coll_x1 = coll_x1;
			//last_weapon_coll_y1 = coll_y1;
			//last_weapon_coll_x2 = coll_x2;
			//last_weapon_coll_y2 = coll_y2;
		}
		else
		{
			if (hsp == 0)
			{
				next_animation = sPlayer;
			}
			else
			{
				next_animation = sPlayer;
			}
		}
	}
}

if (hsp != 0) 
	image_xscale = sign(hsp);

if ((abs(hsp) > 0.5) && (vsp == 0) && (is_attacking == false))
{
	next_animation = sPlayerR;
}

if(sprite_index != next_animation)
{
	show_debug_message("Changing animation to " + sprite_get_name(next_animation));
	sprite_index = next_animation;
}


