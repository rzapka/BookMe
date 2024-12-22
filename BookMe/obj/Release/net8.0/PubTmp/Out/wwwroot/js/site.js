// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function clearInput(inputId) {
    const input = document.getElementById(inputId);
    input.value = '';
    input.focus();
    document.getElementById(inputId + 'Suggestions').style.display = 'none'; // Ukryj sugestie
}

// Ukrywanie wyników sugestii przy przejściu do drugiego inputa
document.getElementById('serviceSearch').addEventListener('focus', function () {
    document.getElementById('citySuggestions').style.display = 'none';
});

document.getElementById('citySearch').addEventListener('focus', function () {
    document.getElementById('serviceSuggestions').style.display = 'none';
});