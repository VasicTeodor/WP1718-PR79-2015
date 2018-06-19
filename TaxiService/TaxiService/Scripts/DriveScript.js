$(document).ready(function () {
    var token = sessionStorage.getItem('accessToken');
    var user = sessionStorage.getItem('activeUser');

    $('#btnCreateDrive').click(function () {

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

            $.ajax({
                type: 'GET',
                url: '/api/Driver/GetDriver',
                data: id = JSON.stringify($('#freeDriver').val()),
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                headers: {
                    'Authorization': 'Basic ' + token.toString()
                },
                success: function (data) {
                    alert(JSON.stringify(data));
                    driver = JSON.stringify(data);
                },
                error: function () {
                    alert("Greska pri kreiranju voznje, molimo vas pokusajte kasnije!");
                }
            });

            let drive = {
                date: `${$('#driveDate').val()}`,
                address:
                    {
                        address: `${$('#driveAddress').val()}`,
                        x: $('#driveAddressX').val(),
                        y: $('#driveAddressY').val()
                    },
                carType: `${$('#driveCar').val()}`,
                approvedBy: JSON.parse(user),
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
        }
    });
});