using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            if (!File.Exists("databaseBETA.xml")) {
                File.WriteAllText("databaseBETA.xml", "<?xml version=\"1.0\" encoding=\"utf-8\"?>" + Environment.NewLine + "<root>" + Environment.NewLine + "</root>");
            }
            XElement xElement = XElement.Load("databaseBETA.xml",
                LoadOptions.SetBaseUri | LoadOptions.SetLineInfo);
            var database = xElement.Descendants("Person");
            foreach(var a in database)
            {
                var personW = a.Elements();
                    persons.Add(new Person { FirstName= personW.ElementAt(0).Value,
                        SecondName = personW.ElementAt(1).Value,
                        gender =int.Parse(personW.ElementAt(2).Value),
                        temperament = personW.ElementAt(3).Value,
                        bd =int.Parse(personW.ElementAt(4).Value),
                        bm = int.Parse(personW.ElementAt(5).Value),
                        by = int.Parse(personW.ElementAt(6).Value),
                        userid = int.Parse(personW.ElementAt(7).Value),
                        stage = int.Parse(personW.ElementAt(8).Value),
                        answer1 = personW.ElementAt(8).Value,
                        answer2 = personW.ElementAt(9).Value,
                        answer3 = personW.ElementAt(10).Value,
                        answer4 = personW.ElementAt(11).Value,
                        answer5 = personW.ElementAt(12).Value,
                    });
            }
            gamesphoto.Add(new GamePhoto {
                firstPath = "GAME1.jpg",
                secondPath = "GAME2.jpg",
                thirdPath = "GAME3.jpg",
                fourPath = "GAME4.jpg",
                count = 0,
                user1 = -1,
                user2 = -1,
            user3 = -1,
            user4 = -1


        });
        }
        public class Person
        {
            public string FirstName;
            public string SecondName;
            public int gender;
            public string temperament;
            public int bd;
            public int bm;
            public int by;
            public int userid;
            public int stage;
            public string answer1;
            public string answer2;
            public string answer3;
            public string answer4;
            public string answer5;
        }


        public class GamePhoto
        {
            public string firstPath;
            public string secondPath;
            public string thirdPath;
            public string fourPath;
            public int user1;
            public int user2;
            public int user3;
            public int user4;
            public long chatID1;
            public long chatID2;
            public long chatID3;
            public long chatID4;
            public int count;

        }

        List<GamePhoto> gamesphoto = new List<GamePhoto>(); 

        List<Person> persons = new List<Person>();
    private async void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            var worker = sender as BackgroundWorker;
            var key = e.Argument as String;
            try
            {
                var Bot = new Telegram.Bot.TelegramBotClient(key);
                Bot.SetWebhookAsync("").Wait();
              
                int offset = 0;
                int stageM = 0;
                while (true)
                {
                    var updates = await Bot.GetUpdatesAsync(offset); 

                    foreach (var update in updates)
                    {
                        var message = update.Message;
                        stageM = 0;
                        int index = -1;
                        for (int i = 0; i < persons.Count; i++)
                        {
                            if (persons[i].userid == message.From.Id)
                            {
                                index = i;
                                break;
                            }
                        }
                        if (message.Type == Telegram.Bot.Types.Enums.MessageType.TextMessage)
                        {
                            if (message.Text == "/reset" && index >= 0)
                            {
                                persons.RemoveAt(index);
                                await Bot.SendTextMessageAsync(message.Chat.Id, "Ваш профиль очищен.");
                            }
                            else if (message.Text == "/reset")
                            {
                                await Bot.SendTextMessageAsync(message.Chat.Id, "Профиль чист.");
                            }
                            else
                            {
                                if (index >= 0)
                                {
                                    stageM = persons[index].stage;
                                }
                                else
                                {
                                    persons.Add(new Person
                                    {
                                        FirstName = message.From.FirstName,
                                        SecondName = message.From.LastName,
                                        gender = 0,
                                        temperament = "",
                                        bd = 0,
                                        bm = 0,
                                        by = 0,
                                        userid = message.From.Id,
                                        stage = 0
                                    });
                                    index = persons.Count - 1;
                                    stageM = 0;
                                }
                                switch (stageM)
                                {
                                    case 0:
                                        Telegram.Bot.Types.FileToSend file = new Telegram.Bot.Types.FileToSend();
                                        file.Content = File.Open("hello.jpg", FileMode.Open);
                                        file.Filename = "hello.jpg";
                                        await Bot.SendPhotoAsync(message.Chat.Id, file);
                                        await Bot.SendTextMessageAsync(message.Chat.Id, "Привет, " + message.From.FirstName + "!\nЯ Миша.\nЯ постараюсь быть тебе другом.\nРасскажи немного о себе😊");
                                        var keyboard = new Telegram.Bot.Types.ReplyMarkups.ReplyKeyboardMarkup
                                        {
                                            Keyboard = new[] {
                                                new[] // row 1
                                                {
                                                    new Telegram.Bot.Types.KeyboardButton("👨Мужской"),
                                                    new Telegram.Bot.Types.KeyboardButton("👱‍♀Женский")
                                                },
                                            },
                                            ResizeKeyboard = true,
                                            OneTimeKeyboard = true
                                        };

                                        await Bot.SendTextMessageAsync(message.Chat.Id, "Укажи свой пол", ParseMode.Default, false, false, 0, keyboard);
                                        stageM++;
                                        break;
                                    case 1:
                                        if (message.Text == "👨Мужской")
                                        {
                                            persons[index].gender = 1;
                                        }
                                        else if (message.Text == "👱‍♀Женский")
                                        {
                                            persons[index].gender = 2;
                                        }
                                        else
                                        {
                                            stageM--;
                                            break;
                                        }
                                        var keyboard1 = new Telegram.Bot.Types.ReplyMarkups.ReplyKeyboardMarkup
                                        {
                                            Keyboard = new[] {
                                                new[] // row 1
                                                {
                                                    new Telegram.Bot.Types.KeyboardButton("👍Хорошо"),
                                                    new Telegram.Bot.Types.KeyboardButton("😞Плохо"),
                                                    new Telegram.Bot.Types.KeyboardButton("😐Не очень")
                                                },
                                            },
                                            ResizeKeyboard = true,
                                            OneTimeKeyboard = true
                                        };

                                        await Bot.SendTextMessageAsync(message.Chat.Id, "Как прошел твой день?", ParseMode.Default, false, false, 0, keyboard1);
                                        stageM++;
                                        break;
                                    case 2:
                                        bool inGame = false;
                                        int indexgame = -1;
                                        int indexGamer = 0;
                                        string pathGamePhoto="";
                                        for(int i = 0; i < gamesphoto.Count; i++) { 
                                            if (gamesphoto[i].user1 == message.From.Id
                                                || gamesphoto[i].user2 == message.From.Id ||
                                                gamesphoto[i].user3 == message.From.Id ||
                                                    gamesphoto[i].user4 == message.From.Id)
                                            {
                                                inGame = true;
                                                indexgame = i;
                                            }
                                        }
                                        if (!inGame)
                                        await Bot.SendTextMessageAsync(message.Chat.Id, "Я понял. Давай сыграем в игру, где надо будет собрать части картинки в одну. Я отправлю картинки всем в твоей команде и ты должен найти недостающие части.");
                                        else if (gamesphoto[indexgame].count == 4) {
                                         if (message.Text.ToLower() == "кот" || message.Text.ToLower() == "котик" || message.Text.ToLower() == "кошка")
                                            {
                                                await Bot.SendTextMessageAsync(message.Chat.Id, "Правильно!");
                                                var keyboard6 = new Telegram.Bot.Types.ReplyMarkups.ReplyKeyboardMarkup
                                                {
                                                    Keyboard = new[] {
                                                new[] // row 1
                                                {
                                                    new Telegram.Bot.Types.KeyboardButton("Учился в школе"),
                                                    new Telegram.Bot.Types.KeyboardButton("Гуляли с друзьями"),
                                                    new Telegram.Bot.Types.KeyboardButton("Справляли день рождения"),
                                                                                                        new Telegram.Bot.Types.KeyboardButton("Ничего")
                                                },
                                            },
                                                    ResizeKeyboard = true,
                                                    OneTimeKeyboard = true
                                                };

                                                await Bot.SendTextMessageAsync(message.Chat.Id, "Что запомнилось тебе за этот день?", ParseMode.Default, false, false, 0, keyboard6);
                                                stageM++;
                                                break;
                                            } else
                                            {
                                                await Bot.SendTextMessageAsync(message.Chat.Id, "Подумай еще!😊");
                                                break;
                                            }
                                        }
                                        if (!inGame) {
                                            for (int i = 0; i < gamesphoto.Count; i++) { 
                                                if (gamesphoto[i].count != 4)
                                                {
                                                    if (gamesphoto[i].user1 == -1)
                                                    {
                                                        gamesphoto[i].chatID1 = message.Chat.Id;
                                                        gamesphoto[i].user1 = message.From.Id;
                                                        pathGamePhoto = gamesphoto[i].firstPath;
                                                    }

                                                    else if (gamesphoto[i].user2 == -1) {
                                                        indexGamer = 1;
                                                        gamesphoto[i].chatID2 = message.Chat.Id;
                                                        gamesphoto[i].user2 = message.From.Id;
                                                        pathGamePhoto = gamesphoto[i].secondPath;
                                                    }
                                                    else if (gamesphoto[i].user3 == -1) {
                                                        indexGamer = 2;
                                                        gamesphoto[i].chatID3 = message.Chat.Id;
                                                        gamesphoto[i].user3 = message.From.Id;
                                                        pathGamePhoto = gamesphoto[i].thirdPath;
                                                    }
                                                    else if (gamesphoto[i].user4 == -1) {
                                                        indexGamer = 3;
                                                        gamesphoto[i].chatID4 = message.Chat.Id;
                                                        gamesphoto[i].user4 = message.From.Id;
                                                        pathGamePhoto = gamesphoto[i].fourPath;
                                                    }
                                                    inGame = true;
                                                    indexgame = i;
                                                    gamesphoto[indexgame].count++;
                                                    if (gamesphoto[indexgame].count == 4)
                                                    {
                                                            Telegram.Bot.Types.FileToSend file1 = new Telegram.Bot.Types.FileToSend();
                                                            file1.Content = File.Open(gamesphoto[indexgame].firstPath, FileMode.Open);
                                                            file1.Filename = gamesphoto[indexgame].firstPath;
                                                            await Bot.SendPhotoAsync(gamesphoto[indexgame].chatID1, file1);
                                                            await Bot.SendTextMessageAsync(gamesphoto[indexgame].chatID1, "Найди другие части картинки и скажи что на картинке. Удачи!😊");
                                                       
                                                            file1.Content = File.Open(gamesphoto[indexgame].secondPath, FileMode.Open);
                                                            file1.Filename = gamesphoto[indexgame].secondPath;
                                                            await Bot.SendPhotoAsync(gamesphoto[indexgame].chatID2, file1);
                                                            await Bot.SendTextMessageAsync(gamesphoto[indexgame].chatID2, "Найди другие части картинки и скажи что на картинке. Удачи!😊");
                                                           
                                                            file1.Content = File.Open(gamesphoto[indexgame].thirdPath, FileMode.Open);
                                                            file1.Filename = gamesphoto[indexgame].thirdPath;
                                                            await Bot.SendPhotoAsync(gamesphoto[indexgame].chatID3, file1);
                                                            await Bot.SendTextMessageAsync(gamesphoto[indexgame].chatID3, "Найди другие части картинки и скажи что на картинке. Удачи!😊");
                                                           
                                                            file1.Content = File.Open(gamesphoto[indexgame].fourPath, FileMode.Open);
                                                            file1.Filename = gamesphoto[indexgame].fourPath;
                                                            await Bot.SendPhotoAsync(gamesphoto[indexgame].chatID4, file1);
                                                            await Bot.SendTextMessageAsync(gamesphoto[indexgame].chatID4, "Найди другие части картинки и скажи что на картинке. Удачи!😊");
                                                        

                                                        break;
                                                    }
                                                }
                                            }
                                        }
                                        if (!inGame)
                                        {
                                            await Bot.SendTextMessageAsync(message.Chat.Id, "Игр пока нет😊");
                                            
                                        }else {
                                            if (gamesphoto[indexgame].count != 4)
                                            {
                                                await Bot.SendTextMessageAsync(message.Chat.Id, "Подожди немного, я поищу тебе команду😊");
                                            }
                                        }
                                        break;
                                  
                                    case 3:
                                        persons[index].answer2 = message.Text;
                                        var keyboard3 = new Telegram.Bot.Types.ReplyMarkups.ReplyKeyboardMarkup
                                        {
                                            Keyboard = new[] {
                                                new[] // row 1
                                                {
                                                    new Telegram.Bot.Types.KeyboardButton("Да"),
                                                    new Telegram.Bot.Types.KeyboardButton("Нет"),

                                                },
                                            },
                                            ResizeKeyboard = true,
                                            OneTimeKeyboard = true
                                        };

                                        await Bot.SendTextMessageAsync(message.Chat.Id, "Понравилось ли тебе сегодняшнее задание?", ParseMode.Default, false, false, 0, keyboard3);
                                        stageM++;
                                        break;
                                    case 4:
                                        persons[index].answer3 = message.Text;

                                        await Bot.SendTextMessageAsync(message.Chat.Id, "Какое задание ты сегодня выполнял?");
                                        stageM++;
                                        break;
                                    case 5:
                                        persons[index].answer4 = message.Text;
                                        var keyboard5 = new Telegram.Bot.Types.ReplyMarkups.ReplyKeyboardMarkup
                                        {
                                            Keyboard = new[] {
                                                new[] // row 1
                                                {
                                                    new Telegram.Bot.Types.KeyboardButton("Да"),
                                                    new Telegram.Bot.Types.KeyboardButton("Нет"),

                                                },
                                            },
                                            ResizeKeyboard = true,
                                            OneTimeKeyboard = true
                                        };
                                        await Bot.SendTextMessageAsync(message.Chat.Id, "У тебя есть чем поделиться со мной?", ParseMode.Default, false, false, 0, keyboard5);
                                        stageM++;
                                        break;
                                    case 6:
                                        if (message.Text.ToLower() == "нет")
                                        {
                                            await Bot.SendTextMessageAsync(message.Chat.Id, "До завтра!");
                                        }
                                        else if (message.Text.ToLower() == "да")
                                        {
                                            await Bot.SendTextMessageAsync(message.Chat.Id, "Излагай свои мысли");
                                        }
                                        stageM++;
                                        break;
                                    case 7:
                                        persons[index].answer5 += message.Text + Environment.NewLine;
                                        break;

                                };
                                persons[index].stage = stageM;
                            }
                            offset = update.Id + 1;

                        }

                    }
                }
            }
            catch (Telegram.Bot.Exceptions.ApiRequestException ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            var text = textBox1.Text;
            if (this.backgroundWorker1.IsBusy != true)
            {
                this.backgroundWorker1.RunWorkerAsync(text);
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            string databasetoSave = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +Environment.NewLine+ "<root>"+ Environment.NewLine;
            foreach (var a in persons)
            {
                databasetoSave += "<Person>"+ Environment.NewLine;
                databasetoSave += "<FirstName>" + a.FirstName + "</FirstName>"+ Environment.NewLine;
                databasetoSave += "<SecondName>" + a.SecondName + "</SecondName>"+ Environment.NewLine;
                databasetoSave += "<gender>" + a.gender.ToString() + "</gender>"+ Environment.NewLine;
                databasetoSave += "<temperament>" + a.temperament + "</temperament>"+ Environment.NewLine;
                databasetoSave += "<bd>" + a.bd + "</bd>"+ Environment.NewLine;
                databasetoSave += "<bm>" + a.bm + "</bm>"+Environment.NewLine;
                databasetoSave += "<by>" + a.by + "</by>"+Environment.NewLine;
                databasetoSave += "<userid>" + a.userid + "</userid>"+ Environment.NewLine;
                databasetoSave += "<stage>" + a.stage + "</stage>" + Environment.NewLine;
                databasetoSave += "<answer1>" + a.answer1 + "</answer1>" + Environment.NewLine;
                databasetoSave += "<answer2>" + a.answer2+ "</answer2>" + Environment.NewLine;
                databasetoSave += "<answer3>" + a.answer3 + "</answer3>" + Environment.NewLine;
                databasetoSave += "<answer4>" + a.answer4 + "</answer4>" + Environment.NewLine;
                databasetoSave += "<answer5>" + a.answer5 + "</answer5>" + Environment.NewLine;
                databasetoSave += "</Person>"+ Environment.NewLine;
            }
            databasetoSave += "</root>";
            File.WriteAllText("databaseBETA.xml", databasetoSave);
        }
    }
}
