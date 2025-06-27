while True:
    print("\nChoose a task number (1–10), or 0 to exit:")
    choice = int(input("Choose: "))

    match choice:
        case 1:
            count = 0
            while count < 5:
                print("Hello, world!")
                count += 1

        case 2:
            i = 1
            while i <= 10:
                print(i, end=", " if i < 10 else "\n")
                i += 1

        case 3:
            i = 2
            while i <= 20:
                print(i, end=", " if i < 20 else "\n")
                i += 2

        case 4:
            i = 0
            while i < 10:
                print("*", end="")
                i += 1

        case 5:
            i = 10
            while i >= 1:
                print(i, end=", " if i > 1 else "\n")
                i -= 1

        case 6:
            i = 1
            total = 0
            while i <= 100:
                total += i
                i += 1
            print("Sum from 1 to 100:", total)

        case 7:
            row = 0
            while row < 5:
                col = 0
                while col < 10:
                    print("*", end="" if col < 9 else "\n")  # Changed condition to col < 9
                    col += 1
                row += 1



        case 8:
            i = 1
            num = 3
            while i <= 10:
                print(f"{num} x {i} = {num * i}")
                i += 1

        case 9:
            i = 1
            count = 0
            while i <= 100:
                if i % 2 != 0:
                    count += 1
                i += 1
            print("Count of odd numbers from 1 to 100:", count)

        case 10:
            n = int(input("Enter a number n: "))
            i = 1
            while i <= n:
                print(i, end=", " if i < n else "\n")
                i += 1

        case 0:
            print("Goodbye!")
            break

        case _:
            print("Invalid task number. Please try again.")
