using IStemmer = Syn.Bot.Oscova.Interfaces.IStemmer;

namespace TelegramNureBot.WPF.Helper
{
    public class RussianStemmer:IStemmer
    {
        public string Stem(string word)
        {
            return new Iveonik.Stemmers.RussianStemmer().Stem(word);
        }
    }
}