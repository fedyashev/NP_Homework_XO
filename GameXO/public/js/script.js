"use strict"

const CROSS = 1;
const ROUND = -1;
const EMPTY = 0;

class Game {
    constructor(id, p1, p2, key) {
        this.Id = id;
        this.Player1Name = p1;
        this.Player2Name = p2;
        this.Key = key;
        this.State = "";
        this.Turn = "";
        this.Winner = "";
        this.Field = [[EMPTY, EMPTY, EMPTY], [EMPTY, EMPTY, EMPTY], [EMPTY, EMPTY, EMPTY]];
    }
}

let gameField;
let playerName;
let game = null;

window.onload = () => {
    playerName = prompt("Enter user name:", "player");
    users.className = "d-none";
    field.className = "d-none";
    btnCreateGame.onclick = btnCreateGameOnclick;
    btnShowPlayers.onclick = btnShowPlayersOnclick;
    userName.innerText = playerName;
    RefreshPlayersList(users, 500);
    RefreshGameState(500, (err, packet) => {
        if (err) {
            console.log(err);
            return;
        }
        if (IsGameStateChanged(game, packet)) {
            game.Player1Name = packet.Player1Name;
            game.Player2Name = packet.Player2Name;
            game.State = packet.State;
            game.Turn = packet.Turn;
            game.Winner = packet.Winner;
            game.Field = packet.Field;
            RefreshGameField();
        }
        console.log(JSON.stringify(game));
    });
};

function GetListeners(callback) {
    $.ajax({
        method: "GET",
        url: "http://localhost:8080/api/v1/game/listeners",
        crossDomain: true
    })
    .done((data) => {
        return callback(null, data);
    })
    .fail((err) => {
        return callback(err);
    });
}

function CreatePlayersList(players) {
    let ul = document.createElement("ul");
    ul.className = "list-group";
    players.forEach(player => {
        let li = document.createElement("li");
        li.className = "list-group-item";
        let name = document.createElement("span");
        name.innerText = player.Name;
        let btnJoin = document.createElement("button");
        btnJoin.setAttribute("type", "button");
        btnJoin.setAttribute("id", player.Id);
        btnJoin.setAttribute("data-name", player.Name);
        btnJoin.innerText = "Join";
        btnJoin.onclick = btnJoinOnclick;
        li.appendChild(name);
        li.appendChild(btnJoin);
        ul.appendChild(li);
    });
    return ul;
}

function RefreshPlayersList(container, timeout) {
    setInterval(() => {
        GetListeners((err, data) => {
            if (err) {
                users.innerHTML = "<span>ERROR</span>";
                return;
            }
            let players = CreatePlayersList(JSON.parse(data).Players);
            users.innerHTML = "";
            users.appendChild(players);
        });
    }, timeout);
}

function btnJoinOnclick() {
    let id = this.id;
    let enemyName = this.getAttribute("data-name");
    let packet = {
        Type: "JOIN",
        Name: playerName
    };
    $.ajax({
        method: "POST",
        url: `/api/v1/game/${id}/join`,
        data: JSON.stringify(packet)
    })
    .done((data) => {
        let packet = JSON.parse(data);
        if (packet.Type === "JOINSUCCESS") {
            alert("Success");
            game = new Game(id, enemyName, playerName, packet.Key);
            ShowGameField();
        }
        else if (packet.Type === "JOINFAIL") {
            alert(packet.Message);
        }
        else {
            alert("Incorrect response");
        }
    })
    .fail((err) => {
        alert(JSON.stringify(err));
    });
}

function RefreshGameState(timeout, callback) {
    setInterval(() => {
        if (game) {
            $.ajax({
                method: "GET",
                url: `/api/v1/game/${game.Id}/state`
            })
            .done((data) => {
                return callback(null, JSON.parse(data));
            })
            .fail((err) => {
                return callback(err);
            });
        }
    }, timeout);
}

function btnCreateGameOnclick() {
    let packet = {
        type: "CREATEGAME",
        name: playerName
    };
    $.ajax({
        method: "POST",
        url: "api/v1/game/create",
        data: JSON.stringify(packet)
    })
    .done((data) => {
        data = JSON.parse(data);
        if (data.Type === "CREATEGAMESUCCESS") {
            game = new Game(data.Id, playerName, "", data.Key);
            ShowGameField();
        }
    })
    .fail((err) => {
        console.log(err);
    });
}

function btnShowPlayersOnclick() {
    game = null;
    users.className = "d-block";
    field.className = "d-none";
}

function CreateGameFieldCompanent(obj) {
    let gameFieldCompanent = document.createElement("div");

    let gamePlayerX = document.createElement("span");
    gamePlayerX.className = "badge badge-primary mr-2";
    gamePlayerX.innerText = `Player X : ${obj.Player1Name}`;
    gameFieldCompanent.appendChild(gamePlayerX);

    let gamePlayerO = document.createElement("span");
    gamePlayerO.className = "badge badge-danger mr-2";
    gamePlayerO.innerText = `Player O : ${obj.Player2Name}`;
    gameFieldCompanent.appendChild(gamePlayerO);

    let gameState = document.createElement("span");
    gameState.className = "badge badge-warning mr-2";
    gameState.innerText = `Game state: ${obj.State}`;
    gameFieldCompanent.appendChild(gameState);

    if (obj.Turn) {
        let gameTurn = document.createElement("span");
        gameTurn.className = `badge badge-${obj.Turn === obj.Player1Name ? "primary" : "danger"} mr-2`;
        gameTurn.innerText = `Turn: ${obj.Turn}`;
        gameFieldCompanent.appendChild(gameTurn);
    }

    if (obj.Winner) {
        let gameWinner = document.createElement("span");
        gameWinner.className = "badge badge-success";
        gameWinner.innerText = `Winner: ${obj.Winner}`;
        gameFieldCompanent.appendChild(gameWinner);
    }

    let gameField = document.createElement("table");
    gameField.className = "game-field align-self-center";
    for (let i = 0; i < obj.Field.length; i++) {
        let row = document.createElement("tr");
        row.className = "game-field-row";
        for (let j = 0; j < obj.Field[i].length; j++) {
            let col = document.createElement("td");
            col.className = `game-field-col text-center h1 bg-light border border-dark ${GetTextColorClass(obj.Field[i][j])}`;
            col.setAttribute("data-i", i);
            col.setAttribute("data-j", j);
            col.innerText = TransformValue(obj.Field[i][j]);
            col.onclick = FieldCellOnclick;
            row.appendChild(col);
        }
        gameField.appendChild(row);
    }
    gameFieldCompanent.appendChild(gameField);
    return gameFieldCompanent;
}

function TransformValue(value) {
    switch (value) {
        case CROSS : return "X";
        case ROUND : return "O";
        default: return "";
    }
}

function GetTextColorClass(sign) {
    switch (sign) {
        case CROSS : return "text-primary";
        case ROUND : return "text-danger";
        default: return "";
    }
}

function ShowGameField() {
    if (game) {
        users.className = "d-none";
        field.className = "d-block";
        let gameField = CreateGameFieldCompanent(game);
        field.innerHTML = "";
        field.appendChild(gameField);
    }
    else {
        field.innerText = "Error";
    }
}

function RefreshGameField() {
    if (game) {
        let gameField = CreateGameFieldCompanent(game);
        field.innerHTML = "";
        field.appendChild(gameField);
    }
}

function FieldCellOnclick() {
    if (game) {
        let i = this.getAttribute("data-i");
        let j = this.getAttribute("data-j");
        let packet = {
            Type: "ACTION",
            Name: playerName,
            Key: game.Key,
            Row: i,
            Col: j
        };
        $.ajax({
            method: "POST",
            url: `api/v1/game/${game.Id}/action`,
            data: JSON.stringify(packet)
        })
        .done((data) => {
            data = JSON.parse(data);
            if (data.Type === "ACTIONSUCCESS") {
                console.log("Action success.");
            }
            else if (data.Type === "ACTIONFAIL") {
                console.log(data.Message);
            }
            else {
                console.log("Unknow response packet.");
            }
        })
        .fail((err) => {
            console.log(err);
        });
    }
}

function IsGameStateChanged(gameObject, packet) {
    if (gameObject.Player1Name !== packet.Player1Name) return true;
    if (gameObject.Player2Name !== packet.Player2Name) return true;
    if (gameObject.State !== packet.State) return true;
    if (gameObject.Turn !== packet.Turn) return true;
    if (gameObject.Winner !== packet.Winner) return true;
    return false;
}