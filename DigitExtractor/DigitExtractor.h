/* November 2011
	Digit Extractor by Haskell (Mike) Murray

	The purpose of this program is to take any number from 1 to 9999 and display each individual digit. This is only for fun,
	but it could have a future use in a game. */

class DigitExtractor
{
public:
	DigitExtractor();
	~DigitExtractor() {}
	void ExtractDigits(int numberToExtract);	//takes a number from 1 to 9999 and extracts digits
private:
	int number;
	int thousandsDigit;
	int hundredsDigit;
	int tensDigit;
	int onesDigit;
};