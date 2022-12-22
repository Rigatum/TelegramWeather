using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types.Enums;
using Newtonsoft.Json;
using TelegramWeather;

namespace TelegramBot
{
    class Program
    {
        static ITelegramBotClient bot = new TelegramBotClient("5983782369:AAHyd5o5ZjoWC_Op-KzHsY4Se8bw8bIOL2o");
        static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            System.Console.WriteLine(JsonConvert.SerializeObject(update) + '\n');

            if(update?.Message?.ReplyToMessage?.Text == "Ваша геолокация")
            {
                var str = JsonConvert.SerializeObject(update).ToString();
                Geolocation.Root deserializedGeolocation = JsonConvert.DeserializeObject<Geolocation.Root>(str);
                System.Console.WriteLine(deserializedGeolocation?.message.location.latitude);
            }

            if(update.Type == UpdateType.Message && update?.Message?.Text != null)
            {
                await HandleMessage(botClient, update.Message);
                return;
            }
            if(update.Type == UpdateType.CallbackQuery)
            {
                await HandleCallbackQuery(botClient, update.CallbackQuery);
                return;
            }
        }

        static async Task HandleMessage(ITelegramBotClient botClient, Message message)
        {
            if(message.Text.ToLower() == "/start")
            {
                ReplyKeyboardMarkup keyboard = new(new[]
                {
                    new KeyboardButton[] {"/weather", "тест"},
                    new KeyboardButton[] {"тест", "тест"}
                })
                {
                    ResizeKeyboard = true
                };
                await botClient.SendTextMessageAsync(message.Chat.Id, "Бот начал работу", replyMarkup: keyboard);
                return;
            }
            if(message.Text.ToLower() == "/weather")
            {
                KeyboardButton button = KeyboardButton.WithRequestLocation("Отправить геолокацию");
                ReplyKeyboardMarkup keyboard = new ReplyKeyboardMarkup(button);
                await botClient.SendTextMessageAsync(message.Chat.Id, "Ваша геолокация", replyMarkup: keyboard);
                return;
            }
            await botClient.SendTextMessageAsync(message.Chat.Id, $"You said: \n{message.Text}");
        }
        static async Task HandleCallbackQuery(ITelegramBotClient botClient, CallbackQuery callbackQuery)
        {

        }

        static Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var ErrorMessage = exception switch
            {
                ApiRequestException apiRequestException
                    => $"Ошибка телеграм АПИ:{apiRequestException.ErrorCode}\n{apiRequestException.Message}",
                _ => exception.ToString()
            };
            System.Console.WriteLine(ErrorMessage);
            return Task.CompletedTask;
        }

        
        static void Main(string[] args)
        {
            System.Console.WriteLine("Запущен бот" + bot.GetMeAsync().Result.FirstName + '\n');

            var cts = new CancellationTokenSource();
            var cancellationToken = cts.Token;
            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = { }, // receive all update types
            };
            bot.StartReceiving(
                HandleUpdateAsync,
                HandleErrorAsync,
                receiverOptions,
                cancellationToken
            );
            Console.ReadLine();
        }
    }
}