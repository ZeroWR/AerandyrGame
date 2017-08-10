/// @description Insert description here
// You can write your code in this editor
halfwidth = sprite_width / 2;
halfheight = sprite_height / 2;
barheight = 10;
draw_healthbar(x, y - barheight, x + sprite_width, y, (Health / MaxHealth) * 100, c_black, c_red, c_green, 0, true, true );

draw_text(x, y + halfheight, "ENEMY");