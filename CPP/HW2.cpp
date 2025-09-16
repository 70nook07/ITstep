#include <iostream>
#include <cmath>

using namespace std;

int main() {
    int choice;
    cout << "Choose a task (1-5): ";
    cin >> choice;
    
    switch(choice) {
        case 1: {
            // Task 1
            int a;
            cout << "Enter value of a: ";
            cin >> a;
            
            int sum = 0;
            for(int i = a; i <= 500; i++) {
                sum += i;
            }
            cout << "Sum of numbers from " << a << " to 500: " << sum << endl;
            break;
        }
        
        case 2: {
            // Task 2:
            int x, y;
            cout << "Enter x: ";
            cin >> x;
            cout << "Enter y: ";
            cin >> y;
            
            double result = pow(x, y);
            cout << x << " raised to the power of " << y << " = " << result << endl;
            break;
        }
        
        case 3: {
            // Task 3
            int sum = 0;
            for(int i = 1; i <= 1000; i++) {
                sum += i;
            }
            double average = static_cast<double>(sum) / 1000;
            cout << "Arithmetic mean of numbers from 1 to 1000: " << average << endl;
            break;
        }
        
        case 4: {
            // Task 4
            int a;
            cout << "Enter a (1 <= a <= 20): ";
            cin >> a;
            
            if(a < 1 || a > 20) {
                cout << "Error: a must be between 1 and 20!" << endl;
                break;
            }
            
            long long product = 1;
            for(int i = a; i <= 20; i++) {
                product *= i;
            }
            cout << "Product of numbers from " << a << " to 20: " << product << endl;
            break;
        }
        
        case 5: {
            // Task 5
            int k = 7;
            cout << "Multiplication table for " << k << ":" << endl;
            
            for(int i = 2; i <= 9; i++) { // example 2 to 9
                cout << k << " x " << i << " = " << k * i << endl;
            }
            break;
        }
        
        default:
            cout << "Invalid choice!" << endl;
    }
    
    return 0;
}
