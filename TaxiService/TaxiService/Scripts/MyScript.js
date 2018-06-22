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

let formReset = function ResetAllForms() {
    $("input[type='text'], textarea").val('');
    $("input[type='password'], textarea").val('');
    $("input[type='email'], textarea").val('');
    $("input[type='number'], textarea").val('');
    $("input[type='date'], textarea").val('');
    $("select").each(function () { this.selectedIndex = 0; });
    $('#regName').removeClass().addClass("reg-table-td-input");
    $('#errorName').val(' ');
    $('#regSurname').removeClass().addClass("reg-table-td-input");
    $('#errorSurname').val(' ');
    $('#regEmail').removeClass().addClass("reg-table-td-input");
    $('#errorEmail').val(' ');
    $('#regPhone').removeClass().addClass("reg-table-td-input");
    $('#errorPhone').val(' ');
    $('#regJmbg').removeClass().addClass("reg-table-td-input");
    $('#errorJmbg').val(' ');
    $('#regUsername').removeClass().addClass("reg-table-td-input");
    $('#errorUsername').val(' ');
    $('#regCarId').removeClass().addClass("reg-table-td-input");
    $('#errorCarId').val(' ');
    $('#regModelYear').removeClass().addClass("reg-table-td-input");
    $('#errorModelYear').val(' ');
    $('#regNumber').removeClass().addClass("reg-table-td-input");
    $('#errorRegNum').val(' ');
    $('#regPass').removeClass().addClass("reg-table-td-input");
    $('#regPassRpt').removeClass().addClass("reg-table-td-input");
    $('#errorRegPass').val(' ');
    $('#loginUsername').css('borderColor', 'black');
    $('#loginPassword').css('borderColor', 'black');
    $('#errorLoginLbl').val(' ');
};

var updateDriverLocation = function (address, addressX, addressY) {
    var token = sessionStorage.getItem('accessToken');
    var user = JSON.parse(sessionStorage.getItem('activeUser'));

    let location = {
        drivedBy: user.id,
        address: address,
        addressX: addressX,
        addressY: addressY
    };

    $.ajax({
        type: 'PUT',
        url: '/api/Driver/UpdateDriverLocation',
        data: JSON.stringify(location),
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        headers: {
            'Authorization': 'Basic ' + token.toString()
        },
        success: function (data) {
            myDrives();
            $("#blurBackground").fadeOut('slow', 'swing');
            $("#displayDriverLocation").fadeOut('slow', 'swing');
        },
        error: function () {
            alert("Error while updating location, try again later!");
        }
    });
};

var freeDrivers = function GetFreeDrivers(carType) {
    let token = sessionStorage.getItem('accessToken');

    $.ajax({
        type: 'GET',
        url: '/api/Driver/GetFreeDrivers',
        data: {
            type: carType
        },
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
            alert("There is no free drivers, please try again later!");
        }
    });
};

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
            $('#btnLoginForm').hide();
            $('#profileButtons').show();
            let user = JSON.parse(sessionStorage.getItem('activeUser'));
            myDrives();
            if (user.role === 'Dispatcher') {
                $('#btnNewDrive').show();
                $('#btnDriveFilters').show();
                $('#btnDriverLocation').hide();
                $('#btnDriverAllDrives').hide();
                $('#btnDispatcherAllDrives').show();
                $('#btnAddDriver').show();
                $('#btnRegisterFormMenu').hide();
                $('#menu').css('height', '200');
                $('.admin-filters').show();
                $('.customer-filters').hide();
            } else if (user.role === 'Driver') {
                $('#btnNewDrive').hide();
                $('#btnDriveFilters').hide();
                $('#btnDriverLocation').show();
                $('#btnDriverAllDrives').show();
                $('#btnDispatcherAllDrives').hide();
                $('#btnAddDriver').hide();
                $('#btnRegisterFormMenu').hide();
                $('#menu').css('height', '152');
            } else {
                $('#btnNewDrive').show();
                $('#btnDriveFilters').show();
                $('#btnDriverLocation').hide();
                $('#btnDriverAllDrives').hide();
                $('#btnDispatcherAllDrives').hide();
                $('#btnAddDriver').hide();
                $('#btnRegisterFormMenu').hide();
                $('#menu').css('height', '152');
                $('.admin-filters').hide();
                $('.customer-filters').show();
            }
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
        home();
        formReset();
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
        $('#driveAddress').parent().parent().show();
        $('#driveAddressX').parent().parent().show();
        $('#driveAddressY').parent().parent().show();
        $('#driveCar').parent().parent().show();

        if (user.role === "Customer") {
            $('#dispatcherDrive').hide();
            $('#driverDrive').hide();
            $('#btnEditDrive').hide();
            $('#btnCreateDrive').show();
            $('#driveAddress').attr('readonly', false);
            $('#driveAddressX').attr('readonly', false);
            $('#driveAddressY').attr('readonly', false);
            $('#driveCar').attr('readonly', false);
        }
        else {
            $('#driveAddress').attr('readonly', false);
            $('#driveAddressX').attr('readonly', false);
            $('#driveAddressY').attr('readonly', false);
            $('#driveCar').attr('readonly', false);
            $('#dispatcherDrive').show();
            $('#driverDrive').hide();
            $('#btnCreateDrive').show();
            $('#btnEditDrive').hide();
            freeDrivers("Bez_Naznake");
        }
        $("#blurBackground").fadeIn('slow', 'swing');
        $("#displayNewDrive").fadeIn('slow', 'swing');
    });

    $('#btnExitNewDrive').click(function () {
        $("#blurBackground").fadeOut('slow', 'swing');
        $("#displayNewDrive").fadeOut('slow', 'swing');
        home();
        formReset();
        $('#displayTrips').fadeIn('slow', 'swing');
    });

    $('#btnExitFinishedDrive').click(function () {
        $("#blurBackground").fadeOut('slow', 'swing');
        $("#displayDriverFinished").fadeOut('slow', 'swing');
        home();
        formReset();
        $('#displayTrips').fadeIn('slow', 'swing');
    });

    $('#btnExitComment').click(function () {
        $("#blurBackground").fadeOut('slow', 'swing');
        $("#displayCommentForm").fadeOut('slow', 'swing');
        home();
        formReset();
        $('#displayTrips').fadeIn('slow', 'swing');
    });

    $('#btnExitLocationChange').click(function () {
        $("#blurBackground").fadeOut('slow', 'swing');
        $("#displayDriverLocation").fadeOut('slow', 'swing');
        home();
        formReset();
        $('#displayTrips').fadeIn('slow', 'swing');
    });

    let logout = function LogOut() {
        if (sessionStorage.getItem('accessToken')) {
            $('#profileButtons').hide();
            $('#btnLoginForm').show();
            $('#btnRegisterFormMenu').show();
            $('#btnAddDriver').hide();
            $('#menu').css('height', '200');
            $('#fillInTrips').empty();
            $('#displayTrips').fadeOut('slow', 'swing');
            sessionStorage.removeItem('accessToken');
            sessionStorage.removeItem('activeUser');
            $('#filtersTable').hide();
            home();
            formReset();
        }
    };

    $('#btnLogout').click(logout);
    $('#btnLogoutFooter').click(logout);

    $('#btnDriverLocation').click(function () {
        $("#blurBackground").fadeIn('slow', 'swing');
        $("#displayDriverLocation").fadeIn('slow', 'swing');
    });
    
    $('#btnChangeDLocation').click(function () {
        updateDriverLocation($('#driverLocation').val(), $('#driverLocationX').val(), $('#driverLocationY').val());
        $("#blurBackground").fadeOut('slow', 'swing');
        $("#displayDriverLocation").fadeOut('slow', 'swing');
    });                       
});                           