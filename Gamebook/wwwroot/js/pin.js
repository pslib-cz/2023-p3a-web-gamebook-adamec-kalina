let codeLength = 5;
let code = generateCode();
let attempts = 10;

function generateCode() {
    let numbers = new Set();
    let X = Math.floor(Math.random() * 9);
    let Y;

    do {
        Y = Math.floor(Math.random() * 9);
    } while (Y === X);

    numbers.add(X);
    numbers.add(X + 1);
    numbers.add(Y);
    numbers.add(Y + 1);

    // Fill the middle part of the code
    while (numbers.size < codeLength) {
        let num = Math.floor(Math.random() * 10);
        if (!numbers.has(num)) {
            numbers.add(num);
        }
    }

    // Convert set to array and arrange as XY....(Y+1)(X+1)
    let codeArray = Array.from(numbers);
    let XIndex = codeArray.indexOf(X);
    let YIndex = codeArray.indexOf(Y);

    // Swap to position X and Y at the beginning
    [codeArray[0], codeArray[XIndex]] = [codeArray[XIndex], codeArray[0]];
    [codeArray[1], codeArray[YIndex]] = [codeArray[YIndex], codeArray[1]];

    // Ensure (Y+1) and (X+1) are at the end
    let YPlusIndex = codeArray.indexOf(Y + 1);
    let XPlusIndex = codeArray.indexOf(X + 1);
    
    [codeArray[codeLength - 2], codeArray[YPlusIndex]] = [codeArray[YPlusIndex], codeArray[codeLength - 2]];
    [codeArray[codeLength - 1], codeArray[XPlusIndex]] = [codeArray[XPlusIndex], codeArray[codeLength - 1]];

    console.log(codeArray);
    return codeArray.join('');
    
}






function submitGuess() {
    let guess = document.getElementById('pin-input').value;
    if (guess.length !== codeLength) {
        alert(`Please enter a ${codeLength}-digit code.`);
        return;
    }

    let result = checkGuess(guess);
    document.getElementById('pin-history').innerHTML += `<p>${guess}: ${result.W}<span>W</span> ${result.L}<span>L</span></p>`;

    attempts--;
    document.getElementById('pin-attempts').innerHTML = `Attempts left: ${attempts}`;

    if (result.W === codeLength) {
        alert('Congratulations! You cracked the code!');
        return;
    }

    if (attempts === 0) {
        alert(`Game over! The code was ${code.join('')}.`);
    }
}

function checkGuess(guess) {
    let correctPositions = 0;
    let correctNumbers = 0;
    let guessArray = guess.split('');
    let tempCode = [...code];

    // First pass for correct positions (W)
    guessArray.forEach((num, idx) => {
        if (num === tempCode[idx]) {
            correctPositions++;
            tempCode[idx] = null; 
            guessArray[idx] = '-'; 
        }
    });

    // Second pass for correct numbers but wrong positions (L)
    guessArray.forEach((num, idx) => {
        if (tempCode.includes(num)) {
            correctNumbers++;
            tempCode[tempCode.indexOf(num)] = null;
        }
    });

    return { W: correctPositions, L: correctNumbers };
}

function ShowPin() {
    document.getElementById('hitbox').classList.add('hidden');
    document.querySelector('.sidemenu').classList.add('hidden');
    document.getElementById('location-choice').classList.add('hidden');

    document.getElementById('pin').classList.remove('pin--hidden');  
    console.log('ShowPin');
}

function HidePin() {
    document.getElementById('hitbox').classList.remove('hidden');
    document.querySelector('.sidemenu').classList.remove('hidden');
    document.getElementById('location-choice').classList.remove('hidden');

    document.getElementById('pin').classList.add('pin--hidden');
}

const closePin = document.querySelector('.pin__close');
if (document.body.contains(closePin)) {
    closePin.addEventListener('click', HidePin);
}