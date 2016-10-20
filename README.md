# Qlik Manufacturing Analytics
VR example of using Qlik Sense with Streaming Data

This Unity application will display a factory simulation with streaming data into Qlik Sense. 
It has been designed and built for the Oculus Rift. 
When a user starts the application they will see a factory with widgets moving along conveyor belts. 
The widgets will show the current sensor readings.
Along the wall in front of the user, the current sensor status code is shown. There is also a control panel which can be used to stop & start the conveyor belts.
In the corner on the wall behind the user is a Qlik Sense web mashup showing 3 charts: 
- On the top, users can see realtime streaming sensor data
- In the middle, users can see all the sensor readings for the current widget
- On the bottom, users can see all of the historical sensor values

[![Video documentation of Manufacturing Analytics](https://img.youtube.com/vi/VwbG20pvPzY/0.jpg)](https://www.youtube.com/watch?v=VwbG20pvPzY)

## Requirements
- *Used in conjunction with  [Qlik Lambda Package Node Server](https://github.com/ImmersiveAnalytics/LambdaPackage)*
- Must have Qlik Sense Desktop or Server application running with Node.js middleware app
- Must have Unity 5.3.4p1 installed

## To Run
Either play from the Unity Editor or build a standalone app for the Oculus Rift.
*Tested with DK2 and the first Consumer release*
