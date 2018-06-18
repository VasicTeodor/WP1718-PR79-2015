$(document).ready(function () {
    var sendData = false;

    function CheckUsername() {
        let input = $('loginUsername');
        let is_name = input.val();
        if (is_name) {
            input.css('borderColor','lightgreen')
            $('#errorLoginLbl').text(' ');
            sendData = true;
        }
        else {
            input.css('borderColor', 'red')
            $('#errorLoginLbl').text('Username is required!');
            sendData = false;
        }
    };

    function CheckPassword() {
        let input = $('#loginPassword');
        let is_name = input.val();
        if (is_name) {
            input.css('borderColor', 'lightgreen')
            $('#errorLoginLbl').text(' ');
            sendData = true;
        }
        else {
            input.css('borderColor', 'red')
            $('#errorLoginLbl').text('Password is required!');
            sendData = false;
        }
    };

    $('#btnSendLogin').click(function () {
        CheckUsername();
        CheckPassword();

        if (sendData) {
            let loginClass = {
                username: `${$('#loginUsername').val()}`,
                password: `${$('#loginPassword').val()}`
            };

            $.ajax({
                type: 'POST',
                url: '/api/Login/SignIn',
                data: JSON.stringify(loginClass),
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function (data) {
                    alert(data.accessToken);
                    alert(JSON.stringify(data.user));
                    sessionStorage.setItem('accessToken', data.accessToken);
                    sessionStorage.setItem('activeUser', JSON.stringify(data.user));
                    $('#displayLoginForm').fadeOut('slow', 'swing');
                    $("#blurBackground").fadeOut('slow', 'swing');
                    $('#btnLogin').hide();
                    $('#profileButtons').show();
                },
                error: function (jqXHR) {
                    $('#errorLoginLbl').text('Wrong username or password!');
                }
            });
        }
    });
});