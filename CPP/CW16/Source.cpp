#include "City.h"

int main() {
    string langs[] = { "Ukrainian", "English" };

    City city("Ukraine", "Kyiv", 2, langs);
    city.printInfo();

    return 0;
}
