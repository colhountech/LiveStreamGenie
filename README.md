

**Live Streaming Genie** links OBS Commands embeded in your PowerPoint Presentation notes to a remote OBS Server.

I do a lot of online live web classes and when I am presenting, I used to use a clicker to advanced my powerpoint slides and then use my Elgato StreamDeck to change to the relevant OBS Scenes. This allows me to to switch between full screen Camera, Desktop Display or Green Screen Scenes based on where I am in the presentation.

With **Live Stream Genie**, I can incorporate both functions together and change the OBS Scene for different power points slides.

Now **Live Stream Genie** will notify you of relevant OBS Events like scene changes and quickly disappears after a few seconds so you can concentrate on your Presentation.  This has been a game changer for me.

Live Stream Genia is designed to work with a 2 PC Stream Setup.

1. From This PC, present your PowerPoint and run Live Stream Genie
2. From your Streaming PC, Setup OBS and turn on OBS Web Socket Server port 4455 and set the password

When you start Live Streaming Genie:

	1. Right Click Live Streaming Genie icon -> Settings to update the password
	2. Right Click Live Streaming Genie icon -> Reconnet to reconnect to OBS

Live Streaming Genie uses the new OBS Web Socket Server 5.x that is shipped with OBS OBS 29.x

Live Streaming Genie is based off Scot Hanselman's [PowerPointToOBSSceneSwitcher](https://github.com/shanselman/PowerPointToOBSSceneSwitcher) which I have been using for years, but just needs that little big of magic to remove friction.

This App has so much potential! Can't wait to see what you do with it.

WARNING: This is the result of a day's work, so please be patient as I add basic features like save settings, etc

Supported Commands:

* OBS: - switch to named scene
* **Start - Start Recording
* **STOP - Stop Recording
* OBSDEF: - set the OBS default scene if a scene doesn't exist

# Notification
 
Make sure you turn off nofication sounds or it will beep every time you get a notification.

![](./turn-off-sounds.png)

To see the notifications when running Powerpoint in presentation mode, you must also allow notifications in full screen node. To do this go to:

* Windows + I  (Settings)
* System
* Focus Assist
* "When I'm using an App in Full Screen Mode"
* Turn Off

![](./focus-assist.png)


# Technical Stuff (again, from Chat GPT)

This C# code creates an instance of the ApplicationContext class and starts a message loop by calling the Application.Run method, which runs the message loop on the ApplicationContext instance.

While this is running, the main Program class is listening out for events from PowerPoint and sending relevant commands to the App Context for processing.

An ApplicationContext is a class that manages the lifetime of a Windows Forms application. It provides a context in which forms can be shown and closed, and allows you to define application-level behavior, such as handling unhandled exceptions and controlling the shutdown of the application.

The Application.Run method starts a message loop that processes messages from the operating system and dispatches them to the appropriate forms and controls in the application. This message loop is responsible for handling user input, redrawing the user interface, and executing timers and other asynchronous operations.


# The Logo

I asked Chat GPT to create the logo. Here is the chat gpt descriptions that I got back and then I sent one of these description into MidJourney to create the logo. I Love the Logo and hope you do too.

```Sure, here's a detailed description of an icon that incorporates the genie lamp and a wifi signal in purple for Live Stream Genie:

The Live Stream Genie icon could feature a genie lamp with a wifi signal emanating from it. The genie lamp could be depicted in a shade of purple, with a shiny gold or silver base and a smoke-like aura rising from the spout. The lamp could be drawn in a whimsical style to convey a sense of fun and magic.

The wifi signal could be depicted as a series of purple waves or arcs radiating from the genie lamp. The waves could be drawn with a subtle gradient to suggest movement and connectivity. The color purple could be used for the wifi signal to match the genie lamp and create a cohesive color scheme for the icon.

The background of the icon could be a light blue or white to create a sense of airiness and openness. This could suggest the app's ability to live stream from anywhere, anytime, with ease. The combination of the purple lamp and the wifi signal could suggest that the app is magical and easy to use for live streaming over a wifi connection.

Overall, the Live Stream Genie icon with a purple genie lamp and wifi signal could convey a sense of fun, magic, and connectivity. The icon could grab the user's attention, convey the app's core features and values, and be easily recognizable on a user's device.
```

![Live Streaming Genie Logo](./LiveStreamGenie/Media/Live%20Stream%20Genie.png)