#include <iostream>
using namespace std;

int main() {
    // Task 1
    float R1, R2, R3;
    cout << "Enter R1, R2, R3 for Task 1: ";
    cin >> R1 >> R2 >> R3;
    
    float R0 = 1.0 / (1.0/R1 + 1.0/R2 + 1.0/R3);
    cout << "R0 = " << R0 << endl;

    // Task 2
    const float PI = 3.14;
    float L;
    cout << "Enter circumference L for Task 2: ";
    cin >> L;
    
    float R = L / (2 * PI);
    float S = PI * R * R;
    cout << "S = " << S << endl;

	  // Task 3
	  int totalSeconds;
	
    cout << "Enter time in seconds for Task 3: ";
    cin >> totalSeconds;
    
    int hours = totalSeconds / 3600;
    int minutes = (totalSeconds % 3600) / 60;
    int seconds = totalSeconds % 60;
    
    cout << totalSeconds << " seconds = " << hours << " hours, " << minutes << " minutes, " << seconds << " seconds" << endl;
    
    
    return 0;
}
