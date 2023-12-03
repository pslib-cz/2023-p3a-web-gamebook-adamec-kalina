function toggleMenu() {
    var menu = document.querySelector('.menu');
    var overflow = document.querySelector('.menu__overflow');
    if (menu.classList.contains('menu--closed')) {
        menu.classList.remove('menu--closed');
        overflow.style.display = 'block';
    } else {
        menu.classList.add('menu--closed');
        overflow.style.display = 'none';
    }
}
function hideMenu() {
    toggleMenu();
    var overflow = document.querySelector('.menu__overflow');
    overflow.style.display = 'none';
}