using System;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Constants;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Rendering;
using SharpDX;

namespace HoolaRiven
{
    class Program
    {
        // todo: add this
        //private static readonly HpBarIndicator Indicator = new HpBarIndicator();
        private const string IsFirstR = "RivenFengShuiEngine";
        private const string IsSecondR = "rivenizunablade";
        public static Menu menu;
        private static readonly AIHeroClient _Player = ObjectManager.Player;
        private static SpellDataInst Flash; //Player.GetSpellSlot("summonerFlash");
        public static Spell.Active Q = new Spell.Active(SpellSlot.Q);
        public static Spell.Active E = new Spell.Active(SpellSlot.E, 325);

        public static Spell.Skillshot R = new Spell.Skillshot(SpellSlot.R, 900, SkillShotType.Cone, 250, 1600, 45)
        {
            MinimumHitChance = HitChance.High,
            AllowedCollisionCount = -1
        };
        private static int QStack = 1;
        //public static Render.Text Timer, Timer2;
        private static bool forceQ;
        private static bool forceW;
        private static bool forceR;
        private static bool forceR2;
        private static bool forceItem;
        private static float LastQ;
        private static float LastR;
        private static AttackableUnit QTarget;
        public static Item Hydra;

        public static Spell.Active W = new Spell.Active(SpellSlot.W,(uint)(70 + ObjectManager.Player.BoundingRadius + 120));

        public static uint WRange
        {
            get
            {
                return (uint)
                        (70 + ObjectManager.Player.BoundingRadius +
                         (ObjectManager.Player.HasBuff("RivenFengShuiEngine") ? 195 : 120));
            }
        }

        private static bool Dind
        {
            get { return menu["Dind"].Cast<CheckBox>().CurrentValue; }
        }

        private static bool DrawCB
        {
            get { return menu["DrawCB"].Cast<CheckBox>().CurrentValue; }
        }

        private static bool KillstealW
        {
            get { return menu["killstealw"].Cast<CheckBox>().CurrentValue; }
        }

        private static bool KillstealR
        {
            get { return menu["killstealr"].Cast<CheckBox>().CurrentValue; }
        }

        private static bool DrawAlwaysR
        {
            get { return menu["DrawAlwaysR"].Cast<CheckBox>().CurrentValue; }
        }

        private static bool DrawUseHoola
        {
            get { return menu["DrawUseHoola"].Cast<CheckBox>().CurrentValue; }
        }

        private static bool DrawFH
        {
            get { return menu["DrawFH"].Cast<CheckBox>().CurrentValue; }
        }
/*

        private static bool DrawTimer1
        {
            get { return menu["DrawTimer1"].Cast<CheckBox>().CurrentValue; }
        }

        private static bool DrawTimer2
        {
            get { return menu["DrawTimer2"].Cast<CheckBox>().CurrentValue; }
        }
*/

        private static bool DrawHS
        {
            get { return menu["DrawHS"].Cast<CheckBox>().CurrentValue; }
        }

        private static bool DrawBT
        {
            get { return menu["DrawBT"].Cast<CheckBox>().CurrentValue; }
        }

        private static bool UseHoola
        {
            get { return menu["UseHoola"].Cast<KeyBind>().CurrentValue; }
        }

        private static bool BurstCombo
        {
            get { return menu["burst"].Cast<KeyBind>().CurrentValue; }
        }
        private static bool FastHarassCombo
        {
            get { return menu["fastharass"].Cast<KeyBind>().CurrentValue; }
        }

        private static bool AlwaysR
        {
            get { return menu["AlwaysR"].Cast<KeyBind>().CurrentValue; }
        }

        private static bool AutoShield
        {
            get { return menu["AutoShield"].Cast<CheckBox>().CurrentValue; }
        }

        private static bool Shield
        {
            get { return menu["Shield"].Cast<CheckBox>().CurrentValue; }
        }

        private static bool KeepQ
        {
            get { return menu["KeepQ"].Cast<CheckBox>().CurrentValue; }
        }

        private static int QD
        {
            get { return menu["QD"].Cast<Slider>().CurrentValue; }
        }

        private static int QLD
        {
            get { return menu["QLD"].Cast<Slider>().CurrentValue; }
        }

        private static int AutoW
        {
            get { return menu["AutoW"].Cast<Slider>().CurrentValue; }
        }

        private static bool ComboW
        {
            get { return menu["ComboW"].Cast<CheckBox>().CurrentValue; }
        }

        private static bool RMaxDam
        {
            get { return menu["RMaxDam"].Cast<CheckBox>().CurrentValue; }
        }

        private static bool RKillable
        {
            get { return menu["RKillable"].Cast<CheckBox>().CurrentValue; }
        }

        private static int LaneW
        {
            get { return menu["LaneW"].Cast<Slider>().CurrentValue; }
        }

        private static bool LaneE
        {
            get { return menu["LaneE"].Cast<CheckBox>().CurrentValue; }
        }

        private static bool WInterrupt
        {
            get { return menu["WInterrupt"].Cast<CheckBox>().CurrentValue; }
        }

        private static bool Qstrange
        {
            get { return false; } //menu["Qstrange"].Cast<CheckBox>().CurrentValue; }
        }

        private static bool FirstHydra
        {
            get { return menu["FirstHydra"].Cast<CheckBox>().CurrentValue; }
        }

        private static bool LaneQ
        {
            get { return menu["LaneQ"].Cast<CheckBox>().CurrentValue; }
        }

        private static bool Youmu
        {
            get { return menu["youmu"].Cast<CheckBox>().CurrentValue; }
        }


        private static void Main()
        {
            Loading.OnLoadingComplete += OnGameLoad;
        }

        //private static void Main() => CustomEvents.Game.OnGameLoad += OnGameLoad;

        private static void OnGameLoad(EventArgs args)
        {
            Hacks.RenderWatermark = false;
            if (_Player.ChampionName != "Riven") return;
            Flash = ObjectManager.Player.Spellbook.Spells.FirstOrDefault(a => a.Name.ToLower().Contains("summonerflash"));
            Chat.Print("Loaded Succesfully", Color.DodgerBlue);

            OnMenuLoad();


            //Timer = new Render.Text("Q Expiry =>  " + ((double)(LastQ - Environment.TickCount + 3800) / 1000).ToString("0.0"), (int)Drawing.WorldToScreen(Player.Position).X - 140, (int)Drawing.WorldToScreen(Player.Position).Y + 10, 30, Color.MidnightBlue, "calibri");
            //Timer2 = new Render.Text("R Expiry =>  " + (((double)LastR - Environment.TickCount + 15000) / 1000).ToString("0.0"), (int)Drawing.WorldToScreen(Player.Position).X - 60, (int)Drawing.WorldToScreen(Player.Position).Y + 10, 30, Color.IndianRed, "calibri");

            Game.OnTick += OnTick;
            Drawing.OnDraw += Drawing_OnDraw;
            //todo: add damage indicator
            Drawing.OnEndScene += Drawing_OnEndScene;
            Obj_AI_Base.OnProcessSpellCast += OnCast;
            Obj_AI_Base.OnSpellCast += OnDoCast;
            Obj_AI_Base.OnSpellCast += OnDoCastLC;
            Obj_AI_Base.OnPlayAnimation += OnPlay;
            Obj_AI_Base.OnProcessSpellCast += OnCasting;
        }

        //Interrupter2.OnInterruptableTarget += Interrupt;

        
        private static bool HasTitan()
        {
            //(Items.HasItem(3748) && Items.CanUseItem(3748));
            var id = ObjectManager.Player.InventoryItems.FirstOrDefault(a => a.Id == (ItemId) 3748);
            if (id == null || !((new Item(id.Id, 300)).IsReady()))
            {
                return false;
            }
            return true;
        }


        private static void CastTitan()
        {
            var id = ObjectManager.Player.InventoryItems.FirstOrDefault(a => a.Id == (ItemId) 3748);
            if (id != null)
            {
                var HydraItem = new Item(id.Id, 300);
                if (HydraItem.IsReady())
                {
                    HydraItem.Cast();
                    Orbwalker.ResetAutoAttack();
                }
            }
        }
        private static readonly float _barLength = 104;
        private static readonly float _xOffset = 2;
        private static readonly float _yOffset = 9;
        private static void Drawing_OnEndScene(EventArgs args)
        {
            if (_Player.IsDead)
                return;
            if (!Dind) return;
            foreach (var aiHeroClient in EntityManager.Heroes.Enemies)
            {
                if (!aiHeroClient.IsHPBarRendered || !aiHeroClient.VisibleOnScreen) continue;

                var pos = new Vector2(aiHeroClient.HPBarPosition.X + _xOffset, aiHeroClient.HPBarPosition.Y + _yOffset);
                var fullbar = (_barLength) * (aiHeroClient.HealthPercent / 100);
                var damage = (_barLength) *
                                 ((getComboDamage(aiHeroClient) / aiHeroClient.MaxHealth) > 1
                                     ? 1
                                     : (getComboDamage(aiHeroClient) / aiHeroClient.MaxHealth));
                Line.DrawLine(System.Drawing.Color.Aqua, 9f, new Vector2(pos.X, pos.Y),
                    new Vector2(pos.X + (damage > fullbar ? fullbar : damage), pos.Y));
                Line.DrawLine(System.Drawing.Color.Black, 9, new Vector2(pos.X + (damage > fullbar ? fullbar : damage) - 2, pos.Y), new Vector2(pos.X + (damage > fullbar ? fullbar : damage) + 2, pos.Y));
            }
        }

        private static void OnDoCastLC(Obj_AI_Base Sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (_Player.IsDead)
                return;
            if (!Sender.IsMe || !args.SData.IsAutoAttack()) return;
            QTarget = (Obj_AI_Base) args.Target;
            if (args.Target is Obj_AI_Minion)
            {
                if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear))
                {
                    var Minions = EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Enemy, ObjectManager.Player.Position, 190).ToArray();
                    if (Minions.Length > 0)
                    {


                        if (HasTitan())
                        {
                            CastTitan();
                            return;
                        }
                        if (Q.IsReady() && LaneQ)
                        {
                            ForceItem();
                            Utils.DelayAction(() => ForceCastQ(Minions[0]), 1);
                        }
                        if ((!Q.IsReady() || (Q.IsReady() && !LaneQ)) && W.IsReady() && LaneW != 0 &&
                            Minions.Length >= LaneW)
                        {
                            ForceItem();
                            Utils.DelayAction(ForceW, 1);
                        }
                        if ((!Q.IsReady() || (Q.IsReady() && !LaneQ)) &&
                            (!W.IsReady() || (W.IsReady() && LaneW == 0) || Minions.Length < LaneW) &&
                            E.IsReady() && LaneE)
                        {
                            Player.CastSpell(SpellSlot.E, Minions[0].Position);
                            Utils.DelayAction(ForceItem, 1);
                        }
                    }
                }
            }
        }
        
        private static Item HasHydra()
        {

            var hydraId =
                ObjectManager.Player.InventoryItems.FirstOrDefault(
                    it => it.Id == ItemId.Ravenous_Hydra_Melee_Only || it.Id == ItemId.Tiamat_Melee_Only);
            if (hydraId != null)
            {
                return new Item(hydraId.Id, 300);
            }
            return null;
        }

        //private static  => Items.CanUseItem(3077) && Items.HasItem(3077) ? 3077 : Items.CanUseItem(3074) && Items.HasItem(3074) ? 3074 : 0;
        private static void OnDoCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (_Player.IsDead)
                return;
            //var spellName = args.SData.Name;
            if (!sender.IsMe || !args.SData.IsAutoAttack()) return;
            QTarget = (Obj_AI_Base) args.Target;

            if (args.Target is Obj_AI_Minion)
            {
                if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear))
                {
                    var Minions = EntityManager.MinionsAndMonsters.Monsters.Where(
                    m => m.Distance(ObjectManager.Player) < 250 + _Player.AttackRange + 70).ToArray();
                    if (Minions.Length > 0)
                    {


                        if (HasTitan())
                        {
                            CastTitan();
                            return;
                        }
                        if (Q.IsReady() && LaneQ)
                        {
                            ForceItem();
                            Utils.DelayAction(() => ForceCastQ(Minions[0]), 1);
                        }
                        if ((!Q.IsReady() || (Q.IsReady() && !LaneQ)) && W.IsReady() && LaneW != 0 &&
                            Minions.Length >= LaneW)
                        {
                            ForceItem();
                            Utils.DelayAction(ForceW, 1);
                        }
                        if ((!Q.IsReady() || (Q.IsReady() && !LaneQ)) &&
                            (!W.IsReady() || (W.IsReady() && LaneW == 0) || Minions.Length < LaneW) &&
                            E.IsReady() && LaneE)
                        {
                            Player.CastSpell(SpellSlot.E, Minions[0].Position);
                            Utils.DelayAction(ForceItem, 1);
                        }
                    }
                }
            }
    
            if (args.Target is Obj_AI_Turret || args.Target is Obj_Barracks || args.Target is Obj_BarracksDampener ||
                args.Target is Obj_Building)
                if (args.Target.IsValid && args.Target != null && Q.IsReady() && LaneQ &&
                    Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear))
                    ForceCastQ((Obj_AI_Base) args.Target);
            AIHeroClient client = args.Target as AIHeroClient;
            if (client != null)
            {
                var target = client;
                if (!target.IsValidTarget()) return;
                if (KillstealR && R.IsReady() && R.Name == IsSecondR)
                    if (target.Health < (Rdame(target, target.Health) + _Player.GetAutoAttackDamage(target)) &&
                        target.Health > _Player.GetAutoAttackDamage(target)) R.Cast(target.Position);
                if (KillstealW && W.IsReady())
                    if (target.Health <
                        (_Player.GetSpellDamage(target, SpellSlot.W)) + _Player.GetAutoAttackDamage(target) &&
                        target.Health > _Player.GetAutoAttackDamage(target)) W.Cast();
                if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
                {
                    if (HasTitan())
                    {
                        CastTitan();
                        return;
                    }
                    if (Q.IsReady())
                    {
                        ForceItem();
                        Utils.DelayAction(() => ForceCastQ(target), 1);
                    }
                    else if (W.IsReady() && InWRange(target))
                    {
                        ForceItem();
                        Utils.DelayAction(ForceW, 1);
                    }
                    else if (E.IsReady() && _Player.IsInAutoAttackRange(target)) Player.CastSpell(SpellSlot.E, target.Position);
                }
                // todo: fast harass
                
                if (FastHarassCombo)
                {
                    if (HasTitan())
                    {
                        CastTitan();
                        return;
                    }
                    if (W.IsReady() && InWRange(target))
                    {
                        ForceItem();
                        Utils.DelayAction( ForceW, 1);
                        Utils.DelayAction( () => ForceCastQ(target), 2);
                    }
                    else if (Q.IsReady())
                    {
                        ForceItem();
                        Utils.DelayAction(()=>ForceCastQ(target), 1);
                    }
                    else if (E.IsReady() && !ObjectManager.Player.IsInAutoAttackRange(target) && !InWRange(target))
                    {
                        EloBuddy.Player.CastSpell(SpellSlot.E, target.Position);
                    }
                }
                

                if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass))
                {
                    if (HasTitan())
                    {
                        CastTitan();
                        return;
                    }
                    if (QStack == 2 && Q.IsReady())
                    {
                        ForceItem();
                        Utils.DelayAction(() => ForceCastQ(target), 1);
                    }
                }
                // todo: burst
                
                if (BurstCombo)
                {
                    if (HasTitan())
                    {
                        CastTitan();
                        return;
                    }
                    if (R.IsReady() && R.Name == IsSecondR)
                    {
                        ForceItem();
                        Utils.DelayAction(ForceR2, 1);
                    }
                    else if (Q.IsReady())
                    {
                        ForceItem();
                        Utils.DelayAction(()=>ForceCastQ(target), 1);
                    }
                }
                
            }
        }

        private static void OnMenuLoad()
        {
            menu = MainMenu.AddMenu("Hoola Riven", "hoolariven");
            menu.AddGroupLabel("Combo");
            menu.Add("AlwaysR", new KeyBind("Forced R", false, KeyBind.BindTypes.PressToggle, 'G'));
            menu.Add("UseHoola",
                new KeyBind("Use Hoola Combo Logic (Toggle)", false, KeyBind.BindTypes.PressToggle, 'L'));
            menu.Add("ComboW", new CheckBox("Always use W"));
            menu.Add("RKillable", new CheckBox("Use R When Target Can Killable"));
            menu.AddGroupLabel("Lane");
            menu.Add("LaneQ", new CheckBox("Use Q While Laneclear"));
            menu.Add("LaneW", new Slider("Use W on {0} Minion (0 = Don't)", 5, 0, 5));
            menu.Add("LaneE", new CheckBox("Use E While Laneclear"));



            menu.AddGroupLabel("Misc");
            menu.Add("youmu", new CheckBox("Use Youmys When E", false));


            menu.Add("FirstHydra", new CheckBox("Flash Burst Hydra Cast before W", false));
            //menu.Add("Qstrange", new CheckBox("Reset animation for manual Q"));
            menu.Add("Winterrupt", new CheckBox("W interrupt"));
            menu.Add("AutoW", new Slider("Use W on {0} Minion (0 = Don't)", 5, 0, 5));
            menu.Add("RMaxDam", new CheckBox("Use Second R Max Damage"));
            menu.Add("killstealw", new CheckBox("Killsteal W"));
            menu.Add("killstealr", new CheckBox("Killsteal Second R"));
            menu.Add("AutoShield", new CheckBox("Auto Cast E"));
            menu.Add("Shield", new CheckBox("Auto Cast E While LastHit"));
            menu.Add("KeepQ", new CheckBox("Keep Q Alive"));
            menu.Add("QD", new Slider("First,Second Q Delay {0}", 29, 23, 43));
            menu.Add("QLD", new Slider("Third Q Delay {0}", 39, 36, 53));


            menu.AddGroupLabel("Draw");

            menu.Add("DrawAlwaysR", new CheckBox("Draw Always R Status"));
            //menu.Add("DrawTimer1", new CheckBox("Draw Q Expiry Time", true));
            //menu.Add("DrawTimer2", new CheckBox("Draw R Expiry Time", true));
            menu.Add("DrawUseHoola", new CheckBox("Draw Hoola Logic Status"));
            menu.Add("Dind", new CheckBox("Draw Damage Indicator"));
            menu.Add("DrawCB", new CheckBox("Draw Combo Engage Range"));
            menu.Add("DrawBT", new CheckBox("Draw Burst Engage Range"));
            menu.Add("DrawFH", new CheckBox("Draw FastHarass Engage Range"));
            menu.Add("DrawHS", new CheckBox("Draw Harass Engage Range"));

            menu.AddGroupLabel("Additional Keys");

            menu.Add("burst", new KeyBind("Shy burst", false, KeyBind.BindTypes.HoldActive, 'T'));
            menu.Add("fastharass", new KeyBind("Fast Harass", false, KeyBind.BindTypes.HoldActive, 'N'));
        }

        private static void Interrupt(AIHeroClient sender, EventArgs args)
        {
            if (sender.IsEnemy && W.IsReady() && sender.IsValidTarget() && !sender.IsZombie && WInterrupt)
            {
                if (sender.IsValidTarget(125 + _Player.BoundingRadius + sender.BoundingRadius)) W.Cast();
            }
        }

        private static void AutoUseW()
        {
            if (AutoW > 0)
            {
                if (_Player.CountEnemiesInRange(WRange) >= AutoW)
                {
                    ForceW();
                }
            }
        }

        private static int TickLimiter = 1;
        private static int LastGameTick = 0;
        private static void OnTick(EventArgs args)
        {
            if (_Player.IsDead)
                return;
            
            
            
            /*if (Orbwalker.ActiveModesFlags == Orbwalker.ActiveModes.None
                && !FastHarassCombo
                && !BurstCombo) return;*/
            /*if (LastGameTick + TickLimiter > Environment.TickCount) return;
            LastGameTick = Environment.TickCount;*/
            /* Timer.X = (int) Drawing.WorldToScreen(Player.Position).X - 60;
            Timer.Y = (int) Drawing.WorldToScreen(Player.Position).Y + 43;
            Timer2.X = (int) Drawing.WorldToScreen(Player.Position).X - 60;
            Timer2.Y = (int) Drawing.WorldToScreen(Player.Position).Y + 65;*/
            ForceSkill();
            UseRMaxDam();
            AutoUseW();
            Killsteal();
            if (BurstCombo) Burst();
            if (FastHarassCombo) FastHarass();
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo)) Combo();
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear)) Jungleclear();
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass)) Harass();
            
            //if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.)) FastHarass();
            
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Flee)) Flee();
            if (Environment.TickCount - LastQ >= 3650 && QStack != 1 && !_Player.IsRecalling() && KeepQ && Q.IsReady())
                Player.CastSpell(SpellSlot.Q, Game.CursorPos); //Game.CursorPosition
        }

        private static void Killsteal()
        {
            if (KillstealW && W.IsReady())
            {
                var targets = EntityManager.Heroes.Enemies.Where(x => x.IsValidTarget(R.Range) && !x.IsZombie);
                foreach (var target in targets)
                {
                    if (target.Health < _Player.GetSpellDamage(target, SpellSlot.W) && InWRange(target))
                        W.Cast();
                }
            }
            if (KillstealR && R.IsReady() && R.Name == IsSecondR)
            {
                var targets = EntityManager.Heroes.Enemies.Where(x => x.IsValidTarget(R.Range) && !x.IsZombie);
                foreach (var target in targets)
                {
                    if (target.Health < Rdame(target, target.Health) &&
                        (!target.HasBuff("kindrednodeathbuff") && !target.HasBuff("Undying Rage") &&
                         !target.HasBuff("JudicatorIntervention")))
                        R.Cast(target.Position);
                }
            }
        }

        private static void UseRMaxDam()
        {
            if (RMaxDam && R.IsReady() && R.Name == IsSecondR)
            {
                var targets = EntityManager.Heroes.Enemies.Where(x => x.IsValidTarget(R.Range) && !x.IsZombie);
                foreach (var target in targets)
                {
                    if (target.HealthPercent <= 0.25 &&
                        (!target.HasBuff("kindrednodeathbuff") || !target.HasBuff("Undying Rage") ||
                         !target.HasBuff("JudicatorIntervention")))
                        R.Cast(target.Position);
                }
            }
        }

        private static void Drawing_OnDraw(EventArgs args)
        {
            //temp
            if (_Player.IsDead)
                return;
            var heropos = Drawing.WorldToScreen(ObjectManager.Player.Position);


            /*if (QStack != 1 && DrawTimer1)
            {
                Timer.text = ("Q Expiry =>  " + ((double) (LastQ - Environment.TickCount + 3800)/1000).ToString("0.0") +
                              "S");
                Timer.OnEndScene();
            }

            if (Player.HasBuff("RivenFengShuiEngine") && DrawTimer2)
            {
                Timer2.text = ("R Expiry =>  " +
                               (((double) LastR - Environment.TickCount + 15000)/1000).ToString("0.0") + "S");
                Timer2.OnEndScene();
            }*/
            var green = Color.LimeGreen;
            var red = Color.IndianRed;
            if (DrawCB)
                Circle.Draw(E.IsReady() ? green : red, 250 + _Player.AttackRange + 70, ObjectManager.Player.Position);
            if (DrawBT && Flash != null && Flash.Slot != SpellSlot.Unknown)
                Circle.Draw(R.IsReady() && Flash.IsReady ? green : red, 800, ObjectManager.Player.Position);
            if (DrawFH)
                Circle.Draw(E.IsReady() && Q.IsReady() ? green : red, 450 + _Player.AttackRange + 70,
                    ObjectManager.Player.Position);
            if (DrawHS)
                Circle.Draw(Q.IsReady() && W.IsReady() ? green : red, 400, ObjectManager.Player.Position);
            if (DrawAlwaysR)
            {
                Drawing.DrawText(heropos.X - 40, heropos.Y + 20, System.Drawing.Color.DodgerBlue, "Always R  (     )");
                Drawing.DrawText(heropos.X + 32, heropos.Y + 20,
                    AlwaysR ? System.Drawing.Color.LimeGreen : System.Drawing.Color.Red,
                    AlwaysR ? "On" : "Off");
            }
            if (DrawUseHoola)
            {
                Drawing.DrawText(heropos.X - 40, heropos.Y + 33, System.Drawing.Color.DodgerBlue, "Hoola Logic  (     )");
                Drawing.DrawText(heropos.X + 50, heropos.Y + 33,
                    UseHoola ? System.Drawing.Color.LimeGreen : System.Drawing.Color.Red,
                    UseHoola ? "On" : "Off");
            }
            
            //Drawing.DrawText(heropos.X - 40, heropos.Y + 43, System.Drawing.Color.DodgerBlue, "Can AA:");
            //Drawing.DrawText(heropos.X + 50, heropos.Y + 43,
            //        Orbwalker.CanAutoAttack ? System.Drawing.Color.LimeGreen : System.Drawing.Color.Red,
            //        Orbwalker.CanAutoAttack ? "true" : "false");
        }

        private static void Jungleclear()
        {
            //temp
            var Mobs =
                EntityManager.MinionsAndMonsters.Monsters.Where(
                    m => m.Distance(ObjectManager.Player) < 250 + _Player.AttackRange + 70).OrderBy(m => m.MaxHealth);

            if (!Mobs.Any())
                return;
            var mob = Mobs.First();
            if (W.IsReady(200) && E.IsReady(1) && ObjectManager.Player.IsInAutoAttackRange(mob))
            {
                Player.CastSpell(SpellSlot.E, mob.Position);
                Utils.DelayAction(ForceItem, 1);
                Utils.DelayAction(ForceW, 200);

            }
            
            
            
            
        }

        private static void Combo()
        {
            var targetR = TargetSelector.GetTarget(250 + _Player.AttackRange + 70, DamageType.Physical);
            if (targetR == null || !targetR.IsValidTarget()) return;
            if (R.IsReady() && R.Name == IsFirstR && ObjectManager.Player.IsInAutoAttackRange(targetR) && AlwaysR)
            {
                ForceR();
                //CastR1();
            }
                
            if (R.IsReady() && R.Name == IsFirstR && W.IsReady() && InWRange(targetR) && ComboW && AlwaysR)
            {
                ForceR();
                //CastR1();
                Utils.DelayAction(ForceW, 1);
            }
            if (W.IsReady() && InWRange(targetR) && ComboW) W.Cast();
            if (UseHoola && R.IsReady() && R.Name == IsFirstR && W.IsReady() && E.IsReady() &&
                targetR.IsValidTarget() && !targetR.IsZombie && (IsKillableR(targetR) || AlwaysR))
            {
                if (!InWRange(targetR))
                {
                    Player.CastSpell(SpellSlot.E, targetR.Position);
                    ForceR();
                    Utils.DelayAction(ForceW, 200);
                    Utils.DelayAction(() => ForceCastQ(targetR), 200);
                }
            }
            else if (!UseHoola && R.IsReady() && R.Name == IsFirstR && W.IsReady() &&
                     E.IsReady() && targetR.IsValidTarget() && !targetR.IsZombie &&
                     (IsKillableR(targetR) || AlwaysR))
            {
                if (!InWRange(targetR))
                {
                    Player.CastSpell(SpellSlot.E, targetR.Position);
                    ForceR();
                    Utils.DelayAction(ForceW, 200);
                }
            }
            else if (UseHoola && W.IsReady() && E.IsReady())
            {
                if (targetR.IsValidTarget() && !targetR.IsZombie && !InWRange(targetR))
                {
                    Player.CastSpell(SpellSlot.E, targetR.Position);
                    Utils.DelayAction(CastYoumoo, 1);
                    Utils.DelayAction(ForceItem, 10);
                    Utils.DelayAction(ForceW, 200);
                    Utils.DelayAction(() => ForceCastQ(targetR), 305);
                }
            }
            else if (!UseHoola && W.IsReady() && E.IsReady())
            {
                if (targetR.IsValidTarget() && !targetR.IsZombie && !InWRange(targetR))
                {
                    Player.CastSpell(SpellSlot.E, targetR.Position);
                    Utils.DelayAction(CastYoumoo, 1);
                    Utils.DelayAction(ForceItem, 10);
                    Utils.DelayAction(ForceW, 240);
                }
            }
            else if (E.IsReady())
            {
                if (targetR.IsValidTarget() && !targetR.IsZombie && !InWRange(targetR))
                {
                    Player.CastSpell(SpellSlot.E, targetR.Position);
                }
            }
        }

        private static void Burst()
        {
            var target = TargetSelector.SelectedTarget;
            if (target == null || !target.IsValidTarget()) return;
            Orbwalker.ForcedTarget = target;
            Orbwalker.OrbwalkTo(target.ServerPosition);
            if (target.IsValidTarget() && !target.IsZombie)
            {
                if (R.IsReady() && R.Name == IsFirstR && W.IsReady() && E.IsReady() &&
                    _Player.Distance(target.Position) <= 250 + 70 + _Player.AttackRange)
                {
                    Player.CastSpell(SpellSlot.E, target.Position);
                   // CastYoumoo();
                    ForceR();
                    Utils.DelayAction(ForceW, 100);
                }
                else if (R.IsReady() && R.Name == IsFirstR && E.IsReady() && W.IsReady() && Q.IsReady() &&
                         _Player.Distance(target.Position) <= 400 + 70 + _Player.AttackRange)
                {
                    //CastYoumoo();
                    Player.CastSpell(SpellSlot.E, target.Position);
                    ForceR();
                    Utils.DelayAction(() => ForceCastQ(target), 150);
                    Utils.DelayAction(ForceW, 160);
                }
                else if (Flash.IsReady
                         && R.IsReady() && R.Name == IsFirstR && (_Player.Distance(target.Position) <= 800) &&
                         (!FirstHydra || HasHydra() == null || !HasHydra().IsReady() ))
                    
                {
                    Player.CastSpell(SpellSlot.E, target.Position);
                    //CastYoumoo();
                    ForceR();
                    Utils.DelayAction(FlashW, 180);
                }
                else if (Flash.IsReady
                         && R.IsReady() && E.IsReady() && W.IsReady() && R.Name == IsFirstR &&
                         (_Player.Distance(target.Position) <= 800) && FirstHydra && HasHydra() != null)
                {
                    Player.CastSpell(SpellSlot.E, target.Position);
                    ForceR();
                    Utils.DelayAction(ForceItem, 100);
                    Utils.DelayAction(FlashW, 210);
                }
            }
        }

        private static void FastHarass()
        {
            var target = TargetSelector.SelectedTarget;

            if (target == null || !target.IsValidTarget()) target = TargetSelector.GetTarget(450 + _Player.AttackRange + 70, DamageType.Physical);
            if (target == null || !target.IsValidTarget()) return;
            Orbwalker.ForcedTarget = target;
            Orbwalker.OrbwalkTo(target.ServerPosition);
            if (Q.IsReady() && E.IsReady())
            {
                
                if (!target.IsValidTarget() || target.IsZombie) return;
                if (!ObjectManager.Player.IsInAutoAttackRange(target) && !InWRange(target)) Player.CastSpell(SpellSlot.E, target.Position);
                Utils.DelayAction(ForceItem, 10);
                Utils.DelayAction(() => ForceCastQ(target), 170);
            }
        }

        private static void Harass()
        {
            var target = TargetSelector.GetTarget(400, DamageType.Physical);
            if (target == null || !target.IsValidTarget()) return;
            if (Q.IsReady() && W.IsReady() && E.IsReady() && QStack == 1)
            {
                if (target.IsValidTarget() && !target.IsZombie)
                {
                    ForceCastQ(target);
                    Utils.DelayAction(ForceW, 1);
                }
            }
            if (Q.IsReady() && E.IsReady() && QStack == 3 && !Orbwalker.CanAutoAttack && Orbwalker.CanMove)
            {
                var epos = _Player.ServerPosition +
                           (_Player.ServerPosition - target.ServerPosition).Normalized()*300;
                Player.CastSpell(SpellSlot.E, epos);
                Utils.DelayAction(() => Player.CastSpell(SpellSlot.Q, epos), 190);
            }
        }

        private static void Flee()
        {
            var enemy =
                EntityManager.Heroes.Enemies.Where(
                    hero =>
                        hero.IsValidTarget(WRange) && W.IsReady());
            var x = _Player.Position.Extend(Game.CursorPos, 300);
            if (W.IsReady() && enemy.Any()) W.Cast();
            if (Q.IsReady() && !_Player.IsDashing()) Player.CastSpell(SpellSlot.Q, Game.CursorPos);
            if (E.IsReady() && !_Player.IsDashing()) Player.CastSpell(SpellSlot.E, x.To3D());
        }

        private static void OnPlay(Obj_AI_Base sender, GameObjectPlayAnimationEventArgs args)
        {
            if (_Player.IsDead)
                return;
            if (!sender.IsMe) return;

            switch (args.Animation)
            {
                case "Spell1a":
                    LastQ = Environment.TickCount;
                    if (Qstrange && (Orbwalker.ActiveModesFlags != Orbwalker.ActiveModes.None))  Player.DoEmote(Emote.Dance);
                    QStack = 2;
                    if (BurstCombo || Orbwalker.ActiveModesFlags != Orbwalker.ActiveModes.None &&
                        Orbwalker.ActiveModesFlags != Orbwalker.ActiveModes.LastHit &&
                        Orbwalker.ActiveModesFlags != Orbwalker.ActiveModes.Flee)
                        Utils.DelayAction(Reset, (QD*10) + 1);
                    break;
                case "Spell1b":
                    LastQ = Environment.TickCount;
                    if (Qstrange && (Orbwalker.ActiveModesFlags != Orbwalker.ActiveModes.None)) Player.DoEmote(Emote.Dance);
                    QStack = 3;
                    if (BurstCombo || Orbwalker.ActiveModesFlags != Orbwalker.ActiveModes.None &&
                        Orbwalker.ActiveModesFlags != Orbwalker.ActiveModes.LastHit &&
                        Orbwalker.ActiveModesFlags != Orbwalker.ActiveModes.Flee)
                        Utils.DelayAction(Reset, (QD*10) + 1);
                    break;
                case "Spell1c":
                    LastQ = Environment.TickCount;
                    if (Qstrange && (Orbwalker.ActiveModesFlags != Orbwalker.ActiveModes.None )) Player.DoEmote(Emote.Dance);
                    QStack = 1;
                    if (BurstCombo || Orbwalker.ActiveModesFlags != Orbwalker.ActiveModes.None &&
                        Orbwalker.ActiveModesFlags != Orbwalker.ActiveModes.LastHit &&
                        Orbwalker.ActiveModesFlags != Orbwalker.ActiveModes.Flee)
                        Utils.DelayAction(Reset, (QLD*10) + 3);
                    break;
                case "Spell3":
                    if ((BurstCombo ||//Orbwalker.ActiveModesFlags == Orbwalker.ActiveModes.Burst ||
                         Orbwalker.ActiveModesFlags == Orbwalker.ActiveModes.Combo ||
                         //Orbwalker.ActiveModesFlags == Orbwalker.ActiveModes.FastHarass ||
                         Orbwalker.ActiveModesFlags == Orbwalker.ActiveModes.Flee) && Youmu) CastYoumoo();
                    break;
                case "Spell4a":
                    LastR = Environment.TickCount;
                    break;
                case "Spell4b":
                    var target = TargetSelector.SelectedTarget;

                    if (target == null || !target.IsValidTarget()) target = TargetSelector.GetTarget(450 + _Player.AttackRange + 70, DamageType.Physical);
                    if (target == null || !target.IsValidTarget()) return;
                    if (Q.IsReady() && target.IsValidTarget()) ForceCastQ(target);
                    break;
                case "Dance":
                    //var delay = 10;
                    //if (LastQ < Environment.TickCount)
                        Orbwalker.ResetAutoAttack();
                    break;
                //Chat.Print(args.Animation);
            }
        }

        private static void OnCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (_Player.IsDead)
                return;
            if (!sender.IsMe) return;

            if (args.SData.Name.Contains("ItemTiamatCleave")) forceItem = false;
            if (args.SData.Name.Contains("RivenTriCleave")) forceQ = false;
            if (args.SData.Name.Contains("RivenMartyr")) forceW = false;
            if (args.SData.Name == IsFirstR) forceR = false;
            if (args.SData.Name == IsSecondR) forceR2 = false;
        }

        private static void Reset()
        {
            
            Player.DoEmote(Emote.Dance);
            //Orbwalker.ResetAutoAttack();
        }

        private static bool InWRange(GameObject target)
        {
            if (target == null || !target.IsValid) return false;
            return (_Player.HasBuff("RivenFengShuiEngine"))
            ? 330 >= _Player.Distance(target.Position)
            : 265 >= _Player.Distance(target.Position);
            
        }


        private static void ForceSkill()
        {
            if (QTarget == null || !QTarget.IsValidTarget()) return;
            if (forceR && R.Name == IsFirstR)
            {
                //Chat.Print("trying to use R");
                Player.CastSpell(SpellSlot.R);
                return;
            }
            if (forceQ && QTarget != null && QTarget.IsValidTarget(E.Range + _Player.BoundingRadius + 70) && Q.IsReady())
                Player.CastSpell(SpellSlot.Q, ((Obj_AI_Base)QTarget).ServerPosition);
            if (forceW) W.Cast();
            
            var hydra = HasHydra();
            if (forceItem && hydra != null && hydra.IsReady()) hydra.Cast();
            if (forceR2 && R.Name == IsSecondR)
            {
                var target = TargetSelector.SelectedTarget;

                if (target == null || !target.IsValidTarget()) target = TargetSelector.GetTarget(450 + _Player.AttackRange + 70, DamageType.Physical);
                if (target == null || !target.IsValidTarget()) return;
                R.Cast(target);
            }
        }

        private static void CastR1(int delay = 0)
        {
            Chat.Print("Casting R1");
            
            Player.CastSpell(SpellSlot.R);
        }
        private static void ForceItem()
        {
            var hydra = HasHydra();
            if (hydra != null && hydra.IsReady()) forceItem = true;
            Utils.DelayAction(() => forceItem = false, 500);
        }

        private static void ForceR()
        {
            forceR = (R.IsReady() && R.Name == IsFirstR);
            //Chat.Print(forceR);
            Utils.DelayAction(() => forceR = false, 700);
        }

        private static void ForceR2()
        {
            forceR2 = R.IsReady() && R.Name == IsSecondR;
            Utils.DelayAction(() => forceR2 = false, 500);
        }

        private static void ForceW()
        {
            forceW = W.IsReady();
            Utils.DelayAction(() => forceW = false, 500);
        }

        private static void ForceCastQ(AttackableUnit target)
        {
            forceQ = true;
            QTarget = target;
        }


        private static void FlashW()
        {
            var target = TargetSelector.SelectedTarget;
            if (target != null && target.IsValidTarget() && !target.IsZombie)
            {
                W.Cast();
                Utils.DelayAction(() => _Player.Spellbook.CastSpell(Flash.Slot, target.Position), 10);
            }
        }


        private static void CastYoumoo()
        {
            var youmu = ObjectManager.Player.InventoryItems.FirstOrDefault(it => it.Id == ItemId.Youmuus_Ghostblade);
       
            if (youmu != null && youmu.CanUseItem()) youmu.Cast();
        }

        private static void OnCasting(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (!sender.IsEnemy || sender.Type != _Player.Type ||
                (!AutoShield && (!Shield || Orbwalker.ActiveModesFlags != Orbwalker.ActiveModes.LastHit))) return;
            var epos = _Player.ServerPosition +
                       (_Player.ServerPosition - sender.ServerPosition).Normalized()*300;

            if (!(_Player.Distance(sender.ServerPosition) <= args.SData.CastRange)) return;
            switch (args.SData.TargettingType)
            {
                case SpellDataTargetType.Unit:

                    if (args.Target.NetworkId == _Player.NetworkId)
                    {
                        if (Orbwalker.ActiveModesFlags == Orbwalker.ActiveModes.LastHit &&
                            !args.SData.Name.Contains("NasusW"))
                        {
                            if (E.IsReady()) Player.CastSpell(SpellSlot.E, epos);
                        }
                    }

                    break;
                case SpellDataTargetType.SelfAoe:

                    if (Orbwalker.ActiveModesFlags == Orbwalker.ActiveModes.LastHit)
                    {
                        if (E.IsReady()) Player.CastSpell(SpellSlot.E, epos);
                    }

                    break;
            }
            if (args.SData.Name.Contains("IreliaEquilibriumStrike"))
            {
                if (args.Target.NetworkId == _Player.NetworkId)
                {
                    if (W.IsReady() && InWRange(sender)) W.Cast();
                    else if (E.IsReady()) Player.CastSpell(SpellSlot.E, epos);
                }
            }
            if (args.SData.Name.Contains("TalonCutthroat"))
            {
                if (args.Target.NetworkId == _Player.NetworkId)
                {
                    if (W.IsReady()) W.Cast();
                }
            }
            if (args.SData.Name.Contains("RenektonPreExecute"))
            {
                if (args.Target.NetworkId == _Player.NetworkId)
                {
                    if (W.IsReady()) W.Cast();
                }
            }
            if (args.SData.Name.Contains("GarenRPreCast"))
            {
                if (args.Target.NetworkId == _Player.NetworkId)
                {
                    if (E.IsReady()) Player.CastSpell(SpellSlot.E, epos);
                }
            }
            if (args.SData.Name.Contains("GarenQAttack"))
            {
                if (args.Target.NetworkId == _Player.NetworkId)
                {
                    if (E.IsReady()) Player.CastSpell(SpellSlot.E, Game.CursorPos);
                }
            }
            if (args.SData.Name.Contains("XenZhaoThrust3"))
            {
                if (args.Target.NetworkId == _Player.NetworkId)
                {
                    if (W.IsReady()) W.Cast();
                }
            }
            if (args.SData.Name.Contains("RengarQ"))
            {
                if (args.Target.NetworkId == _Player.NetworkId)
                {
                    if (E.IsReady()) Player.CastSpell(SpellSlot.E, Game.CursorPos);
                }
            }
            if (args.SData.Name.Contains("RengarPassiveBuffDash"))
            {
                if (args.Target.NetworkId == _Player.NetworkId)
                {
                    if (E.IsReady()) Player.CastSpell(SpellSlot.E, Game.CursorPos);
                }
            }
            if (args.SData.Name.Contains("RengarPassiveBuffDashAADummy"))
            {
                if (args.Target.NetworkId == _Player.NetworkId)
                {
                    if (E.IsReady()) Player.CastSpell(SpellSlot.E, Game.CursorPos);
                }
            }
            if (args.SData.Name.Contains("TwitchEParticle"))
            {
                if (args.Target.NetworkId == _Player.NetworkId)
                {
                    if (E.IsReady()) Player.CastSpell(SpellSlot.E, Game.CursorPos);
                }
            }
            if (args.SData.Name.Contains("FizzPiercingStrike"))
            {
                if (args.Target.NetworkId == _Player.NetworkId)
                {
                    if (E.IsReady()) Player.CastSpell(SpellSlot.E, Game.CursorPos);
                }
            }
            if (args.SData.Name.Contains("HungeringStrike"))
            {
                if (args.Target.NetworkId == _Player.NetworkId)
                {
                    if (E.IsReady()) Player.CastSpell(SpellSlot.E, Game.CursorPos);
                }
            }
            if (args.SData.Name.Contains("YasuoDash"))
            {
                if (args.Target.NetworkId == _Player.NetworkId)
                {
                    if (E.IsReady()) Player.CastSpell(SpellSlot.E, Game.CursorPos);
                }
            }
            if (args.SData.Name.Contains("KatarinaRTrigger"))
            {
                if (args.Target.NetworkId == _Player.NetworkId)
                {
                    if (W.IsReady() && InWRange(sender)) W.Cast();
                    else if (E.IsReady()) Player.CastSpell(SpellSlot.E, Game.CursorPos);
                }
            }
            if (args.SData.Name.Contains("YasuoDash"))
            {
                if (args.Target.NetworkId == _Player.NetworkId)
                {
                    if (E.IsReady()) Player.CastSpell(SpellSlot.E, Game.CursorPos);
                }
            }
            if (args.SData.Name.Contains("KatarinaE"))
            {
                if (args.Target.NetworkId == _Player.NetworkId)
                {
                    if (W.IsReady()) W.Cast();
                }
            }
            if (args.SData.Name.Contains("MonkeyKingQAttack"))
            {
                if (args.Target.NetworkId == _Player.NetworkId)
                {
                    if (E.IsReady()) Player.CastSpell(SpellSlot.E, Game.CursorPos);
                }
            }
            if (args.SData.Name.Contains("MonkeyKingSpinToWin"))
            {
                if (args.Target.NetworkId == _Player.NetworkId)
                {
                    if (E.IsReady()) Player.CastSpell(SpellSlot.E, Game.CursorPos);
                    else if (W.IsReady()) W.Cast();
                }
            }
            if (args.SData.Name.Contains("MonkeyKingQAttack"))
            {
                if (args.Target.NetworkId == _Player.NetworkId)
                {
                    if (E.IsReady()) Player.CastSpell(SpellSlot.E, Game.CursorPos);
                }
            }
            if (args.SData.Name.Contains("MonkeyKingQAttack"))
            {
                if (args.Target.NetworkId == _Player.NetworkId)
                {
                    if (E.IsReady()) Player.CastSpell(SpellSlot.E, Game.CursorPos);
                }
            }
            if (args.SData.Name.Contains("MonkeyKingQAttack"))
            {
                if (args.Target.NetworkId == _Player.NetworkId)
                {
                    if (E.IsReady()) Player.CastSpell(SpellSlot.E, Game.CursorPos);
                }
            }
        }

        private static double basicdmg(Obj_AI_Base target)
        {
            if (target != null)
            {
                double dmg = 0;
                double passivenhan;
                if (_Player.Level >= 18)
                {
                    passivenhan = 0.5;
                }
                else if (_Player.Level >= 15)
                {
                    passivenhan = 0.45;
                }
                else if (_Player.Level >= 12)
                {
                    passivenhan = 0.4;
                }
                else if (_Player.Level >= 9)
                {
                    passivenhan = 0.35;
                }
                else if (_Player.Level >= 6)
                {
                    passivenhan = 0.3;
                }
                else if (_Player.Level >= 3)
                {
                    passivenhan = 0.25;
                }
                else
                {
                    passivenhan = 0.2;
                }
                if (HasHydra()!=null) dmg = dmg + _Player.GetAutoAttackDamage(target)*0.7;
                if (W.IsReady()) dmg = dmg + _Player.GetSpellDamage(target, SpellSlot.W);
                if (Q.IsReady())
                {
                    var qnhan = 4 - QStack;
                    
                    dmg = dmg + ObjectManager.Player.GetSpellDamage(target, SpellSlot.Q)*qnhan + _Player.GetAutoAttackDamage(target)*qnhan*(1 + passivenhan);
                }
                dmg = dmg + _Player.GetAutoAttackDamage(target)*(1 + passivenhan);
                return dmg;
            }
            return 0;
        }


        private static float getComboDamage(Obj_AI_Base enemy)
        {
            if (enemy != null)
            {
                float damage = 0;
                float passivenhan;
                if (_Player.Level >= 18)
                {
                    passivenhan = 0.5f;
                }
                else if (_Player.Level >= 15)
                {
                    passivenhan = 0.45f;
                }
                else if (_Player.Level >= 12)
                {
                    passivenhan = 0.4f;
                }
                else if (_Player.Level >= 9)
                {
                    passivenhan = 0.35f;
                }
                else if (_Player.Level >= 6)
                {
                    passivenhan = 0.3f;
                }
                else if (_Player.Level >= 3)
                {
                    passivenhan = 0.25f;
                }
                else
                {
                    passivenhan = 0.2f;
                }
                if (HasHydra() != null) damage = damage + _Player.GetAutoAttackDamage(enemy)*0.7f;
                if (W.IsReady()) damage = damage + ObjectManager.Player.GetSpellDamage(enemy, SpellSlot.W);
                if (Q.IsReady())
                {
                    var qnhan = 4 - QStack;
                    damage = damage + ObjectManager.Player.GetSpellDamage(enemy, SpellSlot.Q)*qnhan +
                             _Player.GetAutoAttackDamage(enemy)*qnhan*(1 + passivenhan);
                }
                damage = damage + _Player.GetAutoAttackDamage(enemy)*(1 + passivenhan);
                if (R.IsReady())
                {
                    return damage*1.2f + ObjectManager.Player.GetSpellDamage(enemy, SpellSlot.R);
                }

                return damage;
            }
            return 0;
        }

        public static bool IsKillableR(AIHeroClient target)
        {
            if (RKillable && target.IsValidTarget() && (totaldame(target) >= target.Health
                                                        && basicdmg(target) <= target.Health) ||
                _Player.CountEnemiesInRange(900) >= 2 &&
                (!target.HasBuff("kindrednodeathbuff") && !target.HasBuff("Undying Rage") &&
                 !target.HasBuff("JudicatorIntervention")))
            {
                return true;
            }
            return false;
        }

        private static double totaldame(Obj_AI_Base target)
        {
            if (target != null)
            {
                float dmg = 0;
                float passivenhan;
                if (_Player.Level >= 18)
                {
                    passivenhan = 0.5f;
                }
                else if (_Player.Level >= 15)
                {
                    passivenhan = 0.45f;
                }
                else if (_Player.Level >= 12)
                {
                    passivenhan = 0.4f;
                }
                else if (_Player.Level >= 9)
                {
                    passivenhan = 0.35f;
                }
                else if (_Player.Level >= 6)
                {
                    passivenhan = 0.3f;
                }
                else if (_Player.Level >= 3)
                {
                    passivenhan = 0.25f;
                }
                else
                {
                    passivenhan = 0.2f;
                }
                if (HasHydra() != null) dmg = dmg + _Player.GetAutoAttackDamage(target)*0.7f;
                if (W.IsReady()) dmg = dmg + _Player.GetSpellDamage(target, SpellSlot.W);
                if (Q.IsReady())
                {
                    var qnhan = 4 - QStack;
                    dmg = dmg + ObjectManager.Player.GetSpellDamage(target, SpellSlot.Q)*qnhan + _Player.GetAutoAttackDamage(target)*qnhan*(1 + passivenhan);
                }
                dmg = dmg + _Player.GetAutoAttackDamage(target)*(1 + passivenhan);
                if (R.IsReady())
                {
                    var rdmg = Rdame(target, target.Health - dmg*1.2f);
                    return dmg*1.2 + rdmg;
                }
                return dmg;
            }
            return 0;
        }

        private static double Rdame(Obj_AI_Base target, float health)
        {
            if (target != null)
            {
                float missinghealth = (target.MaxHealth - health)/target.MaxHealth > 0.75f
                    ? 0.75f
                    : (target.MaxHealth - health)/target.MaxHealth;
                float pluspercent = missinghealth * (2.666667F); // 8/3
                float rawdmg = new float[] {80, 120, 160}[R.Level - 1] + 0.6f*_Player.FlatPhysicalDamageMod;
                return ObjectManager.Player.CalculateDamageOnUnit(target, DamageType.Physical, rawdmg*(1 + pluspercent));
            }
            return 0;
        }
    }
}