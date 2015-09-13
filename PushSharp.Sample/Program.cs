using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using PushSharp;
using PushSharp.Android;

using PushSharp.Core;


//using PushSharp.Android;
//using PushSharp.WindowsPhone;
//using PushSharp.Windows;

namespace PushSharp.Sample
{
	class Program
	{
		static void Main(string[] args)
		{		
			//Create our push services broker
			var push = new PushBroker();

			//Wire up the events for all the services that the broker registers
			push.OnNotificationSent += NotificationSent;
			push.OnChannelException += ChannelException;
			push.OnServiceException += ServiceException;
			push.OnNotificationFailed += NotificationFailed;
			push.OnDeviceSubscriptionExpired += DeviceSubscriptionExpired;
			push.OnDeviceSubscriptionChanged += DeviceSubscriptionChanged;
			push.OnChannelCreated += ChannelCreated;
			push.OnChannelDestroyed += ChannelDestroyed;
			

			//------------------------------------------------
			//IMPORTANT NOTE about Push Service Registrations
			//------------------------------------------------
			//Some of the methods in this sample such as 'RegisterAppleServices' depend on you referencing the correct
			//assemblies, and having the correct 'using PushSharp;' in your file since they are extension methods!!!

			// If you don't want to use the extension method helpers you can register a service like this:
			//push.RegisterService<WindowsPhoneToastNotification>(new WindowsPhonePushService());
			
			//If you register your services like this, you must register the service for each type of notification
			//you want it to handle.  In the case of WindowsPhone, there are several notification types!



			

			
			//---------------------------
			// ANDROID GCM NOTIFICATIONS
			//---------------------------
			//Configure and start Android GCM
			//IMPORTANT: The API KEY comes from your Google APIs Console App, under the API Access section, 
			//  by choosing 'Create new Server key...'
			//  You must ensure the 'Google Cloud Messaging for Android' service is enabled in your APIs Console
			push.RegisterGcmService(new GcmPushChannelSettings("YOUR Google API's Console API Access  API KEY for Server Apps HERE"));
			//Fluent construction of an Android GCM Notification
			//IMPORTANT: For Android you MUST use your own RegistrationId here that gets generated within your Android app itself!
			push.QueueNotification(new GcmNotification().ForDeviceRegistrationId("DEVICE REGISTRATION ID HERE")
			                      .WithJson("{\"alert\":\"Hello World!\",\"badge\":7,\"sound\":\"sound.caf\"}"));
			

			

			

			

			Console.WriteLine("Waiting for Queue to Finish...");

			//Stop and wait for the queues to drains
			push.StopAllServices();

			Console.WriteLine("Queue Finished, press return to exit...");
			Console.ReadLine();			
		}

		static void DeviceSubscriptionChanged(object sender, string oldSubscriptionId, string newSubscriptionId, INotification notification)
		{
			//Currently this event will only ever happen for Android GCM
			Console.WriteLine("Device Registration Changed:  Old-> " + oldSubscriptionId + "  New-> " + newSubscriptionId + " -> " + notification);
		}

		static void NotificationSent(object sender, INotification notification)
		{
			Console.WriteLine("Sent: " + sender + " -> " + notification);
		}

		static void NotificationFailed(object sender, INotification notification, Exception notificationFailureException)
		{
			Console.WriteLine("Failure: " + sender + " -> " + notificationFailureException.Message + " -> " + notification);
		}

		static void ChannelException(object sender, IPushChannel channel, Exception exception)
		{
			Console.WriteLine("Channel Exception: " + sender + " -> " + exception);
		}

		static void ServiceException(object sender, Exception exception)
		{
			Console.WriteLine("Service Exception: " + sender + " -> " + exception);
		}

		static void DeviceSubscriptionExpired(object sender, string expiredDeviceSubscriptionId, DateTime timestamp, INotification notification)
		{
			Console.WriteLine("Device Subscription Expired: " + sender + " -> " + expiredDeviceSubscriptionId);
		}

		static void ChannelDestroyed(object sender)
		{
			Console.WriteLine("Channel Destroyed for: " + sender);
		}

		static void ChannelCreated(object sender, IPushChannel pushChannel)
		{
			Console.WriteLine("Channel Created for: " + sender);
		}
	}
}
