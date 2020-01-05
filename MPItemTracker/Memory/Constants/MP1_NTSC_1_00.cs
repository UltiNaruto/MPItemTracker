using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prime.Memory.Constants
{
    internal class MP1_NTSC_1_00 : _MP1
    {
        internal const long OFF_CGAMEGLOBALOBJECTS = 0x457798;
        internal const long OFF_CGAMESTATE = OFF_CGAMEGLOBALOBJECTS + 0x134;
        internal const long OFF_CSTATEMANAGER = 0x45A1A8;
        internal const long OFF_MORPHBALLBOMBS_COUNT = 0x457D1B;

        internal override long CGameState
        {
            get
            {
                long result = Dolphin.ReadUInt32(GC.RAMBaseAddress + OFF_CGAMESTATE);
                if (result < GC.RAMBaseAddress)
                    return -1;
                return result;
            }
        }

        internal override long CPlayerState
        {
            get
            {
                long result = Dolphin.ReadUInt32(GC.RAMBaseAddress + OFF_CSTATEMANAGER + OFF_CPLAYERSTATE);
                if (result < GC.RAMBaseAddress)
                    return -1;
                result = Dolphin.ReadUInt32(result);
                if (result < GC.RAMBaseAddress)
                    return -1;
                return result;
            }
        }

        internal override long CWorld
        {
            get
            {
                long result = Dolphin.ReadUInt32(GC.RAMBaseAddress + OFF_CSTATEMANAGER + OFF_CWORLD);
                if (result < GC.RAMBaseAddress)
                    return -1;
                return result;
            }
        }

        internal override long IGT
        {
            get
            {
                if (CGameState == -1)
                    return -1;
                return (long)(Dolphin.ReadFloat64(CGameState + OFF_PLAYTIME) * 1000);
            }
        }

        internal override string IGTAsStr
        {
            get
            {
                if (IGT == -1)
                    return "00:00:00.000";
                return String.Format("{0:00}:{1:00}:{2:00}.{3:000}", IGT / (60 * 60 * 1000), (IGT / (60 * 1000)) % 60, (IGT / 1000) % 60, IGT % 1000);
            }
        }

        internal override bool HaveIceBeam
        {
            get
            {
                if (CPlayerState == -1)
                    return false;
                return Dolphin.ReadUInt8(CPlayerState + OFF_ICEBEAM_OBTAINED) > 0;
            }
        }

        internal override bool HaveWaveBeam
        {
            get
            {
                if (CPlayerState == -1)
                    return false;
                return Dolphin.ReadUInt8(CPlayerState + OFF_WAVEBEAM_OBTAINED) > 0;
            }
        }

        internal override bool HavePlasmaBeam
        {
            get
            {
                if (CPlayerState == -1)
                    return false;
                return Dolphin.ReadUInt8(CPlayerState + OFF_PLASMABEAM_OBTAINED) > 0;
            }
        }

        internal override bool HaveMissiles
        {
            get
            {
                if (CPlayerState == -1)
                    return false;
                return Dolphin.ReadUInt8(CPlayerState + OFF_MAX_MISSILES) > 0;
            }
        }

        internal override bool HaveMorphBallBombs
        {
            get
            {
                if (CPlayerState == -1)
                    return false;
                return Dolphin.ReadUInt8(CPlayerState + OFF_MORPHBALLBOMBS_OBTAINED) > 0;
            }
        }

        internal override bool HavePowerBombs
        {
            get
            {
                if (CPlayerState == -1)
                    return false;
                return Dolphin.ReadUInt8(CPlayerState + OFF_MAX_POWERBOMBS) > 0;
            }
        }

        internal override bool HaveFlamethrower
        {
            get
            {
                if (CPlayerState == -1)
                    return false;
                return Dolphin.ReadUInt8(CPlayerState + OFF_FLAMETHROWER_OBTAINED) > 0;
            }
        }

        internal override bool HaveThermalVisor
        {
            get
            {
                if (CPlayerState == -1)
                    return false;
                return Dolphin.ReadUInt8(CPlayerState + OFF_THERMALVISOR_OBTAINED) > 0;
            }
        }

        internal override bool HaveChargeBeam
        {
            get
            {
                if (CPlayerState == -1)
                    return false;
                return Dolphin.ReadUInt8(CPlayerState + OFF_CHARGEBEAM_OBTAINED) > 0;
            }
        }

        internal override bool HaveSuperMissile
        {
            get
            {
                if (CPlayerState == -1)
                    return false;
                return Dolphin.ReadUInt8(CPlayerState + OFF_SUPERMISSILE_OBTAINED) > 0;
            }
        }

        internal override bool HaveGrappleBeam
        {
            get
            {
                if (CPlayerState == -1)
                    return false;
                return Dolphin.ReadUInt8(CPlayerState + OFF_GRAPPLEBEAM_OBTAINED) > 0;
            }
        }

        internal override bool HaveXRayVisor
        {
            get
            {
                if (CPlayerState == -1)
                    return false;
                return Dolphin.ReadUInt8(CPlayerState + OFF_XRAYVISOR_OBTAINED) > 0;
            }
        }

        internal override bool HaveIceSpreader
        {
            get
            {
                if (CPlayerState == -1)
                    return false;
                return Dolphin.ReadUInt8(CPlayerState + OFF_ICESPREADER_OBTAINED) > 0;
            }
        }

        internal override bool HaveSpaceJumpBoots
        {
            get
            {
                if (CPlayerState == -1)
                    return false;
                return Dolphin.ReadUInt8(CPlayerState + OFF_SPACEJUMPBOOTS_OBTAINED) > 0;
            }
        }

        internal override bool HaveMorphBall
        {
            get
            {
                if (CPlayerState == -1)
                    return false;
                return Dolphin.ReadUInt8(CPlayerState + OFF_MORPHBALL_OBTAINED) > 0;
            }
        }

        internal override bool HaveBoostBall
        {
            get
            {
                if (CPlayerState == -1)
                    return false;
                return Dolphin.ReadUInt8(CPlayerState + OFF_BOOSTBALL_OBTAINED) > 0;
            }
        }

        internal override bool HaveSpiderBall
        {
            get
            {
                if (CPlayerState == -1)
                    return false;
                return Dolphin.ReadUInt8(CPlayerState + OFF_SPIDERBALL_OBTAINED) > 0;
            }
        }

        internal override bool HaveGravitySuit
        {
            get
            {
                if (CPlayerState == -1)
                    return false;
                return Dolphin.ReadUInt8(CPlayerState + OFF_GRAVITYSUIT_OBTAINED) > 0;
            }
        }

        internal override bool HaveVariaSuit
        {
            get
            {
                if (CPlayerState == -1)
                    return false;
                return Dolphin.ReadUInt8(CPlayerState + OFF_VARIASUIT_OBTAINED) > 0;
            }
        }

        internal override bool HavePhazonSuit
        {
            get
            {
                if (CPlayerState == -1)
                    return false;
                return Dolphin.ReadUInt8(CPlayerState + OFF_PHAZONSUIT_OBTAINED) > 0;
            }
        }

        internal override bool HaveWavebuster
        {
            get
            {
                if (CPlayerState == -1)
                    return false;
                return Dolphin.ReadUInt8(CPlayerState + OFF_WAVEBUSTER_OBTAINED) > 0;
            }
        }

        internal override bool Artifacts(int index)
        {
            if (CPlayerState == -1)
                return false;
            if (index < 0)
                throw new Exception("Index can't be negative");
            switch (index)
            {
                case 0:
                    return Dolphin.ReadUInt8(CPlayerState + OFF_ARTIFACT_OF_TRUTH_OBTAINED) > 0;
                case 1:
                    return Dolphin.ReadUInt8(CPlayerState + OFF_ARTIFACT_OF_STRENGTH_OBTAINED) > 0;
                case 2:
                    return Dolphin.ReadUInt8(CPlayerState + OFF_ARTIFACT_OF_ELDER_OBTAINED) > 0;
                case 3:
                    return Dolphin.ReadUInt8(CPlayerState + OFF_ARTIFACT_OF_WILD_OBTAINED) > 0;
                case 4:
                    return Dolphin.ReadUInt8(CPlayerState + OFF_ARTIFACT_OF_LIFEGIVER_OBTAINED) > 0;
                case 5:
                    return Dolphin.ReadUInt8(CPlayerState + OFF_ARTIFACT_OF_WARRIOR_OBTAINED) > 0;
                case 6:
                    return Dolphin.ReadUInt8(CPlayerState + OFF_ARTIFACT_OF_CHOZO_OBTAINED) > 0;
                case 7:
                    return Dolphin.ReadUInt8(CPlayerState + OFF_ARTIFACT_OF_NATURE_OBTAINED) > 0;
                case 8:
                    return Dolphin.ReadUInt8(CPlayerState + OFF_ARTIFACT_OF_SUN_OBTAINED) > 0;
                case 9:
                    return Dolphin.ReadUInt8(CPlayerState + OFF_ARTIFACT_OF_WORLD_OBTAINED) > 0;
                case 10:
                    return Dolphin.ReadUInt8(CPlayerState + OFF_ARTIFACT_OF_SPIRIT_OBTAINED) > 0;
                case 11:
                    return Dolphin.ReadUInt8(CPlayerState + OFF_ARTIFACT_OF_NEWBORN_OBTAINED) > 0;
                default:
                    throw new Exception("There are no artifacts past the 12th artifact");
            }
        }
    }
}
