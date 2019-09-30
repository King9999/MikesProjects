/* Deck.h 
Author: Mike Murray (mike_murray56@hotmail.com)
*/

class Deck
{
private:
	int cardCount;	//keeps track of the number of cards in the deck (MAX = 52)
	int handCount;	//number of cards in hand (MAX = 4)
	int score;		//The current score (MAX = 99)
	int numberOfPlayers;
	int numberOfAIPlayers;
	int turnMod;	//Changes turn order
public:
	~Deck() {}
	void Shuffle(); //insert Card objects in an array at random
	void Deal();    //select Cards and give them to players
	bool isEmpty() { return (cardCount == 0); } //should never happen, but just in case
	void ShowHand(int h);
	void Play();	//player's actions
	void getPlayers();	//get the number of players
	int getScore() { return score; }
	int Search(int t);  //searches the player's hand for cards to play when score is between 90 & 99
	void Status();		//display the game status (cards in deck, etc.)
	int PlayerAI(int k);	//code for CPU actions
	int BootPlayer(int p);		//eliminates player
	int nextPlayer(int e);		//gives turn to next player
	void Draw(int y);		//draws a card
	void delay(int m);

	//special card functions
	void playTen()
	{
		if (score < 10)
			score = 0;
		else
			score -=10;
	}
	void playNine() { score = 99; }
	void playFour() { turnMod = turnMod * -1; }
	void playThree() { score += 0; }
	void playFace() { score += 10; }

	//in case Jokers are involved, they could be wild cards and have any function
	void playJoker();
};