using Syn.Bot.Oscova.Interfaces;

namespace TelegramNureBot.Helper
{
    public class RussianStemmer:IStemmer
    {
        public string Stem(string word)
        {
            return new RussianStemmer().Stem(word);
        }
    }
}