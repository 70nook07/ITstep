#include "City.h"
#include <iostream>

using namespace std;

City::City() {
    country = "Unknown";
    capital = "Unknown";
    count = 0;
    languages = nullptr;
}

City::City(string country, string capital, int count, string langs[]) {
    this->country = country;
    this->capital = capital;
    this->count = count;

    languages = new string[count];
    for (int i = 0; i < count; i++){
        languages[i] = langs[i];
    }
}

City::~City() {
    delete[] languages;
}

string City::getCountry() { return country; }
string City::getCapital() { return capital; }
int City::getCount() { return count; }

void City::setCountry(string country) {
    this->country = country;
}

void City::setCapital(string capital) {
    this->capital = capital;
}

void City::setCount(int count)
{
    this->count = count;
}

void City::printInfo() {
    cout << "Country: " << country << endl;
    cout << "Capital: " << capital << endl;
    cout << "Languages, " << count << ": " << endl;

    for (int i = 0; i < count; i++) {
        cout << "- " << languages[i] << endl;
    }
}
