$(document).ready(function () {

    $('#btnCreateDrive').click(function () {
        var token = sessionStorage.getItem('accessToken');
        var user = JSON.parse(sessionStorage.getItem('activeUser'));

        if (user.role === 'Customer') {
            alert(JSON.stringify(JSON.parse(user)));

            let drive = {
                date: `${$('#driveDate').val()}`,
                address:
                    {
                        address: `${$('#driveAddress').val()}`,
                        x: $('#driveAddressX').val(),
                        y: $('#driveAddressY').val()
                    },
                carType: `${$('#driveCar').val()}`,
                orderedBy: JSON.parse(user)
            };

            alert(JSON.stringify(drive));

            $.ajax({
                type: 'POST',
                url: '/api/Customer/CreateNewDrive',
                data: JSON.stringify(drive),
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                headers: {
                    'Authorization': 'Basic ' + token.toString()
                },
                success: function (data) {
                    $("#displayNewDrive").fadeOut('slow', 'swing');
                    $("#blurBackground").fadeOut('slow', 'swing');
                },
                error: function () {
                    alert("Greska pri kreiranju voznje!");
                }
            });
        } else if (user.role === 'Dispatcher') {
            let driver;
            let chosen = $('#freeDriver').val();

            $.ajax({
                type: 'GET',
                url: '/api/Driver/GetDriver',
                data: {
                    id: chosen
                },
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                headers: {
                    'Authorization': 'Basic ' + token.toString()
                },
                success: function (data) {
                    alert(JSON.stringify(data));
                    driver = JSON.stringify(data);

                    let drive = {
                        date: `${$('#driveDate').val()}`,
                        address:
                            {
                                address: `${$('#driveAddress').val()}`,
                                x: $('#driveAddressX').val(),
                                y: $('#driveAddressY').val()
                            },
                        carType: $('#driveCar').val(),
                        approvedBy: user,
                        drivedBy: JSON.parse(driver)
                    };

                    alert(JSON.stringify(drive));

                    $.ajax({
                        type: 'POST',
                        url: '/api/Dispatcher/CreateDrive',
                        data: JSON.stringify(drive),
                        contentType: 'application/json; charset=utf-8',
                        dataType: 'json',
                        headers: {
                            'Authorization': 'Basic ' + token.toString()
                        },
                        success: function (data) {
                            $("#displayNewDrive").fadeOut('slow', 'swing');
                            $("#blurBackground").fadeOut('slow', 'swing');
                        },
                        error: function () {
                            alert("Greska pri kreiranju voznje!");
                        }
                    });
                },
                error: function () {
                    alert("Greska pri kreiranju voznje, molimo vas pokusajte kasnije!");
                }
            });
        }
    });
});