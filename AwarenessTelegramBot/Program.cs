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
            int res1 = 0;
            int res2 = 0;
            string[] lines = System.IO.File.ReadAllLines(@"../../../test.txt");
            string[] bookNames = System.IO.File.ReadAllLines(@"../../../BookNames.txt");
            string[] podcasts = System.IO.File.ReadAllLines(@"../../../Подкасты.txt");
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

                    Console.WriteLine($"Message - /start from {chatId}, User - {message.Chat.FirstName + " " + message.Chat.LastName}");
                    Message sentMessage = await botClient.SendTextMessageAsync(
                        chatId, $"Привет, {message.Chat.FirstName + " " + message.Chat.LastName}!\nМы создали бота \"Мы вместе\", который помогает справиться с тревожностью и стрессом 🆘" +
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
                    Console.WriteLine($"Message - Начать тест from {chatId},User - {message.Chat.FirstName + " " + message.Chat.LastName}");

                    Message sentMessage = await botClient.SendTextMessageAsync(
                                chatId,
                                "Проходите тест осознанно, не спешите, прислушайтесь к себе.");
                    //Message sendStiker = await botClient.SendStickerAsync(
                    //    chatId,
                    //    sticker: InputFile.FromUri("https://cdn.tlgrm.app/stickers/a13/772/a1377248-ef80-44a5-88a4-6d89aa2ebad2/192/10.webp"),
                    //    cancellationToken: cancellationToken);
                    message.Text = "Перейти к тесту";
                    Array.Clear(result);
                    i = 0;
                }
                if (message.Text is "Перейти к тесту" || message.Text is "Нет, это не так" || message.Text is "Пожалуй, так" || message.Text is "Верно" || message.Text is "Совершенно верно"
                    || message.Text is "Почти никогда" || message.Text is "Иногда" || message.Text is "Часто" || message.Text is "Почти всегда")
                {
                    var chatId = message.Chat.Id;
                    
                    Console.WriteLine($"Message - {message.Text} from {chatId}, User - {message.Chat.FirstName + " " + message.Chat.LastName}");

                    if (i < 40)
                    {
                        if (i < 20)
                        {
                            string line = lines[i];
                            ReplyKeyboardMarkup replyKeyboardMarkup = new(new[] { new KeyboardButton[] { "Нет, это не так", "Пожалуй, так" },
                                                                                    new KeyboardButton[]{"Верно", "Совершенно верно"}})
                            {
                                ResizeKeyboard = true
                            };
                            if (i == 0)
                            {
                                Message sentMessageIn = await botClient.SendTextMessageAsync(
                                    chatId,
                                    "Первый тест на определение ситуативной тревожности👇",
                                    replyMarkup: replyKeyboardMarkup);
                                i++;
                            }
                            Message sentMessage = await botClient.SendTextMessageAsync(
                                chatId,
                                line.ToString(),
                                replyMarkup: replyKeyboardMarkup);
                            receivedUpdate = new Update();
                        }
                        else
                        {                            
                            string line = lines[i];
                            ReplyKeyboardMarkup replyKeyboardMarkup = new(new[] { new KeyboardButton[] { "Почти никогда", "Иногда" },
                                                                                    new KeyboardButton[]{"Часто", "Почти всегда"} })
                            {
                                ResizeKeyboard = true
                            };
                            if (i == 20)
                            {
                                Message sentMessageIn = await botClient.SendTextMessageAsync(
                                    chatId,
                                    "Второй тест на определение личностной тревожности👇",
                                    replyMarkup: replyKeyboardMarkup);
                            }
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
                                "Ура! Оба теста пройдены, в качестве ннаграды для вас - цитата, надеюсь, она натолкнет вас на правильные мысли🎉",
                                replyMarkup: replyKeyboardMarkup);
                            sentMessage = await botClient.SendTextMessageAsync(
                                chatId,
                                ForMessages.GetRandomQuote(),
                                replyMarkup: replyKeyboardMarkup);
                            sentMessage = await botClient.SendTextMessageAsync(
                                chatId,
                                "А теперь, давайте узнаем результат 🤔",
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

                    Console.WriteLine($"Message \"/help\" from {chatId}, User - {message.Chat.FirstName + " " + message.Chat.LastName}");
                    Message sentMessage = await botClient.SendTextMessageAsync(
                        chatId, "* Список команд"
                        );
                }
                if (message.Text is "Узнать результат")
                {
                    var chatId = message.Chat.Id;

                    res1 = ForMessages.GetResult(result).Item1;
                    res2 = ForMessages.GetResult(result).Item2;

                    string mes = $"Результаты:\nУровень вашей ситуативной тревожности - {ForMessages.IdentifyResult(res1)} ({res1} баллов)" +
                        $"\nУровень вашей личностной тревожности - {ForMessages.IdentifyResult(res2)} ({res2} баллов)";

                    Console.WriteLine($"Message - Узнать результат from {chatId}, User - {message.Chat.FirstName + " " + message.Chat.LastName}");
                    Message sentMessage = await botClient.SendTextMessageAsync(
                            chatId,
                            mes);
                    ReplyKeyboardMarkup replyKeyboardMarkup = new(new[] { new KeyboardButton[] { "Получить интерпретацию", "Перейти к подборкам" } })
                    {
                        ResizeKeyboard = true
                    };
                    sentMessage = await botClient.SendTextMessageAsync(
                            chatId,
                            "Вы можете получить более полное описание уровней тревожности, либо перейти непосредственно к материалам",
                            replyMarkup: replyKeyboardMarkup);
                    receivedUpdate = new Update();
                }
                if (message.Text is "Получить интерпретацию")
                {
                    var chatId = message.Chat.Id;

                    Console.WriteLine($"Message - Получить интерпретацию from {chatId}, User - {message.Chat.Username}");

                    string mes1 = ForMessages.GetInterpretation(ForMessages.IdentifyResult(res1));
                    string mes2 = ForMessages.GetInterpretation(ForMessages.IdentifyResult(res2));
                    ReplyKeyboardMarkup replyKeyboardMarkup = new(new[] { new KeyboardButton[] { "Перейти к подборкам" } })
                    {
                        ResizeKeyboard = true
                    };

                    if (mes1 == mes2)
                    {
                        Message sentMessage = await botClient.SendTextMessageAsync(
                                    chatId,
                                    "Интерпретация для вашего уровня тревожностей👇");
                        sentMessage = await botClient.SendTextMessageAsync(
                                chatId,
                                mes1,
                                replyMarkup: replyKeyboardMarkup);
                    }
                    else
                    {
                        Message sentMessage = await botClient.SendTextMessageAsync(
                                    chatId,
                                    "Интерпретация для вашего уровня ситуативной тревожности👇");
                        sentMessage = await botClient.SendTextMessageAsync(
                                chatId,
                                mes1,
                                replyMarkup: replyKeyboardMarkup);
                        sentMessage = await botClient.SendTextMessageAsync(
                                    chatId,
                                    "Интерпретация для вашего уровня личностной тревожности👇");
                        sentMessage = await botClient.SendTextMessageAsync(
                                chatId,
                                mes2,
                                replyMarkup: replyKeyboardMarkup);
                    }
                }
                if (message.Text is "Перейти к подборкам")
                {
                    var chatId = message.Chat.Id;

                    Console.WriteLine($"Message - Перейти к подборкам from {chatId}, User - {message.Chat.FirstName + " " + message.Chat.LastName}");

                    ReplyKeyboardMarkup replyKeyboardMarkup = new(new[] { new KeyboardButton[] { "Медитации", "Книги" },
                                                                            new KeyboardButton[]{"Подкасты"} })
                    {
                        ResizeKeyboard = true
                    };
                    Message sentMessage = await botClient.SendTextMessageAsync(
                        chatId,
                        "Подборки",
                        replyMarkup: replyKeyboardMarkup);
                }
                if (message.Text is "Медитации")
                {
                    var chatId = message.Chat.Id;

                    Console.WriteLine($"Message - Медитации from {chatId}, User - {message.Chat.FirstName + " " + message.Chat.LastName}");

                    ReplyKeyboardMarkup replyKeyboardMarkup = new(new[] { new KeyboardButton[] { "«Сканирование тела»" }, new KeyboardButton[]{"«Трехминутная медитация-передышка»"}, 
                        new KeyboardButton[] { "«Звуки и мысли»" }, new KeyboardButton[] { "«Дружественная медитация»" }, new KeyboardButton[]{ "«Осознанное движение»" } } )
                    {
                        ResizeKeyboard = true
                    };
                    Message sentMessage = await botClient.SendTextMessageAsync(
                        chatId,
                        "Выбирайте понравившуюся и начинайте медитировать",
                        replyMarkup: replyKeyboardMarkup);
                }
                if (message.Text is "«Сканирование тела»")
                {
                    var chatId = message.Chat.Id;

                    Console.WriteLine($"Message - «Сканирование тела» from {chatId}, User - {message.Chat.FirstName + " " + message.Chat.LastName}");

                    ReplyKeyboardMarkup replyKeyboardMarkup = new(new[] { new KeyboardButton[] { "Медитации", "Перейти к подборкам" } })
                    {
                        ResizeKeyboard = true
                    };
                    string[] medLines = System.IO.File.ReadAllLines(@"../../../Медитация «Сканирование тела».txt");
                    for (int i = 0; i < medLines.Length; i++)
                    {
                        Message sentMessage = await botClient.SendTextMessageAsync(
                            chatId,
                            medLines[i],
                            replyMarkup: replyKeyboardMarkup);
                    }
                }
                if (message.Text is "«Трехминутная медитация-передышка»")
                {
                    var chatId = message.Chat.Id;

                    Console.WriteLine($"Message - «Трехминутная медитация-передышка» from {chatId}, User - {message.Chat.FirstName + " " + message.Chat.LastName}");

                    ReplyKeyboardMarkup replyKeyboardMarkup = new(new[] { new KeyboardButton[] { "Медитации", "Перейти к подборкам" } })
                    {
                        ResizeKeyboard = true
                    };
                    string[] medLines = System.IO.File.ReadAllLines(@"../../../«Трехминутная медитация-передышка».txt");
                    for (int i = 0; i < medLines.Length; i++)
                    {
                        Message sentMessage = await botClient.SendTextMessageAsync(
                            chatId,
                            medLines[i],
                            replyMarkup: replyKeyboardMarkup);
                    }
                }
                if (message.Text is "«Звуки и мысли»")
                {
                    var chatId = message.Chat.Id;

                    Console.WriteLine($"Message - «Звуки и мысли» from {chatId}, User - {message.Chat.FirstName + " " + message.Chat.LastName}");

                    ReplyKeyboardMarkup replyKeyboardMarkup = new(new[] { new KeyboardButton[] { "Медитации", "Перейти к подборкам" } })
                    {
                        ResizeKeyboard = true
                    };
                    string[] medLines = System.IO.File.ReadAllLines(@"../../../Звуки и мысли.txt");
                    for (int i = 0; i < medLines.Length; i++)
                    {
                        Message sentMessage = await botClient.SendTextMessageAsync(
                            chatId,
                            medLines[i],
                            replyMarkup: replyKeyboardMarkup);
                    }
                }
                if (message.Text is "«Дружественная медитация»")
                {
                    var chatId = message.Chat.Id;

                    Console.WriteLine($"Message - «Дружественная медитация» from {chatId}, User - {message.Chat.FirstName + " " + message.Chat.LastName}");

                    ReplyKeyboardMarkup replyKeyboardMarkup = new(new[] { new KeyboardButton[] { "Медитации", "Перейти к подборкам" } })
                    {
                        ResizeKeyboard = true
                    };
                    string[] medLines = System.IO.File.ReadAllLines(@"../../../«Дружественная медитация».txt");
                    for (int i = 0; i < medLines.Length; i++)
                    {
                        Message sentMessage = await botClient.SendTextMessageAsync(
                            chatId,
                            medLines[i],
                            replyMarkup: replyKeyboardMarkup);
                    }
                }
                if (message.Text is "«Осознанное движение»")
                {
                    var chatId = message.Chat.Id;

                    Console.WriteLine($"Message - «Осознанное движение» from {chatId}, User - {message.Chat.FirstName + " " + message.Chat.LastName}");

                    ReplyKeyboardMarkup replyKeyboardMarkup = new(new[] { new KeyboardButton[] { "Медитации", "Перейти к подборкам" } })
                    {
                        ResizeKeyboard = true
                    };
                    string[] medLines = System.IO.File.ReadAllLines(@"../../../«Осознанное движение».txt");
                    for (int i = 0; i < medLines.Length; i++)
                    {
                        Message sentMessage = await botClient.SendTextMessageAsync(
                            chatId,
                            medLines[i],
                            replyMarkup: replyKeyboardMarkup);
                    }
                }
                if (message.Text is "Книги")
                {
                    var chatId = message.Chat.Id;

                    Console.WriteLine($"Message - Книги from {chatId}, User - {message.Chat.FirstName + " " + message.Chat.LastName}");

                    ReplyKeyboardMarkup replyKeyboardMarkup = new(new[] { new KeyboardButton[] { "Век тревожности. Страхи, надежды, неврозы и поиски душевного покоя, Скотт Стоссел" },
                        new KeyboardButton[]{"Внутреннее спокойствие. 101 способ справиться с тревогой, страхом и паническими атаками, Таня Петерсон"},
                        new KeyboardButton[] { "Управление тревогой. Системный подход к борьбе с беспокойством на работе и в отношениях, Катлин Смит" },
                        new KeyboardButton[] { "Тревожность. 10 шагов, которые помогут избавиться от  беспокойства, Эдмунд Борн, Лорна Гарано" },
                        new KeyboardButton[]{ "Терапия беспокойства. Как справляться со страхами, тревогами и паническими атаками без лекарств, Дэвид Бернс" },
                        new KeyboardButton[] { "Тревожный мозг. Как успокоить мысли, исцелить разум и вернуть контроль над собственной жизнью, Джозеф А. Аннибали" },
                        new KeyboardButton[] { "Свобода от тревоги. Справься с тревогой, пока она не расправилась с тобой, Роберт Лихи" },
                        new KeyboardButton[]{ "Беспокойный человек. Как снизить тревожность и меньше волноваться, Стюарт Геддес" },
                        new KeyboardButton[] { "Я с тобой. 149 простых советов как справиться с тревогой, беспокойством и паникой, Гед Дженкинс-Омар" },
                        new KeyboardButton[]{ "Будь спок. Проверенные техники управления тревогой, Джилл Уэбер" }})
                    {
                        ResizeKeyboard = true
                    };
                    Message sentMessage = await botClient.SendTextMessageAsync(
                        chatId,
                        "Вам представлены названия, чтобы узнать о книге больше выберите одно из них",
                        replyMarkup: replyKeyboardMarkup);
                }
                if (message.Text is "Век тревожности. Страхи, надежды, неврозы и поиски душевного покоя, Скотт Стоссел")
                {
                    var chatId = message.Chat.Id;

                    Console.WriteLine($"Message - Книга 1 from {chatId}, User - {message.Chat.FirstName + " " + message.Chat.LastName}");

                    ReplyKeyboardMarkup replyKeyboardMarkup = new(new[] { new KeyboardButton[] { "Книги", "Перейти к подборкам" } })
                    {
                        ResizeKeyboard = true
                    };
                    Message sentMessage = await botClient.SendPhotoAsync(
                        chatId,
                        photo: InputFile.FromUri("https://cv4.litres.ru/pub/c/cover_max1500/19235942.jpg"),
                        replyMarkup: replyKeyboardMarkup);
                    sentMessage = await botClient.SendTextMessageAsync(
                        chatId,
                        bookNames[0],
                        replyMarkup: replyKeyboardMarkup);
                }
                if (message.Text is "Внутреннее спокойствие. 101 способ справиться с тревогой, страхом и паническими атаками, Таня Петерсон")
                {
                    var chatId = message.Chat.Id;

                    Console.WriteLine($"Message - Книга 2 from {chatId}, User - {message.Chat.FirstName + " " + message.Chat.LastName}");

                    ReplyKeyboardMarkup replyKeyboardMarkup = new(new[] { new KeyboardButton[] { "Книги", "Перейти к подборкам" } })
                    {
                        ResizeKeyboard = true
                    };
                    Message sentMessage = await botClient.SendPhotoAsync(
                        chatId,
                        photo: InputFile.FromUri("https://cv9.litres.ru/pub/c/cover_max1500/63910791.jpg"),
                        replyMarkup: replyKeyboardMarkup);
                    sentMessage = await botClient.SendTextMessageAsync(
                        chatId,
                        bookNames[1],
                        replyMarkup: replyKeyboardMarkup);
                }
                if (message.Text is "Управление тревогой. Системный подход к борьбе с беспокойством на работе и в отношениях, Катлин Смит")
                {
                    var chatId = message.Chat.Id;

                    Console.WriteLine($"Message - Книга 3 from {chatId}, User - {message.Chat.FirstName + " " + message.Chat.LastName}");

                    ReplyKeyboardMarkup replyKeyboardMarkup = new(new[] { new KeyboardButton[] { "Книги", "Перейти к подборкам" } })
                    {
                        ResizeKeyboard = true
                    };
                    Message sentMessage = await botClient.SendPhotoAsync(
                        chatId,
                        photo: InputFile.FromUri("https://cv2.litres.ru/pub/c/cover_max1500/64503026.jpg"),
                        replyMarkup: replyKeyboardMarkup);
                    sentMessage = await botClient.SendTextMessageAsync(
                        chatId,
                        bookNames[2],
                        replyMarkup: replyKeyboardMarkup);
                }
                if (message.Text is "Тревожность. 10 шагов, которые помогут избавиться от  беспокойства, Эдмунд Борн, Лорна Гарано")
                {
                    var chatId = message.Chat.Id;

                    Console.WriteLine($"Message - Книга 4 from {chatId}, User - {message.Chat.FirstName + " " + message.Chat.LastName}");

                    ReplyKeyboardMarkup replyKeyboardMarkup = new(new[] { new KeyboardButton[] { "Книги", "Перейти к подборкам" } })
                    {
                        ResizeKeyboard = true
                    };
                    Message sentMessage = await botClient.SendPhotoAsync(
                        chatId,
                        photo: InputFile.FromUri("https://cv2.litres.ru/pub/c/cover_200/65909322.jpg"),
                        replyMarkup: replyKeyboardMarkup);
                    sentMessage = await botClient.SendTextMessageAsync(
                        chatId,
                        bookNames[3],
                        replyMarkup: replyKeyboardMarkup);
                }
                if (message.Text is "Терапия беспокойства. Как справляться со страхами, тревогами и паническими атаками без лекарств, Дэвид Бернс")
                {
                    var chatId = message.Chat.Id;

                    Console.WriteLine($"Message - Книга 5 from {chatId}, User - {message.Chat.FirstName + " " + message.Chat.LastName}");

                    ReplyKeyboardMarkup replyKeyboardMarkup = new(new[] { new KeyboardButton[] { "Книги", "Перейти к подборкам" } })
                    {
                        ResizeKeyboard = true
                    };
                    Message sentMessage = await botClient.SendPhotoAsync(
                        chatId,
                        photo: InputFile.FromUri("https://cv5.litres.ru/pub/c/cover_max1500/64476057.jpg"),
                        replyMarkup: replyKeyboardMarkup);
                    sentMessage = await botClient.SendTextMessageAsync(
                        chatId,
                        bookNames[4],
                        replyMarkup: replyKeyboardMarkup);
                }
                if (message.Text is "Тревожный мозг. Как успокоить мысли, исцелить разум и вернуть контроль над собственной жизнью, Джозеф А. Аннибали")
                {
                    var chatId = message.Chat.Id;

                    Console.WriteLine($"Message - Книга 6 from {chatId}, User - {message.Chat.FirstName + " " + message.Chat.LastName}");

                    ReplyKeyboardMarkup replyKeyboardMarkup = new(new[] { new KeyboardButton[] { "Книги", "Перейти к подборкам" } })
                    {
                        ResizeKeyboard = true
                    };
                    Message sentMessage = await botClient.SendPhotoAsync(
                        chatId,
                        photo: InputFile.FromUri("https://cdn.eksmo.ru/v2/ITD000000000843865/COVER/cover1__w600.jpg"),
                        replyMarkup: replyKeyboardMarkup);
                    sentMessage = await botClient.SendTextMessageAsync(
                        chatId,
                        bookNames[5],
                        replyMarkup: replyKeyboardMarkup);
                }
                if (message.Text is "Свобода от тревоги. Справься с тревогой, пока она не расправилась с тобой, Роберт Лихи")
                {
                    var chatId = message.Chat.Id;

                    Console.WriteLine($"Message - Книга 7 from {chatId}, User - {message.Chat.FirstName + " " + message.Chat.LastName}");

                    ReplyKeyboardMarkup replyKeyboardMarkup = new(new[] { new KeyboardButton[] { "Книги", "Перейти к подборкам" } })
                    {
                        ResizeKeyboard = true
                    };
                    Message sentMessage = await botClient.SendPhotoAsync(
                        chatId,
                        photo: InputFile.FromUri("https://cdn.img-gorod.ru/310x500/nomenclature/26/041/2604153.jpg"),
                        replyMarkup: replyKeyboardMarkup);
                    sentMessage = await botClient.SendTextMessageAsync(
                        chatId,
                        bookNames[6],
                        replyMarkup: replyKeyboardMarkup);
                }
                if (message.Text is "Беспокойный человек. Как снизить тревожность и меньше волноваться, Стюарт Геддес")
                {
                    var chatId = message.Chat.Id;

                    Console.WriteLine($"Message - Книга 8 from {chatId}, User - {message.Chat.FirstName + " " + message.Chat.LastName}");

                    ReplyKeyboardMarkup replyKeyboardMarkup = new(new[] { new KeyboardButton[] { "Книги", "Перейти к подборкам" } })
                    {
                        ResizeKeyboard = true
                    };
                    Message sentMessage = await botClient.SendPhotoAsync(
                        chatId,
                        photo: InputFile.FromUri("https://cv2.litres.ru/pub/c/cover_max1500/55746525.jpg"),
                        replyMarkup: replyKeyboardMarkup);
                    sentMessage = await botClient.SendTextMessageAsync(
                        chatId,
                        bookNames[7],
                        replyMarkup: replyKeyboardMarkup);
                }
                if (message.Text is "Я с тобой. 149 простых советов как справиться с тревогой, беспокойством и паникой, Гед Дженкинс-Омар")
                {
                    var chatId = message.Chat.Id;

                    Console.WriteLine($"Message - Книга 9 from {chatId}, User - {message.Chat.FirstName + " " + message.Chat.LastName}");

                    ReplyKeyboardMarkup replyKeyboardMarkup = new(new[] { new KeyboardButton[] { "Книги", "Перейти к подборкам" } })
                    {
                        ResizeKeyboard = true
                    };
                    Message sentMessage = await botClient.SendPhotoAsync(
                        chatId,
                        photo: InputFile.FromUri("https://cv1.litres.ru/pub/c/cover_max1500/67365210.jpg"),
                        replyMarkup: replyKeyboardMarkup);
                    sentMessage = await botClient.SendTextMessageAsync(
                        chatId,
                        bookNames[8],
                        replyMarkup: replyKeyboardMarkup);
                }
                if (message.Text is "Будь спок. Проверенные техники управления тревогой, Джилл Уэбер")
                {
                    var chatId = message.Chat.Id;

                    Console.WriteLine($"Message - Книга 10 from {chatId}, User - {message.Chat.FirstName + " " + message.Chat.LastName}");

                    ReplyKeyboardMarkup replyKeyboardMarkup = new(new[] { new KeyboardButton[] { "Книги", "Перейти к подборкам" } })
                    {
                        ResizeKeyboard = true
                    };
                    Message sentMessage = await botClient.SendPhotoAsync(
                        chatId,
                        photo: InputFile.FromUri("https://img4.labirint.ru/rc/95d3ca8a9b72e57b15f794bad7cce68a/363x561q80/books85/844154/cover.jpg?1653330452"),
                        replyMarkup: replyKeyboardMarkup);
                    sentMessage = await botClient.SendTextMessageAsync(
                        chatId,
                        bookNames[9],
                        replyMarkup: replyKeyboardMarkup);
                }
                if (message.Text is "Подкасты")
                {
                    var chatId = message.Chat.Id;

                    Console.WriteLine($"Message - Подкасты from {chatId}, User - {message.Chat.FirstName + " " + message.Chat.LastName}");

                    ReplyKeyboardMarkup replyKeyboardMarkup = new(new[] { new KeyboardButton[] { "«НОРМ»" }, new KeyboardButton[]{"«Среда для медитаций»"},
                        new KeyboardButton[] { "«Ты — это важно»" }, new KeyboardButton[] { "«Куда бежишь?»" }, new KeyboardButton[]{ "«Эмоциональный интеллигент»" },
                    new KeyboardButton[]{ "«Год, прожитый не спеша»" } })
                    {
                        ResizeKeyboard = true
                    };
                    Message sentMessage = await botClient.SendTextMessageAsync(
                        chatId,
                        "Выбирайте понравившийся и начинайте слушать",
                        replyMarkup: replyKeyboardMarkup);
                }
                if (message.Text is "«НОРМ»")
                {
                    var chatId = message.Chat.Id;

                    Console.WriteLine($"Message - «НОРМ» from {chatId}, User - {message.Chat.FirstName + " " + message.Chat.LastName}");

                    ReplyKeyboardMarkup replyKeyboardMarkup = new(new[] { new KeyboardButton[] { "Подкасты", "Перейти к подборкам" } })
                    {
                        ResizeKeyboard = true
                    };
                    Message sentMessage = await botClient.SendTextMessageAsync(
                        chatId,
                        podcasts[0],
                        replyMarkup: replyKeyboardMarkup);
                    sentMessage = await botClient.SendTextMessageAsync(
                        chatId,
                        podcasts[1],
                        replyMarkup: replyKeyboardMarkup);
                }
                if (message.Text is "«Среда для медитаций»")
                {
                    var chatId = message.Chat.Id;

                    Console.WriteLine($"Message - «Среда для медитаций» from {chatId}, User - {message.Chat.FirstName + " " + message.Chat.LastName}");

                    ReplyKeyboardMarkup replyKeyboardMarkup = new(new[] { new KeyboardButton[] { "Подкасты", "Перейти к подборкам" } })
                    {
                        ResizeKeyboard = true
                    };
                    Message sentMessage = await botClient.SendTextMessageAsync(
                        chatId,
                        podcasts[2],
                        replyMarkup: replyKeyboardMarkup);
                    sentMessage = await botClient.SendTextMessageAsync(
                        chatId,
                        podcasts[3],
                        replyMarkup: replyKeyboardMarkup);
                }
                if (message.Text is "«Ты — это важно»")
                {
                    var chatId = message.Chat.Id;

                    Console.WriteLine($"Message - «Ты — это важно» from {chatId}, User - {message.Chat.FirstName + " " + message.Chat.LastName}");

                    ReplyKeyboardMarkup replyKeyboardMarkup = new(new[] { new KeyboardButton[] { "Подкасты", "Перейти к подборкам" } })
                    {
                        ResizeKeyboard = true
                    };
                    Message sentMessage = await botClient.SendTextMessageAsync(
                        chatId,
                        podcasts[4],
                        replyMarkup: replyKeyboardMarkup);
                    sentMessage = await botClient.SendTextMessageAsync(
                        chatId,
                        podcasts[5],
                        replyMarkup: replyKeyboardMarkup);
                }
                if (message.Text is "«Куда бежишь?»")
                {
                    var chatId = message.Chat.Id;

                    Console.WriteLine($"Message - «Куда бежишь?» from {chatId}, User - {message.Chat.FirstName + " " + message.Chat.LastName}");

                    ReplyKeyboardMarkup replyKeyboardMarkup = new(new[] { new KeyboardButton[] { "Подкасты", "Перейти к подборкам" } })
                    {
                        ResizeKeyboard = true
                    };
                    Message sentMessage = await botClient.SendTextMessageAsync(
                        chatId,
                        podcasts[6],
                        replyMarkup: replyKeyboardMarkup);
                    sentMessage = await botClient.SendTextMessageAsync(
                        chatId,
                        podcasts[7],
                        replyMarkup: replyKeyboardMarkup);
                }
                if (message.Text is "«Эмоциональный интеллигент»")
                {
                    var chatId = message.Chat.Id;

                    Console.WriteLine($"Message - «Эмоциональный интеллигент» from {chatId}, User - {message.Chat.FirstName + " " + message.Chat.LastName}");

                    ReplyKeyboardMarkup replyKeyboardMarkup = new(new[] { new KeyboardButton[] { "Подкасты", "Перейти к подборкам" } })
                    {
                        ResizeKeyboard = true
                    };
                    Message sentMessage = await botClient.SendTextMessageAsync(
                        chatId,
                        podcasts[8],
                        replyMarkup: replyKeyboardMarkup);
                    sentMessage = await botClient.SendTextMessageAsync(
                        chatId,
                        podcasts[9],
                        replyMarkup: replyKeyboardMarkup);
                }
                if (message.Text is "«Год, прожитый не спеша»")
                {
                    var chatId = message.Chat.Id;

                    Console.WriteLine($"Message - «Год, прожитый не спеша» from {chatId}, User - {message.Chat.FirstName + " " + message.Chat.LastName}");

                    ReplyKeyboardMarkup replyKeyboardMarkup = new(new[] { new KeyboardButton[] { "Подкасты", "Перейти к подборкам" } })
                    {
                        ResizeKeyboard = true
                    };
                    Message sentMessage = await botClient.SendTextMessageAsync(
                        chatId,
                        podcasts[10],
                        replyMarkup: replyKeyboardMarkup);
                    sentMessage = await botClient.SendTextMessageAsync(
                        chatId,
                        podcasts[11],
                        replyMarkup: replyKeyboardMarkup);
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