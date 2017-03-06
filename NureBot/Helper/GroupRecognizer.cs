using System.Text.RegularExpressions;
using Syn.Bot.Oscova;
using Syn.Bot.Oscova.Collection;
using Syn.Bot.Oscova.Entities;
using Syn.Bot.Oscova.Interfaces;

namespace TelegramNureBot.Helper
{
    public class GroupRecognizer:IEntityRecognizer
    {
        public EntityCollection Parse(Request request)
        {
            var regex = new Regex(@"[A-Za-zА-Яа-я]{2,}-\d\d-\d+");
            var entities = new EntityCollection();
            foreach (Match match in regex.Matches(request.NormalizedText))
            {
                var entity = new Entity("groupName", match.Value){Index = match.Index};
                entities.Add(entity);
            }
            return entities;
        }

        public string Type => "groupName";
    }
}