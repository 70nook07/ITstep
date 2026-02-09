#include <iostream>
#include <string>
using namespace std;

struct Car {
    float length;
    float clearance;
    float engineVolume;
    int enginePower;
    float wheelDiameter;
    string color;
    string transmission;
};

void setCar(Car& car) {
    cout << "Enter length: ";
    cin >> car.length;

    cout << "Enter clearance: ";
    cin >> car.clearance;

    cout << "Enter engine volume: ";
    cin >> car.engineVolume;

    cout << "Enter engine power: ";
    cin >> car.enginePower;

    cout << "Enter wheel diameter: ";
    cin >> car.wheelDiameter;

    cout << "Enter color: ";
    cin >> car.color;

    cout << "Enter transmission type: ";
    cin >> car.transmission;
}

void showCar(const Car& car) {
    cout << "Length: " << car.length << endl;
    cout << "Clearance: " << car.clearance << endl;
    cout << "Engine volume: " << car.engineVolume << endl;
    cout << "Engine power: " << car.enginePower << endl;
    cout << "Wheel diameter: " << car.wheelDiameter << endl;
    cout << "Color: " << car.color << endl;
    cout << "Transmission: " << car.transmission << endl;
    cout << "------------------------" << endl;
}

void searchByColor(Car* cars, int size, const string& color) {
    bool found = false;

    for (int i = 0; i < size; i++) {
        if (cars[i].color == color) {
            showCar(cars[i]);
            found = true;
        }
    }

    if (!found) {
        cout << "No cars with color " << color << " found." << endl;
    }
}

void sortByColor(Car* cars, int size) {
    for (int i = 0; i < size - 1; i++) {
        for (int j = 0; j < size - i - 1; j++) {
            if (cars[j].color > cars[j + 1].color) {
                swap(cars[j], cars[j + 1]);
            }
        }
    }
}

int main() {
    int size;
    cout << "Enter number of cars: ";
    cin >> size;

    Car* cars = new Car[size];

    for (int i = 0; i < size; i++) {
        cout << "\nCar " << i + 1 << endl;
        setCar(cars[i]);
    }

    cout << "\nAll cars:\n";
    for (int i = 0; i < size; i++) {
        showCar(cars[i]);
    }

    string searchColor;
    cout << "\nEnter color to search: ";
    cin >> searchColor;
    searchByColor(cars, size, searchColor);

    sortByColor(cars, size);
    cout << "\nCars sorted by color:\n";
    for (int i = 0; i < size; i++) {
        showCar(cars[i]);
    }

    delete[] cars;

    return 0;
}
