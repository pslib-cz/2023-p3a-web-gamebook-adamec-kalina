const energyCost = 10;
const playerDamage = 25;

const playerMaxHealth = 50;
const playerMaxEnergy = 50;

const enemyMaxHealth = 100;


let playerHealth = playerMaxHealth;
let playerEnergy = playerMaxEnergy;
let enemyHealth = enemyMaxHealth;
let miniGameSuccess = null;
let currentAction = null;



function initiateAttack() {
  if (playerEnergy >= energyCost) {
    currentAction = 'attack';
    showMiniGame();
    playerEnergy -= energyCost;
    miniGameSuccess = () => {
            enemyHealth -= playerDamage;
            console.log("You hit the enemy");
            updateInfoBox("You hit the enemy", -playerDamage, -energyCost);
            updateStats();
            enemyTurn();
          };
  } else {
    console.log("Not enough energy to attack!");
    updateInfoBox("Not enough energy to attack!", null, null);
  }
}

function initiateDefense() {
    currentAction = 'defense';
    showMiniGame();
    miniGameSuccess = () => {
        playerHealth += 5; //todo increase/decrease
        playerEnergy += 5; //todo increase/decrease
        if (playerHealth > playerMaxHealth) playerHealth = playerMaxHealth;
        if (playerEnergy > playerMaxEnergy) playerEnergy = playerMaxEnergy;
        console.log("Your defense has held");
        updateInfoBox("Your defense has held", 5, 5); //todo increase/decrease
        updateStats();
        enemyTurn();
    };
}

function enemyTurn() {
  checkGameOver();
  setTimeout(function() {
    console.log("It's opponent's turn.")
    updateInfoBox("It's opponent's turn.", null, null);
    setTimeout(function() {
      if (currentAction === "attack"){
        if(Math.random() < 0.5){
          enemyAttack();
        } else {
          enemyDefend();
        }
      }
      else{
        enemyAttack();
      }
      document.getElementById('attack-btn').classList.remove('hidden')
      document.getElementById('defence-btn').classList.remove('hidden')
    }, 1000);
  }, 1000);
}


function enemyAttack() {
    let damage = Math.floor(Math.random() * 10) + 1; // Random damage between 1 and 10 todo increase/decrease
    playerHealth -= damage;
    console.log("Opponent hit you.")
    updateInfoBox("Opponent hit you", -damage, null);
    updateStats();
    checkGameOver();



}

function enemyDefend() {
    enemyHealth += playerDamage;
    if (enemyHealth > enemyMaxHealth) enemyHealth = enemyMaxHealth;
    console.log("Opponent has dodged the attack")
    updateInfoBox("Opponent has dodged the attack", null, null);
    updateStats();



}

function updateStats() {
    document.getElementById("player-health").textContent = playerHealth;
    document.getElementById("player-energy").textContent = playerEnergy;
    document.getElementById("player-max-health").textContent = playerMaxHealth;
    document.getElementById("player-max-energy").textContent = playerMaxEnergy;
    document.getElementById("enemy-max-health").textContent = enemyMaxHealth;
    document.getElementById("enemy-health").textContent = enemyHealth;
}

function checkGameOver() {
    if (playerHealth <= 0) {
        console.log("Game Over! Enemy wins.");
        resetGame();
    } else if (enemyHealth <= 0) {
        console.log("Congratulations! You win!");
        resetGame();
    }
}

function resetGame() {
    playerHealth = playerMaxHealth;
    playerEnergy = playerMaxEnergy;
    enemyHealth = enemyMaxHealth;
    hideMiniGame();
    updateStats();
}








// Mini-game logic
let stick = null;
let greenSquare = null;
let container = null;
let stickInterval = null;

function showMiniGame() {
    document.getElementById('attack-btn').classList.add('hidden')
    document.getElementById('defence-btn').classList.add('hidden')
    document.getElementById('hit-btn').classList.remove('hidden')


    document.getElementById('mini-game').classList.remove('hidden');
    container = document.getElementById('game-container');
    greenSquare = document.getElementById('green-square');
    stick = document.getElementById('stick');
    stick.style.left = '0px'; // Reset stick position
    randomizeGreenSquare();
    moveStick();
}
function hideMiniGame() {
    document.getElementById('hit-btn').classList.add('hidden')


    document.getElementById('mini-game').classList.add('hidden');
    if (stickInterval) {
        clearInterval(stickInterval);
        stickInterval = null;
    }
}
function randomizeGreenSquare() {
    let maxLeft = container.offsetWidth - greenSquare.offsetWidth;
    greenSquare.style.left = Math.floor(Math.random() * maxLeft) + 'px';
}
function moveStick() {
    if (stickInterval) clearInterval(stickInterval);
    stickInterval = setInterval(() => {
        let stickPos = parseInt(stick.style.left, 10) || 0;
        stickPos += 1;
        if (stickPos > container.offsetWidth - stick.offsetWidth) {
            stickPos = 0;
        }
        stick.style.left = stickPos + 'px';
    }, 5);
}
function checkHit() {
    let stickRect = stick.getBoundingClientRect();
    let greenRect = greenSquare.getBoundingClientRect();
    let containerRect = container.getBoundingClientRect();

    let stickX = stickRect.left - containerRect.left;
    let greenX = greenRect.left - containerRect.left;
    if (stickX >= greenX && stickX <= (greenX + greenSquare.offsetWidth)) {
        hideMiniGame();
        if (typeof miniGameSuccess === 'function') {
            miniGameSuccess();
        }
    } else {
      hideMiniGame();
      if (currentAction === 'attack') {
          console.log("You missed")
          updateInfoBox("You missed", null, -energyCost);
          enemyTurn();
        } else if (currentAction === 'defense') {
          console.log("Your defense collapsed")
          updateInfoBox("Your defense collapsed", null, null);
          enemyTurn();
      }
    }
}



function updateInfoBox(text, healthChange, energyChange) {
  document.getElementById('info-box-text').textContent = text;
  document.getElementById('info-box-health').textContent = healthChange ? `${healthChange > 0 ? '+' : ''}${healthChange}hp` : '';
  document.getElementById('info-box-energy').textContent = energyChange ? `${energyChange > 0 ? '+' : ''}${energyChange}energy` : '';
}


updateStats();
