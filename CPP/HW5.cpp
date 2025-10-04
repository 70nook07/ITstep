#include <iostream>
#include <cmath>
using namespace std;

// Task 1
template <typename T>
T get_max(T a, T b) {
	return (a > b) ? a : b;
}

// Task 2
bool is_prime(int number) {
    if (number <= 1) return false;
    if (number == 2) return true;
    if (number % 2 == 0) return false;
    
    for (int i = 3; i * i <= number; i += 2) {
        if (number % i == 0) return false;
    }
    return true;
}

template<typename T, size_t N>
int count_primes(T (&arr)[N]) {
    int count = 0;
    for (size_t i = 0; i < N; i++) {
        if (is_prime(arr[i])) {
            count++;
        }
    }
    return count;
}

// Task 3
template <typename T, size_t N>
int binarySearch(T (&arr)[N], T key) {
    int left = 0;
    int right = N - 1;
    
    cout << "Searching for a key " << key << " in array of size " << N << endl;
    
    while (left <= right) {
        int mid = left + (right - left) / 2;
        
        cout << "Looking up index [" << mid << "] = " << arr[mid] << endl;
        
        if (arr[mid] == key) {
            return mid; // FOUND
        }
        
        if (arr[mid] < key) {
            left = mid + 1;
            cout << "Searching right: [" << left << "..." << right << "]" << endl;
        } else {
            right = mid - 1;
            cout << "Searching left: [" << left << "..." << right << "]" << endl;
        }
    }
    
    return -1; // NOT FOUND
}

// Task 4
void find_free_parking_rows(int parking_lot[][3], int total_rows) {
    cout << "Free parking rows: ";
    bool found_free_row = false;
    
    for (int row = 0; row < total_rows; row++) {
        bool is_row_free = true;
        
        // parking spots in the current row
        for (int spot = 0; spot < 3; spot++) {
            if (parking_lot[row][spot] == 1) {
                is_row_free = false;
                break;
            }
        }
        
        if (is_row_free) {
            cout << (row + 1) << " ";
            found_free_row = true;
        }
    }
    
    if (!found_free_row) {
        cout << "no free rows";
    }
    cout << endl;
}

int main() {
    int parking_lot[4][3] = {
        {1, 0, 0},  // Row 1
        {0, 0, 0},  // Row 2
        {0, 0, 0},  // Row 3
        {1, 0, 1}   // Row 4
    };
    
    find_free_parking_rows(parking_lot, 4);
    
    // Test 
    cout << "\nTesting Parking Lot" << endl;
    int another_parking_lot[3][3] = {
        {1, 1, 1},  // Row 1
        {0, 1, 0},  // Row 2
        {0, 0, 0}   // Row 3
    };
    
    find_free_parking_rows(another_parking_lot, 3);
    
    return 0;
}

