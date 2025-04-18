using Server.Games;
using Server.Mobiles;
using Server.Network;

namespace Server.Gumps
{
    /*
     * (February 2013)
     * The idea of a pseudoseer system was initially conceived by "a bazooka" of UOSA (PM "the bazookas" on the UOSA forums.uosecondage.com to contact)
     * and proposed to Derrick, the creator of uosecondage as a new way to generate in-game content through more player interactions.
     * I (a bazooka) initially thought of a system that simply changed a playermobile to look like / have the attributes of a "possessed" mob,
     * and there are some implementations of this on the RunUO forums... however, this approach is risky in that it potentially could create
     * a possible exploits and issues with ensuring that the Playermobile is returned to normal.
     * 
     * Derrick came up with the idea of simply transferring NetState over to the mob.  I (a bazooka) played around with it and determined that
     * this was certainly possible and, in fact, it worked extremely well to simply have the player "log in" as the NPC.  Thus the pseudoseer
     * system was born.  Derrick and I tested it significantly, and Derrick has integrated it with UOSA (~May 2012).
     * 
     * When I joined Rel Por as a developer (mob of monsters), I integrated it in Rel Por.  I also redid the permission system so it is now by string
     * rather than grouped (by monster type) bit flags.  i.e. to add somebody as a pseudoseer you would do "[pseudoseeradd orc balron ..." or set 
     * the PseudoSeerPermissions property of the playermobile to a space delimited string of NPC types.
     * 
     * This gump was conceived of and implemented by Derrick.
     * 
     * Anyone is free to use / modify / redistribute this code so long as this byline remains.
     */
    
    public class PossessGump : Gump
    {
        PlayerMobile m_OriginalOwner;
        Mobile m_CurrentOwner;

        public PossessGump(Mobile owner) : this(owner, owner as PlayerMobile) { }
        public PossessGump(Mobile owner, PlayerMobile OriginalOwner)  : base(30, 30)
        {
            m_OriginalOwner = OriginalOwner;
            m_CurrentOwner = owner;
            BuildGump();
        }

        public static void Initialize()
        {
            EventSink.Login += EventSink_Login;
        }

        static void EventSink_Login(LoginEventArgs e)
        {
            if (e.Mobile != null && CreaturePossession.HasAnyPossessPermissions(e.Mobile.NetState))
            {
                e.Mobile.SendGump(new PossessGump(e.Mobile));
            }
        }

        const int Width = 120;
        const int Height = 92;

        const int BorderMarginX = 5;
        const int BorderMarginY = 7;

        const int BodyMarginX = 9;
        const int BodyMarginY = 11;

        const int TextMarginX = 18;
        const int TextMarginY = 12;

        const int LineHeight = 18;

        enum Button
        {
            Close = -1,
            Connect = 1,
            Disconnect = 2
        }

        void BuildGump()
        {
            Closable = false;

            AddPage(0);

            AddBackground(0, 0, Width, Height, 0xA3C);

            AddImageTiled(BorderMarginX, BorderMarginY, Width-(BorderMarginX*2), Height-(BorderMarginY*2), 0xA40);
            AddAlphaRegion(BorderMarginX, BorderMarginY, Width - (BorderMarginX * 2), Height - (BorderMarginY * 2));

            AddImageTiled(BodyMarginX, BodyMarginY, Width - (BodyMarginX * 2), Height - (BodyMarginY * 2), 0xBBC);

            AddHtml(TextMarginX, TextMarginY, 65, 24, "POSSESS", false, false);
            AddButton(TextMarginX, TextMarginY + (1 * LineHeight), 0x943, 0x942, (int)Button.Connect, GumpButtonType.Reply, 0); // Connection

            if (!(m_CurrentOwner is PlayerMobile))
                AddButton(TextMarginX + 8, TextMarginY + (2 * LineHeight + 5), 0x7D9, 0x7DA, (int)Button.Disconnect, GumpButtonType.Reply, 0); // Logout
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            if (sender != null)
            {
                Mobile from = sender.Mobile;
                Button pressedButton = (Button)info.ButtonID;
                if (pressedButton == Button.Connect) {
                    CreaturePossession.OnPossessTargetRequest(from);
                    m_CurrentOwner.SendGump(this);
                }
                else if (pressedButton == Button.Disconnect)
                {
                    if (from != null)
                    {
                        if (!CreaturePossession.AttemptReturnToOriginalBody(from.NetState))
                        {
                            from.NetState.Dispose();
                        }
                    }
                }
                else
                {
                    m_CurrentOwner.SendGump(this);
                }
            }
        }

    }
}
