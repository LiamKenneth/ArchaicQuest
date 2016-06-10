namespace MIMEngine.Core.Events
{
    using System.Text;

    using PlayerSetup;
    using Microsoft.AspNet.SignalR;
    using MIMHubServer;
    class Score
    {

        public static void ReturnScore(Player playerData)
        {
            string scoreTest = "Score:\r\n Name: " + playerData.Name + " Race: " + playerData.Race;

            var context = HubContext.getHubContext;
            context.Clients.All.addNewMessageToPage(scoreTest);
        }
    }
}
