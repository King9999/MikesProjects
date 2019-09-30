/* Deck.cpp 
Author: Mike Murray (mike_murray56@hotmail.com).

This class manipulates arrays of Card objects. */

#include <conio.h>
#include "Deck.h"
#include "Card.h"
#include "EasyRand.h"

const char KEY_DOWN = '1';
const char KEY_UP = '2';
const char KEY_ENTER = (char)13;	//'enter' key
const char KEY_QUIT = '3';			//use this key to give up

//will need this for pointing to the top of the card in the deck
int deckIndex = 0;

string winnerName;		//the name of the winner

//create the cards
Card cards[52] =
{
	Card(1, 1, "Ace of Hearts"), Card(2, 2, "2 of Hearts"), Card(3, 3, "3 of Hearts"),
	Card(4, 4, "4 of Hearts"), Card(5, 5, "5 of Hearts"), Card(6, 6, "6 of Hearts"),
	Card(7, 7, "7 of Hearts"), Card(8, 8, "8 of Hearts"), Card(9, 9, "9 of Hearts"),
	Card(10, 10, "10 of Hearts"), Card(11, 11, "Jack of Hearts"), Card(12, 12, "Queen of Hearts"),
	Card(13, 13, "King of Hearts"), Card(14, 1, "Ace of Diamonds"), Card(15, 2, "2 of Diamonds"),
	Card(16, 3, "3 of Diamonds"), Card(17, 4, "4 of Diamonds"), Card(18, 5, "5 of Diamonds"), 
	Card(19, 6, "6 of Diamonds"), Card(20, 7, "7 of Diamonds"), Card(21, 8, "8 of Diamonds"), 
	Card(22, 9, "9 of Diamonds"), Card(23, 10, "10 of Diamonds"), Card(24, 11, "Jack of Diamonds"),
	Card(25, 12, "Queen of Diamonds"), Card(26, 13, "King of Diamonds"), Card(27, 1, "Ace of Spades"),
	Card(28, 2, "2 of Spades"), Card(29, 3, "3 of Spades"), Card(30, 4, "4 of Spades"), 
	Card(31, 5, "5 of Spades"), Card(32, 6, "6 of Spades"), Card(33, 7, "7 of Spades"), 
	Card(34, 8, "8 of Spades"), Card(35, 9, "9 of Spades"), Card(36, 10, "10 of Spades"),
	Card(37, 11, "Jack of Spades"), Card(38, 12, "Queen of Spades"), Card(39, 13, "King of Spades"),
	Card(40, 1, "Ace of Clubs"), Card(41, 2, "2 of Clubs"), Card(42, 3, "3 of Clubs"), 
	Card(43, 4, "4 of Clubs"), Card(44, 5, "5 of Clubs"), Card(45, 6, "6 of Clubs"),
	Card(46, 7, "7 of Clubs"), Card(47, 8, "8 of Clubs"), Card(48, 9, "9 of Clubs"),
	Card(49, 10, "10 of Clubs"), Card(50, 11, "Jack of Clubs"), Card(51, 12, "Queen of Clubs"),
	Card(52, 13, "King of Clubs")
	
};


//create a new array that will contain randomly inserted cards
Card newDeck[52] =
{
	//the first n * 4 cards will go to the players, where n = # of players present
	Card(0,0,""), Card(0,0,""), Card(0,0,""), Card(0,0,""), Card(0,0,""),Card(0,0,""),
	Card(0,0,""), Card(0,0,""), Card(0,0,""), Card(0,0,""), Card(0,0,""), Card(0,0,""),
	Card(0,0,""), Card(0,0,""), Card(0,0,""), Card(0,0,""), Card(0,0,""), Card(0,0,""), 
	Card(0,0,""), Card(0,0,""), Card(0,0,""), Card(0,0,""), Card(0,0,""), Card(0,0,""),
	Card(0,0,""), Card(0,0,""), Card(0,0,""), Card(0,0,""), Card(0,0,""), Card(0,0,""),
	Card(0,0,""), Card(0,0,""), Card(0,0,""), Card(0,0,""), Card(0,0,""), Card(0,0,""),
	Card(0,0,""), Card(0,0,""), Card(0,0,""), Card(0,0,""), Card(0,0,""), Card(0,0,""),
	Card(0,0,""), Card(0,0,""), Card(0,0,""), Card(0,0,""), Card(0,0,""), Card(0,0,""), 
	Card(0,0,""), Card(0,0,""), Card(0,0,""), Card(0,0,"")
};


//This object is for the top card of the discard pile
Card discard = Card(0,0, "");

//And this object always points to the top card in the deck
Card nextCard = Card(0,0,"");


/** The Player struct represents the players.  Up to 4 players can play 99.
		name: player's name
		min: the first card in the player's hand
		max: the last card in the player's hand

  min and max correspond to the location of an object in the newDeck array.  All objects between
  min and max are reserved. */
struct Player
{
	string name;
	int min;	
	int max;
	bool isAI;	//true = player is CPU-controlled
};

Player player[4];


/**  ShowHand merely displays the current active player's hand. */
void Deck::ShowHand(int p)
{
	//show the player his/her hand
	for (int g = player[p].min; g <= player[p].max; g++)
	{
		cout << newDeck[g].getSuit() << endl;
		cout << "  ";
	}
}



/** Cards are taken from the 'cards' array and inserted into random locations in the 
	'newDeck' array. */
void Deck::Shuffle()
{
	cardCount = 52;

	//create an array to keep track of the cards that have already been inserted
	bool cardCheck[52];
	for (int j = 0; j < 52; j++)
		cardCheck[j] = false;


	//start randomizing the cards
	
	int num;

	EasyRandom r;
	r.SetTimerSeed();
	r.SetInterval(0, 51);

	while (cardCount > 0)
	{
		num = r.DrawRandomNumber();

		//check if the spot in the array has already been filled
		if (cardCheck[num] == true)
		{
			int OK = 0;
			int i = 0;

			//find a number that hasn't been used yet

			//search newDeck for a vacant spot
			while (OK != 1)
			{
				if (cardCheck[i] == false)
				{
					//found an empty space; insert card
					newDeck[i] = cards[cardCount-1];
					cardCheck[i] = true;
					OK = 1;
				}
				else
					i++;
			}
		}
		else
		{
			//insert the card in the current spot
			newDeck[num] = cards[cardCount-1];
			cardCheck[num] = true;
		}
		cardCount--;
	}


	cout << endl;


	//set cardCount to its original number minus the number of cards taken by players
	cardCount = 52 - deckIndex;

	//and then set nextCard to point to the next available card
	nextCard = newDeck[deckIndex];
}


void Deck::Draw(int y)
{
	//discard the card that was played...
	discard = newDeck[y];
	
	//...and draw a card from the deck
	newDeck[y] = nextCard;
	
	//point to next card
	deckIndex++;
	nextCard = newDeck[deckIndex];
	cardCount--;
}


/**Get the number of players.  MAX = 4, MIN = 2.  AI players can be chosen here. */
void Deck::getPlayers()
{
	numberOfPlayers = 1;
	numberOfAIPlayers = 0;
	string name;
	int setMin = 0;
	int	setMax = 3;

	while (numberOfPlayers <= 1 || numberOfPlayers > 4)
	{
		system("cls");
		cout << "How many players? (minimum 2 players): ";
		cin >> numberOfPlayers;

		if (numberOfPlayers <= 1 || numberOfPlayers > 4)
			;   //do nothing
		else
		{
			deckIndex = numberOfPlayers * 4;  //set deckIndex to a useful number

			cout << "How many AI players? (Enter 0 for none, 3 AI players MAX): ";
			cin >> numberOfAIPlayers;

			if (numberOfAIPlayers > 0 && numberOfAIPlayers < 4)
			{
				int humanPlayers = numberOfPlayers - numberOfAIPlayers;

				//get the human players first
				for (int f = 0; f < humanPlayers; f++)
					player[f].isAI = false;

				//now get the AI players
				for (int g = humanPlayers; g < numberOfPlayers; g++)
					player[g].isAI = true;
			}
			else	//all human players
			{
				for (int w = 0; w < numberOfPlayers; w++)
					player[w].isAI = false;
			}

			//enter names for each player			
			for (int i = 1; i <= numberOfPlayers; i++)
			{
				cout << endl;
				cout << "Enter a name for player " << i << ": ";
				cin >> name;
				player[i-1].name = name;

				//set the min and max for the player's hand
				player[i-1].min = setMin;
				setMin = setMin + 4;  //make proper adjustments
				player[i-1].max = setMax;
				setMax = setMax + 4;
			}
		}
	}
}


/** Control the turn order. */
int Deck::nextPlayer(int e)
{
	//go to the next player
	if (e == numberOfPlayers-1 && turnMod == 1)
		e = 0;
	else if (e == 0 && turnMod == -1)
		e = numberOfPlayers - 1;
	else
		//turn order is normal
		e = e + turnMod;

	return e;
}


/** Kick the current player (k) from the game. */
int Deck::BootPlayer(int k)
{
    //get rid of the player
	if (k == numberOfPlayers-1)
	{
		//player is at end of array, so simply decrease capacity
		if (turnMod == -1)
			//send the reference back 1
			k--;
		else
			k = 0;

		numberOfPlayers--;
		
	}
	else
	{
		//Note: no need to change the current reference with this loop
		for (int h=k; h < numberOfPlayers-1; h++)
			player[h] = player[h+1];
		
		//But still must check if turns are reversed
		if (turnMod == -1)
			k--;

		numberOfPlayers--;		
	}
	return k;
}


//Display relevant data 
void Deck::Status()
{
	int num;

	cout << endl;
	cout << "Game Status" << endl;
	cout << "-------------" << endl;
	cout << "# of players: " << numberOfPlayers << endl;
	cout << "Turn Order: ";

	if (turnMod == -1)
	{
		cout << "Reverse\n";
		for (int v=numberOfPlayers-1; v >= 0; v--)
		{
			num = ((numberOfPlayers - 1) - v) + 1;
			cout << num << ".  " << player[v].name << endl;
		}	
	}
	else
	{
		cout << "Normal\n";
		for (int q=0; q < numberOfPlayers; q++)
			cout << (q+1) << ".  " << player[q].name << endl;
	}
		
	cout << endl;
	cout << "Cards in Deck: " << cardCount << endl;
	cout << "Last card played: " << discard.getSuit() << endl;
	cout << "======\n";
	cout << "Score: " << score << endl;
}


/** This function determines if the player cannot play a card when the score is 90 or above.
	The function checks each card in the player's hand; for each card that cannot be
	played, 1 is added to a counter.  If the counter reaches 4, then the player cannot play
	a card, and he/she is booted from the game. */
int Deck::Search(int c)
{
	int unplayable = 0;
	int temp = score;
	
	//start checking the player's cards
	for (int w = player[c].min; w <= player[c].max; w++)
	{
		temp += newDeck[w].getValue();
		if (temp > 99 && newDeck[w].getValue() != 10 && newDeck[w].getValue() != 4 &&
			newDeck[w].getValue() != 3 && newDeck[w].getValue() != 9)
			//card is useless
		{
			temp -= newDeck[w].getValue();
			unplayable++;
		}
	}

	if (unplayable >= 4)  //the '4' = number of cards in the player's hand
	{
		//Player is eliminated, since he has no card to play
		system("cls");
		cout << player[c].name << "'s hand:\n";
		ShowHand(c);

		cout << endl;
		cout << player[c].name << " cannot continue the game!  Get outta here!\n";
		getch();

		//remove the player
		c = BootPlayer(c);
	}
	return c;
}


/*My first-ever attempt at making a CPU-controlled player! */
int Deck::PlayerAI(int b)
{
	/*The only thing that the AI is concerned about is the score.  All of its actions
	  are based on the current score. */
	bool playerOK = false;		//true= player's still in the game
	int flag = 0;				//stores the number of unplayable cards

	EasyRandom s;
	s.SetTimerSeed();
	s.SetInterval(1,100);
	int prob = s.DrawRandomNumber();

	//there is a 40% chance that the CPU will play a 9, raising the score to 99.
	int probNine = 40;

	cout << "CPU is making a move...\n";

	if (score < 90)
	{
		//Search for a 9 in the player's hand
		int y = player[b].min;
		bool cardPicked = false;

		while (y <= player[b].max && cardPicked != true)
		{
			//check if current card is 9 and see if AI will play the card
			if (newDeck[y].getValue() == 9 && prob <= probNine)
			{
				playNine();

				//draw a card
				Draw(y);
				cardPicked = true;
				playerOK = true;
			}
			//AI does nothing if prob > playNine, and moves on

			else if (newDeck[y].getValue() != 10 && newDeck[y].getValue() != 4 &&
				newDeck[y].getValue() != 3 && newDeck[y].getValue() != 9)
			{
				//play the current card
				if (newDeck[y].getValue() > 10)
					//it's a face card
					playFace();
				else
					score += newDeck[y].getValue();

				Draw(y);
				cardPicked = true;
				playerOK = true;
			}
			else
			{
				flag++;		//card is a special card
				y++;
			}
		}
	}

	 
	if (flag >= 4 && score < 90)	//no choice but to play a special card
	{
		int r = player[b].min;
		while (r <= player[b].max)
		{
			if (newDeck[r].getValue() == 10)
			{
				playTen();

				Draw(r);
				//set r to a high number so that the loop breaks
				r = 16;
				playerOK = true;
			}
			else if (newDeck[r].getValue() == 3)
			{
				playThree();
				
				Draw(r);
				r = 16;
				playerOK = true;
			}
			else if (newDeck[r].getValue() == 9)
			{
				playNine();
				
				Draw(r);
				r = 16;
				playerOK = true;
			}
			else if (newDeck[r].getValue() == 4)
			{
				playFour();
				
				Draw(r);
				r = 16;
				playerOK = true;
			}
			else
				r++;
		}
	}

	//reset flag to 0
	flag = 0;

	//Now check if score is 90 or greater
	if (score >= 90 && playerOK != true)
	{
		int temp = score;
		int z = player[b].min;
		bool cardFound = false;

		//want to see what cards the AI can play without going over 99
		while (z <= player[b].max && cardFound != true)
		{
			//don't use a special card yet.  Face cards can't be used, either
			if (newDeck[z].getValue() != 10 && newDeck[z].getValue() != 4 &&
				newDeck[z].getValue() != 3 && newDeck[z].getValue() != 9)
			{
				temp += newDeck[z].getValue();
				if (temp > 99)
				{
					//can't use this card
					temp -= newDeck[z].getValue();
					flag++;
				}
				else
				{
					//change the real score
					score += newDeck[z].getValue();

					Draw(z);
					cardFound = true;
					playerOK = true;
				}

			}
			z++;
			
		}

		//if we get here, then that means the AI has to use a special card to
		//stay in the game.
		if (flag < 4)
		{
			int f = player[b].min;

			while (f <= player[b].max && playerOK != true)
			{
				if (newDeck[f].getValue() == 10)
				{
					playTen();
					Draw(f);
					playerOK = true;
				}
				else if (newDeck[f].getValue() == 3)
				{
					playThree();
					Draw(f);
					playerOK = true;
				}
				else if (newDeck[f].getValue() == 9)
				{
					playNine();
					Draw(f);
					playerOK = true;
				}
				else if (newDeck[f].getValue() == 4)
				{
					playFour();
					Draw(f);
					playerOK = true;
				}
				else
					f++;
			}

		}
	}

	if (playerOK == true)
	{	
		cout << endl;
		cout << player[b].name << " has played a " << discard.getSuit() << endl;

		//go to the next player
		b = nextPlayer(b);
	}
	else
	{
		//get rid of the player
		cout << player[b].name << " is removed from the game!\n";
		b = BootPlayer(b);
	}

	//getch();
	delay (100000000);
	return b;
}

void Deck::delay(int m)
{
	unsigned int d = 0;
	while (d < m)
		d++;
}
/*	Play a card that the player chooses.  This function is the meat of the game.  The loop
	does not break until either a winner emerges or the deck runs out of cards.  The latter
	should never happen, but better to be safe than sorry!  If the deck is empty, then the
	game ends in a draw.  */
void Deck::Play()
{
	//determine the turn order
	EasyRandom t;
	t.SetTimerSeed();
	t.SetInterval(0, numberOfPlayers - 1);

	int current = t.DrawRandomNumber();	//current player's turn (current <= numberOfPlayers - 1)
	int index;
	bool GameOver = false;		//game loop ends when true
	Card target = newDeck[0];	//used in conjunction with the cursor
	bool pickOK;				//loop doesn't end until equal true
	char cursor = (char) 62;	//arrow cursor
	turnMod = 1;				// -1 = turns go in reverse; otherwise, normal
	score = 0;
	
	cout << player[current].name << " goes first.\n";
	getch();
	
	system("cls");

	//start the main game loop	
	while (GameOver == false)
	{
		pickOK = false;

		
		//check if the current player can continue the game
		if (score >= 90 && numberOfPlayers != 1)
			current = Search(current);

		if (numberOfPlayers == 1)
		{
			//We have a winner
			winnerName = player[current].name;
					
			GameOver = true;
			pickOK = true;
		}
		else if (isEmpty())
		{
			//game ends in a draw
			GameOver = true;
			pickOK = true;
		}
		else
		{
			system("cls");
			cout << player[current].name << "'s hand:\n";

			if (player[current].isAI == true)
			{
				//don't let the player see the AI's hand
				for (int u=player[current].min; u <= player[current].max; u++)
					cout << "?\n";
			}
			else
			{

				//place a cursor
				cout << cursor << " ";
				ShowHand(current);
				index = player[current].min;
			}
			Status();
		}
		
		
		//if the current player is AI, then it takes its turn now
		if (player[current].isAI == true && numberOfPlayers != 1)
		{
			current = PlayerAI(current);
			pickOK = true;
		}
		
		//wait for input
		while (pickOK == false)
		{
			switch (getch())
			{
			case KEY_DOWN:
			{
				system("cls");
				cout << player[current].name << "'s hand:\n";

				//First check if we're at the end of the array
				if (index == player[current].max)
				{
					index = player[current].min;
					cout << cursor << " ";
					ShowHand(current);
					target = newDeck[index];
				}
				else
				{
					index++;
					target = newDeck[index];
					
					//place the cursor in its new position
					for (int i = player[current].min; i <= player[current].max; i++)
					{
						if (target.getSuit() == newDeck[i].getSuit())
							//insert cursor
							cout << cursor << " " << newDeck[i].getSuit() << endl;
						else
							cout << "  " << newDeck[i].getSuit() << endl;
					}
				}
				
				Status();
				break;
					
			} //case KEY_DOWN
				
			case KEY_UP:
			{
				system("cls");
				cout << player[current].name << "'s hand:\n";
				//First check if we're at the start of the array
				if (index == player[current].min)
				{
					while (index < player[current].max)
					{
						cout << "  " << newDeck[index].getSuit() << endl;
						index++;
					}
					
					//print the cursor and the last card on the last line
					cout << cursor << " " << newDeck[index].getSuit() << endl;
					target = newDeck[index];
				}
				else
				{
					index--;
					target = newDeck[index];
					//place the cursor in its new position
					for (int i = player[current].min; i <= player[current].max; i++)
					{
						if (target.getSuit() == newDeck[i].getSuit())
							//insert cursor
							cout << cursor << " " << newDeck[i].getSuit() << endl;
						else
							cout << "  " << newDeck[i].getSuit() << endl;
					}
				}
				
				Status();
				break;
				
			} //case KEY_UP
				
			case KEY_ENTER:
			{
				int temp = score;
				bool drawOK = false;
				
				//Check the card that was played
				if (newDeck[index].getValue() == 3)	
					//Card was "Score +0"; do nothing and move on
					playThree();
				else if (newDeck[index].getValue() == 4)
					//Card was "Reverse Turn"; change turnMod
					playFour();
				else if (newDeck[index].getValue() == 10)
					playTen();
				else if (newDeck[index].getValue() == 9)
					//Card was "Score = 99"; score becomes 99
					playNine();
				else if (newDeck[index].getValue() > 10)
					//Card is a face card, or "Score +10"; add 10 to score
					playFace();
				else
					//Card is normal; add its value to the score
					score += newDeck[index].getValue();
				
				//make sure the score hasn't exceeded 99
				if (score > 99)
				{
					cout << "Score must not exceed 99!\n";
					getch();
					score = temp;
				}
				else
					drawOK = true;
				
				
				if (drawOK == true)
				{
					//draw a card
					Draw(index);
					
					//go to next player
					current = nextPlayer(current);
				}
				pickOK = true;
				break;
			} //case KEY_ENTER
				
			case KEY_QUIT:
			{
				char decision;
				cout << endl;
				cout << player[current].name << ", are you sure you want to give up? (Y/N): ";
				cin >> decision;
				
				//check input
				if (decision == 'y' || decision == 'Y')
				{
				
					//get rid of the player
					current = BootPlayer(current);
		
					cout << endl;
					cout << "Goodbye!\n";
					getch();
	
				}
				else
					; //do nothing
				
				pickOK = true;
				break;
			}//case KEY_QUIT
				
				
			default:
				;  //do nothing
		} // switch
		} //while pickOK
	} //while GameOver


	//Display the results
	if (numberOfPlayers == 1 && !isEmpty())
	{
		cout << winnerName << " Wins!!!\n";
	}
	else
		//it was a draw
		cout << "Draw game...you all did equally bad!\n";
} //end Play
