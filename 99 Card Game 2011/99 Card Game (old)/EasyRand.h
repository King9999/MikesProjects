/* Header File: EasyRand.h
   Author: Pieter Claassens (pc@nanoteq.com)
   Date Written: 2002/05/21  
*/

#ifndef H_EASYRAND
#define H_EASYRAND

//#include "stdafx.h"

class EasyRandom {
public:
	// default conmstructor with defaults of 0&1
	EasyRandom(int a=0, int b=1);

	// Mutator:
	// Sets high and low values
	void SetInterval(int a, int b);

	// Faclitators:
	// Display Random numbers...
	int DrawRandomNumber();
	
	// Set seed using current time
	void SetTimerSeed();

private:
	// Inspectors:
	int GetLow();
	int GetHigh();
	
	// Data members
	int Low, High;

};
#endif