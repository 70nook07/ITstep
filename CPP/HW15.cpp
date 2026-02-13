#include <iostream>
#include <ctime>

class Stack {
private:
    char* st;
    int top;
    int capacity;

    enum { EMPTY = -1 };

    void Resize(int newCapacity) {
        char* newData = new char[newCapacity];
        for (int i = 0; i <= top; i++)
            newData[i] = st[i];

        delete[] st;
        st = newData;
        capacity = newCapacity;
    }

public:
    Stack(int size = 20) {
        capacity = size;
        st = new char[capacity];
        top = EMPTY;
    }

    ~Stack() {
        delete[] st;
    }

    void Clear() {
        top = EMPTY;
    }

    bool IsEmpty() const {
        return top == EMPTY;
    }

    bool IsFull() const {
        return top == capacity - 1;
    }

    int GetCount() const {
        return top + 1;
    }

    void Push(char c) {
        if (IsFull())
            Resize(capacity * 2);

        st[++top] = c;
    }

    char Pop() {
        if (!IsEmpty())
            return st[top--];
        return 0;
    }

    void Output() const {
        for (int i = 0; i <= top; i++)
            std::cout << st[i] << " ";
        std::cout << std::endl;
    }
};

int main() {
    std::srand(std::time(nullptr));

    std::cout << "Creating stack with initial capacity 20\n";
    Stack ST;

    std::cout << "Is stack empty? " << ST.IsEmpty() << std::endl;
    std::cout << "Element count: " << ST.GetCount() << "\n\n";

    std::cout << "Adding 30 elements into stack\n";

    for (int i = 0; i < 30; i++) {
        char c = std::rand() % 4 + 2;
        ST.Push(c);
    }

    std::cout << "Stack content:\n";
    ST.Output();

    std::cout << "Element count: " << ST.GetCount() << "\n\n";

    std::cout << "Popping 5 elements from stack:\n";
    for (int i = 0; i < 5; i++) {
        char c = ST.Pop();
        std::cout << "Popped: " << c << std::endl;
    }

    std::cout << "\nContent after popping:\n";
    ST.Output();

    std::cout << "Element count: " << ST.GetCount() << "\n\n";

    std::cout << "Clearing the stack\n";
    ST.Clear();

    std::cout << "Is stack empty after clear? "
              << ST.IsEmpty() << std::endl;
    std::cout << "Element count after clear: "
              << ST.GetCount() << std::endl;

    return 0;
}
