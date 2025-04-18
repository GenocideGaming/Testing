using System;
using System.Collections.Generic;
using System.Text;
using Server.Gumps;
using Server.Mobiles;

namespace Server.Scripts.Custom.Citizenship
{
    public class CommonwealthElectionGump : Gump
    {
        private ICommonwealth mCommonwealth;

        public CommonwealthElectionGump(ICommonwealth commonwealth, Mobile from) : base(0, 0)
        {
            this.Closable = true;
            this.Disposable = true;
            this.Dragable = true;
            this.Resizable = false;

            mCommonwealth = commonwealth;

            if (mCommonwealth == null)
            {
                Console.WriteLine("MyCommonwealth is null on election gump call");
                return;
            }
            if (mCommonwealth.Election == null)
            {
                Console.WriteLine("Election is null on gump call");
                return;
            }
            AddPage(0);
            AddBackground(0, 0, 282, 490, 9250);
            AddLabel(52, 30, 0, "Township Voting for "+ commonwealth.Definition.TownName);

            TimeSpan span = commonwealth.State.Election.NextStateTime;
            string formattedTime = string.Format("{0}{1}{2}",
            span.Days > 0 ? string.Format("{0:0} d, ", span.Days) : string.Empty,
            span.Hours > 0 ? string.Format("{0:0} h, ", span.Hours) : string.Empty,
            span.Minutes > 0 ? string.Format("{0:0} m, ", span.Minutes) : string.Empty);

            if (mCommonwealth.Election.CurrentState == ElectionState.Pending)
            {
               // string text = "Your town is currently enjoying the rule of  " + commonwealth.Mayor != null ? commonwealth.Mayor.Name : string.Empty;
                //AddLabel(30, 60, 0, text);
                AddLabel(30, 60, 0, "Campaigning begins in " + formattedTime);
            }
            else if (mCommonwealth.Election.CurrentState == ElectionState.Campaign)
            {
                if (mCommonwealth.Election.FindCandidate(from) != null)
                {
                    AddLabel(30, 60, 0, "You are already a candidate.");
                    AddButton(30, 93, 1210, 1209, (int)Buttons.RemoveCandidate, GumpButtonType.Reply, 0);
                    AddLabel(52, 90, 0, "Remove yourself as candidate.");
                    AddLabel(30, 120, 0, "Voting begins in " + formattedTime);
                }
                else
                {
                    AddButton(30, 63, 1210, 1209, (int)Buttons.AddCandidate, GumpButtonType.Reply, 0); //add player as candidate
                    AddLabel(52, 60, 0, "Add yourself as candidate.");
                    AddLabel(30, 90, 0, "Voting begins in " + formattedTime);
                }
                
            }
            else
            {
                for (int i = 0; i < mCommonwealth.Election.Candidates.Count; i++)
                {
                    AddRadio(30, i * 30 + 60, 209, 208, false, i);
                    AddLabel(60, i * 30 + 60, 0, mCommonwealth.Election.Candidates[i].Mobile.Name);
                }

                AddButton(30, 350, 247, 248, (int) Buttons.OkayToCandidateSelection, GumpButtonType.Reply, 0); //okay button after selection of mayor candidate

                AddLabel(30, 320, 0, "Voting ends in " + formattedTime);
            }

        
        }
        public enum Buttons
        {
            OkayToCandidateSelection,
            AddCandidate,
            RemoveCandidate,
        }
        public override void OnResponse(Network.NetState sender, RelayInfo info)
        {

            PlayerMobile pm = sender.Mobile as PlayerMobile;

            if (pm != null)
            {
                if (info.ButtonID == (int)Buttons.OkayToCandidateSelection) //okay button
                {
                    int selected = -1;

                    for (int i = 0; i < mCommonwealth.Election.Candidates.Count; i++)
                    {
                        
                        bool isChecked = info.IsSwitched(i);

                        if (isChecked)
                            selected = i;
                    }

                    if (selected == -1)
                    {
                        pm.SendMessage("You did not select a candidate, please try again.");
                        return;
                    }

                    if (!mCommonwealth.Election.CanVote(pm))
                    {
                        pm.SendMessage("You cannot vote.");
                        return;
                    }

                    mCommonwealth.Election.AddVoter(pm, mCommonwealth.Election.Candidates[selected]);

                    pm.SendMessage("You have succesfully voted for " + mCommonwealth.Election.Candidates[selected].Mobile.Name);
                }
                else if (info.ButtonID == (int)Buttons.AddCandidate) //add candidate
                {
                    
                    mCommonwealth.Election.AddCandidate(pm);
                    
                }
                else if (info.ButtonID == (int)Buttons.RemoveCandidate)
                {
                    mCommonwealth.Election.RemoveCandidate(pm);
                }
            
            }
        }
    }
}
