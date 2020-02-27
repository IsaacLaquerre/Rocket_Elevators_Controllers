using System;
using System.Collections.Generic;

namespace Program
{

    public class Battery {
        public int floors;
        public int elevators;
        public int basements;
        public List<object> columnsList;

        public Battery(int columns, int floors, int basements, int elevators, int elevatorsPerColumn) {
            this.columms = columns;
            this.floorss = floors;
            this.basements = basements;
            this.elevators = elevators;

            for (int i = 0; i != this.columns; i++) {
                this.columnsList.push(new Column(this.floors, elevatorsPerColumn));
            }
        }

        public int decideColumn(int requestedFloor) {
            int floorsPerColumn = Math.round(this.floors / this.columns);
            for (int index = 0; index < this.columnsList.length; index++) {
                if (requestedFloor > ((floorsPerColumn * index) - floorsPerColumn) && requestedFloor < ((floorsPerColumn * index) + floorsPerColumn)) {
                    console.write("Chosen column #" + (index + 1));
                    return index;
                }
            }
        }

        public object findElevator(int requestedFloor, string direction, object column) {

            column--;

            object chosenElevator = null;
            int bestGap = this.floors;

            for (int i = 0; i < this.columnsList[column].elevatorsList.length; i++) {
                if (this.columnsList[column].elevatorsList[i].direction == "up" && direction == "up" && requestedFloor > this.columnsList[column].elevatorsList[i].currentFloor) {
                    for (let i = 0; i < this.columnsList[column].elevatorsList.length; i++) {
                        if (this.columnsList[column].elevatorsList[i].direction != "up") continue;
                        let gap = Math.abs(this.columnsList[column].elevatorsList[i].currentFloor - requestedFloor);
                        if (gap < bestGap) {
                        chosenElevator = this.columnsList[column].elevatorsList[i];
                            bestGap = gap;
                        }
                    }
                }else if (this.columnsList[column].elevatorsList[i].direction == "down" && direction == "down" && requestedFloor < this.columnsList[column].elevatorsList[i].currentFloor) {
                    for (let i = 0; i < this.columnsList[column].elevatorsList.length; i++) {
                        if (this.columnsList[column].elevatorsList[i].direction != "down") continue;
                        let gap = Math.abs(this.columnsList[column].elevatorsList[i].currentFloor - requestedFloor);
                        if (gap < bestGap) {
                        chosenElevator = this.columnsList[column].elevatorsList[i];
                            bestGap = gap;
                        }
                    }
                }else if (this.columnsList[column].elevatorsList[i].status == "idle") {
                    chosenElevator = this.columnsList[column].elevatorsList[i];
                }else {
                    bestGap = this.floors;
                    let gap = Math.abs(this.columnsList[column].elevatorsList[i].currentFloor - requestedFloor);
                    if (gap < bestGap) {
                        chosenElevator = this.columnsList[column].elevatorsList[i];
                        bestGap = gap;
                    }
                }
            }
            Console.WriteLine("Best elevator found on floor " + this.columnsList[column].elevatorsList[i].currentFloor);
            return chosenElevator;
        }
        public object requestElevator(int requestedFloor, string direction) {
            if (requestedFloor < -Math.abs(this.basements) || requestedFloor > (this.floors - this.basements)) return Console.WriteLine("Floor " + requestedFloor + " doesn't exist!");

            let column = this.decideColumn(requestedFloor);

            Console.WriteLine("Called an elevator to the floor " + requestedFloor + " in the collumn #" + (column + 1));

            let elevator = this.findElevator(requestedFloor, direction, column);

            elevator.addToQueue(requestedFloor);
            elevator.move();
            return elevator;
        }

        public void requestFloor(object elevator) {
            console.log("Moving elevator on floor " + elevator.currentFloor + " to the ground floor");

            let requestedFloor = 0;

            elevator.addToQueue(requestedFloor);
            elevator.closeDoors();
            elevator.move();
        }
    }
}