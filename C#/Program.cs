using System;
using System.Collections.Generic;

namespace Rocket_Elevators_Controllers
{
    public class Commercial_Controller {

        public class Battery {
            public int columns;
            public int floors;
            public int basements;
            public int elevatorsPerColumn;
            public List<Column> columnsList;

            public Battery(int columns, int floors, int basements, int elevatorsPerColumn) {
                this.columns = columns;
                this.floors = floors;
                this.basements = basements;
                this.columnsList = new List<Column>();

                for (int i = 0; i != this.columns; i++) {
                    this.columnsList.Add(new Column(this.floors, elevatorsPerColumn));
                }
            }

            public int decideColumn(int requestedFloor) {
                decimal floorspercolumn = Convert.ToDecimal(this.floors / this.columns) / 2.0M;
                decimal floorsPerColumn = Math.Round(floorspercolumn, 1);
                for (int index = 0; index < this.columnsList.Count; index++) {
                    if (requestedFloor > ((floorsPerColumn * index) - floorsPerColumn) && requestedFloor < ((floorsPerColumn * index) + floorsPerColumn)) {
                        Console.WriteLine("Chosen column #" + (index + 1));
                        return index;
                    }
                }
                Console.WriteLine("Error returning collumn index");
                throw new NotImplementedException();
            }

            public Elevator findElevator(int requestedFloor, string direction, int column) {

                Elevator chosenElevator = null;
                int bestGap = this.floors;

                for (int i = 0; i < this.columnsList[column].elevatorsList.Count; i++) {
                    if (this.columnsList[column].elevatorsList[i].direction == "up" && direction == "up" && requestedFloor > this.columnsList[column].elevatorsList[i].currentFloor) {
                        for (int ind = 0; ind < this.columnsList[column].elevatorsList.Count; ind++) {
                            if (this.columnsList[column].elevatorsList[ind].direction != "up") continue;
                            int gap = Math.Abs(this.columnsList[column].elevatorsList[ind].currentFloor - requestedFloor);
                            Console.WriteLine(gap);
                            if (gap < bestGap) {
                                chosenElevator = this.columnsList[column].elevatorsList[ind];
                                bestGap = gap;
                            }
                        }
                    }else if (this.columnsList[column].elevatorsList[i].direction == "down" && direction == "down" && requestedFloor < this.columnsList[column].elevatorsList[i].currentFloor) {
                        for (int ind = 0; ind < this.columnsList[column].elevatorsList.Count; ind++) {
                            if (this.columnsList[column].elevatorsList[ind].direction != "down") continue;
                            int gap = Math.Abs(this.columnsList[column].elevatorsList[ind].currentFloor - requestedFloor);
                            Console.WriteLine(gap);
                            if (gap < bestGap) {
                            chosenElevator = this.columnsList[column].elevatorsList[ind];
                                bestGap = gap;
                            }
                        }
                    }else if (this.columnsList[column].elevatorsList[i].status == "idle") {
                        chosenElevator = this.columnsList[column].elevatorsList[i];
                    }else {
                        bestGap = this.floors;
                        int gap = Math.Abs(this.columnsList[column].elevatorsList[i].currentFloor - requestedFloor);
                        if (gap < bestGap) {
                            chosenElevator = this.columnsList[column].elevatorsList[i];
                            bestGap = gap;
                        }
                    }
                }
                Console.WriteLine("Best elevator found on floor " + chosenElevator.currentFloor);
                return chosenElevator;
            }
            public Elevator requestElevator(int requestedFloor, string direction) {
                if (requestedFloor < (Math.Abs(this.basements) * -1) || requestedFloor > (this.floors - this.basements)) {
                    Console.WriteLine("Floor " + requestedFloor + " doesn't exist!");
                    throw new NotImplementedException();
                }

                int column = this.decideColumn(requestedFloor);

                Console.WriteLine("Called an elevator to the floor " + requestedFloor + " in the collumn #" + (column + 1));

                Elevator elevator = this.findElevator(requestedFloor, direction, column);

                elevator.addToQueue(requestedFloor);
                elevator.move();
                return elevator;
            }

            public void requestFloor(Elevator elevator) {
                Console.WriteLine("Moving elevator on floor " + elevator.currentFloor + " to the ground floor");

                int requestedFloor = 0;

                elevator.addToQueue(requestedFloor);
                elevator.closeDoors();
                elevator.move();
            }
        }
        public class Column {
            public List<ExternalButton> externalButtonsList = new List<ExternalButton>();
            public List<Elevator> elevatorsList = new List<Elevator>();
            public Column(int floors, int elevators) {
                for (int i = 0; i < elevators; i++) {
                    this.elevatorsList.Add(new Elevator(0, floors));
                }
                for (int i = 0; i < floors; i++) {
                    if (i == 0) {
                        this.externalButtonsList.Add(new ExternalButton(i, "up"));
                    }else {
                        this.externalButtonsList.Add(new ExternalButton(i, "up"));
                        this.externalButtonsList.Add(new ExternalButton(i, "down"));
                    }
                }
            }
        }

        public class Elevator {
            public string direction;
            public int currentFloor;
            public string status;
            public List<int> queue;
            public List<InternalButton> internalButtonsList;
            public string door;
            public Elevator(int currentFloor, int floors) {
                this.direction = null;
                this.currentFloor = currentFloor;
                this.status = "idle";
                this.queue = new List<int>();
                this.internalButtonsList = new List<InternalButton>();
                this.door = "closed";

                for (int i = 0; i < floors; i++) {
                    this.internalButtonsList.Add(new InternalButton());
                }
            }
            public void addToQueue(int requestedFloor) {
                this.queue.Add(requestedFloor);

                if (this.direction == "up") {
                    this.queue.Sort((x, y) => x.CompareTo(y));
                }
                if (this.direction == "down") {
                    this.queue.Sort((x, y) => y.CompareTo(x));
                }

                Console.WriteLine("Added floor " + requestedFloor + " to the elevator's queue. Current queue: " + String.Join(", ", this.queue));
            }
            public void move() {
                Console.WriteLine("Moving elevator");
                while (this.queue.Count > 0) {

                    int firstElement = this.queue[0];

                    if (this.door == "open") {
                        Console.WriteLine("Waiting 7 seconds for the doorway to be cleared");
                        this.closeDoors();
                    }
                    if (firstElement == this.currentFloor) {
                        this.queue.RemoveAt(0);
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
                if (this.queue.Count == 0) {
                    Console.WriteLine("Waiting 7 seconds for the doorway to be cleared");
                    this.closeDoors();
                    Console.WriteLine("Elevator is now idle");
                    this.status = "idle";
                }
            }
            public void moveUp() {
                this.currentFloor++;
                Console.WriteLine("^^^ Elevator on floor " + this.currentFloor.ToString().Replace("-", "basement "));
            }

            public void moveDown() {
                this.currentFloor--;
                Console.WriteLine("vvv Elevator on floor " + this.currentFloor.ToString().Replace("-", "basement "));
            }

            public void openDoors() {
                    this.door = "open";
                    Console.WriteLine("<> Opened doors");
            }

            public void closeDoors() {
                    this.door="closed";
                    Console.WriteLine(">< Closed doors");
            }
        }

        public class ExternalButton {

            public int requestFloor;
            public string direction;
            public ExternalButton(int requestFloor, string direction) {
                this.requestFloor = requestFloor;
                this.direction = direction;
            }
        }

        public class InternalButton {
            public int floor;
            public InternalButton() {
                this.floor = 0;
            }
        }

        //------------------------------------------------------- TESTING SECTION --------------------------------------------------------


        public static void Main() {
            Console.WriteLine("--------------------------------------- TEST #1 ------------------------------------------------------\n\n");
            Test1_requestElevator();
            Console.WriteLine("\n\n--------------------------------------- TEST #2 ------------------------------------------------------\n\n");
            Test2_requestFloor();
        }
        public static void Test1_requestElevator() {

            var battery1 = new Battery(3, 100, 10, 4);

            battery1.columnsList[0].elevatorsList[0].currentFloor = 2;
            battery1.columnsList[0].elevatorsList[0].direction = "up";
            battery1.columnsList[0].elevatorsList[0].status = "moving";
            battery1.columnsList[0].elevatorsList[0].queue.AddRange(new List<int>() {2, 5});

            battery1.columnsList[0].elevatorsList[1].currentFloor = 3;
            battery1.columnsList[0].elevatorsList[1].direction = "down";
            battery1.columnsList[0].elevatorsList[1].status = "moving";
            battery1.columnsList[0].elevatorsList[1].queue.AddRange(new List<int>() {3, 5, 8});

            battery1.columnsList[0].elevatorsList[2].currentFloor = 38;
            battery1.columnsList[0].elevatorsList[2].direction = "up";
            battery1.columnsList[0].elevatorsList[2].status = "moving";
            battery1.columnsList[0].elevatorsList[2].queue.AddRange(new List<int>() {-1, 3, 6});

            battery1.columnsList[0].elevatorsList[3].currentFloor = 38;
            battery1.columnsList[0].elevatorsList[3].direction = "up";
            battery1.columnsList[0].elevatorsList[3].status = "moving";
            battery1.columnsList[0].elevatorsList[3].queue.AddRange(new List<int>() {-1, 5, 8});

            battery1.columnsList[1].elevatorsList[0].currentFloor = 5;
            battery1.columnsList[1].elevatorsList[0].direction = null;
            battery1.columnsList[1].elevatorsList[0].status = "idle";
            battery1.columnsList[1].elevatorsList[0].queue.AddRange(new List<int>() {});

            battery1.columnsList[1].elevatorsList[1].currentFloor = 6;
            battery1.columnsList[1].elevatorsList[1].direction = "up";
            battery1.columnsList[1].elevatorsList[1].status = "moving";
            battery1.columnsList[1].elevatorsList[1].queue.AddRange(new List<int>() {1, 2, 3, 4});

            battery1.columnsList[1].elevatorsList[2].currentFloor = 7;
            battery1.columnsList[1].elevatorsList[2].direction = "down";
            battery1.columnsList[1].elevatorsList[2].status = "moving";
            battery1.columnsList[1].elevatorsList[2].queue.AddRange(new List<int>() {-3, 2, 7, 8});

            battery1.columnsList[1].elevatorsList[3].currentFloor = 7;
            battery1.columnsList[1].elevatorsList[3].direction = "down";
            battery1.columnsList[1].elevatorsList[3].status = "moving";
            battery1.columnsList[1].elevatorsList[3].queue.AddRange(new List<int>() {-3, 2, 7, 8});

            battery1.columnsList[2].elevatorsList[0].currentFloor = 6;
            battery1.columnsList[2].elevatorsList[0].direction = "down";
            battery1.columnsList[2].elevatorsList[0].status = "moving";
            battery1.columnsList[2].elevatorsList[0].queue.AddRange(new List<int>() {2, 4});

            battery1.columnsList[2].elevatorsList[1].currentFloor = 9;
            battery1.columnsList[2].elevatorsList[1].direction = null;
            battery1.columnsList[2].elevatorsList[1].status = "idle";
            battery1.columnsList[2].elevatorsList[1].queue.AddRange(new List<int>() {});

            battery1.columnsList[2].elevatorsList[2].currentFloor = 38;
            battery1.columnsList[2].elevatorsList[2].direction = "up";
            battery1.columnsList[2].elevatorsList[2].status = "moving";
            battery1.columnsList[2].elevatorsList[2].queue.AddRange(new List<int>() {4, 6, 7});

            battery1.columnsList[1].elevatorsList[3].currentFloor = 7;
            battery1.columnsList[1].elevatorsList[3].direction = "down";
            battery1.columnsList[1].elevatorsList[3].status = "moving";
            battery1.columnsList[1].elevatorsList[3].queue.AddRange(new List<int>() {-3, 2, 7, 8});

            battery1.requestElevator(9, "up");
        }

        public static void Test2_requestFloor(){
            var battery2 = new Battery(3, 3, 3, 4);

            battery2.columnsList[0].elevatorsList[0].currentFloor = 2;
            battery2.columnsList[0].elevatorsList[0].direction = "up";
            battery2.columnsList[0].elevatorsList[0].status = "moving";
            battery2.columnsList[0].elevatorsList[0].queue.AddRange(new List<int>() {2, 5});

            battery2.columnsList[0].elevatorsList[1].currentFloor = 3;
            battery2.columnsList[0].elevatorsList[1].direction = "down";
            battery2.columnsList[0].elevatorsList[1].status = "moving";
            battery2.columnsList[0].elevatorsList[1].queue.AddRange(new List<int>() {3, 5, 8});

            battery2.columnsList[0].elevatorsList[2].currentFloor = 38;
            battery2.columnsList[0].elevatorsList[2].direction = "up";
            battery2.columnsList[0].elevatorsList[2].status = "moving";
            battery2.columnsList[0].elevatorsList[2].queue.AddRange(new List<int>() {-1, 3, 6});

            battery2.columnsList[0].elevatorsList[3].currentFloor = 38;
            battery2.columnsList[0].elevatorsList[3].direction = "up";
            battery2.columnsList[0].elevatorsList[3].status = "moving";
            battery2.columnsList[0].elevatorsList[3].queue.AddRange(new List<int>() {-1, 5, 8});

            battery2.columnsList[1].elevatorsList[0].currentFloor = 5;
            battery2.columnsList[1].elevatorsList[0].direction = null;
            battery2.columnsList[1].elevatorsList[0].status = "idle";
            battery2.columnsList[1].elevatorsList[0].queue.AddRange(new List<int>() {});

            battery2.columnsList[1].elevatorsList[1].currentFloor = 6;
            battery2.columnsList[1].elevatorsList[1].direction = "up";
            battery2.columnsList[1].elevatorsList[1].status = "moving";
            battery2.columnsList[1].elevatorsList[1].queue.AddRange(new List<int>() {1, 2, 3, 4});

            battery2.columnsList[1].elevatorsList[2].currentFloor = 7;
            battery2.columnsList[1].elevatorsList[2].direction = "down";
            battery2.columnsList[1].elevatorsList[2].status = "moving";
            battery2.columnsList[1].elevatorsList[2].queue.AddRange(new List<int>() {-3, 2, 7, 8});

            battery2.columnsList[1].elevatorsList[3].currentFloor = 7;
            battery2.columnsList[1].elevatorsList[3].direction = "down";
            battery2.columnsList[1].elevatorsList[3].status = "moving";
            battery2.columnsList[1].elevatorsList[3].queue.AddRange(new List<int>() {-3, 2, 7, 8});

            battery2.columnsList[2].elevatorsList[0].currentFloor = 6;
            battery2.columnsList[2].elevatorsList[0].direction = "down";
            battery2.columnsList[2].elevatorsList[0].status = "moving";
            battery2.columnsList[2].elevatorsList[0].queue.AddRange(new List<int>() {2, 4});

            battery2.columnsList[2].elevatorsList[1].currentFloor = 9;
            battery2.columnsList[2].elevatorsList[1].direction = null;
            battery2.columnsList[2].elevatorsList[1].status = "idle";
            battery2.columnsList[2].elevatorsList[1].queue.AddRange(new List<int>() {});

            battery2.columnsList[2].elevatorsList[2].currentFloor = 38;
            battery2.columnsList[2].elevatorsList[2].direction = "up";
            battery2.columnsList[2].elevatorsList[2].status = "moving";
            battery2.columnsList[2].elevatorsList[2].queue.AddRange(new List<int>() {4, 6, 7});

            battery2.columnsList[1].elevatorsList[3].currentFloor = 7;
            battery2.columnsList[1].elevatorsList[3].direction = "down";
            battery2.columnsList[1].elevatorsList[3].status = "moving";
            battery2.columnsList[1].elevatorsList[3].queue.AddRange(new List<int>() {-3, 2, 7, 8});

            Elevator elevator = battery2.columnsList[1].elevatorsList[2];

            battery2.requestFloor(elevator);
        }
    }
}