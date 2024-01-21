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
        if (data.hitbox == "Fight") {
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
        else if (data.hitbox == "Hack") {
            hitbox.addEventListener('click', function () {
                ShowHack()
                SetHitboxNotAvailable();
            });
        }
        else if (data.dialog != null && data.hitbox == "") {
            hitbox.addEventListener('click', function () {
                updateDialogueText();
                ToggleDialog();
            });
        }
    }

    if (data.hitbox == "" && data.dialog == null && data.choices == null) {
        hitbox.classList.add("hidden");
    }



}); 


function SetHitboxNotAvailable() {
    console.log("Hitbox over");
    fetch('/Gameplay/SetHitboxNotAvailable ', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        }
    });
}