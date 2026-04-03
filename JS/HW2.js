// TASK 1

let car = {
  manufacturer: "Toyota",
  model: "Corolla",
  year: 2020,
  averageSpeed: 80
};

function showCarInfo(car) {
  console.log("Manufacturer:", car.manufacturer);
  console.log("Model:", car.model);
  console.log("Year:", car.year);
  console.log("Average speed:", car.averageSpeed, "km/h");
}

function calculateTime(car, distance) {
  let time = distance / car.averageSpeed;
  let breaks = Math.floor(time / 4);
  return time + breaks;
}

showCarInfo(car);
console.log("Travel time:", calculateTime(car, 50), "hours\n");

// TASK 2

function createFraction(numerator, denominator) {
  return { numerator, denominator };
}

function addFractions(a, b) {
  return reduceFraction({
    numerator: a.numerator * b.denominator + b.numerator * a.denominator,
    denominator: a.denominator * b.denominator
  });
}

function subtractFractions(a, b) {
  return reduceFraction({
    numerator: a.numerator * b.denominator - b.numerator * a.denominator,
    denominator: a.denominator * b.denominator
  });
}

function multiplyFractions(a, b) {
  return reduceFraction({
    numerator: a.numerator * b.numerator,
    denominator: a.denominator * b.denominator
  });
}

function divideFractions(a, b) {
  return reduceFraction({
    numerator: a.numerator * b.denominator,
    denominator: a.denominator * b.numerator
  });
}

function reduceFraction(frac) {
  function divisor(a, b) {
    return b === 0 ? a : divisor(b, a % b);
  }

  let vdivisor = divisor(frac.numerator, frac.denominator);
  return {
    numerator: frac.numerator / vdivisor,
    denominator: frac.denominator / vdivisor
  };
}

let f1 = createFraction(40, 2);
let f2 = createFraction(34, 4);

console.log("Add:", addFractions(f1, f2));
console.log("Subtract:", subtractFractions(f1, f2));
console.log("Multiply:", multiplyFractions(f1, f2));
console.log("Divide:", divideFractions(f1, f2));

// TASK 3

let time = {
  hours: 20,
  minutes: 30,
  seconds: 45
};

function showTime(t) {
  console.log(
    String(t.hours).padStart(2, '0') + ":" +
    String(t.minutes).padStart(2, '0') + ":" +
    String(t.seconds).padStart(2, '0')
  );
}

function timeNormalize(t) {
  if (t.seconds >= 60) {
    t.minutes += Math.floor(t.seconds / 60);
    t.seconds %= 60;
  }

  if (t.minutes >= 60) {
    t.hours += Math.floor(t.minutes / 60);
    t.minutes %= 60;
  }

  if (t.hours >= 24) {
    t.hours %= 24;
  }
}

function addSecs(t, sec) {
  t.seconds += sec;
  timeNormalize(t);
}

function addMins(t, min) {
  t.minutes += min;
  timeNormalize(t);
}

function addHours(t, hrs) {
  t.hours += hrs;
  timeNormalize(t);
}

showTime(time);
addSecs(time, 40);
showTime(time);

addMins(time, 10);
showTime(time);

addHours(time, 51);
showTime(time);
