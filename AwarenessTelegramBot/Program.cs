using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Args;

namespace AwarenessTelegramBot
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int[] result = new int[41];
            int i = 0;
            string[] lines = System.IO.File.ReadAllLines(@"C:\Users\kitikek\source\repos\AwarenessTelegramBot\AwarenessTelegramBot\test.txt");
            var botClient = new TelegramBotClient("6073096280:AAGjLrQdWF0j3phbnOMIip0g_8aYSVu_Vf0");

            var me = botClient.GetMeAsync().Result;
            Console.WriteLine($"Hello, World! I am user {me.Id} and my name is {me.FirstName} username {me.Username}.");

            ReceiverOptions receiverOptions = new()
            {
                AllowedUpdates = Array.Empty<UpdateType>() // receive all update types except ChatMember related updates
            };

            botClient.StartReceiving(
            updateHandler: HandleUpdateAsync,
            pollingErrorHandler: HandlePollingErrorAsync);

            Console.WriteLine($"Start listening for @{me.Username}");
            Console.ReadLine();

            static (int, int) GetResult(int[] array)
            {
                int[] straightQuestions = new int[] { 3, 4, 6, 7, 9, 12, 13, 14, 17, 18 };
                int[] reverseQuestions = new int[] { 1, 2, 5, 8, 10, 11, 15, 16, 19, 20 };

                int[] straightPersQuestions = new int[] { 22, 23, 24, 25, 28, 29, 31, 32, 34, 35, 37, 38, 40 };
                int[] reversePersQuestions = new int[] { 21, 26, 27, 30, 33, 36, 39};

                for (int i = 0; i < array.Length; i++)
                {
                    if (straightQuestions.Contains(i))
                        straightQuestions[straightQuestions.ToList().IndexOf(i)] = array[i];
                    if (reverseQuestions.Contains(i))
                        reverseQuestions[reverseQuestions.ToList().IndexOf(i)] = array[i];
                    if (straightPersQuestions.Contains(i))
                        straightPersQuestions[straightPersQuestions.ToList().IndexOf(i)] = array[i];
                    if (reversePersQuestions.Contains(i))
                        reversePersQuestions[reversePersQuestions.ToList().IndexOf(i)] = array[i];
                }
                int reactAnx = straightQuestions.Sum() - reverseQuestions.Sum() +50;
                int persAnx = straightPersQuestions.Sum() - reversePersQuestions.Sum() + 35;
                return (reactAnx , persAnx);
            }
            static string IdentifyResult(int res)
            {
                if (res < 30 && res >= 20)
                    return "низкий😌";
                if (res < 45 && res >= 30)
                    return "умеренный😐";
                if (res >= 45 && res <= 80)
                    return "высокий😧";
                else
                    return "не достоверный, стоит пройти тест еще раз";
            }

            async Task HandleUpdateAsync(ITelegramBotClient botClient, Update receivedUpdate, CancellationToken cancellationToken)
            {
                if (receivedUpdate.Message is not { } message)
                    return;
                if (message.Text is not { } messageText)
                    return;

                if (message.Text is "/start")
                {
                    ReplyKeyboardMarkup replyKeyboardMarkup = new(new[] { new KeyboardButton[] { "Начать тест" }, })
                    {
                        ResizeKeyboard = true
                    };
                    var chatId = message.Chat.Id;

                    Console.WriteLine($"Message \"/start\" from {chatId}, User - {message.Chat.Username}");
                    Message sentMessage = await botClient.SendTextMessageAsync(
                        chatId, "Привет! Мы создали бота \"Мы вместе\", который помогает справиться с тревожностью и стрессом 🆘" +
                        "\nПройди тест на уровень тревожности и получи доступ к медитациям, дыхательным практикам, подкастам, книгам, музыке и цитатам 🧘‍♀️" +
                        "\nНаш бот - это твой надежный друг и помощник" +
                        "\nНачни прямо сейчас, нажми кнопку \"Начать тест\" и найди внутреннюю гармонию 💗",
                        replyMarkup: replyKeyboardMarkup);
                    Array.Clear(result);
                    i = 0;
                }
                if (message.Text is "Начать тест")
                {
                    var chatId = message.Chat.Id;
                    Console.WriteLine($"Message \"Начать тест\" from {chatId},User - {message.Chat.Username}");

                    Message sentMessage = await botClient.SendTextMessageAsync(
                                chatId,
                                "Проходите тест осознанно, не спешите, прислушайтесь к себе.");
                    Message sendStiker = await botClient.SendStickerAsync(
                        chatId,
                        sticker: InputFile.FromUri("https://cdn.tlgrm.app/stickers/a13/772/a1377248-ef80-44a5-88a4-6d89aa2ebad2/192/10.webp"),
                        cancellationToken: cancellationToken);
                    message.Text = "Перейти к тесту";
                    Array.Clear(result);
                    i = 0;
                }
                if (message.Text is "Перейти к тесту" || message.Text is "Нет, это не так" || message.Text is "Пожалуй, так" || message.Text is "Верно" || message.Text is "Совершенно верно"
                    || message.Text is "Почти никогда" || message.Text is "Иногда" || message.Text is "Часто" || message.Text is "Почти всегда")
                {
                    var chatId = message.Chat.Id;

                    
                    Console.WriteLine($"Message - {message.Text} from {chatId}, User - {message.Chat.Username}");

                    if (i < 40)
                    {
                        if (i < 20)
                        {
                            string line = lines[i];
                            ReplyKeyboardMarkup replyKeyboardMarkup = new(new[] { new KeyboardButton[] { "Нет, это не так", "Пожалуй, так", "Верно", "Совершенно верно" }, })
                            {
                                ResizeKeyboard = true
                            };
                            Message sentMessage = await botClient.SendTextMessageAsync(
                                chatId,
                                line.ToString(),
                                replyMarkup: replyKeyboardMarkup);
                            receivedUpdate = new Update();
                            if (i == 0)
                                i++;
                        }
                        else
                        {
                            string line = lines[i];
                            ReplyKeyboardMarkup replyKeyboardMarkup = new(new[] { new KeyboardButton[] { "Почти никогда", "Иногда", "Часто", "Почти всегда" }, })
                            {
                                ResizeKeyboard = true
                            };
                            Message sentMessage = await botClient.SendTextMessageAsync(
                                chatId,
                                line.ToString(),
                                replyMarkup: replyKeyboardMarkup);
                            receivedUpdate = new Update();
                        }
                    }
                    else
                    {
                        if (i >= 40)
                        {
                            ReplyKeyboardMarkup replyKeyboardMarkup = new(new[] { new KeyboardButton[] { "Узнать результат" }, })
                            {
                                ResizeKeyboard = true
                            };
                            Message sentMessage = await botClient.SendTextMessageAsync(
                                chatId,
                                "Давайте узнаем результат 🤔",
                                replyMarkup: replyKeyboardMarkup);
                        }
                        receivedUpdate = new Update();
                    }

                }
                if (message.Text is "Нет, это не так" || message.Text is "Почти никогда")
                {
                    result[i] += 1;
                    i++;
                }
                if (message.Text is "Пожалуй, так" || message.Text is "Иногда")
                {
                    result[i] += 2;
                    i++;
                }
                if (message.Text is "Верно" || message.Text is "Часто")
                {
                    result[i] += 3;
                    i++;
                }
                if (message.Text is "Совершенно верно" || message.Text is "Почти всегда")
                {
                    result[i] += 4;
                    i++;
                }
                if (message.Text is "/help")
                {
                    var chatId = message.Chat.Id;

                    Console.WriteLine($"Message \"/help\" from {chatId}, User - {message.Chat.Username}");
                    Message sentMessage = await botClient.SendTextMessageAsync(
                        chatId, "* Список команд"
                        );
                }
                if (message.Text is "Узнать результат")
                {
                    var chatId = message.Chat.Id;

                    int res1 = GetResult(result).Item1;
                    int res2 = GetResult(result).Item2;

                    string mes = $"Результаты:\nУровень вашей ситуативной тревожности - {IdentifyResult(res1)} ({res1} баллов)" +
                        $"\nУровень вашей личностной тревожности - {IdentifyResult(res2)} ({res2} баллов)";

                    Console.WriteLine($"Message Узнать результат from {chatId}, User - {message.Chat.Username}");
                    Message sentMessage = await botClient.SendTextMessageAsync(
                            chatId,
                            mes);
                    receivedUpdate = new Update();
                }
            }

            Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
            {
                var ErrorMessage = exception switch
                {
                    ApiRequestException apiRequestException
                        => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                    _ => exception.ToString()
                };

                Console.WriteLine(ErrorMessage);
                return Task.CompletedTask;
            }
        }
    }
}