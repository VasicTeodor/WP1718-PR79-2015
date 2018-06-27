var updateDriveId;
var price = true;

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

        let d = new Date(data.comments.createdOn),
            minutes = d.getMinutes().toString().length == 1 ? '0' + d.getMinutes() : d.getMinutes(),
            hours = d.getHours().toString().length == 1 ? '0' + d.getHours() : d.getHours(),
            seconds = d.getSeconds().toString().length == 1 ? '0' + d.getSeconds() : d.getSeconds(),
            months = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'],
            days = ['Sun', 'Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat'];
        let realDate = days[d.getDay()] + ' ' + d.getDate() + ' ' + months[d.getMonth()] + ' ' + d.getFullYear() + ' ' + hours + ':' + minutes + ':' + seconds;

        comment = '<tr class="table-tr-drive">' +
            '<td class="table-td-drive">Comment:</td>' +
            '<td class="table-td-drive">' +
            '<div>' +
            '<p class="comment-bold">' + data.comments.description.toString() + '</p>' +
            '<p class="comment-italic">' + realDate + '</p>' +
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

        $('#displayCurDrive').empty();
        $('#displayCurDrive').append('<table class="cur-drive-table">' +
            '<tr>' +
            '<th colspan="2">CURRENT DRIVE</th>' +
            '</tr>' +
            '<tr>' +
            '<td>SATE:</td>' +
            '<td> On waiting list </td>' +
            '</tr>' +
            '<tr>' +
            '<td>ADDRESS:</td>' +
            '<td>' + data.address.address.slice(0, data.address.address.indexOf(',')) + '</td>' +
            '</tr>' +
            '<tr>' +
            '<td>CAR TYPE:</td>' +
            '<td>' + carType + '</td>' +
            '</tr>' +
            '</table>');
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

    let state = '';

    if (data.state === 0) {
        state = 'On waiting list';
    } else if (data.state === 1) {
        state = 'Canceled';
    } else if (data.state === 2) {
        state = 'Formated';
    } else if (data.state === 3) {
        state = 'Processed';
    } else if (data.state === 4) {
        state = 'Accepted';
    } else if (data.state === 5) {
        state = 'Successful';
    } else {
        state = 'Unsuccessful';
    }

    let d = new Date(data.date),
        minutes = d.getMinutes().toString().length == 1 ? '0' + d.getMinutes() : d.getMinutes(),
        hours = d.getHours().toString().length == 1 ? '0' + d.getHours() : d.getHours(),
        seconds = d.getSeconds().toString().length == 1 ? '0' + d.getSeconds() : d.getSeconds(),
        months = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'],
        days = ['Sun', 'Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat'];
    let realDate = days[d.getDay()] + ' ' + d.getDate() + ' ' + months[d.getMonth()] + ' ' + d.getFullYear() + ' ' + hours + ':' + minutes + ':' + seconds;

    $('#fillInTrips').append('<div class="home-tt-main">' +
        '<p class="home-tt-display">' + realDate + '</p>' +
        '<p class="home-tt-display">' + data.address.address.slice(0, data.address.address.indexOf(',')) + '</p>' +
        '<button class="expand-table" onclick="showOtherInfo(this);" id="' + counter.toString() + '">+</button>' +
        '</div>' +
        '<div class="home-tt-other" style="display: none;" id="otherInfo' + counter.toString() + '">' +
        '<div class="tableWrap">' +
        '<table class="table-drive">' +
        '<tr class="table-tr-drive">' +
        '<td class="table-td-drive">Drive State:</td>' +
        '<td class="table-td-drive"><p class="home-tt-display2">' + state + '</p></td>' +
        '</tr>' +
        '<tr class="table-tr-drive">' +
        '<td class="table-td-drive">Date:</td>' +
        '<td class="table-td-drive"><p class="home-tt-display2">' + realDate + '</p></td>' +
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

function LeaveComment(elem) {
    let driveId = $('#' + elem.id).val();
    updateDriveId = driveId;

    $("#blurBackground").fadeIn('slow', 'swing');
    $("#displayCommentForm").fadeIn('slow', 'swing');
}

function QuitDrive(elem) {
    let user = JSON.parse(sessionStorage.getItem('activeUser'));
    let token = sessionStorage.getItem('accessToken');
    let driveId = $('#' + elem.id).val();
    updateDriveId = driveId;

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

        $("#blurBackground").fadeIn('slow', 'swing');
        $('#btnExitComment').hide();
        $("#displayCommentForm").fadeIn('slow', 'swing');
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
                $('#map').parent().parent().show();
            }
            else if (user.role === 'Dispatcher') {
                $('#driveAddress').parent().parent().hide();
                $('#driveAddressX').parent().parent().hide();
                $('#driveAddressY').parent().parent().hide();
                $('#driveCar').parent().parent().hide();
                $('#map').parent().parent().hide();
                $('#driverDrive').hide();
                $('#dispatcherDrive').show();
                $('#btnCreateDrive').hide();
                $('#btnEditDrive').show();
                freeDriversLocation(data.carType, data.address.x, data.address.y);
            } else {
                $('#driveAddress').parent().parent().hide();
                $('#driveAddressX').parent().parent().hide();
                $('#driveAddressY').parent().parent().hide();
                $('#driveCar').parent().parent().hide();
                $('#map').parent().parent().hide();
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
            success: function (data) {
                sessionStorage.setItem('activeUser', JSON.stringify(data));
                let text = elem.id;
                let dispEdit = text.replace('btnAcceptDrive', 'btnEditDrive');
                $('#' + elem.id).fadeOut('slow', 'swing');
                $('#' + dispEdit).fadeIn('slow', 'swing');
            },
            statusCode: {
                410: function () {
                    alert('This drive is not active anymore.');
                    drives();
                },
                404: function () {
                    alert("Error while accepting drive, try again later!");
                }
            }
        });
    }
}

var myDrives = function GetAllMyDrives() {
    if (!filtersOn && !allDrivesOn) {
        let user = JSON.parse(sessionStorage.getItem('activeUser'));
        let token = sessionStorage.getItem('accessToken');

        $.ajax({
            type: 'GET',
            url: '/api/Drive/GetAllDrivesForId',
            data: {
                id: user.id,
                role: user.role
            },
            dataType: 'json',
            headers: {
                'Authorization': 'Basic ' + token.toString()
            },
            success: function (data) {
                PrintAllDrives(data);
            },
            error: function () {
                alert("Server side error, please try again later!");
            }
        });
    }
};

var drives = function GetAllDrives() {
    //if(!allDrivesOn && !filtersOn)
    let user = JSON.parse(sessionStorage.getItem('activeUser'));
    let token = sessionStorage.getItem('accessToken');

    $.ajax({
        type: 'GET',
        url: '/api/Drive/GetAllDrives',
        data: {
            id: user.id,
            role: user.role
        },
        dataType: 'json',
        headers: {
            'Authorization': 'Basic ' + token.toString()
        },
        success: function (data) {
            PrintAllDrives(data);
            allDrivesOn = true;
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
                    DisplayNewDrive(data);
                    $("#displayNewDrive").fadeOut('slow', 'swing');
                    $("#blurBackground").fadeOut('slow', 'swing');
                    $('#btnNewDrive').prop("disabled", true);
                    formReset();
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
                            DisplayNewDrive(data);
                            $("#displayNewDrive").fadeOut('slow', 'swing');
                            $("#blurBackground").fadeOut('slow', 'swing');
                            formReset();
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
                    myDrives();
                    $("#blurBackground").fadeOut('slow', 'swing');
                    $("#displayNewDrive").fadeOut('slow', 'swing');
                    formReset();
                },
                statusCode: {
                    410: function () {
                        if (user.role === 'Dispatcher') {
                            alert('This drive is not active anymore.');
                            drives();
                        } else {
                            alert("Your drive is acceppted, you can not edit it anymore.");
                            myDrives();
                        }
                    },
                    404: function () {
                        alert("Error while updating drive, try again later!");
                    }
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
            price: $('#drivePrice').val().replace(/\./,',')
        };

        if (price && $('#drivePrice').val()) {
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
                    updateDriverLocation($('#driveDestinationAddress').val(), $('#driveDestinationAddressX').val(), $('#driveDestinationAddressY').val());
                    myDrives();
                    sessionStorage.setItem('activeUser', JSON.stringify(data));
                    $("#blurBackground").fadeOut('slow', 'swing');
                    $("#displayNewDrive").fadeOut('slow', 'swing');
                    formReset();
                },
                error: function () {
                    alert("Error while updating drive, try again later!");
                }
            });

            $("#blurBackground").fadeOut('slow', 'swing');
            $("#displayDriverFinished").fadeOut('slow', 'swing');
            updateDriveId = '';
        }
    });

    $('#btnCreateComment').click(function () {
        var token = sessionStorage.getItem('accessToken');
        var user = JSON.parse(sessionStorage.getItem('activeUser'));

        let comment = $('#commentText').val().toString();
        if (comment.length > 10) {
            let uri = '/api/';
            let drive;
            if (user.role === 'Driver') {
                drive = {
                    driveId: updateDriveId,
                    drivedBy: user.id,
                    state: $('#driverFinishedState').val(),
                    text: $('#commentText').val(),
                    grade: $('#commentGrade').val()
                };

                uri += 'Driver/FailedDrive';
            } else {
                drive = {
                    driveId: updateDriveId,
                    orderedBy: user.id,
                    text: $('#commentText').val(),
                    grade: $('#commentGrade').val()
                };

                uri += 'Customer/AddComment';
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
                    myDrives();
                    $("#blurBackground").fadeOut('slow', 'swing');
                    $("#displayCommentForm").fadeOut('slow', 'swing');
                    formReset();
                },
                error: function () {
                }
            });

            $('#btnExitComment').show();
            $("#blurBackground").fadeOut('slow', 'swing');
            $("#displayCommentForm").fadeOut('slow', 'swing');
            updateDriveId = '';
        }
    });

    $('#btnDriverAllDrives').click(drives);
    $('#btnDispatcherAllDrives').click(drives);

    $('#btnApplyFilters').click(function () {
        var token = sessionStorage.getItem('accessToken');
        var user = JSON.parse(sessionStorage.getItem('activeUser'));
        
        let filters;
            filters = {
                userRole: user.role,
                userId: user.id,
                sortBy: $('#selectSortBy').val(),
                filterBy: $('#slectFilterBy').val(),
                fromDate: $('#filterDateFrom').val(),
                toDate: $('#filterDateTo').val(),
                gradeFrom: $('#filterGradeFrom').val(),
                gradeTo: $('#filterGradeTo').val(),
                priceFrom: $('#filterPriceFrom').val().replace(/\./,','),
                priceTo: $('#filterPriceTo').val().replace(/\./, ','),
                searchRole: $('#filterRole').val(),
                filterName: $('#filterName').val(),
                filterSurname: $('#filterSurname').val(),
                whatDrives: $('#selectDrives').val(),
                drivesNearMe: $('#drivesNear').val()
        };

        if (user.role !== 'Customer') {
            filtersOn = true;
        }

        $.ajax({
            type: 'POST',
            url: '/api/Drive/Filters',
            data: JSON.stringify(filters),
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            headers: {
                'Authorization': 'Basic ' + token.toString()
            },
            success: function (data) {
                PrintAllDrives(data);
            },
            error: function () {
            }
        });
    });

    $("#drivePrice").on("keypress keyup blur", function (event) {
        //this.value = this.value.replace(/[^0-9\.]/g,'');
        $(this).val($(this).val().replace(/[^0-9\.]/g, ''));
        if ((event.which != 46 || $(this).val().indexOf(',') != -1) && (event.which < 48 || event.which > 57)) {
            if (event.keyCode != 8 && event.keyCode != 37 && event.keyCode != 39) {
                event.preventDefault();
            }
        }
    });

    $("#filterPriceFrom").on("keypress keyup blur", function (event) {
        //this.value = this.value.replace(/[^0-9\.]/g,'');
        $(this).val($(this).val().replace(/[^0-9\.]/g, ''));
        if ((event.which != 46 || $(this).val().indexOf(',') != -1) && (event.which < 48 || event.which > 57)) {
            if (event.keyCode != 8 && event.keyCode != 37 && event.keyCode != 39) {
                event.preventDefault();
            }
        }
    });

    $("#filterPriceTo").on("keypress keyup blur", function (event) {
        //this.value = this.value.replace(/[^0-9\.]/g,'');
        $(this).val($(this).val().replace(/[^0-9\.]/g, ''));
        if ((event.which != 46 || $(this).val().indexOf(',') != -1) && (event.which < 48 || event.which > 57)) {
            if (event.keyCode != 8 && event.keyCode != 37 && event.keyCode != 39) {
                event.preventDefault();
            }
        }
    });

    $('#driveCar').on('change', function () {
        var user = JSON.parse(sessionStorage.getItem('activeUser'));

        if (user.role === 'Dispatcher' && $('#driveAddressX').val()) {
            freeDriversLocation($('#driveCar').val(), $('#driveAddressX').val(), $('#driveAddressY').val());
        }
    });

    $('#drivePrice').on('input', function () {
        let input = $(this);
        if (input.val()) {

            price = true;
        }
        else {
            price = false;
        }
    });
});