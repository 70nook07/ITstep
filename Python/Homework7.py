import random

print("\nTASK 1\n")

size = int(input("Enter size of array: "))
arrRange = [random.randint(-50, 50) for _ in range(size)]

print(arrRange)

sumNeg = 0
i = 0
while i < len(arrRange):
    if arrRange[i] < 0:
        sumNeg += arrRange[i]
    i += 1

sumEven = 0
for x in arrRange:
    if x % 2 == 0:
        sumEven += x

sumOdd = 0
for x in arrRange:
    if x % 2 != 0:
        sumOdd += x

sumIndex3 = 1
i = 0
while i < len(arrRange):
    if i % 3 == 0:
        sumIndex3 *= arrRange[i]
    i += 1

minIndex = 0
maxIndex = 0
i = 1
while i < len(arrRange):
    if arrRange[i] < arrRange[minIndex]:
        minIndex = i
    if arrRange[i] > arrRange[maxIndex]:
        maxIndex = i
    i += 1

start = min(minIndex, maxIndex) + 1
end = max(minIndex, maxIndex)
betweenMinMax = 1
if start < end:
    i = start
    while i < end:
        betweenMinMax *= arrRange[i]
        i += 1
else:
    betweenMinMax = 0 

firstPos = -1
lastPos = -1
i = 0
while i < len(arrRange):
    if arrRange[i] > 0:
        if firstPos == -1:
            firstPos = i
        lastPos = i
    i += 1

sumBetweenPositives = 0
if firstPos != -1 and firstPos < lastPos:
    i = firstPos + 1
    while i < lastPos:
        sumBetweenPositives += arrRange[i]
        i += 1

print("Sum of negative numbers:", sumNeg)
print("Sum of even numbers:", sumEven)
print("Sum of odd numbers:", sumOdd)
print("Product of elements with indexes divisible by 3:", sumIndex3)
print("Product between min and max elements:", betweenMinMax)
print("Sum between first and last positive elements:", sumBetweenPositives)


print("\nTASK 2 (Using same array)\n")

evens = [x for x in arrRange if x % 2 == 0]
odds = [x for x in arrRange if x % 2 != 0]
negatives = [x for x in arrRange if x < 0]
positives = [x for x in arrRange if x > 0]

print("Evens:", evens)
print("Odds:", odds)
print("Negatives:", negatives)
print("Positives:", positives)


print("\nTASK 3\n")

strings = input("Enter strings separated by space: ").split()

shortest = strings[0]

for s in strings[1:]:
    if len(s) < len(shortest):
        shortest = s

print("Shortest string:", shortest)


print("\nTASK 4 (Using the same string array)\n")

letter = input("Enter the starting letter: ")

filtered = [s for s in strings if len(s) > 0 and s[0] == letter]

print("Strings starting with", letter, ":", filtered)


print("\nTASK 5\n")

def is_palindrome(s):
    return s == s[::-1]

palindromes = sorted([s for s in strings if is_palindrome(s)], key=len, reverse=True)

print("Palindromes sorted by length:", palindromes)

print("\nTASK 6\n")

# arrRange = [5, 10, -3, 7, 2]
target = int(input("Enter target number: "))

subsets = []

for mask in range(1, 1 << len(arrRange)):   # 1 << n == 2^n
    subset = [arrRange[i] for i in range(len(arrRange)) if mask & (1 << i)]
    if sum(subset) == target:
        subsets += [subset]
       
print("Subsets with sum", target, ":", subsets)
