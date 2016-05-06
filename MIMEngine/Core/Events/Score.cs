namespace MIMEngine.Core.Events
{
    using PlayerSetup;

    class Score
    {

        public static void ReturnScore(PlayerSetup playerData)
        {
            string scoreTest = "Score:\r\n Name: " + playerData.name + " Race: " + playerData.race;
 

           HubProxy.MimHubServer.Invoke("SendToClient", scoreTest);

        }
    }
}
