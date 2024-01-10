document.addEventListener('DOMContentLoaded', function () {

        console.log(locationResponseData);

});





document.addEventListener('DOMContentLoaded', function () {

    const hitbox = document.getElementById('hitbox');


    if (locationResponseData.hitbox == "Dialog") {
        hitbox.addEventListener('click', function () {
            ToggleDialog();
            SetHitboxNotAvailable();
        });
    }
    else if (locationResponseData.hitbox == "Fight") {
        hitbox.addEventListener('click', function () {
            InitiateGame();
            SetHitboxNotAvailable();
        });
    } else if (locationResponseData.hitbox == "Pin") {
        hitbox.addEventListener('click', function () {
            ShowPin()
            SetHitboxNotAvailable();
        });
    }


    const nextInDialogue = document.querySelector('.textbox__next');
    nextInDialogue.addEventListener('click', onNextButtonClick);

    const closePin = document.querySelector('.pin__close');
    closePin.addEventListener('click', HidePin);




    updateDialogueText(currentDialog);
}); 


function SetHitboxNotAvailable() {
    fetch('/Gameplay/SetHitboxNotAvailable ', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        }
    });
}