let currentDialogueIndex = 0;
let currentDialog = 0;


function updateDialogueText(Dialog) {

    const dialogue = locationResponseData.dialogs[currentDialogueIndex];

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
            closeButton.addEventListener('click', ToggleDialog);

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
        SetDialogNotAvailable();
    }

}


function SetDialogNotAvailable() {
    fetch('/Gameplay/SetDialogNotAvailable', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        }
    });
}