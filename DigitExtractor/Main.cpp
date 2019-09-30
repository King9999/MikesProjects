#include "DigitExtractor.h"
#include <iostream>
using namespace std;

int main()
{
	DigitExtractor game;
	int number;

	cout << "Enter a number between 1 and 9999: ";
	cin >> number;
	cout << endl;

	//extract numbers
	game.ExtractDigits(number);

	system("PAUSE");
	return 0;
}