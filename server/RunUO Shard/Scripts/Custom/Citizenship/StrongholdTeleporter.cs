using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Server.Factions;
using Server.Scripts.Custom.Citizenship;

namespace Server.Items
{
    class StrongholdTeleporter : Teleporter
    {
        Faction mMilitia;

        [CommandProperty(AccessLevel.GameMaster, AccessLevel.GameMaster)]
        public Faction Militia { get { return mMilitia; } set { mMilitia = value; } }
       
        [Constructable]
        public StrongholdTeleporter(string townName) : base(new Point3D(0, 0, 0), Map.Felucca, true)
        {
            mMilitia = Faction.Find(townName);
            if (mMilitia == null)
            {
                Delete();
                return;
            }
        }

        public StrongholdTeleporter(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            
            writer.Write((int)0); //version
            Faction.WriteReference(writer, mMilitia);
        }
        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
            mMilitia = Faction.ReadReference(reader);
        }

        public override bool OnMoveOver(Mobile m)
        {
            if (Active)
            {

                Faction playerMilitia = Faction.Find(m);
                ICommonwealth playerTownship = Commonwealth.Find(m);

                if (playerTownship == null && m.AccessLevel == AccessLevel.Player)
                    return true;

                if (playerTownship != mMilitia.OwningCommonwealth && playerMilitia == null && m.AccessLevel == AccessLevel.Player)
                    return true;

                StartTeleport(m);
                return false;
            }
            return true;
        }
    }
}
