let currentDialog = 0;


function updateDialogueText(Dialog) {
    if (locationResponseData.dialogs !== null) {
        const dialogue = locationResponseData.dialogs[0];

        if (dialogue && dialogue.hasOwnProperty('texts')) {
            if (dialogue.texts.length >= Dialog + 1 && dialogue.texts.length > 0) {
                const [character, text] = dialogue.texts[Dialog].split(': ')
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

                if (dialogue.texts.length <= Dialog + 1) {
                    const nextButton = document.querySelector('.textbox__next');
                    nextButton.classList.add('textbox__next--close');
                    const closeButton = document.querySelector('.textbox__next--close');
                    closeButton.addEventListener('click', function () {
                        ToggleDialog();
                        SetDialogNotAvailable();
                        if (dialogue.unlock !== null) {
                            dialogue.unlock.forEach(location => {
                                UnlockLocation(location);
                                console.log(location);
                            })
                        }
                        reload();
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

    if (textbox.classList.contains('hidden')) {
        choices.classList.add('hidden');
        textbox.classList.remove('hidden');
    }
    else {
        choices.classList.remove('hidden');
        textbox.classList.add('hidden');
    }

}

const infobox = document.getElementById('info-box');
const infoBoxText = document.getElementById('info-box-text');


function UnlockLocation(location) {
    fetch('/Gameplay/UnlockLocation', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(location),
    });

    infobox.classList.remove('hidden');
    infoBoxText.textContent = `Location ${location} unlocked`;
    setTimeout(function () {
        infobox.classList.add('hidden');
    }, 500);
}

function SetDialogNotAvailable() {
    fetch('/Gameplay/SetDialogNotAvailable', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        }
    });
}

function reload() {
    setTimeout(function () {
        location.reload(true);
    }, 500);
}

const nextInDialogue = document.querySelector('.textbox__next');
if (document.body.contains(nextInDialogue)) {
    nextInDialogue.addEventListener('click', onNextButtonClick);
}