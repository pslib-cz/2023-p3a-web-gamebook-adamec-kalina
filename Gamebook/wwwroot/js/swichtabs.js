function switchTab(element, contentId) {
    var buttons = document.querySelectorAll('#menu .swich');
    var contents = document.querySelectorAll('.content');

    buttons.forEach(btn => btn.classList.remove('active'));
    contents.forEach(content => content.classList.add('hidden'));

    element.classList.add('active');
    document.getElementById(contentId).classList.remove('hidden');
}


window.onload = function () {
    var urlParams = new URLSearchParams(window.location.search);
    var section = urlParams.get('section');

    let element = section ? document.getElementById(`button-${section}`) : document.getElementById('button-map');

    if (element) {
        switchTab(element, section || 'map');
    };
};

document.addEventListener('DOMContentLoaded', function () {
    var choiceButtons = document.querySelectorAll('.content__choice');
    var mapLocations = document.querySelectorAll('.content__map__location');

    function updateSelection(event) {
        var clickedButton = event.currentTarget;
        if (!clickedButton.classList.contains('hidden')) {
            choiceButtons.forEach((button, index) => {
                button.classList.remove('selected');
                mapLocations[index].classList.remove('active');
            });

            clickedButton.classList.add('selected');
            console.log(clickedButton);

            var buttonIndex = Array.from(choiceButtons).indexOf(clickedButton);

            if (mapLocations[buttonIndex]) {
                mapLocations[buttonIndex].classList.add('active');
            }

            var locationName = clickedButton.textContent.trim().replace(/\s+/g, '');
            var ctaLink = document.querySelector('.content__cta');
            ctaLink.href = 'Location/' + locationName;
            ctaLink.classList.add('enabled');
        }
    }

    choiceButtons.forEach(button => {
        button.addEventListener('click', updateSelection);
    });


    var acc = document.getElementsByClassName("quest__card__heading");
    var i;

    for (i = 0; i < acc.length; i++) {
        acc[i].addEventListener("click", function () {
            var questCard = this.parentElement;
            questCard.classList.toggle("active");
            var panel = this.nextElementSibling;
            if (panel.style.maxHeight) {
                panel.style.maxHeight = null;
            } else {
                panel.style.maxHeight = panel.scrollHeight + "px";
            }
        });
    }
});