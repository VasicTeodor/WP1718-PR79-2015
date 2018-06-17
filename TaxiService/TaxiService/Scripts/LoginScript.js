$(document).ready(function () {
    $('#btnSendLogin').click(function () {
        $.ajax({
            url: '/api/Login/SignIn',
            method: 'POST',
            contentType: 'application/json; charset=utf-8',
            data: {
                Username: $('#loginUsername').val(),
                Password: $('#loginPassword').val()
            },
            success: function (data) {
                $('#displayLoginForm').fadeOut('slow', 'swing');
                $("#blurBackground").fadeOut('slow', 'swing');
            },
            error: function (jqXHR) {
                $('#errorLoginLbl').text('Wrong username or password!');
            }
        });
    });
});