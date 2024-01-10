var choiceButtons = document.querySelectorAll('.choices__choice');
var submit = document.querySelector('.choices__submit');

function updateSelection(event) {
    choiceButtons.forEach((button, index) => {
        button.classList.remove('selected');
    });

    var clickedButton = event.currentTarget;
    clickedButton.classList.add('selected');

    document.querySelector('.choices__submit').classList.add('enabled');
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

    document.querySelector('.choices').classList.remove('hidden');
};


choiceButtons.forEach(button => {
    button.addEventListener('click', updateSelection);
});


if (document.body.contains(submit)) {
    submit.addEventListener('click', hideChoices);
}