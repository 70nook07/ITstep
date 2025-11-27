#include <iostream>
#include "Fraction.h"

using namespace std;


//TESTING
int main() {
	
	Fraction a, b;
	
	cout << "Enter first fraction: " << endl;
    a.input();

    cout << "Enter second fraction: " << endl;
    b.input();
	
	cout << "A = "; a.show();
    cout << "B = "; b.show();
    
    Fraction s = a.add(b);
    cout << "A + B = "; s.show();

    Fraction d = a.sub(b);
    cout << "A - B = "; d.show();

    Fraction m = a.mul(b);
    cout << "A * B = "; m.show();

    Fraction q = a.div(b);
    cout << "A / B = "; q.show();
	
	return 0;
}
