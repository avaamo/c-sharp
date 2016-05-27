# Avaamo C# Bot SDK

Before start developing a bot you should first create a bot in the Avaamo Dashboard.
Follow the steps in this [Getting Started](https://github.com/avaamo/java/wiki) page to create a bot in the dashboard.

#### Download and add library
Avaamo C# Bot SDK is a single nuget package file, Avaamo.1.0.0.0.nupkg

[ Download ](https://github.com/avaamo/c-sharp/blob/master/Avaamo.1.0.0.0.nupkg?raw=true) SDK

#### Sample Bot

This [file](https://github.com/avaamo/c-sharp/blob/master/SampleBot.cs?raw=true) has the full example referred in this page.

#### Receiving Messages

```c#
using Avaamo;

```
Initialize the library with your BOT UUID and Access Token.

```c#

static void Main(string[] args)
{
    Avaamo.Client avaamo = new Avaamo.Client(<YOUR-BOT-UUID>, <YOUR-BOT-ACCESS-TOKEN>);
    avaamo.MessageHandeler += new Avaamo.MessageHandeler(ProcessMessage);
    avaamo.Connect();
}

static void ProcessMessage(object sender, Avaamo.MessageArgs e)
{
    Avaamo.IncomingMessage incoming_message = e.Message();
    Avaamo.Message message = incoming_message.message;
    Avaamo.Conversation conversation = message.conversation;
    Console.WriteLine("Got Message");
    Console.WriteLine(message);
    Avaamo.Client avaamo = e.Client();
    Console.WriteLine(message.content);a
}

```
#### Sending Text Messages

```c#
// message is the JSON string
avaamo.SendTextMessage(conversation, "Hello");
```

#### Sending an image

```c#
Avaamo.Image img = new Avaamo.Image("C:\\Users\\User1\\Downloads\\rocket.jpg");
img.setCaption("This is the image caption");
avaamo.SendAttachment(conversation, img);
```
![image](screenshots/image.png)

#### Sending a file

```c#
Avaamo.File file = new Avaamo.File("C:\\Users\\User1\\Downloads\\notes.pdf");
avaamo.SendAttachment(conversation, file);
```
![image](screenshots/file.png)

#### Sending a card

```c#
Avaamo.Card card = new Avaamo.Card("Card Title", "Card Description. This has minimal rich text capabilities as well. For example <b>Bold</b> <i>Italics</i>");
card.addShowCaseImage("C:\\Users\\User1\\Downloads\\rocket.jpg");
card.addLink("Google", "web_page", "http://google.com");
card.addLink("Facebook", "web_page", "http://facebook.com");
avaamo.SendCard(conversation, card);
```
![image](screenshots/card.png)

