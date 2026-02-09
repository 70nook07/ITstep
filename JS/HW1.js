// 1.
let firstname = prompt("Enter your name:");
alert("Hello, " + firstname + "!");

// 2.
const CURRENT_YEAR = 2026;
let birthYear = Number(prompt("Enter your birth year:"));
let age = CURRENT_YEAR - birthYear;
alert("You are " + age + " years old.");

// 3.
let squareSide = Number(prompt("Enter the side length of the square:"));
let squarePerimeter = 4 * squareSide;
alert("The perimeter of the square is " + squarePerimeter);

// 4.
let radius = Number(prompt("Enter the radius of the circle:"));
const PI = 3.14159;
let circleArea = PI * radius ** 2;
alert("The area of the circle is " + circleArea);

// 5.
let distance = Number(prompt("Enter the distance between two cities (km):"));
let time = Number(prompt("Enter how many hours you want to travel:"));
let speed = distance / time;
alert("You need to move at a speed of " + speed + " km/h");

// 6.
const USD_TO_EUR = 0.84;
let dollars = Number(prompt("Enter amount in dollars:"));
let euros = dollars * USD_TO_EUR;
alert(dollars + " USD = " + euros + " EUR");

// 7.
let flashGB = Number(prompt("Enter flash drive size in GB:"));
let flashMB = flashGB * 1024;
let fileSize = 820;
let fileCount = Math.floor(flashMB / fileSize);
alert("You can store " + fileCount + " files of 820 MB");

// 8.
let money = Number(prompt("Enter the amount of money you have:"));
let chocolatePrice = Number(prompt("Enter the price of one chocolate:"));
let chocolates = Math.floor(money / chocolatePrice);
let change = money - chocolates * chocolatePrice;
alert(
  "You can buy " + chocolates + " chocolates and will have " + change + " money left"
);

// 9.
let number = Number(prompt("Enter a three-digit number:"));

let hundreds = Math.floor(number / 100);
let tens = Math.floor((number % 100) / 10);
let ones = number % 10;

let reversed = ones * 100 + tens * 10 + hundreds;
alert("Reversed number: " + reversed);

// 10.
let integer = Number(prompt("Enter an integer number:"));
let result = (integer % 2 === 0 && "Even") || "Odd";
alert("The number is " + result);
