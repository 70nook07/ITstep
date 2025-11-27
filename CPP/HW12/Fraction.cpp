#include <iostream>
#include "Fraction.h"

using namespace std;

Fraction::Fraction() : num(0), denom(1) {}

Fraction::Fraction(int n, int d) : num(n), denom(d == 0 ? 1 : d) {}

void Fraction::input() {
	cout << "Enter numerator: ";
	cin >> num;
	
	cout << "Enter denominator: ";
	cin >> denom;
	
	if (denom == 0) {
		cout << "Denominator cannot be 0, setting it to 1" << endl; // For possibility for operations by 0 
		denom = 1;
	} 
}

void Fraction::show() const {
	cout << num << "/" << denom << endl;
}

Fraction Fraction::add(const Fraction& other) const {
	Fraction result;
	result.num = num * other.denom + other.num * denom;
	result.denom = denom * other.denom;
	
	return result;
}

Fraction Fraction::sub(const Fraction& other) const {
	Fraction result;
	result.num = num * other.denom - other.num * denom;
	result.denom = denom * other.denom;
	
	return result;
}

Fraction Fraction::mul(const Fraction& other) const {
	Fraction result;
	result.num = num * other.num;
	result.denom = denom * other.denom;
	
	return result;
}

Fraction Fraction::div(const Fraction& other) const {
	Fraction result;
	result.num = num * other.denom;
	result.denom = denom * other.num == 0 ? 1 : other.num;
	
	return result;
}



