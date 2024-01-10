document.addEventListener('DOMContentLoaded', function () {

        console.log(locationResponseData);

});





document.addEventListener('DOMContentLoaded', function () {

    const hitbox = document.getElementById('hitbox');

    
    if (locationResponseData.hitbox == "Dialog") {
        hitbox.addEventListener('click', function () {
            updateDialogueText(currentDialog);
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
    } else if (locationResponseData.hitbox == "Choice") {
        hitbox.addEventListener('click', function () {
                ShowChoices()
                SetHitboxNotAvailable();
        });
    }





}); 


function SetHitboxNotAvailable() {
    fetch('/Gameplay/SetHitboxNotAvailable ', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        }
    });
}