#include <iostream>
#include <cstring>

// 1
class Student {
protected:
    char name[50];
    int age;
    char university[50];

public:
    Student(const char* n = "", int a = 0, const char* u = "") {
        std::strcpy(name, n);
        age = a;
        std::strcpy(university, u);
    }

    void Output() const {
        std::cout << "Name: " << name << std::endl;
        std::cout << "Age: " << age << std::endl;
        std::cout << "University: " << university << std::endl;
    }
};

class Aspirant : public Student {
private:
    char thesisTopic[100];

public:
    Aspirant(const char* n, int a, const char* u, const char* t) : Student(n, a, u) {
        std::strcpy(thesisTopic, t);
    }

    void Output() const {
        Student::Output();
        std::cout << "Thesis topic: " << thesisTopic << std::endl;
    }
};

//2

class Passport {
protected:
    char fullName[50];
    char nationality[30];
    char passportNumber[20];

public:
    Passport(const char* n = "", const char* nat = "", const char* num = "") {
        std::strcpy(fullName, n);
        std::strcpy(nationality, nat);
        std::strcpy(passportNumber, num);
    }

    void Output() const {
        std::cout << "Name: " << fullName << std::endl;
        std::cout << "Nationality: " << nationality << std::endl;
        std::cout << "Passport number: " << passportNumber << std::endl;
    }
};

class ForeignPassport : public Passport {
private:
    char foreignNumber[20];
    char visas[100];

public:
    ForeignPassport(
        const char* n,
        const char* nat,
        const char* num,
        const char* fnum,
        const char* v
    ) : Passport(n, nat, num) {
        std::strcpy(foreignNumber, fnum);
        std::strcpy(visas, v);
    }

    void Output() const {
        Passport::Output();
        std::cout << "Foreign passport number: " << foreignNumber << std::endl;
        std::cout << "Visas: " << visas << std::endl;
    }
};

// 3

class ITransport {
public:
    virtual double GetTime(double distance) const = 0;
    virtual double GetCost(double distance, double weight) const = 0;
    virtual ~ITransport() {}
};

class Car : public ITransport {
public:
    double GetTime(double distance) const override {
        return distance / 80.0;
    }

    double GetCost(double distance, double weight) const override {
        return distance * 2.5 + weight;
    }
};

class Bicycle : public ITransport {
public:
    double GetTime(double distance) const override {
        return distance / 20.0;
    }

    double GetCost(double, double) const override {
        return 0.0;
    }
};

class Cart : public ITransport {
public:
    double GetTime(double distance) const override {
        return distance / 10.0;
    }

    double GetCost(double distance, double weight) const override {
        return distance * 1.0 + weight * 0.5;
    }
};

int main() {
    std::cout << "Task 1\n";
    Student s("Ivan", 20, "ONPU");
    s.Output();

    std::cout << std::endl;

    Aspirant a("Oleh", 25, "Mechnikova", "Psychology");
    a.Output();

    std::cout << "\nTAsk 2\n";
    Passport p("Vasyl Goroshko", "Ukraine", "AB123456");
    p.Output();

    std::cout << std::endl;

    ForeignPassport fp(
        "Vasyl Goroshko",
        "Ukraine",
        "AB123456",
        "FP987654",
        "Chech Rep., Germany"
    );
    fp.Output();

    std::cout << "\nTask 3\n";
    ITransport* t1 = new Car();
    ITransport* t2 = new Bicycle();
    ITransport* t3 = new Cart();

    double distance = 133;
    double weight = 40;

    std::cout << "Car time: " << t1->GetTime(distance)
              << " cost: " << t1->GetCost(distance, weight) << std::endl;

    std::cout << "Bicycle time: " << t2->GetTime(distance)
              << " cost: " << t2->GetCost(distance, weight) << std::endl;

    std::cout << "Cart time: " << t3->GetTime(distance)
              << " cost: " << t3->GetCost(distance, weight) << std::endl;

    delete t1;
    delete t2;
    delete t3;

    return 0;
}





