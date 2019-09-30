#include "DigitExtractor.h"
#include <iostream>
using namespace std;

DigitExtractor::DigitExtractor()
{
	thousandsDigit = 0;
	hundredsDigit = 0;
	tensDigit = 0;
	onesDigit = 0;
}

//we get individual digits by using division and modulus. By using integers, they
//will be rounded down by default.
void DigitExtractor::ExtractDigits(int number)
{
	//make sure the number is between 1 and 9999
	if (number < 1 || number > 9999)
	{
		cout << number << " is not valid." << endl;
	}
	else
	{
		//begin the extraction
		thousandsDigit = number / 1000;
		hundredsDigit = (number % 1000) / 100;
		tensDigit = (number % 100) / 10;
		onesDigit = number % 10;

		//display each digit
		cout << "Thousands: " << thousandsDigit << endl;
		cout << "Hundreds: " << hundredsDigit << endl;
		cout << "Tens: " << tensDigit << endl;
		cout << "Ones: " << onesDigit << endl;
	}
}