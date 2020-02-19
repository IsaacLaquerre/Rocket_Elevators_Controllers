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

    findElevator(requestedFloor, direction) {

		var bestGap = this.floors;
		var chosenElevator = null;

		for (let i = 0; i < this.elevatorsList.length; i++) {
			if (this.elevatorsList[i].direction === "up" && direction === "up" && requestedFloor > this.elevatorsList[i].currentFloor) {
                chosenElevator = this.elevatorsList[i];
			}else if (this.elevatorsList[i].direction === "down" && direction === "down" && requestedFloor < this.elevatorsList[i].currentFloor) {
                chosenElevator = this.elevatorsList[i];
			}else if (this.elevatorsList[i].status === "idle") {
				chosenElevator = this.elevatorsList[i];
			}else {
				for (let i = 0; i < this.elevatorsList.length; i++) {
                    let gap = Math.abs(this.elevatorsList[i].currentFloor - requestedFloor);
                    if (gap < bestGap) {
                        chosenElevator = this.elevatorsList[i];
                        bestGap = gap;
                    }
                }
			}
		}
		console.log("Best elevator found on floor " + chosenElevator.currentFloor);
		return chosenElevator;
	}
	requestElevator(requestedFloor, direction) {

		console.log("Called an elevator to the floor " + requestedFloor);

		let elevator = this.findElevator(requestedFloor, direction);

		elevator.addToQueue(requestedFloor);
        elevator.move();
		return elevator;
	}

	requestFloor(elevator, requestedFloor) {
        console.log("Moving elevator on floor " + elevator.currentFloor + " to the floor " + requestedFloor);
		elevator.addToQueue(requestedFloor);
		elevator.closeDoors();
		elevator.move();
    }
}

class Elevator {
	constructor(currentFloor, floors) {

		this.direction = null;
		this.floors = floors;
		this.currentFloor = currentFloor;
		this.status = "idle";
		this.queue = [];
		this.internalButtonsList = [];
		this.door = "closed";

		for (let i = 0; i < this.floors; i++) {
            this.internalButtonsList.push(new InternalButton(i, false));
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

			if (this.door === "open") {
				console.log("Waiting 7 seconds for the doorway to be cleared");
				this.closeDoors();
            }
			if (firstElement === this.currentFloor) {
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
		console.log("^^^ Elevator on floor " + this.currentFloor);
	}

	moveDown() {
		this.currentFloor--;
		console.log("vvv Elevator on floor " + this.currentFloor);
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
	constructor(floor) {
		this.floor = floor;
	}
}

console.log("--------------------------------------- TEST #1 ------------------------------------------------------\n\n")

function Test1_requestElevator() {

	column1 = new Column(10, 2);

	column1.elevatorsList[0].currentFloor = 2
    column1.elevatorsList[0].direction = "up"
    column1.elevatorsList[0].status = "moving"
	column1.elevatorsList[0].queue = [4, 6, 7]

	column1.elevatorsList[1].currentFloor = 6
	column1.elevatorsList[1].direction = "down"
	column1.elevatorsList[1].status = "moving"
	column1.elevatorsList[1].queue = [4, 3]

	column1.requestElevator(1, "down");
}

Test1_requestElevator();

//console.log("\n\n--------------------------------------- TEST #2 ------------------------------------------------------\n\n")

function  Test2_requestFloor(){
	column2 = new Column(10, 2);

	column2.elevatorsList[0].currentFloor = 2;
	column2.elevatorsList[0].direction  =  "down";
	column2.elevatorsList[0].status =  "moving";
	column2.elevatorsList[0].queue = [3,10,5,7,1];

	elevator = column2.elevatorsList[0];

	column2.requestFloor(elevator, 9);
}

//Test2_requestFloor();