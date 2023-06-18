//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Telegram.Bot;
//using Telegram.Bot.Polling;
//using Telegram.Bot.Types;
//using Telegram.Bot.Types.Enums;

//namespace AwarenessTelegramBot
//{
//    public class Test
//    {
//        public static Message OnMessage()
//        {
//            var botClient = new TelegramBotClient("6073096280:AAGjLrQdWF0j3phbnOMIip0g_8aYSVu_Vf0");
//            var me = botClient.GetMeAsync();

//            ReceiverOptions receiverOptions = new()
//            {
//                AllowedUpdates = Array.Empty<UpdateType>() // receive all update types except ChatMember related updates
//            };

//            botClient.StartReceiving(
//            updateHandler: HandleUpdateAsync,
//            pollingErrorHandler: HandlePollingErrorAsync);

//            Console.WriteLine($"Start listening for @{me.Username}");
//            Console.ReadLine();


//        }

//        async Message HandleUpdateAsync(ITelegramBotClient botClient, Update receivedUpdate, CancellationToken cancellationToken)
//        {
//            if (receivedUpdate.Message is not { } message)
//                return message;
//            if (message.Text is not { } messageText)
//                return;
//        }
//    }
//}
