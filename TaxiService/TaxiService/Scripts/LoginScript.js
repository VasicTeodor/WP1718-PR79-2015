var counter = 1;
$(document).ready(function () {
    var sendData = false;


    function CheckUsername() {
        let input = $('loginUsername');
        let is_name = input.val();
        if (is_name) {
            input.css('borderColor', 'lightgreen');
            $('#errorLoginLbl').text(' ');
            sendData = true;
        }
        else {
            input.css('borderColor', 'red');
            $('#errorLoginLbl').text('Username is required!');
            sendData = false;
        }
    }

    function CheckPassword() {
        let input = $('#loginPassword');
        let is_name = input.val();
        if (is_name) {
            input.css('borderColor', 'lightgreen');
            $('#errorLoginLbl').text(' ');
            sendData = true;
        }
        else {
            input.css('borderColor', 'red');
            $('#errorLoginLbl').text('Password is required!');
            sendData = false;
        }
    }

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
                    $('#btnLoginForm').hide();
                    $('#profileButtons').show();
                    if (data.user.role === 'Dispatcher') {
                        $('#btnAddDriver').show();
                        $('#menu').css('height', '248');
                    } else {
                        $('#btnAddDriver').hide();
                        $('#menu').css('height', '200');
                    }
                    let user = JSON.parse(sessionStorage.getItem('activeUser'));
                    let allDrives = user.drives;
                    for (let i = 0; i < allDrives.length; i++) {
                        //let fullName = JSON.stringify(data[i].name) + ' ' + JSON.stringify(data[i].surname);
                        //$('#freeDriver').append('<option value="' + JSON.stringify(data[i].id).replace(/"|_/g, '') + '">' + fullName.replace(/"|_/g, '') + '</option>');
                        let carType = 'Any';

                        if (allDrives[i].carType == 1) {
                            carType = 'Car'
                        } else if (allDrives[i].carType == 2) {
                            carType = 'Van'
                        }

                        let comment = '';

                        if (allDrives[i].comments != null) {

                            let role = '';
                            if (allDrives[i].comments.createdBy.role == 0) {
                                role = 'Driver';
                            } else {
                                role = 'Customer';
                            }

                            comment = '<tr class="table-tr-drive">' +
                                '<td class="table-td-drive">Comment:</td>' +
                                '<td class="table-td-drive">' +
                                '<div>' +
                                '<p class="comment-bold">' + allDrives[i].comments.description.toString() + '</p>' +
                                '<p class="comment-italic">' + allDrives[i].comments.createdOn.toString() + '</p>' +
                                '<p class="comment-bold">GRADE: ' + allDrives[i].grade.toString() + '</p>' +
                                '<p class="comment-italic">' + allDrives[i].createdBy.name + ' ' + allDrives[i].createdBy.surname + ' - ' + role + '</p>' +
                                '</div>' +
                                '</td>' +
                                '</tr>';
                        }

                        let driver = '';

                        if (allDrives[i].drivedBy != null) {
                            driver = '<tr class="table-tr-drive">' +
                                '<td class="table-td-drive">Driver:</td>' +
                                '<td class="table-td-drive"><p class="home-tt-display2">' + allDrives[i].drivedBy.name + ' ' + allDrives[i].drivedBy.surname +'</p></td>' +
                                '</tr>'; 
                        }

                        let dispatcher = '';

                        if (allDrives[i].approvedBy != null) {
                            dispatcher = '<tr class="table-tr-drive">' +
                                '<td class="table-td-drive">Dispatcher:</td>' +
                                '<td class="table-td-drive"><p class="home-tt-display2">' + allDrives[i].approvedBy.name + ' ' + allDrives[i].approvedBy.surname +'</p></td>' +
                                '</tr>';
                        }

                        let customer = '';

                        if (allDrives[i].orderedBy != null) {
                            customer = '<tr class="table-tr-drive">' +
                                '<td class="table-td-drive">Dispatcher:</td>' +
                                '<td class="table-td-drive"><p class="home-tt-display2">' + allDrives[i].orderedBy.name + ' ' + allDrives[i].orderedBy.surname + '</p></td>' +
                                '</tr>';
                        }

                        let destination = '';

                        if (allDrives[i].destination.address.toString() != 'Unset') {
                            destination = '<tr class="table-tr-drive">' +
                                '<td class="table-td-drive">Destination:</td>' +
                                '<td class="table-td-drive"><p class="home-tt-display2">' + llDrives[i].destination.address.toString() +'</p></td>' +
                                '</tr>';
                        }

                        $('#displayTrips').append('<div class="home-tt-main">' +
                            '<p class="home-tt-display">' + allDrives[i].date.toString() + '</p>' +
                            '<p class="home-tt-display">' + allDrives[i].address.address.toString() + '</p>' +
                            '<button class="expand-table" onclick="showOtherInfo(this);" id="' + counter.toString() + '">+</button>' +
                            '</div>' +
                            '<div class="home-tt-other" style="display: none;" id="otherInfo' + counter.toString() + '">' +
                            '<div class="tableWrap">' +
                            '<table class="table-drive">' +
                            '<tr class="table-tr-drive">' +
                            '<td class="table-td-drive">Date:</td>' +
                            '<td class="table-td-drive"><p class="home-tt-display2">' + allDrives[i].date.toString() + '</p></td>' +
                            '</tr>' +
                            '<tr class="table-tr-drive">' +
                            '<td class="table-td-drive">Address:</td>' +
                            '<td class="table-td-drive"><p class="home-tt-display2">' + allDrives[i].address.address.toString() + '</p></td>' +
                            '</tr>' +
                            '<tr class="table-tr-drive">' +
                            '<td class="table-td-drive">Car Type:</td>' +
                            '<td class="table-td-drive"><p class="home-tt-display2">' + carType + '</p></td>' +
                            '</tr>'
                            + destination +
                            '<tr class="table-tr-drive">' +
                            '<td class="table-td-drive">Price:</td>' +
                            '<td class="table-td-drive"><p class="home-tt-display2">' + allDrives[i].price.toString() + ' RSD</p></td>' +
                            '</tr>'
                            + customer
                            + dispatcher
                             + driver
                            + comment +
                            '<tr class="table-tr-drive">' +
                            '<td colspan="2" class="table-td-drive" align="right">' +
                            '<button class="edit-drive" id="btnEditDrive" value="' + allDrives[i].driveId.toString() + '">Edit</button>' +
                            '<button class="edit-drive" id="btnQuitDrive" value="' + allDrives[i].driveId.toString() + '">Quit</button>' +
                            '</td>' +
                            '</tr>' +
                            '</table>' +
                            '</div>' +
                            '</div>');
                        counter++;
                    }
                },
                error: function (jqXHR) {
                    $('#errorLoginLbl').text('Wrong username or password!');
                }
            });
        }
    });
});