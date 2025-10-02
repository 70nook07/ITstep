#include <iostream>
#include <cstdlib>
#include <ctime>

using namespace std;

template <typename T>
T compare(T a, T b) {
	if (a < b) {
		return a;
	} else {
		return b;
	}
}

template <typename T, size_t N>
T min_element(T (&arr)[N]) {
    T min_val = arr[0];
    for (size_t i = 1; i < N; i++) {
        if (arr[i] < min_val) {
            min_val = arr[i];
        }
    }
    return min_val;
}

// MAIN

int main() {
    
    srand(time(0));
    
    const int ROWS = 3;
    const int COLS = 5;
    int array[ROWS][COLS];
    int swapped[ROWS][COLS];
    int diagonal_zeros[ROWS][COLS];
    
    for (int i = 0; i < ROWS; i++) {
        for (int j = 0; j < COLS; j++) {
            array[i][j] = rand() % 21 - 10;
            swapped[i][j] = array[i][j];	//copying
            diagonal_zeros[i][j] = array[i][j];
        }
    }
    
    int positives = 0;
    int evens = 0;
    
    cout << "Number table 3x5:" << endl;
    for (int i = 0; i < ROWS; i++) {
        for (int j = 0; j < COLS; j++) {
            cout << array[i][j] << "\t";
            if (array[i][j] > 0) {
				positives++;	//counting positives
			}
			if (array[i][j] % 2 == 0) {
				evens++;		//counting even nums
			}
        }
        cout << endl;
    }
    
	for (int j = 0; j < COLS; j++) {
        int temp = swapped[0][j];
        swapped[0][j] = swapped[ROWS-1][j];
        swapped[ROWS-1][j] = temp;
    }
    
    cout << "\nPositive numbers: " << positives	//showing off with 3 tasks
	  	 << "\nEven numbers: " << evens
    	 << "\nSwapped last and first lines array:" << endl;
    for (int i = 0; i < ROWS; i++) {
        for (int j = 0; j < COLS; j++) {
            cout << swapped[i][j] << "\t";
        }
        cout << endl;
    }
    cout << endl;
    
    int min_dimensions = (ROWS < COLS) ? ROWS : COLS;	//task 4 with zeroes
    
    for (int i = 0; i < min_dimensions; i++) {
        diagonal_zeros[i][i] = 0;
    }	
    
    cout << "Array with zeroed main diagonal:" << endl;
    for (int i = 0; i < ROWS; i++) {
        for (int j = 0; j < COLS; j++) {
            cout << diagonal_zeros[i][j] << "\t";
        }
        cout << endl;
    }
    
    cout << "\nComparing numbers (change in code): " << compare(2,4) << endl; //comparing numbers
    
    int arr[5] = {1, 2, 4, -6, 8};
	cout << "\nFinding smallest element in an 1-dimensional array using an address link (change in code): " << min_element(arr);	//min element in an array

	return 0;
}
