import random

while True:
    print("\nChoose task number (1–4), or 0 to exit:")
    choice = input("Your choice: ")

    choice = int(choice)

    match choice:

        case 0:
            print("Exiting...")
            break

        case 1:
            # Task 1
            rows = int(input("Input desired number of rows: "))
            cols = int(input("Input desired number of columns: "))

            i = 0
            while i < rows:
                j = 0
                row = [0] * cols
                while j < cols:
                    num = random.randint(3, 130)
                    row[j] = num
                    j += 1

                max_val = row[0]
                min_val = row[0]
                k = 1
                while k < len(row):
                    if row[k] > max_val:
                        max_val = row[k]
                    if row[k] < min_val:
                        min_val = row[k]
                    k += 1

                for num in row:
                    print(num, end="\t")
                print(f"max:{max_val} min:{min_val}\n")
                i += 1

        case 2:
            # Task 2
            text = input("Input text string: ")

            letters = 0
            digits = 0
            i = 0

            while i < len(text):
                ch = text[i]

                if ('a' <= ch <= 'z') or ('A' <= ch <= 'Z'):
                    letters += 1
                elif '0' <= ch <= '9':
                    digits += 1

                i += 1

            print(f"Number of letters: {letters}")
            print(f"Number of digits: {digits}")

        case 3:
            # Task 3
            text = input("Input text string: ")

            reversedText = ""
            i = 0

            while i < len(text):
                reversedText = text[i] + reversedText
                i += 1

            print(f"Reversed text string: {reversedText}")

        case 4:
            # Task 4
            text = input("Input text string: ")

            i = 0
            current_word = ""
            words = []

            while i < len(text):
                ch = text[i]

                if ch != ' ':
                    current_word = current_word + ch
                else:
                    if current_word != "":
                        words += [current_word]
                        current_word = ""
                i += 1

            if current_word != "":
                words += [current_word]

            longest_word = words[0]
            j = 1
            while j < len(words):
                if len(words[j]) > len(longest_word):
                    longest_word = words[j]
                j += 1

            print("Longest word:", longest_word)

        case _:
            print("Invalid choice. Please enter 1–4, or 0 to exit.")
