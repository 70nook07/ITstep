#include <iostream>
#include <ctime>

using namespace std;

void sort_arr(int arr[], int size) {
    for (int i = 0; i < size - 1; i++) {
        for (int j = 0; j < size - i - 1; j++) {
			
            if (arr[j] > arr[j + 1]) swap(arr[j], arr[j + 1]);
        }
    }
}

int main() {

	// TASK 1
    
    cout << "TASK 1" << endl;
    
    int n;
    cout << "Enter size of array: ";
    cin >> n;

    int *arr1 = new int[n];

    srand(time(0));
    
    for (int i = 0; i < n; i++) arr1[i] = rand() % 21 - 10;

    cout << "Array: ";
    for (int i = 0; i < n; i++) cout << arr1[i] << " ";
    
    cout << endl;

    int sum = 0;
    for (int i = 0; i < n; i++) sum += arr1[i];

    int avg = sum / n;
    cout << "Average is " << avg << endl;

    delete[] arr1;
    
    // TASK 2
    
    cout << "TASK 2" << endl;
    
    int n1, n2;
    cout << "Enter size of first array: ";
    cin >> n1;
    cout << "Enter size of second array: ";
    cin >> n2;

    int *arr2a = new int[n1];
    int *arr2b = new int[n2];

    for (int i = 0; i < n1; i++) arr2a[i] = rand() % 100;
    for (int i = 0; i < n2; i++) arr2b[i] = rand() % 100;

    cout << "Array A: ";
    for (int i = 0; i < n1; i++) cout << arr2a[i] << " ";
    cout << endl;

    cout << "Array B: ";
    for (int i = 0; i < n2; i++) cout << arr2b[i] << " ";
    cout << endl;

    int total = n1 + n2;
    int *arr2c = new int[total];

    for (int i = 0; i < n1; i++) arr2c[i] = arr2a[i];
    for (int i = 0; i < n2; i++) arr2c[n1 + i] = arr2b[i];

    sort_arr(arr2c, total);

    cout << "Merged n' sorted: ";
    for (int i = 0; i < total; i++) cout << arr2c[i] << " ";
    cout << endl;

    delete[] arr2a;
    delete[] arr2b;
    delete[] arr2c;
    
    return 0;
}
