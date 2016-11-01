using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MIMWebClient.Core.PlayerSetup
{
    public class PlayerName
    {
        private static List<string> _humanNameList { get; set; }
 
        public static List<string> GenerateHumanNames()
        {
            if (_humanNameList != null) return _humanNameList;

            List<string> stringList = new List<string>();

            stringList.InsertRange(stringList.Count, new string[]
            {
               "Esau","Laurentius","Windham","Aurel","Quincy","Ursus","Muck","Beal","Triston","Baldwin","Clive","Karlens","Valdis","Weston","Bradney","Goliat","Denver","Urbano","Scipione","Kegan","Trent","Moor","Keenan","Holm","Ryder","Stokley","Rylee","Aron","Bellamy","Nino","Claus","Mallory","Jeremiah","Barnabas","Aleron","Brentan","Marti","Wynton","Muhammad","Battista","Gerrit","Emmery","Wayne","Arjun","Hugh","Dalton","Rodolfo","Durand","Estevan","Dominikus","Garvin","Valdemar","Dabbert","Carsten","Aubert","Welch","Walton","Maurus","Alvord","Russel","Gower","Lafayette","Dirk","Paul","Palmer","Dickens","Falko","Wirt","Barklay","Lionello","Goddard","Raimundo","Ramon","Rudolfo","Hall","Brook","Leal","Benjamin","Maximus","Derrek","Reuben","Sebastian","Audric","Cristobal","Jendrick","Anselm","Ignatz","Horton","Beardsley","Flemming","Adger","Kinnell","Harv","Jason","Spence","Wendell","Mandel","Immanuel","Tjorven","Carleton","Bernt","Matze","Valdimar","Alain","Esteban","Carol","Cynric","Aurélien","Marquise","Warner","Berkley","Bonifatius","Redwald","Gehrt","Noreis","Rudi","Görkem","Olin","Marinus","Garnell","Pierce","Gratianus","Sinjin","Elijah","Ramzey","Sandon","Gauthier","Deshawn","Nastasia","Lee","Sofie","Yazmin","Berangaria","Lynette","Nora","Pearl","Viktoria","Janina","Melanee","Jewel","Fernanda","Berangaria","Ronja","Jaylin","Roterica","Ryleigh","Yamina","Brooklyn","Arnalda","Maryl","Joslin","Hanne","Jayden","Liana","Klarinda","Manuela","Jackie","Alex","Blondene","Petra","Karlotta","Caitlyn","Ysabel","Belana","Nicolle","Dagmara","Justine","Natassja","Jaylyn","Salma","Dericka","Jordane","Hanne","Pierretta","Katherine","Grit","Carling","Livia","Kianna","Frauke","Narcisse","Bernadine","Jocelyn","Nafia","Livia","Hana","arcell","Philippina","Belinda","Orva","Dorothy","Viktoria","Gloriana","Kelsie","Lillian","Wally","Michelle","Gratia","Vallerie","Elisabetta","Jella","Alisha","Anka","Sage","Dorothee","Dawina","Lydie","Nichole","Rilla","Stacey","Auberta","Blanche","Megan","Kari","Palmira","Bria","Mafalda","Andie","Makenna","Cherise","Amaya","Fontanne","Leopolda","Philina","Felina","Elishia","Sina","Valeria","Holli","Edlyn","Fae","Lara","Hunter","Dalia","Melissa","Makenna","Aubry","Billie","Jacquenette","Guadalupe","Balda","Piersym","Ernam","Cola","Eodbean","Ricio","Coenber","Gyles","Geoffry","Balde","Bethon","Ealhbun","Ethed","Eryen","Ellyn","Ellet","Aethild","Cily","Ealketh","Withiua"
            });

            _humanNameList = stringList;


            return _humanNameList;
        }


        public static string GetHumanName()
        {

            var name = GenerateHumanNames();

            var randomIndex = Helpers.Rand(0, name.Count);

            return name[randomIndex];
        }
    }
}