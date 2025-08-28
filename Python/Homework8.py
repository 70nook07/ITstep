print("\nTask 1\n")

nums = [int(x) for x in input("Enter numbers separated by space: ").split()]

result = [x for x in nums if nums.count(x) == 1]

print("Elements that appear once:", result)


print("\nTask 2\n")

nums = [int(x) for x in input("Enter numbers separated by space: ").split()]

longest = []
current = []
current.append(nums[0])

for i in range(1, len(nums)):
    if nums[i] > nums[i - 1]:
        current.append(nums[i])
    else:
        if len(current) > len(longest):
            longest = current
        current = []
        current.append(nums[i])

if len(current) > len(longest):
    longest = current

print("Length of sequence:", len(longest))
print("Sequence:", longest)


print("\nTask 3\n")

nums = [int(x) for x in input("Enter numbers separated by space: ").split()]

# bubble sort
for i in range(len(nums)):
    for j in range(0, len(nums) - i - 1):
        if nums[j] > nums[j + 1]:
            nums[j], nums[j + 1] = nums[j + 1], nums[j]

result = []
while len(nums) > 0:
    if len(nums) > 0:
        result.append(nums.pop(0))   # smallest
    if len(nums) > 0:
        result.append(nums.pop(-1))  # largest

print("Alternating order:", result)
