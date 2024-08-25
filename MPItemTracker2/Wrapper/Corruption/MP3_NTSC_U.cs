using System;

namespace Wrapper.Corruption
{
    class MP3_NTSC_U : Corruption
    {
        protected const long OFF_CSTATEMANAGER = 0x805C4F98;
        protected const long OFF_CGAMEGLOBALOBJECTS = 0x805C7C4C;
        protected const long OFF_CGAMESTATE = OFF_CGAMEGLOBALOBJECTS + 0x134;

        protected override long CPlayer
        {
            get
            {
                long CStateManager = GCMem.ReadUInt32(OFF_CSTATEMANAGER);
                if (CStateManager == 0)
                    return 0;
                return GCMem.ReadUInt32(CStateManager + OFF_CPLAYER);
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
                if (CPlayer == 0)
                    return 0;
                return GCMem.ReadUInt32(CPlayer + OFF_CPLAYERSTATE);
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

        protected override int MaxEnergyTanks
        {
            get
            {
                if (CPlayerState == 0)
                    return 0;
                return GCMem.ReadInt32(CPlayerState + OFF_MAX_ENERGYTANKS);
            }
        }

        protected override int MaxShipMissiles
        {
            get
            {
                if (CPlayerState == 0)
                    return 0;
                return GCMem.ReadInt32(CPlayerState + OFF_MAX_SHIP_MISSILES);
            }
        }

        protected override bool HavePlasmaBeam
        {
            get
            {
                if (CPlayerState == 0)
                    return false;
                return GCMem.ReadUInt32(CPlayerState + OFF_PLASMABEAM_OBTAINED) > 0;
            }
        }

        protected override bool HaveNovaBeam
        {
            get
            {
                if (CPlayerState == 0)
                    return false;
                return GCMem.ReadUInt32(CPlayerState + OFF_NOVABEAM_OBTAINED) > 0;
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

        protected override bool HaveShipGrappleBeam
        {
            get
            {
                if (CPlayerState == 0)
                    return false;
                return GCMem.ReadUInt32(CPlayerState + OFF_SHIP_GRAPPLEBEAM_OBTAINED) > 0;
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

        protected override bool HaveMorphBallBombs
        {
            get
            {
                if (CPlayerState == 0)
                    return false;
                return GCMem.ReadUInt32(CPlayerState + OFF_MORPHBALLBOMBS_OBTAINED) > 0;
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

        protected override bool HaveHyperBall
        {
            get
            {
                if (CPlayerState == 0)
                    return false;
                return GCMem.ReadUInt32(CPlayerState + OFF_HYPERBALL_OBTAINED) > 0;
            }
        }

        protected override bool HaveIceMissile
        {
            get
            {
                if (CPlayerState == 0)
                    return false;
                return GCMem.ReadUInt32(CPlayerState + OFF_ICEMISSILE_OBTAINED) > 0;
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

        protected override bool HaveHyperMissile
        {
            get
            {
                if (CPlayerState == 0)
                    return false;
                return GCMem.ReadUInt32(CPlayerState + OFF_HYPERMISSILE_OBTAINED) > 0;
            }
        }

        protected override bool HaveGrappleLasso
        {
            get
            {
                if (CPlayerState == 0)
                    return false;
                return GCMem.ReadUInt32(CPlayerState + OFF_GRAPPLELASSO_OBTAINED) > 0;
            }
        }

        protected override bool HaveGrappleSwing
        {
            get
            {
                if (CPlayerState == 0)
                    return false;
                return GCMem.ReadUInt32(CPlayerState + OFF_GRAPPLESWING_OBTAINED) > 0;
            }
        }

        protected override bool HaveGrappleVoltage
        {
            get
            {
                if (CPlayerState == 0)
                    return false;
                return GCMem.ReadUInt32(CPlayerState + OFF_GRAPPLEVOLTAGE_OBTAINED) > 0;
            }
        }

        protected override bool HaveHyperGrapple
        {
            get
            {
                if (CPlayerState == 0)
                    return false;
                return GCMem.ReadUInt32(CPlayerState + OFF_HYPERGRAPPLE_OBTAINED) > 0;
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

        protected override bool HaveScanVisor
        {
            get
            {
                if (CPlayerState == 0)
                    return false;
                return GCMem.ReadUInt32(CPlayerState + OFF_SCANVISOR_OBTAINED) > 0;
            }
        }

        protected override bool HaveCommandVisor
        {
            get
            {
                if (CPlayerState == 0)
                    return false;
                return GCMem.ReadUInt32(CPlayerState + OFF_COMMANDVISOR_OBTAINED) > 0;
            }
        }

        protected override bool HaveXRayVisor
        {
            get
            {
                if (CPlayerState == 0)
                    return false;
                return GCMem.ReadUInt32(CPlayerState + OFF_XRAYVISOR_OBTAINED) > 0;
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

        protected override bool HaveHypermode
        {
            get
            {
                if (CPlayerState == 0)
                    return false;
                return GCMem.ReadUInt32(CPlayerState + OFF_HYPERMODE_OBTAINED) > 0;
            }
        }

        protected override bool HaveHazardShield
        {
            get
            {
                if (CPlayerState == 0)
                    return false;
                return GCMem.ReadUInt32(CPlayerState + OFF_SUITTYPE_OBTAINED) >= 5;
            }
        }

        protected override bool EnergyCells(int index)
        {
            if (CPlayerState == 0)
                return false;
            if (index < 0)
                throw new Exception("Index can't be negative");
            switch (index)
            {
                case 0:
                    return GCMem.ReadUInt32(CPlayerState + OFF_ENERGY_CELL_1_OBTAINED) > 0 || GCMem.ReadUInt32(CPlayerState + OFF_ENERGY_CELL_1_USED) > 0;
                case 1:
                    return GCMem.ReadUInt32(CPlayerState + OFF_ENERGY_CELL_2_OBTAINED) > 0 || GCMem.ReadUInt32(CPlayerState + OFF_ENERGY_CELL_2_USED) > 0;
                case 2:
                    return GCMem.ReadUInt32(CPlayerState + OFF_ENERGY_CELL_3_OBTAINED) > 0 || GCMem.ReadUInt32(CPlayerState + OFF_ENERGY_CELL_3_USED) > 0;
                case 3:
                    return GCMem.ReadUInt32(CPlayerState + OFF_ENERGY_CELL_4_OBTAINED) > 0 || GCMem.ReadUInt32(CPlayerState + OFF_ENERGY_CELL_4_USED) > 0;
                case 4:
                    return GCMem.ReadUInt32(CPlayerState + OFF_ENERGY_CELL_5_OBTAINED) > 0 || GCMem.ReadUInt32(CPlayerState + OFF_ENERGY_CELL_5_USED) > 0;
                case 5:
                    return GCMem.ReadUInt32(CPlayerState + OFF_ENERGY_CELL_6_OBTAINED) > 0 || GCMem.ReadUInt32(CPlayerState + OFF_ENERGY_CELL_6_USED) > 0;
                case 6:
                    return GCMem.ReadUInt32(CPlayerState + OFF_ENERGY_CELL_7_OBTAINED) > 0 || GCMem.ReadUInt32(CPlayerState + OFF_ENERGY_CELL_7_USED) > 0;
                case 7:
                    return GCMem.ReadUInt32(CPlayerState + OFF_ENERGY_CELL_8_OBTAINED) > 0 || GCMem.ReadUInt32(CPlayerState + OFF_ENERGY_CELL_8_USED) > 0;
                case 8:
                    return GCMem.ReadUInt32(CPlayerState + OFF_ENERGY_CELL_9_OBTAINED) > 0 || GCMem.ReadUInt32(CPlayerState + OFF_ENERGY_CELL_9_USED) > 0;
                default:
                    throw new Exception("There are no 10th Energy Cell");
            }
        }
    }
}
