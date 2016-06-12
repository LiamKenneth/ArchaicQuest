namespace MIMWebClient.Core.Events
{
    using System.Text;
    using System.Web.Helpers;

    using MongoDB.Bson;

    using PlayerSetup;

    class Score
    {

        public static void ReturnScore(Player playerData)
        {
            string scoreTest = "Score:\r\n Name: " + playerData.Name + " Race: " + playerData.Race;

            var context = HubContext.getHubContext;
            context.Clients.All.addNewMessageToPage(scoreTest);
        }

        public static string ReturnScoreUI(Player playerData)
        {
            return playerData.ToJson();
        }
    }
}
