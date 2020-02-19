class Column():

    def __init__(self, floors, elevators):
        self.floors = floors
        self.elevators = elevators
        self.externalButtonsList = []
        self.elevatorsList = []
        for i in range(elevators):
            self.elevatorsList.append(Elevator(0, floors))
        for i in range(floors):
            if i == 0:
                self.externalButtonsList.append(ExternalButton(i, "up"))
            else:
                self.externalButtonsList.append(ExternalButton(i, "up"))
                self.externalButtonsList.append(ExternalButton(i, "down"))

    def findElevator(self, requestedFloor, direction):

        bestGap = self.floors
        chosenElevator = None

        for elevator in self.elevatorsList:
            if elevator.direction == "up" and direction == "up" and requestedFloor < elevator.currentFloor:
                chosenElevator = elevator
            elif elevator.direction == "down" and direction == "down" and requestedFloor > elevator.currentFloor:
                chosenElevator = elevator
            elif elevator.status == "idle":
                chosenElevator = elevator
            else:
                for elevator in self.elevatorsList:
                    gap = abs(elevator.currentFloor - requestedFloor)
                    if gap < bestGap:
                        chosenElevator = elevator
                        bestGap = gap
        print("Best elevator found on floor " + str(chosenElevator.currentFloor))
        return chosenElevator

    def requestElevator(self, requestedFloor, direction):
        print("Called an elevator to the floor " + str(requestedFloor))

        elevator = self.findElevator(requestedFloor, direction)

        elevator.addToQueue(requestedFloor)
        elevator.move()
        return elevator

    def requestFloor(elevator, requestedFloor):
        print("Moving elevator on floor " + elevator.currentFloor + " to the floor " + requestedFloor)
        elevator.addToQueue(requestedFloor)
        elevator.closeDoors()
        elevator.move()

class Elevator():
    def __init__(self, currentFloor, floors):
        self.direction =  None
        self.floors = floors
        self.currentFloor = currentFloor
        self.status = "idle"
        self.queue = []
        self.internalButtonsList = []
        self.door = "closed"

        for i in range(self.floors):
            self.internalButtonsList.append(InternalButton(i))

    def addToQueue(self, requestedFloor):
        self.queue.append(requestedFloor)

        if self.direction == "up":
            self.queue.sort(reverse=True)
        if self.direction == "down":
            self.queue.sort(reverse=True)

        print("Added floor " + str(requestedFloor) + " to the elevator's queue. Current queue: " + ', '.join(str(x) for x in self.queue))

    def move(self):
        print ("Moving elevator")
        while len(self.queue) > 0:

            firstElement = self.queue[0]

            if self.door == "open":
                print("Waiting 7 seconds for the doorway to be cleared")
                self.closeDoors()
                if firstElement == self.currentFloor:
                    self.queue.pop(0)
                    self.openDoors()
                elif firstElement > self.currentFloor:
                    self.status = "moving"
                    self.direction = "up"
                    self.moveUp()
                elif firstElement < self.currentFloor:
                    self.status = "moving"
                    self.direction = "down"
                    self.moveDown()
        if len(self.queue) == 0:
            print("Waiting 7 seconds for the doorway to be cleared")
            self.closeDoors()
            print("Elevator is now idle")
            self.status = "idle"

    def moveUp(self):
        self.currentFloor = self.currentFloor + 1
        print("^^^ Elevator on floor " + self.currentFloor)

    def moveDown(self):
        self.currentFloor = self.currentFloor - 1
        print("vvv Elevator on floor " + self.currentFloor)

    def openDoors(self):
        self.door = "open"
        print("<> Opened doors")

    def closeDoors(self):
        self.door="closed"
        print(">< Closed doors")

class ExternalButton():
    def __init__(self, requestFloor, direction):
        self.requestFloor = requestFloor
        self.direction = direction

class InternalButton():
    def __init__(self, floor):
        self.floor = floor

print("--------------------------------------- TEST #1 -------------------------------------------------------------\n\n")

def Test1_requestElevator():
	column1 = Column(10, 2)

	column1.elevatorsList[0].currentFloor = 1
	column1.elevatorsList[0].direction  =  "up"
	column1.elevatorsList[0].status =  "moving"
	column1.elevatorsList[0].queue = [3,5]

	column1.elevatorsList[1].currentFloor = 6
	column1.elevatorsList[1].direction  =  "down"
	column1.elevatorsList[1].status =  "moving"
	column1.elevatorsList[1].queue = [6,2]

	column1.requestElevator(7, "up")

Test1_requestElevator()

print("\n\n--------------------------------------- TEST #2 -------------------------------------------------------------\n\n")

def Test2_requestFloor():
	column2 = Column(10, 2)

	column2.elevatorsList[0].currentFloor = 2
	column2.elevatorsList[0].direction  =  "up"
	column2.elevatorsList[0].status =  "moving"
	column2.elevatorsList[0].queue = [3,4]

	elevator = column2.elevatorsList[0]

	column2.requestFloor(elevator, 9)
Test2_requestFloor()