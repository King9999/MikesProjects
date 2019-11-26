/* This sets up a level editor. */





//by default, the entire screen is filled with dirt tiles.
var col = floor(room_width / TILE_SIZE);
var row = floor(room_height / TILE_SIZE);
var global.currentTile = 0;     //the tile to draw after clicking mouse;
var global.tiles = 0;           //holds all tile constants
var global.tileIndex = 0;
var global.playerOnMap = false; //ensures that only one player tile can be on the screen
global.levelArray = 0;           //used to iterate through the grid
global.inverseArray = 0;        //inverted level data.
global.isInverted = false;      //controls how level is drawn, and affects collision.

//array setup
global.tiles[0] = TILE_FORE;
global.tiles[1] = TILE_BACK;
global.tiles[2] = TILE_FORE_INV;    //in the inverted world, 0 blocks the player instead of 1
global.tiles[3] = TILE_BACK_INV;
global.tiles[4] = TILE_PLAYER;
global.tiles[5] = TILE_EXIT;


//map setup
global.levelGrid = ds_grid_create(room_width / TILE_SIZE, room_height / TILE_SIZE);


ds_grid_add_region(global.levelGrid, 0, 0, ds_grid_width(global.levelGrid) - 1, 
    ds_grid_height(global.levelGrid) - 1, TILE_FORE);

//draw tiles
for (var i = 0; i < ds_grid_width(global.levelGrid); i++)
{
    for (var j = 0; j < ds_grid_height(global.levelGrid); j++)
    {
        if (j < ds_grid_height(global.levelGrid) / 2)
        {
            global.levelArray[i, j] = instance_create(i * TILE_SIZE, j * TILE_SIZE, tile2Obj);
            global.inverseArray[i, j] = instance_create(i * TILE_SIZE, j * TILE_SIZE, tile1Obj);
        }
        else
        {
            global.levelArray[i, j] = instance_create(i * TILE_SIZE, j * TILE_SIZE, tile1Obj);
            global.inverseArray[i, j] = instance_create(i * TILE_SIZE, j * TILE_SIZE, tile2Obj);
        }
        
    }
}

//if (!global.loadFileFlag)
/*{
    for (var i = 0; i < row; i++)
    {
        for (var j = 0; j < col; j++)
        {
            global.tileArray[i, j] = instance_create(j * TILE_SIZE, i * TILE_SIZE, tile1Obj);
            global.editorArray[i, j] = TILE_FORE;
        }
    }
}*/

//var file = get_save_filename("*.txt", "");

