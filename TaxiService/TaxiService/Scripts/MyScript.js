function generateMenu() {
    if (document.getElementById("menu").style.display == "none") {
        document.getElementById("menu").style.display = "block";
    } else {
        document.getElementById("menu").style.display = "none";
    }
}

function formatTime() {
    var d = new Date(),
        minutes = d.getMinutes().toString().length == 1 ? '0' + d.getMinutes() : d.getMinutes(),
        hours = d.getHours().toString().length == 1 ? '0' + d.getHours() : d.getHours(),
        seconds = d.getSeconds().toString().length == 1 ? '0' + d.getSeconds() : d.getSeconds(),
        months = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'],
        days = ['Sun', 'Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat'];
    document.getElementById("clock").innerHTML = days[d.getDay()] + ' ' +  d.getDate() + ' ' + months[d.getMonth()] + ' ' + d.getFullYear() + ' ' + hours + ':' + minutes + ':' + seconds;
    setTimeout(formatTime, 1000);
}

function showOtherInfo(elem) {
    if (document.getElementById("otherInfo" + elem.id).style.display == "none") {
        document.getElementById("otherInfo" + elem.id).style.display = "block";
        document.getElementById(elem.id).innerHTML = "-";
    } else {                     
        document.getElementById("otherInfo" + elem.id).style.display = "none";
        document.getElementById(elem.id).innerHTML = "+";
    }
}

$(document).ready(function () {
    //$('#btnMenu').click(function () {
    //    $('#menu').slideToggle(300);
    //});

    $("#btnLogin").click(function () {
        $("#blurBackground").show();
        $("#displayLoginForm").fadeIn("slow");
    });

    $('#btnExitLogin').click(function () {
        $('#displayLoginForm').hide();
        $("#blurBackground").hide();
    });

    $('#btnRegisterForm').click(function () {
        $('#btnAddNewDriver').hide();
        $('#addDriverFormCarId').hide();
        $('#addDriverFormCarIdError').hide();
        $('#addDriverFormModel').hide();
        $('#addDriverFormModelError').hide();
        $('#addDriverFormRegNum').hide();
        $('#addDriverFormRegNumError').hide();
        $('#addDriverFormCarType').hide();
        $('#addDriverFormCarTypeError').hide();
        $('#btnRegister').show();
        $('#displayLoginForm').fadeOut('slow', 'swing');
        $("#blurBackground").fadeOut('slow', 'swing');
        $('#displayTrips').fadeOut('slow', 'swing');
        $('#displayBanner').fadeOut('slow', 'swing');
        $('#displayRegister').fadeIn('slow', 'swing');
    });

    $('#btnRegisterFormMenu').click(function () {
        $('#btnAddNewDriver').hide();
        $('#addDriverFormCarId').hide();
        $('#addDriverFormCarIdError').hide();
        $('#addDriverFormModel').hide();
        $('#addDriverFormModelError').hide();
        $('#addDriverFormRegNum').hide();
        $('#addDriverFormRegNumError').hide();
        $('#addDriverFormCarType').hide();
        $('#addDriverFormCarTypeError').hide();
        $('#btnRegister').show();
        $('#displayLoginForm').fadeOut('slow', 'swing');
        $("#blurBackground").fadeOut('slow', 'swing');
        $('#displayTrips').fadeOut('slow', 'swing');
        $('#displayBanner').fadeOut('slow', 'swing');
        $('#displayRegister').fadeIn('slow', 'swing');
    });

    $('main').hover(function () {
        $('#menu').hide();
    });

    $('#btnDriveFilters').click(function () {
        $('#filtersTable').slideToggle(800);
    });

    $('#btnNewDrive').click(function () {
        $("#blurBackground").fadeIn('slow', 'swing');
        $("#displayNewDrive").fadeIn('slow', 'swing');
    });

    $('#btnExitNewDrive').click(function () {
        $("#blurBackground").fadeOut('slow', 'swing');
        $("#displayNewDrive").fadeOut('slow', 'swing');
    });
});