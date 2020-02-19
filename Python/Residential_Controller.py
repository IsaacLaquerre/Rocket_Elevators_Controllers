class Column():

    def __init__(floors, elevators):
        self.floors = floors
        self.elevators = elevators
        self.externalButtonsList = []
        self.elevatorsList = []
        for i in range(elevators):
            self.elevatorsList.append(Elevator(0, floors))
        for i in range(floors):
            if i == 0:
                self.externalButtonsList.append(ExternalButton(i, "up", false))
            else:
                self.externalButtonsList.append(ExternalButton(i, "up", false))

    def findElevator(requestedFloor, direction):

        bestGap = self.floors
        chosenElevator = None

        for elevator in range(self.elevatorsList):
            if elevator.direction == "up" && direct)on == "up" && requestedFloor < elevator.currentFloor:
                chosenElevator = elevator
            else if elevator.direction == "down" && directon == "down" && requestedFloor > elevator.currentFloor:
                chosenElevator = elevator
            else if elevator.status == "idle":
                chosenElevator = elevator
            else:
                for elevator in range(self.elevatorsList):
                    gap = #*MATH ABSOLUTE NUMBER OF (elevator.currentFloor - requestedFloor)*#
                    if gap < bestGap:
                        chosenElevator = elevator
                        bestGap = gap
        print("Best elevator found on floor " + chosenElevator.currentFloor)
        return chosenElevator

    def requestElevator(requestedFloor, direction):
        print("Called an elevator to the floor " + requestedFloor)

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
    def __init__(currentFloor, floors):
        self.direction =  null
		self.floors = floors
		self.currentFloor = currentFloor
		self.status = "idle"
		self.queue = []
		self.internalButtonsList = []
		self.door = "closed"

        for i in range(self.floors):
            self.internalButtonsList.append(InternalButton(i, false))

    def addToQueue(requestedFloor):
        self.queue.append(requestedFloor)

		if self.direction == "up":
			self.queue.sort((a, b) => a - b)
		if self.direction == "down":
			self.queue.sort((a, b) => b - a)

		print("Added floor " + requestedFloor + " to the elevator's queue. Current queue: " + self.queue.join(", "))

    def move():
        print ("Moving elevator")
        while self.queue.length > 0:

            firstElement = self.queue[0]

            if self.door == "open":
                print("Waiting 7 seconds for the doorway to be cleared")
                self.closeDoors()
                if firstElement == self.currentFloor:
                    self.queue.shift()
                    self.openDoors()
                else if firstElement > self.currentFloor:
                    self.status = "moving"
                    self.direction = "up"
                    self.moveUp()
                else if firstElement < self.currentFloor:
                    self.status = "moving"
                    self.direction = "down"
                    self.moveDown()
        if self.queue.length == 0:
			print("Waiting 7 seconds for the doorway to be cleared")
			self.closeDoors()
			print("Elevator is now idle")
			self.status = "idle"

    def moveUp():
        self.currentFloor++
		print("^^^ Elevator on floor " + self.currentFloor)

	def moveDown():
		self.currentFloor--
		print("vvv Elevator on floor " + self.currentFloor)

	def openDoors():
			self.door = "open"
			print("<> Opened doors")

	def closeDoors():
			self.door="closed"
			print(">< Closed doors")

class ExternalButton():
    def __init__(requestFloor, direction):
        self.requestFloor = requestFloor
        self.direction = direction

class InternalButton():
    def __init__(floor):
        self.floor = floor