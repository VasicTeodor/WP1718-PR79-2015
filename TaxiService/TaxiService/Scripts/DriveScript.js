$(document).ready(function () {
    var user = sessionStorage.getItem('activeUser');

    $('#btnCreateDrive').click(function () {

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
            success: function (data) {
                $("#displayNewDrive").fadeOut('slow', 'swing');
                $("#blurBackground").fadeOut('slow', 'swing');
            },
            error: function (jqXHR) {
                
            }
        });

    });
});