# NetworksCourseProject
Project consist of 2 programms, the first one is DataHandler, it is receiving data from the second program - Simulate.
## Simulate
This program is imitating process sensor.
Before starting, you must to put ip and port of the socket you want to connect to. IP must be v4 and entered as 4 decimal numbers.
After that you must enter the expected value and then data will be raising before it reaches this number. Then it will be fluctuating near it.

If program fails to connect, it will stop. If connection is lost during work, program will stop.

## DataHandler
This program is receiving data and drawing the charts.
Scatter points of recieved data and also 2 linecharts of regression functions:
- linear;
- logarithmic.

Regression charts is only available after at least 6 recieved points from Simulate program.
User can stop receiving by clicking stop button, it will send signal to simulate program to shutdown.

Also user can choose to not receive points via socket and build charts from coordinates in file.
Data in file must be in the following format: x1 y1;x2 y2; ...
If format is incorrect the message will be showed.
