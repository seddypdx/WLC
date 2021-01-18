using Azure.Communication.Administration;
using Azure.Communication.Sms;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WLC.Models;

namespace WLC.Areas.Notices.Services
{
    public class NotificationService
    {

        public static void QueueNotification(WLC.Models.WLCRacesContext context, WLC.Models.Notices Notice)
        {

            if (Notice.NoticeTypeId == (int)NoticeTypeEnum.Social)
                 NotifyForSocial(context, Notice);

            if (Notice.NoticeTypeId == (int)NoticeTypeEnum.Informational)
                NotifyForInformation(context, Notice);

            if (Notice.NoticeTypeId == (int)NoticeTypeEnum.Emergency)
                NotifyForEmergency(context, Notice);



        }
        private static void NotifyForInformation(WLCRacesContext context, Models.Notices Notice)
        {
            foreach (var member in context.Members.Where(x => x.NotifyOnInformationText))
            {
                try
                {
                    var queueItem = new NoticeQueueItems()
                    {
                        NoticeId = Notice.NoticeId,
                        Notice = Notice,
                        DateLastChanged = DateTime.Now,
                        NoticeStatusId = (int)NoticeStatusEnum.New,
                        NotificationLocation = GetMemeberTextEmail(member)
                    };
                    context.Add(queueItem);
                }
                catch (Exception ex)
                {
                    //log error here
                }
            }

            foreach (var member in context.Members.Where(x => x.NotifyOnInformationEmail))
            {
                try
                {
                    var queueItem = new NoticeQueueItems()
                    {
                        NoticeId = Notice.NoticeId,
                        Notice = Notice,
                        DateLastChanged = DateTime.Now,
                        NoticeStatusId = (int)NoticeStatusEnum.New,
                        NotificationLocation = member.EmailName
                    };
                    context.Add(queueItem);
                }
                catch (Exception ex)
                {
                    // log error here
                }
            }

            context.SaveChanges();
        }
        private static void NotifyForEmergency(WLCRacesContext context, Models.Notices Notice)
        {
            foreach (var member in context.Members.Where(x => x.NotifyOnEmergencyText))
            {
                try
                {
                    var queueItem = new NoticeQueueItems()
                    {
                        NoticeId = Notice.NoticeId,
                        Notice = Notice,
                        DateLastChanged = DateTime.Now,
                        NoticeStatusId = (int)NoticeStatusEnum.New,
                        NotificationLocation = GetMemeberTextEmail(member)
                    };
                    context.Add(queueItem);
                }
                catch (Exception ex)
                {
                    //log error here
                }
            }

            foreach (var member in context.Members.Where(x => x.NotifyOnEmergencyEmail))
            {
                try
                {
                    var queueItem = new NoticeQueueItems()
                    {
                        NoticeId = Notice.NoticeId,
                        Notice = Notice,
                        DateLastChanged = DateTime.Now,
                        NoticeStatusId = (int)NoticeStatusEnum.New,
                        NotificationLocation = member.EmailName
                    };
                    context.Add(queueItem);
                }
                catch (Exception ex)
                {
                    // log error here
                }
            }

            context.SaveChanges();
        }

        private static void NotifyForSocial(WLCRacesContext context, Models.Notices Notice)
        {
            foreach (var member in context.Members.Where(x => x.NotifyOnSocialText))
            {
                try
                {
                    var queueItem = new NoticeQueueItems()
                    {
                        NoticeId = Notice.NoticeId,
                        Notice = Notice,
                        DateLastChanged = DateTime.Now,
                        NoticeStatusId = (int)NoticeStatusEnum.New,
                        NotificationLocation = GetMemeberTextEmail(member)
                    };
                    context.Add(queueItem);
                }
                catch (Exception ex)
                {
                    //log error here
                }
            }

            foreach (var member in context.Members.Where(x => x.NotifyOnSocialEmail))
            {
                try
                {
                    var queueItem = new NoticeQueueItems()
                    {
                        NoticeId = Notice.NoticeId,
                        Notice = Notice,
                        DateLastChanged = DateTime.Now,
                        NoticeStatusId = (int)NoticeStatusEnum.New,
                        NotificationLocation = member.EmailName
                    };
                    context.Add(queueItem);
                }
                catch (Exception ex)
                {
                    // log error here
                }
            }

            context.SaveChanges();
        }

        public static string GetMemeberTextEmail(Member member)
        {
            var carrier = "ATT";

            Regex regexObj = new Regex(@"[^\d]");
            var phoneOnly = regexObj.Replace(member.HomeCell, "");


            return $"{phoneOnly}@{GetCarrierSuffix(carrier)}";

        }

        private static string GetCarrierSuffix(string carrier)
        {
            if (carrier == "ATT")
                return "txt.att.net";

            if (carrier == "Verizon")
                return "vtext.com";

            if (carrier == "Sprint")
                return "messaging.sprintpcs.com";

            if (carrier == "TMobile")
                return "tmomail.net";

            if (carrier == "VirginMobile")
                return "vmobl.com";

            if (carrier == "Nextel")
                return "messaging.nextel.com";

            if (carrier == "Boost")
                return "myboostmobile.com";

            if (carrier == "Alltel")
                return "message.alltel.com";

            if (carrier == "EE")
                return "mms.ee.co.uk";

            throw new Exception("Carrier Not Handled");

        }


    
        public static void ProcessNotificationQueue(IEmailSender emailSender,  WLC.Models.WLCRacesContext context)
        {
            foreach (var notificationQueueItem in context.NoticeQueueItems
                                                          .Where(x => x.NoticeStatusId == (int) NoticeStatusEnum.New)
                                                          .Include(x => x.Notice)
                                                          .ThenInclude(x => x.NoticeType))
            {

                var message = $"From Wauna Lake; {notificationQueueItem.Notice.NoticeType.Description} Notification ---- {notificationQueueItem.Notice.Message}";
                 
                try
                {
                    //TextMessage(notificationQueueItem.NotificationLocation, message);
                    if (notificationQueueItem.NotificationLocation.Contains("@"))
                        SendMessage(emailSender, notificationQueueItem.NotificationLocation, message);
                    else
                        TextMessageViaCommunicationService(null, notificationQueueItem.NotificationLocation, message);

                    notificationQueueItem.NoticeStatusId = (int)NoticeStatusEnum.Completed;


                }
                catch(Exception ex)
                {
                    notificationQueueItem.NoticeStatusId = (int)NoticeStatusEnum.Error;

                }
            }
            context.SaveChanges();

        }

        public async static void TextMessageViaCommunicationService(IConfiguration configuration, string PhoneNumber, string Message)
        {
            string connectionString = "endpoint=https://smsforwlctest.communication.azure.com/;accesskey=ir/swruC+focwatNzk+379NVuhj0+UdlUMZ6Qa9mzi+XbNplGJPewqduDUPZwYtweeeuW3YDKwPnbfuvph46pw==";
           string Key = "ir/swruC+focwatNzk+379NVuhj0+UdlUMZ6Qa9mzi+XbNplGJPewqduDUPZwYtweeeuW3YDKwPnbfuvph46pw==";
            string endPoint = "https://smsforwlctest.communication.azure.com/";

            SmsClient smsClient = new SmsClient(connectionString);
            smsClient.Send(
                from: new Azure.Communication.PhoneNumber("503-816-9054"),
                to: new Azure.Communication.PhoneNumber(PhoneNumber),
                message: Message,
                new SendSmsOptions { EnableDeliveryReport = true } // optional

            );


            //var identityResponse = await client.CreateUserAsync();
            //var identity = identityResponse.Value;
            //Console.WriteLine($"\nCreated an identity with ID: {identity.Id}");

            //// Issue an access token with the "voip" scope for an identity
            //var tokenResponse = await client.IssueTokenAsync(identity, scopes: new[] { CommunicationTokenScope.VoIP });
            //var token = tokenResponse.Value.Token;
            //var expiresOn = tokenResponse.Value.ExpiresOn;
            //Console.WriteLine($"\nIssued an access token with 'voip' scope that expires at {expiresOn}:");
            //Console.WriteLine(token);


        }

        public async static void SendMessage(IEmailSender emailSender,  string email, string noticeMessage)
        {

              //message.CC.Add(new MailAddress("carboncopy@foo.bar.com"));
             await emailSender.SendEmailAsync(email,  "Wauna Lake Notice",  noticeMessage);



        }


    }
}
