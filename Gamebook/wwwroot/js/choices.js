var choiceButtons = document.querySelectorAll('.choices__choice');
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
    if (!submit.classList.contains("enabled")) {
        return;
    }

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
    choiceBButton.textContent = data.choices.choiceA;
    descriptionParagraph.textContent = data.choices.description;

    document.querySelector('.choices').classList.remove('hidden');

};


choiceButtons.forEach(button => {
    button.addEventListener('click', updateSelection);
});


if (document.body.contains(submit)) {
    submit.addEventListener('click', function () {

        hideChoices();

    });
}