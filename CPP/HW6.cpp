#include <iostream>
using namespace std;

// Task 1
void copy_array(int* source, int* destination, int size) {
    int* src_ptr = source;
    int* dest_ptr = destination;
    
    for (int i = 0; i < size; i++) {
        *dest_ptr = *src_ptr;
        src_ptr++;
        dest_ptr++;
    }
}

// Task 2
void reverse_array(int* arr, int size) {
    int* start_ptr = arr;
    int* end_ptr = arr + size - 1;
    
    while (start_ptr < end_ptr) {
        int temp = *start_ptr;
        *start_ptr = *end_ptr;
        *end_ptr = temp;
        
        start_ptr++;
        end_ptr--;
    }
}

void print_array(int* arr, int size) {
    int* ptr = arr;
    for (int i = 0; i < size; i++) {
        cout << *ptr << " ";
        ptr++;
    }
    cout << endl;
}

int main() {
    const int size = 5;
    int source[size] = {1, 2, 3, 4, 5};
    int destination[size];
    int reversed[size];
    
    // Task 1
    cout << "TASK 1" << endl;
    cout << "OG array: ";
    print_array(source, size);
    
    copy_array(source, destination, size);
    cout << "Copied array: ";
    print_array(destination, size);
    
    // Task 2 
    cout << "\nTASK 2" << endl;
    copy_array(source, reversed, size);
    cout << "OG array: ";
    print_array(reversed, size);
    
    reverse_array(reversed, size);
    cout << "Reversed array: ";
    print_array(reversed, size);
    
    return 0;
}

