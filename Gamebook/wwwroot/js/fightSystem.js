// changable variables from backend
console.log(locationResponseData.playerStats.maxHealth);


const energyCost = locationResponseData.equipedWeapon.energyConsumption;
//const playerDamage = locationResponseData.equipedWeapon.damage;
const playerDamage = 80;

const playerMaxHealth = locationResponseData.playerStats.maxHealth;
const playerMaxEnergy = locationResponseData.playerStats.maxEnergy;

const enemyMaxHealth = 100;
const enemyMaxDamage = 10;




function InitiateGame() {
    playerHealth = playerMaxHealth;
    playerEnergy = playerMaxEnergy;
    enemyHealth = enemyMaxHealth;
    currentAction = null;
    isGameOver = false;

    // show max health, energy
    document.getElementById("enemy-max-health").textContent = enemyMaxHealth;
    updateStats();


    // show stats, info box
    document.getElementById('player-stats').classList.remove('hidden');
    document.getElementById('enemy-stats').classList.remove('hidden');
    document.getElementById('info-box').classList.remove('hidden');

    // hide menu , hitbox and location choices
    document.getElementById('hitbox').classList.add('hidden');
    document.querySelector('.sidemenu').classList.add('hidden');
    document.getElementById('location-choice').classList.add('hidden');
    console.log("Game started");    

}

function initiateAttack() {
    if (playerEnergy >= -energyCost) { // if player has less energy than needed -> dont attack
        playerEnergy -= -energyCost;
        showMiniGame(() => { // show minigame
            if (checkHit()) { // if hit -> decrease health, update infobox
                currentAction = 'attack';
                enemyHealth -= playerDamage;
                console.log("You hit the enemy");
                updateInfoBox("You hit the enemy", -playerDamage, energyCost);
            } else { // if not hit -> update infobox
                currentAction = 'mised';
                console.log("You missed")
                updateInfoBox("You missed", null, energyCost);
            }
            updateStats();
            enemyTurn();
        });
    } else {
        console.log("Not enough energy to attack!");
        updateInfoBox("Not enough energy to attack!", null, null);
    }
}



function initiateMeditation() {
    currentAction = 'meditation';
    showMiniGame(() => {// show minigame
        if (checkHit()) { // if hit -> increase health and energy, update infobox
            playerHealth += 5;
            playerEnergy += 5;
            if (playerHealth > playerMaxHealth) playerHealth = playerMaxHealth;
            if (playerEnergy > playerMaxEnergy) playerEnergy = playerMaxEnergy;
            console.log("Your meditation has held");
            updateInfoBox("Your meditation has held", 5, 5);
        } else {
            console.log("Your meditation collapsed");
            updateInfoBox("Your meditation collapsed", null, null);
        }
        updateStats();
        enemyTurn();
    });
}


function enemyTurn() {
    if (isGameOver) return;
    checkGameOver();
    setTimeout(function () {
        if (isGameOver) return;
        console.log("It's opponent's turn.")
        updateInfoBox("It's opponent's turn.", null, null);
        setTimeout(function () {
            if (isGameOver) return;
            if (currentAction === "attack") {
                if (Math.random() < 0.6) { //TODO adjust values
                    enemyAttack();
                } else {
                    enemyDefend();
                }
            }
            else {
                enemyAttack();
            }
            document.getElementById('attack-btn').classList.remove('hidden')
            document.getElementById('meditate-btn').classList.remove('hidden')
        }, 1000);
    }, 1000);
}


function enemyAttack() {
    let damage = Math.floor(Math.random() * enemyMaxDamage) + 1; // Random damage between 1 and 10
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
    var enemyHealthPercentage = (enemyHealth / enemyMaxHealth) * 100;
    document.getElementById('bar-enemy-health').style.width = enemyHealthPercentage + '%';

    var playerHealthPercentage = (playerHealth / playerMaxHealth) * 100;
    document.getElementById('bar-player-health').style.width = playerHealthPercentage + '%';
    var playerEnergyPercentage = (playerEnergy / playerMaxEnergy) * 100;
    document.getElementById('bar-player-energy').style.width = playerEnergyPercentage + '%';


    // stats - health, energy
    document.getElementById("player-health").textContent = playerHealth;
    document.getElementById("player-energy").textContent = playerEnergy;
    document.getElementById("enemy-health").textContent = enemyHealth;
}

function checkGameOver() {
    if (playerHealth <= 0 || enemyHealth <= 0) {
        isGameOver = true;
        if (playerHealth <= 0) {
            console.log("Game Over! Enemy wins.");
            updateInfoBox("Game Over! Enemy wins.", null, null)
        } else if (enemyHealth <= 0) {
            console.log("Congratulations! You win!");
            updateInfoBox("Congratulations! You win!", null, null)
        }
        GameOver();
    }

}



function GameOver() {

    document.getElementById('attack-btn').disabled = true;
    document.getElementById('meditate-btn').disabled = true;

    hideMiniGame();


    // hide stats, infobox, hit button
    document.getElementById('player-stats').classList.add('hidden');
    document.getElementById('enemy-stats').classList.add('hidden');
    document.getElementById('hit-btn').classList.add('hidden');
    document.getElementById('info-box').classList.add('hidden');

    // show hitbox, menu and location choices
    document.getElementById('hitbox').classList.remove('hidden');
    document.querySelector('.sidemenu').classList.remove('hidden');
    document.getElementById('location-choice').classList.remove('hidden');

    //send data to backend
    HealthChange(playerHealth);
    EnergyChange(playerEnergy);

    console.log("backend updated")

    setTimeout(function () {
        location.reload(true);
    }, 200);
}




// Mini-game logic
let stick = null;
let greenSquare = null;
let container = null;
let stickInterval = null;

function showMiniGame(onComplete) {
    document.getElementById('attack-btn').classList.add('hidden')
    document.getElementById('meditate-btn').classList.add('hidden')

    document.getElementById('hit-btn').classList.remove('hidden')
    document.getElementById('game-container').classList.remove('hidden');
    container = document.getElementById('game-container');
    greenSquare = document.getElementById('green-square');
    stick = document.getElementById('stick');
    stick.style.left = '0px';
    randomizeGreenSquare();
    moveStick();

    document.getElementById('hit-btn').onclick = () => {
        onComplete();
        hideMiniGame();
    };
}
function hideMiniGame() {
    document.getElementById('hit-btn').classList.add('hidden')
    document.getElementById('game-container').classList.add('hidden');
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
    }, 1);
}
function checkHit() {
    let stickRect = stick.getBoundingClientRect();
    let greenRect = greenSquare.getBoundingClientRect();
    let containerRect = container.getBoundingClientRect();

    let stickX = stickRect.left - containerRect.left;
    let greenX = greenRect.left - containerRect.left;
    return stickX >= greenX && stickX <= (greenX + greenSquare.offsetWidth);
}


function updateInfoBox(text, healthChange, energyChange) {
    document.getElementById('info-box-text').textContent = text;
    document.getElementById('info-box-health').textContent = healthChange ? `${healthChange > 0 ? '+' : ''}${healthChange}hp` : '';
    document.getElementById('info-box-energy').textContent = energyChange ? `${energyChange > 0 ? '+' : ''}${energyChange}energy` : '';
}



function HealthChange(amount) {

    fetch('/Gameplay/HealthChange', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(amount),
    })
        .then(response => response.json())
        .then(data => {
            if (data.redirectToDeath) {
                window.location.href = data.redirectToDeath; // Handle redirection
            } else {
                console.log('Response:', data);
            }
        })
        .catch((error) => {
            console.error('Error:', error);
        });
}

function EnergyChange(amount) {

    fetch('/Gameplay/EnergyChange', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(amount),
    })
}
