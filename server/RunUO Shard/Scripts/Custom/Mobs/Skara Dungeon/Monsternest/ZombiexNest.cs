using System;

namespace Server.Items
{
    public class ZombieNest : MonsterNest
    {
        public override string DefaultName { get { return "Curious Bones (Double click to attack)"; } }

        [Constructable]
        public ZombieNest() : base()
        {
            Hue = 0;
            MaxCount = 10;
            RespawnTime = TimeSpan.FromSeconds(25.0);
            HitsMax = 3600;
            Hits = 3600;
            NestSpawnType = "COVID";
            ItemID = 3793;
            LootLevel = 8;
            RangeHome = 100;
        }

        public override void AddLoot()
        {
            MonsterNestLoot loot = new MonsterNestLoot(6927, 0, LootLevel, "Scattered Bones");
            loot.MoveToWorld(Location, Map);
        }

        public ZombieNest(Serial serial) : base(serial)
        { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            reader.ReadInt();
        }
    }
}
