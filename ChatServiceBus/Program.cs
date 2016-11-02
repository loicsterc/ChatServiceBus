using Microsoft.ServiceBus.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatServiceBus
{
    /// @Author : Loic Sterckx
    /// @Date : 30 October 2016
    /// <summary>
    /// Class that contains the main method of the software
    /// </summary>
    class Program
    {
        static string userName;

        static void Main(string[] args)
        {
            //Initiate the topic if not exist
            Helper.CreateTopic();

            //get the current user information
            Console.WriteLine("Welcome on ChatServiceBus, please enter your user name:");
            userName = Console.ReadLine();            
            Console.WriteLine("Hello " + userName + ", which mode do you want to use?");            
            
            //create the subscription if not exist
            Helper.CreateSubscription(userName);
            
            //show the menu
            MainMenu();
        }

        /// <summary>
        /// Method to display the main menu of the software
        /// </summary>
        private static void MainMenu()
        {
            Console.WriteLine("1. Receive My Messages");
            Console.WriteLine("2. Send Messages");
            Console.WriteLine("3. Exit");
            string result = Console.ReadLine();
            switch (result)
            {
                case "1":
                    DisplayReceiverMenu();
                    break;
                case "2":
                    DisplaySenderMenu();
                    break;
                case "3":
                    Environment.Exit(0);
                    break;
                default:
                    MainMenu();
                    break;
            }
        }

        /// <summary>
        /// Method to display the receiver menu of the software
        /// </summary>
        private static void DisplayReceiverMenu()
        {
            Console.WriteLine("New Messages will be received here. Press 1 and enter to return to main menu");
            Helper.ReceiveMessageSubscription(userName);
            string result = Console.ReadLine();
            if(result == "1")
            {
                MainMenu();
                return;
            }                       
        }

        /// <summary>
        /// Method to display the send menu of the software
        /// </summary>
        private static void DisplaySenderMenu()
        {            
            Console.WriteLine("To who will you send a new message? (press 1 and enter to go to home menu)");
            foreach(SubscriptionDescription subscription in Helper.GetSubscriptionsNames())
            {
                Console.WriteLine("\t"+subscription.Name);
            }
            Console.WriteLine("\tAll");
            string toUserName = Console.ReadLine();
            //check if we have to exit
            if(toUserName == "1")
            {
                MainMenu();
                return;
            }
            //check if the sender exist            
            if (toUserName.ToLowerInvariant() != "all" && !Helper.IsSubscriptionExist(toUserName))
            {
                Console.WriteLine("Your user doesn't exist, please choose another one");
                DisplaySenderMenu();
                return;
            }
            Console.WriteLine("Type your message for " + toUserName);
            string message = Console.ReadLine();
            //check if we still don't have to exit
            if(message == "1")
            {
                MainMenu();
                return;
            }
            Helper.SendMessageTopic(toUserName, userName, message);
            Console.WriteLine("\n**Message Sent!**");

            //check to send another message or not
            Console.WriteLine("Do you want to send another one? (Y/N)");
            switch (Console.ReadLine().ToLowerInvariant())
            {
                case "y":
                    DisplaySenderMenu();
                    break;
                case "n":
                    MainMenu();
                    break;
                default:
                    MainMenu();
                    break;
            }
        }


    }
}
