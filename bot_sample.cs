using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avaamo;

namespace BotSample
{
    class Program
    {

        static void ProcessReadAck(object sender, Avaamo.ReadAckArgs e)
        {
            Console.WriteLine("ACK received");
            Avaamo.ReadAckModel ack_model = e.ReadAckModel();
            Console.WriteLine(ack_model.user.firstName);
            Console.WriteLine(ack_model.read_ack.message_uuid);
        }

        static void ProcessMessage(object sender, Avaamo.MessageArgs e)
        {
            string imagePath = "C:\\Users\\User1\\Downloads\\rocket.jpg";
            string filePath = "C:\\Users\\User1\\Downloads\\notes.pdf";
            string DownloadPath = "C:\\Users\\User1\\Downloads\\assets\\";

            Avaamo.IncomingMessage incoming_message = e.Message();
            Avaamo.Message message = incoming_message.message;
            Avaamo.Client client = e.Client();
            switch (message.content_type)
            {
                case "text":
                    switch (message.content.ToLower())
                    {
                        case "text":
                            // Send text message
                            client.SendTextMessage(message.conversation, "Hello");
                            break;
                        case "image":
                            // Send Image Attachment without caption
                            Avaamo.Image img = new Avaamo.Image(imagePath);
                            client.SendAttachment(message.conversation, img);
                            break;
                        case "image and caption":
                            // Send Image Attachment with caption
                            Avaamo.Image img1 = new Avaamo.Image(imagePath);
                            img1.setCaption("Just an caption");
                            client.SendAttachment(message.conversation, img1);
                            break;
                        case "file":
                            Avaamo.File file = new Avaamo.File(filePath);
                            client.SendAttachment(message.conversation, file);
                            break;
                        case "card":
                            Avaamo.Card card = new Avaamo.Card("Hello", "descriptions <b>lols</b>");
                            card.addShowCaseImage(imagePath);
                            Avaamo.Links.Web web_link = new Avaamo.Links.Web("Google", "https://google.com");
                            card.addLink(web_link);

                            Avaamo.Links.SendMessage message_link = new Avaamo.Links.SendMessage("Post Message", "Sample");
                            card.addLink(message_link);

                            Avaamo.Links.FormLink form_link = new Avaamo.Links.FormLink("Submit Form", "d6c32cd0-a092-4f5b-dd68-ec5eb2049b82", "Form Name");
                            card.addLink(form_link);

                            client.SendCard(message.conversation, card);
                            break;
                        default:
                            client.SendTextMessage(message.conversation, "Please type one of the following: \ntext\nimage\nimage and caption\nfile\ncard");
                            break;
                    }
                    break;
                case "form_response":
                    Console.WriteLine(message.form_response.uuid);
                    foreach (Avaamo.FormQuestion q in message.form_response.questions)
                    {
                        Console.WriteLine(q.title);
                        Console.WriteLine(q.answer);
                        var files = q.Files();
                        Console.WriteLine("Assets Count: " + files.Count);
                        for (int i = 0; i < files.Count; i++)
                        {
                            var asset = files[i];
                            asset.Download(DownloadPath + asset.file_name);
                        }
                        Console.WriteLine("***********");
                    }
                    break;
                case "photo":
                case "video":
                case "file":
                case "audio":
                    Console.WriteLine(message.content_type + " recieved.");
                    var ast = message.attached_asset;
                    ast.Download(DownloadPath + ast.file_name);
                    break;
            }

        }

        static void Main(string[] args)
        {
            string uuid = "<bot uuid>";
            string access_token = "<bot access_token>";

            // Create avaamo client
            Avaamo.Client avaamo = new Avaamo.Client(uuid, access_token);

            // Assign the message handler
            avaamo.MessageHandeler += new Avaamo.MessageHandeler(ProcessMessage);

            // Assign Read Acknowledgemt handler
            avaamo.ReadAckHandler += new Avaamo.ReadAckHandler(ProcessReadAck);

            // Connect to avaamo server
            avaamo.Connect();
        }


    }
}
