

$(document).ready(function () {
    $("#HideSelections").click(function () {
        $("#Selections").addClass('d-none');
        $("#HiddenSelections").removeClass('d-none');

    });

    $("#ShowSelections").click(function () {
        $("#Selections").removeClass('d-none');
        $("#HiddenSelections").addClass('d-none');

    });


    $('#RaceNext').click(function () {
        $('#RaceDropdown option:selected').next().attr('selected', 'selected');
        $(RaceForm).submit();

    });

    $('#RacePrevious').click(function () {
        $('#RaceDropdown option:selected').prev().attr('selected', 'selected');
        $(RaceForm).submit();

    });

   


});


function RefreshPage(raceId) {
    loadEntrants(raceId);
    loadQualified(raceId);


}


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

function GetRacer(racerId) {

    $.ajax({
        type: "GET",
        url: "/Races/Results/?handler=Racer&racerId=" + racerId,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            if (response.error === false) {
                setRacerInfo(response.racer);
                $('#AddRacerModal').modal();
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

function EditRacer(racerId) {

    GetRacer(racerId);
    this.event.stopPropagation();

    return false;

}

function PopupRacer() {

    clearRacerInfo();
    $('#AddRacerModal').modal();

}

function SaveRacerInfo(raceId) {

    var racer = {};
    racer.RacerId = $('[id*=RacerId]').val();
    racer.FirstName = $('[id*=FirstName]').val();
    racer.LastName = $('[id*=LastName]').val();
    racer.BoyOrGirl = $('[id*=BoyOrGirl]').val();
    racer.BirthDate =new Date($('[id*=Birthdate]').val());
    racer.Age = $('[id*=Age]').val();
    racer.MemberStatusId = parseInt($('[id*=MemberStatusId]').val());
    racer.CabinId = parseInt($('[id*=CabinId]').val());
   
    $.ajax({
        type: "POST",
        url: "/Races/Results/?handler=SaveRacer",
        data: JSON.stringify(racer),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            if (response.error === false) {
                loadEntrants(raceId);
                loadQualified(raceId);
                $('#AddRacerModal').modal('toggle');

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

function clearRacerInfo() {

    var date = new Date(2010, 7, 1);
    $('#NewRacer_RacerId').val('0');

    $('#NewRacer_FirstName').val('');
    $('#NewRacer_LastName').val('');
    $('#NewRacer_BoyOrGirl').val('b');
    $('#NewRacer_Birthdate').val('');
    $('#NewRacer_Age').val('');
    $('#NewRacer_CabinId option[value="60"').attr("selected", "selected");


}

function setRacerInfo(racer) {

    if (racer.birthdate !== null) {
        var date = parseJsonDate(racer.birthdate);
        $('#NewRacer_Birthdate').val(date.toISOString().slice(0, 10));
    }
    else {
        $('#NewRacer_Birthdate').val('');
    }
    $('#NewRacer_RacerId').val(racer.racerId);

    $('#NewRacer_FirstName').val(racer.firstName);
    $('#NewRacer_LastName').val(racer.lastName);
    $('#NewRacer_BoyOrGirl').val(racer.boyOrGirl);

    $('#NewRacer_MemberStatusId').val(racer.memberStatusId);
    $('#NewRacer_CabinId').val(racer.cabinId);
    $('#NewRacer_Age').val(racer.age);

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
    cardContainer = document.getElementById("cardListQualified");
    cards = cardContainer.getElementsByClassName("card");
    for (i = 0; i < cards.length; i++) {
        title = cards[i].querySelector(".card-title a");
        cabin = cards[i].querySelector(".cabinName");
        if ((title.innerText.toUpperCase().indexOf(filter) > -1) ||
            (cabin.innerText.toUpperCase().indexOf(filter) > -1)
        ) {
            cards[i].style.display = "";
        } else {
            cards[i].style.display = "none";
        }
    }
}

function parseJsonDate(jsonDate) {

    var year = parseInt(jsonDate.substring(0, 4));
    var day = parseInt(jsonDate.substring(8, 10));
    var month = parseInt(jsonDate.substring(5, 7));
    var date = new Date(year,month-1, day);
    return date;
}