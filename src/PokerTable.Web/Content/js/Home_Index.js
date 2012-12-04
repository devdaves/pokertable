var Index = function () {
    var selectors = {
        CreateTableButton: '',
        TableNameInput: '',
        JoinTableButton: '',
        TableCodeInput: '',
        PlayerNameInput: ''
    }

    var urls = {
        CreateTable: '',
        JoinTable: '',
        TableView: '',
        PlayerView: ''
    }

    var JoinTable = function () {
        var tcode = $(selectors.TableCodeInput).val();
        var pname = $(selectors.PlayerNameInput).val();
        var button = $(selectors.JoinTableButton);
        $.ajax({
            dataType: "json",
            url: urls.JoinTable,
            type: "post",
            data: {
                tablePassword: tcode,
                playerName: pname
            },
            success: JoinTable_Success,
            error: Ajax_Error
        })
    }

    var JoinTable_Success = function (response) {
        console.log(response);
        var button = $(selectors.JoinTableButton);
        if (response.result == "fail") {
            //refactor this away
            alert(response.message);
        } else {
            var tableId = response.tableId;
            var playerId = response.playerId;
            window.location = urls.PlayerView + "?tableId=" + tableId + "&playerId=" + playerId;
        }
        $(button).button('reset');
    }

    var CreateTable = function () {
        var tname = $(selectors.TableNameInput).val();
        var button = $(selectors.CreateTableButton);
        $(button).button('loading');
        $.ajax({
            dataType: "json",
            url: urls.CreateTable,
            type: "post",
            data: { tableName: tname },
            success: CreateTable_Success,
            error: Ajax_Error
        })
    }

    var CreateTable_Success = function (response) {
        console.log(response);
        var button = $(selectors.CreateTableButton);
        if (response.result == "fail") {
            // refactor this away
            alert(response.message);
        } else {
            var tableId = response.tableId;
            window.location = urls.TableView + "?tableId=" + tableId;
        }
        $(button).button('reset');
    }

    var Ajax_Error = function (jqXHR, textStatus, errorThrown) {
        // log the error to the console
        console.log(
            "The following error occured: " +
            textStatus, errorThrown
        );
    }

    var setupevents = function () {
        $(selectors.JoinTableButton).click(JoinTable);
        $(selectors.CreateTableButton).click(CreateTable);
    }

    var init = function () {
        setupevents();
    }

    return {
        Selectors: selectors,
        Urls: urls,
        Init: init
    }
}();