// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your Javascript code.


function filterCards() {
    var input, filter, cards, cardContainer, h5, title, i, cabin;
    input = document.getElementById("myFilter");
    filter = input.value.toUpperCase();
    cardContainer = document.getElementById("cardList");
    cards = cardContainer.getElementsByClassName("card");
    for (i = 0; i < cards.length; i++) {
        title = cards[i].querySelector(".card-body a");
        cabin = cards[i].querySelector(".card-body p");
        if ((title.innerText.toUpperCase().indexOf(filter) > -1) ||
            (cabin.innerText.toUpperCase().indexOf(filter) > -1)
            ){
            cards[i].style.display = "";
        } else {
            cards[i].style.display = "none";
        }
    }
}