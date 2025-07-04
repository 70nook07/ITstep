import random

while True:
    choice = int(input("\nChoose a task number (1–3), or 0 to exit: "))

    match choice:
        case 1:
            num = random.randint(1, 100)
            guess = 0
            while num != guess:
                guess = int(input("\nGuess a number (1-100): "))
                if guess > 100 and guess < 1:
                    print("\n\nERROR: Number not in range!")
                    continue
                if guess > num:
                    print("\n\nLower!")
                elif guess < num:
                    print("\n\nHigher!")
                else: 
                    print("You WIN!")
        case 2:
            total = 0.0

            while True:
                price = float(input("Enter item price (0 to finish): "))
                if price == 0:
                    break
                total += price

            print(f"Total purchase amount: ${total}")

        case 3:
            candidate1 = 0
            candidate2 = 0
            candidate3 = 0

            for i in range(1, 4):
                votes = int(input(f"Votes for Candidate {i}: "))
                if i == 1:
                    candidate1 = votes
                elif i == 2:
                    candidate2 = votes
                else:
                    candidate3 = votes


            if candidate1 > candidate2 and candidate1 > candidate3:
                winner = "Candidate 1"
            elif candidate2 > candidate1 and candidate2 > candidate3:
                winner = "Candidate 2"
            elif candidate3 > candidate1 and candidate3 > candidate2:
                winner = "Candidate 3"
            else:
                winner = "No clear winner (tie)"

            print(f"The winner is: {winner}")

        case 0:
            print("Goodbye!")
            break

        case _:
            print("Invalid task number. Please try again.")

