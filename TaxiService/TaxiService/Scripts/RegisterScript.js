$(document).ready(function () {
    var sendData = false;

    let showHome = function () {
        $('#displayBanner').fadeIn('slow', 'swing');
        $('#displayTrips').fadeIn('slow', 'swing');
    };

    $('#btnProfile').click(function () {
        let user = JSON.parse(sessionStorage.getItem('activeUser'));

        if (sessionStorage.getItem('accessToken')) {
            if (user.role === 'Driver') {
                $('#addDriverFormCarId').show();
                $('#displayRegisterHeader').text('MY PROFILE');
                $('#addDriverFormCarIdError').show();
                $('#addDriverFormModel').show();
                $('#addDriverFormModelError').show();
                $('#addDriverFormRegNum').show();
                $('#addDriverFormRegNumError').show();
                $('#addDriverFormCarType').show();
                $('#addDriverFormCarTypeError').show();
                $('#btnRegister').hide();
                $('#btnUpdateAccount').show();
                $('#btnAddNewDriver').hide();

                $('#regName').val(user.name);
                $('#regSurname').val(user.surname);
                $('#regEmail').val(user.email);
                $('#regPhone').val(user.phone);
                $('#regJmbg').val(user.jmbg);
                $('#regGender').val(user.gender.toString());
                $('#regUsername').val(user.username);
                $('#regUsername').attr('readonly', true);
                $('#regModelYear').val(user.car.modelYear);
                $('#regNumber').val(user.car.regNumber);
                $('#regCarId').val(user.car.carId);
                $('#newCarType').val(user.car.type.toString());
            }
            else if (user.role === 'Customer' || user.role === 'Dispatcher') {
                $('#btnAddNewDriver').hide();
                $('#btnRegister').hide();
                $('#btnUpdateAccount').show();
                $('#displayRegisterHeader').text('MY PROFILE');
                $('#addDriverFormCarId').hide();
                $('#addDriverFormCarIdError').hide();
                $('#addDriverFormModel').hide();
                $('#addDriverFormModelError').hide();
                $('#addDriverFormRegNum').hide();
                $('#addDriverFormRegNumError').hide();
                $('#addDriverFormCarType').hide();
                $('#addDriverFormCarTypeError').hide();

                $('#regName').val(user.name);
                $('#regSurname').val(user.surname);
                $('#regEmail').val(user.email);
                $('#regPhone').val(user.phone);
                $('#regJmbg').val(user.jmbg);
                $('#regGender').val(user.gender.toString());
                $('#regUsername').val(user.username);
                $('#regUsername').attr('readonly',true);
            }

            $('#displayLoginForm').fadeOut('slow', 'swing');
            $("#blurBackground").fadeOut('slow', 'swing');
            $('#displayTrips').fadeOut('slow', 'swing');
            $('#displayNewRide').fadeOut('slow', 'swing');
            $('#displayBanner').fadeOut('slow', 'swing');
            $('#displayHeader').fadeIn('slow', 'swing');
            $('#displayRegister').fadeIn('slow', 'swing');
            $('#displayFooter').fadeIn('slow', 'swing');
        }
    });

    $('#btnAddDriver').click(function () {
        let user = JSON.parse(sessionStorage.getItem('activeUser'));
        if (user.role === 'Dispatcher') {
            $('#addDriverFormCarId').show();
            $('#displayRegisterHeader').text('NEW DRIVER');
            $('#addDriverFormCarIdError').show();
            $('#addDriverFormModel').show();
            $('#addDriverFormModelError').show();
            $('#addDriverFormRegNum').show();
            $('#addDriverFormRegNumError').show();
            $('#addDriverFormCarType').show();
            $('#addDriverFormCarTypeError').show();
            $('#btnRegister').hide();
            $('#btnUpdateAccount').hide();
            $('#btnAddNewDriver').show();

            $('#displayLoginForm').fadeOut('slow', 'swing');
            $("#blurBackground").fadeOut('slow', 'swing');
            $('#displayTrips').fadeOut('slow', 'swing');
            $('#displayNewRide').fadeOut('slow', 'swing');
            $('#displayBanner').fadeOut('slow', 'swing');
            $('#displayHeader').fadeIn('slow', 'swing');
            $('#displayRegister').fadeIn('slow', 'swing');
            $('#displayFooter').fadeIn('slow', 'swing');
        }
    });

    $('#btnUpdateAccount').click(function () {
        CheckInput();
        if (sessionStorage.getItem('accessToken')) {
            let user = JSON.parse(sessionStorage.getItem('activeUser'));

            if (sendData) {
                if (user.role === 'Driver') {
                    let driver = {
                        id: user.id,
                        name: $('#regName').val(),
                        surname: $('#regSurname').val(),
                        email: $('#regEmail').val(),
                        phone: $('#regPhone').val(),
                        jmbg: $('#regJmbg').val(),
                        gender: $('#regGender').val(),
                        username: $('#regUsername').val(),
                        password: $('#regPass').val(),
                        car: {
                            modelYear: $('#regModelYear').val(),
                            regNumber: $('#regNumber').val(),
                            carId: $('#regCarId').val(),
                            type: $('#newCarType').val()
                        }
                    };

                    let token = sessionStorage.getItem('accessToken');

                    $.ajax({
                        type: 'PUT',
                        url: '/api/Driver/UpdateDriver',
                        data: JSON.stringify(driver),
                        contentType: 'application/json; charset=utf-8',
                        dataType: 'json',
                        headers: {
                            'Authorization': 'Basic ' + token.toString()
                        },
                        success: function (data) {
                            sessionStorage.setItem('activeUser', JSON.stringify(data));

                            $('#displayRegister').fadeOut('slow', 'swing', showHome);
                            formReset();
                        },
                        error: function () {
                            alert("Greska pri update-u!");
                        }
                    });
                } else  {
                    let userUpdate = {
                        id: user.id,
                        name: $('#regName').val(),
                        surname: $('#regSurname').val(),
                        email: $('#regEmail').val(),
                        phone: $('#regPhone').val(),
                        jmbg: $('#regJmbg').val(),
                        gender: $('#regGender').val(),
                        username: $('#regUsername').val(),
                        password: $('#regPass').val(),
                        role: user.role
                    };

                    if (user.role === 'Dispatcher') {
                        let token = sessionStorage.getItem('accessToken');
                        $.ajax({
                            type: 'PUT',
                            url: '/api/Dispatcher/UpdateDispatcher',
                            data: JSON.stringify(userUpdate),
                            contentType: 'application/json; charset=utf-8',
                            dataType: 'json',
                            headers: {
                                'Authorization': 'Basic ' + token.toString()
                            },
                            success: function (data) {
                                sessionStorage.setItem('activeUser', JSON.stringify(data));

                                $('#displayRegister').fadeOut('slow', 'swing', showHome);
                                formReset();
                            },
                            error: function () {
                                alert("Greska pri update-u!");
                            }
                        });
                    } else if (user.role === 'Customer') {
                        let token = sessionStorage.getItem('accessToken');
                        $.ajax({
                            type: 'PUT',
                            url: '/api/Customer/UpdateCustomer',
                            data: JSON.stringify(userUpdate),
                            contentType: 'application/json; charset=utf-8',
                            dataType: 'json',
                            headers: {
                                'Authorization': 'Basic ' + token.toString()
                            },
                            success: function (data) {
                                sessionStorage.setItem('activeUser', JSON.stringify(data));

                                $('#displayRegister').fadeOut('slow', 'swing', showHome);
                                formReset();
                            },
                            error: function () {
                                alert("Greska pri update-u!");
                            }
                        });
                    }
                }
            }
        }
    });

    $('#btnAddNewDriver').click(function () {
        CheckInput();
        if (sendData) {

            let driver = {
                name: $('#regName').val(),
                surname: $('#regSurname').val(),
                email: $('#regEmail').val(),
                phone: $('#regPhone').val(),
                jmbg: $('#regJmbg').val(),
                gender: $('#regGender').val(),
                username: $('#regUsername').val(),
                password: $('#regPass').val(),
                car: {
                    modelYear: $('#regModelYear').val(),
                    regNumber: $('#regNumber').val(),
                    carId: $('#regCarId').val(),
                    type: $('#newCarType').val()
                }
            };

            let token = sessionStorage.getItem('accessToken');

            $.ajax({
                type: 'POST',
                url: '/api/Dispatcher/AddDriver',
                data: JSON.stringify(driver),
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                headers: {
                    'Authorization': 'Basic ' + token.toString()
                },
                success: function (data) {
                    $('#displayRegister').fadeOut('slow', 'swing', showHome);
                    formReset();
                },
                error: function (jqXHR) {
                    alert("Greska pri registraciji!");
                }
            });
        }
    });

    $("#btnRegister").click(function () {
        CheckInput();
        if (sendData) {
            $.ajax({
                url: '/api/Register/RegisterAccount',
                method: 'POST',
                data: {
                    Name: $('#regName').val(),
                    Surname: $('#regSurname').val(),
                    Email: $('#regEmail').val(),
                    Phone: $('#regPhone').val(),
                    Jmbg: $('#regJmbg').val(),
                    Gender: $('#regGender').val(),
                    Username: $('#regUsername').val(),
                    Password: $('#regPass').val()
                },
                success: function (data) {
                    alert("Uspesna registracija!");
                    sessionStorage.setItem('accessToken', data.accessToken);
                    sessionStorage.setItem('activeUser', JSON.stringify(data.user));
                    $('#btnLoginForm').hide();
                    $('#profileButtons').show();
                    $('#btnNewDrive').show();
                    $('#btnDriveFilters').show();
                    $('#btnDriverLocation').hide();
                    $('#btnDriverAllDrives').hide();
                    $('#btnDispatcherAllDrives').hide();
                    $('#displayRegister').fadeOut('slow', 'swing', showHome);
                    formReset();
                },
                error: function () {
                    alert("Greska pri registraciji!");
                }
            });
        }
    });

    //$("#btnRegister").click(function () {
    //    $('#displayRegister').fadeOut('slow', 'swing', showHome);
    //});

    $('#regName').on('input', function () {
        let input = $(this);
        let is_name = input.val();
        if (is_name)
        {
            input.removeClass("reg-table-td-input").addClass("reg-table-td-ok");
            input.removeClass("reg-table-td-error").addClass("reg-table-td-ok");
            $('#errorName').text(' ');
            sendData = true;
        }
        else
        {
            input.removeClass("reg-table-td-input").addClass("reg-table-td-error");
            input.removeClass("reg-table-td-ok").addClass("reg-table-td-error");
            $('#errorName').text('This field is required!');
            sendData = false;
        }
    });

    $('#regSurname').on('input', function () {
        let input = $(this);
        let is_surname = input.val();
        if (is_surname) {
            input.removeClass("reg-table-td-input").addClass("reg-table-td-ok");
            input.removeClass("reg-table-td-error").addClass("reg-table-td-ok");
            $('#errorSurname').text(' ');
            sendData = true;
        }
        else {
            input.removeClass("reg-table-td-input").addClass("reg-table-td-error");
            input.removeClass("reg-table-td-ok").addClass("reg-table-td-error");
            $('#errorSurname').text('This field is required!');
            sendData = false;
        }
    });

    $('#regJmbg').on('input', function () {
        let input = $(this);
        let re = /^\b\d{13}\b$/i;
        let is_jmbg = re.test(input.val());
        if (is_jmbg) {
            input.removeClass("reg-table-td-input").addClass("reg-table-td-ok");
            input.removeClass("reg-table-td-error").addClass("reg-table-td-ok");
            $('#errorJmbg').text(' ');
            sendData = true;
        }
        else {
            input.removeClass("reg-table-td-input").addClass("reg-table-td-error");
            input.removeClass("reg-table-td-ok").addClass("reg-table-td-error");
            $('#errorJmbg').text('You must enter 13 numbers!');
            sendData = false;
        }
    });

    $('#regEmail').on('input', function () {
        let input = $(this);
        let re = /^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$/;
        let is_email = re.test(input.val());
        if (is_email)
        {
            input.removeClass("reg-table-td-input").addClass("reg-table-td-ok");
            input.removeClass("reg-table-td-error").addClass("reg-table-td-ok");
            $('#errorEmail').text(' ');
            sendData = true;
        }
        else
        {
            input.removeClass("reg-table-td-input").addClass("reg-table-td-error");
            input.removeClass("reg-table-td-ok").addClass("reg-table-td-error");
            $('#errorEmail').text('Email is invalid!');
            sendData = false;
        }
    });

    $('#regPhone').on('input', function () {
        let input = $(this);
        let re = /^((\+[1-9]{1,4}[ \-]*)|(\([0-9]{2,3}\)[ \-]*)|([0-9]{2,4})[ \-]*)*?[0-9]{3,4}?[ \-]*[0-9]{3,4}?$/;
        let is_phone = re.test(input.val());
        if (is_phone) {
            input.removeClass("reg-table-td-error").addClass("reg-table-td-ok");
            input.removeClass("reg-table-td-input").addClass("reg-table-td-ok");
            $('#errorPhone').text(' ');
            sendData = true;
        }
        else {
            input.removeClass("reg-table-td-input").addClass("reg-table-td-error");
            input.removeClass("reg-table-td-ok").addClass("reg-table-td-error");
            $('#errorPhone').text('Phone number is invalid!');
            sendData = false;
        }
    });

    $("#regUsername").focusout(function () {
        if ($('#displayRegisterHeader').text() === 'REGISTER FORM' || $('#displayRegisterHeader').text() === 'NEW DRIVER') {
            $.ajax({
                url: '/api/Register/CheckUsername',
                method: 'POST',
                data: {
                    Username: $('#regUsername').val(),
                    Password: $('#regPass').val()
                },
                success: function (data) {
                    $('#regUsername').removeClass("reg-table-td-input").addClass("reg-table-td-ok");
                    $('#regUsername').removeClass("reg-table-td-error").addClass("reg-table-td-ok");
                    $('#errorUsername').text(' ');
                    sendData = true;
                },
                error: function () {
                    $('#regUsername').removeClass("reg-table-td-input").addClass("reg-table-td-error");
                    $('#regUsername').removeClass("reg-table-td-ok").addClass("reg-table-td-error");
                    $('#errorUsername').text('Sorry but that username is allready taken!'),
                    sendData = false;
                }
            });
        }
    });

    $('#regPassRpt').focusout(function () {
        let input2 = $(this);
        let input1 = $('#regPass');
        let is_input1 = input1.val();
        let is_input2 = input2.val();
        if (is_input1 === is_input2) {
            $('#regPass').removeClass("reg-table-td-input").addClass("reg-table-td-ok");
            $('#regPass').removeClass("reg-table-td-error").addClass("reg-table-td-ok");
            $('#regPassRpt').removeClass("reg-table-td-input").addClass("reg-table-td-ok");
            $('#regPassRpt').removeClass("reg-table-td-error").addClass("reg-table-td-ok");
            $('#errorRegPass').text(' ');
            sendData = true;
        } else {
            $('#regPass').removeClass("reg-table-td-input").addClass("reg-table-td-error");
            $('#regPass').removeClass("reg-table-td-ok").addClass("reg-table-td-error");
            $('#regPassRpt').removeClass("reg-table-td-input").addClass("reg-table-td-error");
            $('#regPassRpt').removeClass("reg-table-td-ok").addClass("reg-table-td-error");
            $('#errorRegPass').text('Passwords must match!'),
            sendData = false;
        }
    });

    $('#regPass').on('input', function () {
        let input = $(this);
        let is_name = input.val();
        if (is_name) {
            $('#regPass').removeClass("reg-table-td-input").addClass("reg-table-td-ok");
            $('#regPass').removeClass("reg-table-td-error").addClass("reg-table-td-ok");
            $('#regPassRpt').removeClass("reg-table-td-input").addClass("reg-table-td-ok");
            $('#regPassRpt').removeClass("reg-table-td-error").addClass("reg-table-td-ok");
            $('#errorRegPass').text(' ');
            sendData = true;
        }
        else {
            $('#regPass').removeClass("reg-table-td-input").addClass("reg-table-td-error");
            $('#regPass').removeClass("reg-table-td-ok").addClass("reg-table-td-error");
            $('#regPassRpt').removeClass("reg-table-td-input").addClass("reg-table-td-error");
            $('#regPassRpt').removeClass("reg-table-td-ok").addClass("reg-table-td-error");
            $('#errorRegPass').text('You must enter password!'),
            sendData = false;
        }
    });

    $('#regCarId').on('input',function () {
        let input = $(this);
        let is_carId = input.val();
        if (is_carId) {
            input.removeClass("reg-table-td-input").addClass("reg-table-td-ok");
            input.removeClass("reg-table-td-error").addClass("reg-table-td-ok");
            $('#errorCarId').text(' ');
            sendData = true;
        }
        else {
            input.removeClass("reg-table-td-input").addClass("reg-table-td-error");
            input.removeClass("reg-table-td-ok").addClass("reg-table-td-error");
            $('#errorCarId').text('This field is required!');
            sendData = false;
        }
    });

    $('#regModelYear').on('input',function () {
        let input = $(this);
        let re = /^\b\d{4}\b$/i;
        let is_modelYear = re.test(input.val());
        if (is_modelYear) {
            input.removeClass("reg-table-td-input").addClass("reg-table-td-ok");
            input.removeClass("reg-table-td-error").addClass("reg-table-td-ok");
            $('#errorModelYear').text(' ');
            sendData = true;
        }
        else {
            input.removeClass("reg-table-td-input").addClass("reg-table-td-error");
            input.removeClass("reg-table-td-ok").addClass("reg-table-td-error");
            $('#errorModelYear').text('This field is required!');
            sendData = false;
        }
    });

    $('#regNumber').on('input',function () {
        let input = $(this);
        let re = /[A-Z][A-Z][0-9]+[A-Z][A-Z]/;
        let is_regNum = re.test(input.val());
        if (is_regNum) {
            input.removeClass("reg-table-td-input").addClass("reg-table-td-ok");
            input.removeClass("reg-table-td-error").addClass("reg-table-td-ok");
            $('#errorRegNum').text(' ');
            sendData = true;
        }
        else {
            input.removeClass("reg-table-td-input").addClass("reg-table-td-error");
            input.removeClass("reg-table-td-ok").addClass("reg-table-td-error");
            $('#errorRegNum').text('This field is required!');
            sendData = false;
        }
    });

    function CheckInput() {
        if ($('#regPass').val() && $('#regName').val() && $('#regSurname').val() && $('#regUsername').val() && $('#regJmbg').val() && $('#regEmail').val() && $('#regPhone').val()) {
            sendData = true;
        } else {
            sendData = false;
        }
    }
});