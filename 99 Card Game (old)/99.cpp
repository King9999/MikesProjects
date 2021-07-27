/* 99.cpp 
Author: Mike Murray (mike_murray56@hotmail.com)

//The main program is defined in this file.  It handles all of the gameplay. */

#include "Deck.h"
#include "Card.h"
#include <conio.h>

int main()
{
	char selection;
	bool endGame = false;
	bool reShuffle = false;

	Deck d; 
	d.getPlayers();
	d.Shuffle();

	while (endGame != true)
	{
		if (reShuffle == true)
		{
			d.getPlayers();
			d.Shuffle();
		}

		d.Play();
		
		getch();
		system("cls");
		
		cout << "Play again? (Y/N): ";
		cin >> selection;
		if (selection == 'y' || selection == 'Y')
			reShuffle = true;
		else
		{
			cout << "See ya!\n";
			endGame = true;
		}
	} //while
		return 0;
}
