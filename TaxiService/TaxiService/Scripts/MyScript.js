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
        $('#otherInfo' + elem.id).slideDown('slow');
        document.getElementById(elem.id).innerHTML = "-";
    } else {
        $('#otherInfo' + elem.id).slideUp('slow');
        document.getElementById(elem.id).innerHTML = "+";
    }
}

$(document).ready(function () {
    //$('#btnMenu').click(function () {
    //    $('#menu').slideToggle(300);
    //});

    let displayRegister = function DisplayRegisterForm() {
        $('#btnAddNewDriver').hide();
        $('#btnUpdateAccount').hide();
        $('#displayRegisterHeader').text('REGISTER FORM');
        $('#addDriverFormCarId').hide();
        $('#addDriverFormCarIdError').hide();
        $('#addDriverFormModel').hide();
        $('#addDriverFormModelError').hide();
        $('#addDriverFormRegNum').hide();
        $('#addDriverFormRegNumError').hide();
        $('#addDriverFormCarType').hide();
        $('#addDriverFormCarTypeError').hide();
        $('#regUsername').attr('readonly', false);
        $('#btnRegister').show();
        $('#displayLoginForm').fadeOut('slow', 'swing');
        $("#blurBackground").fadeOut('slow', 'swing');
        $('#displayTrips').fadeOut('slow', 'swing');
        $('#displayNewRide').fadeOut('slow', 'swing');
        $('#displayBanner').fadeOut('slow', 'swing');
        $('#displayHeader').fadeIn('slow', 'swing');
        $('#displayRegister').fadeIn('slow', 'swing');
        $('#displayFooter').fadeIn('slow', 'swing');
    };

    $('#btnRegisterForm').click(displayRegister);
    $('#btnRegisterFooter').click(displayRegister);
    $('#btnRegisterFormMenu').click(displayRegister);

    let login = function ShowLogin() {
        $('#blurBackground').show();
        $('#displayLoginForm').fadeIn("slow",'swing');
    };

    $('#btnLoginForm').click(login);
    $("#btnLoginMenu").click(login);


    let home = function DisplayHome() {
        $('#displayLoginForm').fadeOut('slow', 'swing');
        $("#blurBackground").fadeOut('slow', 'swing');
        if (sessionStorage.getItem('accessToken')) {
            $('#displayTrips').fadeIn('slow', 'swing');
        } else {
            $('#displayTrips').fadeOut('slow', 'swing');
        }
        $('#displayRegister').fadeOut('slow', 'swing');
        $('#displayNewRide').fadeOut('slow', 'swing');
        $('#displayHeader').fadeIn('slow', 'swing');
        $('#displayBanner').fadeIn('slow', 'swing');
        $('#displayFooter').fadeIn('slow', 'swing');
    };

    $('#btnHomeMenu').click(home);
    $('#logoClickHome').click(home);
    $('#logoClickHome2').click(home);
    $('#btnHomeFooter').click(home);

    $('#btnExitLogin').click(function () {
        $('#displayLoginForm').hide();
        $("#blurBackground").hide();
        home;
    });

    $('main').hover(function () {
        $('#menu').hide();
    });

    $('#btnDriveFilters').click(function () {
        $('#filtersTable').slideToggle(800);
    });

    $('#btnNewDrive').click(function () {
        let user = JSON.parse(sessionStorage.getItem('activeUser'));
        let token = sessionStorage.getItem('accessToken');

        if (user.role === "Customer") {
            $('#dispatcherDrive').hide();
        }
        else {
            $('#dispatcherDrive').show();
            $.ajax({
                type: 'GET',
                url: '/api/Driver/GetFreeDrivers',
                dataType: 'json',
                headers: {
                    'Authorization': 'Basic ' + token.toString()
                },
                success: function (data) {
                    alert(JSON.stringify(data));
                    driver = JSON.stringify(data);
                    $('#freeDriver').empty();
                    for (let i = 0; i < data.length; i++) {
                        let fullName = JSON.stringify(data[i].name) + ' ' + JSON.stringify(data[i].surname);
                        $('#freeDriver').append('<option value="' + JSON.stringify(data[i].id).replace(/"|_/g, '') + '">' + fullName.replace(/"|_/g, '') + '</option>');
                    }
                },
                error: function () {
                    alert("Greska na serveru, molimo vas pokusajte kasnije!");
                }
            });
        }
        $("#blurBackground").fadeIn('slow', 'swing');
        $("#displayNewDrive").fadeIn('slow', 'swing');
    });

    $('#btnExitNewDrive').click(function () {
        $("#blurBackground").fadeOut('slow', 'swing');
        $("#displayNewDrive").fadeOut('slow', 'swing');
        home;
        $('#displayTrips').fadeIn('slow', 'swing');
    });

    let logout = function LogOut() {
        if (sessionStorage.getItem('accessToken')) {
            $('#profileButtons').hide();
            $('#btnLoginForm').show();
            $('#btnAddDriver').hide();
            $('#menu').css('height', '200');
            $('#displayTrips').fadeOut('slow', 'swing');
            sessionStorage.removeItem('accessToken');
            sessionStorage.removeItem('activeUser');
            home;
        }
    };

    $('#btnLogout').click(logout);
    $('#btnLogoutFooter').click(logout);
});