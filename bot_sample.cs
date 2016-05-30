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
            Avaamo.IncomingMessage incoming_message = e.Message();
            Avaamo.Message message = incoming_message.message;
            Console.WriteLine("Got Message");
            Console.WriteLine(message);
            Avaamo.Client client = e.Client();
            if (message.content == "text")
            {
                // Send text message
                client.SendTextMessage(message.conversation, "Hello");
            }

            if (message.content == "image")
            {
                // Send Image Attachment without caption
                Avaamo.Image img = new Avaamo.Image(imagePath);
                client.SendAttachment(message.conversation, img);
            }

            if (message.content == "image and caption")
            {
                // Send Image Attachment with caption
                Avaamo.Image img = new Avaamo.Image(imagePath);
                img.setCaption("Just an caption");
                client.SendAttachment(message.conversation, img);
            }

            if (message.content == "file")
            {
                Avaamo.File file = new Avaamo.File(filePath);
                client.SendAttachment(message.conversation, file);
            }

            if (message.content == "card")
            {
                Avaamo.Card card = new Avaamo.Card("Hello", "descriptions");
                card.addShowCaseImage(imagePath);
                Avaamo.Links.Web web_link = new Avaamo.Links.Web("Google", "https://google.com");
                card.addLink(web_link);

                Avaamo.Links.SendMessage message_link = new Avaamo.Links.SendMessage("Post Message", "Sample Message");
                card.addLink(message_link);

                Avaamo.Links.FormLink form_link = new Avaamo.Links.FormLink("Submit Form", "d6c32cd0-a092-4f5b-dd68-ec5eb2049b82", "Form Name");
                card.addLink(form_link);

                client.SendCard(message.conversation, card);
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
