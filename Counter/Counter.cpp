// Counter.cpp by Mike Murray
//This is an all-purpose program used for quick calculations for games.

#include "stdafx.h"
#include "iostream"

int _tmain(int argc, _TCHAR* argv[])
{
	float seconds = 1.0f;	//number of seconds.
	int playerLevel = 50;

	//countdown
	for (int i = 0; i < playerLevel; i++)
	{
		std::cout << seconds << std::endl;
		seconds -= 0.02;
		
	}
	
	system("pause");

	return 0;
}

