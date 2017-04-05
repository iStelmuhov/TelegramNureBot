using System;
using System.Diagnostics;
using System.Dynamic;
using System.IO;
using Jint;
using Syn.Bot.Oscova;
using Syn.Bot.Oscova.Collection;
using Syn.Bot.Oscova.Entities;
using Syn.Bot.Oscova.Interfaces;

namespace TelegramNureBot.WPF.Helper
{
    public class RuDateRecognizer : IEntityRecognizer
    {
        public EntityCollection Parse(Request request)
        {
            string script;
            var entities = new EntityCollection();
            string dir = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            using (StreamReader sr=new StreamReader(dir+@"\Helper\RuDateParser.js"))
            {
                script = sr.ReadToEnd();
            }
            var dateParse=new Engine().Execute(script).GetValue("ParseDate");
            try
            {
                dynamic obj = dateParse.Invoke(request.Text).ToObject() as ExpandoObject;
                if (obj != null && obj?.title != null)
                {
                    string textdate = obj.title;
                    textdate = textdate.Replace("+ ", "").Trim();
                    var entity = new DateEntity(obj.title, DateTime.Parse(obj.date))
                    {
                        Index = request.Text.IndexOf(textdate, StringComparison.Ordinal)
                    };
                    entities.Add(entity);
                }
            }
            catch (Exception ex)
            {
                Debugger.Log(0,nameof(RuDateRecognizer),ex.Message);
            }


            return entities;
        }

        public string Type => Sys.Date;
    }
}