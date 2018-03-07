"use strict"

const CROSS = 1;
const ROUND = -1;
const EMPTY = 0;

class Field {

    constructor() {
        this.field = [[EMPTY, EMPTY, EMPTY], [EMPTY, EMPTY, EMPTY], [EMPTY, EMPTY, EMPTY]]
    }

    setCross(i, j) {
        this.field[i][j] = "x";
    }

    setRound() {
        this.field[i][j] = "y";
    }

    getCompanent() {
        let gameField = document.createElement("table");
        gameField.className = "game-field";
        for (let i = 0; i < this.field.length; i++) {
            let row = document.createElement("tr");
            row.className = "game-field-row";
            for (let j = 0; j < this.field[i].length; j++) {
                let col = document.createElement("td");
                col.className = "game-field-col";
                col.setAttribute("data-i", i);
                col.setAttribute("data-j", j);
                row.appendChild(col);
            }
            gameField.appendChild(row);
        }
        return gameField;
    }
}

let gameField;

window.onload = () => {
    gameField = new Field();
    field.appendChild(gameField.getCompanent());
    GetListeners((err, data) => {
        if (err) {
            users.innerText = JSON.stringify(err);
            return;
        }
        users.innerText = data;
    });
};

function GetListeners(callback) {
    $.ajax({
        method: "GET",
        url: "http://localhost:8080/api/v1/game/listeners",
        headers: {
            "Access-Control-Allow-Origin": "true"
        },
        crossDomain: true
    })
    .done((data) => {
        return callback(null, data);
    })
    .fail((err) => {
        return callback(err);
    });
} 