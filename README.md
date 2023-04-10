<h1 align="center">🚗🚧 Car Racing Console Simulator 🚩</h1>

<p align = center>
by <a href="https://github.com/Cecilia-Coutinho">Cecilia Coutinho</a>
</p>

<h2>🌍 Overview</h2>

<p align = center>
The Car Racing Console Simulator is designed to run races with multiple cars over a set distance while tracking the time spent by each car to complete the distance. The system includes random events that may occur during the race to add excitement and unpredictability. Each car runs on its own thread to enable parallel processing and improve performance. The simulation is console-based only and does not include graphics.
</p>

<h2>📋 MVP</h2>
<p>
The Minimum Viable Product(MVP) of the Car Racing Console Simulator includes the following features:
</p>

>🎯 Simulation of a race with multiple cars.

>🎯 Simulation of random events that may occur during the race.

>🎯 Implementation of logic for each car to run on its own thread.

>🎯 Tracking of the time spent by each car and identification of the winner.

>🎯 Printing of status updates in console when a car experiences a problem or reaches the finish line.


<h2>💻 Technology Stack</h2>

>📌 <b>C#</b>: primary language.

>📌 <b>.NET6</b>: software framework.

>📌 <b>Git</b>: version control.

>📌 <b>Visual Studio</b>: IDE.

<h2>📏 Project Methodology</h2>

>📝 Kanban

<h3>💭Reflections or Aditional Info</h3>
<p>
The code simulates a simple race between multiple cars and demonstrates the use of asynchronous programming, as well as using C# interfaces to produce events' behaviour. It's a functional console-based car racing simulation system with no graphics. 
</p>
<p>
The program uses asynchronous programming concepts to start the races concurrently and simulate different events that can happen during the race. The simulation of each event is defined in classes that implement the IEvent interface. The race simulation is performed in the StartRace() method, which calculates the time it takes to finish the race based on the distance and speed of the car and simulates different events by calling the Apply() method of a random event from the RandEvents list.
</p>
<p>
The winner of the race is determined by comparing the Elapsed Time property of the Race object of each car. The DefineWinner() method is called after all races are completed, and it determines which car finished first and prints the result.
</p>
