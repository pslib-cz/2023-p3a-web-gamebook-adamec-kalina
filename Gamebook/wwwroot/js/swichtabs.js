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
        choiceButtons.forEach((button, index) => {
            button.classList.remove('selected');
            mapLocations[index].classList.remove('active');
        });

        var clickedButton = event.currentTarget;
        clickedButton.classList.add('selected');

        var buttonIndex = Array.from(choiceButtons).indexOf(clickedButton);

        if (mapLocations[buttonIndex]) {
            mapLocations[buttonIndex].classList.add('active');
        }

        var locationName = clickedButton.textContent.trim().replace(/\s+/g, '');
        var ctaLink = document.querySelector('.content__cta');
        ctaLink.href = 'Location/' + locationName;
        ctaLink.classList.add('enabled');
    }

    choiceButtons.forEach(button => {
        button.addEventListener('click', updateSelection);
    });
});