document.addEventListener('DOMContentLoaded', function () {

        console.log(locationResponseData);

});





document.addEventListener('DOMContentLoaded', function () {

    const hitbox = document.getElementById('hitbox');

    
    //hitbox.addEventListener('click', ToggleDialog);
    //hitbox.addEventListener('click', InitiateGame); 

    hitbox.addEventListener('click', ShowPin);

    const nextInDialogue = document.querySelector('.textbox__next');
    nextInDialogue.addEventListener('click', onNextButtonClick);

    const closePin = document.querySelector('.pin__close');
    closePin.addEventListener('click', HidePin);




    updateDialogueText(currentDialog);
}); 

