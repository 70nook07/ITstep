#ifndef CITY_H
#define CITY_H

#include <string>
using namespace std;

class City {
private:
    string country;
    string capital;
    int count;
    string* languages;

public:
    City();
    City(string country, string capital, int count, string langs[]);
    ~City();

    string getCountry();
    string getCapital();
    int getCount();

    void setCountry(string country);
    void setCapital(string capital);
    void setCount(int count);

    void printInfo();
};

#endif
