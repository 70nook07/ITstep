#include <iostream>
using namespace std;


// TASK 1
class ITransport
{
public:
    virtual void Drive() = 0;
};

class Auto : public ITransport
{
public:
    void Drive() override
    {
        cout << "The car drives on the road.\n";
    }
};

class Driver
{
public:
    void Travel(ITransport* transport)
    {
        transport->Drive();
    }
};

class IAnimal
{
public:
    virtual void Move() = 0;
};

class Camel : public IAnimal
{
public:
    void Move() override
    {
        cout << "Camel rides on the sands of the desert.\n";
    }
};

class Horse : public IAnimal
{
public:
    void Move() override
    {
        cout << "Horse runs across the plain.\n";
    }
};

class Elephant : public IAnimal
{
public:
    void Move() override
    {
        cout << "Elephant walks through the savanna.\n";
    }
};

// adapter
class AnimalToTransportAdapter : public ITransport
{
    IAnimal* animal;
public:
    AnimalToTransportAdapter(IAnimal* a)
    {
        animal = a;
    }

    void Drive() override
    {
        animal->Move();
    }
};

// TASK 2
class Engine
{
protected:
    string engineType;

public:
    Engine(string type)
    {
        engineType = type;
    }

    void StartEngine()
    {
        cout << "Engine started: " << engineType << endl;
    }
};

class Car : virtual public Engine
{
public:
    Car(string type) : Engine(type) {}

    void Drive()
    {
        StartEngine();
        cout << "Car drives on the road.\n";
    }
};

class Boat : virtual public Engine
{
public:
    Boat(string type) : Engine(type) {}

    void Sail()
    {
        StartEngine();
        cout << "Boat sails on the water.\n";
    }
};

class AmphibiousVehicle : public Car, public Boat
{
public:
    AmphibiousVehicle(string type)
        : Engine(type), Car(type), Boat(type) {}

    void MoveOnLand()
    {
        Drive();
    }

    void MoveOnWater()
    {
        Sail();
    }
};

int main()
{
	cout << "\nTASK 1\n";
    Driver driver;
    Auto car1;
    driver.Travel(&car1);

    Camel camel;
    Horse horse;
    Elephant elephant;

    AnimalToTransportAdapter camelAdapter(&camel);
    AnimalToTransportAdapter horseAdapter(&horse);
    AnimalToTransportAdapter elephantAdapter(&elephant);

    driver.Travel(&camelAdapter);
    driver.Travel(&horseAdapter);
    driver.Travel(&elephantAdapter);
    
    
    cout << "\nTASK 2\n";
    Car car2("V8");
    car2.Drive();

    Boat boat("V1");
    boat.Sail();

    AmphibiousVehicle amphibious("V6");
    amphibious.MoveOnLand();
    amphibious.MoveOnWater();

    return 0;
}





