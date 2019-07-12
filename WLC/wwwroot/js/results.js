﻿

$(document).ready(function () {
    $("#HideSelections").click(function () {
        $("#Selections").addClass('d-none');
        $("#HiddenSelections").removeClass('d-none');

    });

    $("#ShowSelections").click(function () {
        $("#Selections").removeClass('d-none');
        $("#HiddenSelections").addClass('d-none');

    });



});



function AddRacerToRace(racerId, raceId) {


    $.ajax({
        type: "GET",
        url: "/Races/Results/?handler=EnterRacer&racerId="+ racerId + "&raceId=" + raceId,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            if (response.error === false) {
                loadEntrants(raceId);
                loadQualified(raceId);
            }
            else {
                toastr.error(response.message);
            }

        },
        failure: function (response) {
            toastr.error(response);
        }
    });

    this.event.stopPropagation();

    return false;
  
}

function AddTeamToRace(raceId) {

    var racerIds = $("#gridQualified input:checkbox:checked").map(function () {
        return $(this).val();
    }).get();

    var addTeamRequest = {
        raceId: raceId,
        racerIds: racerIds
    };

    $.ajax({
        type: "POST",
        url: "/Races/Results/?handler=EnterTeam",
        data: JSON.stringify(addTeamRequest),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            if (response.error === false) {
                loadEntrants(raceId);
                loadQualified(raceId);
            }
            else {
                toastr.error(response.message);
            }

        },
        failure: function (response) {
            alert(response);
        }
    });

    this.event.stopPropagation();

    return false;

}

function RemoveRacerFromRace(teamId, raceId) {

    $.ajax({
        type: "GET",
        url: "/Races/Results/?handler=RemoveRacer&teamId=" + teamId+ "&raceId=" + raceId,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            if (response.error === false) {
                loadEntrants(raceId);
                loadQualified(raceId);
            }
            else {
                toastr.error(response.message);
            }

        },
        failure: function (response) {
            alert(response);
        }
    });

    this.event.stopPropagation();

    return false;

}


function SetRacerPosition(teamId, raceId, place) {

    $.ajax({
        type: "GET",
        url: "/Races/Results/?handler=SetRacerPosition&teamId=" + teamId + "&raceId=" + raceId + "&place=" + place,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            if (response.error === false) {
                loadEntrants(raceId);
            }
            else {
                toastr.error(response.message);
            }

        },
        failure: function (response) {
            alert(response);
        }
    });

    this.event.stopPropagation();

    return true;

}

function loadEntrants(raceId) {
    $('#gridEntrant').load('/Races/Results?handler=EntrantsPartial&raceId=' + raceId);

}

function loadQualified(raceId) {
    $('#gridQualified').load('/Races/Results?handler=QualifiedPartial&raceId=' + raceId);

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
