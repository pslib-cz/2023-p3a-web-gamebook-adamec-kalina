
const codeList = document.getElementById('codeList');
const resultDisplay = document.getElementById('result');
const currentLevel = 3;
let currentSnippetIndex = 0;

// Snippets for different levels
const levelSnippetsOriginal = {
	1: [
		[
			"function greet() {",
			"\tconsole.log('Hello World');",
			"}"],
		[
			"for (let i = 0; i < 3; i++) {",
			"\tconsole.log(i);",
			"}"],
		[
			"let sum = 0;",
			"sum += 5;",
			"console.log(sum);"]
	],
	2: [
		[
			"let numbers = [1, 2, 3];",
			"let squares = numbers.map(n => n * n);",
			"console.log(squares);"],
		[
			"let user = { name: 'Alex', age: 30 };",
			"for (let key in user) {",
			"\tconsole.log(key + ': ' + user[key]);",
			"}"]
	],
	3: [
		[
			"document.addEventListener('DOMContentLoaded', () => {",
			"\tconst button = document.getElementById('myButton');",
			"\tbutton.addEventListener('click', () => {",
			"\t\tconst contentDiv = document.getElementById('content');",
			"\t\tcontentDiv.innerHTML = '<p>New content loaded!</p>';",
			"\t});",
			"});"],

		[
			"function unlockSystem() {",
			"\tlet password = decrypt('encryptedString');",
			"\tif (password === 'correctPassword') {",
			"\t\tconsole.log('Access Granted');",
			"\t}",
			"\telse {",
			"\t\tconsole.log('Access Denied');",
			"\t}",
			"}"],
		[
			"function factorial(n) {",
			"\tif (n === 0 || n === 1) {",
			"\t\treturn 1;",
			"\t} else {",
			"\t\treturn n * factorial(n - 1);",
			"\t}",
			"}",
			"console.log('Factorial of 5:', factorial(5));"

		]
	]
	// Add more levels and snippets as needed
};

const levelSnippets = JSON.parse(JSON.stringify(levelSnippetsOriginal));

function loadSnippet(level, index) {
	codeList.innerHTML = '';
	let snippets = levelSnippets[level][index];
	shuffleArray(snippets);
	snippets.forEach(snippet => {
		let li = document.createElement('li');
		li.textContent = snippet;
		li.draggable = true;

		li.addEventListener('dragstart', handleDragStart);
		li.addEventListener('dragover', handleDragOver);
		li.addEventListener('drop', handleDrop);
		li.addEventListener('dragend', handleDragEnd);
		codeList.appendChild(li);
	});
}

let draggedItem = null;

function handleDragStart(e) {
	draggedItem = this;
	e.dataTransfer.effectAllowed = 'move';
	e.dataTransfer.setData('text/plain', this.textContent);
}

function handleDragOver(e) {
	e.preventDefault();
	e.dataTransfer.dropEffect = 'move';
}

function handleDrop(e) {
	e.stopPropagation();
	if (draggedItem !== this) {
		let temp = draggedItem.textContent;
		draggedItem.textContent = this.textContent;
		this.textContent = temp;
	}
	return false;
}

function handleDragEnd(e) {
	draggedItem = null;
}

function checkCodeOrder(currentOrder, correctOrder) {
	console.log(JSON.stringify(currentOrder));
	console.log(JSON.stringify(correctOrder));
	return JSON.stringify(currentOrder) === JSON.stringify(correctOrder);
}

var submitBtn = document.getElementById('submitBtn');
if (document.body.contains(submitBtn)) {
	submitBtn.addEventListener('click', function () {
		let currentOrder = Array.from(codeList.children).map(li => li.textContent);
		if (checkCodeOrder(currentOrder, levelSnippetsOriginal[currentLevel][currentSnippetIndex])) {
			resultDisplay.textContent = 'Access Granted!';
			currentSnippetIndex++;
			if (currentSnippetIndex >= levelSnippets[currentLevel].length) {
				resultDisplay.textContent = 'All levels complete!';
				SetHitboxNotAvailable();
				HideHack();

			}
			loadSnippet(currentLevel, currentSnippetIndex);
		} else {
			resultDisplay.textContent = 'Access Denied. Try again.';
		}
	});
}

function shuffleArray(array) {
	for (let i = array.length - 1; i > 0; i--) {
		let j = Math.floor(Math.random() * (i + 1));
		[array[i], array[j]] = [array[j], array[i]];
	}
}



function ShowHack() {
	loadSnippet(currentLevel, currentSnippetIndex);

	document.querySelector('.hack-game').classList.remove("hidden");

	document.getElementById('location-choice').classList.add("hidden");
	document.querySelector('.menu__swich').classList.add("hidden");
	document.getElementById('hitbox').classList.add("hidden");


}

function HideHack() {

	document.querySelector('.hack-game').classList.add("hidden");

	document.getElementById('location-choice').classList.remove("hidden");
	document.querySelector('.menu__swich').classList.remove("hidden");
	document.getElementById('hitbox').classList.remove("hidden");
}




function SetHitboxNotAvailable() {
	console.log("Hitbox over");
	fetch('/Gameplay/SetHitboxNotAvailable ', {
		method: 'POST',
		headers: {
			'Content-Type': 'application/json',
		}
	});
	setTimeout(function () {
		location.reload(true);
	}, 200);
}