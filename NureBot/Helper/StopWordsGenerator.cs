﻿using System.Collections.Generic;

namespace TelegramNureBot.Helper
{
    public static class StopWordsGenerator
    {
        public static HashSet<string> GenerateRussianStopWords()
        {
            HashSet<string> result=new HashSet<string>();
            string words =
                "а\r\nе\r\nи\r\nж\r\nм\r\nо\r\nна\r\nне\r\nни\r\nоб\r\nно\r\nон\r\nмне\r\nмои\r\nмож\r\nона\r\nони\r\nоно\r\nмной\r\nмного\r\nмногочисленное\r\nмногочисленная\r\nмногочисленные\r\nмногочисленный\r\nмною\r\nмой\r\nмог\r\nмогут\r\nможно\r\nможет\r\nможхо\r\nмор\r\nмоя\r\nмоё\r\nмочь\r\nнад\r\nнее\r\nоба\r\nнам\r\nнем\r\nнами\r\nними\r\nмимо\r\nнемного\r\nодной\r\nодного\r\nменее\r\nоднажды\r\nоднако\r\nменя\r\nнему\r\nменьше\r\nней\r\nнаверху\r\nнего\r\nниже\r\nмало\r\nнадо\r\nодин\r\nодиннадцать\r\nодиннадцатый\r\nназад\r\nнаиболее\r\nнедавно\r\nмиллионов\r\nнедалеко\r\nмежду\r\nнизко\r\nмеля\r\nнельзя\r\nнибудь\r\nнепрерывно\r\nнаконец\r\nникогда\r\nникуда\r\nнас\r\nнаш\r\nнет\r\nнею\r\nнеё\r\nних\r\nмира\r\nнаша\r\nнаше\r\nнаши\r\nничего\r\nначала\r\nнередко\r\nнесколько\r\nобычно\r\nопять\r\nоколо\r\nмы\r\nну\r\nнх\r\nот\r\nотовсюду\r\nособенно\r\nнужно\r\nочень\r\nотсюда\r\nв\r\nво\r\nвон\r\nвниз\r\nвнизу\r\nвокруг\r\nвот\r\nвосемнадцать\r\nвосемнадцатый\r\nвосемь\r\nвосьмой\r\nвверх\r\nвам\r\nвами\r\nважное\r\nважная\r\nважные\r\nважный\r\nвдали\r\nвезде\r\nведь\r\nвас\r\nваш\r\nваша\r\nваше\r\nваши\r\nвпрочем\r\nвесь\r\nвдруг\r\nвы\r\nвсе\r\nвторой\r\nвсем\r\nвсеми\r\nвремени\r\nвремя\r\nвсему\r\nвсего\r\nвсегда\r\nвсех\r\nвсею\r\nвсю\r\nвся\r\nвсё\r\nвсюду\r\nг\tгод\r\nговорил\r\nговорит\r\nгода\r\nгоду\r\nгде\r\nда\r\nее\r\nза\r\nиз\r\nли\r\nже\r\nим\r\nдо\r\nпо\r\nими\r\nпод\r\nиногда\r\nдовольно\r\nименно\r\nдолго\r\nпозже\r\nболее\r\nдолжно\r\nпожалуйста\r\nзначит\r\nиметь\r\nбольше\r\nпока\r\nему\r\nимя\r\nпор\r\nпора\r\nпотом\r\nпотому\r\nпосле\r\nпочему\r\nпочти\r\nпосреди\r\nей\r\nдва\r\nдве\r\nдвенадцать\r\nдвенадцатый\r\nдвадцать\r\nдвадцатый\r\nдвух\r\nего\r\nдел\r\nили\r\nбез\r\nдень\r\nзанят\r\nзанята\r\nзанято\r\nзаняты\r\nдействительно\r\nдавно\r\nдевятнадцать\r\nдевятнадцатый\r\nдевять\r\nдевятый\r\nдаже\r\nалло\r\nжизнь\r\nдалеко\r\nблизко\r\nздесь\r\nдальше\r\nдля\r\nлет\r\nзато\r\nдаром\r\nпервый\r\nперед\r\nзатем\r\nзачем\r\nлишь\r\nдесять\r\nдесятый\r\nею\r\nеё\r\nих\r\nбы\r\nеще\r\nпри\r\nбыл\r\nпро\r\nпроцентов\r\nпротив\r\nпросто\r\nбывает\r\nбывь\r\nесли\r\nлюди\r\nбыла\r\nбыли\r\nбыло\r\nбудем\r\nбудет\r\nбудете\r\nбудешь\r\nпрекрасно\r\nбуду\r\nбудь\r\nбудто\r\nбудут\r\nещё\r\nпятнадцать\r\nпятнадцатый\r\nдруго\r\nдругое\r\nдругой\r\nдругие\r\nдругая\r\nдругих\r\nесть\r\nпять\r\nбыть\r\nлучше\r\nпятый\r\nк\r\nком\r\nконечно\r\nкому\r\nкого\r\nкогда\r\nкоторой\r\nкоторого\r\nкоторая\r\nкоторые\r\nкоторый\r\nкоторых\r\nкем\r\nкаждое\r\nкаждая\r\nкаждые\r\nкаждый\r\nкажется\r\nкак\r\nкакой\r\nкакая\r\nкто\r\nкроме\r\nкуда\r\nкругом\r\nс\tт\r\nу\r\nя\r\nта\r\nте\r\nуж\r\nсо\r\nто\r\nтом\r\nснова\r\nтому\r\nсовсем\r\nтого\r\nтогда\r\nтоже\r\nсобой\r\nтобой\r\nсобою\r\nтобою\r\nсначала\r\nтолько\r\nуметь\r\nтот\r\nтою\r\nхорошо\r\nхотеть\r\nхочешь\r\nхоть\r\nхотя\r\nсвое\r\nсвои\r\nтвой\r\nсвоей\r\nсвоего\r\nсвоих\r\nсвою\r\nтвоя\r\nтвоё\r\nраз\r\nуже\r\nсам\r\nтам\r\nтем\r\nчем\r\nсама\r\nсами\r\nтеми\r\nсамо\r\nрано\r\nсамом\r\nсамому\r\nсамой\r\nсамого\r\nсемнадцать\r\nсемнадцатый\r\nсамим\r\nсамими\r\nсамих\r\nсаму\r\nсемь\r\nчему\r\nраньше\r\nсейчас\r\nчего\r\nсегодня\r\nсебе\r\nтебе\r\nсеаой\r\nчеловек\r\nразве\r\nтеперь\r\nсебя\r\nтебя\r\nседьмой\r\nспасибо\r\nслишком\r\nтак\r\nтакое\r\nтакой\r\nтакие\r\nтакже\r\nтакая\r\nсих\r\nтех\r\nчаще\r\nчетвертый\r\nчерез\r\nчасто\r\nшестой\r\nшестнадцать\r\nшестнадцатый\r\nшесть\r\nчетыре\r\nчетырнадцать\r\nчетырнадцатый\r\nсколько\r\nсказал\r\nсказала\r\nсказать\r\nту\r\nты\r\nтри\r\nэта\r\nэти\r\nчто\r\nэто\r\nчтоб\r\nэтом\r\nэтому\r\nэтой\r\nэтого\r\nчтобы\r\nэтот\r\nстал\r\nтуда\r\nэтим\r\nэтими\r\nрядом\r\nтринадцать\r\nтринадцатый\r\nэтих\r\nтретий\r\nтут\r\nэту\r\nсуть\r\nчуть\r\nтысяч";

            words=words.Replace("\r", "");
            words=words.Replace("\n",":");

            foreach (var word in words.Split(':'))
            {
                result.Add(word);
            }

            return result;
        }
    }
}