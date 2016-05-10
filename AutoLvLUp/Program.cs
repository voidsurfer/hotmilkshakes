using System.Threading;
using EloBuddy.SDK.Menu.Values;

namespace AutoSpellUp
{
    using System;
    using System.Linq;

    using EloBuddy;
    using EloBuddy.SDK.Events;
    using EloBuddy.SDK.Menu;

    internal class Program
    {

        public static int[] AbilitySequence;

        public static int QOff = 0, WOff = 0, EOff = 0, ROff;

        public static string Tipo = "";

        public static SpellSlot Smite;

        public static SpellSlot Heal;

        public static Menu Menu;

        public static int[] Level = {0, 0, 0, 0};

        public static AIHeroClient Player
        {
            get { return EloBuddy.Player.Instance; }
        }

        public static int Delay
        {
            get { return Menu["delay"].Cast<Slider>().CurrentValue; }
        }

        private static void Main()
        {
            Loading.OnLoadingComplete += Game_OnStart;
        }

        private static void Game_OnStart(EventArgs args)
        {
            Menu = MainMenu.AddMenu("AutoLevelUp", "AutoLevelUp");

            Menu.AddGroupLabel("Dakota's and KarmaPanda's Auto Level Up");
            Menu.AddLabel(Player.ChampionName + " loaded. Credits to Dakota and KarmaPanda.");
            Menu.Add("delay", new Slider("Maximum Randomize Delay Value", 1000, 0, 10000));

            var heal = EloBuddy.Player.Spells.FirstOrDefault(o => o.SData.Name == "summonerHeal");
            var smite = EloBuddy.Player.Spells.FirstOrDefault(o => o.SData.Name == "summonerSmite");

            #region Champion Ability Sequences

            switch (Player.ChampionName)
            {
                case "Aatrox":
                    AbilitySequence = new[] { 2, 1, 3, 2, 2, 4, 2, 3, 2, 3, 4, 3, 3, 1, 1, 4, 1, 1 };
                    break;
                case "Ahri":
                    AbilitySequence = new[] { 1, 3, 2, 1, 1, 4, 1, 2, 1, 2, 4, 2, 2, 3, 3, 4, 3, 3 };
                    break;
                case "Akali":
                    AbilitySequence = new[] { 1, 2, 3, 1, 1, 4, 1, 3, 1, 3, 4, 3, 3, 2, 2, 4, 2, 2 };
                    break;
                case "Alistar":
                    AbilitySequence = new[] { 3, 1, 2, 3, 3, 4, 3, 1, 3, 1, 4, 1, 1, 2, 2, 4, 2, 2 };
                    break;
                case "Amumu":
                    AbilitySequence = new[] { 3, 1, 2, 3, 2, 4, 3, 3, 3, 2, 4, 2, 2, 1, 1, 4, 1, 1 };
                    break;
                case "Anivia":
                    AbilitySequence = new[] { 1, 3, 2, 3, 3, 4, 3, 1, 3, 1, 4, 1, 1, 2, 2, 4, 2, 2 };
                    break;
                case "Annie":
                    AbilitySequence = new[] { 1, 2, 3, 1, 1, 4, 1, 2, 1, 2, 4, 2, 2, 3, 3, 4, 3, 3 };
                    break;
                case "Ashe":
                    AbilitySequence = new[] { 2, 1, 3, 1, 1, 4, 1, 2, 1, 2, 4, 2, 2, 3, 3, 4, 3, 3 };
                    break;
                case "AurelionSol":
                    AbilitySequence = new[] { 1, 2, 1, 3, 1, 4, 1, 2, 1, 2, 4, 2, 2, 3, 3, 4, 3, 3 };
                    break;
                case "Azir":
                    AbilitySequence = new[] { 2, 1, 3, 2, 2, 4, 1, 2, 2, 1, 4, 1, 3, 1, 3, 4, 3, 3 };
                    break;
                case "Bard":
                    AbilitySequence = new[] { 1, 2, 1, 2, 1, 4, 1, 2, 1, 2, 4, 2, 3, 3, 3, 4, 3, 3 };
                    break;
                case "Blitzcrank":
                    AbilitySequence = new[] { 1, 3, 2, 1, 1, 4, 1, 3, 1, 3, 4, 3, 3, 2, 2, 4, 2, 2 };
                    break;
                case "Brand":
                    AbilitySequence = new[] { 2, 1, 3, 2, 2, 4, 2, 3, 2, 3, 4, 3, 3, 1, 1, 4, 1, 1 };
                    break;
                case "Braum":
                    AbilitySequence = new[] { 1, 3, 2, 1, 1, 4, 1, 3, 1, 3, 4, 3, 3, 2, 2, 4, 2, 2 };
                    break;
                case "Caitlyn":
                    AbilitySequence = new[] { 1, 2, 3, 1, 1, 4, 1, 3, 1, 3, 4, 3, 3, 2, 2, 4, 2, 2 };
                    break;
                case "Cassiopeia":
                    AbilitySequence = new[] { 1, 3, 3, 2, 3, 4, 3, 1, 3, 1, 4, 1, 1, 2, 2, 4, 2, 2 };
                    break;
                case "Chogath":
                    AbilitySequence = new[] { 1, 3, 2, 1, 1, 4, 1, 3, 1, 3, 4, 3, 3, 2, 2, 4, 2, 2 };
                    break;
                case "Corki":
                    if (Player.PercentMagicDamageMod > Player.PercentPhysicalDamageMod)
                    {
                        AbilitySequence = new[] { 1, 2, 3, 1, 1, 4, 1, 2, 1, 2, 4, 2, 2, 3, 3, 4, 3, 3 };
                        Tipo = " AP";
                    }
                    else
                    {
                        AbilitySequence = new[] { 1, 2, 3, 1, 1, 4, 1, 3, 1, 3, 4, 3, 3, 2, 2, 4, 2, 2 };
                        Tipo = " AD";
                    }
                    break;
                case "Darius":
                    AbilitySequence = new[] { 1, 3, 2, 1, 1, 4, 1, 2, 1, 2, 4, 2, 2, 3, 3, 4, 3, 3 };
                    break;
                case "Diana":
                    AbilitySequence = new[] { 1, 2, 3, 1, 1, 4, 1, 2, 1, 2, 4, 2, 2, 3, 3, 4, 3, 3 };
                    break;
                case "DrMundo":
                    AbilitySequence = new[] { 1, 2, 3, 1, 1, 4, 1, 2, 1, 2, 4, 2, 2, 3, 3, 4, 3, 3 };
                    break;
                case "Draven":
                    AbilitySequence = new[] { 1, 2, 3, 1, 1, 4, 1, 2, 1, 2, 4, 2, 2, 3, 3, 4, 3, 3 };
                    break;
                case "Elise":
                    ROff = -1;
                    if (smite != null && smite.Slot != SpellSlot.Unknown)
                    {
                        AbilitySequence = new[] { 2, 1, 3, 1, 1, 4, 1, 2, 1, 2, 4, 2, 2, 3, 3, 4, 3, 3 };
                        Tipo = " Jungler";
                    }
                    else
                    {
                        AbilitySequence = new[] { 1, 2, 3, 1, 1, 4, 1, 2, 1, 2, 4, 2, 2, 3, 3, 4, 3, 3 };
                        Tipo = " Lane";
                    }
                    break;
                case "Evelynn":
                    AbilitySequence = new[] { 1, 3, 1, 2, 1, 4, 1, 3, 1, 3, 4, 3, 3, 2, 2, 4, 2, 2 };
                    break;
                case "Ezreal":
                    if (Player.TotalMagicalDamage > 0)
                    {
                        AbilitySequence = new[] { 1, 2, 3, 2, 2, 4, 2, 1, 2, 1, 4, 1, 1, 3, 3, 4, 3, 3 };
                        Tipo = " AP";
                    }
                    else
                    {
                        AbilitySequence = new[] { 1, 3, 1, 2, 1, 4, 1, 3, 1, 3, 4, 3, 3, 2, 2, 4, 2, 2 };
                        Tipo = " AD";
                    }
                    break;
                case "Ekko":
                    AbilitySequence = new[] { 1, 3, 2, 1, 1, 4, 1, 3, 1, 3, 4, 3, 3, 2, 2, 4, 2, 2 };
                    break;
                case "FiddleSticks":
                    AbilitySequence = new[] { 2, 3, 1, 2, 2, 4, 2, 1, 2, 1, 4, 1, 1, 3, 3, 4, 3, 3 };
                    break;
                case "Fiora":
                    if (smite != null && smite.Slot != SpellSlot.Unknown)
                    {
                        AbilitySequence = new[] { 3, 1, 2, 1, 1, 4, 1, 3, 1, 3, 4, 3, 3, 2, 2, 4, 2, 2 };
                        Tipo = " Jungle";
                    }
                    else
                    {
                        AbilitySequence = new[] { 1, 2, 3, 1, 1, 4, 1, 3, 1, 3, 4, 3, 3, 2, 2, 4, 2, 2 };
                        Tipo = " Lane";
                    }
                    break;
                case "Fizz":
                    AbilitySequence = new[] { 1, 3, 2, 3, 3, 4, 3, 1, 3, 1, 4, 1, 1, 2, 2, 4, 2, 2 };
                    break;
                case "Galio":
                    AbilitySequence = new[] { 1, 3, 2, 1, 1, 4, 1, 3, 1, 3, 4, 3, 3, 2, 2, 4, 2, 2 };
                    break;
                case "Gangplank":
                    AbilitySequence = new[] { 1, 2, 3, 1, 1, 4, 1, 3, 1, 3, 4, 3, 3, 2, 2, 4, 2, 2 };
                    break;
                case "Garen":
                    AbilitySequence = new[] { 1, 3, 2, 3, 3, 4, 3, 1, 3, 1, 4, 1, 1, 2, 2, 4, 2, 2 };
                    break;
                case "Gnar":
                    AbilitySequence = new[] { 1, 2, 3, 1, 1, 4, 1, 2, 1, 2, 4, 2, 2, 3, 3, 4, 3, 3 };
                    break;
                case "Gragas":
                    AbilitySequence = new[] { 1, 3, 1, 2, 1, 4, 1, 2, 1, 2, 4, 2, 2, 3, 3, 4, 3, 3 };
                    break;
                case "Graves":
                    AbilitySequence = new[] { 1, 3, 2, 1, 1, 4, 1, 3, 1, 3, 4, 3, 3, 2, 2, 4, 2, 2 };
                    break;
                case "Hecarim":
                    AbilitySequence = new[] { 1, 2, 3, 1, 1, 4, 1, 2, 1, 2, 4, 2, 2, 3, 3, 4, 3, 3 };
                    break;
                case "Heimerdinger":
                    AbilitySequence = new[] { 1, 2, 3, 1, 1, 4, 1, 2, 1, 2, 4, 2, 2, 3, 3, 4, 3, 3 };
                    break;
                case "Illaoi":
                    AbilitySequence = new[] { 1, 2, 3, 1, 1, 4, 1, 2, 1, 2, 4, 2, 2, 3, 3, 4, 3, 3 };
                    break;
                case "Irelia":
                    AbilitySequence = new[] { 3, 1, 2, 2, 2, 4, 2, 3, 2, 3, 4, 3, 3, 1, 1, 4, 1, 1 };
                    break;
                case "Janna":
                    AbilitySequence = new[] { 3, 1, 2, 3, 3, 4, 3, 2, 3, 2, 4, 2, 2, 1, 1, 4, 1, 1 };
                    break;
                case "JarvanIV":
                    if (smite != null && smite.Slot != SpellSlot.Unknown)
                    {
                        AbilitySequence = new[] { 3, 1, 2, 1, 1, 4, 1, 3, 1, 3, 4, 3, 3, 2, 2, 4, 2, 2 };
                        Tipo = " Jungler";
                    }
                    else
                    {
                        AbilitySequence = new[] { 1, 3, 1, 2, 1, 4, 1, 3, 1, 3, 4, 3, 2, 3, 2, 4, 2, 2 };
                        Tipo = " Lane";
                    }
                    break;
                case "Jax":
                    AbilitySequence = new[] { 3, 1, 2, 2, 2, 4, 2, 3, 2, 3, 4, 3, 3, 1, 1, 4, 1, 1 };
                    break;
                case "Jayce":
                    AbilitySequence = new[] { 1, 3, 2, 1, 1, 2, 1, 3, 1, 3, 1, 3, 3, 2, 2, 3, 2, 2 };
                    ROff = -1;
                    break;                
                case "Jhin":
                    AbilitySequence = new[] { 1, 2, 1, 3, 1, 4, 1, 2, 1, 3, 4, 2, 2, 2, 3, 4, 3, 3 };
                    break;
                case "Jinx":
                    AbilitySequence = new[] { 1, 3, 2, 1, 1, 4, 1, 2, 1, 2, 4, 2, 2, 3, 3, 4, 3, 3 };
                    break;
                case "Karma":
                    AbilitySequence = new[] { 1, 3, 1, 2, 1, 4, 1, 2, 1, 2, 4, 2, 2, 3, 3, 4, 3, 3 };
                    ROff = -1;
                    break;
                case "Karthus":
                    AbilitySequence = new[] { 1, 3, 2, 1, 1, 4, 1, 3, 1, 3, 4, 3, 3, 2, 2, 4, 2, 2 };
                    break;
                case "Kassadin":
                    AbilitySequence = new[] { 1, 2, 3, 1, 1, 4, 1, 3, 1, 3, 4, 3, 3, 2, 2, 4, 2, 2 };
                    break;
                case "Katarina":
                    AbilitySequence = new[] { 1, 3, 2, 1, 1, 4, 1, 2, 1, 2, 4, 2, 2, 3, 3, 4, 3, 3 };
                    break;
                case "Kalista":
                    AbilitySequence = new[] { 3, 1, 3, 2, 3, 4, 1, 3, 3, 1, 4, 1, 1, 2, 2, 4, 2, 2 };
                    break;
                case "Kayle":
                    AbilitySequence = new[] { 3, 1, 2, 3, 3, 4, 3, 1, 3, 1, 4, 1, 1, 2, 2, 4, 2, 2 };
                    break;
                case "Kennen":
                    AbilitySequence = new[] { 2, 1, 3, 2, 2, 4, 2, 1, 2, 1, 4, 1, 1, 3, 3, 4, 3, 3 };
                    break;
                case "Khazix":
                    AbilitySequence = new[] { 1, 2, 3, 1, 1, 4, 1, 2, 1, 2, 4, 2, 2, 3, 3, 4, 3, 3 };
                    break;
                case "Kindred":
                    AbilitySequence = new[] { 2, 1, 3, 1, 1, 4, 1, 3, 1, 3, 4, 3, 3, 2, 2, 4, 2, 2 };
                    break;
                case "KogMaw":
                    if (Player.TotalMagicalDamage > 0)
                    {
                        AbilitySequence = new[] { 3, 2, 1, 3, 3, 4, 3, 1, 3, 1, 4, 1, 1, 2, 2, 4, 2, 2 };
                        Tipo = " AP";
                    }
                    else
                    {
                        AbilitySequence = new[] { 2, 1, 3, 2, 2, 4, 2, 1, 2, 1, 4, 1, 1, 3, 3, 4, 3, 3 };
                        Tipo = " AD";
                    }
                    break;
                case "Leblanc":
                    AbilitySequence = new[] { 2, 1, 3, 2, 2, 4, 2, 1, 2, 1, 4, 1, 1, 3, 3, 4, 3, 3 };
                    break;
                case "LeeSin":
                    if (smite != null && smite.Slot != SpellSlot.Unknown)
                    {
                        AbilitySequence = new[] { 1, 3, 2, 1, 1, 4, 1, 2, 1, 2, 4, 2, 2, 3, 3, 4, 3, 3 };
                        Tipo = " Jungler";
                    }
                    else
                    {
                        AbilitySequence = new[] { 1, 2, 3, 1, 1, 4, 1, 3, 1, 3, 4, 3, 3, 2, 2, 4, 2, 2 };
                        Tipo = " Lane";
                    }
                    break;
                case "Leona":
                    AbilitySequence = new[] { 3, 1, 2, 2, 2, 4, 2, 3, 2, 3, 4, 3, 3, 1, 1, 4, 1, 1 };
                    break;
                case "Lissandra":
                    AbilitySequence = new[] { 1, 3, 2, 1, 1, 4, 1, 2, 1, 2, 4, 2, 2, 3, 3, 4, 3, 3 };
                    break;
                case "Lucian":
                    AbilitySequence = new[] { 1, 3, 2, 1, 1, 4, 1, 2, 1, 2, 4, 2, 2, 3, 3, 4, 3, 3 };
                    break;
                case "Lulu":
                    AbilitySequence = new[] { 1, 3, 2, 3, 3, 4, 3, 2, 3, 2, 4, 2, 2, 1, 1, 4, 1, 1 };
                    break;
                case "Lux":
                    AbilitySequence = new[] { 3, 1, 2, 3, 3, 4, 3, 1, 3, 1, 4, 1, 1, 2, 2, 4, 2, 2 };
                    break;
                case "Malphite":
                    AbilitySequence = new[] { 3, 1, 2, 3, 3, 4, 3, 2, 3, 2, 4, 2, 2, 1, 1, 4, 1, 1 };
                    break;
                case "Malzahar":
                    AbilitySequence = new[] { 1, 3, 2, 3, 3, 4, 3, 1, 3, 1, 4, 1, 1, 2, 2, 4, 2, 2 };
                    break;
                case "Maokai":
                    if (smite != null && smite.Slot != SpellSlot.Unknown)
                    {
                        AbilitySequence = new[] { 3, 2, 1, 1, 1, 4, 1, 2, 1, 2, 4, 2, 2, 3, 3, 4, 3, 3 };
                        Tipo = " Jungler";
                    }
                    else
                    {
                        AbilitySequence = new[] { 3, 1, 2, 1, 1, 4, 1, 2, 1, 2, 4, 2, 2, 3, 3, 4, 3, 3 };
                        Tipo = " Lane";
                    }
                    break;
                case "MasterYi":
                    AbilitySequence = new[] { 1, 3, 2, 1, 1, 4, 1, 3, 1, 3, 4, 3, 3, 2, 2, 4, 2, 2 };
                    break;
                case "MissFortune":
                    AbilitySequence = new[] { 1, 2, 3, 2, 2, 4, 2, 1, 2, 1, 4, 1, 1, 3, 3, 4, 3, 3 };
                    break;
                case "Mordekaiser":
                    AbilitySequence = new[] { 3, 1, 2, 3, 3, 4, 3, 1, 3, 1, 4, 1, 1, 2, 2, 4, 2, 2 };
                    break;
                case "Morgana":
                    AbilitySequence = new[] { 1, 3, 2, 1, 1, 4, 1, 2, 1, 2, 4, 2, 2, 3, 3, 4, 3, 3 };
                    break;
                case "Nami":
                    AbilitySequence = new[] { 2, 1, 3, 1, 1, 4, 1, 3, 1, 3, 4, 3, 3, 2, 2, 4, 2, 2 };
                    break;
                case "Nasus":
                    if (smite != null && smite.Slot != SpellSlot.Unknown)
                    {
                        AbilitySequence = new[] { 3, 1, 2, 3, 3, 4, 3, 2, 3, 2, 4, 2, 1, 2, 1, 4, 1, 1 };
                        Tipo = " Jungler";
                    }
                    else
                    {
                        AbilitySequence = new[] { 1, 2, 3, 1, 1, 4, 1, 2, 1, 2, 4, 2, 2, 3, 3, 4, 3, 3 };
                        Tipo = " Lane";
                    }
                    break;
                case "Nautilus":
                    AbilitySequence = new[] { 2, 3, 1, 3, 3, 4, 3, 2, 3, 2, 4, 2, 2, 1, 1, 4, 1, 1 };
                    break;
                case "Nidalee":
                    AbilitySequence = new[] { 1, 3, 2, 1, 1, 4, 1, 3, 1, 3, 4, 3, 3, 2, 2, 4, 2, 2 };
                    ROff = -1;
                    break;
                case "Nocturne":
                    if (smite != null && smite.Slot != SpellSlot.Unknown)
                    {
                        AbilitySequence = new[] { 1, 3, 2, 1, 1, 4, 1, 3, 1, 3, 4, 3, 2, 3, 2, 4, 2, 2 };
                        Tipo = " Jungler";
                    }
                    else
                    {
                        AbilitySequence = new[] { 1, 3, 2, 1, 1, 4, 1, 3, 1, 3, 4, 3, 3, 2, 2, 4, 2, 2 };
                        Tipo = " Lane";
                    }
                    break;
                case "Nunu":
                    if (smite != null && smite.Slot != SpellSlot.Unknown)
                    {
                        AbilitySequence = new[] { 1, 3, 2, 3, 3, 4, 3, 2, 3, 2, 4, 2, 2, 1, 1, 4, 1, 1 };
                        Tipo = " Jungler";
                    }
                    else
                    {
                        AbilitySequence = new[] { 3, 1, 2, 3, 3, 4, 3, 1, 3, 1, 4, 1, 1, 2, 2, 4, 2, 2 };
                        Tipo = " Lane";
                    }
                    break;
                case "Olaf":
                    AbilitySequence = new[] { 2, 1, 3, 3, 3, 4, 3, 1, 3, 1, 4, 1, 1, 2, 2, 4, 2, 2 };
                    break;
                case "Orianna":
                    AbilitySequence = new[] { 1, 2, 3, 1, 1, 4, 1, 2, 1, 2, 4, 2, 2, 3, 3, 4, 3, 3 };
                    break;
                case "Pantheon":
                    AbilitySequence = new[] { 1, 2, 3, 1, 1, 4, 1, 3, 1, 3, 4, 3, 3, 2, 2, 4, 2, 2 };
                    break;
                case "Poppy":
                    AbilitySequence = new[] { 1, 3, 2, 1, 1, 4, 1, 3, 1, 3, 4, 3, 3, 2, 2, 4, 2, 2 };
                    break;
                case "Quinn":
                    AbilitySequence = new[] { 1, 3, 2, 1, 1, 4, 1, 3, 1, 3, 4, 3, 3, 2, 2, 4, 2, 2 };
                    break;
                case "Rammus":
                    AbilitySequence = new[] { 2, 1, 3, 2, 3, 4, 2, 3, 3, 3, 4, 2, 2, 1, 1, 4, 1, 1 };
                    break;
                case "Renekton":
                    AbilitySequence = new[] { 2, 1, 3, 1, 1, 4, 1, 3, 1, 3, 4, 3, 3, 2, 2, 4, 2, 2 };
                    break;
                case "Rengar":
                    if (smite != null && smite.Slot != SpellSlot.Unknown)
                    {
                        AbilitySequence = new[] { 1, 2, 3, 1, 1, 4, 1, 3, 1, 3, 4, 3, 3, 2, 2, 4, 2, 2 };
                        Tipo = " Jungler";
                    }
                    else
                    {
                        AbilitySequence = new[] { 1, 2, 3, 1, 1, 4, 1, 3, 1, 3, 4, 3, 3, 2, 2, 4, 2, 2 };
                        Tipo = " Lane";
                    }
                    break;
                case "Riven":
                    if (smite != null && smite.Slot != SpellSlot.Unknown)
                    {
                        AbilitySequence = new[] { 1, 2, 1, 3, 1, 4, 1, 2, 1, 2, 4, 2, 2, 3, 3, 4, 3, 3 };
                        Tipo = " Jungler";
                    }
                    else
                    {
                        AbilitySequence = new[] { 1, 2, 3, 1, 1, 4, 1, 2, 1, 2, 4, 2, 2, 3, 3, 4, 3, 3 };
                        Tipo = " Lane";
                    }
                    break;
                case "Rumble":
                    AbilitySequence = new[] { 1, 3, 2, 1, 1, 4, 1, 3, 1, 3, 4, 3, 3, 2, 2, 4, 2, 2 };
                    break;
                case "RekSai":
                    if (smite != null && smite.Slot != SpellSlot.Unknown)
                    {
                        AbilitySequence = new[] { 1, 2, 3, 1, 1, 4, 1, 3, 1, 3, 4, 3, 3, 2, 2, 4, 2, 2 };
                        Tipo = " Jungler";
                    }
                    else
                    {
                        AbilitySequence = new[] { 1, 2, 3, 1, 1, 4, 1, 3, 1, 3, 4, 3, 3, 2, 2, 4, 2, 2 };
                        Tipo = " Lane";
                    }
                    break;
                case "Ryze":
                    AbilitySequence = new[] { 1, 2, 3, 1, 1, 4, 1, 3, 1, 2, 4, 2, 2, 2, 3, 4, 3, 3 };
                    break;
                case "Sejuani":
                    AbilitySequence = new[] { 2, 3, 1, 2, 2, 4, 2, 1, 2, 3, 4, 3, 3, 3, 1, 4, 1, 1 };
                    break;
                case "Shaco":
                    if (Player.PercentMagicDamageMod > Player.PercentPhysicalDamageMod)
                    {
                        AbilitySequence = new[] { 2, 1, 3, 2, 2, 4, 2, 3, 2, 3, 4, 3, 3, 1, 1, 4, 1, 1 };
                        Tipo = " AP";
                    }
                    else
                    {
                        AbilitySequence = new[] { 1, 3, 2, 1, 1, 4, 1, 3, 1, 3, 4, 3, 3, 2, 2, 4, 2, 2 };
                        Tipo = " AD";
                    }
                    break;
                case "Shen":
                    AbilitySequence = new[] { 1, 3, 2, 1, 1, 4, 1, 2, 1, 2, 4, 2, 2, 3, 3, 4, 3, 3 };
                    break;
                case "Shyvana":
                    AbilitySequence = new[] { 2, 1, 3, 2, 2, 4, 2, 3, 2, 3, 4, 3, 3, 1, 1, 4, 1, 1 };
                    break;
                case "Singed":
                    AbilitySequence = new[] { 1, 3, 2, 1, 1, 4, 1, 3, 1, 3, 4, 3, 3, 2, 2, 4, 2, 2 };
                    break;
                case "Sion":
                    AbilitySequence = new[] { 1, 2, 3, 1, 1, 4, 1, 2, 1, 2, 4, 2, 2, 3, 3, 4, 3, 3 };
                    break;
                case "Sivir":
                    AbilitySequence = new[] { 1, 2, 3, 1, 1, 4, 1, 2, 1, 2, 4, 2, 2, 3, 3, 4, 3, 3 };
                    break;
                case "Skarner":
                    if (smite != null && smite.Slot != SpellSlot.Unknown)
                    {
                        AbilitySequence = new[] { 1, 3, 2, 1, 1, 4, 1, 2, 1, 3, 4, 3, 2, 2, 3, 4, 3, 2 };
                        Tipo = " Jungler";
                    }
                    else
                    {
                        AbilitySequence = new[] { 1, 2, 1, 2, 1, 4, 1, 2, 1, 2, 4, 2, 3, 3, 3, 4, 3, 3 };
                        Tipo = " Lane";
                    }
                    break;
                case "Sona":
                    AbilitySequence = new[] { 1, 2, 3, 1, 2, 4, 1, 2, 1, 2, 4, 1, 2, 3, 3, 4, 3, 3 };
                    break;
                case "Soraka":
                    AbilitySequence = new[] { 2, 1, 3, 2, 2, 4, 2, 1, 2, 1, 4, 1, 1, 3, 3, 4, 3, 3 };
                    break;
                case "Swain":
                    AbilitySequence = new[] { 1, 2, 3, 1, 1, 4, 1, 3, 1, 3, 4, 3, 3, 2, 2, 4, 2, 2 };
                    break;
                case "Syndra":
                    AbilitySequence = new[] { 1, 3, 2, 1, 1, 4, 1, 2, 1, 2, 4, 2, 2, 3, 3, 4, 3, 3 };
                    break;
                case "Talon":
                    AbilitySequence = new[] { 2, 3, 1, 2, 2, 4, 2, 1, 2, 1, 4, 1, 1, 3, 3, 4, 3, 3 };
                    break;
                case "Taric":
                    AbilitySequence = new[] { 3, 2, 1, 2, 2, 4, 2, 3, 2, 3, 4, 3, 3, 1, 1, 4, 1, 1 };
                    break;
                case "TahmKench":
                    AbilitySequence = new[] { 1, 2, 3, 1, 1, 4, 1, 2, 1, 2, 4, 2, 2, 3, 3, 4, 3, 3 };
                    break;
                case "Teemo":
                    AbilitySequence = new[] { 3, 1, 2, 3, 3, 4, 3, 1, 3, 1, 4, 1, 1, 2, 2, 4, 2, 2 };
                    break;
                case "Thresh":
                    AbilitySequence = new[] { 1, 3, 2, 2, 2, 4, 2, 3, 2, 3, 4, 3, 3, 1, 1, 4, 1, 1 };
                    break;
                case "Tristana":
                    AbilitySequence = new[] { 3, 1, 2, 3, 3, 4, 3, 1, 3, 1, 4, 1, 1, 2, 2, 4, 2, 2 };
                    break;
                case "Trundle":
                    if (smite != null && smite.Slot != SpellSlot.Unknown)
                    {
                        AbilitySequence = new[] { 1, 2, 3, 1, 1, 4, 1, 2, 1, 3, 4, 2, 2, 2, 3, 4, 3, 3 };
                        Tipo = " Jungler";
                    }
                    else
                    {
                        AbilitySequence = new[] { 1, 2, 1, 3, 1, 4, 1, 2, 1, 2, 4, 2, 2, 3, 3, 4, 3, 3 };
                        Tipo = " Lane";
                    }
                    break;
                case "Tryndamere":
                    AbilitySequence = new[] { 3, 2, 1, 1, 1, 4, 1, 2, 1, 2, 4, 2, 2, 3, 3, 4, 3, 3 };
                    break;
                case "TwistedFate":
                    if (Player.TotalMagicalDamage > 0)
                    {
                        AbilitySequence = new[] { 1, 3, 1, 2, 1, 4, 1, 3, 1, 3, 4, 3, 3, 2, 2, 4, 2, 2 };
                        Tipo = " AP";
                    }
                    else
                    {
                        AbilitySequence = new[] { 2, 3, 3, 2, 3, 4, 3, 2, 3, 2, 4, 2, 4, 1, 1, 1, 1, 1 };
                        Tipo = " AD";
                    }
                    break;
                case "Twitch":
                    AbilitySequence = new[] { 3, 2, 1, 3, 3, 4, 3, 1, 3, 1, 4, 1, 1, 2, 2, 4, 2, 2 };
                    break;
                case "Udyr":
                    AbilitySequence = new[] { 4, 2, 3, 4, 4, 1, 4, 2, 4, 2, 2, 2, 3, 3, 3, 3, 1, 1 };
                    Tipo = " Jungler";
                    break;
                case "Urgot":
                    AbilitySequence = new[] { 3, 1, 2, 1, 1, 4, 1, 2, 1, 2, 4, 2, 2, 3, 3, 4, 3, 3 };
                    break;
                case "Varus":
                    AbilitySequence = new[] { 1, 2, 3, 1, 1, 4, 1, 2, 1, 2, 4, 2, 2, 3, 3, 4, 3, 3 };
                    break;
                case "Vayne":
                    if (smite != null && smite.Slot != SpellSlot.Unknown)
                    {
                        AbilitySequence = new[] { 2, 1, 3, 2, 2, 4, 2, 1, 2, 1, 4, 1, 1, 3, 3, 4, 3, 3 };
                        Tipo = " Jungler";
                    }
                    else
                    {
                        AbilitySequence = new[] { 1, 3, 2, 2, 2, 4, 2, 1, 2, 1, 4, 1, 1, 3, 3, 4, 3, 3 };
                        Tipo = " Lane";
                    }
                    break;
                case "Veigar":
                    AbilitySequence = new[] { 1, 3, 2, 1, 1, 4, 1, 3, 1, 3, 4, 3, 3, 2, 2, 4, 2, 2 };
                    break;
                case "Velkoz":
                    AbilitySequence = new[] { 1, 2, 3, 1, 1, 4, 1, 2, 1, 2, 4, 2, 2, 3, 3, 4, 3, 3 };
                    break;
                case "Vi":
                    if (smite != null && smite.Slot != SpellSlot.Unknown)
                    {
                        AbilitySequence = new[] { 3, 1, 1, 2, 1, 4, 1, 2, 1, 2, 4, 2, 2, 3, 3, 4, 3, 3 };
                        Tipo = " Jungler";
                    }
                    else
                    {
                        AbilitySequence = new[] { 3, 1, 1, 2, 1, 4, 1, 2, 1, 2, 4, 2, 2, 3, 3, 4, 3, 3 };
                        Tipo = " Lane";
                    }
                    break;
                case "Viktor":
                    AbilitySequence = new[] { 3, 1, 2, 3, 3, 4, 3, 1, 3, 1, 4, 1, 1, 2, 2, 4, 2, 2 };
                    break;
                case "Vladimir":
                    AbilitySequence = new[] { 1, 2, 3, 1, 1, 4, 1, 3, 1, 3, 4, 3, 3, 2, 2, 4, 2, 2 };
                    break;
                case "Volibear":
                    AbilitySequence = new[] { 2, 1, 3, 2, 2, 4, 2, 3, 2, 3, 4, 3, 3, 1, 1, 4, 1, 1 };
                    break;
                case "Warwick":
                    AbilitySequence = new[] { 2, 1, 3, 2, 2, 4, 2, 1, 2, 1, 4, 1, 1, 3, 3, 4, 3, 3 };
                    break;
                case "MonkeyKing":
                    AbilitySequence = new[] { 3, 1, 2, 1, 1, 4, 3, 1, 3, 1, 4, 3, 3, 2, 2, 4, 2, 2 };
                    break;
                case "Xerath":
                    AbilitySequence = new[] { 1, 3, 2, 1, 1, 4, 1, 2, 1, 2, 4, 2, 2, 3, 3, 4, 3, 3 };
                    break;
                case "XinZhao":
                    if (smite != null && smite.Slot != SpellSlot.Unknown)
                    {
                        AbilitySequence = new[] { 1, 2, 3, 1, 1, 4, 1, 3, 1, 2, 4, 2, 3, 2, 3, 4, 2, 3 };
                        Tipo = " Jungler";
                    }
                    else
                    {
                        AbilitySequence = new[] { 1, 2, 3, 1, 1, 4, 1, 2, 1, 2, 4, 2, 2, 3, 3, 4, 3, 3 };
                        Tipo = " Lane";
                    }
                    break;
                case "Yasuo":
                    AbilitySequence = new[] { 1, 3, 2, 1, 1, 4, 1, 3, 1, 3, 4, 3, 3, 2, 2, 4, 2, 2 };
                    break;
                case "Yorick":
                    AbilitySequence = new[] { 2, 3, 1, 3, 3, 4, 3, 2, 3, 1, 4, 2, 1, 2, 1, 4, 2, 1 };
                    break;
                case "Zac":
                    if (smite != null && smite.Slot != SpellSlot.Unknown)
                    {
                        AbilitySequence = new[] { 2, 1, 3, 3, 1, 4, 3, 1, 3, 1, 4, 3, 1, 2, 2, 4, 2, 2 };
                        Tipo = " Jungler";
                    }
                    else
                    {
                        AbilitySequence = new[] { 2, 3, 1, 2, 2, 4, 2, 3, 2, 3, 4, 3, 3, 1, 1, 4, 1, 1 };
                        Tipo = " Lane";
                    }
                    break;
                case "Zed":
                    AbilitySequence = new[] { 1, 2, 3, 1, 1, 4, 1, 3, 3, 1, 4, 3, 3, 2, 2, 4, 2, 2 };
                    break;
                case "Ziggs":
                    AbilitySequence = new[] { 1, 3, 2, 1, 1, 4, 1, 3, 1, 3, 4, 3, 3, 2, 2, 4, 2, 2 };
                    break;
                case "Zilean":
                    AbilitySequence = new[] { 1, 2, 3, 1, 1, 4, 1, 3, 1, 3, 4, 3, 3, 2, 2, 4, 2, 2 };
                    break;
                case "Zyra":
                    AbilitySequence = new[] { 3, 2, 1, 3, 1, 4, 3, 1, 3, 1, 4, 3, 1, 2, 2, 4, 2, 2 };
                    break;
            }

            #endregion

            Game.OnTick += Game_OnTick;
        }

        private static void Game_OnTick(EventArgs args)
        {
            try
            {
                /* Debug
                Chat.Print("Q Array: " + Level[0]);
                Chat.Print("W Array: " + Level[1]);
                Chat.Print("E Array: " + Level[2]);
                Chat.Print("R Array: " + Level[3]);
                */

                var qL = Player.Spellbook.GetSpell(SpellSlot.Q).Level + QOff;
                var wL = Player.Spellbook.GetSpell(SpellSlot.W).Level + WOff;
                var eL = Player.Spellbook.GetSpell(SpellSlot.E).Level + EOff;
                var rL = Player.Spellbook.GetSpell(SpellSlot.R).Level + ROff;

                Level = new[] {0, 0, 0, 0};

                for (var i = 1; i <= ObjectManager.Player.Level; i++)
                {
                    switch (AbilitySequence[i - 1])
                    {
                        case 1:
                            Level[0] += 1;
                            break;
                        case 2:
                            Level[1] += 1;
                            break;
                        case 3:
                            Level[2] += 1;
                            break;
                        case 4:
                            Level[3] += 1;
                            break;
                    }
                }

                if (qL < Level[0])
                {
                    LevelUp(SpellSlot.Q);
                }

                if (wL < Level[1])
                {
                    LevelUp(SpellSlot.W);
                }

                if (eL < Level[2])
                {
                    LevelUp(SpellSlot.E);
                }
                
                if (rL < Level[3])
                {
                    LevelUp(SpellSlot.R);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public static void LevelUp(SpellSlot slot)
        {
            EloBuddy.SDK.Core.DelayAction(() =>
            {
                Player.Spellbook.LevelSpell(slot);
            }, new Random().Next(0, Delay));
        }
    }
}
