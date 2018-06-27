var filtersOn = false;
var allDrivesOn = false;

function generateMenu() {
    if (document.getElementById("menu").style.display == "none") {
        document.getElementById("menu").style.display = "block";
    } else {
        document.getElementById("menu").style.display = "none";
    }
}

function GetDrivesPeriod() {
    if (sessionStorage.getItem('accessToken')) {
        myDrives();

        setTimeout(GetDrivesPeriod, 30000);
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

var freeDriversLocation = function GetFreeDriversBylocation(carType, addressX, addressY) {
    let token = sessionStorage.getItem('accessToken');

    $.ajax({
        type: 'GET',
        url: '/api/Driver/GetFreeDriversByLen',
        data: {
            type: carType,
            x: addressX,
            y: addressY
        },
        dataType: 'json',
        headers: {
            'Authorization': 'Basic ' + token.toString()
        },
        success: function (data) {
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

function UnBanDriver(elem) {
    let token = sessionStorage.getItem('accessToken');
    let userId = $('#' + elem.id).val();
    banUser = userId;
    let banned = false;
    bann = {
        id: banUser,
        ban: banned
    };

    $.ajax({
        type: 'PUT',
        url: '/api/Driver/BanDriver',
        data: JSON.stringify(bann),
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        headers: {
            'Authorization': 'Basic ' + token.toString()
        },
        success: function (data) {
            $('#' + elem.id).attr("onclick", "BanDriver(this)");
            $('#' + elem.id).text("Ban Driver");
        },
        error: function () {
            alert("Error while banning user, try again later!");
        }
    });
}

function BanDriver(elem) {
    let token = sessionStorage.getItem('accessToken');
    let userId = $('#' + elem.id).val();
    banUser = userId;
    let banned = true;
    bann = {
        id: banUser,
        ban: banned
    };

    $.ajax({
        type: 'PUT',
        url: '/api/Driver/BanDriver',
        data: JSON.stringify(bann),
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        headers: {
            'Authorization': 'Basic ' + token.toString()
        },
        success: function (data) {
            $('#' + elem.id).attr("onclick", "UnBanDriver(this)");
            $('#' + elem.id).text("Unban Driver");
        },
        error: function () {
            alert("Error while banning user, try again later!");
        }
    });
}

function UnBanCustomer(elem) {
    let token = sessionStorage.getItem('accessToken');
    let userId = $('#' + elem.id).val();
    banUser = userId;
    let banned = false;
    bann = {
        id: banUser,
        ban: banned
    };

    $.ajax({
        type: 'PUT',
        url: '/api/Customer/BanCustomer',
        data: JSON.stringify(bann),
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        headers: {
            'Authorization': 'Basic ' + token.toString()
        },
        success: function (data) {
            $('#' + elem.id).attr("onclick", "BanCustomer(this)");
            $('#' + elem.id).text("Ban Customer");
        },
        error: function () {
            alert("Error while banning user, try again later!");
        }
    });
}

function BanCustomer(elem) {
    let token = sessionStorage.getItem('accessToken');
    let userId = $('#' + elem.id).val();
    banUser = userId;
    let banned = true;
    bann = {
        id: banUser,
        ban: banned
    };

    $.ajax({
        type: 'PUT',
        url: '/api/Customer/BanCustomer',
        data: JSON.stringify(bann),
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        headers: {
            'Authorization': 'Basic ' + token.toString()
        },
        success: function (data) {
            $('#' + elem.id).attr("onclick", "UnBanCustomer(this)");
            $('#' + elem.id).text("Unban Customer");
        },
        error: function () {
            alert("Error while banning user, try again later!");
        }
    });
}

$(document).ready(function () {
    //$('#btnMenu').click(function () {
    //    $('#menu').slideToggle(300);
    //});

    GetDrivesPeriod();

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
            $('body').css('backgroundImage', 'url(Images/tax3i.jpg)');
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
                $('#btnBanUsers').show();
                $('#btnRegisterFormMenu').hide();
                $('#menu').css('height', '248');
                $('.admin-filters').show();
                $('.common-filters').show();
                $('.driver-filters').hide();
                $('.customer-filters').show();
                $('#displayBanner').fadeOut('slow', 'swing');
            } else if (user.role === 'Driver') {
                $('#btnNewDrive').hide();
                $('#btnDriveFilters').show();
                $('.admin-filters').hide();
                $('.common-filters').show();
                $('.driver-filters').show();
                $('.customer-filters').show();
                $('#btnDriverLocation').show();
                $('#btnDriverAllDrives').show();
                $('#btnDispatcherAllDrives').hide();
                $('#btnAddDriver').hide();
                $('#btnBanUsers').hide();
                $('#btnRegisterFormMenu').hide();
                $('#menu').css('height', '152');
                $('#displayBanner').fadeOut('slow', 'swing');
            } else {
                $('#btnNewDrive').show();
                $('#btnDriveFilters').show();
                $('#btnDriverLocation').hide();
                $('#btnDriverAllDrives').hide();
                $('#btnDispatcherAllDrives').hide();
                $('#btnAddDriver').hide();
                $('#btnBanUsers').hide();
                $('#btnRegisterFormMenu').hide();
                $('#menu').css('height', '152');
                $('.admin-filters').hide();
                $('.customer-filters').show();
                $('.common-filters').hide();
                $('.driver-filters').hide();
                $('#displayBanner').fadeOut('slow', 'swing');
            }
        } else {
            $('#displayTrips').fadeOut('slow', 'swing');
            $('#displayBanner').fadeIn('slow', 'swing');
        }
        $('#displayRegister').fadeOut('slow', 'swing');
        $('#displayUsers').fadeOut('slow', 'swing');
        $('#displayNewRide').fadeOut('slow', 'swing');
        $('#displayHeader').fadeIn('slow', 'swing');
        $('#displayFooter').fadeIn('slow', 'swing');
    };

    $('#btnHomeMenu').click(function () {
        allDrivesOn = false;
        filtersOn = false;
        home();
    });
    $('#logoClickHome').click(function () {
        allDrivesOn = false;
        filtersOn = false;
        home();
    });
    $('#logoClickHome2').click(function () {
        allDrivesOn = false;
        filtersOn = false;
        home();
    });
    $('#btnHomeFooter').click(function () {
        allDrivesOn = false;
        filtersOn = false;
        home();
    });

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
        $('#map').parent().parent().show();

        if (user.role === "Customer") {
            $('#dispatcherDrive').hide();
            $('#driverDrive').hide();
            $('#btnEditDrive').hide();
            $('#btnCreateDrive').show();
            $('#driveAddress').attr('readonly', true);
            $('#driveAddressX').attr('readonly', true);
            $('#driveAddressY').attr('readonly', true);
            $('#driveCar').attr('readonly', false);
        }
        else {
            $('#driveAddress').attr('readonly', true);
            $('#driveAddressX').attr('readonly', true);
            $('#driveAddressY').attr('readonly', true);
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
        $('#btnNewDrive').prop("disabled", false);
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
            $('#displayUsers').hide();
            $('#usersTable').empty();
            $('#displayCurDrive').empty();
            $('#btnBanUsers').hide();
            $('#btnNewDrive').prop("disabled", false);
            $('#menu').css('height', '200');
            $('#fillInTrips').empty();
            $('body').css('backgroundImage', 'url(Images/allBlack.png)');
            $('#displayTrips').fadeOut('slow', 'swing');
            sessionStorage.removeItem('accessToken');
            sessionStorage.removeItem('activeUser');
            $('#filtersTable').hide();
            home();
            formReset();
            var inProgress = false;
            var driving = false;
            var filtersOn = false;
            var allDrivesOn = false;
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

    $('#btnBanUsers').click(function () {
        let token = sessionStorage.getItem("accessToken");

        $.ajax({
            type: 'GET',
            url: '/api/Dispatcher/GetAllUsers',
            dataType: 'json',
            headers: {
                'Authorization': 'Basic ' + token.toString()
            },
            success: function (data) {
                $('#usersTable').empty();
                let counter = 1;

                for (let i = 0; i < data.length; i++) {
                    let fullName = data[i].name + ' ' + data[i].surname;

                    let btn = '';
                    if (data[i].role === 'Driver') {
                        if (data[i].isBanned) {
                            btn = '<button id="btnBan' + counter + '" onclick="UnBanDriver(this);" value="' + data[i].id + '" class="btn-register">Unban Driver</button>';
                        } else {
                            btn = '<button id="btnBan' + counter + '" onclick="BanDriver(this);" value="' + data[i].id + '" class="btn-register">Ban Driver</button>';
                        }
                    } else if (data[i].role === 'Customer') {
                        if (data[i].isBanned) {
                            btn = '<button id="btnBan' + counter + '" onclick="UnBanCustomer(this);" value="' + data[i].id + '" class="btn-register">Unban Customer</button>';
                        } else {
                            btn = '<button id="btnBan' + counter + '" onclick="BanCustomer(this);" value="' + data[i].id + '" class="btn-register">Ban Customer</button>';
                        }
                    }

                    $('#usersTable').append('<tr class="reg-table-normal">' +
                        '<td class="ban-table-td">' + data[i].username + '</td>' +
                        '<td class="ban-table-td">' + fullName + '</td>' +
                        '<td class="ban-table-td">' + btn + '</td>' +
                        '</tr>');
                    counter++;
                }
                $('#displayTrips').fadeOut('slow', 'swing');
                $('#displayRegister').fadeOut('slow', 'swing');
                $('#displayNewRide').fadeOut('slow', 'swing');
                $('#displayBanner').fadeOut('slow', 'swing');
                $('#displayHeader').fadeIn('slow', 'swing');
                $('#displayUsers').fadeIn('slow', 'swing');
                $('#displayFooter').fadeIn('slow', 'swing');
            },
            error: function () {
                alert("There is no free drivers, please try again later!");
            }
        });

    });
});                           