using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Scripts.Custom
{
    public class SerializableTimer : Timer
    {
        protected DateTime mStartTime;

        public SerializableTimer(DateTime startTime, TimeSpan delay, TimeSpan interval)
            : base(delay, interval)
        {
            mStartTime = startTime;
        }
        public SerializableTimer(GenericReader reader, TimeSpan delay, TimeSpan interval)
            : base(delay, interval)
        {
            int version = reader.ReadInt();
            mStartTime = reader.ReadDateTime();
        }

        public virtual void Serialize(GenericWriter writer)
        {
            writer.Write((int)0);
            writer.Write((DateTime)mStartTime);
        }
        protected override void OnTick()
        {
            base.OnTick();
        }
    }
}
