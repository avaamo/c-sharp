# Avaamo C# Bot SDK

Before start developing a bot you should first create a bot in the Avaamo Dashboard.
Follow the steps in this [Getting Started](https://github.com/avaamo/java/wiki) page to create a bot in the dashboard.

### Pre-requisite
Visual Stidio 2012 or above & .NET Framework 4.5.2

#### Download and add library
Avaamo C# Bot SDK is a single nuget package file, Avaamo.1.0.0.0.nupkg

[ Download ](https://github.com/avaamo/c-sharp/blob/master/Avaamo.1.0.0.0.nupkg?raw=true) SDK

#### Sample Bot

This [file](https://github.com/avaamo/c-sharp/blob/master/bot_sample.cs?raw=true) has the full C# code for Bot implementation.

#### Receiving Messages

```c#
using Avaamo;

```
Initialize the library with your BOT UUID and Access Token.

```c#

static void Main(string[] args)
{
    Avaamo.Client avaamo = new Avaamo.Client(<YOUR-BOT-UUID>, <YOUR-BOT-ACCESS-TOKEN>, false);

    // With proxy
    // System.Net.NetworkCredential credentials = new System.Net.NetworkCredential("username", "password");
    // System.Net.WebProxy proxy = new System.Net.WebProxy("192.168.1.86:8080", true, null, credentials);
    // Avaamo.Client avaamo = new Avaamo.Client("<Bot-UUID>", "<Bot-Access-Token>", true, proxy);

    // Handle incoming messages
    avaamo.MessageHandeler += new Avaamo.MessageHandeler(ProcessMessage);
    // Handle incoming message read acknowledgment
    avaamo.ReadAckHandler += new Avaamo.ReadAckHandler(ProcessReadAck);
    // Assign User Visited handler
    avaamo.UserVisitedHandler += new Avaamo.UserVisitedHandler(HandleOnUserVisited);
    //Connect avaamo client
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
    Console.WriteLine(message.content);
}

static void ProcessReadAck(object sender, Avaamo.ReadAckArgs e)
{
    Console.WriteLine("ACK received");
    Avaamo.ReadAckModel ackModel = e.ReadAckModel();
    Console.WriteLine("User "+ackModel.user.firstName+" has read the message: "+ack_model.read_ack.message_uuid);
}

static void HandleOnUserVisited(object sender, Avaamo.UserActivityArgs e)
{
    var activity = e.UserActivity();
    var client = (Avaamo.Client)sender;
    Console.WriteLine("User "+  activity.user.firstName + "has visited the bot.");
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
Avaamo.File file = new Avaamo.File("C:\\Users\\User1\\Downloads\\TestFile.txt");
avaamo.SendAttachment(conversation, file);
```
![image](screenshots/file.png)

#### Sending a card

```c#
Avaamo.Card card = new Avaamo.Card("Card Title", "Card Description. This has minimal rich text capabilities as well. For example <b>Bold</b> <i>Italics</i>");
card.addShowCaseImage("C:\\Users\\User1\\Downloads\\rocket.jpg");

// Web page link
Avaamo.Links.Web web_link = new Avaamo.Links.Web("Google", "https://google.com");
card.addLink(web_link);

//Send Message Link
Avaamo.Links.SendMessage message_link = new Avaamo.Links.SendMessage("Post Message", "Sample Message");
card.addLink(message_link);

//Form link
Avaamo.Links.FormLink form_link = new Avaamo.Links.FormLink("Submit Form", "8e893b85-f206-4156-ae49-e917d584bcf3", "Form Name");
card.addLink(form_link);

avaamo.SendCard(conversation, card);

```
![image](screenshots/card.png)

#### Sending message to user without incoming message using email/phone number.
```c#
//Getting conversation from user's email
Avaamo.Conversation conversation = new Avaamo.Conversation(new Avaamo.Email("jalendra@avaamo.com"));
avaamo.sendTextMessage(conversation, "Hello");

//Getting conversation from user's phone
Avaamo.Conversation conversation2 = new Avaamo.Conversation(new Avaamo.Phone("+919595134315"));
avaamo.sendTextMessage(conversation2, "Conversation");
```
