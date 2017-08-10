/// @description Debug Drawing Stuff
// You can write your code in this editor
if(last_weapon_coll_x1 != undefined)
{
	//show_debug_message("last_weapon_coll_x1: " + string(last_weapon_coll_x1) + "\nlast_weapon_coll_y1: " + string(last_weapon_coll_y1) + "\nlast_weapon_coll_x2: " + string(last_weapon_coll_x2) + "\nlast_weapon_coll_y2: " + string(last_weapon_coll_y2));
	draw_rectangle_color(last_weapon_coll_x1, last_weapon_coll_y1, last_weapon_coll_x2, last_weapon_coll_y2, c_aqua, c_aqua, c_aqua, c_aqua, false);
}

draw_text(x, y, "x(" + string(x) + "),y(" + string(y) + ")");