
Live Streaming Genie is designed to work with a 2 Stream Setup.

1. From This PC, present your PowerPoint and run Live Stream Genie
2. From your Streaming PC, Setup OBS and turn on OBS Web Socket Server port 4455 and set the password

When you start Live Streaming Genie:
	1. Right Click Live Streaming Genie icon -> Settings to update the password
	2. Right Click Live Streaming Genie icon -> Reconnet to reconnect to OBS

Live Streaming Genie uses the new OBS Web Socket Server 5.x that is shipped with OBS OBS 29.x

Live Streaming Genie is based off Scot Hanselman's [PowerPointToOBSSceneSwitcher](https://github.com/shanselman/PowerPointToOBSSceneSwitcher) which I have been using for years, but just needs that little big of magic to remove friction.

This App has so much potential.

Supported Commands:

* OBS: - switch to named scene
* **Start - Start Recording
* **STOP - Stop Recording
* OBSDEF: - set the OBS default scene if a scene doesn't exist


# Notification
 
Make sure you turn off nofication sounds or it will beep every time you get a notification

![](./turn-off-sounds.png)

To see the notifications when running Powerpoint in presentation mode, you must also allow notifications in full screen node. To do this go to:

Windows + I  (Settings)

System

Focus Assist

"When I'm using an App in Full Screen Mode"

Turn Off

![](./focus-assist.png)


# Technical Design

This C# code creates an instance of the ApplicationContext class and starts a message loop by calling the Application.Run method, which runs the message loop on the ApplicationContext instance.

While this is running, the main Program class is listening out for events from PowerPoint and sending relevant commands to the App Context for processing.

An ApplicationContext is a class that manages the lifetime of a Windows Forms application. It provides a context in which forms can be shown and closed, and allows you to define application-level behavior, such as handling unhandled exceptions and controlling the shutdown of the application.

The Application.Run method starts a message loop that processes messages from the operating system and dispatches them to the appropriate forms and controls in the application. This message loop is responsible for handling user input, redrawing the user interface, and executing timers and other asynchronous operations.

Overall, this code is a standard way to start a Windows Forms application and run its message loop, which enables the application to handle user input and interact with the operating system.

