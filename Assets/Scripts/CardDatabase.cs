using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
[System.Serializable]

public enum CardID
{
    Null,

    DrMario,
    LittleMac,
    DonkeyKong,
    Steve,
    Link,
    Fox,
    CaptainFalcon,
    Peach,
    Roy,
    Kirby,
    Yoshi,
    Luigi,

    MewTwo,
    Pikachu,
    Ike,
    Ness,
    Bowser,
    Wolf,

    Falco,
    Jigglypuff,
    Cloud,
    Mario,

    Byleth,
    ROB,
    Samus,

    Joker,
    Pyra
};

public enum Rarity
{
    Null,
    Common,
    Uncommon,
    Rare,
    Epic,
    Legendary
};

public enum Passive
{
    Null,
    Straight,
    Area,
    Piercing,
    InstaKill,
    Regen,
    Fast,
    Double,
    Dodge
}

public class CardConfig
{
    public Rarity rarity;
    public string name;
    public int health;
    public int attack;
    public int manaCost;
    public Passive passive;
    public Texture2D texture;
    public CardConfig(Rarity rarity, string name, int health, int attack, int manaCost, Passive passive)
    {
        this.rarity = rarity;
        this.name = name;
        this.health = health;
        this.attack = attack;
        this.manaCost = manaCost;
        this.passive = passive;
    }
}


public class CardDatabase : MonoBehaviour
{

    public static int ProbabilityMax = 100;
    public static int CommonProbability = 50;
    public static int UncommonProbability = 25 + CommonProbability;
    public static int RareProbability = 13 + UncommonProbability;
    public static int EpicProbability = 8 + RareProbability;
    public static int LegendaryProbability = 4 + EpicProbability;
    public static List<Rarity> Probability = new List<Rarity>();

    public static CardID[] Common = {
        CardID.DrMario,
        CardID.LittleMac,
        CardID.DonkeyKong,
        CardID.Steve,
        CardID.Link,
        CardID.Fox,
        CardID.CaptainFalcon,
        CardID.Peach,
        CardID.Roy,
        CardID.Kirby,
        CardID.Yoshi,
        CardID.Luigi
    };

    public static CardID[] Uncommon = {
        CardID.MewTwo,
        CardID.Pikachu,
        CardID.Ike,
        CardID.Ness,
        CardID.Bowser,
        CardID.Wolf,
    };

    public static CardID[] Rare = {
        CardID.Falco,
        CardID.Jigglypuff,
        CardID.Cloud,
        CardID.Mario
    };

    public static CardID[] Epic = {
        CardID.Byleth,
        CardID.ROB,
        CardID.Samus,
    };

    public static CardID[] Legendary = {
        CardID.Joker,
        CardID.Pyra
    };

    public static readonly Dictionary<CardID, CardConfig> CardConfigs = new Dictionary<CardID, CardConfig> {
        {CardID.DrMario, new CardConfig(Rarity.Common, "Dr. Mario", 1, 2, 1, Passive.Straight)},
        {CardID.LittleMac, new CardConfig(Rarity.Common, "Little Mac", 1, 1, 2, Passive.Double)},
        {CardID.DonkeyKong, new CardConfig(Rarity.Common, "Donkey Kong", 2, 3, 2, Passive.Straight)},
        {CardID.Steve, new CardConfig(Rarity.Common, "Steve", 1, 1, 1, Passive.Straight)},
        {CardID.Link, new CardConfig(Rarity.Common, "Link", 1, 2, 1, Passive.Straight)},
        {CardID.Fox, new CardConfig(Rarity.Common, "Fox", 2, 2, 1, Passive.Fast)},
        {CardID.CaptainFalcon, new CardConfig(Rarity.Common, "Captain Falcon", 2, 2, 2, Passive.Straight)},
        {CardID.Peach, new CardConfig(Rarity.Common, "Peach", 1, 2, 1, Passive.Straight)},
        {CardID.Roy, new CardConfig(Rarity.Common, "Roy", 2, 1, 1, Passive.Double)},
        {CardID.Kirby, new CardConfig(Rarity.Common, "Kirby", 1, 2, 1, Passive.Straight)},
        {CardID.Yoshi, new CardConfig(Rarity.Common, "Yoshi", 3, 1, 2, Passive.Straight)},
        {CardID.Luigi, new CardConfig(Rarity.Common, "Luigi", 2, 2, 2, Passive.InstaKill)},
        {CardID.MewTwo, new CardConfig(Rarity.Uncommon, "MewTwo", 2, 3, 2, Passive.Double)},
        {CardID.Pikachu, new CardConfig(Rarity.Uncommon, "Pikachu", 1, 2, 1, Passive.Dodge)},
        {CardID.Ike, new CardConfig(Rarity.Uncommon, "Ike", 3, 2, 2, Passive.Area)},
        {CardID.Ness, new CardConfig(Rarity.Uncommon, "Ness", 3, 3, 3, Passive.Area)},
        {CardID.Bowser, new CardConfig(Rarity.Uncommon, "Bowser", 4, 2, 3, Passive.Regen)},
        {CardID.Wolf, new CardConfig(Rarity.Uncommon, "Wolf", 2, 4, 3, Passive.Double)},
        {CardID.Falco, new CardConfig(Rarity.Rare, "Falco", 3, 3, 3, Passive.Dodge)},
        {CardID.Jigglypuff, new CardConfig(Rarity.Rare, "Jigglypuff", 1, 5, 4, Passive.InstaKill)},
        {CardID.Cloud, new CardConfig(Rarity.Rare, "Cloud", 3, 4, 3, Passive.Fast)},
        {CardID.Mario, new CardConfig(Rarity.Rare, "Mario", 3, 3, 2, Passive.Straight)},
        {CardID.Byleth, new CardConfig(Rarity.Epic, "Byleth", 3, 6, 5, Passive.Area)},
        {CardID.ROB, new CardConfig(Rarity.Epic, "R.O.B.", 4, 4, 5, Passive.Straight)},
        {CardID.Samus, new CardConfig(Rarity.Epic, "Samus", 3, 5, 5, Passive.Piercing)},
        {CardID.Joker, new CardConfig(Rarity.Legendary, "Joker", 5, 5, 6, Passive.Regen)},
        {CardID.Pyra, new CardConfig(Rarity.Legendary, "Pyra", 5, 7, 7, Passive.InstaKill)},
        {CardID.Null, new CardConfig(Rarity.Null, "Null", 0, 0, 0, Passive.Straight)}
    };

    public static readonly Dictionary<Passive, string> PassiveDescriptions = new Dictionary<Passive, string> {
        {Passive.Null, "Null"},
        {Passive.Straight, "Attacks directly ahead"},
        {Passive.Area, "Attacks in a wide area"},
        {Passive.Piercing, "Attacks pierce through enemies"},
        {Passive.InstaKill, "Chance to instantly kill"},
        {Passive.Regen, "Regenerates 1 health per turn"},
        {Passive.Fast, "Always attacks first"},
        {Passive.Dodge, "Chance to dodge enemy attack"},
        {Passive.Double, "Chance to deal double damage"}
    };

    public static void init()
    {
        loadCardImages();
        loadProbabilities();
        Debug.Log("Common Probability: " + CommonProbability.ToString());
        Debug.Log("Uncommon Probability: " + UncommonProbability.ToString());
        Debug.Log("Rare Probability: " + RareProbability.ToString());
        Debug.Log("Epic Probability: " + EpicProbability.ToString());
        Debug.Log("Legendary Probability: " + LegendaryProbability.ToString());
    }

    private static void loadProbabilities()
    {
        for (int numTickets = 0; numTickets < ProbabilityMax; numTickets++)
        {
            switch (numTickets)
            {
                case int n when (numTickets < CommonProbability):
                    Probability.Add(Rarity.Common);
                    break;
                case int n when (numTickets < UncommonProbability):
                    Probability.Add(Rarity.Uncommon);
                    break;
                case int n when (numTickets < RareProbability):
                    Probability.Add(Rarity.Rare);
                    break;
                case int n when (numTickets < EpicProbability):
                    Probability.Add(Rarity.Epic);
                    break;
                case int n when (numTickets < LegendaryProbability):
                    Probability.Add(Rarity.Legendary);
                    break;
                default:
                    Probability.Add(Rarity.Null);
                    break;
            }
        }
    }

    private static void loadCardImages()
    {
        Texture2D textureDrmario = new Texture2D(1000, 800, TextureFormat.RGB24, false); //"drmario.png"
        byte[] bytesDrmario = File.ReadAllBytes("Assets/Resources/CardImages/drmario.png");
        textureDrmario.LoadImage(bytesDrmario);
        CardConfigs[CardID.DrMario].texture = textureDrmario;

        Texture2D textureLittlemac = new Texture2D(1000, 800, TextureFormat.RGB24, false); //"littlemac.png"
        byte[] bytesLittlemac = File.ReadAllBytes("Assets/Resources/CardImages/littlemac.png");
        textureLittlemac.LoadImage(bytesLittlemac);
        CardConfigs[CardID.LittleMac].texture = textureLittlemac;

        Texture2D textureDonkeykong = new Texture2D(1000, 800, TextureFormat.RGB24, false); //"donkeykong.png"
        byte[] bytesDonkeykong = File.ReadAllBytes("Assets/Resources/CardImages/donkeykong.png");
        textureDonkeykong.LoadImage(bytesDonkeykong);
        CardConfigs[CardID.DonkeyKong].texture = textureDonkeykong;

        Texture2D textureSteve = new Texture2D(1000, 800, TextureFormat.RGB24, false); //"steve.png"
        byte[] bytesSteve = File.ReadAllBytes("Assets/Resources/CardImages/steve.png");
        textureSteve.LoadImage(bytesSteve);
        CardConfigs[CardID.Steve].texture = textureSteve;

        Texture2D textureLink = new Texture2D(1000, 800, TextureFormat.RGB24, false); //"link.png"
        byte[] bytesLink = File.ReadAllBytes("Assets/Resources/CardImages/link.png");
        textureLink.LoadImage(bytesLink);
        CardConfigs[CardID.Link].texture = textureLink;

        Texture2D textureFox = new Texture2D(1000, 800, TextureFormat.RGB24, false); //"fox.png"
        byte[] bytesFox = File.ReadAllBytes("Assets/Resources/CardImages/fox.png");
        textureFox.LoadImage(bytesFox);
        CardConfigs[CardID.Fox].texture = textureFox;

        Texture2D textureCaptainfalcon = new Texture2D(1000, 800, TextureFormat.RGB24, false); //"captainfalcon.png"
        byte[] bytesCaptainfalcon = File.ReadAllBytes("Assets/Resources/CardImages/captainfalcon.png");
        textureCaptainfalcon.LoadImage(bytesCaptainfalcon);
        CardConfigs[CardID.CaptainFalcon].texture = textureCaptainfalcon;

        Texture2D texturePeach = new Texture2D(1000, 800, TextureFormat.RGB24, false); //"peach.png"
        byte[] bytesPeach = File.ReadAllBytes("Assets/Resources/CardImages/peach.png");
        texturePeach.LoadImage(bytesPeach);
        CardConfigs[CardID.Peach].texture = texturePeach;

        Texture2D textureRoy = new Texture2D(1000, 800, TextureFormat.RGB24, false); //"roy.png"
        byte[] bytesRoy = File.ReadAllBytes("Assets/Resources/CardImages/roy.png");
        textureRoy.LoadImage(bytesRoy);
        CardConfigs[CardID.Roy].texture = textureRoy;

        Texture2D textureKirby = new Texture2D(1000, 800, TextureFormat.RGB24, false); //"kirby.png"
        byte[] bytesKirby = File.ReadAllBytes("Assets/Resources/CardImages/kirby.png");
        textureKirby.LoadImage(bytesKirby);
        CardConfigs[CardID.Kirby].texture = textureKirby;

        Texture2D textureYoshi = new Texture2D(1000, 800, TextureFormat.RGB24, false); //"yoshi.png"
        byte[] bytesYoshi = File.ReadAllBytes("Assets/Resources/CardImages/yoshi.png");
        textureYoshi.LoadImage(bytesYoshi);
        CardConfigs[CardID.Yoshi].texture = textureYoshi;

        Texture2D textureLuigi = new Texture2D(1000, 800, TextureFormat.RGB24, false); //"luigi.png"
        byte[] bytesLuigi = File.ReadAllBytes("Assets/Resources/CardImages/luigi.png");
        textureLuigi.LoadImage(bytesLuigi);
        CardConfigs[CardID.Luigi].texture = textureLuigi;

        Texture2D textureMewtwo = new Texture2D(1000, 800, TextureFormat.RGB24, false); //"mewtwo.png"
        byte[] bytesMewtwo = File.ReadAllBytes("Assets/Resources/CardImages/mewtwo.png");
        textureMewtwo.LoadImage(bytesMewtwo);
        CardConfigs[CardID.MewTwo].texture = textureMewtwo;

        Texture2D texturePikachu = new Texture2D(1000, 800, TextureFormat.RGB24, false); //"pikachu.png"
        byte[] bytesPikachu = File.ReadAllBytes("Assets/Resources/CardImages/pikachu.png");
        texturePikachu.LoadImage(bytesPikachu);
        CardConfigs[CardID.Pikachu].texture = texturePikachu;

        Texture2D textureIke = new Texture2D(1000, 800, TextureFormat.RGB24, false); //"ike.png"
        byte[] bytesIke = File.ReadAllBytes("Assets/Resources/CardImages/ike.png");
        textureIke.LoadImage(bytesIke);
        CardConfigs[CardID.Ike].texture = textureIke;

        Texture2D textureNess = new Texture2D(1000, 800, TextureFormat.RGB24, false); //"ness.png"
        byte[] bytesNess = File.ReadAllBytes("Assets/Resources/CardImages/ness.png");
        textureNess.LoadImage(bytesNess);
        CardConfigs[CardID.Ness].texture = textureNess;

        Texture2D textureBowser = new Texture2D(1000, 800, TextureFormat.RGB24, false); //"bowser.png"
        byte[] bytesBowser = File.ReadAllBytes("Assets/Resources/CardImages/bowser.png");
        textureBowser.LoadImage(bytesBowser);
        CardConfigs[CardID.Bowser].texture = textureBowser;

        Texture2D textureWolf = new Texture2D(1000, 800, TextureFormat.RGB24, false); //"wolf.png"
        byte[] bytesWolf = File.ReadAllBytes("Assets/Resources/CardImages/wolf.png");
        textureWolf.LoadImage(bytesWolf);
        CardConfigs[CardID.Wolf].texture = textureWolf;

        Texture2D textureFalco = new Texture2D(1000, 800, TextureFormat.RGB24, false); //"falco.png"
        byte[] bytesFalco = File.ReadAllBytes("Assets/Resources/CardImages/falco.png");
        textureFalco.LoadImage(bytesFalco);
        CardConfigs[CardID.Falco].texture = textureFalco;

        Texture2D textureJigglepuff = new Texture2D(1000, 800, TextureFormat.RGB24, false); //"jigglypuff.png"
        byte[] bytesJigglepuff = File.ReadAllBytes("Assets/Resources/CardImages/jigglypuff.png");
        textureJigglepuff.LoadImage(bytesJigglepuff);
        CardConfigs[CardID.Jigglypuff].texture = textureJigglepuff;

        Texture2D textureCloud = new Texture2D(1000, 800, TextureFormat.RGB24, false); //"cloud.png"
        byte[] bytesCloud = File.ReadAllBytes("Assets/Resources/CardImages/cloud.png");
        textureCloud.LoadImage(bytesCloud);
        CardConfigs[CardID.Cloud].texture = textureCloud;

        Texture2D textureMario = new Texture2D(1000, 800, TextureFormat.RGB24, false); //"mario.png"
        byte[] bytesMario = File.ReadAllBytes("Assets/Resources/CardImages/mario.png");
        textureMario.LoadImage(bytesMario);
        CardConfigs[CardID.Mario].texture = textureMario;

        Texture2D textureByleth = new Texture2D(1000, 800, TextureFormat.RGB24, false); //"byleth.png"
        byte[] bytesByleth = File.ReadAllBytes("Assets/Resources/CardImages/byleth.png");
        textureByleth.LoadImage(bytesByleth);
        CardConfigs[CardID.Byleth].texture = textureByleth;

        Texture2D textureRob = new Texture2D(1000, 800, TextureFormat.RGB24, false); //"rob.png"
        byte[] bytesRob = File.ReadAllBytes("Assets/Resources/CardImages/rob.png");
        textureRob.LoadImage(bytesRob);
        CardConfigs[CardID.ROB].texture = textureRob;

        Texture2D textureSamus = new Texture2D(1000, 800, TextureFormat.RGB24, false); //"samus.png"
        byte[] bytesSamus = File.ReadAllBytes("Assets/Resources/CardImages/samus.png");
        textureSamus.LoadImage(bytesSamus);
        CardConfigs[CardID.Samus].texture = textureSamus;

        Texture2D textureJoker = new Texture2D(1000, 800, TextureFormat.RGB24, false); //"joker.png"
        byte[] bytesJoker = File.ReadAllBytes("Assets/Resources/CardImages/joker.png");
        textureJoker.LoadImage(bytesJoker);
        CardConfigs[CardID.Joker].texture = textureJoker;

        Texture2D texturePyra = new Texture2D(1000, 800, TextureFormat.RGB24, false); //"pyra.png"
        byte[] bytesPyra = File.ReadAllBytes("Assets/Resources/CardImages/pyra.png");
        texturePyra.LoadImage(bytesPyra);
        CardConfigs[CardID.Pyra].texture = texturePyra;

    }

    public static Card generateCard(CardID cardId, GameObject CardPrefab)
    {
        GameObject cardGameObject = Instantiate(CardPrefab, Vector3.one, Quaternion.identity);
        Card card = cardGameObject.GetComponent<Card>() as Card;
        card.init(cardGameObject, cardId, CardConfigs[cardId]);
        return card;
    }

    public static Card generateRandomCard(GameObject CardPrefab)
    {
        Rarity selectedRarity = Probability[Random.Range(0, ProbabilityMax)];
        CardID cardIdSelected = CardID.Null;

        switch (selectedRarity)
        {
            case Rarity.Common:
                cardIdSelected = ((CardID)Common[Random.Range(0, Common.Length)]);
                break;
            case Rarity.Uncommon:
                cardIdSelected = ((CardID)Uncommon[Random.Range(0, Uncommon.Length)]);
                break;
            case Rarity.Rare:
                cardIdSelected = ((CardID)Rare[Random.Range(0, Rare.Length)]);
                break;
            case Rarity.Epic:
                cardIdSelected = ((CardID)Epic[Random.Range(0, Epic.Length)]);
                break;
            case Rarity.Legendary:
                cardIdSelected = ((CardID)Legendary[Random.Range(0, Legendary.Length)]);
                break;
        }
        return generateCard(cardIdSelected, CardPrefab);
    }
}