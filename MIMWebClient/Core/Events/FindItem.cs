using MIMWebClient.Core.Room;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MIMWebClient.Core.Events
{
    public class FindItem
    {
        public static Item.Item Item (List<Item.Item> collection, int findNth, string itemToFind)
        {

            if (collection == null)
            {
                return null;
            }

            if (findNth == -1)
            {
                return collection.Find(x => x.name.ToLower().Contains(itemToFind));
            }


            return collection.FindAll(x => x.name.ToLower().Contains(itemToFind)).Skip(findNth - 1).FirstOrDefault();

        }

        public static PlayerSetup.Player Player (List<PlayerSetup.Player> collection, int findNth, string itemToFind)
        {

            if (collection == null)
            {
                return null;
            }

            if (findNth == -1)
            {
                return collection.Find(x => x.Name.ToLower().Contains(itemToFind));
            }


            return collection.FindAll(x => x.Name.ToLower().Contains(itemToFind)).Skip(findNth - 1).FirstOrDefault();

        }

        //public static PlayerSetup.Player Trainer(List<PlayerSetup.Player> collection, int findNth)
        //{

        //    //TODO: findTrainer - add trainer attribute to player class

        //    //if (collection == null)
        //    //{
        //    //    return null;
        //    //}

        //    //if (findNth == -1)
        //    //{
        //    //    return collection.Find(x => x.Name.ToLower().Contains(itemToFind));
        //    //}


        //    //return collection.FindAll(x => x.Name.ToLower().Contains(itemToFind)).Skip(findNth - 1).FirstOrDefault();

        //}

        public static Exit Exit (List<Exit> collection, int findNth, string itemToFind)
        {

            if (collection == null)
            {
                return null;
            }

            if (findNth == -1)
            {
                return collection.Find(x => x.name.ToLower().Contains(itemToFind));
            }


            return collection.FindAll(x => x.name.ToLower().Contains(itemToFind)).Skip(findNth - 1).FirstOrDefault();

        }
    }
}