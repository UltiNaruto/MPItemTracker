using System;
using System.Drawing;

namespace Wrapper
{
    public class Metroid
    {
        public virtual long IGT() { return 0; }
        public String IGTAsStr()
        {
            long __IGT = IGT();
            return String.Format("{0:00}:{1:00}:{2:00}.{3:000}", __IGT / (60 * 60 * 1000), (__IGT / (60 * 1000)) % 60, (__IGT / 1000) % 60, __IGT % 1000);
        }
        public bool IsIngame() { return IGT() > 16; }
        public virtual bool IsMorphed() { return false; }
        public virtual bool IsSwitchingState() { return false; }
        public virtual bool HasPickup(String pickup) { return false; }
        public virtual int GetPickupCount(String pickup) { return 0; }
        public virtual Image GetIcon(String pickup) { return null;  }
    }
}