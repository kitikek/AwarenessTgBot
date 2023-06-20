using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types;

namespace AwarenessTelegramBot
{
    public class ForMessages
    {
        public static (int, int) GetResult(int[] array)
        {
            int[] straightQuestions = new int[] { 3, 4, 6, 7, 9, 12, 13, 14, 17, 18 };
            int[] reverseQuestions = new int[] { 1, 2, 5, 8, 10, 11, 15, 16, 19, 20 };

            int[] straightPersQuestions = new int[] { 22, 23, 24, 25, 28, 29, 31, 32, 34, 35, 37, 38, 40 };
            int[] reversePersQuestions = new int[] { 21, 26, 27, 30, 33, 36, 39 };

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
            int reactAnx = straightQuestions.Sum() - reverseQuestions.Sum() + 50;
            int persAnx = straightPersQuestions.Sum() - reversePersQuestions.Sum() + 35;
            return (reactAnx, persAnx);
        }
        public static string IdentifyResult(int res)
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
        public static string GetInterpretation(string str)
        {
            if (str == "низкий😌")
                return System.IO.File.ReadAllText(@"../../../ResultUnder30.txt");
            if (str == "умеренный😐")
                return System.IO.File.ReadAllText(@"../../../ResultUnder45.txt");
            if (str == "высокий😧")
                return System.IO.File.ReadAllText(@"../../../ResultOver45.txt");
            else
                return "Вам стоит пройти тест снова, интерпретации для такого уровня тревожности нет.";
        }
        public static string GetRandomQuote()
        {
            string[] quotes = System.IO.File.ReadAllLines(@"../../../Цитаты.txt");
            Random rnd = new Random();
            int i = rnd.Next(quotes.Length);
            return quotes[i];
        }
    }
}
