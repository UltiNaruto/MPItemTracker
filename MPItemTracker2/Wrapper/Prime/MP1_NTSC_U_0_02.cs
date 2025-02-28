﻿using System;

namespace Wrapper.Prime
{
    internal class MP1_NTSC_0_02 : Prime
    {
        protected const long OFF_CPLAYERSTATE_GETITEMAMOUNT = 0x800920A0;
        protected const long OFF_CPLAYERSTATE_POWERUPS_MAXCAPACITY = 0x803CE780;
        protected const long OFF_CGAMEGLOBALOBJECTS = 0x804587F8;
        protected const long OFF_CGAMESTATE = OFF_CGAMEGLOBALOBJECTS + 0x134;
        protected const long OFF_CSTATEMANAGER = 0x8045B208;
        protected const long OFF_MORPHBALLBOMBS_COUNT = 0x80458D78;

        protected override long CPlayer
        {
            get
            {
                return GCMem.ReadUInt32(OFF_CSTATEMANAGER + OFF_CPLAYER);
            }
        }

        protected override long CWorld
        {
            get
            {
                return GCMem.ReadUInt32(OFF_CSTATEMANAGER + OFF_CWORLD);
            }
        }

        protected override long CGameState
        {
            get
            {
                return GCMem.ReadUInt32(OFF_CGAMESTATE);
            }
        }

        protected override long CPlayerState
        {
            get
            {
                long result = GCMem.ReadUInt32(OFF_CSTATEMANAGER + OFF_CPLAYERSTATE);
                if (result == 0)
                    return 0;
                return GCMem.ReadUInt32(result); ;
            }
        }

        protected override long _IGT
        {
            get
            {
                if (CGameState == 0)
                    return 0;
                return (long)(GCMem.ReadFloat64(CGameState + OFF_PLAYTIME) * 1000);
            }
        }

        protected override bool _IsMorphed
        {
            get
            {
                if (CPlayer == 0)
                    return false;
                return GCMem.ReadInt32(CPlayer + OFF_CPLAYER_MORPHSTATE + 0x10) == 1;
            }
        }

        protected override bool _IsSwitchingState
        {
            get
            {
                if (CPlayer == 0)
                    return true;
                return GCMem.ReadInt32(CPlayer + OFF_CPLAYER_MORPHSTATE + 0x10) > 1;
            }
        }

        protected override int MaxMissiles
        {
            get
            {
                if (CPlayerState == 0)
                    return 0;
                return GCMem.ReadInt32(CPlayerState + OFF_MAX_MISSILES);
            }
        }

        protected override int MaxPowerBombs
        {
            get
            {
                if (CPlayerState == 0)
                    return 0;
                return GCMem.ReadInt32(CPlayerState + OFF_MAX_POWERBOMBS);
            }
        }

        protected override int MaxEnergyTanks
        {
            get
            {
                if (CPlayerState == 0)
                    return 0;
                return GCMem.ReadInt32(CPlayerState + OFF_ENERGYTANKS_OBTAINED);
            }
        }

        protected override bool HavePowerBeam
        {
            get
            {
                if (CPlayerState == 0)
                    return false;
                return GCMem.ReadInt32(CPlayerState + OFF_POWERBEAM_OBTAINED) > 0;
            }
        }

        protected override bool HaveIceBeam
        {
            get
            {
                if (CPlayerState == 0)
                    return false;
                return GCMem.ReadInt32(CPlayerState + OFF_ICEBEAM_OBTAINED) > 0;
            }
        }

        protected override bool HaveWaveBeam
        {
            get
            {
                if (CPlayerState == 0)
                    return false;
                return GCMem.ReadInt32(CPlayerState + OFF_WAVEBEAM_OBTAINED) > 0;
            }
        }

        protected override bool HavePlasmaBeam
        {
            get
            {
                if (CPlayerState == 0)
                    return false;
                return GCMem.ReadInt32(CPlayerState + OFF_PLASMABEAM_OBTAINED) > 0;
            }
        }

        protected override bool HaveMorphBallBombs
        {
            get
            {
                if (CPlayerState == 0)
                    return false;
                return GCMem.ReadInt32(CPlayerState + OFF_MORPHBALLBOMBS_OBTAINED) > 0;
            }
        }

        protected override bool HaveFlamethrower
        {
            get
            {
                if (CPlayerState == 0)
                    return false;
                return GCMem.ReadInt32(CPlayerState + OFF_FLAMETHROWER_OBTAINED) > 0;
            }
        }

        protected override bool HaveScanVisor
        {
            get
            {
                if (CPlayerState == 0)
                    return false;
                return GCMem.ReadInt32(CPlayerState + OFF_SCANVISOR_OBTAINED) > 0;
            }
        }

        protected override bool HaveThermalVisor
        {
            get
            {
                if (CPlayerState == 0)
                    return false;
                return GCMem.ReadInt32(CPlayerState + OFF_THERMALVISOR_OBTAINED) > 0;
            }
        }

        protected override bool HaveChargeBeam
        {
            get
            {
                if (CPlayerState == 0)
                    return false;
                return GCMem.ReadInt32(CPlayerState + OFF_CHARGEBEAM_OBTAINED) > 0;
            }
        }

        protected override bool HaveSuperMissile
        {
            get
            {
                if (CPlayerState == 0)
                    return false;
                return GCMem.ReadInt32(CPlayerState + OFF_SUPERMISSILE_OBTAINED) > 0;
            }
        }

        protected override bool HaveGrappleBeam
        {
            get
            {
                if (CPlayerState == 0)
                    return false;
                return GCMem.ReadInt32(CPlayerState + OFF_GRAPPLEBEAM_OBTAINED) > 0;
            }
        }

        protected override bool HaveXRayVisor
        {
            get
            {
                if (CPlayerState == 0)
                    return false;
                return GCMem.ReadInt32(CPlayerState + OFF_XRAYVISOR_OBTAINED) > 0;
            }
        }

        protected override bool HaveIceSpreader
        {
            get
            {
                if (CPlayerState == 0)
                    return false;
                return GCMem.ReadInt32(CPlayerState + OFF_ICESPREADER_OBTAINED) > 0;
            }
        }

        protected override bool HaveSpaceJumpBoots
        {
            get
            {
                if (CPlayerState == 0)
                    return false;
                return GCMem.ReadInt32(CPlayerState + OFF_SPACEJUMPBOOTS_OBTAINED) > 0;
            }
        }

        protected override bool HaveMorphBall
        {
            get
            {
                if (CPlayerState == 0)
                    return false;
                return GCMem.ReadInt32(CPlayerState + OFF_MORPHBALL_OBTAINED) > 0;
            }
        }

        protected override bool HaveBoostBall
        {
            get
            {
                if (CPlayerState == 0)
                    return false;
                return GCMem.ReadInt32(CPlayerState + OFF_BOOSTBALL_OBTAINED) > 0;
            }
        }

        protected override bool HaveSpiderBall
        {
            get
            {
                if (CPlayerState == 0)
                    return false;
                return GCMem.ReadInt32(CPlayerState + OFF_SPIDERBALL_OBTAINED) > 0;
            }
        }

        protected override bool HaveGravitySuit
        {
            get
            {
                if (CPlayerState == 0)
                    return false;
                return GCMem.ReadInt32(CPlayerState + OFF_GRAVITYSUIT_OBTAINED) > 0;
            }
        }

        protected override bool HaveVariaSuit
        {
            get
            {
                if (CPlayerState == 0)
                    return false;
                return GCMem.ReadInt32(CPlayerState + OFF_VARIASUIT_OBTAINED) > 0;
            }
        }

        protected override bool HavePhazonSuit
        {
            get
            {
                if (CPlayerState == 0)
                    return false;
                return GCMem.ReadInt32(CPlayerState + OFF_PHAZONSUIT_OBTAINED) > 0;
            }
        }

        protected override bool HaveWavebuster
        {
            get
            {
                if (CPlayerState == 0)
                    return false;
                return GCMem.ReadInt32(CPlayerState + OFF_WAVEBUSTER_OBTAINED) > 0;
            }
        }

        protected override bool Artifacts(int index)
        {
            if (CPlayerState == 0)
                return false;
            if (index < 0)
                throw new Exception("Index can't be negative");
            switch (index)
            {
                case 0:
                    return GCMem.ReadInt32(CPlayerState + OFF_ARTIFACT_OF_TRUTH_OBTAINED) > 0;
                case 1:
                    return GCMem.ReadInt32(CPlayerState + OFF_ARTIFACT_OF_STRENGTH_OBTAINED) > 0;
                case 2:
                    return GCMem.ReadInt32(CPlayerState + OFF_ARTIFACT_OF_ELDER_OBTAINED) > 0;
                case 3:
                    return GCMem.ReadInt32(CPlayerState + OFF_ARTIFACT_OF_WILD_OBTAINED) > 0;
                case 4:
                    return GCMem.ReadInt32(CPlayerState + OFF_ARTIFACT_OF_LIFEGIVER_OBTAINED) > 0;
                case 5:
                    return GCMem.ReadInt32(CPlayerState + OFF_ARTIFACT_OF_WARRIOR_OBTAINED) > 0;
                case 6:
                    return GCMem.ReadInt32(CPlayerState + OFF_ARTIFACT_OF_CHOZO_OBTAINED) > 0;
                case 7:
                    return GCMem.ReadInt32(CPlayerState + OFF_ARTIFACT_OF_NATURE_OBTAINED) > 0;
                case 8:
                    return GCMem.ReadInt32(CPlayerState + OFF_ARTIFACT_OF_SUN_OBTAINED) > 0;
                case 9:
                    return GCMem.ReadInt32(CPlayerState + OFF_ARTIFACT_OF_WORLD_OBTAINED) > 0;
                case 10:
                    return GCMem.ReadInt32(CPlayerState + OFF_ARTIFACT_OF_SPIRIT_OBTAINED) > 0;
                case 11:
                    return GCMem.ReadInt32(CPlayerState + OFF_ARTIFACT_OF_NEWBORN_OBTAINED) > 0;
                default:
                    throw new Exception("There are no artifacts past the 12th artifact");
            }
        }

        protected override bool HaveUnlimitedMissiles
        {
            get
            {
                if (!IsCustomItemsPatchEnabled)
                    return false;
                if (CPlayerState == 0)
                    return false;
                var value = Unk2ToCustomItem("Unlimited Missiles");
                return (GCMem.ReadInt32(CPlayerState + OFF_UNKNOWN_ITEM_2_OBTAINED) & value) == value;
            }
        }

        protected override bool HaveUnlimitedPowerBombs
        {
            get
            {
                if (!IsCustomItemsPatchEnabled)
                    return false;
                if (CPlayerState == 0)
                    return false;
                var value = Unk2ToCustomItem("Unlimited Power Bombs");
                return (GCMem.ReadInt32(CPlayerState + OFF_UNKNOWN_ITEM_2_OBTAINED) & value) == value;
            }
        }

        protected override bool HavePowerBombLauncher
        {
            get
            {
                if (!IsCustomItemsPatchEnabled)
                    return true;
                if (CPlayerState == 0)
                    return false;
                var value = Unk2ToCustomItem("Power Bomb Launcher");
                return (GCMem.ReadInt32(CPlayerState + OFF_UNKNOWN_ITEM_2_OBTAINED) & value) == value;
            }
        }

        protected override bool HaveMissileLauncher
        {
            get
            {
                if (!IsCustomItemsPatchEnabled)
                    return true;
                if (CPlayerState == 0)
                    return false;
                var value = Unk2ToCustomItem("Missile Launcher");
                return (GCMem.ReadInt32(CPlayerState + OFF_UNKNOWN_ITEM_2_OBTAINED) & value) == value;
            }
        }

        protected override int ProgressivePowerBeam
        {
            get
            {
                var result = 0;
                if (CPlayerState == 0)
                    return result;
                result += GCMem.ReadInt32(CPlayerState + OFF_POWERBEAM_OBTAINED);
                result += GCMem.ReadInt32(CPlayerState + OFF_SUPERMISSILE_OBTAINED);
                return result;
            }
        }

        protected override int ProgressiveWaveBeam
        {
            get
            {
                var result = 0;
                if (CPlayerState == 0)
                    return result;
                result += GCMem.ReadInt32(CPlayerState + OFF_WAVEBEAM_OBTAINED);
                result += GCMem.ReadInt32(CPlayerState + OFF_WAVEBUSTER_OBTAINED);
                return result;
            }
        }

        protected override int ProgressiveIceBeam
        {
            get
            {
                var result = 0;
                if (CPlayerState == 0)
                    return result;
                result += GCMem.ReadInt32(CPlayerState + OFF_ICEBEAM_OBTAINED);
                result += GCMem.ReadInt32(CPlayerState + OFF_ICESPREADER_OBTAINED);
                return result;
            }
        }

        protected override int ProgressivePlasmaBeam
        {
            get
            {
                var result = 0;
                if (CPlayerState == 0)
                    return result;
                result += GCMem.ReadInt32(CPlayerState + OFF_PLASMABEAM_OBTAINED);
                result += GCMem.ReadInt32(CPlayerState + OFF_FLAMETHROWER_OBTAINED);
                return result;
            }
        }

        protected override bool IsProgressiveBeamEnabled
        {
            get
            {
                for (var i = 0; i < 4; i++)
                    if (GCMem.ReadInt32(OFF_CPLAYERSTATE_POWERUPS_MAXCAPACITY + i * 4) != 2)
                        return false;
                return true;
            }
        }

        protected override bool IsCustomItemsPatchEnabled => GCMem.ReadUInt32(OFF_CPLAYERSTATE_GETITEMAMOUNT) != 0x2c040000;
    }
}
