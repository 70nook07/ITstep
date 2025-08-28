print("\nTask 1\n")

def check_age(age):
    if age < 0 or age > 120:
        raise ValueError("Invalid age: must be between 0 and 120")
    else:
        print("Your age is:", age)

try:
    age = int(input("Enter your age: "))
    check_age(age)
except ValueError as e:
    print("Error:", e)

print("\nTask 2\n")

def count_lines(filename):
    try:
        with open(filename, "r", encoding="utf-8") as f:
            count = 0
            for _ in f:
                count += 1
            return count
    except FileNotFoundError:
        print("File not found!")
        return 0

file_name = str(input("Enter file name to count lines in: "))
print("Number of lines:", count_lines(file_name))
