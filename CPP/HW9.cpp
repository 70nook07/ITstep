#include <iostream>

using namespace std;

// TASK 1
char *remove_at(char arr[], int len, int index) {
	
    if (index < 0 || index >= len) return nullptr;

    char *res = new char[len - 1];

    for (int i = 0, j = 0; i < len; i++) {
        if (i != index) {
            res[j] = arr[i];
            j++;
        }
    }

    return res;
}

// TASK 2
char *remove_char(char arr[], int len, char ch, int &new_len) {
	
    new_len = 0;

    for (int i = 0; i < len; i++) {
        if (arr[i] != ch) new_len++;
    }
    
    char *res = new char[new_len];

    for (int i = 0, j = 0; i < len; i++) {
        if (arr[i] != ch) {
            res[j] = arr[i];
            j++;
        }
    }

    return res;
}

// TASK 3
char *insert_at(char arr[], int len, int index, char ch) {
	
    if (index < 0 || index > len) return nullptr;

    char *res = new char[len + 1];

    for (int i = 0, j = 0; i < len + 1; i++) {
        if (i == index) {
            res[i] = ch;
        } else {
            res[i] = arr[j];
            j++;
        }
    }

    return res;
}


// TASK 4
int **create_2D(int rows, int cols) {
    int **matrix = new int*[rows];

    for (int i = 0; i < rows; i++) {
        matrix[i] = new int[cols];
    }

    return matrix;
}

void fill_2D(int **matrix, int rows, int cols) {
    for (int i = 0; i < rows; i++)
        for (int j = 0; j < cols; j++)
            matrix[i][j] = rand() % 100;
}

void print_2D(int **matrix, int rows, int cols) {
    for (int i = 0; i < rows; i++) {
        for (int j = 0; j < cols; j++) {
            cout << matrix[i][j] << " ";
        }
        cout << "\n";
    }
}

void get_extremes_2D(int **matrix, int rows, int cols, int &min, int &max) {
    min = matrix[0][0];
    max = matrix[0][0];

    for (int i = 0; i < rows; i++)
        for (int j = 0; j < cols; j++) {
            if (matrix[i][j] < min) min = matrix[i][j];
            if (matrix[i][j] > max) max = matrix[i][j];
        }
}

void delete_2D(int **matrix, int rows) {
    for (int i = 0; i < rows; i++)
        delete[] matrix[i];
    delete[] matrix;
}

int main() {

    int len;
    cout << "Enter length of char array: ";
    cin >> len;

    char *str = new char[len];
    cout << "Enter " << len << " characters: ";
    for (int i = 0; i < len; i++) cin >> str[i];

    // TEST 1
    int index;
    cout << "\nEnter index to remove char from: ";
    cin >> index;
    char *r1 = remove_at(str, len, index);
    if (r1) {
        cout << "After: ";
        for (int i = 0; i < len - 1; i++) cout << r1[i];
        cout << endl;
        delete[] r1;
    }

    // TEST 2
    char ch;
    cout << "\nEnter char to banish all occurrences of char into oblivion: ";
    cin >> ch;
    int new_len;
    char *r2 = remove_char(str, len, ch, new_len);
    cout << "After: ";
    for (int i = 0; i < new_len; i++) cout << r2[i];
    cout << endl;
    delete[] r2;

    // TEST 3
    cout << "\nInsert at position: ";
    cin >> index;
    cout << "Insert char: ";
    cin >> ch;
    char *r3 = insert_at(str, len, index, ch);
    if (r3) {
        cout << "After insert: ";
        for (int i = 0; i < len + 1; i++) cout << r3[i];
        cout << endl;
        delete[] r3;
    }

    delete[] str;

    // TEST 4
    int rows, cols;
    cout << "\nEnter rows and columns for 2D array: ";
    cin >> rows >> cols;

    int **a = create_2D(rows, cols);
    fill_2D(a, rows, cols);

    cout << "2D array:\n";
    print_2D(a, rows, cols);

    int mn, mx;
    get_extremes_2D(a, rows, cols, mn, mx);
    cout << "Min = " << mn << ", Max = " << mx << endl;

    delete_2D(a, rows);

    return 0;
}


