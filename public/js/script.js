"use strict"

class Field {
    constructor() {
        this.field = [[0, 0, 0], [0, 0, 0], [0, 0, 0]]
    }

    setX(i, j) {
        this.field[i][j] = "x";
    }

    setO() {
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
};
