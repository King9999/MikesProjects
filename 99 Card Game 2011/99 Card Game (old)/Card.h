/* Card.h 
Author: Mike Murray (mike_murray56@hotmail.com)

This file deals with the creation of a card. */

#include <iostream>
#include <string>
using namespace std;

class Card
{	
public:
	Card(int ID, int val, string su);
	~Card() {}
	int getValue() { return value; }
	string getSuit() { return suit; } 
	

private:
	int id;			//ID number used for sorting purposes only
	int value;		//The value of the card
	string suit;	//The suit	
};

