﻿var choiceButtons = document.querySelectorAll('.choices__choice');
var submit = document.querySelector('.choices__submit');

function updateSelection(event) {
    choiceButtons.forEach((button, index) => {
        button.classList.remove('selected');
    });

    var clickedButton = event.currentTarget;
    clickedButton.classList.add('selected');

    submit.classList.add('enabled');
}



function hideChoices() {

    document.getElementById('hitbox').classList.remove('hidden');
    document.querySelector('.sidemenu').classList.remove('hidden');
    document.getElementById('location-choice').classList.remove('hidden');
    document.querySelector('.choices').classList.add('hidden');


}

function ShowChoices(){
    document.getElementById('hitbox').classList.add('hidden');
    document.querySelector('.sidemenu').classList.add('hidden');
    document.getElementById('location-choice').classList.add('hidden');

    var choicesDiv = document.querySelector('.choices');
    var choiceAButton = choicesDiv.querySelector('.button--choice:nth-of-type(1)');
    var choiceBButton = choicesDiv.querySelector('.button--choice:nth-of-type(2)');
    var descriptionParagraph = choicesDiv.querySelector('.choices__text');
    var data = locationResponseData;

    choiceAButton.textContent = data.choices.choiceA;
    choiceBButton.textContent = data.choices.choiceB;
    descriptionParagraph.textContent = data.choices.description;

    document.querySelector('.choices').classList.remove('hidden');

};


choiceButtons.forEach(button => {
    button.addEventListener('click', updateSelection);
});


if (document.body.contains(submit)) {
    submit.addEventListener('click', function () {
        if (!submit.classList.contains("enabled")) {
            return;
        }

        hideChoices();
        ChoiceNotAvailable();
        PlayerFocusChoice(document.querySelector('.choices__choice.selected').textContent);
        PlayerDealingTypeChoice(document.querySelector('.choices__choice.selected').textContent);
        setTimeout(function () {
            location.reload(true);
        }, 500);
    });
}


function ChoiceNotAvailable() {
    fetch('/Gameplay/SetChoiceNotAvailable', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        }
    });
}

function PlayerFocusChoice(choice) {
    var endChoice = "";
    if (choice == "SKELLETRON") {
        endChoice = "Physics";
    }
    else if (choice == "Brain Chip") {
        endChoice = "Hack"
    }
    else {
        return;
    }

    console.log(endChoice);
    MoralScoreChange(25);
    fetch('/Gameplay/PlayerFocusChoice', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(endChoice),
    });
}
function PlayerDealingTypeChoice(choice) {
    var endChoice = "";
    if (choice == "Violence") {
        endChoice = "Violent";
    }
    else if (choice == "Talk them Down") {
        endChoice = "Peaceful"
    }
    else {
        return;
    }

    console.log(endChoice);

    MoralScoreChange(25);
    fetch('/Gameplay/PlayerDealingTypeChoice', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(endChoice),
    });
}
function MoralScoreChange(amount) {

    fetch('/Gameplay/MoralScoreChange', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(amount),
    });
}

