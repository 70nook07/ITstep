#include <iostream>
#include <fstream>
#include <vector>
#include <string>

using namespace std;

class Directory
{
private:
    string companyName;
    string owner;
    string phone;
    string address;
    string activity;

public:
    Directory() {}

    Directory(string c, string o, string p, string a, string act) : companyName(c), owner(o), phone(p), address(a), activity(act) {}

    string getCompanyName() const { return companyName; }
    string getPhone() const { return phone; }

    void show() const
    {
        cout << "Company: " << companyName << endl;
        cout << "Owner: " << owner << endl;
        cout << "Phone: " << phone << endl;
        cout << "Address: " << address << endl;
        cout << "Activity: " << activity << endl;
        cout << "-------------------\n";
    }

    string toFileString() const
    {
        return companyName + ";" + owner + ";" + phone + ";" + address + ";" + activity;
    }

    static Directory fromFileString(string line)
    {
        string c, o, p, a, act;
        size_t pos;

        pos = line.find(";"); c = line.substr(0, pos); line.erase(0, pos + 1);
        pos = line.find(";"); o = line.substr(0, pos); line.erase(0, pos + 1);
        pos = line.find(";"); p = line.substr(0, pos); line.erase(0, pos + 1);
        pos = line.find(";"); a = line.substr(0, pos); line.erase(0, pos + 1);
        act = line;

        return Directory(c, o, p, a, act);
    }
};

vector<Directory> loadAll()
{
    vector<Directory> list;
    ifstream file("data.txt");
    string line;

    while (getline(file, line)) list.push_back(Directory::fromFileString(line));

    return list;
}

void addRecord()
{
    string c, o, p, a, act;

    cout << "Company: ";
    getline(cin, c);
    cout << "Owner: ";
    getline(cin, o);
    cout << "Phone: ";
    getline(cin, p);
    cout << "Address: ";
    getline(cin, a);
    cout << "Activity: ";
    getline(cin, act);

    Directory dir(c, o, p, a, act);

    ofstream file("data.txt", ios::app);
    file << dir.toFileString() << endl;

    cout << "Record added!\n";
}

void showAll()
{
    auto list = loadAll();
    for (auto& dir : list)
        dir.show();
}

void searchByName()
{
    string name;
    cout << "Enter company name: ";
    getline(cin, name);

    auto list = loadAll();
    for (auto& dir : list)
        if (dir.getCompanyName() == name)
            dir.show();
}

void searchByPhone()
{
    string phone;
    cout << "Enter phone: ";
    getline(cin, phone);

    auto list = loadAll();
    for (auto& dir : list)
        if (dir.getPhone() == phone)
            dir.show();
}

int main()
{
    int choice;

    while (true)
    {
        cout << "\n1. Add\n";
        cout << "2. Show all\n";
        cout << "3. Search by name\n";
        cout << "4. Search by phone\n";
        cout << "0. Exit\n";
        cout << "Choice: ";

        cin >> choice;
        cin.ignore();

        if (choice == 0) break;

        switch (choice)	{
	        case 1: addRecord(); break;
	        case 2: showAll(); break;
	        case 3: searchByName(); break;
	        case 4: searchByPhone(); break;
        }
    }

    return 0;
}



