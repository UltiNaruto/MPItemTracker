using System;

namespace Wrapper.Echoes
{
    class MP2_NTSC_J : Echoes
    {
        protected const long OFF_CSTATEMANAGER = 0x803DE690;
        protected const long OFF_CGAMEGLOBALOBJECTS = 0x803C8B10;
        protected const long OFF_CGAMESTATE = OFF_CGAMEGLOBALOBJECTS + 0x134;

        protected override long CPlayer
        {
            get
            {
                long CPlayer_Array = GCMem.ReadUInt32(OFF_CSTATEMANAGER + OFF_CPLAYER);
                if (CPlayer_Array == 0)
                    return 0;
                return GCMem.ReadUInt32(CPlayer_Array); // CPlayer[0]
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
                return GCMem.ReadUInt32(OFF_CSTATEMANAGER + OFF_CPLAYERSTATE);
            }
        }

        protected override long _IGT
        {
            get
            {
                if (CGameState == 0)
                    return 0;
                return (long)(GCMem.ReadFloat64(CGameState + OFF_PLAYTIME) * 1000.0);
            }
        }

        protected override bool _IsMorphed
        {
            get
            {
                if (CPlayer == 0)
                    return false;
                return GCMem.ReadUInt32(CPlayer + OFF_CPLAYER_MORPHSTATE) == 1;
            }
        }

        protected override bool _IsSwitchingState
        {
            get
            {
                if (CPlayer == 0)
                    return true;
                return GCMem.ReadUInt32(CPlayer + OFF_CPLAYER_MORPHSTATE) > 1;
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
                return GCMem.ReadInt32(CPlayerState + OFF_ENERGYTANKS);
            }
        }

        protected override int MaxDarkAmmo
        {
            get
            {
                if (CPlayerState == 0)
                    return 0;
                return GCMem.ReadInt32(CPlayerState + OFF_DARKBEAM_AMMO);
            }
        }

        protected override int MaxLightAmmo
        {
            get
            {
                if (CPlayerState == 0)
                    return 0;
                return GCMem.ReadInt32(CPlayerState + OFF_LIGHTBEAM_AMMO);
            }
        }

        protected override bool HaveDarkBeam
        {
            get
            {
                if (CPlayerState == 0)
                    return false;
                return GCMem.ReadUInt32(CPlayerState + OFF_DARKBEAM_OBTAINED) > 0;
            }
        }

        protected override bool HaveLightBeam
        {
            get
            {
                if (CPlayerState == 0)
                    return false;
                return GCMem.ReadUInt32(CPlayerState + OFF_LIGHTBEAM_OBTAINED) > 0;
            }
        }

        protected override bool HaveAnnihilatorBeam
        {
            get
            {
                if (CPlayerState == 0)
                    return false;
                return GCMem.ReadUInt32(CPlayerState + OFF_ANNIHILATORBEAM_OBTAINED) > 0;
            }
        }

        protected override bool HaveMorphBallBombs
        {
            get
            {
                if (CPlayerState == 0)
                    return false;
                return GCMem.ReadUInt32(CPlayerState + OFF_MORPHBALLBOMBS_OBTAINED) > 0;
            }
        }

        protected override bool HaveSonicBoom
        {
            get
            {
                if (CPlayerState == 0)
                    return false;
                return GCMem.ReadUInt32(CPlayerState + OFF_SONICBOOM_OBTAINED) > 0;
            }
        }

        protected override bool HaveDarkVisor
        {
            get
            {
                if (CPlayerState == 0)
                    return false;
                return GCMem.ReadUInt32(CPlayerState + OFF_DARKVISOR_OBTAINED) > 0;
            }
        }

        protected override bool HaveChargeBeam
        {
            get
            {
                if (CPlayerState == 0)
                    return false;
                return GCMem.ReadUInt32(CPlayerState + OFF_CHARGEBEAM_OBTAINED) > 0;
            }
        }

        protected override bool HaveSeekerLauncher
        {
            get
            {
                if (CPlayerState == 0)
                    return false;
                return GCMem.ReadUInt32(CPlayerState + OFF_SEEKERLAUNCHER_OBTAINED) > 0;
            }
        }

        protected override bool HaveSuperMissile
        {
            get
            {
                if (CPlayerState == 0)
                    return false;
                return GCMem.ReadUInt32(CPlayerState + OFF_SUPERMISSILE_OBTAINED) > 0;
            }
        }

        protected override bool HaveGrappleBeam
        {
            get
            {
                if (CPlayerState == 0)
                    return false;
                return GCMem.ReadUInt32(CPlayerState + OFF_GRAPPLEBEAM_OBTAINED) > 0;
            }
        }

        protected override bool HaveScrewAttack
        {
            get
            {
                if (CPlayerState == 0)
                    return false;
                return GCMem.ReadUInt32(CPlayerState + OFF_SCREWATTACK_OBTAINED) > 0;
            }
        }

        protected override bool HaveEchoVisor
        {
            get
            {
                if (CPlayerState == 0)
                    return false;
                return GCMem.ReadUInt32(CPlayerState + OFF_ECHOVISOR_OBTAINED) > 0;
            }
        }

        protected override bool HaveSunburst
        {
            get
            {
                if (CPlayerState == 0)
                    return false;
                return GCMem.ReadUInt32(CPlayerState + OFF_SUNBURST_OBTAINED) > 0;
            }
        }

        protected override bool HaveSpaceJumpBoots
        {
            get
            {
                if (CPlayerState == 0)
                    return false;
                return GCMem.ReadUInt32(CPlayerState + OFF_SPACEJUMPBOOTS_OBTAINED) > 0;
            }
        }

        protected override bool HaveMorphBall
        {
            get
            {
                if (CPlayerState == 0)
                    return false;
                return GCMem.ReadUInt32(CPlayerState + OFF_MORPHBALL_OBTAINED) > 0;
            }
        }

        protected override bool HaveBoostBall
        {
            get
            {
                if (CPlayerState == 0)
                    return false;
                return GCMem.ReadUInt32(CPlayerState + OFF_BOOSTBALL_OBTAINED) > 0;
            }
        }

        protected override bool HaveSpiderBall
        {
            get
            {
                if (CPlayerState == 0)
                    return false;
                return GCMem.ReadUInt32(CPlayerState + OFF_SPIDERBALL_OBTAINED) > 0;
            }
        }

        protected override bool HaveLightSuit
        {
            get
            {
                if (CPlayerState == 0)
                    return false;
                return GCMem.ReadUInt32(CPlayerState + OFF_LIGHTSUIT_OBTAINED) > 0;
            }
        }

        protected override bool HaveDarkSuit
        {
            get
            {
                if (CPlayerState == 0)
                    return false;
                return GCMem.ReadUInt32(CPlayerState + OFF_DARKSUIT_OBTAINED) > 0;
            }
        }

        protected override bool HaveGravityBoost
        {
            get
            {
                if (CPlayerState == 0)
                    return false;
                return GCMem.ReadUInt32(CPlayerState + OFF_GRAVITYBOOST_OBTAINED) > 0;
            }
        }

        protected override bool HaveScanVisor
        {
            get
            {
                if (CPlayerState == 0)
                    return false;
                return GCMem.ReadUInt32(CPlayerState + OFF_SCANVISOR_OBTAINED) > 0;
            }
        }

        protected override bool HaveDarkburst
        {
            get
            {
                if (CPlayerState == 0)
                    return false;
                return GCMem.ReadUInt32(CPlayerState + OFF_DARKBURST_OBTAINED) > 0;
            }
        }

        protected override bool HaveVioletTranslator
        {
            get
            {
                if (CPlayerState == 0)
                    return false;
                return GCMem.ReadUInt32(CPlayerState + OFF_VIOLET_TRANSLATOR_OBTAINED) > 0;
            }
        }

        protected override bool HaveAmberTranslator
        {
            get
            {
                if (CPlayerState == 0)
                    return false;
                return GCMem.ReadUInt32(CPlayerState + OFF_AMBER_TRANSLATOR_OBTAINED) > 0;
            }
        }

        protected override bool HaveEmeraldTranslator
        {
            get
            {
                if (CPlayerState == 0)
                    return false;
                return GCMem.ReadUInt32(CPlayerState + OFF_EMERALD_TRANSLATOR_OBTAINED) > 0;
            }
        }

        protected override bool HaveCobaltTranslator
        {
            get
            {
                if (CPlayerState == 0)
                    return false;
                return GCMem.ReadUInt32(CPlayerState + OFF_COBALT_TRANSLATOR_OBTAINED) > 0;
            }
        }

        protected override bool HaveEnergyTransferModule
        {
            get
            {
                if (CPlayerState == 0)
                    return false;
                return GCMem.ReadUInt32(CPlayerState + OFF_ENERGY_TRANSFER_MODULE_OBTAINED) > 0;
            }
        }

        protected override bool DarkAgonKeys(int index)
        {
            if (CPlayerState == 0)
                return false;
            if (index < 0)
                throw new Exception("Index can't be negative");
            switch (index)
            {
                case 0:
                    return GCMem.ReadUInt32(CPlayerState + OFF_DARK_AGON_KEY_1_OBTAINED) > 0;
                case 1:
                    return GCMem.ReadUInt32(CPlayerState + OFF_DARK_AGON_KEY_2_OBTAINED) > 0;
                case 2:
                    return GCMem.ReadUInt32(CPlayerState + OFF_DARK_AGON_KEY_3_OBTAINED) > 0;
                default:
                    throw new Exception("There are no 4th Dark Agon key");
            }
        }

        protected override bool DarkTorvusKeys(int index)
        {
            if (CPlayerState == 0)
                return false;
            if (index < 0)
                throw new Exception("Index can't be negative");
            switch (index)
            {
                case 0:
                    return GCMem.ReadUInt32(CPlayerState + OFF_DARK_TORVUS_KEY_1_OBTAINED) > 0;
                case 1:
                    return GCMem.ReadUInt32(CPlayerState + OFF_DARK_TORVUS_KEY_2_OBTAINED) > 0;
                case 2:
                    return GCMem.ReadUInt32(CPlayerState + OFF_DARK_TORVUS_KEY_3_OBTAINED) > 0;
                default:
                    throw new Exception("There are no 4th Dark Torvus key");
            }
        }

        protected override bool IngHiveKeys(int index)
        {
            if (CPlayerState == 0)
                return false;
            if (index < 0)
                throw new Exception("Index can't be negative");
            switch (index)
            {
                case 0:
                    return GCMem.ReadUInt32(CPlayerState + OFF_ING_HIVE_KEY_1_OBTAINED) > 0;
                case 1:
                    return GCMem.ReadUInt32(CPlayerState + OFF_ING_HIVE_KEY_2_OBTAINED) > 0;
                case 2:
                    return GCMem.ReadUInt32(CPlayerState + OFF_ING_HIVE_KEY_3_OBTAINED) > 0;
                default:
                    throw new Exception("There are no 4th Ing Hive key");
            }
        }

        protected override bool SkyTempleKeys(int index)
        {
            if (CPlayerState == 0)
                return false;
            if (index < 0)
                throw new Exception("Index can't be negative");
            switch (index)
            {
                case 0:
                    return GCMem.ReadUInt32(CPlayerState + OFF_SKY_TEMPLE_KEY_1_OBTAINED) > 0;
                case 1:
                    return GCMem.ReadUInt32(CPlayerState + OFF_SKY_TEMPLE_KEY_2_OBTAINED) > 0;
                case 2:
                    return GCMem.ReadUInt32(CPlayerState + OFF_SKY_TEMPLE_KEY_3_OBTAINED) > 0;
                case 3:
                    return GCMem.ReadUInt32(CPlayerState + OFF_SKY_TEMPLE_KEY_4_OBTAINED) > 0;
                case 4:
                    return GCMem.ReadUInt32(CPlayerState + OFF_SKY_TEMPLE_KEY_5_OBTAINED) > 0;
                case 5:
                    return GCMem.ReadUInt32(CPlayerState + OFF_SKY_TEMPLE_KEY_6_OBTAINED) > 0;
                case 6:
                    return GCMem.ReadUInt32(CPlayerState + OFF_SKY_TEMPLE_KEY_7_OBTAINED) > 0;
                case 7:
                    return GCMem.ReadUInt32(CPlayerState + OFF_SKY_TEMPLE_KEY_8_OBTAINED) > 0;
                case 8:
                    return GCMem.ReadUInt32(CPlayerState + OFF_SKY_TEMPLE_KEY_9_OBTAINED) > 0;
                default:
                    throw new Exception("There are no 10th Sky Temple key");
            }
        }
    }
}
