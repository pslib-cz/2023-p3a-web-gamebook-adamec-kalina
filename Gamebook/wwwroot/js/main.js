document.addEventListener('DOMContentLoaded', function () {

        console.log(locationResponseData);

});





document.addEventListener('DOMContentLoaded', function () {

    const startDialog = document.getElementById('hitbox');
    startDialog.addEventListener('click', ToggleDialog);

    const nextButton = document.querySelector('.textbox__next');
    nextButton.addEventListener('click', onNextButtonClick);



    updateDialogueText(currentDialog);
}); 

