namespace MIMWebClient.Core.Events
{
    using System;
    using System.Collections.Generic;

    public class FindFirstAndLast
    {
        public  static KeyValuePair<string, string> FindFirstAndLastIndex(string thingToFind)
        {
            string item = thingToFind;
            string itemContainer = null;

            int indexOfSpaceInUserInput = thingToFind.IndexOf(" ", StringComparison.Ordinal);
            int lastIndexOfSpaceInUserInput = thingToFind.LastIndexOf(" ", StringComparison.Ordinal);

            if (indexOfSpaceInUserInput > 0 && lastIndexOfSpaceInUserInput == -1)
            {
                item = thingToFind.Substring(0, indexOfSpaceInUserInput);
            }

            if (lastIndexOfSpaceInUserInput != -1)
            {
                item = thingToFind.Substring(0, indexOfSpaceInUserInput);
                itemContainer = thingToFind.Substring(lastIndexOfSpaceInUserInput).TrimStart();
            }
            return new KeyValuePair<string, string>(item, itemContainer);
        }
    }
}