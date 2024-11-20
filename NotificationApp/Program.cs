using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/*
 * An application that collects various Notifications shows a list to the user of the type of Notification that he will see 
 * and he decides on the type of Notification that he wants to see which is based on the principle of event
 */

namespace NotificationApp
{
    public delegate void Notification_Delegate(object obj, string msg);//Delegates
    internal class Program
    {
        
        public class Notification//publisher
        {
            public event Notification_Delegate OnNotification;
            public void SendNotification(string msg)
            {
                if (OnNotification != null)//make sure there is subscriber
                {
                    OnNotification(this, msg);//firing the event 
                }
            }
        }
        public class Email //subscriber class
        {
          
            public void Receiving_the_alarm(object obj,string type)//Which is the function that will be called when the event is published
            {
                if (type == "email")
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"{type} received : Click to see it ");
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine($"Date:{DateTime.Now.ToString("dd/MM/yyyy")}");
                    Console.WriteLine($"Receive since {DateTime.Now.ToString("mm:ss")} ago ");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine();
                }
               
            }
        }
        public class SMS//subscriber class
        {
            public void Receiving_the_alarm(object obj, string type)//Which is the function that will be called when the event is published
            {
                if (type == "sms")
                {

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"{type} received : Click to see it ");
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine($"Date:{DateTime.Now.ToString("dd/MM/yyyy")}");
                    Console.WriteLine($"Receive since {DateTime.Now.ToString("mm:ss")} ago ");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine();
                }
            }
        }

        public class SubscriberCollection: IEnumerable//collection class that stores subscribers.
        {
            private List<Notification_Delegate>subscribers= new List<Notification_Delegate>();

            public void Add_Subscriber(Notification_Delegate subscriber)//A method for adding subscribers
            {
                subscribers.Add(subscriber);
            }
            public IEnumerator GetEnumerator()
            {
                return new Enum(subscribers);
            }

            private class Enum : IEnumerator
            {
                private List<Notification_Delegate> _subscribers;
                private int position = -1;
                public Enum(List<Notification_Delegate> subscribers)
                {
                   this._subscribers = subscribers;
                }

                object IEnumerator.Current
                {
                    get
                    {
                        return Current;
                    }
                }

                public Notification_Delegate Current
                {
                    get
                    {
                        try
                        {
                            return _subscribers[position];
                        }
                        catch (IndexOutOfRangeException)
                        {
                            throw new IndexOutOfRangeException();
                        }
                    }
                }
                bool IEnumerator.MoveNext()
                {
                    ++position;
                    return (position<this._subscribers.Count);
                }

                void IEnumerator.Reset()
                {
                    position = -1;
                }
            }
        }
        
        
        static void Main(string[] args)
        {
            Notification notification = new Notification();//Creating an object from the publisher
            Email email = new Email();//Create an object from the email class that will be added as a subscriber
            SMS sms = new SMS();//Create an object from the sms class that will be added as a subscriber

            SubscriberCollection sub = new SubscriberCollection();//Create a collection for storing subscribers

            sub.Add_Subscriber(email.Receiving_the_alarm);
            sub.Add_Subscriber(sms.Receiving_the_alarm);

            foreach (Notification_Delegate item in sub)
            {
                notification.OnNotification += item;//Participation in the event

            }


            
            int x;
            do
            {
                Console.WriteLine("Please choose one of the options");
                Console.WriteLine("1- Check mail notifications");
                Console.WriteLine("2- Check sms notifications");
                Console.WriteLine("3-Exit");
               x = int.Parse(Console.ReadLine());
                if (x == 1)
                {
                    notification.SendNotification("email");
                }
                else if (x == 2)
                {
                    notification.SendNotification("sms");
                }
                else
                {
                    break;
                }
            } while (true);
            
        }
       
    }
}
