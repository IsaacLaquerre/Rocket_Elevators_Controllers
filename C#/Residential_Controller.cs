public class Column {
    public int floors;
    public int elevators;
    public List<object> elevatorsList;
    public List<object> externalButtonList;
    public Column(int floors, int elevators){
        this.floors = floors;
        this.elevators = elevators;
        for (int i = 0; i < elevators; i++) {
            this.elevatorsList.Add(new Elevator(0, floors));
        }
		for (int i = 0; i < this.floors; i++) {
            if (i == 0) {
                this.externalButtonList.Add(new ExternalButton(i, "up", false));
            }else {
                this.externalButtonList.Add(new ExternalButton(i, "up", false));
                this.externalButtonList.Add(new ExternalButton(i, "down", false));
            }
		}
    }

    function findElevator(int requestedFloor, string direction) {

        int chosenElevator = null;
        int bestGap = this.floors;

		for (let i = 0; i < this.elevatorsList.length; i++) {
			if (this.elevatorsList[i].direction == "up" && direction == "up" && requestedFloor > this.elevatorsList[i].currentFloor) {
                chosenElevator = this.elevatorsList[i];
			}else if (this.elevatorsList[i].direction == "down" && direction == "down" && requestedFloor < this.elevatorsList[i].currentFloor) {
                chosenElevator = this.elevatorsList[i];
			}else if (this.elevatorsList[i].status == "idle") {
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
		console.write("Best elevator found on floor " + chosenElevator.currentFloor);
		return chosenElevator;
    }
	requestElevator(int requestedFloor, string direction) {
		if (requestedFloor > this.floors) return console.log("Floor " + requestedFloor + " doesn't exist!");

		console.write("Called an elevator to the floor " + requestedFloor);

		let elevator = this.findElevator(requestedFloor, direction);

		elevator.addToQueue(requestedFloor);
        elevator.move();
		return elevator;
	}

	requestFloor(object elevator, int requestedFloor) {
        console.write("Moving elevator on floor " + elevator.currentFloor + " to the floor " + requestedFloor);
		elevator.addToQueue(requestedFloor);
		elevator.closeDoors();
		elevator.move();
    }
}
class Elevator {

    public List<object> internalButtonsList;
    public List<object> queue;
	constructor(int currentFloor, int floors) {

		this.direction = null;
		this.floors = floors;
		this.currentFloor = currentFloor;
		this.status = "idle";
		this.door = "closed";

		for (let i = 0; i < this.floors; i++) {
            this.internalButtonsList.push(new InternalButton(i));
        }
    }
	addToQueue(int requestedFloor) {
		this.queue.push(requestedFloor);

		if (this.direction == "up") {
			this.queue.sort((a, b) => a - b);
		}
		if (this.direction == "down") {
			this.queue.sort((a, b) => b - a);
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
				this.moveDown();
			}
		}
		console.log("Waiting 7 seconds for the doorway to be cleared");
		this.closeDoors();
		console.log("Elevator is now idle");
		this.status = "idle";
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
			this.door = "open";
			console.log("<> Opened doors");
	}

	closeDoors() {
			this.door="closed";
			console.log(">< Closed doors");
	}

}

class ExternalButton {
	constructor(int requestFloor, string direction) {
		this.requestFloor = requestFloor;
		this.direction = direction;
	}
}

class InternalButton {
	constructor(int floor) {
		this.floor = floor;
	}
}

/* -------------------------- TESTING SECTION -------------------------- */

int Main() {


console.write("--------------------------------------- TEST #1 ------------------------------------------------------\n\n");

function Test1_requestElevator() {

    List<int> queue1 = new List<int>();
    int[] array1 = new int[] { 4, 5, 7 };
    queue1.AddRange(array);
    List<int> queue2 = new List<int>();
    int[] array2 = new int[] { 4, 3 };
    queue2.AddRange(array);
    List<int> queue3 = new List<int>();
    int[] array3 = new int[] { 1, 2, 5, 7 };
    queue3.AddRange(array);

	column1 = new Column(10, 2);

	column1.elevatorsList[0].currentFloor = 2;
    column1.elevatorsList[0].direction = "up";
    column1.elevatorsList[0].status = "moving";
	column1.elevatorsList[0].queue = queue1;

	column1.elevatorsList[1].currentFloor = 6;
	column1.elevatorsList[1].direction = "down";
	column1.elevatorsList[1].status = "moving";
	column1.elevatorsList[1].queue = queue2;

	column1.requestElevator(1, "down");
}

Test1_requestElevator();

console.write("\n\n--------------------------------------- TEST #2 ------------------------------------------------------\n\n");

function  Test2_requestFloor(){
	column2 = new Column(10, 2);

	column2.elevatorsList[0].currentFloor = 2;
	column2.elevatorsList[0].direction  =  "down";
	column2.elevatorsList[0].status =  "moving";
	column2.elevatorsList[0].queue = queue3;

	elevator = column2.elevatorsList[0];

	column2.requestFloor(elevator, 9);
}

Test2_requestFloor();
}