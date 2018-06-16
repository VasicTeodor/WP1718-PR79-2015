$(document).ready(function () {
    $("#btnRegister").click(function () {
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
                Username: $('#regName').val(),
                Password: $('#regPassword').val()
            },
            success: function () {
                alert("Uspesna registracija!");
            },
            error: function (jqXHR) {
                alert("Greska pri registraciji!");
            }
        });
    });
});