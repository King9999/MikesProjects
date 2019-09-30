/* Source File: EasyRandom
   Author: Pieter Claassens (pc@nanoteq.com)
   Date Written: 2002/05/21
*/

//#include "stdafx.h"
#include <iostream>
#include <time.h>
#include "EasyRand.h"
using namespace std;

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif


// Default contructor

EasyRandom::EasyRandom(int a, int b) {
	SetInterval(a,b);
}

// Assign Low And High Values to class EasyRandom data members. Use Defaults on error.

void EasyRandom::SetInterval(int a, int b) {
	if (a > b) {
		
		cout << "Error: HIGH value cannot be less than LOW value!\nUsing defaults: High = 1, Low = 0" << endl;
		Low = 0;
		High = 1;
	} 
	else {
		Low = a;
		High = b;
	}
}

// Set the seed to generate pseudo-random numbers. Use the current time & date as seed.

void EasyRandom::SetTimerSeed() {
	time_t SeedTime;
	struct tm SeedDate;
	SeedTime = time(0);
	SeedDate = *localtime(&SeedTime);
	int FinalSeed = SeedTime + SeedDate.tm_mday + (SeedDate.tm_mon+1) + (SeedDate.tm_year+1900);
	srand((unsigned int) FinalSeed);
}

// Return the Random number, brief explanation on the method
// Interval : Self explanatory - Get the difference between the High and Low Values 
// RandomOffset = using rand() function to generate random numbers from 0 to Interval-1
// RandomNumber = Add the RandomOffset to the lowest number.

int EasyRandom::DrawRandomNumber() {
	int Interval = GetHigh() - GetLow() + 1;
	int RandomOffset = rand() % Interval;
	int RandomNumber = GetLow() + RandomOffset;
	return RandomNumber;
}

// Standard Inpectors - Return High & Low values

int EasyRandom::GetHigh() {
	return High;
}

int EasyRandom::GetLow() {
	return Low;
}
