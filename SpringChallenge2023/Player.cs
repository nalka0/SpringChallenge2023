namespace SpringChallenge2023;

public abstract class Player
{
    public static List<FriendlyPlayer> Me { get; set; }

    public static List<EnnemyPlayer> Ennemy { get; set; }

    public Cell Base { get; set; }

    public int TotalAnts { get; set; }
}