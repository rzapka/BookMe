$(document).ready(function () {
    function highlightMatch(text, term) {
        var regex = new RegExp('(' + term + ')', 'gi');
        return text.replace(regex, "<strong>$1</strong>");
    }

    function updateSuggestions(inputElement, suggestionsElement, data, term) {
        suggestionsElement.empty();
        if (data.length) {
            data.forEach(item => {
                var highlighted = highlightMatch(item, term);
                suggestionsElement.append(`<div class="suggestion-item">${highlighted}</div>`);
            });
            suggestionsElement.show();
        } else {
            suggestionsElement.hide();
        }
        suggestionsElement.find('.suggestion-item').on('click', function () {
            console.log('Suggestion clicked');
            inputElement.val($(this).text());
            suggestionsElement.hide();
        });
    }

    $("#serviceSearch").on("input", function () {
        var term = $(this).val();
        var suggestionsElement = $("#serviceSuggestions");
        axios.get('/Search/SearchOffers', { params: { term: term } })
            .then(response => {
                var offers = response.data.map(offer => offer);
                updateSuggestions($(this), suggestionsElement, offers, term);
            })
            .catch(error => console.error(error));
    });

    $("#citySearch").on("input", function () {
        var term = $(this).val();
        var suggestionsElement = $("#citySuggestions");
        axios.get('/Search/SearchCities', { params: { term: term } })
            .then(response => {
                var cities = response.data;
                updateSuggestions($(this), suggestionsElement, cities, term);
            })
            .catch(error => console.error(error));
    });

    $(document).on('click', function (e) {
        if (!$(e.target).closest('.position-relative').length) {
            $(".suggestions").hide();
        }
    });
});
