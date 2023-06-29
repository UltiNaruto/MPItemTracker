using System;
using System.Drawing;

namespace Wrapper
{
    public class Metroid
    {
        public virtual long IGT() { return 0; }
        public String IGTAsStr(IGTDisplayType igt_display_type)
        {
            long __IGT = IGT();
            String res = String.Empty;
            res = String.Format("{0:00}:{1:00}:{2:00}", __IGT / (60 * 60 * 1000), (__IGT / (60 * 1000)) % 60, (__IGT / 1000) % 60);
            if(igt_display_type == IGTDisplayType.WithMS)
                res += String.Format(".{0:000}", __IGT % 1000);
            return res;
        }
        public bool IsIngame() { return IGT() > 16; }
        public virtual bool IsMorphed() { return false; }
        public virtual bool IsSwitchingState() { return false; }
        public virtual bool HasPickup(String pickup) { return false; }
        public virtual int GetPickupCount(String pickup) { return 0; }

        public virtual int GetIntState(String state) { return -1; }
        public virtual bool GetBoolState(String state) { return false; }
        public virtual void SetIntState(String state, int value) { }
        public virtual void SetBoolState(String state, bool value) { }
        public virtual Image GetIcon(String pickup) { return null; }
        public virtual void Update() { }
    }
}