$(document).ready(function () {
    var sendData = false;

    let showHome = function () {
        $('#displayBanner').fadeIn('slow', 'swing');
        $('#displayTrips').fadeIn('slow', 'swing');
    };

    $("#btnRegister").click(function () {
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
                success: function () {
                    alert("Uspesna registracija!");
                    $('#displayRegister').fadeOut('slow', 'swing', showHome);
                },
                error: function (jqXHR) {
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
        let is_jmbg = input.val();
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
        $.ajax({
            url: '/api/Register/CheckUsername',
            method: 'POST',
            data: {
                Username: $('#regUsername').val(),
                Password: $('#regPass').val()
            },
            success: function () {
                $('#regUsername').removeClass("reg-table-td-input").addClass("reg-table-td-ok");
                $('#regUsername').removeClass("reg-table-td-error").addClass("reg-table-td-ok");
                $('#errorUsername').text(' ');
                sendData = true;
            },
            error: function (jqXHR) {
                $('#regUsername').removeClass("reg-table-td-input").addClass("reg-table-td-error");
                $('#regUsername').removeClass("reg-table-td-ok").addClass("reg-table-td-error");
                $('#errorUsername').text('Sorry but that username is allready taken!')
                sendData = false;
            }
        });
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
            $('#errorRegPass').text('Passwords must match!')
            sendData = false;
        }
    });
});