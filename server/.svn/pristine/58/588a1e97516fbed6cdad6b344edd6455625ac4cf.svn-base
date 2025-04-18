using System;
using Server;
using Server.Gumps;
using Server.Mobiles;
using Server.Targeting;
using Server.Scripts;

namespace Server.SkillHandlers
{
    public class AnimalLore
    {
        public static void Initialize()
        {
            SkillInfo.Table[(int)SkillName.AnimalLore].Callback = new SkillUseCallback(OnUse);
        }

        public static TimeSpan OnUse(Mobile m)
        {
            m.Target = new InternalTarget();

            m.SendLocalizedMessage(500328); // What animal should I look at?

            return TimeSpan.FromSeconds(1.0);
        }

        private class InternalTarget : Target
        {
            public InternalTarget()
                : base(8, false, TargetFlags.None)
            {
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (!from.Alive)
                {
                    from.SendLocalizedMessage(500331); // The spirits of the dead are not the province of animal lore.
                }

                else if (targeted is BaseCreature)
                {
                    BaseCreature c = (BaseCreature)targeted;

                    if (!c.IsDeadPet && c != null)
                    {
                        //Failure
                        if (!from.CheckTargetSkill(SkillName.AnimalLore, c, 0.0, 100.0))
                        {
                            from.SendLocalizedMessage(500334); // You can't think of anything you know offhand.
                        }

                        //Success
                        else
                        {
                            from.CloseGump(typeof(AnimalLoreGump));
                            from.SendGump(new AnimalLoreGump(c));
                        }
                    }

                    else
                    {
                        from.SendLocalizedMessage(500331); // The spirits of the dead are not the province of animal lore.
                    }
                }

                else
                {
                    from.SendLocalizedMessage(500329); // That's not an animal!
                }
            }
        }
    }

    public class AnimalLoreGump : Gump
    {
        private static string FormatSkill(BaseCreature c, SkillName name)
        {
            Skill skill = c.Skills[name];

            if (skill.Base < 10.0)
                return "<div align=right>---</div>";

            return String.Format("<div align=right>{0:F1}</div>", skill.Value);
        }

        private static string FormatAttributes(int cur, int max)
        {
            if (max == 0)
                return "<div align=right>---</div>";

            return String.Format("<div align=right>{0}/{1}</div>", cur, max);
        }

        private static string FormatStat(int val)
        {
            if (val == 0)
                return "<div align=right>---</div>";

            return String.Format("<div align=right>{0}</div>", val);
        }

        private static string FormatDouble(double val)
        {
            if (val == 0)
                return "<div align=right>---</div>";

            return String.Format("<div align=right>{0:F1}</div>", val);
        }

        private static string FormatElement(int val)
        {
            if (val <= 0)
                return "<div align=right>---</div>";

            return String.Format("<div align=right>{0}%</div>", val);
        }

        #region Mondain's Legacy
        private static string FormatDamage(int min, int max)
        {
            if (min <= 0 || max <= 0)
                return "<div align=right>---</div>";

            return String.Format("<div align=right>{0}-{1}</div>", min, max);
        }
        #endregion

        private const int LabelColor = 0x24E5;

        public AnimalLoreGump(BaseCreature c)
            : base(250, 50)
        {
            AddPage(0);

            AddImage(100, 100, 2080);
            AddImage(118, 137, 2081);
            AddImage(118, 207, 2081);
            AddImage(118, 277, 2081);
            AddImage(118, 347, 2083);

            AddHtml(147, 108, 210, 18, String.Format("<center><i>{0}</i></center>", c.Name), false, false);

            AddButton(240, 77, 2093, 2093, 2, GumpButtonType.Reply, 0);

            AddImage(140, 138, 2091);
            AddImage(140, 335, 2091);

            int pages = (Core.AOS ? 5 : 3);
            int page = 0;


            #region Attributes
            AddPage(++page);

            AddHtmlLocalized(153, 150, 160, 18, 1049578, LabelColor, false, false); // Hits
            AddHtml(298, 150, 75, 25, FormatAttributes(c.Hits, c.HitsMax), false, false);

            AddHtmlLocalized(153, 168, 160, 18, 1049579, LabelColor, false, false); // Stamina
            AddHtml(298, 168, 75, 25, FormatAttributes(c.Stam, c.StamMax), false, false);

            AddHtmlLocalized(153, 186, 160, 18, 1049580, LabelColor, false, false); // Mana
            AddHtml(298, 186, 75, 25, FormatAttributes(c.Mana, c.ManaMax), false, false);

            AddHtmlLocalized(153, 204, 222, 18, 1061155, LabelColor, false, false); // Damage
            AddHtml(320, 204, 50, 25, FormatDamage(c.DamageMin, c.DamageMax), false, false);

            AddHtmlLocalized(153, 222, 160, 18, 1044103, LabelColor, false, false); // Wrestling
            AddHtml(320, 222, 50, 25, FormatSkill(c, SkillName.Wrestling), false, false);

            AddHtmlLocalized(153, 240, 160, 18, 1049581, LabelColor, false, false); // Armor Rating
            AddHtml(320, 240, 50, 25, FormatStat(c.VirtualArmor), false, false);

            AddHtmlLocalized(153, 258, 160, 18, 1044086, LabelColor, false, false); // Magic Resistance
            AddHtml(320, 258, 50, 25, FormatSkill(c, SkillName.MagicResist), false, false);

            //Only On Tameable Creatures
            if (c.Tamable)
            {
                AddHtmlLocalized(153, 276, 160, 18, 1044095, LabelColor, false, false); // Animal Taming
                AddHtml(320, 276, 50, 25, FormatDouble(c.MinTameSkill), false, false);

                AddHtmlLocalized(153, 294, 160, 18, 1115783, LabelColor, false, false); // Pet Slots
                AddHtml(320, 294, 50, 25, FormatStat(c.ControlSlots), false, false);
            }

            AddHtmlLocalized(153, 312, 160, 18, 1070793, LabelColor, false, false); // Barding Difficulty

            int difficultyScaled = (int)(Math.Round((GetMobileDifficulty.GetDifficultyValue(c) / FeatureList.BardChanges.MaxProvocationDifficulty) * 100));
            AddHtml(320, 312, 50, 25, FormatStat(difficultyScaled), false, false);

            AddButton(340, 358, 5601, 5605, 0, GumpButtonType.Page, page + 1);
            AddButton(317, 358, 5603, 5607, 0, GumpButtonType.Page, pages);
            #endregion

            #region Skills
            AddPage(++page);

            String poisonType = "---";
            
            if (c.PoisonCustomHit != null)
            {
                poisonType = c.PoisonCustomHit.ToString();
            }
            else if (c.HitPoison != null)
            {
                poisonType = c.HitPoison.ToString();
            }
            
            AddHtmlLocalized(153, 150, 160, 40, 1015018, LabelColor, false, false); // Poison
            AddHtml(320, 150, 50, 25, String.Format("<div align=right>{0}</div>", poisonType), false, false);
           
            AddHtmlLocalized(153, 168, 160, 18, 1044085, LabelColor, false, false); // Magery
            AddHtml(320, 168, 50, 25, FormatSkill(c, SkillName.Magery), false, false);

            AddHtmlLocalized(153, 186, 160, 18, 1044076, LabelColor, false, false); // Evaluating Intelligence
            AddHtml(320, 186, 50, 25, FormatSkill(c, SkillName.EvalInt), false, false);            

            //If Tameable
            if (c.Tamable)
            {
                AddImage(128, 204, 2086);
                AddHtmlLocalized(147, 204, 160, 18, 1049563, 200, false, false); // Preferred Foods

                int foodPref = 3000340;

                if ((c.FavoriteFood & FoodType.FruitsAndVegies) != 0)
                    foodPref = 1049565; // Fruits and Vegetables
                else if ((c.FavoriteFood & FoodType.GrainsAndHay) != 0)
                    foodPref = 1049566; // Grains and Hay
                else if ((c.FavoriteFood & FoodType.Fish) != 0)
                    foodPref = 1049568; // Fish
                else if ((c.FavoriteFood & FoodType.Meat) != 0)
                    foodPref = 1049564; // Meat
                else if ((c.FavoriteFood & FoodType.Eggs) != 0)
                    foodPref = 1044477; // Eggs

                AddHtmlLocalized(153, 222, 160, 18, foodPref, LabelColor, false, false);

                AddHtmlLocalized(147, 240, 160, 18, 1049594, 200, false, false); // Loyalty Rating
                AddHtmlLocalized(153, 256, 160, 18, (!c.Controlled || c.Loyalty == 0) ? 1061643 : 1049595 + (c.Loyalty / 10), LabelColor, false, false);
            }

            AddButton(340, 358, 5601, 5605, 0, GumpButtonType.Page, page + 1);
            AddButton(317, 358, 5603, 5607, 0, GumpButtonType.Page, page - 1);
            #endregion
        }
    }
}