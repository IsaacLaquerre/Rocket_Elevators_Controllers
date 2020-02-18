class Column {
    constructor(floors, elevators) {
        this.floors = floors;
        this.elevators = elevators;
        this.externalButtonList = [];
        this.elevatorList = [];
        for (let i = 0; i < elevators; i++) {
            this.elevatorList.push(new Elevator(1, floors));
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

		let bestGap = this.floors;

		for (let i = 0; i < this.elevatorList.length; i++) {
			if (this.elevatorList[i].status == "idle") {
                console.log("Best elevator found on floor " + this.elevatorList[i].currentFloor + ", going " + this.elevatorList[i].direction);
				return this.elevatorList[i];
			}else if (this.elevatorList[i].direction === "up" && direction === "up" && requestedFloor > this.elevatorList[i].currentFloor) {
                console.log("Best elevator found on floor " + this.elevatorList[i].currentFloor + ", going " + this.elevatorList[i].direction);
				return this.elevatorList[i];
			}else if (this.elevatorList[i].direction === "down" && direction === "down" && requestedFloor < this.elevatorList[i].currentFloor) {
                console.log("Best elevator found on floor " + this.elevatorList[i].currentFloor + ", going " + this.elevatorList[i].direction);
				return this.elevatorList[i];
			}else {
				for (let i = 0; i < this.elevatorList.length; i++) {
                    let gap = Math.abs(this.elevatorList[i].currentFloor - requestedFloor);
                    if (gap < bestGap) {
                        var chosenElevator = this.elevatorList[i];
                        bestGap = gap;
                    }
                }
                console.log("Best elevator found on floor " + this.elevatorList[i].currentFloor + " with a gap of " + bestGap + ", going " + this.elevatorList[i].direction);
				return chosenElevator;
			}
		}
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
				console.log("Waiting 7 seconds for the doorway to be cleared")
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
	constructor(requestFloor, direction, buttonPushed) {
		this.requestFloor = requestFloor;
		this.direction = direction;
		this.pushed = buttonPushed;
	}
}

class InternalButton {
	constructor(floor, buttonPushed) {
		this.floor = floor;
		this.buttonPushed = buttonPushed;
	}
}

console.log("--------------------------------------- TEST #1 ------------------------------------------------------\n\n")

function Test1_requestElevator() {
	
	column1 = new Column(10, 2);

	column1.elevatorList[0].currentFloor = 2
    column1.elevatorList[0].direction = "up"
    column1.elevatorList[0].status = "moving"
	column1.elevatorList[0].queue = [4, 6, 7]

	column1.elevatorList[1].currentFloor = 6
	column1.elevatorList[1].direction = "down"
	column1.elevatorList[1].status = "moving"
	column1.elevatorList[1].queue = [4, 3]

	column1.requestElevator(1, "down");
}

//Test1_requestElevator();

console.log("\n\n--------------------------------------- TEST #2 ------------------------------------------------------\n\n")

function  Test2_requestFloor(){
	column2 = new Column(10, 2);

	column2.elevatorList[0].currentFloor = 2;
	column2.elevatorList[0].direction  =  "up";  
	column2.elevatorList[0].status =  "moving";
	column2.elevatorList[0].queue = [3,4];

	column2.elevatorList[1].currentFloor = 2;
	column2.elevatorList[1].direction  =  null;
	column2.elevatorList[1].status =  "idle";
	column2.elevatorList[1].queue = [];

	elevator = column2.elevatorList[0];

	column2.requestFloor(elevator, 9);
}

//Test2_requestFloor();