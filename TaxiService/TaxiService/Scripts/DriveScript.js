﻿var updateDriveId;

function DisplayNewDrive(data) {
    let user = JSON.parse(sessionStorage.getItem('activeUser'));

    let carType = 'Any';

    if (data.carType == 1) {
        carType = 'Car';
    } else if (data.carType == 2) {
        carType = 'Van';
    }

    let comment = '';

    if (data.comments != null) {

        let role = '';
        if (data.comments.createdBy.role == 0) {
            role = 'Driver';
        } else {
            role = 'Customer';
        }

        comment = '<tr class="table-tr-drive">' +
            '<td class="table-td-drive">Comment:</td>' +
            '<td class="table-td-drive">' +
            '<div>' +
            '<p class="comment-bold">' + data.comments.description.toString() + '</p>' +
            '<p class="comment-italic">' + data.comments.createdOn.toString() + '</p>' +
            '<p class="comment-bold">GRADE: ' + data.grade.toString() + '</p>' +
            '<p class="comment-italic">' + data.createdBy.name + ' ' + data.createdBy.surname + ' - ' + role + '</p>' +
            '</div>' +
            '</td>' +
            '</tr>';
    }

    let buttons = '';

    if (user.role === 'Customer') {
        buttons = '<tr class="table-tr-drive">' +
            '<td colspan="2" class="table-td-drive" align="right">' +
            '<button class="edit-drive" id="btnEditDrive' + counter + '"  onclick="EditDrive(this);" value="' + data.driveId.toString() + '">Edit</button>' +
            '<button class="edit-drive" id="btnQuitDrive' + counter + '"  onclick="QuitDrive(this);" value="' + data.driveId.toString() + '">Quit</button>' +
            '</td>' +
            '</tr>';
    }

    let driver = '';

    if (data.drivedBy != null) {
        driver = '<tr class="table-tr-drive">' +
            '<td class="table-td-drive">Driver:</td>' +
            '<td class="table-td-drive"><p class="home-tt-display2">' + data.drivedBy.name + ' ' + data.drivedBy.surname + '</p></td>' +
            '</tr>';
    }

    let dispatcher = '';

    if (data.approvedBy != null) {
        dispatcher = '<tr class="table-tr-drive">' +
            '<td class="table-td-drive">Dispatcher:</td>' +
            '<td class="table-td-drive"><p class="home-tt-display2">' + data.approvedBy.name + ' ' + data.approvedBy.surname + '</p></td>' +
            '</tr>';
    }

    let customer = '';

    if (data.orderedBy != null) {
        customer = '<tr class="table-tr-drive">' +
            '<td class="table-td-drive">Customer:</td>' +
            '<td class="table-td-drive"><p class="home-tt-display2">' + data.orderedBy.name + ' ' + data.orderedBy.surname + '</p></td>' +
            '</tr>';
    }

    let destination = '';

    if (data.destination.address.toString() != 'Unset') {
        destination = '<tr class="table-tr-drive">' +
            '<td class="table-td-drive">Destination:</td>' +
            '<td class="table-td-drive"><p class="home-tt-display2">' + data.destination.address.toString() + '</p></td>' +
            '</tr>';
    }

    $('#fillInTrips').append('<div class="home-tt-main">' +
        '<p class="home-tt-display">' + data.date.toString() + '</p>' +
        '<p class="home-tt-display">' + data.address.address.toString() + '</p>' +
        '<button class="expand-table" onclick="showOtherInfo(this);" id="' + counter.toString() + '">+</button>' +
        '</div>' +
        '<div class="home-tt-other" style="display: none;" id="otherInfo' + counter.toString() + '">' +
        '<div class="tableWrap">' +
        '<table class="table-drive">' +
        '<tr class="table-tr-drive">' +
        '<td class="table-td-drive">Date:</td>' +
        '<td class="table-td-drive"><p class="home-tt-display2">' + data.date.toString() + '</p></td>' +
        '</tr>' +
        '<tr class="table-tr-drive">' +
        '<td class="table-td-drive">Address:</td>' +
        '<td class="table-td-drive"><p class="home-tt-display2">' + data.address.address.toString() + '</p></td>' +
        '</tr>' +
        '<tr class="table-tr-drive">' +
        '<td class="table-td-drive">Car Type:</td>' +
        '<td class="table-td-drive"><p class="home-tt-display2">' + carType + '</p></td>' +
        '</tr>'
        + destination +
        '<tr class="table-tr-drive">' +
        '<td class="table-td-drive">Price:</td>' +
        '<td class="table-td-drive"><p class="home-tt-display2">' + data.price.toString() + ' RSD</p></td>' +
        '</tr>'
        + customer
        + dispatcher
        + driver
        + comment +
        buttons  +
        '</table>' +
        '</div>' +
        '</div>');
    counter++;
}

function QuitDrive(elem) {
    let user = JSON.parse(sessionStorage.getItem('activeUser'));
    let token = sessionStorage.getItem('accessToken');
    let driveId = $('#' + elem.id).val();

    let dataS = {
        quitId: driveId
    };

    if (user.role === 'Customer') {
        $.ajax({
            type: 'PUT',
            url: '/api/Customer/QuitDrive',
            data: JSON.stringify(dataS),
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            headers: {
                'Authorization': 'Basic ' + token.toString()
            },
            success: function(data) {},
            error: function () {
                alert("Error while canceling drive, try again later!");
            }
        });
    }
}

function EditDrive(elem) {
    let user = JSON.parse(sessionStorage.getItem('activeUser'));
    let token = sessionStorage.getItem('accessToken');
    let driveId = $('#' + elem.id).val();
    updateDriveId = driveId;

    $.ajax({
        type: 'GET',
        url: '/api/Drive/GetDrive',
        data: {
            id: driveId
        },
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        headers: {
            'Authorization': 'Basic ' + token.toString()
        },
        success: function (data) {
            alert(JSON.stringify(data));

            $('#driveAddress').val(data.address.address);
            $('#driveAddressX').val(data.address.x);
            $('#driveAddressY').val(data.address.y);
            if (data.carType == 0) {
                $('#driveCar').val('Bez_Naznake');
            } else if (data.carType == 1) {
                $('#driveCar').val('Car');
            } else {
                $('#driveCar').val('Kombi');
            }

            if (user.role === "Customer") {
                $('#dispatcherDrive').hide();
                $('#driverDrive').hide();
                $('#btnEditDrive').show();
                $('#btnCreateDrive').hide();
                $('#driveAddress').parent().parent().show();
                $('#driveAddressX').parent().parent().show();
                $('#driveAddressY').parent().parent().show();
                $('#driveCar').parent().parent().show();
            }
            else if (user.role === 'Dispatcher') {
                $('#driveAddress').parent().parent().hide();
                $('#driveAddressX').parent().parent().hide();
                $('#driveAddressY').parent().parent().hide();
                $('#driveCar').parent().parent().hide();
                $('#driverDrive').hide();
                $('#dispatcherDrive').show();
                $('#btnCreateDrive').hide();
                $('#btnEditDrive').show();
                $.ajax({
                    type: 'GET',
                    url: '/api/Driver/GetFreeDrivers',
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
                        alert("Server side error, please try again later!");
                    }
                });
            } else {
                $('#driveAddress').parent().parent().hide();
                $('#driveAddressX').parent().parent().hide();
                $('#driveAddressY').parent().parent().hide();
                $('#driveCar').parent().parent().hide();
                $('#driverDrive').show();
                $('#dispatcherDrive').hide();
                $('#btnCreateDrive').hide();
                $('#btnEditDrive').show();
            }

            $("#blurBackground").fadeIn('slow', 'swing');
            $("#displayNewDrive").fadeIn('slow', 'swing');
            
        },
        error: function () {
            alert("Server side error, please try again later!");
        }
    });
}

function DriverDrive(elem) {
    let user = JSON.parse(sessionStorage.getItem('activeUser'));
    let token = sessionStorage.getItem('accessToken');
    let driveId = $('#' + elem.id).val();

    let dataS = {
        id: driveId,
        driverId: user.id
    };

    if (user.role === 'Driver') {
        $.ajax({
            type: 'PUT',
            url: '/api/Driver/AcceptDrive',
            data: JSON.stringify(dataS),
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            headers: {
                'Authorization': 'Basic ' + token.toString()
            },
            success: function (data) { },
            error: function () {
                alert("Error while accepting drive, try again later!");
            }
        });
    }
}

var drives = function GetAllDrives() {
    let token = sessionStorage.getItem('accessToken');

    $.ajax({
        type: 'GET',
        url: '/api/Drive/GetAllDrives',
        dataType: 'json',
        headers: {
            'Authorization': 'Basic ' + token.toString()
        },
        success: function (data) {
            alert(JSON.stringify(data));
            PrintAllDrives(data);
        },
        error: function () {
            alert("Server side error, please try again later!");
        }
    });
};

$(document).ready(function () {

    $('#btnCreateDrive').click(function () {
        var token = sessionStorage.getItem('accessToken');
        var user = JSON.parse(sessionStorage.getItem('activeUser'));

        if (user.role === 'Customer') {
            alert(JSON.stringify(user));

            let drive = {
                address:
                    {
                        address: `${$('#driveAddress').val()}`,
                        x: $('#driveAddressX').val(),
                        y: $('#driveAddressY').val()
                    },
                carType: `${$('#driveCar').val()}`,
                orderedBy: user
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
                    alert(JSON.stringify(data));
                    DisplayNewDrive(data);
                    $("#displayNewDrive").fadeOut('slow', 'swing');
                    $("#blurBackground").fadeOut('slow', 'swing');
                },
                error: function () {
                    alert("Error while creating new drive!");
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
                        address:
                            {
                                address: `${$('#driveAddress').val()}`,
                                x: $('#driveAddressX').val(),
                                y: $('#driveAddressY').val()
                            },
                        carType: `${$('#driveCar').val()}`,
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
                            alert(JSON.stringify(data));
                            DisplayNewDrive(data);
                            $("#displayNewDrive").fadeOut('slow', 'swing');
                            $("#blurBackground").fadeOut('slow', 'swing');
                        },
                        error: function () {
                            alert("Error while creating new drive!");
                        }
                    });
                },
                error: function () {
                    alert("Error while creating new drive, please try again later!");
                }
            });
        }
    });

    $('#btnEditDrive').click(function () {
        var token = sessionStorage.getItem('accessToken');
        var user = JSON.parse(sessionStorage.getItem('activeUser'));

        if (user.role !== 'Driver') {
            let uri = '/api/';
            let drive;
            if (user.role === 'Customer') {
                uri += 'Customer/UpdateDrive';
                drive = {
                    driveId: updateDriveId,
                    address:
                        {
                            address: `${$('#driveAddress').val()}`,
                            x: $('#driveAddressX').val(),
                            y: $('#driveAddressY').val()
                        },
                    carType: `${$('#driveCar').val()}`,
                    orderedBy: user
                };
            } else if (user.role === 'Dispatcher') {
                uri += 'Dispatcher/UpdateDrive';
                drive = {
                    driveId: updateDriveId,
                    approvedBy: user.id,
                    drivedBy: $('#freeDriver').val()
                };
            }

            $.ajax({
                type: 'PUT',
                url: uri,
                data: JSON.stringify(drive),
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                headers: {
                    'Authorization': 'Basic ' + token.toString()
                },
                success: function (data) {
                    $("#blurBackground").fadeOut('slow', 'swing');
                    $("#displayNewDrive").fadeOut('slow', 'swing');
                },
                error: function () {
                    alert("Error while updating drive, try again later!");
                }
            });

            updateDriveId = '';
        } else {
            if ($('#driverFinishedState').val() === 'Successful') {
                $("#displayNewDrive").fadeOut('slow', 'swing');
                $("#displayDriverFinished").fadeIn('slow', 'swing');
            } else {
                $("#displayNewDrive").fadeOut('slow', 'swing');
                $("#displayCommentForm").fadeIn('slow', 'swing');
            }
        }
    });

    $('#btnFinishDrive').click(function () {
        var token = sessionStorage.getItem('accessToken');
        var user = JSON.parse(sessionStorage.getItem('activeUser'));

        let drive = {
            driveId: updateDriveId,
            drivedBy: user.id,
            state: $('#driverFinishedState').val(),
            destination: $('#driveDestinationAddress').val(),
            destinationX: $('#driveDestinationAddressX').val(),
            destinationY: $('#driveDestinationAddressY').val(),
            price: $('#drivePrice').val()
        };

        $.ajax({
            type: 'PUT',
            url: '/api/Driver/UpdateDrive',
            data: JSON.stringify(drive),
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            headers: {
                'Authorization': 'Basic ' + token.toString()
            },
            success: function (data) {
                $("#blurBackground").fadeOut('slow', 'swing');
                $("#displayNewDrive").fadeOut('slow', 'swing');
            },
            error: function () {
                alert("Error while updating drive, try again later!");
            }
        });

        $("#blurBackground").fadeOut('slow', 'swing');
        $("#displayDriverFinished").fadeOut('slow', 'swing');
        updateDriveId = '';
    });

    $('#btnCreateComment').click(function () {
        var token = sessionStorage.getItem('accessToken');
        var user = JSON.parse(sessionStorage.getItem('activeUser'));

        let drive = {
            driveId: updateDriveId,
            drivedBy: user.id,
            state: $('#driverFinishedState').val(),
            text: $('#commentText').val(),
            grade: $('#commentGrade').val()
        };

        $.ajax({
            type: 'PUT',
            url: '/api/Driver/FailedDrive',
            data: JSON.stringify(drive),
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            headers: {
                'Authorization': 'Basic ' + token.toString()
            },
            success: function (data) {
                $("#blurBackground").fadeOut('slow', 'swing');
                $("#displayCommentForm").fadeOut('slow', 'swing');
            },
            error: function () {
                alert("Error while updating drive, try again later!");
            }
        });

        $("#blurBackground").fadeOut('slow', 'swing');
        $("#displayCommentForm").fadeOut('slow', 'swing');
        updateDriveId = '';
    });

    $('#btnDriverAllDrives').click(drives);
    $('#btnDispatcherAllDrives').click(drives);

});