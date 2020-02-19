class Battery():
    def __init__(self, columns, floors, basements, elevatorsPerColumn):
        self.columns = columns
        self.floors = floors
        self.basements = basements
        self.columnsList = []

        for i in range(self.columns):
            self.columnsList.append(Column(self.floors, elevatorsPerColumn))

    def decideColumn(self, requestedFloor):
        floorsPerColumn = round(self.floors / self.columns)
        for index in len(self.columnsList):
            for i in len(self.columnsList[index].elevatorsList):
                if requestedFloor > ((floorsPerColumn * index) - floorsPerColumn) and requestedFloor < ((floorsPerColumn * index) + floorsPerColumn):
                    print("Chosen column #" + str(index + 1))
                    return index

    def findElevator(self, requestedFloor, directon, column):
        column -= 1

        chosenElevator = None
        bestGap = self.floors

        for elevator in self.elevatorsList:
            if elevator.direction == "up" and direction == "up" and requestedFloor > elevator.currentFloor:
                for i in len(self.columnsList[column].elevatorsList):
                    if elevator.direction != "up":
                        continue
                    gap = abs(elevator.currentFloor - requestedFloor)
                    if gap < bestGap:
                        chosenElevator = elevator
                        bestGap = gap
            elif elevator.direction == "down" and direction == "down" and requestedFloor < elevator.currentFloor:
                for i in len(self.columnsList[column].elevatorsList):
                    if elevator.direction != "down":
                        continue
                    gap = abs(elevator.currentFloor - requestedFloor)
                    if gap < bestGap:
                        chosenElevator = elevator
                        bestGap = gap
            elif elevator.status == "idle":
                chosenElevator = elevator
            else:
                for i in len(self.columnsList[column].elevatorsList):
                    gap = abs(elevator.currentFloor - requestedFloor)
                    if gap < bestGap:
                        chosenElevator = elevator
                        bestGap = gap
            print("Best elevator found on floor " + elevator.currentFloor)
            return chosenElevator

    def requestElevator(self, requestedFloor, direction):
        if requestedFloor < -abs(self.basements) or requestedFloor > (self.floors - self.basements):
            return print("Floor " + str(requestedFloor) + " doesn't exist!")

        column = self.decideColumn(requestedFloor)

        print("Called an elevator to the floor " + str(requestedFloor) + " in the collumn #" + str(column + 1))

        elevator = self.findElevator(requestedFloor, direction, column)

        elevator.addToQueue(requestedFloor)
        elevator.move()
        return elevator

    def requestFloor(self, elevator):
        print("Moving elevator on floor " + str(elevator.currentFloor) + " to the ground floor")

        requestedFloor = 0

        elevator.addToQueue(requestedFloor)
        elevator.closeDoors()
        elevator.move()

class Column():
    def __init__(self, floors, elevators):
        self.externalButtonList = []
        self.elevatorsList = []
        for i in range(elevators):
            self.elevatorsList.append(Elevator(0, floors))
            for i in range(floors):
                if i == 0:
                    self.externalButtonList.append(ExternalButton(i, "up"))
                else:
                    self.externalButtonList.append(ExternalButton(i, "up"))
                    self.externalButtonList.append(ExternalButton(i, "down"))

class Elevator():
    def __init__(self, currentFloor, floors):
        self.direction = None
        self.currentFloor = currentFloor
        self.status = "idle"
        self.queue = []
        self.internalButtonsList = []
        self.door = "closed"

        self.internalButtonsList.append(InternalButton())

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
                    del self.queue[0]
                    self.openDoors()
                elif firstElement > self.currentFloor:
                    self.status = "moving"
                    self.direction = "up"
                    self.moveUp()
                elif firstElement < self.currentFloor:
                    self.status = "moving"
                    self.direction = "down"
                    self.moveDown()
            print("Waiting 7 seconds for the doorway to be cleared")
            self.closeDoors()
            print("Elevator is now idle")
            self.status = "idle"

    def moveUp(self):
        self.currentFloor += 1
        print("^^^ Elevator on floor " + str(self.currentFloor))

    def moveDown(self):
        self.currentFloor -= 1
        print("vvv Elevator on floor " + str(self.currentFloor))

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
    def __init__(self):
        self.floor = 0


### -------------------------- TESTING SECTION -------------------------- ###


#print("--------------------------------------- TEST #1 ------------------------------------------------------\n\n")

def Test1_requestElevator():
    battery1 = Battery(3, 100, 10, 4)

    battery1.columnsList[0].elevatorsList[0].currentFloor = 5
    battery1.columnsList[0].elevatorsList[0].direction = "up"
    battery1.columnsList[0].elevatorsList[0].status = "moving"
    battery1.columnsList[0].elevatorsList[0].queue = [1,2]

    battery1.columnsList[0].elevatorsList[1].currentFloor = 3
    battery1.columnsList[0].elevatorsList[1].direction = "down"
    battery1.columnsList[0].elevatorsList[1].status = "moving"
    battery1.columnsList[0].elevatorsList[1].queue = [3,7,8]

    battery1.columnsList[0].elevatorsList[2].currentFloor = 38
    battery1.columnsList[0].elevatorsList[2].direction = "up"
    battery1.columnsList[0].elevatorsList[2].status = "moving"
    battery1.columnsList[0].elevatorsList[2].queue = [-1,5,8]

    battery1.columnsList[0].elevatorsList[3].currentFloor = 38
    battery1.columnsList[0].elevatorsList[3].direction = "up"
    battery1.columnsList[0].elevatorsList[3].status = "moving"
    battery1.columnsList[0].elevatorsList[3].queue = [-1,5,8]

    battery1.columnsList[1].elevatorsList[0].currentFloor = 5
    battery1.columnsList[1].elevatorsList[0].direction = None
    battery1.columnsList[1].elevatorsList[0].status = "idle"
    battery1.columnsList[1].elevatorsList[0].queue = []

    battery1.columnsList[1].elevatorsList[1].currentFloor = 6
    battery1.columnsList[1].elevatorsList[1].direction = "up"
    battery1.columnsList[1].elevatorsList[1].status = "moving"
    battery1.columnsList[1].elevatorsList[1].queue = [1,2,3,4]

    battery1.columnsList[1].elevatorsList[2].currentFloor = 7
    battery1.columnsList[1].elevatorsList[2].direction = "down"
    battery1.columnsList[1].elevatorsList[2].status = "moving"
    battery1.columnsList[1].elevatorsList[2].queue = [-3,2,7,8]

    battery1.columnsList[1].elevatorsList[3].currentFloor = 7
    battery1.columnsList[1].elevatorsList[3].direction = "down"
    battery1.columnsList[1].elevatorsList[3].status = "moving"
    battery1.columnsList[1].elevatorsList[3].queue = [-3,2,7,8]

    battery1.columnsList[2].elevatorsList[0].currentFloor = 6
    battery1.columnsList[2].elevatorsList[0].direction = "down"
    battery1.columnsList[2].elevatorsList[0].status = "moving"
    battery1.columnsList[2].elevatorsList[0].queue = [1,4]

    battery1.columnsList[2].elevatorsList[1].currentFloor = 9
    battery1.columnsList[2].elevatorsList[1].direction = None
    battery1.columnsList[2].elevatorsList[1].status = "idle"
    battery1.columnsList[2].elevatorsList[1].queue = []

    battery1.columnsList[2].elevatorsList[2].currentFloor = 38
    battery1.columnsList[2].elevatorsList[2].direction = "up"
    battery1.columnsList[2].elevatorsList[2].status = "moving"
    battery1.columnsList[2].elevatorsList[2].queue = [4, 6, 7]

    battery1.columnsList[1].elevatorsList[3].currentFloor = 7
    battery1.columnsList[1].elevatorsList[3].direction = "down"
    battery1.columnsList[1].elevatorsList[3].status = "moving"
    battery1.columnsList[1].elevatorsList[3].queue = [-3,2,7,8]

    battery1.requestElevator(99, "down")

#Test1_requestElevator()

#print("\n\n--------------------------------------- TEST #2 ------------------------------------------------------\n\n")

def Test2_requestFloor():
    battery2 = Battery(3, 3, 3, 4)

    battery2.columnsList[0].elevatorsList[0].currentFloor = 2
    battery2.columnsList[0].elevatorsList[0].direction = "up"
    battery2.columnsList[0].elevatorsList[0].status = "moving"
    battery2.columnsList[0].elevatorsList[0].queue = [-3,4,6,7]

    battery2.columnsList[0].elevatorsList[1].currentFloor = 2
    battery2.columnsList[0].elevatorsList[1].direction = "down"
    battery2.columnsList[0].elevatorsList[1].status = "moving"
    battery2.columnsList[0].elevatorsList[1].queue = [3,7,8]

    battery2.columnsList[0].elevatorsList[2].currentFloor = 2
    battery2.columnsList[0].elevatorsList[2].direction = "up"
    battery2.columnsList[0].elevatorsList[2].status = "moving"
    battery2.columnsList[0].elevatorsList[2].queue = [-1,5,8]

    battery2.columnsList[1].elevatorsList[0].currentFloor = 2
    battery2.columnsList[1].elevatorsList[0].direction = None
    battery2.columnsList[1].elevatorsList[0].status = "idle"
    battery2.columnsList[1].elevatorsList[0].queue = []

    battery2.columnsList[1].elevatorsList[1].currentFloor = 2
    battery2.columnsList[1].elevatorsList[1].direction = "up"
    battery2.columnsList[1].elevatorsList[1].status = "moving"
    battery2.columnsList[1].elevatorsList[1].queue = [1,2,3,4]

    battery2.columnsList[1].elevatorsList[2].currentFloor = 2
    battery2.columnsList[1].elevatorsList[2].direction = "down"
    battery2.columnsList[1].elevatorsList[2].status = "moving"
    battery2.columnsList[1].elevatorsList[2].queue = [-3,2,7,8]

    battery2.columnsList[2].elevatorsList[0].currentFloor = 2
    battery2.columnsList[2].elevatorsList[0].direction = "down"
    battery2.columnsList[2].elevatorsList[0].status = "moving"
    battery2.columnsList[2].elevatorsList[0].queue = [1,4]

    battery2.columnsList[2].elevatorsList[1].currentFloor = 2
    battery2.columnsList[2].elevatorsList[1].direction = None
    battery2.columnsList[2].elevatorsList[1].status = "idle"
    battery2.columnsList[2].elevatorsList[1].queue = []

    battery2.columnsList[2].elevatorsList[2].currentFloor = 2
    battery2.columnsList[2].elevatorsList[2].direction = "up"
    battery2.columnsList[2].elevatorsList[2].status = "moving"
    battery2.columnsList[2].elevatorsList[2].queue = [4, 6, 7]

    elevator = battery2.columnsList[1].elevatorsList[2]

    battery2.requestFloor(elevator)

#Test2_requestFloor()