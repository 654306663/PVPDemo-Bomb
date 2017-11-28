namespace MyGameServer.Model
{
    public class PlayerData
    {
        public int id;
        public string username;
        public string nickname;
        public HeroData heroData = new HeroData();
    }
}
