#include <iostream>

template <typename T>
class Array {
private:
    T* data;
    int size;
    int capacity;
    int grow;

    void Resize(int newCapacity) {
        T* newData = new T[newCapacity];
        int limit = (size < newCapacity) ? size : newCapacity;

        for (int i = 0; i < limit; i++)
            newData[i] = data[i];

        delete[] data;
        data = newData;
        capacity = newCapacity;

        if (size > capacity)
            size = capacity;
    }

public:
    Array(int grow = 1) {
        data = nullptr;
        size = 0;
        capacity = 0;
        this->grow = grow;
    }

    Array(const Array& other) {
        size = other.size;
        capacity = other.capacity;
        grow = other.grow;

        data = new T[capacity];
        for (int i = 0; i < size; i++)
            data[i] = other.data[i];
    }

    ~Array() {
        delete[] data;
    }

    int GetSize() const {
        return capacity;
    }

    void SetSize(int newSize, int newGrow = 1) {
        grow = newGrow;
        Resize(newSize);
        size = newSize;
    }

    int GetUpperBound() const {
        return size - 1;
    }

    bool IsEmpty() const {
        return size == 0;
    }

    void FreeExtra() {
        Resize(size);
    }

    void RemoveAll() {
        delete[] data;
        data = nullptr;
        size = 0;
        capacity = 0;
    }

    T& GetAt(int index) {
        return data[index];
    }

    void SetAt(int index, const T& value) {
        if (index >= 0 && index < capacity)
            data[index] = value;
    }

    T& operator[](int index) {
        return data[index];
    }

    void Add(const T& value) {
        if (size >= capacity) {
            Resize(capacity + grow);
        }
        data[size++] = value;
    }

    void Append(const Array& other) {
        for (int i = 0; i < other.size; i++)
            Add(other.data[i]);
    }

    Array& operator=(const Array& other) {
        if (this == &other)
            return *this;

        delete[] data;

        size = other.size;
        capacity = other.capacity;
        grow = other.grow;

        data = new T[capacity];
        for (int i = 0; i < size; i++)
            data[i] = other.data[i];

        return *this;
    }

    T* GetData() {
        return data;
    }

    void InsertAt(const T& value, int index) {
        if (index < 0 || index > size)
            return;

        if (size >= capacity)
            Resize(capacity + grow);

        for (int i = size; i > index; i--)
            data[i] = data[i - 1];

        data[index] = value;
        size++;
    }

    void RemoveAt(int index, int count = 1) {
        if (index < 0 || index >= size || count <= 0)
            return;

        if (index + count > size)
            count = size - index;

        for (int i = index; i < size - count; i++)
            data[i] = data[i + count];

        size -= count;
    }
};

int main() {
    Array<int> a(3);

    std::cout << "IsEmpty: " << a.IsEmpty() << std::endl;

    a.Add(1);
    a.Add(2);
    a.Add(3);
    a.Add(4);

    std::cout << "After Add: ";
    for (int i = 0; i <= a.GetUpperBound(); i++)
        std::cout << a[i] << " ";
    std::cout << std::endl;

    std::cout << "GetSize: " << a.GetSize() << std::endl;
    std::cout << "GetUpperBound: " << a.GetUpperBound() << std::endl;

    a.SetAt(1, 20);
    std::cout << "After SetAt: ";
    for (int i = 0; i <= a.GetUpperBound(); i++)
        std::cout << a.GetAt(i) << " ";
    std::cout << std::endl;

    a.InsertAt(99, 2);
    std::cout << "After InsertAt: ";
    for (int i = 0; i <= a.GetUpperBound(); i++)
        std::cout << a[i] << " ";
    std::cout << std::endl;

    a.RemoveAt(1);
    std::cout << "After RemoveAt: ";
    for (int i = 0; i <= a.GetUpperBound(); i++)
        std::cout << a[i] << " ";
    std::cout << std::endl;

    a.FreeExtra();
    std::cout << "After FreeExtra, size: " << a.GetSize() << std::endl;

    Array<int> b(2);
    b.Add(7);
    b.Add(8);

    a.Append(b);
    std::cout << "After Append: ";
    for (int i = 0; i <= a.GetUpperBound(); i++)
        std::cout << a[i] << " ";
    std::cout << std::endl;

    Array<int> c;
    c = a;
    std::cout << "After operator=: ";
    for (int i = 0; i <= c.GetUpperBound(); i++)
        std::cout << c[i] << " ";
    std::cout << std::endl;

    int* raw = c.GetData();
    std::cout << "GetData first element: " << raw[0] << std::endl;

    c.RemoveAll();
    std::cout << "After RemoveAll, IsEmpty: " << c.IsEmpty() << std::endl;

    return 0;
}
