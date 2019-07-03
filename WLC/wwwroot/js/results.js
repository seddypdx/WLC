
$(function () {
    $('.EnterRacer').on('click', function () {

      //  $('#gridEntrant').load('<%=Url.Action("EnterRacer", "YourController")%>', new { catID =  tree.CategoryCode, langID= 1 }, null);
        $('#gridEntrant').load(this.href);
        return false();
    });
});



$(function () {
    $('#load').on('click', function () {
        loadEntrants();
        loadQualified();
    });
});





function loadEntrants() {
    $('#gridEntrant').load('/Races/Result?handler=EntrantsPartial&raceId=@Model.ActiveRaceId');

}

function loadQualified() {
    $('#gridQualified').load('/Races/Result?handler=QualifiedPartial&raceId=@Model.ActiveRaceId');

}


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
        ) {
            cards[i].style.display = "";
        } else {
            cards[i].style.display = "none";
        }
    }
}
