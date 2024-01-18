﻿let currentDialog = 0;

function updateDialogueText() {
    if (locationResponseData.dialogs !== null) {
        const dialogue = locationResponseData.dialog;

        if (dialogue && dialogue.hasOwnProperty('texts')) {
            if (dialogue.texts.length >= currentDialog + 1 && dialogue.texts.length > 0) {
                const [character, text] = dialogue.texts[currentDialog].split(': ')
                const textBox = document.getElementById('textbox');

                if (character === 'Shadow Viper') {
                    textbox.classList.remove('textbox--red');
                }
                else {
                    textbox.classList.add('textbox--red');
                }

                const textBoxText = document.querySelector('.textbox__text');
                const textBoxCharacter = document.querySelector('.textbox__character');
                textBoxText.textContent = text;
                textBoxCharacter.textContent = character;

                if (dialogue.texts.length <= currentDialog + 1) {
                    const nextButton = document.querySelector('.textbox__next');
                    nextButton.classList.add('textbox__next--close');
                    const closeButton = document.querySelector('.textbox__next--close');

                    closeButton.addEventListener('click', function () {
                        ToggleDialog();
                        if (dialogue.itemsAdd !== null) {
                            dialogue.itemsAdd.forEach(item => {
                                AddItem(item);
                            })
                        }
                        if (dialogue.itemsRemove !== null) {
                            dialogue.itemsRemove.forEach(item => {
                                RemoveItem(item);
                            })
                        }

                        SetDialogNotAvailable();

                    });

                }
            }
        }
    }
}

function onNextButtonClick() {
    currentDialog++;
    updateDialogueText(currentDialog);
}

function ToggleDialog() {
    const choices = document.getElementById('location-choice');
    const textbox = document.getElementById('textbox');
    const menu = document.querySelector('.menu__swich');
    const hitbox = document.getElementById('hitbox');

    if (textbox.classList.contains('hidden')) {
        choices.classList.add('hidden');
        menu.classList.add('hidden');
        hitbox.classList.add('hidden');
        textbox.classList.remove('hidden');
    }
    else {
        menu.classList.remove('hidden');
        hitbox.classList.remove('hidden');
        textbox.classList.add('hidden');
    }

}

const infobox = document.getElementById('info-box');
const infoBoxText = document.getElementById('info-box-text');


function SetDialogNotAvailable() {
    console.log("Dialog over");
    fetch('/Gameplay/DialogOver', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        }
    })
    .then(response => {
        if (response.status === 200) {
            console.log("Request successful with status 200");
            reloadNow();
        } else {
            console.error("Request failed with status:", response.status);
        }
    });
}


function reloadNow() {
    const choices = document.getElementById('location-choice');
    choices.classList.remove('hidden');
    location.reload(true);
}

const nextInDialogue = document.querySelector('.textbox__next');
if (document.body.contains(nextInDialogue)) {
    nextInDialogue.addEventListener('click', onNextButtonClick);
}

function AddItem(item) {
    fetch('/Gameplay/AddItem', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(item),
    });

    console.log(item);
}
function RemoveItem(item) {
    fetch('/Gameplay/RemoveItem', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(item),
    });

    console.log(item);
}