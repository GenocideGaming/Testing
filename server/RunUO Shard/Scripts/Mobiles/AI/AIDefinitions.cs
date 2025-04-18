using System;
using System.Collections;
using Server;
using Server.Mobiles;
using Server.Factions;
using Server.Scripts;

namespace Server
{
    public class AIDefinitions
    {
        public AIDefinitions()
        {
        }

        public void UpdateAI(BaseCreature target)
        {
            //BaseAI AIGroup
            switch (target.Group)
            {
                case AIGroup.MonsterGeneric:
                    target.DictCombatTargeting[CombatTargeting.PlayerAny] = 1;

                    target.DictCombatFlee[CombatFlee.Flee25] = 1;
                    target.DictCombatFlee[CombatFlee.Flee10] = 2;
                    break;

                case AIGroup.EvilHuman:
                    target.DictCombatTargeting[CombatTargeting.PlayerAny] = 1;

                    target.DictCombatFlee[CombatFlee.Flee25] = 1;
                    target.DictCombatFlee[CombatFlee.Flee10] = 2;
                    break;

                case AIGroup.NeutralHuman:
                    target.DictCombatFlee[CombatFlee.Flee25] = 1;
                    target.DictCombatFlee[CombatFlee.Flee10] = 2;
                    break;

                case AIGroup.GoodHuman:
                    target.DictCombatTargeting[CombatTargeting.PlayerCriminal] = 1;

                    target.DictCombatFlee[CombatFlee.Flee25] = 1;
                    target.DictCombatFlee[CombatFlee.Flee10] = 2;
                    break;

                case AIGroup.FactionHuman:
                    target.DictCombatTargeting[CombatTargeting.OpposingFaction] = 1;
                    break;

                case AIGroup.Undead:
                    target.DictCombatTargeting[CombatTargeting.PlayerAny] = 1;

                    target.DictCombatFlee[CombatFlee.Flee75] = 0;
                    target.DictCombatFlee[CombatFlee.Flee50] = 0;
                    target.DictCombatFlee[CombatFlee.Flee25] = 0;
                    target.DictCombatFlee[CombatFlee.Flee10] = 0;
                    break;

                case AIGroup.Animal:
                    target.DictCombatFlee[CombatFlee.Flee25] = 2;
                    target.DictCombatFlee[CombatFlee.Flee10] = 3;
                    break;

                case AIGroup.EvilAnimal:
                    target.DictCombatTargeting[CombatTargeting.PlayerAny] = 1;

                    target.DictCombatFlee[CombatFlee.Flee25] = 1;
                    target.DictCombatFlee[CombatFlee.Flee10] = 2;

                    break;

                case AIGroup.Summoned:
                    break;

                case AIGroup.Boss:
                    target.DictCombatTargeting[CombatTargeting.PlayerAny] = 1;
                    break;

                case AIGroup.None:
                    break;
            }

            //BaseAI AISubgroup
            switch (target.Subgroup)
            {
                case AISubgroup.MeleeMage1:
                    target.DictCombatRange[CombatRange.WeaponAttackRange] = 1;

                    target.DictCombatAction[CombatAction.AttackOnly] = 1;
                    target.DictCombatAction[CombatAction.CombatSpell] = 1;

                    target.DictCombatSpell[CombatSpell.SpellDamage1] = 2;
                    target.DictCombatSpell[CombatSpell.SpellDamage2] = 3;
                    target.DictCombatSpell[CombatSpell.SpellDamage3] = 4;
                    target.DictCombatSpell[CombatSpell.SpellDamage4] = 2;
                    target.DictCombatSpell[CombatSpell.SpellDamage5] = 1;
                    target.DictCombatSpell[CombatSpell.SpellNegative1to3] = 1;
                    //target.DictCombatSpell[CombatSpell.SpellDispelSummon] = 1;
                    break;

                case AISubgroup.MeleeMage2:
                    target.DictCombatRange[CombatRange.WeaponAttackRange] = 1;

                    target.DictCombatAction[CombatAction.AttackOnly] = 1;
                    target.DictCombatAction[CombatAction.CombatSpell] = 2;

                    target.DictCombatSpell[CombatSpell.SpellDamage1] = 1;
                    target.DictCombatSpell[CombatSpell.SpellDamage2] = 3;
                    target.DictCombatSpell[CombatSpell.SpellDamage3] = 4;
                    target.DictCombatSpell[CombatSpell.SpellDamage4] = 5;
                    target.DictCombatSpell[CombatSpell.SpellDamage5] = 3;
                    target.DictCombatSpell[CombatSpell.SpellDamage6] = 1;
                    target.DictCombatSpell[CombatSpell.SpellNegative1to3] = 1;
                    //target.DictCombatSpell[CombatSpell.SpellDispelSummon] = 2;
                    break;

                case AISubgroup.MeleeMage3:
                    target.DictCombatRange[CombatRange.WeaponAttackRange] = 1;

                    target.DictCombatAction[CombatAction.AttackOnly] = 1;
                    target.DictCombatAction[CombatAction.CombatSpell] = 3;

                    target.DictCombatSpell[CombatSpell.SpellDamage1] = 1;
                    target.DictCombatSpell[CombatSpell.SpellDamage2] = 2;
                    target.DictCombatSpell[CombatSpell.SpellDamage3] = 4;
                    target.DictCombatSpell[CombatSpell.SpellDamage4] = 6;
                    target.DictCombatSpell[CombatSpell.SpellDamage5] = 5;
                    target.DictCombatSpell[CombatSpell.SpellDamage6] = 3;
                    target.DictCombatSpell[CombatSpell.SpellDamage7] = 1;
                    target.DictCombatSpell[CombatSpell.SpellPoison] = 1;
                    target.DictCombatSpell[CombatSpell.SpellNegative1to3] = 1;
                    //target.DictCombatSpell[CombatSpell.SpellDispelSummon] = 3;
                    break;

                case AISubgroup.MeleeMage4:
                    target.DictCombatRange[CombatRange.WeaponAttackRange] = 1;

                    target.DictCombatAction[CombatAction.AttackOnly] = 1;
                    target.DictCombatAction[CombatAction.CombatSpell] = 10;
                    target.DictCombatAction[CombatAction.CombatHealSelf] = 1;

                    target.DictCombatSpell[CombatSpell.SpellDamage1] = 1;
                    target.DictCombatSpell[CombatSpell.SpellDamage2] = 1;
                    target.DictCombatSpell[CombatSpell.SpellDamage3] = 2;
                    target.DictCombatSpell[CombatSpell.SpellDamage4] = 3;
                    target.DictCombatSpell[CombatSpell.SpellDamage5] = 4;
                    target.DictCombatSpell[CombatSpell.SpellDamage6] = 5;
                    target.DictCombatSpell[CombatSpell.SpellDamage7] = 3;
                    target.DictCombatSpell[CombatSpell.SpellDamageAOE7] = 1;

                    target.DictCombatSpell[CombatSpell.SpellPoison] = 2;
                    target.DictCombatSpell[CombatSpell.SpellNegative4to7] = 1;
                    //target.DictCombatSpell[CombatSpell.SpellDispelSummon] = 4;
                    target.DictCombatSpell[CombatSpell.SpellBeneficial5] = 1;

                    target.DictCombatHealSelf[CombatHealSelf.SpellHealSelf50] = 1;
                    target.DictCombatHealSelf[CombatHealSelf.SpellCureSelf] = 1;
                    break;

                case AISubgroup.MeleeMage5:
                    target.DictCombatRange[CombatRange.WeaponAttackRange] = 1;

                    target.DictCombatAction[CombatAction.AttackOnly] = 1;
                    target.DictCombatAction[CombatAction.CombatSpell] = 15;
                    target.DictCombatAction[CombatAction.CombatHealSelf] = 2;

                    target.DictCombatSpell[CombatSpell.SpellDamage1] = 0;
                    target.DictCombatSpell[CombatSpell.SpellDamage2] = 0;
                    target.DictCombatSpell[CombatSpell.SpellDamage3] = 1;
                    target.DictCombatSpell[CombatSpell.SpellDamage4] = 2;
                    target.DictCombatSpell[CombatSpell.SpellDamage5] = 3;
                    target.DictCombatSpell[CombatSpell.SpellDamage6] = 6;
                    target.DictCombatSpell[CombatSpell.SpellDamage7] = 6;
                    target.DictCombatSpell[CombatSpell.SpellDamageAOE7] = 2;

                    target.DictCombatSpell[CombatSpell.SpellPoison] = 3;
                    target.DictCombatSpell[CombatSpell.SpellNegative4to7] = 1;
                    //target.DictCombatSpell[CombatSpell.SpellDispelSummon] = 5;
                    target.DictCombatSpell[CombatSpell.SpellBeneficial5] = 1;

                    target.DictCombatHealSelf[CombatHealSelf.SpellHealSelf50] = 1;
                    target.DictCombatHealSelf[CombatHealSelf.SpellCureSelf] = 1;
                    break;

                case AISubgroup.Mage1:
                    target.DictCombatRange[CombatRange.WeaponAttackRange] = 0;
                    target.DictCombatRange[CombatRange.SpellRange] = 5;
                    target.DictCombatRange[CombatRange.Withdraw] = 1;

                    target.DictCombatAction[CombatAction.AttackOnly] = 1;
                    target.DictCombatAction[CombatAction.CombatSpell] = 1;

                    target.DictCombatSpell[CombatSpell.SpellDamage1] = 2;
                    target.DictCombatSpell[CombatSpell.SpellDamage2] = 3;
                    target.DictCombatSpell[CombatSpell.SpellDamage3] = 4;
                    target.DictCombatSpell[CombatSpell.SpellDamage4] = 2;
                    target.DictCombatSpell[CombatSpell.SpellDamage5] = 1;
                    target.DictCombatSpell[CombatSpell.SpellNegative1to3] = 1;
                    //target.DictCombatSpell[CombatSpell.SpellDispelSummon] = 1;
                    break;

                case AISubgroup.Mage2:
                    target.DictCombatRange[CombatRange.WeaponAttackRange] = 0;
                    target.DictCombatRange[CombatRange.SpellRange] = 5;
                    target.DictCombatRange[CombatRange.Withdraw] = 1;

                    target.DictCombatAction[CombatAction.AttackOnly] = 1;
                    target.DictCombatAction[CombatAction.CombatSpell] = 2;

                    target.DictCombatSpell[CombatSpell.SpellDamage1] = 1;
                    target.DictCombatSpell[CombatSpell.SpellDamage2] = 3;
                    target.DictCombatSpell[CombatSpell.SpellDamage3] = 4;
                    target.DictCombatSpell[CombatSpell.SpellDamage4] = 5;
                    target.DictCombatSpell[CombatSpell.SpellDamage5] = 3;
                    target.DictCombatSpell[CombatSpell.SpellDamage6] = 1;
                    target.DictCombatSpell[CombatSpell.SpellNegative1to3] = 1;
                    //target.DictCombatSpell[CombatSpell.SpellDispelSummon] = 2;
                    break;

                case AISubgroup.Mage3:
                    target.DictCombatRange[CombatRange.WeaponAttackRange] = 0;
                    target.DictCombatRange[CombatRange.SpellRange] = 5;
                    target.DictCombatRange[CombatRange.Withdraw] = 1;

                    target.DictCombatAction[CombatAction.AttackOnly] = 1;
                    target.DictCombatAction[CombatAction.CombatSpell] = 3;

                    target.DictCombatSpell[CombatSpell.SpellDamage1] = 1;
                    target.DictCombatSpell[CombatSpell.SpellDamage2] = 2;
                    target.DictCombatSpell[CombatSpell.SpellDamage3] = 4;
                    target.DictCombatSpell[CombatSpell.SpellDamage4] = 6;
                    target.DictCombatSpell[CombatSpell.SpellDamage5] = 5;
                    target.DictCombatSpell[CombatSpell.SpellDamage6] = 3;
                    target.DictCombatSpell[CombatSpell.SpellDamage7] = 1;
                    target.DictCombatSpell[CombatSpell.SpellPoison] = 1;
                    target.DictCombatSpell[CombatSpell.SpellNegative1to3] = 1;
                    //target.DictCombatSpell[CombatSpell.SpellDispelSummon] = 3;
                    break;

                case AISubgroup.Mage4:
                    target.DictCombatRange[CombatRange.WeaponAttackRange] = 0;
                    target.DictCombatRange[CombatRange.SpellRange] = 5;
                    target.DictCombatRange[CombatRange.Withdraw] = 1;

                    target.DictCombatAction[CombatAction.AttackOnly] = 1;
                    target.DictCombatAction[CombatAction.CombatSpell] = 10;
                    target.DictCombatAction[CombatAction.CombatHealSelf] = 1;

                    target.DictCombatSpell[CombatSpell.SpellDamageAOE7] = 1;
                    
                    target.DictCombatSpell[CombatSpell.SpellDamage1] = 1;
                    target.DictCombatSpell[CombatSpell.SpellDamage2] = 1;
                    target.DictCombatSpell[CombatSpell.SpellDamage3] = 2;
                    target.DictCombatSpell[CombatSpell.SpellDamage4] = 3;
                    target.DictCombatSpell[CombatSpell.SpellDamage5] = 4;
                    target.DictCombatSpell[CombatSpell.SpellDamage6] = 5;
                    target.DictCombatSpell[CombatSpell.SpellDamage7] = 3;
                    target.DictCombatSpell[CombatSpell.SpellDamageAOE7] = 1;

                    target.DictCombatSpell[CombatSpell.SpellPoison] = 2;
                    target.DictCombatSpell[CombatSpell.SpellNegative4to7] = 1;
                    target.DictCombatSpell[CombatSpell.SpellBeneficial5] = 1;
                    //target.DictCombatSpell[CombatSpell.SpellDispelSummon] = 4;                    

                    target.DictCombatHealSelf[CombatHealSelf.SpellHealSelf50] = 1;
                    target.DictCombatHealSelf[CombatHealSelf.SpellCureSelf] = 1;

                    target.DictWanderAction[WanderAction.None] = 10;
                    target.DictWanderAction[WanderAction.SpellHealSelf100] = 1;
                    target.DictWanderAction[WanderAction.SpellCureSelf] = 1;
                    break;

                case AISubgroup.Mage5:
                    target.DictCombatRange[CombatRange.WeaponAttackRange] = 0;
                    target.DictCombatRange[CombatRange.SpellRange] = 5;
                    target.DictCombatRange[CombatRange.Withdraw] = 1;

                    target.DictCombatAction[CombatAction.AttackOnly] = 1;
                    target.DictCombatAction[CombatAction.CombatSpell] = 15;
                    target.DictCombatAction[CombatAction.CombatHealSelf] = 2;

                    target.DictCombatSpell[CombatSpell.SpellDamage1] = 0;
                    target.DictCombatSpell[CombatSpell.SpellDamage2] = 0;
                    target.DictCombatSpell[CombatSpell.SpellDamage3] = 1;
                    target.DictCombatSpell[CombatSpell.SpellDamage4] = 2;
                    target.DictCombatSpell[CombatSpell.SpellDamage5] = 3;
                    target.DictCombatSpell[CombatSpell.SpellDamage6] = 6;
                    target.DictCombatSpell[CombatSpell.SpellDamage7] = 6;
                    target.DictCombatSpell[CombatSpell.SpellDamageAOE7] = 2;

                    target.DictCombatSpell[CombatSpell.SpellPoison] = 3;
                    target.DictCombatSpell[CombatSpell.SpellNegative1to3] = 0;
                    target.DictCombatSpell[CombatSpell.SpellNegative4to7] = 1;
                    //target.DictCombatSpell[CombatSpell.SpellDispelSummon] = 5;
                    target.DictCombatSpell[CombatSpell.SpellBeneficial5] = 1;

                    target.DictCombatHealSelf[CombatHealSelf.SpellHealSelf50] = 1;
                    target.DictCombatHealSelf[CombatHealSelf.SpellCureSelf] = 1;

                    target.DictWanderAction[WanderAction.None] = 10;
                    target.DictWanderAction[WanderAction.SpellHealSelf100] = 1;
                    target.DictWanderAction[WanderAction.SpellCureSelf] = 1;
                    break;

                case AISubgroup.GroupHealerMage1:
                    target.DictCombatRange[CombatRange.WeaponAttackRange] = 0;
                    target.DictCombatRange[CombatRange.SpellRange] = 5;
                    target.DictCombatRange[CombatRange.Withdraw] = 1;

                    target.DictCombatAction[CombatAction.AttackOnly] = 2;
                    target.DictCombatAction[CombatAction.CombatSpell] = 2;
                    target.DictCombatAction[CombatAction.CombatHealOther] = 4;
                    target.DictCombatAction[CombatAction.CombatHealSelf] = 2;

                    target.DictCombatSpell[CombatSpell.SpellDamage1] = 2;
                    target.DictCombatSpell[CombatSpell.SpellDamage2] = 3;
                    target.DictCombatSpell[CombatSpell.SpellDamage3] = 4;
                    target.DictCombatSpell[CombatSpell.SpellDamage4] = 2;
                    target.DictCombatSpell[CombatSpell.SpellDamage5] = 1;
                    target.DictCombatSpell[CombatSpell.SpellNegative1to3] = 1;
                    //target.DictCombatSpell[CombatSpell.SpellDispelSummon] = 1;

                    target.DictCombatHealOther[CombatHealOther.SpellHealOther75] = 0;
                    target.DictCombatHealOther[CombatHealOther.SpellHealOther50] = 0;
                    target.DictCombatHealOther[CombatHealOther.SpellHealOther25] = 1;
                    target.DictCombatHealOther[CombatHealOther.SpellCureOther] = 1;

                    target.DictCombatHealSelf[CombatHealSelf.SpellHealSelf25] = 1;
                    target.DictCombatHealSelf[CombatHealSelf.SpellCureSelf] = 1;

                    target.DictWanderAction[WanderAction.None] = 10;
                    target.DictWanderAction[WanderAction.SpellHealSelf100] = 1;
                    target.DictWanderAction[WanderAction.SpellCureSelf] = 1;
                    target.DictWanderAction[WanderAction.SpellHealOther100] = 1;
                    target.DictWanderAction[WanderAction.SpellCureOther] = 1;
                    break;

                case AISubgroup.GroupHealerMage2:
                    target.DictCombatRange[CombatRange.WeaponAttackRange] = 0;
                    target.DictCombatRange[CombatRange.SpellRange] = 5;
                    target.DictCombatRange[CombatRange.Withdraw] = 1;

                    target.DictCombatAction[CombatAction.AttackOnly] = 2;
                    target.DictCombatAction[CombatAction.CombatSpell] = 2;
                    target.DictCombatAction[CombatAction.CombatHealOther] = 4;
                    target.DictCombatAction[CombatAction.CombatHealSelf] = 2;

                    target.DictCombatSpell[CombatSpell.SpellDamage1] = 1;
                    target.DictCombatSpell[CombatSpell.SpellDamage2] = 3;
                    target.DictCombatSpell[CombatSpell.SpellDamage3] = 4;
                    target.DictCombatSpell[CombatSpell.SpellDamage4] = 5;
                    target.DictCombatSpell[CombatSpell.SpellDamage5] = 3;
                    target.DictCombatSpell[CombatSpell.SpellDamage6] = 1;
                    target.DictCombatSpell[CombatSpell.SpellNegative1to3] = 1;
                    //target.DictCombatSpell[CombatSpell.SpellDispelSummon] = 2;

                    target.DictCombatHealOther[CombatHealOther.SpellHealOther75] = 0;
                    target.DictCombatHealOther[CombatHealOther.SpellHealOther50] = 1;
                    target.DictCombatHealOther[CombatHealOther.SpellHealOther25] = 2;
                    target.DictCombatHealOther[CombatHealOther.SpellCureOther] = 1;

                    target.DictCombatHealSelf[CombatHealSelf.SpellHealSelf50] = 1;
                    target.DictCombatHealSelf[CombatHealSelf.SpellHealSelf25] = 2;
                    target.DictCombatHealSelf[CombatHealSelf.SpellCureSelf] = 1;

                    target.DictWanderAction[WanderAction.None] = 10;
                    target.DictWanderAction[WanderAction.SpellHealSelf100] = 1;
                    target.DictWanderAction[WanderAction.SpellCureSelf] = 1;
                    target.DictWanderAction[WanderAction.SpellHealOther100] = 1;
                    target.DictWanderAction[WanderAction.SpellCureOther] = 1;
                    break;

                case AISubgroup.GroupHealerMage3:
                    target.DictCombatRange[CombatRange.WeaponAttackRange] = 0;
                    target.DictCombatRange[CombatRange.SpellRange] = 5;
                    target.DictCombatRange[CombatRange.Withdraw] = 1;

                    target.DictCombatAction[CombatAction.AttackOnly] = 2;
                    target.DictCombatAction[CombatAction.CombatSpell] = 2;
                    target.DictCombatAction[CombatAction.CombatHealOther] = 4;
                    target.DictCombatAction[CombatAction.CombatHealSelf] = 2;

                    target.DictCombatSpell[CombatSpell.SpellDamage1] = 1;
                    target.DictCombatSpell[CombatSpell.SpellDamage2] = 2;
                    target.DictCombatSpell[CombatSpell.SpellDamage3] = 4;
                    target.DictCombatSpell[CombatSpell.SpellDamage4] = 6;
                    target.DictCombatSpell[CombatSpell.SpellDamage5] = 5;
                    target.DictCombatSpell[CombatSpell.SpellDamage6] = 3;
                    target.DictCombatSpell[CombatSpell.SpellDamage7] = 1;
                    target.DictCombatSpell[CombatSpell.SpellPoison] = 1;
                    target.DictCombatSpell[CombatSpell.SpellNegative1to3] = 1;
                    //target.DictCombatSpell[CombatSpell.SpellDispelSummon] = 3;

                    target.DictCombatHealOther[CombatHealOther.SpellHealOther75] = 1;
                    target.DictCombatHealOther[CombatHealOther.SpellHealOther50] = 2;
                    target.DictCombatHealOther[CombatHealOther.SpellHealOther25] = 3;
                    target.DictCombatHealOther[CombatHealOther.SpellCureOther] = 1;

                    target.DictCombatHealSelf[CombatHealSelf.SpellHealSelf75] = 1;
                    target.DictCombatHealSelf[CombatHealSelf.SpellHealSelf50] = 2;
                    target.DictCombatHealSelf[CombatHealSelf.SpellHealSelf25] = 3;
                    target.DictCombatHealSelf[CombatHealSelf.SpellCureSelf] = 1;

                    target.DictWanderAction[WanderAction.None] = 10;
                    target.DictWanderAction[WanderAction.SpellHealSelf100] = 1;
                    target.DictWanderAction[WanderAction.SpellCureSelf] = 1;
                    target.DictWanderAction[WanderAction.SpellHealOther100] = 1;
                    target.DictWanderAction[WanderAction.SpellCureOther] = 1;
                    break;

                case AISubgroup.GroupHealerMage4:
                    target.DictCombatRange[CombatRange.WeaponAttackRange] = 0;
                    target.DictCombatRange[CombatRange.SpellRange] = 5;
                    target.DictCombatRange[CombatRange.Withdraw] = 1;

                    target.DictCombatAction[CombatAction.AttackOnly] = 2;
                    target.DictCombatAction[CombatAction.CombatSpell] = 2;
                    target.DictCombatAction[CombatAction.CombatHealOther] = 4;
                    target.DictCombatAction[CombatAction.CombatHealSelf] = 2;

                    target.DictCombatSpell[CombatSpell.SpellDamage1] = 1;
                    target.DictCombatSpell[CombatSpell.SpellDamage2] = 1;
                    target.DictCombatSpell[CombatSpell.SpellDamage3] = 2;
                    target.DictCombatSpell[CombatSpell.SpellDamage4] = 3;
                    target.DictCombatSpell[CombatSpell.SpellDamage5] = 4;
                    target.DictCombatSpell[CombatSpell.SpellDamage6] = 5;
                    target.DictCombatSpell[CombatSpell.SpellDamage7] = 3;
                    target.DictCombatSpell[CombatSpell.SpellDamageAOE7] = 1;

                    target.DictCombatSpell[CombatSpell.SpellPoison] = 2;
                    target.DictCombatSpell[CombatSpell.SpellNegative4to7] = 1;
                    //target.DictCombatSpell[CombatSpell.SpellDispelSummon] = 4;
                    target.DictCombatSpell[CombatSpell.SpellBeneficial5] = 1;

                    target.DictCombatHealOther[CombatHealOther.SpellHealOther75] = 1;
                    target.DictCombatHealOther[CombatHealOther.SpellHealOther50] = 2;
                    target.DictCombatHealOther[CombatHealOther.SpellHealOther25] = 3;
                    target.DictCombatHealOther[CombatHealOther.SpellCureOther] = 1;

                    target.DictCombatHealSelf[CombatHealSelf.SpellHealSelf75] = 1;
                    target.DictCombatHealSelf[CombatHealSelf.SpellHealSelf50] = 2;
                    target.DictCombatHealSelf[CombatHealSelf.SpellHealSelf25] = 3;
                    target.DictCombatHealSelf[CombatHealSelf.SpellCureSelf] = 1;

                    target.DictWanderAction[WanderAction.None] = 10;
                    target.DictWanderAction[WanderAction.SpellHealSelf100] = 1;
                    target.DictWanderAction[WanderAction.SpellCureSelf] = 1;
                    target.DictWanderAction[WanderAction.SpellHealOther100] = 1;
                    target.DictWanderAction[WanderAction.SpellCureOther] = 1;
                    break;

                case AISubgroup.GroupHealerMage5:
                    target.DictCombatRange[CombatRange.WeaponAttackRange] = 0;
                    target.DictCombatRange[CombatRange.SpellRange] = 5;
                    target.DictCombatRange[CombatRange.Withdraw] = 1;

                    target.DictCombatAction[CombatAction.AttackOnly] = 2;
                    target.DictCombatAction[CombatAction.CombatSpell] = 2;
                    target.DictCombatAction[CombatAction.CombatHealOther] = 4;
                    target.DictCombatAction[CombatAction.CombatHealSelf] = 2;

                    target.DictCombatSpell[CombatSpell.SpellDamage1] = 0;
                    target.DictCombatSpell[CombatSpell.SpellDamage2] = 0;
                    target.DictCombatSpell[CombatSpell.SpellDamage3] = 1;
                    target.DictCombatSpell[CombatSpell.SpellDamage4] = 2;
                    target.DictCombatSpell[CombatSpell.SpellDamage5] = 3;
                    target.DictCombatSpell[CombatSpell.SpellDamage6] = 6;
                    target.DictCombatSpell[CombatSpell.SpellDamage7] = 6;
                    target.DictCombatSpell[CombatSpell.SpellDamageAOE7] = 2;

                    target.DictCombatSpell[CombatSpell.SpellPoison] = 3;
                    target.DictCombatSpell[CombatSpell.SpellNegative4to7] = 1;
                    //target.DictCombatSpell[CombatSpell.SpellDispelSummon] = 5;
                    target.DictCombatSpell[CombatSpell.SpellBeneficial5] = 1;

                    target.DictCombatHealOther[CombatHealOther.SpellHealOther75] = 1;
                    target.DictCombatHealOther[CombatHealOther.SpellHealOther50] = 2;
                    target.DictCombatHealOther[CombatHealOther.SpellHealOther25] = 3;
                    target.DictCombatHealOther[CombatHealOther.SpellCureOther] = 1;

                    target.DictCombatHealSelf[CombatHealSelf.SpellHealSelf75] = 1;
                    target.DictCombatHealSelf[CombatHealSelf.SpellHealSelf50] = 2;
                    target.DictCombatHealSelf[CombatHealSelf.SpellHealSelf25] = 3;
                    target.DictCombatHealSelf[CombatHealSelf.SpellCureSelf] = 1;

                    target.DictWanderAction[WanderAction.None] = 10;
                    target.DictWanderAction[WanderAction.SpellHealSelf100] = 1;
                    target.DictWanderAction[WanderAction.SpellCureSelf] = 1;
                    target.DictWanderAction[WanderAction.SpellHealOther100] = 1;
                    target.DictWanderAction[WanderAction.SpellCureOther] = 1;
                    break;

                case AISubgroup.GroupHealerMeleeMage1:
                    target.DictCombatRange[CombatRange.WeaponAttackRange] = 1;

                    target.DictCombatAction[CombatAction.AttackOnly] = 2;
                    target.DictCombatAction[CombatAction.CombatSpell] = 2;
                    target.DictCombatAction[CombatAction.CombatHealOther] = 4;
                    target.DictCombatAction[CombatAction.CombatHealSelf] = 2;

                    target.DictCombatSpell[CombatSpell.SpellDamage1] = 2;
                    target.DictCombatSpell[CombatSpell.SpellDamage2] = 3;
                    target.DictCombatSpell[CombatSpell.SpellDamage3] = 4;
                    target.DictCombatSpell[CombatSpell.SpellDamage4] = 2;
                    target.DictCombatSpell[CombatSpell.SpellDamage5] = 1;
                    target.DictCombatSpell[CombatSpell.SpellNegative1to3] = 1;
                    //target.DictCombatSpell[CombatSpell.SpellDispelSummon] = 1;

                    target.DictCombatHealOther[CombatHealOther.SpellHealOther75] = 0;
                    target.DictCombatHealOther[CombatHealOther.SpellHealOther50] = 0;
                    target.DictCombatHealOther[CombatHealOther.SpellHealOther25] = 1;
                    target.DictCombatHealOther[CombatHealOther.SpellCureOther] = 1;

                    target.DictCombatHealSelf[CombatHealSelf.SpellHealSelf25] = 1;
                    target.DictCombatHealSelf[CombatHealSelf.SpellCureSelf] = 1;

                    target.DictWanderAction[WanderAction.None] = 10;
                    target.DictWanderAction[WanderAction.SpellHealSelf100] = 1;
                    target.DictWanderAction[WanderAction.SpellCureSelf] = 1;
                    target.DictWanderAction[WanderAction.SpellHealOther100] = 1;
                    target.DictWanderAction[WanderAction.SpellCureOther] = 1;
                    break;

                case AISubgroup.GroupHealerMeleeMage2:
                    target.DictCombatRange[CombatRange.WeaponAttackRange] = 1;

                    target.DictCombatAction[CombatAction.AttackOnly] = 2;
                    target.DictCombatAction[CombatAction.CombatSpell] = 2;
                    target.DictCombatAction[CombatAction.CombatHealOther] = 4;
                    target.DictCombatAction[CombatAction.CombatHealSelf] = 2;

                    target.DictCombatSpell[CombatSpell.SpellDamage1] = 1;
                    target.DictCombatSpell[CombatSpell.SpellDamage2] = 3;
                    target.DictCombatSpell[CombatSpell.SpellDamage3] = 4;
                    target.DictCombatSpell[CombatSpell.SpellDamage4] = 5;
                    target.DictCombatSpell[CombatSpell.SpellDamage5] = 3;
                    target.DictCombatSpell[CombatSpell.SpellDamage6] = 1;
                    target.DictCombatSpell[CombatSpell.SpellNegative1to3] = 1;
                    target.DictCombatSpell[CombatSpell.SpellBeneficial1to2] = 0;

                    target.DictCombatHealOther[CombatHealOther.SpellHealOther75] = 0;
                    target.DictCombatHealOther[CombatHealOther.SpellHealOther50] = 1;
                    target.DictCombatHealOther[CombatHealOther.SpellHealOther25] = 2;
                    target.DictCombatHealOther[CombatHealOther.SpellCureOther] = 1;

                    target.DictCombatHealSelf[CombatHealSelf.SpellHealSelf50] = 1;
                    target.DictCombatHealSelf[CombatHealSelf.SpellHealSelf25] = 2;
                    target.DictCombatHealSelf[CombatHealSelf.SpellCureSelf] = 1;

                    target.DictWanderAction[WanderAction.None] = 10;
                    target.DictWanderAction[WanderAction.SpellHealSelf100] = 1;
                    target.DictWanderAction[WanderAction.SpellCureSelf] = 1;
                    target.DictWanderAction[WanderAction.SpellHealOther100] = 1;
                    target.DictWanderAction[WanderAction.SpellCureOther] = 1;
                    break;

                case AISubgroup.GroupHealerMeleeMage3:
                    target.DictCombatAction[CombatAction.AttackOnly] = 2;
                    target.DictCombatAction[CombatAction.CombatSpell] = 2;
                    target.DictCombatAction[CombatAction.CombatHealOther] = 4;
                    target.DictCombatAction[CombatAction.CombatHealSelf] = 2;

                    target.DictCombatSpell[CombatSpell.SpellDamage1] = 1;
                    target.DictCombatSpell[CombatSpell.SpellDamage2] = 2;
                    target.DictCombatSpell[CombatSpell.SpellDamage3] = 4;
                    target.DictCombatSpell[CombatSpell.SpellDamage4] = 6;
                    target.DictCombatSpell[CombatSpell.SpellDamage5] = 5;
                    target.DictCombatSpell[CombatSpell.SpellDamage6] = 3;
                    target.DictCombatSpell[CombatSpell.SpellDamage7] = 1;
                    target.DictCombatSpell[CombatSpell.SpellPoison] = 1;
                    target.DictCombatSpell[CombatSpell.SpellNegative1to3] = 1;
                    //target.DictCombatSpell[CombatSpell.SpellDispelSummon] = 3;

                    target.DictCombatHealOther[CombatHealOther.SpellHealOther75] = 1;
                    target.DictCombatHealOther[CombatHealOther.SpellHealOther50] = 2;
                    target.DictCombatHealOther[CombatHealOther.SpellHealOther25] = 3;
                    target.DictCombatHealOther[CombatHealOther.SpellCureOther] = 1;

                    target.DictCombatHealSelf[CombatHealSelf.SpellHealSelf75] = 1;
                    target.DictCombatHealSelf[CombatHealSelf.SpellHealSelf50] = 2;
                    target.DictCombatHealSelf[CombatHealSelf.SpellHealSelf25] = 3;
                    target.DictCombatHealSelf[CombatHealSelf.SpellCureSelf] = 1;

                    target.DictWanderAction[WanderAction.None] = 10;
                    target.DictWanderAction[WanderAction.SpellHealSelf100] = 1;
                    target.DictWanderAction[WanderAction.SpellCureSelf] = 1;
                    target.DictWanderAction[WanderAction.SpellHealOther100] = 1;
                    target.DictWanderAction[WanderAction.SpellCureOther] = 1;
                    break;

                case AISubgroup.GroupHealerMeleeMage4:
                    target.DictCombatRange[CombatRange.WeaponAttackRange] = 1;

                    target.DictCombatAction[CombatAction.AttackOnly] = 2;
                    target.DictCombatAction[CombatAction.CombatSpell] = 2;
                    target.DictCombatAction[CombatAction.CombatHealOther] = 4;
                    target.DictCombatAction[CombatAction.CombatHealSelf] = 2;

                    target.DictCombatSpell[CombatSpell.SpellDamage1] = 1;
                    target.DictCombatSpell[CombatSpell.SpellDamage2] = 1;
                    target.DictCombatSpell[CombatSpell.SpellDamage3] = 2;
                    target.DictCombatSpell[CombatSpell.SpellDamage4] = 3;
                    target.DictCombatSpell[CombatSpell.SpellDamage5] = 4;
                    target.DictCombatSpell[CombatSpell.SpellDamage6] = 5;
                    target.DictCombatSpell[CombatSpell.SpellDamage7] = 3;
                    target.DictCombatSpell[CombatSpell.SpellDamageAOE7] = 1;

                    target.DictCombatSpell[CombatSpell.SpellPoison] = 2;
                    target.DictCombatSpell[CombatSpell.SpellNegative4to7] = 1;
                    //target.DictCombatSpell[CombatSpell.SpellDispelSummon] = 4;
                    target.DictCombatSpell[CombatSpell.SpellBeneficial5] = 1;

                    target.DictCombatHealOther[CombatHealOther.SpellHealOther75] = 1;
                    target.DictCombatHealOther[CombatHealOther.SpellHealOther50] = 2;
                    target.DictCombatHealOther[CombatHealOther.SpellHealOther25] = 3;
                    target.DictCombatHealOther[CombatHealOther.SpellCureOther] = 1;

                    target.DictCombatHealSelf[CombatHealSelf.SpellHealSelf75] = 1;
                    target.DictCombatHealSelf[CombatHealSelf.SpellHealSelf50] = 2;
                    target.DictCombatHealSelf[CombatHealSelf.SpellHealSelf25] = 3;
                    target.DictCombatHealSelf[CombatHealSelf.SpellCureSelf] = 1;

                    target.DictWanderAction[WanderAction.None] = 10;
                    target.DictWanderAction[WanderAction.SpellHealSelf100] = 1;
                    target.DictWanderAction[WanderAction.SpellCureSelf] = 1;
                    target.DictWanderAction[WanderAction.SpellHealOther100] = 1;
                    target.DictWanderAction[WanderAction.SpellCureOther] = 1;
                    break;

                case AISubgroup.GroupHealerMeleeMage5:
                    target.DictCombatRange[CombatRange.WeaponAttackRange] = 1;

                    target.DictCombatAction[CombatAction.AttackOnly] = 2;
                    target.DictCombatAction[CombatAction.CombatSpell] = 2;
                    target.DictCombatAction[CombatAction.CombatHealOther] = 4;
                    target.DictCombatAction[CombatAction.CombatHealSelf] = 2;

                    target.DictCombatSpell[CombatSpell.SpellDamage1] = 0;
                    target.DictCombatSpell[CombatSpell.SpellDamage2] = 0;
                    target.DictCombatSpell[CombatSpell.SpellDamage3] = 1;
                    target.DictCombatSpell[CombatSpell.SpellDamage4] = 2;
                    target.DictCombatSpell[CombatSpell.SpellDamage5] = 3;
                    target.DictCombatSpell[CombatSpell.SpellDamage6] = 6;
                    target.DictCombatSpell[CombatSpell.SpellDamage7] = 6;
                    target.DictCombatSpell[CombatSpell.SpellDamageAOE7] = 2;

                    target.DictCombatSpell[CombatSpell.SpellPoison] = 3;
                    target.DictCombatSpell[CombatSpell.SpellNegative4to7] = 1;
                    //target.DictCombatSpell[CombatSpell.SpellDispelSummon] = 5;
                    target.DictCombatSpell[CombatSpell.SpellBeneficial5] = 1;

                    target.DictCombatHealOther[CombatHealOther.SpellHealOther75] = 1;
                    target.DictCombatHealOther[CombatHealOther.SpellHealOther50] = 2;
                    target.DictCombatHealOther[CombatHealOther.SpellHealOther25] = 3;
                    target.DictCombatHealOther[CombatHealOther.SpellCureOther] = 1;

                    target.DictCombatHealSelf[CombatHealSelf.SpellHealSelf75] = 1;
                    target.DictCombatHealSelf[CombatHealSelf.SpellHealSelf50] = 2;
                    target.DictCombatHealSelf[CombatHealSelf.SpellHealSelf25] = 3;
                    target.DictCombatHealSelf[CombatHealSelf.SpellCureSelf] = 1;

                    target.DictWanderAction[WanderAction.None] = 10;
                    target.DictWanderAction[WanderAction.SpellHealSelf100] = 1;
                    target.DictWanderAction[WanderAction.SpellCureSelf] = 1;
                    target.DictWanderAction[WanderAction.SpellHealOther100] = 1;
                    target.DictWanderAction[WanderAction.SpellCureOther] = 1;
                    break;

                case AISubgroup.GroupHealerMelee:
                    target.DictCombatRange[CombatRange.WeaponAttackRange] = 1;

                    target.DictCombatAction[CombatAction.AttackOnly] = 4;
                    target.DictCombatAction[CombatAction.CombatHealOther] = 4;
                    target.DictCombatAction[CombatAction.CombatHealSelf] = 2;

                    target.DictCombatHealOther[CombatHealOther.SpellHealOther75] = 1;
                    target.DictCombatHealOther[CombatHealOther.SpellHealOther50] = 2;
                    target.DictCombatHealOther[CombatHealOther.SpellHealOther25] = 3;
                    target.DictCombatHealOther[CombatHealOther.SpellCureOther] = 1;

                    target.DictCombatHealSelf[CombatHealSelf.SpellHealSelf75] = 1;
                    target.DictCombatHealSelf[CombatHealSelf.SpellHealSelf50] = 2;
                    target.DictCombatHealSelf[CombatHealSelf.SpellHealSelf25] = 3;
                    target.DictCombatHealSelf[CombatHealSelf.SpellCureSelf] = 1;

                    target.DictWanderAction[WanderAction.None] = 10;
                    target.DictWanderAction[WanderAction.SpellHealSelf100] = 1;
                    target.DictWanderAction[WanderAction.SpellCureSelf] = 1;
                    target.DictWanderAction[WanderAction.SpellHealOther100] = 1;
                    target.DictWanderAction[WanderAction.SpellCureOther] = 1;
                    break;

                case AISubgroup.WanderingHealer:
                    target.DictCombatRange[CombatRange.WeaponAttackRange] = 1;

                    target.DictCombatAction[CombatAction.AttackOnly] = 1;
                    target.DictCombatAction[CombatAction.CombatSpell] = 5;
                    target.DictCombatAction[CombatAction.CombatHealSelf] = 5;

                    target.DictCombatSpell[CombatSpell.SpellDamage1] = 1;
                    target.DictCombatSpell[CombatSpell.SpellDamage2] = 1;
                    target.DictCombatSpell[CombatSpell.SpellDamage3] = 2;
                    target.DictCombatSpell[CombatSpell.SpellDamage4] = 3;
                    target.DictCombatSpell[CombatSpell.SpellDamage5] = 4;
                    target.DictCombatSpell[CombatSpell.SpellDamage6] = 5;
                    target.DictCombatSpell[CombatSpell.SpellDamage7] = 3;
                    target.DictCombatSpell[CombatSpell.SpellPoison] = 2;
                    target.DictCombatSpell[CombatSpell.SpellNegative4to7] = 1;
                    //target.DictCombatSpell[CombatSpell.SpellDispelSummon] = 4;
                    target.DictCombatSpell[CombatSpell.SpellBeneficial5] = 1;

                    target.DictCombatHealSelf[CombatHealSelf.SpellHealSelf75] = 2;
                    target.DictCombatHealSelf[CombatHealSelf.SpellCureSelf] = 2;
                    target.DictCombatHealSelf[CombatHealSelf.BandageHealSelf75] = 1;

                    target.DictCombatFlee[CombatFlee.Flee25] = 0;
                    target.DictCombatFlee[CombatFlee.Flee10] = 0;
                    break;

                case AISubgroup.GroupMedicMelee:
                    target.DictCombatRange[CombatRange.WeaponAttackRange] = 1;
                    target.DictCombatRange[CombatRange.SpellRange] = 0;
                    target.DictCombatRange[CombatRange.Withdraw] = 0;

                    target.DictCombatAction[CombatAction.AttackOnly] = 5;
                    target.DictCombatAction[CombatAction.CombatHealOther] = 4;
                    target.DictCombatAction[CombatAction.CombatHealSelf] = 3;

                    target.DictCombatHealOther[CombatHealOther.BandageHealOther75] = 1;
                    target.DictCombatHealOther[CombatHealOther.BandageHealOther50] = 2;
                    target.DictCombatHealOther[CombatHealOther.BandageHealOther25] = 3;
                    target.DictCombatHealOther[CombatHealOther.BandageCureOther] = 1;

                    target.DictCombatHealSelf[CombatHealSelf.BandageHealSelf75] = 1;
                    target.DictCombatHealSelf[CombatHealSelf.BandageHealSelf50] = 2;
                    target.DictCombatHealSelf[CombatHealSelf.BandageHealSelf25] = 3;
                    target.DictCombatHealSelf[CombatHealSelf.BandageCureSelf] = 1;

                    target.DictWanderAction[WanderAction.BandageHealOther100] = 1;
                    target.DictWanderAction[WanderAction.BandageHealSelf100] = 1;
                    target.DictWanderAction[WanderAction.BandageCureOther] = 1;
                    target.DictWanderAction[WanderAction.BandageCureSelf] = 1;
                    break;

                case AISubgroup.GroupMedicRanged:
                    target.DictCombatRange[CombatRange.WeaponAttackRange] = 1;
                    target.DictCombatRange[CombatRange.Withdraw] = 5;

                    target.DictCombatAction[CombatAction.AttackOnly] = 5;
                    target.DictCombatAction[CombatAction.CombatHealOther] = 4;
                    target.DictCombatAction[CombatAction.CombatHealSelf] = 3;

                    target.DictCombatHealOther[CombatHealOther.BandageHealOther75] = 1;
                    target.DictCombatHealOther[CombatHealOther.BandageHealOther50] = 2;
                    target.DictCombatHealOther[CombatHealOther.BandageHealOther25] = 3;
                    target.DictCombatHealOther[CombatHealOther.BandageCureOther] = 1;

                    target.DictCombatHealSelf[CombatHealSelf.BandageHealSelf75] = 1;
                    target.DictCombatHealSelf[CombatHealSelf.BandageHealSelf50] = 2;
                    target.DictCombatHealSelf[CombatHealSelf.BandageHealSelf25] = 3;
                    target.DictCombatHealSelf[CombatHealSelf.BandageCureSelf] = 1;

                    target.DictWanderAction[WanderAction.BandageHealOther100] = 1;
                    target.DictWanderAction[WanderAction.BandageHealSelf100] = 1;
                    target.DictWanderAction[WanderAction.BandageCureOther] = 1;
                    target.DictWanderAction[WanderAction.BandageCureSelf] = 1;
                    break;

                case AISubgroup.SuperPredator:
                    target.DictCombatTargeting[CombatTargeting.Aggressor] = 3;
                    target.DictCombatTargeting[CombatTargeting.Predator] = 1;
                    target.DictCombatTargeting[CombatTargeting.Prey] = 2;

                    target.DictCombatFlee[CombatFlee.Flee10] = 1;

                    target.SuperPredator = true;
                    break;

                case AISubgroup.Predator:
                    target.DictCombatTargeting[CombatTargeting.Prey] = 1;

                    target.DictCombatFlee[CombatFlee.Flee25] = 1;
                    target.DictCombatFlee[CombatFlee.Flee10] = 2;

                    target.Predator = true;
                    break;

                case AISubgroup.Prey:
                    target.DictCombatFlee[CombatFlee.Flee25] = 3;
                    target.DictCombatFlee[CombatFlee.Flee10] = 5;

                    target.Prey = true;
                    break;

                case AISubgroup.Berserk:
                    target.DictCombatTargeting[CombatTargeting.Any] = 1;
                    target.DictCombatTargetingWeight[CombatTargetingWeight.Closest] = 10;
                    break;

                case AISubgroup.MeleePotion:
                    target.DictCombatAction[CombatAction.AttackOnly] = 5;
                    target.DictCombatAction[CombatAction.CombatHealSelf] = 1;

                    target.DictCombatHealSelf[CombatHealSelf.PotionHealSelf50] = 1;
                    target.DictCombatHealSelf[CombatHealSelf.PotionCureSelf] = 3;
                    break;

                case AISubgroup.AntiArmor:
                    target.DictCombatTargetingWeight[CombatTargetingWeight.HighestArmor] = 5;
                    break;

                case AISubgroup.Ranged:
                    target.DictCombatRange[CombatRange.WeaponAttackRange] = 5;
                    target.DictCombatRange[CombatRange.Withdraw] = 5;
                    break;

                case AISubgroup.Scout:
                    target.DictCombatRange[CombatRange.WeaponAttackRange] = 5;
                    target.DictCombatRange[CombatRange.Withdraw] = 5;

                    target.DictWanderAction[WanderAction.None] = 5;
                    target.DictWanderAction[WanderAction.Stealth] = 1;
                    break;

                case AISubgroup.Thief:
                    target.DictCombatRange[CombatRange.WeaponAttackRange] = 5;

                    target.DictWanderAction[WanderAction.None] = 5;
                    target.DictWanderAction[WanderAction.Stealth] = 1;
                    break;

                case AISubgroup.Assassin:
                    target.DictCombatAction[CombatAction.AttackOnly] = 5;
                    target.DictCombatAction[CombatAction.CombatHealSelf] = 1;
                    target.DictCombatAction[CombatAction.CombatSpecialAction] = 5;

                    target.DictCombatTargetingWeight[CombatTargetingWeight.LowestHitPoints] = 5;

                    target.DictCombatSpecialAction[CombatSpecialAction.ApplyWeaponPoison] = 1;

                    target.DictCombatHealSelf[CombatHealSelf.PotionCureSelf] = 1;

                    target.DictWanderAction[WanderAction.None] = 5;
                    target.DictWanderAction[WanderAction.Stealth] = 1;
                    break;

                case AISubgroup.Bomber:
                    target.DictCombatAction[CombatAction.AttackOnly] = 5;
                    target.DictCombatAction[CombatAction.CombatSpecialAction] = 1;

                    target.DictCombatSpecialAction[CombatSpecialAction.ThrowBomb] = 1;
                    break;

                case AISubgroup.DungeonGuardMelee:
                    target.DictCombatTargeting[CombatTargeting.PlayerCriminal] = 10;
                    target.DictCombatTargetingWeight[CombatTargetingWeight.Closest] = 5;
                    target.DictCombatTargetingWeight[CombatTargetingWeight.HighestArmor] = 5;
                    target.DictCombatTargetingWeight[CombatTargetingWeight.LowestHitPoints] = 5;

                    target.DictCombatRange[CombatRange.WeaponAttackRange] = 1;

                    target.DictCombatAction[CombatAction.AttackOnly] = 6;
                    target.DictCombatAction[CombatAction.CombatHealSelf] = 2;
                    target.DictCombatAction[CombatAction.CombatHealOther] = 1;

                    target.DictCombatHealSelf[CombatHealSelf.BandageHealSelf75] = 1;
                    target.DictCombatHealSelf[CombatHealSelf.PotionCureSelf] = 10;

                    target.DictCombatHealOther[CombatHealOther.BandageHealOther75] = 1;

                    target.DictCombatFlee[CombatFlee.Flee25] = 0;
                    target.DictCombatFlee[CombatFlee.Flee10] = 0;
                    break;

                case AISubgroup.DungeonGuardRanged:
                    target.DictCombatTargeting[CombatTargeting.PlayerCriminal] = 10;
                    target.DictCombatTargetingWeight[CombatTargetingWeight.Closest] = 7;
                    target.DictCombatTargetingWeight[CombatTargetingWeight.LowestArmor] = 5;
                    target.DictCombatTargetingWeight[CombatTargetingWeight.LowestHitPoints] = 10;

                    target.DictCombatRange[CombatRange.WeaponAttackRange] = 4;
                    target.DictCombatRange[CombatRange.Withdraw] = 1;

                    target.DictCombatAction[CombatAction.AttackOnly] = 10;
                    target.DictCombatAction[CombatAction.CombatHealSelf] = 3;
                    target.DictCombatAction[CombatAction.CombatHealOther] = 1;

                    target.DictCombatHealSelf[CombatHealSelf.BandageHealSelf75] = 1;
                    target.DictCombatHealSelf[CombatHealSelf.PotionCureSelf] = 10;

                    target.DictCombatHealOther[CombatHealOther.BandageHealOther75] = 1;

                    target.DictCombatFlee[CombatFlee.Flee25] = 0;
                    target.DictCombatFlee[CombatFlee.Flee10] = 0;
                    break;

                case AISubgroup.None:
                    break;
            }
        }
    }
}