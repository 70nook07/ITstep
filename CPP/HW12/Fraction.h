#ifndef FRACTION_H_
#define FRACTION_H_

class Fraction {
private:
	int num;
	int denom;
	
public:
	Fraction();
	Fraction(int number, int denominator);
	
	void input();
	void show() const;
	
	Fraction add(const Fraction& other) const;
	Fraction sub(const Fraction& other) const;
	Fraction mul(const Fraction& other) const;
	Fraction div(const Fraction& other) const;
};

#endif /* FRACTION_H_ */
