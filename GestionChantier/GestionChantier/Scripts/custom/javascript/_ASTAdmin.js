//A louverture loader si il y a des dates dinactivite de projets
function AjaxObtenirDateInactivite() {
    if ($("#SelectProjet").val() == "" || $("#SelectProjet").val() == undefined || $("#datedebutinactivite") == undefined)
        return;

    $.ajax({
        url: '/' + $(".photographerIdentifier").attr('id') + '/ObtenirDateInactiviteProjet/',
        type: "POST",
        dataType: "html",
        data: {
            idProjet: $("#SelectProjet").val()
        }
    }).done(function (data) {
        var jsonArray = JSON.parse(data);
        $("#datedebutinactivite").val(jsonArray["dateDebutInactivite"]);
        $("#datefininactivite").val(jsonArray["dateFinInactivite"]);
    }).fail(function (data) {
        alert("Récupération des dates d'inactivité échouée.")
    }).always(function () {
    });
}

function AjaxUpdateDateInactivite() {
    if ($("#SelectProjet").val() == "" || $("#SelectProjet").val() == undefined || $("#datedebutinactivite") == undefined)
        return;

    $.ajax({
        url: '/' + $(".photographerIdentifier").attr('id') + '/UpdateDateInactiviteProjet/',
        type: "POST",
        dataType: "html",
        data: {
            idProjet: $("#SelectProjet").val(), dateDebutInactivite: $("#datedebutinactivite").val(), dateFinInactivite: $("#datefininactivite").val()
        }
    }).done(function (data) {
    }).fail(function (data) {
        alert("Sauvegarde des dates d'inactivité échouée.")
    }).always(function () {
    });
}

function AjaxIsPhotoCompletedByUserToday() {
    if ($("#SelectProjet").val() == "" || $("#SelectProjet").val() == undefined || $("#datedebutinactivite") == undefined)
        return;

    $.ajax({
        url: '/' + $(".photographerIdentifier").attr('id') + '/IsPhotoTakenByUserToday/',
        type: "POST",
        dataType: "html",
        data: {
            idProjet: $("#SelectProjet").val()
        }
    }).done(function (data) {
        var jsonArray = JSON.parse(data);
        if (jsonArray["isPhotoTaken"] && $("#ASTCompleted").hasClass("d-none"))
            $("#ASTCompleted").removeClass("d-none");
        else if (!jsonArray["isPhotoTaken"] && !$("#ASTCompleted").hasClass("d-none"))
            $("#ASTCompleted").addClass("d-none");
    }).fail(function (data) {
        alert("Erreur lors de la récupération du nombre d'AST complété par l'utilisateur aujourd'hui.")
    }).always(function () {
    });
}


function AjaxObtenirNombrePhotoProjetActifAujourdhui() {
    if ($("#SelectProjet").val() == "" || $("#SelectProjet").val() == undefined || $("#datedebutinactivite") == undefined)
        return;

    $.ajax({
        url: '/' + $(".photographerIdentifier").attr('id') + '/GetNbPhotoForToday/',
        type: "POST",
        dataType: "html",
        data: {
            idProjet: $("#SelectProjet").val()
        }
    }).done(function (data) {
        var jsonArray = JSON.parse(data);
        $("#lblASTCount").text(jsonArray["nbPhotoFound"]);
    }).fail(function (data) {
        alert("Erreur lors de la récupération du nombre d'AST complété aujourd'hui pour ce projet.")
    }).always(function () {
    });
}

//Au changement de projet dans le select loader si il y a des dates dinactivites
$(document).on("change", "#SelectProjet", function () {
    AjaxObtenirDateInactivite();
    AjaxObtenirNombrePhotoProjetActifAujourdhui();
    AjaxIsPhotoCompletedByUserToday();
});


//Au changement de date dinactivite updater les dates
$(document).on("change", ".dateInactivite", function () {
    if ($("#datefininactivite").val() != "" && $("#datedebutinactivite").val() != "")
        if (new Date($("#datefininactivite").val()) < new Date($("#datedebutinactivite").val())) {
            alert("La date de fin d'inactivite ne peut pas être plus petite que la date de début d'inactivité.")
            return;
        }

    AjaxUpdateDateInactivite();
});


//Si lutilisateur a un ast pour ce projet mettre le nom de projet en vert


//Afficher avec une puce la quantite dast pour ce projet

$(document).on('click', "#btnScreenshot", function () {
    AjaxObtenirDateInactivite();
    AjaxObtenirNombrePhotoProjetActifAujourdhui();
    AjaxIsPhotoCompletedByUserToday();
});