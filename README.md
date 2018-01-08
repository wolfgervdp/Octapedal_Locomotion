# Octapedal Locomotion

In this work, existing techniques in the field of genetic algorithms are explored and applied to a problem in which an octapedal 2-dimensional robot with eight legs learns how to move forward. The robot learns how to move forward by just extending and retracting the right legs at the right time. We show that it is certainly possible to let the robot move at a decent speed.

## Setup

The project was made in [Unity](https://unity3d.com/unity). The editor is necessary to open open and run the project. When unity is installed, the folder "Unity Project" serves as project folder. 

## Usage

When opening Unity, pressing the play button will start the simulator. There is an onscreen gui to adjust population size at runtime. This gui also allows to save a population and the all-time best individual to file. The statistics of the population over the lifetime of the simulator can be written to file too. Last, the simulator can be paused from the gui as well. Changing simulation speed can be done with alphanumerical keys 1,2 and 3. These respectively multiply the speed with x2, x5 and x10. 

Most of the parameter values can be changed in the inspector of the God object. The God object can be found at the left hand panel. When opening the inspector (right hand panel), it's possible to change values for the initial population, elitism rates, etc. Some parameters can only be changed in the code, such as the operators which are used. For a guide on how to do this, please refer to "Changing the code" lower on this page.

## Changing the code
The main class that'll need to be adjusted when trying to update the code is the God class. This is the backbone of the application. The God class oversees instantiation, removal and evaluation of the robots. In this class it's also specified which operators are used. 
