class Battery {
    constructor(columns, floors, basements, elevatorsPerColumn) {
        this.columns = columns;
        this.floors = floors + basements;
        this.basements = basements;
        this.elevatorsPerColumn = elevatorsPerColumn
        this.columnsList = [];
        for (let i = 0; i != this.columns; i++) {
            this.columnsList.push(new Column(this.floors, elevatorsPerColumn));
        }
    }

    decideColumn(requestedFloor) {
        let floorsPerColumn = Math.round(this.floors / this.columns);
        for (let index = 0; index < this.columnsList.length; index++) {
            for (let i = 0; i < this.columnsList[index].elevatorsList.length; i++) {
                if (requestedFloor > ((floorsPerColumn * index) - floorsPerColumn) && requestedFloor < ((floorsPerColumn * index) + floorsPerColumn)) {
                    console.log("Chosen column #" + (index + 1));
                    return (index + 1);
                }
            }
        }
    }

    findElevator(requestedFloor, direction, column) {

        column--;

        let chosenElevator = null;

        for (let i = 0; i < this.columnsList[column].elevatorsList.length; i++) {
            if (this.columnsList[column].elevatorsList[i].direction === "up" && direction === "up" && requestedFloor > this.columnsList[column].elevatorsList[i].currentFloor) {
                var bestGap = this.floors;
                for (let i = 0; i < this.columnsList[column].elevatorsList.length; i++) {
                    if (this.columnsList[column].elevatorsList[i].direction != "up") continue;
                    let gap = Math.abs(this.columnsList[column].elevatorsList[i].currentFloor - requestedFloor);
                    if (gap < bestGap) {
                    chosenElevator = this.columnsList[column].elevatorsList[i];
                        bestGap = gap;
                    }
                }
            }else if (this.columnsList[column].elevatorsList[i].direction === "down" && direction === "down" && requestedFloor < this.columnsList[column].elevatorsList[i].currentFloor) {
                bestGap = this.floors;
                for (let i = 0; i < this.columnsList[column].elevatorsList.length; i++) {
                    if (this.columnsList[column].elevatorsList[i].direction != "down") continue;
                    let gap = Math.abs(this.columnsList[column].elevatorsList[i].currentFloor - requestedFloor);
                    if (gap < bestGap) {
                    chosenElevator = this.columnsList[column].elevatorsList[i];
                        bestGap = gap;
                    }
                }
            }else if (this.columnsList[column].elevatorsList[i].status === "idle") {
                chosenElevator = this.columnsList[column].elevatorsList[i];
            }else {
                bestGap = this.floors;
                let gap = Math.abs(this.columnsList[column].elevatorsList[i].currentFloor - requestedFloor);
                if (gap < bestGap) {
                    chosenElevator = this.columnsList[column].elevatorsList[i];
                    bestGap = gap;
                }
            }
            console.log("Best elevator found on floor " + this.columnsList[column].elevatorsList[i].currentFloor);
            return chosenElevator;
        }
    }

	requestElevator(requestedFloor, direction) {
        if (requestedFloor < -Math.abs(this.basements) || requestedFloor > (this.floors - this.basements)) return console.log("Floor " + requestedFloor + " doesn't exist!");

        let column = this.decideColumn(requestedFloor);

		console.log("Called an elevator to the floor " + requestedFloor + " in the collumn #" + column);

		let elevator = this.findElevator(requestedFloor, direction, column);

		elevator.addToQueue(requestedFloor);
        elevator.move();
		return elevator;
	}

	requestFloor(elevator) {
        console.log("Moving elevator on floor " + elevator.currentFloor + " to the ground floor");

        let requestedFloor = 0;

		elevator.addToQueue(requestedFloor);
		elevator.closeDoors();
		elevator.move();
    }
}

class Column {
    constructor(floors, elevators) {
        this.floors = floors;
        this.elevators = elevators;
        this.externalButtonList = [];
        this.elevatorsList = [];
        for (let i = 0; i < elevators; i++) {
            this.elevatorsList.push(new Elevator(0, floors));
        }
		for (let i = 0; i < this.floors; i++) {
            if (i === 0) {
                this.externalButtonList.push(new ExternalButton(i, "up", false));
            }else {
                this.externalButtonList.push(new ExternalButton(i, "up", false));
                this.externalButtonList.push(new ExternalButton(i, "down", false));
            }
		}
    }
}

class Elevator { 
	constructor(currentFloor, floors) {

		this.direction = null;
		this.floors = floors;
		this.currentFloor = currentFloor;
		this.status = "idle";
		this.queue = [];
		this.internalButtonList = [];
		this.door = "closed";

		for (let i = 0; i < this.floors; i++) {
            this.internalButtonList.push(new InternalButton(i, false));
        }
    }
	addToQueue(requestedFloor) {
		this.queue.push(requestedFloor)

		if (this.direction == "up") {
			this.queue.sort((a, b) => a - b)
		}
		if (this.direction == "down") {
			this.queue.sort((a, b) => b - a)
		}

		console.log("Added floor " + requestedFloor + " to the elevator's queue. Current queue: " + this.queue.join(", "));
	}
	move() {
		console.log("Moving elevator");
		while (this.queue.length > 0) {

            let firstElement = this.queue[0];
            
			if (this.door == "open") {
				console.log("Waiting 7 seconds for the doorway to be cleared");
				this.closeDoors();
            }
			if (firstElement == this.currentFloor) {
				this.queue.shift();
				this.openDoors();
			}
			if (firstElement > this.currentFloor) {
				this.status = "moving";
				this.direction = "up";
				this.moveUp();
			}
			if (firstElement < this.currentFloor) {
				this.status = "moving";
				this.direction = "down";
				this.moveDown()
			}
		}
		if (this.queue.length === 0) {
			console.log("Waiting 7 seconds for the doorway to be cleared");
			this.closeDoors();
			console.log("Elevator is now idle");
			this.status = "idle";
		}
	}
	moveUp() {
		this.currentFloor++;
		console.log("^^^ Elevator on floor " + this.currentFloor.toString().replace(/-/, "basement "));
	}

	moveDown() {
		this.currentFloor--;
		console.log("vvv Elevator on floor " + this.currentFloor.toString().replace(/-/, "basement "));
	}

	openDoors() {
			this.door = "open"
			console.log("<> Opened doors");
	}
	
	closeDoors() {
			this.door="closed"
			console.log(">< Closed doors");
	}

}

class ExternalButton {
	constructor(requestFloor, direction) {
		this.requestFloor = requestFloor;
		this.direction = direction;
	}
}

class InternalButton {
	constructor() {
		this.floor = 0;
	}
}
//console.log("--------------------------------------- TEST #1 ------------------------------------------------------\n\n")

function Test1_requestElevator() {
	
	var battery1 = new Battery(3, 100, 10, 4);

	battery1.columnsList[0].elevatorsList[0].currentFloor = 2;
    battery1.columnsList[0].elevatorsList[0].direction = "up";
    battery1.columnsList[0].elevatorsList[0].status = "moving";
	battery1.columnsList[0].elevatorsList[0].queue = [1,2];

	battery1.columnsList[0].elevatorsList[1].currentFloor = 3;
    battery1.columnsList[0].elevatorsList[1].direction = "down";
    battery1.columnsList[0].elevatorsList[1].status = "moving";
	battery1.columnsList[0].elevatorsList[1].queue = [3,7,8];

	battery1.columnsList[0].elevatorsList[2].currentFloor = 38;
    battery1.columnsList[0].elevatorsList[2].direction = "up";
    battery1.columnsList[0].elevatorsList[2].status = "moving";
    battery1.columnsList[0].elevatorsList[2].queue = [-1,5,8];
    
	battery1.columnsList[0].elevatorsList[3].currentFloor = 38;
    battery1.columnsList[0].elevatorsList[3].direction = "up";
    battery1.columnsList[0].elevatorsList[3].status = "moving";
	battery1.columnsList[0].elevatorsList[3].queue = [-1,5,8];

	battery1.columnsList[1].elevatorsList[0].currentFloor = 5;
    battery1.columnsList[1].elevatorsList[0].direction = null;
    battery1.columnsList[1].elevatorsList[0].status = "idle";
	battery1.columnsList[1].elevatorsList[0].queue = [];

	battery1.columnsList[1].elevatorsList[1].currentFloor = 6;
    battery1.columnsList[1].elevatorsList[1].direction = "up";
    battery1.columnsList[1].elevatorsList[1].status = "moving";
	battery1.columnsList[1].elevatorsList[1].queue = [1,2,3,4];

	battery1.columnsList[1].elevatorsList[2].currentFloor = 7;
    battery1.columnsList[1].elevatorsList[2].direction = "down";
    battery1.columnsList[1].elevatorsList[2].status = "moving";
    battery1.columnsList[1].elevatorsList[2].queue = [-3,2,7,8];
    
	battery1.columnsList[1].elevatorsList[3].currentFloor = 7;
    battery1.columnsList[1].elevatorsList[3].direction = "down";
    battery1.columnsList[1].elevatorsList[3].status = "moving";
	battery1.columnsList[1].elevatorsList[3].queue = [-3,2,7,8];

	battery1.columnsList[2].elevatorsList[0].currentFloor = 6;
    battery1.columnsList[2].elevatorsList[0].direction = "down";
    battery1.columnsList[2].elevatorsList[0].status = "moving";
	battery1.columnsList[2].elevatorsList[0].queue = [1,4];

	battery1.columnsList[2].elevatorsList[1].currentFloor = 9;
    battery1.columnsList[2].elevatorsList[1].direction = null;
    battery1.columnsList[2].elevatorsList[1].status = "idle";
	battery1.columnsList[2].elevatorsList[1].queue = [];

	battery1.columnsList[2].elevatorsList[2].currentFloor = 38;
    battery1.columnsList[2].elevatorsList[2].direction = "up";
    battery1.columnsList[2].elevatorsList[2].status = "moving";
	battery1.columnsList[2].elevatorsList[2].queue = [4, 6, 7];

	battery1.columnsList[1].elevatorsList[3].currentFloor = 7;
    battery1.columnsList[1].elevatorsList[3].direction = "down";
    battery1.columnsList[1].elevatorsList[3].status = "moving";
	battery1.columnsList[1].elevatorsList[3].queue = [-3,2,7,8];

	battery1.requestElevator(99, "down");
}

//Test1_requestElevator();

//console.log("\n\n--------------------------------------- TEST #2 ------------------------------------------------------\n\n")

function  Test2_requestFloor(){
	var battery2 = new Battery(3, 3, 3, 4);

	battery2.columnsList[0].elevatorsList[0].currentFloor = 2;
    battery2.columnsList[0].elevatorsList[0].direction = "up";
    battery2.columnsList[0].elevatorsList[0].status = "moving";
	battery2.columnsList[0].elevatorsList[0].queue = [-3,4,6,7];

	battery2.columnsList[0].elevatorsList[1].currentFloor = 2;
    battery2.columnsList[0].elevatorsList[1].direction = "down";
    battery2.columnsList[0].elevatorsList[1].status = "moving";
	battery2.columnsList[0].elevatorsList[1].queue = [3,7,8];

	battery2.columnsList[0].elevatorsList[2].currentFloor = 2;
    battery2.columnsList[0].elevatorsList[2].direction = "up";
    battery2.columnsList[0].elevatorsList[2].status = "moving";
	battery2.columnsList[0].elevatorsList[2].queue = [-1,5,8];

	battery2.columnsList[1].elevatorsList[0].currentFloor = 2;
    battery2.columnsList[1].elevatorsList[0].direction = null;
    battery2.columnsList[1].elevatorsList[0].status = "idle";
	battery2.columnsList[1].elevatorsList[0].queue = [];

	battery2.columnsList[1].elevatorsList[1].currentFloor = 2;
    battery2.columnsList[1].elevatorsList[1].direction = "up";
    battery2.columnsList[1].elevatorsList[1].status = "moving";
	battery2.columnsList[1].elevatorsList[1].queue = [1,2,3,4];

	battery2.columnsList[1].elevatorsList[2].currentFloor = 2;
    battery2.columnsList[1].elevatorsList[2].direction = "down";
    battery2.columnsList[1].elevatorsList[2].status = "moving";
	battery2.columnsList[1].elevatorsList[2].queue = [-3,2,7,8];

	battery2.columnsList[2].elevatorsList[0].currentFloor = 2;
    battery2.columnsList[2].elevatorsList[0].direction = "down";
    battery2.columnsList[2].elevatorsList[0].status = "moving";
	battery2.columnsList[2].elevatorsList[0].queue = [1,4];

	battery2.columnsList[2].elevatorsList[1].currentFloor = 2;
    battery2.columnsList[2].elevatorsList[1].direction = null;
    battery2.columnsList[2].elevatorsList[1].status = "idle";
	battery2.columnsList[2].elevatorsList[1].queue = [];

	battery2.columnsList[2].elevatorsList[2].currentFloor = 2;
    battery2.columnsList[2].elevatorsList[2].direction = "up";
    battery2.columnsList[2].elevatorsList[2].status = "moving";
	battery2.columnsList[2].elevatorsList[2].queue = [4, 6, 7];

    elevator = battery2.columnsList[1].elevatorsList[2];
    
	battery2.requestFloor(elevator);
}

//Test2_requestFloor();