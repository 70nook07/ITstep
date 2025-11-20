#include <iostream>

using namespace std;

// TASK 1
void swapper1(int &a, int &b) {
	a = a + b;
    b = a - b;
    a = a - b;
}

// TASK 2

template<typename T>
void swapper2(T &a, T &b) {
	T temp = a;
    a = b;
    b = temp;
}

int main() {
	
	//TASK 3
	
	int n;
    cout << "Enter size of array: ";
    cin >> n;

    int *arr = new int[n];

    cout << "Enter " << n << " numbers:" << endl;
    for (int i = 0; i < n; i++) {
        cin >> arr[i];
    }

    cout << "Your array: ";
    for (int i = 0; i < n; i++) {
        cout << arr[i] << " ";
    }
    cout << endl;
    
    // New array without last index
    
    int *new_arr = new int[n - 1];
    
    cout << "Your array, but less: ";
    for (int i = 0; i < n - 1; i++) {
		new_arr[i] = arr[i];
		cout << new_arr[i] << " ";
	}
    
    delete[] arr;
    delete[] new_arr;
    
    return 0;
}
