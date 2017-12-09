$(function () {

    //================================================================================
    // Setup the auto-generated proxy for the hub.
    //================================================================================
    $.connection.hub.url = "/signalr";

    var chat = $.connection.mIMHub;
    var client = chat.client;
    var server = chat.server;

    var mapData = "no data bro";

    //================================================================================
    // Global Variables for playerCreation
    //================================================================================
    var race = "Human";
    var playerClass;
    var str;
    var dex;
    var con;
    var int;
    var wis;
    var cha;
    var gender;
    var name;
    var email;
    var password;

    var groomId;
    var gregion;
    var garea;
    var gzindex;

    var mainWindow = document.getElementById("discussion");
    var mainWindowFragment = document.createDocumentFragment();

    var channelWindow = document.getElementById("Channel");
    var channelWindowFragment = document.createDocumentFragment();
    //================================================================================
    // Helper Functions
    //================================================================================
    var MIM = {
        setDesc: function (desc) {
            console.log("setDesc ")
            var guid = $.connection.hub.id;
            server.updateDescription(desc, guid);
        },
        getRaceInfo: function (getValue) {
            server.characterSetupWizard(getValue, "race");
        },
        getClassInfo: function (getValue) {
            server.characterSetupWizard(getValue, "class");
        },
        selectOption: function () {
            $(".modal-body a").click(function () {

                if ($(this).hasClass("active")) {

                    //don't call server if same tab
                    return false;
                }
                else {
                    $(".modal-body a").removeClass("active");
                    $(this).addClass("active");

                    var getValue = $(this).text().trim().toLowerCase().toString();

                    if ($('#select-race').is(':visible')) {
                        MIM.getRaceInfo(getValue)
                    }
                    else if ($('#select-class').is(':visible')) {
                        MIM.getClassInfo(getValue)
                    }


                }
            });

        },
        CharacterNextStep: function () {
            var raceStep = document.getElementById('select-race');
            var classStep = document.getElementById('select-class');
            var statsStep = document.getElementById('select-stats');
            var infoStep = document.getElementById('select-char');
            var modelHeaderDiv = $(".modal-header div");
            var classBreadCrumb = $(".classBreadCrumb");
            var statsBreadCrumb = $(".statsBreadCrumb");

            $("#RaceSelectedBtn").click(function () {
                MIM.getClassInfo("fighter");
                raceStep.style.display = "none";
                classStep.style.display = "block";
                modelHeaderDiv.removeClass("active");
                classBreadCrumb.addClass("active");
            });

            $("#backToRace").click(function () {
                raceStep.style.display = "block";
                classStep.style.display = "none";
                modelHeaderDiv.removeClass("active");
                $(".raceBreadCrumb").addClass("active");
            });


            $("#selectedClassBtn").click(function () {
                server.getStats();
                classStep.style.display = "none";
                infoStep.style.display = "block";
                modelHeaderDiv.removeClass("active");
                $(".infoBreadCrumb").addClass("active");
            });

            $("#backToClass").click(function () {

                classStep.style.display = "block";
                infoStep.style.display = "none";
                modelHeaderDiv.removeClass("active");
                classBreadCrumb.addClass("active");
            });

            //$("#backToStats").click(function () {

            //    statsStep.style.display = "block";
            //    infoStep.style.display = "none";
            //    modelHeaderDiv.removeClass("active");
            //    statsBreadCrumb.addClass("active");
            //});

            //$("#reRollStats").click(function () {
            //    server.getStats();
            //});

            //$("#selectedStatsBtn").click(function () {
            //    // server.getStats();
            //    statsStep.style.display = "none";
            //    infoStep.style.display = "block";
            //    modelHeaderDiv.removeClass("active");
            //    $(".infoBreadCrumb").addClass("active");
            //});

            $("#createCharWizard").click(function () {
                document.getElementById('login').style.display = "none";
                raceStep.style.display = "block";
                $(".modal-header").show();
            });

            $("#goToLogin").click(function () {

                document.getElementById('login').style.display = "block";
                raceStep.style.display = "none";
                $(".modal-header").hide();
            });

            $("#CreateCharBtn").on("click", function () {


                name = document.getElementById('playerName').value.trim();
                gender = $("input[name=gender]:checked").val();
                email = document.getElementById('email').value.trim();

                var pass = document.getElementById('password').value;
                var confirmPass = document.getElementById('confirmPassword').value;

                if (pass == confirmPass && confirmPass != "") {
                    password = confirmPass;
                    $(this).prop('disabled', true);
                }
                else {
                    document.getElementById('passwordMatchError').style.display = "block";
                    document.getElementById('confirmPassword').className += " has-error";
                    return false;
                }



            });
        },
        sendMessageToServer: function () {
            $('#sendmessage').keypress(function (e) {
                var key = e.which;
                if (key == 13)  // the enter key code
                {
                    var message = $('#message');
                    var playerGuid = $.connection.hub.id;

                    // Call the Send method on the hub.
                    server.recieveFromClient(message.val(), playerGuid);

                    message.select().focus();
                    return false;
                }
            });

            $('#sendmessage').click(function () {

                var message = $('#message');

                if (message.length) {

                    var playerGuid = $.connection.hub.id;
                    server.recieveFromClient(message.val(), playerGuid);
                    message.select().focus();

                }
            });
        },
        htmlEncode: function (value) {
            var encodedValue = $('<div />').text(value).html();
            return encodedValue;
        },
        getMap: function (value) {
            console.log("this one...p")
            console.log(value)
            mapData = value;
        },
        createCharacter: function (char) {


            var name = char.Name.toLowerCase();
                name = name.charAt(0).toUpperCase() + name.slice(1);
            
         

            server.charSetup($.connection.hub.id, name, char.Email, char.Password, char.Gender, char.Race, char.Class);


            document.getElementById('signUpModal').style.display = "none";




        },
        login: function (char) {

            var name = char.Name.toLowerCase();
            name = name.charAt(0).toUpperCase() + name.slice(1);



            server.login($.connection.hub.id, name, char.password);

              document.getElementById('signUpModal').style.display = "none";



        },
        getGuid: function (guid) {
            var guid = $.cookie("playerGuid");
            return guid;
        },
        UI: {
            setWindowHeight: function () {
                console.log($("#info-char").height())
                var viewPort = $(window).height() - 150;
                $("#discussion").css({ "height": viewPort + 7, "max-height": viewPort + 7 });
                $("#info-inv").css({ "height": viewPort - $("#info-char").height(), "max-height": viewPort - $("#info-char").height()});   
                $("#info-quest").css({ "height": viewPort - $("#info-char").height(), "max-height": viewPort - $("#info-char").height()});   
            },
            openPanels: function () {


                var leftPanel = false;
                $("#open-left-panel").click(function () {

                    if (rightPanel == true) {
                        $("#right-panel").attr("style", "display: none!important;");
                        $("#main-panel").show();
                        rightPanel = false;
                    }
                    if (leftPanel === false) {
                        $("#left-panel").attr("style", "display: table!important;");
                        $("#main-panel").hide();
                        leftPanel = true;
                    }
                    else {
                        $("#left-panel").attr("style", "display: none!important;");
                        $("#main-panel").show();
                        leftPanel = false;
                    }

                });


                var rightPanel = false;
                $("#open-right-panel").click(function () {



                    if (leftPanel == true) {
                        $("#left-panel").attr("style", "display: none!important;");
                        $("#main-panel").show();
                        leftPanel = false;
                    }

                    if (rightPanel === false) {

                        //map
                       
                        $("#right-panel").attr("style", "display: table!important;");
                        $("#main-panel").hide();
                        rightPanel = true;
                        client.UpdateMap(groomId, garea, gregion, gzindex);
                    }
                    else {
                        $("#right-panel").attr("style", "display: none!important;");
                        $("#main-panel").show();
                        rightPanel = false;
                    }

                });


            }
        },
        addItem: function () {
            $("#js-AddItem").click(function () {
                alert("Handler for .click() called.");

                var data = {
                    "type": "object",
                    "location": "room",
                    "equipable": true,
                    "slot": "RightHand",
                    "name": "test Sword",
                    "actions": {
                        "container": false,
                        "wield": "wield"
                    },
                    "description": {
                        "look": "You look at a short sword",
                        "exam": "You look closely at a short sword",
                        "room": "A short Sword is here."
                    },
                    "stats": {
                        "damMin": 1,
                        "damMax": 5
                    }
                };

                $.post("/Room/Create/addItem", function (data) {
                    alert("success")
                });
            });
        },
        init: function () {
            console.log("INIT")
            //init when signalr is ready
            MIM.selectOption();
            MIM.CharacterNextStep();
            MIM.UI.setWindowHeight();
            MIM.UI.openPanels();
            MIM.addItem();

            var resizeTimer;

            $(window).on('resize', function (e) {

                clearTimeout(resizeTimer);
                resizeTimer = setTimeout(function () {

                    MIM.UI.setWindowHeight();

                }, 250);

            });

            $('[data-toggle="tooltip"]').tooltip();

            $(".quick-buttons a").click(function () {
                $('#message').val($(this).text() + " ");
                $('#message').focus();

                if ($(this).data("type") === "instant") {
                    $('#sendmessage').click();
                }
              
                


            });
 
            $('.quick-buttons').slick({
                dots: false,
                infinite: true,
                speed: 300,
                slidesToShow: 1,
                slidesToScroll: 2,
                centerMode: false,
                variableWidth: true,

            });


            $("#fontSize").on("change",
                function (event) {
 
                    $('head').append('<style>#discussion { font-size: ' + $(this).val() + 'px;');
                });

            $("#textSpacing").on("change",
                function (event) {

                    $('head').append('<style>#discussion p { margin-bottom: ' + $(this).val() + 'px;');
                });

           
        }
    }




    //// Set focus to input box ////
    $('#message').focus();

    //================================================================================
    // Client Functions
    //================================================================================


    client.quit = function () {

        $.connection.hub.stop();
    };

    client.getMap = function (map) {
        console.log("get me the map")
        mapData = map;
        client.UpdateMap();

    }
    //// Add a new message to the page ////
    client.addNewMessageToPage = function (message) {
        console.time('addtopage');
        var p = document.createElement('p');
        p.innerHTML = message;
        mainWindowFragment.appendChild(p);
        mainWindow.appendChild(mainWindowFragment);
        console.timeEnd('addtopage');
        $("#discussion").scrollTop($("#discussion")[0].scrollHeight);
    };

    //// Add a new message to the page ////
    client.updateScore = function (score) {
      
        var playerData = score;

 
        $('#player-name').html(score.Name);
        $('#player-level').html(score.Level);
        $('#player-race').html(playerData.Race);
        $('#player-class').html(playerData.SelectedClass);
        $('#player-gender').html(playerData.Gender);
        $('#player-alignment').html(playerData.AlignmentScore);

        $('#player-str').html(score.Strength);
        $('#player-max-str').html(score.MaxStrength);
        $('#player-dex').html(score.Dexterity);
        $('#player-max-dex').html(score.MaxDexterity);
        $('#player-con').html(score.Constitution);
        $('#player-max-con').html(score.MaxConstitution);
        $('#player-wis').html(score.Wisdom);
        $('#player-max-wis').html(score.MaxWisdom);
        $('#player-int').html(score.Intelligence);
        $('#player-max-int').html(score.MaxIntelligence);
        $('#player-cha').html(score.Charisma);
        $('#player-max-cha').html(score.MaxCharisma);

       
        $('#stat-HP').html(score.HitPoints); 
        $('#stat-max-HP').html(score.MaxHitPoints);

        $('#stat-mana').html(score.ManaPoints);      
        $('#stat-max-mana').html(score.MaxManaPoints);
     
        $('#stat-endurance').html(score.MovePoints); 
        $('#stat-max-endurance').html(score.MaxMovePoints);

        $('#stat-tnl').html(score.ExperienceToNextLevel);

        $('#player-hitroll').html(score.HitRoll);
        $('#player-damroll').html(score.DamRoll);
        $('#player-wimpy').html(score.Wimpy);

 
        $('#player-armorDef').html(score.ArmorRating);
        $('#player-damageRating').html(score.DamageRating);
        $('#player-magicDef').html(score.SpellResistance);


        $('#player-weight').html(score.Weight);
        $('#player-max-weight').html(score.MaxWeight);
        $('#player-status').html(score.Status);

        $('#player-hours').html(score.Hours);
        $('#player-experience').html(score.Experience);

        $('#player-copper').html(score.Copper);
        $('#player-silver').html(score.Silver);
        $('#player-gold').html(score.Gold);


    };

    client.updateStat = function (stat, maxStat, statType) {

        if (isNaN(stat) || isNaN(maxStat)) {
            return;
        }

        console.log(stat, maxStat, statType)
        var statPercentage = (stat / maxStat) * 100;
 

        if (statType == "hp") {
            document.getElementById('HP-bar').style.width = statPercentage + "%";
            document.getElementById('stat-HP').innerHTML = stat;
        }


        if (statType == "mana") {
            document.getElementById('mana-bar').style.width = statPercentage + "%";
            document.getElementById('stat-mana').innerHTML = stat;
        }

        if (statType == "endurance") {
            document.getElementById('end-bar').style.width = statPercentage + "%";
        }

        if (statType == "tnl") {

            statPercentage = (stat / maxStat) * 100;

            document.getElementById('tnl-bar').style.width = statPercentage + "%";
            $('#stat-tnl').html(maxStat - stat);
       
        }

    };


    client.updateExits = function (exits) {
        //reset
        $(".northNode").hide();
        $(".eastNode").hide();
        $(".southNode").hide();
        $(".westNode").hide();
       // console.log(exits)
        for (var i = 0; i < exits.length; i++) {
             console.log(exits[i])
            if (exits[i] == "North") {
                $(".northNode").show();
            }
            if (exits[i] == "East") {
                $(".eastNode").show();
            };

            if (exits[i] == "South") {
                $(".southNode").show();
            };

            if (exits[i] == "West") {
                $(".westNode").show();
            };
        }

    };

    client.UpdateUiChannels= function (channelText, channelClass) {
    
        var p = document.createElement('p');
        p.innerHTML = channelText;
        p.className  = channelClass;
        channelWindowFragment.appendChild(p);
        channelWindow.appendChild(channelWindowFragment);
    };


    client.UpdateUiMap = function (roomId, area, region, zindex) {


        groomId = roomId;
        garea = area;
        gregion = region;
        gzindex = zindex;

 
        client.UpdateMap(roomId, area, region, zindex);

    };


    client.UpdateUiDescription = function(ex) {

        $("#descriptionContainer").html(ex)
    }


    client.UpdateMap = function (roomId, area, region, zindex) {

        if (typeof roomId == 'undefined') {
            roomId = 0;
        }

        $("#roomTitleInfo").html("");

        var mapData = maps(area, region, "0");

        console.log(mapData)

  
        for (var i in mapData.nodes) {
            if (mapData.nodes[i].id == "node" + roomId) {
                mapData.nodes[i].color = "#2ECC71";

                const s = new sigma({
                    graph: mapData,
                    container: 'roomTitleInfo',
                    settings: {
                        defaultNodeColor: '#ccc',
                        labelThreshold: 5000,
                        skipErrors: true
                    },
                    type: 'square'
                });

                s.cameras[0].goTo({
                    x: parseInt(mapData.nodes[i].x),
                    y: parseInt(mapData.nodes[i].y),
                    angle: 0,
                    ratio: 1
                });
                s.refresh();

                break; //Stop this loop, we found it!
            }
        }
    }

    client.updateInventory = function (inventory) {
        var inventoryCount = inventory.length;


        $(".js-info").animate({ scrollTop: 0 }, "fast");


        if (inventory == 0) {
            $("#invList").html("You are not carrying anything");
            return;
        }
        $("#invList").empty();
        for (var i = 0; i < inventoryCount; i++) {
            $("#invList").append("<li>" + inventory[i] + "</li>");
        }

    };

    client.updateEquipment= function (eq) {

     
        $("#eqList").empty();
        
         $("#eqList").append(eq);
        
    

    };

    client.updateQuestLog = function (qlog) {
    

        $("#qlList").empty();

        $("#qlList").append(qlog);

      
    };

    client.updateAffects= function (affects) {
     

        $("#afList").empty();

        $("#afList").append(affects);

       

    };

    //// Update Race Info ////
    client.updateCharacterSetupWizard = function (step, dataName, dataHelp, dataImg) {

        var info = "<h2>" + dataName + "</h2>" + "<p>" + dataHelp + "</p>";

        if (step === "race") {
            race = dataName; //global Race

            $('.raceInfo').html(info);

            $('.raceImg').attr('src', dataImg);

            $('#Race').val(dataName);
        }
        else if (step === "class") {
            playerClass = dataName; //global player Class
            $('.classInfo').html(info);

            $('.classImg').attr('src', dataImg);

            $('#Class').val(dataName);
        }


        console.log(dataName + " " + dataHelp)



    }

    //// generate Stats ////
    client.setStats = function (stats) {

        str = stats[0];
        dex = stats[1];
        con = stats[2];
        int = stats[3];
        wis = stats[4];
        cha = stats[5];

        $('#statStr').html(str);
        $('#statDex').html(dex);
        $('#statCon').html(con);
        $('#statInt').html(int);
        $('#statWis').html(wis);
        $('#statCha').html(cha);

        $("#Strength").val(str);
        $("#Dexterity").val(dex);
        $("#Constitution").val(con);
        $("#Intelligence").val(int);
        $("#Wisdom").val(wis);
        $("#Charisma").val(cha);

    }

    client.characterNameLoginError = function (errors) {
        var charlogin = $('#charName');


        charlogin.addClass('error');

        $("#charName .errorMsg").html(errors);

        if (errors == "") {
            charlogin.removeClass('error');
        }



    }

    client.characterPasswordError = function (errors) {

        var charpass = $('#charPass');


        charpass.addClass('error');

        $("#charPass .errorMsg").html(errors);

        if (errors == "") {
            charpass.removeClass('error');
        }

    }

    client.ui = function (errors) {



    }



    client.checkCharExists = function (charStatus, caller) {

        console.log("this validates " + charStatus)

        if (!charStatus && caller === 'login') {

            var charlogin = $('#charName');

            charlogin.addClass('error');

            $("#charName .errorMsg").html("This character does not exist.");

            return false;
        }

        return true;
    }

    client.savePlayerGuid = function (guid) {
        $.cookie("playerGuid", guid);
    }



    //================================================================================
    // Hub has loaded & Server functions
    //================================================================================
    $.connection.hub.start().done(function () {

        //// Load 1st race choice
        MIM.getRaceInfo("human"); //set default;

        /// send info to server
        MIM.sendMessageToServer();

        //// Start scripts
        MIM.init();


        $(".channelFilter").click(function () {

            var val = $(this).html();
            $(".channelFilter").removeClass("activeUiLink");
            $(this).addClass("activeUiLink");
            if (val === "Newbie") {
                $(".newbieChannelF").show();
                $(".gossipChannelF").hide();
                $(".oocChannelF").hide();
                $(".roomChannelF").hide();
            } else if (val === "Gossip") {
                $(".newbieChannelF").hide();
                $(".gossipChannelF").show();
                $(".oocChannelF").hide();
                $(".roomChannelF").hide();
            } else if (val === "Room") {
                $(".newbieChannelF").hide();
                $(".gossipChannelF").hide();
                $(".oocChannelF").hide();
                $(".roomChannelF").show();
            } else if (val === "OOC") {
                $(".newbieChannelF").hide();
                $(".gossipChannelF").hide();
                $(".oocChannelF").show();
                $(".roomChannelF").hide();
            } else {
                $(".newbieChannelF").show();
                $(".gossipChannelF").show();
                $(".oocChannelF").show();
                $(".roomChannelF").show();
            }


        });

        $(".infoFilter").click(function () {

            console.log("click");
            var val = $(this).html();
            $(".infoFilter").removeClass("activeUiLink");
            $(this).addClass("activeUiLink");
            $(".js-info").animate({ scrollTop: 0 }, "fast"); 
            if (val === "Inv") {
                
                $("#invList").show();
                $("#eqList").hide();
                $("#afList").hide();
                $("#qlList").hide();
            } else if (val === "EQ") {
                $("#invList").hide();
               
                $("#eqList").show();
                $("#afList").hide();
                $("#qlList").hide();
            } else if (val === "Aff") {
                $("#invList").hide();
                $("#eqList").hide();
                
                $("#afList").show();
                $("#qlList").hide();
            }   else {
                $("#invList").hide();
                $("#eqList").hide();
                $("#afList").hide();
               
                $("#qlList").show();
            }


        });

        $(".infoPlayerFilter").click(function () {

            console.log("click");
            var val = $(this).html();
            $(".infoPlayerFilter").removeClass("activeUiLink");
            $(this).addClass("activeUiLink");
            if (val === "Score") {
                $(".js-score").show();
                $(".js-description").hide();
                
            } else {
                $(".js-score").hide();
                $(".js-description").show();
           
            }


        });

        

        $("#SaveDescription").click(function () {

  
            var desc = $("#descriptionText").val();

            MIM.setDesc(desc);
        });


        $(".js-generateName").click(function () {

            var nameField = $('#createCharaterForm #Name');
 
            $.ajax({
                    type: "get",
                    url: "/dev/Home/Generatename"
                })
                .done(function(data) {

                    nameField.val(data);

                });
           
        });

        var submitted = 0;

        $("#createCharaterForm").on("submit", function (event) {
            
            if (submitted == 0) {
          
                var valid = $('#createCharaterForm').validate().form();

                if (!valid) {
                    return;
                }

                var $this = $(this);
                var frmValues = $this.serialize();
                $.ajax({
                        type: $this.attr('method'),
                        url: $this.attr('action'),
                        data: frmValues
                    })
                    .done(function(data) {

                        MIM.createCharacter(data);

                    })
                    .fail(function() {
                        alert("failed");
                    });
            }

            submitted++;
            event.preventDefault();

        });
        var submittedLogin = 0;
        $("#loginForm").on("submit", function (event) {

            if (submittedLogin == 0) {

                var valid = $('#loginForm').validate().form();

                if (!valid) {
                    return;
                }

                var $this = $(this);
                var frmValues = $this.serialize();
                $.ajax({
                        type: $this.attr('method'),
                        url: $this.attr('action'),
                        data: frmValues
                    })
                    .done(function(data) {

                        MIM.login(data);


                    })
                    .fail(function() {
                        alert("failed");
                    });

                submittedLogin++;

            }
            event.preventDefault();
        });

        $.validator.unobtrusive.parse("#loginForm");

        $.validator.unobtrusive.parse("#createCharaterForm");

        //http://stackoverflow.com/questions/10163683/multiple-fields-validation-using-remote-validation
        function initializeRemotelyValidatingElementsWithAdditionalFields($form) {
            var remotelyValidatingElements = $form.find("[data-val-remote]");

            $.each(remotelyValidatingElements, function (i, element) {
                var $element = $(element);

                var additionalFields = $element.attr("data-val-remote-additionalfields");

                if (additionalFields.length == 0) return;

                var rawFieldNames = additionalFields.split(",");

                var fieldNames = $.map(rawFieldNames, function (fieldName) { return fieldName.replace("*.", ""); });

                $.each(fieldNames, function (i, fieldName) {
                    $form.find("#" + fieldName).change(function () {
                        // force re-validation to occur
                        $element.removeData("previousValue");
                        $element.valid();
                    });
                });
            });
        }

        initializeRemotelyValidatingElementsWithAdditionalFields($("#loginForm"));


        $('#discussion').perfectScrollbar();
        $('.js-score').perfectScrollbar();
        $('.js-room').perfectScrollbar();
        $('.js-channels').perfectScrollbar();
        $('.js-info').perfectScrollbar();
        $('.js-description').perfectScrollbar();
        

      
        var resizeTimer;

        $(window).on('resize', function (e) {

            clearTimeout(resizeTimer);
            resizeTimer = setTimeout(function () {

                $('#discussion').perfectScrollbar('update');
                $('.js-score').perfectScrollbar('update');
                $('.js-room').perfectScrollbar('update');
                $('.js-channels').perfectScrollbar('update');
                $('.js-info').perfectScrollbar('update');
                $('.js-description').perfectScrollbar('update');
            }, 250);

        });
    });


    var _player = document.getElementById("player");

     function playAudio() {

         _player.src = "https://t4.bcbits.com/stream/4c0348d5007801f13639b0b521017188/mp3-128/2360130612?p=0&ts=1509145882&t=a1dbd6a390427c9216c0d3a506db2454dda4cd8d&token=1509145882_f8a6264d146bd22e61efd9b44577c0fe91da9e30";

         _player.volume = 0.2;
            _player.play();

         
    };

    //if ($(window).width() > 768) {
    //    playAudio();
    //}

 

    var isPlaying = false;
    var music = $("#music");

    function togglePlay() {
        if (isPlaying) {

            if (_player.volume == 0.2) {
                _player.volume = 0.02;
                music.css("opacity", "0.5");
            } else {
                _player.pause();
            }
          
        } else {
            _player.volume = 0.2;

            _player.play();
        }
    };
    _player.onplaying = function () {
        isPlaying = true;
       music.css("opacity", "1");
    };
    _player.onpause = function () {
        isPlaying = false;
        music.css("opacity", "0.2");
    };

    var i = 0;
    var playlist = [

        "https://t4.bcbits.com/stream/c69445e10427e06b5147b38135377ba7/mp3-128/292646295?p=0&ts=1509146281&t=ef67230a54182e308196fd076b93212e6989532c&token=1509146281_b6011744182063e055f23c1165b3d6f5f51753dc",

        "https://t4.bcbits.com/stream/979024ecf6069cdaa0eea36bc5dce0bf/mp3-128/1314921283?p=0&ts=1509146170&t=466893c1361dcf3d44755871189bd7c10b9f611e&token=1509146170_ac0781e61b7e2195b134c553868ae7ac2d7e359c",

        "https://t4.bcbits.com/stream/a4974bc23a6612474cb8571d283b3435/mp3-128/2667258246?p=0&ts=1509146086&t=7441ce1b6afa217719e17576a7a224f76f2b86bf&token=1509146086_d49e2b5c40146ffcf9565d8e38cd83656beaf524",

        'https://t4.bcbits.com/stream/bebf264abf69f8ef271ffcad17f819a6/mp3-128/2142516999?p=0&ts=1509146000&t=7a1b065b6d04da80a46323bac5bc317ff0360867&token=1509146000_3e58321b1c65a42a3b66c63f7e288c1f1b382d0a',

        'https://t4.bcbits.com/stream/d69b144982422622205741c4b5db656b/mp3-128/3105472715?p=0&ts=1509145759&t=1875dbf265efe883fd9cea08eb2a4dae6311e3b2&token=1509145759_533f848a6476216b94a849f5438c96556923a5a7',

        "https://t4.bcbits.com/stream/1659b135351aa4b1dd359e7ca6d98be4/mp3-128/1805184470?p=0&ts=1509145759&t=ab13c07d6ac48ed68fcf68ccc8808ef881c075f6&token=1509145759_abd72c2f8a0ab6ede62e6484a95f073ab4ba03f2"];


    _player.addEventListener('ended', function () {
        i = ++i < playlist.length ? i : 0;
        
        _player.src = playlist[i];
        _player.play();
    }, true);

    music.on("click",
        function (event) {

            console.log("click")
            togglePlay(this);
        });



    function maps(area, region, zindex) {
 

        var key = area + region + zindex;
    
        console.log(key)
      
      var  map = {
           "tutorialtutorial0" : {
                "nodes": [
                    {
                        "id": "node1",
                        "label": "Deep in the forest",
                        "x": "0",
                        "y": "-1",
                        "size": 2,
                        "defaultLabelSize": 12
                    },
                    {
                        "id": "node0",
                        "label": "Deep in the forest",
                        "x": "0",
                        "y": "0",
                        "size": 2,
                        "defaultLabelSize": 12
                    },
                    { "id": "node3", "label": "A goblin camp", "x": "1", "y": "-2", "size": 2, "defaultLabelSize": 12 },
                    {
                        "id": "node2",
                        "label": "Deep in the forest",
                        "x": "0",
                        "y": "-2",
                        "size": 2,
                        "defaultLabelSize": 12
                    },
                    {
                        "id": "node5",
                        "label": "A tent in the goblin camp",
                        "x": "1",
                        "y": "-1",
                        "size": 2,
                        "defaultLabelSize": 12
                    },
                    {
                        "id": "node4",
                        "label": "A tent in the goblin camp",
                        "x": "1",
                        "y": "-3",
                        "size": 2,
                        "defaultLabelSize": 12
                    },
                    {
                        "id": "node7",
                        "label": "Deep in the forest",
                        "x": "3",
                        "y": "-2",
                        "size": 2,
                        "defaultLabelSize": 12
                    },
                    {
                        "id": "node6",
                        "label": "Deep in the forest",
                        "x": "2",
                        "y": "-2",
                        "size": 2,
                        "defaultLabelSize": 12
                    },
                    {
                        "id": "node10",
                        "label": "Deep in the forest",
                        "x": "3",
                        "y": "-4",
                        "size": 2,
                        "defaultLabelSize": 12
                    },
                    {
                        "id": "node9",
                        "label": "Deep in the forest",
                        "x": "3",
                        "y": "-3",
                        "size": 2,
                        "defaultLabelSize": 12
                    },
                    {
                        "id": "node8",
                        "label": "A natural spring",
                        "x": "3",
                        "y": "-1",
                        "size": 2,
                        "defaultLabelSize": 12
                    }
                ],
                "edges": [
                    { "id": "edge12", "source": "node1", "target": "node2", "color": "#ccc" },
                    { "id": "edge01", "source": "node0", "target": "node1", "color": "#ccc" },
                    { "id": "edge34", "source": "node3", "target": "node4", "color": "#ccc" },
                    { "id": "edge36", "source": "node3", "target": "node6", "color": "#ccc" },
                    { "id": "edge35", "source": "node3", "target": "node5", "color": "#ccc" },
                    { "id": "edge32", "source": "node3", "target": "node2", "color": "#ccc" },
                    { "id": "edge23", "source": "node2", "target": "node3", "color": "#ccc" },
                    { "id": "edge21", "source": "node2", "target": "node1", "color": "#ccc" },
                    { "id": "edge53", "source": "node5", "target": "node3", "color": "#ccc" },
                    { "id": "edge43", "source": "node4", "target": "node3", "color": "#ccc" },
                    { "id": "edge79", "source": "node7", "target": "node9", "color": "#ccc" },
                    { "id": "edge78", "source": "node7", "target": "node8", "color": "#ccc" },
                    { "id": "edge76", "source": "node7", "target": "node6", "color": "#ccc" },
                    { "id": "edge67", "source": "node6", "target": "node7", "color": "#ccc" },
                    { "id": "edge63", "source": "node6", "target": "node3", "color": "#ccc" },
                    { "id": "edge910", "source": "node9", "target": "node10", "color": "#ccc" },
                    { "id": "edge97", "source": "node9", "target": "node7", "color": "#ccc" },
                    { "id": "edge87", "source": "node8", "target": "node7", "color": "#ccc" }
                ]
          },
           "anker farmanker0": { "nodes": [{ "id": "node1", "label": "Road through farm", "x": "1", "y": "0", "size": 2, "defaultLabelSize": 12, "color": "#ccc" }, { "id": "node0", "label": "Anker road, east of the Anker gate", "x": "0", "y": "0", "size": 2, "defaultLabelSize": 12, "color": "#ccc" }, { "id": "node8", "label": "The windmill", "x": "2", "y": "-1", "size": 2, "defaultLabelSize": 12, "color": "#ccc" }, { "id": "node10", "label": "Anker Road", "x": "3", "y": "0", "size": 2, "defaultLabelSize": 12, "color": "#ccc" }, { "id": "node9", "label": "The riverbank", "x": "2", "y": "1", "size": 2, "defaultLabelSize": 12, "color": "#ccc" }, { "id": "node7", "label": "Anker Road", "x": "2", "y": "0", "size": 2, "defaultLabelSize": 12, "color": "#ccc" }, { "id": "node4", "label": "Crop Fields", "x": "1", "y": "2", "size": 2, "defaultLabelSize": 12, "color": "#ccc" }, { "id": "node6", "label": "Crop Fields", "x": "0", "y": "1", "size": 2, "defaultLabelSize": 12, "color": "#ccc" }, { "id": "node3", "label": "Crop Fields", "x": "1", "y": "1", "size": 2, "defaultLabelSize": 12, "color": "#ccc" }, { "id": "node2", "label": "Farmer O'Neil's House", "x": "1", "y": "-1", "size": 2, "defaultLabelSize": 12, "color": "#ccc" }, { "id": "node16", "label": "The palus river", "x": "3", "y": "2", "size": 2, "defaultLabelSize": 12, "color": "#446CB3" }, { "id": "node15", "label": "The palus river", "x": "2", "y": "2", "size": 2, "defaultLabelSize": 12, "color": "#446CB3" }, { "id": "node11", "label": "Pasture", "x": "3", "y": "-1", "size": 2, "defaultLabelSize": 12, "color": "#ccc" }, { "id": "node12", "label": "Saw mill", "x": "3", "y": "1", "size": 2, "defaultLabelSize": 12, "color": "#ccc" }, { "id": "node14", "label": "The bridge leading into the goblin forest", "x": "5", "y": "0", "size": 2, "defaultLabelSize": 12, "color": "#ccc" }, { "id": "node13", "label": "Anker Road", "x": "4", "y": "0", "size": 2, "defaultLabelSize": 12, "color": "#ccc" }, { "id": "node5", "label": "Crop Fields", "x": "0", "y": "2", "size": 2, "defaultLabelSize": 12, "color": "#ccc" }, { "id": "node19", "label": "The palus river", "x": "4", "y": "1", "size": 2, "defaultLabelSize": 12, "color": "#ccc" }, { "id": "node18", "label": "The palus river", "x": "5", "y": "2", "size": 2, "defaultLabelSize": 12, "color": "#446CB3" }, { "id": "node17", "label": "The palus river", "x": "4", "y": "2", "size": 2, "defaultLabelSize": 12, "color": "#446CB3" }, { "id": "node20", "label": "The palus river", "x": "5", "y": "1", "size": 2, "defaultLabelSize": 12, "color": "#446CB3" }], "edges": [{ "id": "edge12", "source": "node1", "target": "node2", "color": "#ccc" }, { "id": "edge13", "source": "node1", "target": "node3", "color": "#ccc" }, { "id": "edge17", "source": "node1", "target": "node7", "color": "#ccc" }, { "id": "edge10", "source": "node1", "target": "node0", "color": "#ccc" }, { "id": "edge01", "source": "node0", "target": "node1", "color": "#ccc" }, { "id": "edge87", "source": "node8", "target": "node7", "color": "#ccc" }, { "id": "edge1011", "source": "node10", "target": "node11", "color": "#ccc" }, { "id": "edge1013", "source": "node10", "target": "node13", "color": "#ccc" }, { "id": "edge1012", "source": "node10", "target": "node12", "color": "#ccc" }, { "id": "edge107", "source": "node10", "target": "node7", "color": "#ccc" }, { "id": "edge97", "source": "node9", "target": "node7", "color": "#ccc" }, { "id": "edge915", "source": "node9", "target": "node15", "color": "#ccc" }, { "id": "edge78", "source": "node7", "target": "node8", "color": "#ccc" }, { "id": "edge710", "source": "node7", "target": "node10", "color": "#ccc" }, { "id": "edge79", "source": "node7", "target": "node9", "color": "#ccc" }, { "id": "edge71", "source": "node7", "target": "node1", "color": "#ccc" }, { "id": "edge43", "source": "node4", "target": "node3", "color": "#ccc" }, { "id": "edge45", "source": "node4", "target": "node5", "color": "#ccc" }, { "id": "edge63", "source": "node6", "target": "node3", "color": "#ccc" }, { "id": "edge65", "source": "node6", "target": "node5", "color": "#ccc" }, { "id": "edge31", "source": "node3", "target": "node1", "color": "#ccc" }, { "id": "edge34", "source": "node3", "target": "node4", "color": "#ccc" }, { "id": "edge36", "source": "node3", "target": "node6", "color": "#ccc" }, { "id": "edge21", "source": "node2", "target": "node1", "color": "#ccc" }, { "id": "edge1617", "source": "node16", "target": "node17", "color": "#ccc" }, { "id": "edge1615", "source": "node16", "target": "node15", "color": "#ccc" }, { "id": "edge159", "source": "node15", "target": "node9", "color": "#ccc" }, { "id": "edge1516", "source": "node15", "target": "node16", "color": "#ccc" }, { "id": "edge1110", "source": "node11", "target": "node10", "color": "#ccc" }, { "id": "edge1210", "source": "node12", "target": "node10", "color": "#ccc" }, { "id": "edge1413", "source": "node14", "target": "node13", "color": "#ccc" }, { "id": "edge1314", "source": "node13", "target": "node14", "color": "#ccc" }, { "id": "edge1310", "source": "node13", "target": "node10", "color": "#ccc" }, { "id": "edge56", "source": "node5", "target": "node6", "color": "#ccc" }, { "id": "edge54", "source": "node5", "target": "node4", "color": "#ccc" }, { "id": "edge1920", "source": "node19", "target": "node20", "color": "#ccc" }, { "id": "edge1917", "source": "node19", "target": "node17", "color": "#ccc" }, { "id": "edge1820", "source": "node18", "target": "node20", "color": "#ccc" }, { "id": "edge1817", "source": "node18", "target": "node17", "color": "#ccc" }, { "id": "edge1719", "source": "node17", "target": "node19", "color": "#ccc" }, { "id": "edge1718", "source": "node17", "target": "node18", "color": "#ccc" }, { "id": "edge1716", "source": "node17", "target": "node16", "color": "#ccc" }, { "id": "edge2018", "source": "node20", "target": "node18", "color": "#ccc" }, { "id": "edge2019", "source": "node20", "target": "node19", "color": "#ccc" }] },
           "ankeranker0": {
               "nodes": [{ "id": "node1", "label": "Square walk, outside the Red Lion", "x": "0", "y": "-1", "size": 2, "defaultLabelSize": 12, "color": "#ccc" }, { "id": "node8", "label": "Square walk, east of the centre", "x": "1", "y": "0", "size": 2, "defaultLabelSize": 12, "color": "#ccc" }, { "id": "node6", "label": "Square walk, south of the centre", "x": "0", "y": "1", "size": 2, "defaultLabelSize": 12, "color": "#ccc" }, { "id": "node4", "label": "Square walk, west of the centre", "x": "-1", "y": "0", "size": 2, "defaultLabelSize": 12, "color": "#ccc" }, { "id": "node0", "label": "Village Square", "x": "0", "y": "0", "size": 2, "defaultLabelSize": 12, "color": "#ccc" }, { "id": "node11", "label": "The Village Hall", "x": "3", "y": "0", "size": 2, "defaultLabelSize": 12, "color": "#ccc" }, { "id": "node10", "label": "Village Hall Entrance", "x": "2", "y": "0", "size": 2, "defaultLabelSize": 12, "color": "#ccc" }, { "id": "node14", "label": "Stables of The Red Lion", "x": "-1", "y": "-2", "size": 2, "defaultLabelSize": 12, "color": "#ccc" }, { "id": "node2", "label": "The Red Lion", "x": "0", "y": "-2", "size": 2, "defaultLabelSize": 12, "color": "#ccc" }, { "id": "node5", "label": "Square walk, south west of the centre", "x": "-1", "y": "1", "size": 2, "defaultLabelSize": 12, "color": "#ccc" }, { "id": "node15", "label": "Temple Walk", "x": "2", "y": "1", "size": 2, "defaultLabelSize": 12, "color": "#ccc" }, { "id": "node19", "label": "A path to the square", "x": "1", "y": "2", "size": 2, "defaultLabelSize": 12, "color": "#ccc" }, { "id": "node7", "label": "Square walk, Entrance", "x": "1", "y": "1", "size": 2, "defaultLabelSize": 12, "color": "#ccc" }, { "id": "node12", "label": "Odds and sods shoppe", "x": "1", "y": "-2", "size": 2, "defaultLabelSize": 12, "color": "#fed967" }, { "id": "node13", "label": "Metal Medley", "x": "2", "y": "-1", "size": 2, "defaultLabelSize": 12, "color": "#fed967" }, { "id": "node9", "label": "Square walk, commerce corner", "x": "1", "y": "-1", "size": 2, "defaultLabelSize": 12, "color": "#ccc" }, { "id": "node3", "label": "Square walk, outside the stables of the Red Lion", "x": "-1", "y": "-1", "size": 2, "defaultLabelSize": 12, "color": "#ccc" }, { "id": "node22", "label": "Anker Lane", "x": "2", "y": "3", "size": 2, "defaultLabelSize": 12, "color": "#ccc" }, { "id": "node21", "label": "Anker Lane", "x": "0", "y": "3", "size": 2, "defaultLabelSize": 12, "color": "#ccc" }, { "id": "node20", "label": "Middle of Anker Lane", "x": "1", "y": "3", "size": 2, "defaultLabelSize": 12, "color": "#ccc" }, { "id": "node16", "label": "Temple Walk", "x": "4", "y": "1", "size": 2, "defaultLabelSize": 12, "color": "#ccc" }, { "id": "node42", "label": "Temple Walk", "x": "3", "y": "1", "size": 2, "defaultLabelSize": 12, "color": "#ccc" }, { "id": "node18", "label": "The Village Hall, Elder Chamber", "x": "3", "y": "-1", "size": 2, "defaultLabelSize": 12, "color": "#ccc" }, { "id": "node32", "label": "A small cosy home", "x": "2", "y": "4", "size": 2, "defaultLabelSize": 12, "color": "#ccc" }, { "id": "node31", "label": "An Alchemists home", "x": "2", "y": "2", "size": 2, "defaultLabelSize": 12, "color": "#ccc" }, { "id": "node29", "label": "A small cosy home", "x": "0", "y": "4", "size": 2, "defaultLabelSize": 12, "color": "#ccc" }, { "id": "node28", "label": "Niamh's small cosy home", "x": "0", "y": "2", "size": 2, "defaultLabelSize": 12, "color": "#ccc" }, { "id": "node33", "label": "A small cosy home", "x": "3", "y": "2", "size": 2, "defaultLabelSize": 12, "color": "#ccc" }, { "id": "node24", "label": "Anker Lane", "x": "4", "y": "3", "size": 2, "defaultLabelSize": 12, "color": "#ccc" }, { "id": "node34", "label": "A small cosy home", "x": "3", "y": "4", "size": 2, "defaultLabelSize": 12, "color": "#ccc" }, { "id": "node23", "label": "Anker Lane", "x": "3", "y": "3", "size": 2, "defaultLabelSize": 12, "color": "#ccc" }, { "id": "node26", "label": "A small cosy home", "x": "-1", "y": "2", "size": 2, "defaultLabelSize": 12, "color": "#ccc" }, { "id": "node27", "label": "A small cosy home", "x": "-1", "y": "4", "size": 2, "defaultLabelSize": 12, "color": "#ccc" }, { "id": "node37", "label": "Anker Lane", "x": "-2", "y": "3", "size": 2, "defaultLabelSize": 12, "color": "#ccc" }, { "id": "node25", "label": "Anker Lane", "x": "-1", "y": "3", "size": 2, "defaultLabelSize": 12, "color": "#ccc" }, { "id": "node17", "label": "Temple Entrance", "x": "5", "y": "0", "size": 2, "defaultLabelSize": 12, "color": "#ccc" }, { "id": "node43", "label": "Temple Walk", "x": "5", "y": "1", "size": 2, "defaultLabelSize": 12, "color": "#ccc" }, { "id": "node65", "label": "Temple of Tyr", "x": "5", "y": "-1", "size": 2, "defaultLabelSize": 12, "color": "#ccc" }, { "id": "node41", "label": "Arched gate of Anker", "x": "6", "y": "3", "size": 2, "defaultLabelSize": 12, "color": "#ccc" }, { "id": "node40", "label": "Anker Lane west of the gate", "x": "5", "y": "3", "size": 2, "defaultLabelSize": 12, "color": "#ccc" }, { "id": "node36", "label": "A small cosy home", "x": "4", "y": "4", "size": 2, "defaultLabelSize": 12, "color": "#ccc" }, { "id": "node35", "label": "A small cosy home", "x": "4", "y": "2", "size": 2, "defaultLabelSize": 12, "color": "#ccc" }, { "id": "node38", "label": "A small cosy home", "x": "-2", "y": "2", "size": 2, "defaultLabelSize": 12, "color": "#ccc" }, { "id": "node39", "label": "A small cosy home", "x": "-2", "y": "4", "size": 2, "defaultLabelSize": 12, "color": "#ccc" }, { "id": "node45", "label": "Temple Graveyard", "x": "4", "y": "-1", "size": 2, "defaultLabelSize": 12, "color": "#ccc" }, { "id": "node44", "label": "Temple Graveyard", "x": "4", "y": "0", "size": 2, "defaultLabelSize": 12, "color": "#ccc" }, { "id": "node63", "label": "Temple Graveyard", "x": "5", "y": "-2", "size": 2, "defaultLabelSize": 12, "color": "#ccc" }, { "id": "node62", "label": "Temple Graveyard", "x": "4", "y": "-2", "size": 2, "defaultLabelSize": 12, "color": "#ccc" }, { "id": "node46", "label": "Temple Graveyard", "x": "6", "y": "-1", "size": 2, "defaultLabelSize": 12, "color": "#ccc" }, { "id": "node64", "label": "Temple Graveyard", "x": "6", "y": "-2", "size": 2, "defaultLabelSize": 12, "color": "#ccc" }, { "id": "node47", "label": "Temple Graveyard", "x": "6", "y": "0", "size": 2, "defaultLabelSize": 12, "color": "#ccc" }], "edges": [{ "id": "edge12", "source": "node1", "target": "node2", "color": "#ccc" }, { "id": "edge19", "source": "node1", "target": "node9", "color": "#ccc" }, { "id": "edge10", "source": "node1", "target": "node0", "color": "#ccc" }, { "id": "edge13", "source": "node1", "target": "node3", "color": "#ccc" }, { "id": "edge89", "source": "node8", "target": "node9", "color": "#ccc" }, { "id": "edge810", "source": "node8", "target": "node10", "color": "#ccc" }, { "id": "edge80", "source": "node8", "target": "node0", "color": "#ccc" }, { "id": "edge87", "source": "node8", "target": "node7", "color": "#ccc" }, { "id": "edge60", "source": "node6", "target": "node0", "color": "#ccc" }, { "id": "edge67", "source": "node6", "target": "node7", "color": "#ccc" }, { "id": "edge65", "source": "node6", "target": "node5", "color": "#ccc" }, { "id": "edge43", "source": "node4", "target": "node3", "color": "#ccc" }, { "id": "edge40", "source": "node4", "target": "node0", "color": "#ccc" }, { "id": "edge45", "source": "node4", "target": "node5", "color": "#ccc" }, { "id": "edge01", "source": "node0", "target": "node1", "color": "#ccc" }, { "id": "edge08", "source": "node0", "target": "node8", "color": "#ccc" }, { "id": "edge06", "source": "node0", "target": "node6", "color": "#ccc" }, { "id": "edge04", "source": "node0", "target": "node4", "color": "#ccc" }, { "id": "edge1118", "source": "node11", "target": "node18", "color": "#ccc" }, { "id": "edge1110", "source": "node11", "target": "node10", "color": "#ccc" }, { "id": "edge1011", "source": "node10", "target": "node11", "color": "#ccc" }, { "id": "edge108", "source": "node10", "target": "node8", "color": "#ccc" }, { "id": "edge142", "source": "node14", "target": "node2", "color": "#ccc" }, { "id": "edge143", "source": "node14", "target": "node3", "color": "#ccc" }, { "id": "edge214", "source": "node2", "target": "node14", "color": "#ccc" }, { "id": "edge21", "source": "node2", "target": "node1", "color": "#ccc" }, { "id": "edge54", "source": "node5", "target": "node4", "color": "#ccc" }, { "id": "edge56", "source": "node5", "target": "node6", "color": "#ccc" }, { "id": "edge1542", "source": "node15", "target": "node42", "color": "#ccc" }, { "id": "edge157", "source": "node15", "target": "node7", "color": "#ccc" }, { "id": "edge197", "source": "node19", "target": "node7", "color": "#ccc" }, { "id": "edge1920", "source": "node19", "target": "node20", "color": "#ccc" }, { "id": "edge78", "source": "node7", "target": "node8", "color": "#ccc" }, { "id": "edge715", "source": "node7", "target": "node15", "color": "#ccc" }, { "id": "edge719", "source": "node7", "target": "node19", "color": "#ccc" }, { "id": "edge76", "source": "node7", "target": "node6", "color": "#ccc" }, { "id": "edge129", "source": "node12", "target": "node9", "color": "#ccc" }, { "id": "edge139", "source": "node13", "target": "node9", "color": "#ccc" }, { "id": "edge912", "source": "node9", "target": "node12", "color": "#ccc" }, { "id": "edge913", "source": "node9", "target": "node13", "color": "#ccc" }, { "id": "edge98", "source": "node9", "target": "node8", "color": "#ccc" }, { "id": "edge91", "source": "node9", "target": "node1", "color": "#ccc" }, { "id": "edge314", "source": "node3", "target": "node14", "color": "#ccc" }, { "id": "edge31", "source": "node3", "target": "node1", "color": "#ccc" }, { "id": "edge34", "source": "node3", "target": "node4", "color": "#ccc" }, { "id": "edge2231", "source": "node22", "target": "node31", "color": "#ccc" }, { "id": "edge2223", "source": "node22", "target": "node23", "color": "#ccc" }, { "id": "edge2232", "source": "node22", "target": "node32", "color": "#ccc" }, { "id": "edge2220", "source": "node22", "target": "node20", "color": "#ccc" }, { "id": "edge2128", "source": "node21", "target": "node28", "color": "#ccc" }, { "id": "edge2120", "source": "node21", "target": "node20", "color": "#ccc" }, { "id": "edge2129", "source": "node21", "target": "node29", "color": "#ccc" }, { "id": "edge2125", "source": "node21", "target": "node25", "color": "#ccc" }, { "id": "edge2019", "source": "node20", "target": "node19", "color": "#ccc" }, { "id": "edge2022", "source": "node20", "target": "node22", "color": "#ccc" }, { "id": "edge2021", "source": "node20", "target": "node21", "color": "#ccc" }, { "id": "edge1643", "source": "node16", "target": "node43", "color": "#ccc" }, { "id": "edge1642", "source": "node16", "target": "node42", "color": "#ccc" }, { "id": "edge4216", "source": "node42", "target": "node16", "color": "#ccc" }, { "id": "edge4215", "source": "node42", "target": "node15", "color": "#ccc" }, { "id": "edge1811", "source": "node18", "target": "node11", "color": "#ccc" }, { "id": "edge3222", "source": "node32", "target": "node22", "color": "#ccc" }, { "id": "edge3122", "source": "node31", "target": "node22", "color": "#ccc" }, { "id": "edge2921", "source": "node29", "target": "node21", "color": "#ccc" }, { "id": "edge2821", "source": "node28", "target": "node21", "color": "#ccc" }, { "id": "edge3323", "source": "node33", "target": "node23", "color": "#ccc" }, { "id": "edge2440", "source": "node24", "target": "node40", "color": "#ccc" }, { "id": "edge2435", "source": "node24", "target": "node35", "color": "#ccc" }, { "id": "edge2436", "source": "node24", "target": "node36", "color": "#ccc" }, { "id": "edge2423", "source": "node24", "target": "node23", "color": "#ccc" }, { "id": "edge3423", "source": "node34", "target": "node23", "color": "#ccc" }, { "id": "edge2333", "source": "node23", "target": "node33", "color": "#ccc" }, { "id": "edge2324", "source": "node23", "target": "node24", "color": "#ccc" }, { "id": "edge2334", "source": "node23", "target": "node34", "color": "#ccc" }, { "id": "edge2322", "source": "node23", "target": "node22", "color": "#ccc" }, { "id": "edge2625", "source": "node26", "target": "node25", "color": "#ccc" }, { "id": "edge2725", "source": "node27", "target": "node25", "color": "#ccc" }, { "id": "edge3738", "source": "node37", "target": "node38", "color": "#ccc" }, { "id": "edge3725", "source": "node37", "target": "node25", "color": "#ccc" }, { "id": "edge3739", "source": "node37", "target": "node39", "color": "#ccc" }, { "id": "edge2526", "source": "node25", "target": "node26", "color": "#ccc" }, { "id": "edge2521", "source": "node25", "target": "node21", "color": "#ccc" }, { "id": "edge2527", "source": "node25", "target": "node27", "color": "#ccc" }, { "id": "edge2537", "source": "node25", "target": "node37", "color": "#ccc" }, { "id": "edge1765", "source": "node17", "target": "node65", "color": "#ccc" }, { "id": "edge1747", "source": "node17", "target": "node47", "color": "#ccc" }, { "id": "edge1743", "source": "node17", "target": "node43", "color": "#ccc" }, { "id": "edge1744", "source": "node17", "target": "node44", "color": "#ccc" }, { "id": "edge4317", "source": "node43", "target": "node17", "color": "#ccc" }, { "id": "edge4316", "source": "node43", "target": "node16", "color": "#ccc" }, { "id": "edge6517", "source": "node65", "target": "node17", "color": "#ccc" },   { "id": "edge4041", "source": "node40", "target": "node41", "color": "#ccc" }, { "id": "edge4024", "source": "node40", "target": "node24", "color": "#ccc" }, { "id": "edge3624", "source": "node36", "target": "node24", "color": "#ccc" }, { "id": "edge3524", "source": "node35", "target": "node24", "color": "#ccc" }, { "id": "edge3837", "source": "node38", "target": "node37", "color": "#ccc" }, { "id": "edge3937", "source": "node39", "target": "node37", "color": "#ccc" }, { "id": "edge4562", "source": "node45", "target": "node62", "color": "#ccc" }, { "id": "edge4544", "source": "node45", "target": "node44", "color": "#ccc" }, { "id": "edge4445", "source": "node44", "target": "node45", "color": "#ccc" }, { "id": "edge4417", "source": "node44", "target": "node17", "color": "#ccc" }, { "id": "edge6364", "source": "node63", "target": "node64", "color": "#ccc" }, { "id": "edge6362", "source": "node63", "target": "node62", "color": "#ccc" }, { "id": "edge6263", "source": "node62", "target": "node63", "color": "#ccc" }, { "id": "edge6245", "source": "node62", "target": "node45", "color": "#ccc" }, { "id": "edge4664", "source": "node46", "target": "node64", "color": "#ccc" }, { "id": "edge4647", "source": "node46", "target": "node47", "color": "#ccc" }, { "id": "edge6463", "source": "node64", "target": "node63", "color": "#ccc" }, { "id": "edge6446", "source": "node64", "target": "node46", "color": "#ccc" }, { "id": "edge4746", "source": "node47", "target": "node46", "color": "#ccc" }, { "id": "edge4717", "source": "node47", "target": "node17", "color": "#ccc" }]
           }
      };

      return map[key] || null;

    }

});


