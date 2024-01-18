document.addEventListener('DOMContentLoaded', function () {

    const hitbox = document.getElementById('hitbox');
    const data = locationResponseData;
    console.log(data);

    if (data.choices != null) {
        hitbox.addEventListener('click', function () {
            ShowChoices()
        });
    }
    else {
        if (data.hitbox == "Dialog") {
            hitbox.addEventListener('click', function () {
                updateDialogueText(currentDialog);
                ToggleDialog();
                SetHitboxNotAvailable();
            });
        }
        else if (data.hitbox == "Fight") {
            hitbox.addEventListener('click', function () {
                InitiateGame();
                SetHitboxNotAvailable();
            });
        }
        else if (data.hitbox == "Pin") {
            hitbox.addEventListener('click', function () {
                ShowPin()
                SetHitboxNotAvailable();
            });
        }
    }

    if (data.hitbox == "") {
        hitbox.classList.add("hidden");
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