namespace Prime.Memory.Constants
{
    internal abstract class _MP1
    {
        internal const long OFF_PLAYTIME = 0xA0;
        internal const long OFF_CPLAYER = 0x84C;
        internal const long OFF_CPLAYER_MORPHSTATE = 0x304;
        internal const long OFF_CWORLD = 0x850;
        internal const long OFF_CWORLD_MREAID = 0x68;
        internal const long OFF_CWORLD_MLVLID = 0x6C;
        internal const long OFF_CWORLD_MORPHSTATE = 0x2F0;
        internal const long OFF_CPLAYERSTATE = 0x8B8;
        internal const long OFF_HEALTH = 0x0C;
        internal const long OFF_CRITICAL_HEALTH = OFF_HEALTH + 4;
        internal const long OFF_POWERBEAM_OBTAINED = 0x2C;
        internal const long OFF_ICEBEAM_OBTAINED = 0x34;
        internal const long OFF_WAVEBEAM_OBTAINED = 0x3C;
        internal const long OFF_PLASMABEAM_OBTAINED = 0x44;
        internal const long OFF_MISSILES = 0x48;
        internal const long OFF_MAX_MISSILES = OFF_MISSILES + 4;
        internal const long OFF_SCANVISOR_OBTAINED = 0x54;
        internal const long OFF_MORPHBALLBOMBS_OBTAINED = 0x5C;
        internal const long OFF_POWERBOMBS = 0x60;
        internal const long OFF_MAX_POWERBOMBS = OFF_POWERBOMBS + 4;
        internal const long OFF_FLAMETHROWER_OBTAINED = 0x6C;
        internal const long OFF_THERMALVISOR_OBTAINED = 0x74;
        internal const long OFF_CHARGEBEAM_OBTAINED = 0x7C;
        internal const long OFF_SUPERMISSILE_OBTAINED = 0x84;
        internal const long OFF_GRAPPLEBEAM_OBTAINED = 0x8C;
        internal const long OFF_XRAYVISOR_OBTAINED = 0x94;
        internal const long OFF_ICESPREADER_OBTAINED = 0x9C;
        internal const long OFF_SPACEJUMPBOOTS_OBTAINED = 0xA4;
        internal const long OFF_MORPHBALL_OBTAINED = 0xAC;
        internal const long OFF_COMBATVISOR_OBTAINED = 0xB4;
        internal const long OFF_BOOSTBALL_OBTAINED = 0xBC;
        internal const long OFF_SPIDERBALL_OBTAINED = 0xC4;
        internal const long OFF_POWERSUIT_OBTAINED = 0xCC;
        internal const long OFF_GRAVITYSUIT_OBTAINED = 0xD4;
        internal const long OFF_VARIASUIT_OBTAINED = 0xDC;
        internal const long OFF_PHAZONSUIT_OBTAINED = 0xE4;
        internal const long OFF_ENERGYTANKS_OBTAINED = 0xEC;
        internal const long OFF_ENERGYREFILL_OBTAINED = 0xFC;
        internal const long OFF_WAVEBUSTER_OBTAINED = 0x10C;
        internal const long OFF_ARTIFACT_OF_TRUTH_OBTAINED = 0x114;
        internal const long OFF_ARTIFACT_OF_STRENGTH_OBTAINED = 0x11C;
        internal const long OFF_ARTIFACT_OF_ELDER_OBTAINED = 0x124;
        internal const long OFF_ARTIFACT_OF_WILD_OBTAINED = 0x12C;
        internal const long OFF_ARTIFACT_OF_LIFEGIVER_OBTAINED = 0x134;
        internal const long OFF_ARTIFACT_OF_WARRIOR_OBTAINED = 0x13C;
        internal const long OFF_ARTIFACT_OF_CHOZO_OBTAINED = 0x144;
        internal const long OFF_ARTIFACT_OF_NATURE_OBTAINED = 0x14C;
        internal const long OFF_ARTIFACT_OF_SUN_OBTAINED = 0x154;
        internal const long OFF_ARTIFACT_OF_WORLD_OBTAINED = 0x15C;
        internal const long OFF_ARTIFACT_OF_SPIRIT_OBTAINED = 0x164;
        internal const long OFF_ARTIFACT_OF_NEWBORN_OBTAINED = 0x16C;

        internal abstract long CPlayer { get; }
        internal abstract long CWorld { get; }
        internal abstract long CGameState { get; }
        internal abstract long CPlayerState { get; }
        internal abstract long IGT { get; }
        internal abstract string IGTAsStr { get; }

        internal abstract bool IsMorphed { get; }
        internal abstract bool IsSwitchingState { get; }

        internal abstract int MaxMissiles { get; }
        internal abstract int MaxPowerBombs { get; }

        internal abstract bool HaveIceBeam { get; }
        internal abstract bool HaveWaveBeam { get; }
        internal abstract bool HavePlasmaBeam { get; }
        internal abstract bool HaveMissiles { get; }
        internal abstract bool HaveMorphBallBombs { get; }
        internal abstract bool HavePowerBombs { get; }
        internal abstract bool HaveFlamethrower { get; }
        internal abstract bool HaveThermalVisor { get; }
        internal abstract bool HaveChargeBeam { get; }
        internal abstract bool HaveSuperMissile { get; }
        internal abstract bool HaveGrappleBeam { get; }
        internal abstract bool HaveXRayVisor { get; }
        internal abstract bool HaveIceSpreader { get; }
        internal abstract bool HaveSpaceJumpBoots { get; }
        internal abstract bool HaveMorphBall { get; }
        internal abstract bool HaveBoostBall { get; }
        internal abstract bool HaveSpiderBall { get; }
        internal abstract bool HaveGravitySuit { get; }
        internal abstract bool HaveVariaSuit { get; }
        internal abstract bool HavePhazonSuit { get; }
        internal abstract bool HaveWavebuster { get; }
        internal abstract bool Artifacts(int index);
    }
}