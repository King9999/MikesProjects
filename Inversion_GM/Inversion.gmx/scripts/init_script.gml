//sets up the gameScreen.

var readInverted = false;
//global.levelArray = 0;
global.inverseArray = 0;
global.levelInverted = false;
global.shadowCreated = false;   //prevents the game from making more than one of these.
maxLevels = 6;
global.currentLevel = 0;        //iterator for the fileList linked list.  


//check for any gamepads
global.padDetected = false;
pads = gamepad_get_device_count();

var k = 0;
while (k < pads && !global.padDetected)
{
    if (gamepad_is_connected(k))
    {
        global.padDetected = true;
    }
    else
    {
        k++;
    } 
}
  

//locate all levels in directory
global.fileList = ds_list_create();
var file = working_directory + "level1.txt";

var i = 1;
while (i <= maxLevels)
{
    ds_list_add(global.fileList, working_directory + "level" + string(i) + ".txt");
    i++;
}


if (file != "")
{
    
    //load file           
    var fileName = file_text_open_read(file);
    var i = 0;
    var j = 0;
    
    //capture level dimensions
    var roomWidth = file_text_read_real(fileName);
    show_debug_message("Showing width " + string(roomWidth)); 
    var roomHeight = file_text_read_real(fileName);
    show_debug_message("Showing grid value " + string(roomHeight)); 
    
    global.mapGrid = ds_grid_create(floor(roomWidth / TILE_SIZE), floor(roomHeight / TILE_SIZE));
    global.inverseGrid = ds_grid_create(floor(roomWidth / TILE_SIZE), floor(roomHeight / TILE_SIZE));
    
    while (!file_text_eof(fileName))
    {
        if (!readInverted)
        {
            if (i < ds_grid_width(global.mapGrid)) //if true, begin drawing inverted level.
            {   
                if(!file_text_eoln(fileName))
                {

                     ds_grid_set(global.mapGrid, i, j, file_text_read_real(fileName));                                                        
                     j++;
                }
                else
                {
                    file_text_readln(fileName);
                    j = 0;
                    i++;
                }
            }
            else
            {
                readInverted = true;
                show_debug_message("Reading inverted level now!"); 
                file_text_readln(fileName);
                j = 0;
                i = 0;
                
            }
        }
        else    //reading the inverted level now
        {
            if(!file_text_eoln(fileName))
            {
                
                 ds_grid_set(global.inverseGrid, i, j, file_text_read_real(fileName));                                     
                 j++;

            }
            else
            {
                file_text_readln(fileName);
                j = 0;
                i++;
            }
        }
    }        
    file_text_close(fileName);
}

room_set_width(gameScreen, roomWidth);
room_set_height(gameScreen, roomHeight);
room_goto(gameScreen);


