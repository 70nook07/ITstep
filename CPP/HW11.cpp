#include <iostream>
#include <cstring>

using namespace std;

class String {
private:
    char* data;
    int size;
    static int objectCount;

public:
    String() : String(80) {
    }

    String(int size) {
        this->size = size;
        data = new char[size + 1];
        data[0] = '\0';
        objectCount++;
    }

    String(const char* str) : String(strlen(str)) {
        strcpy(data, str);
    }

    ~String() {
        delete[] data;
        objectCount--;
    }

    void input() {
        cout << "Enter string: ";
        cin.getline(data, size + 1);
    }

    void output() const {
        cout << data << endl;
    }

    static int getObjectCount() {
        return objectCount;
    }
};

int String::objectCount = 0;

int main() {
    String s1;
    s1.input();

    String s2(50);
    s2.input();

    String s3("Hello world");

    s1.output();
    s2.output();
    s3.output();

    cout << "Number of String objects: " << String::getObjectCount() << endl;

    return 0;
}
