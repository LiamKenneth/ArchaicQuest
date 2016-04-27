$(function () {

    //================================================================================
    // Setup the auto-generated proxy for the hub.
    //================================================================================

    var chat = $.connection.mIMHub;
    var client = chat.client;
    var server = chat.server;

    //================================================================================
    // Helper Functions
    //================================================================================
    var MIM = {
        getRaceInfo: function (getValue) {
            server.pickRace(getValue);
        },
        selectRace: function () {
            $(".modal-body a").click(function () {

                if ($(this).hasClass("active")) {

                    //don't call server if same tab
                    return false;
                }
                else {
                    $(".modal-body a").removeClass("active");
                    $(this).addClass("active");

                    var getValue = $(this).text().trim().toLowerCase().toString();

                    MIM.getRaceInfo(getValue)
                }
            });
        },
        sendMessageToServer: function() {
            $('#sendmessage').click(function () {

                var message = $('#message');

                // Call the Send method on the hub.
                server.recieveFromClient(message.val());

                message.select().focus();
            });
        },
        htmlEncode: function (value) {
            var encodedValue = $('<div />').text(value).html();
            return encodedValue;
        }
    }


    //// Set focus to input box ////
    $('#message').focus();

    //================================================================================
    // Client Functions
    //================================================================================


    //// Add a new message to the page ////
    client.addNewMessageToPage = function (message) {
        $('#discussion').append("<p>" + MIM.htmlEncode(message) + "</p>");
    };

    //// Update Race Info ////
    client.updateRaceInfo = function (dataName, dataHelp, dataImg) {

        var raceInfo = "<h2>" + dataName + "</h2>" + "<p>" + dataHelp + "</p>";

        $('#raceInfo').html(raceInfo);

        $('#raceImg').attr('src', dataImg);
    }

    //// generate Stats ////
    client.setStats = function (stats) {
        return stats;
    }



    //================================================================================
    // Hub has loaded & Server functions
    //================================================================================
    $.connection.hub.start().done(function () {

        //// Load 1st race choice
         MIM.getRaceInfo("human"); //set default;

        //server.welcome();
        server.loadRoom();
   
        /// send info to server
        MIM.sendMessageToServer();

        //// select race tabs
            MIM.selectRace();

        });
        
    });

 
