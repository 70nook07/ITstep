#include <iostream>

class Fraction {
private:
    int numerator;
    int denominator;

public:
    Fraction(int num = 0, int den = 1) {
        numerator = num;
        denominator = (den == 0) ? 1 : den;
    }

    Fraction operator+(const Fraction& other) const {
        return Fraction(
            numerator * other.denominator + other.numerator * denominator,
            denominator * other.denominator
        );
    }

    Fraction operator-(const Fraction& other) const {
        return Fraction(
            numerator * other.denominator - other.numerator * denominator,
            denominator * other.denominator
        );
    }

    Fraction operator*(const Fraction& other) const {
        return Fraction(
            numerator * other.numerator,
            denominator * other.denominator
        );
    }

    Fraction operator/(const Fraction& other) const {
        return Fraction(
            numerator * other.denominator,
            denominator * other.numerator
        );
    }

    void output() const {
        std::cout << numerator << "/" << denominator << std::endl;
    }
};

int main() {
    Fraction a(8, 3);
    Fraction b(3, 4);

    (a + b).output();
    (a - b).output();
    (a * b).output();
    (a / b).output();

    return 0;
}
