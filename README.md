# Rocket_Elevators_Controllers - CodeBoxx Odyssey project


Remove the "//" or "#" sign in front of `Test1_requestElevator()`, `Test2_requestFloor()` and both console logs for a simulation with detailed console logs

Elevators' attributes can also be redefined inside the source code;

JavaScript
---------------------------------------
```javascript
function Test1_requestElevator() {

	column1 = new Column(10, 2);

	column1.elevatorsList[0].currentFloor = 6;
  	column1.elevatorsList[0].direction = "up";
  	column1.elevatorsList[0].status = "moving";
	column1.elevatorsList[0].queue = [4, 6, 7];

	column1.elevatorsList[1].currentFloor = 1;
	column1.elevatorsList[1].direction = null;
	column1.elevatorsList[1].status = "idle";
	column1.elevatorsList[1].queue = [];

  //Call the function within the Collumn class "requestElevator(requestedFloor, direction)"
	column1.requestElevator(4, "up");
}

Test1_requestElevator()
```

Python
---------------------------------------
```python
def Test1_requestElevator():
    column1 = Column(10, 2)

    column1.elevatorsList[0].currentFloor = 6
    column1.elevatorsList[0].direction  =  "up"
    column1.elevatorsList[0].status =  "moving"
    column1.elevatorsList[0].queue = [1,5,8]

    column1.elevatorsList[1].currentFloor = 3
    column1.elevatorsList[1].direction  =  "down"
    column1.elevatorsList[1].status =  "moving"
    column1.elevatorsList[1].queue = [2,1]

  #Call the function within the Collumn class "requestElevator(requestedFloor, direction)"
    column1.requestElevator(4, "down")
 
 Test1_requestElevator()
```
