print("\nTask 1\n")

def show_quote():
    print('"Don\'t compare yourself with anyone in this world…')
    print('    if you do so, you are insulting yourself."')
    print('        Bill Gates')

show_quote()


print("\nTask 2\n")

def show_even(a, b):
    for num in range(a + 1, b):
        if num % 2 == 0:
            print(num, end=" ")
    print()

show_even(
    int(input("Enter 1st number: ")),
    int(input("Enter 2nd number: "))
    )


print("\nTask 3\n")

def draw_square(size, symbol, filled):
    for i in range(size):
        if filled or i == 0 or i == size - 1:
            print(symbol * size)
        else:
            print(symbol + " " * (size - 2) + symbol)



draw_square(
    int(input("Enter square size: ")),
    input("Enter symbol to use: "),
    bool(input("Is it filled(true) or empty(false)? "))
    )


print("\nTask 4\n")

def min_of_five(a, b, c, d, e):
    minimum = a
    for x in [b, c, d, e]:
        if x < minimum:
            minimum = x
    return minimum

print(min_of_five(10, 3, 7, -2, 15))   # == -2


print("\nTask 5\n")

def count_digits(n):
    n = abs(n)
    count = 0
    while n > 0:
        n //= 10
        count += 1
    return count if count > 0 else 1  # handle n=0


print(count_digits(int(input("Enter a number to count digits: "))))

