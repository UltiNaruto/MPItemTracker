using MPItemTracker2;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text.Json;
using Utils;

namespace Wrapper.Prime
{
    public class Prime : Metroid
    {
        protected const long OFF_PLAYTIME = 0xA0;
        protected const long OFF_CPLAYER = 0x84C;
        protected const long OFF_CPLAYER_MORPHSTATE = 0x2F4;
        protected const long OFF_CWORLD = 0x850;
        protected const long OFF_CWORLD_MREAID = 0x68;
        protected const long OFF_CWORLD_MLVLID = 0x6C;
        protected const long OFF_CWORLD_MORPHSTATE = 0x2F0;
        protected const long OFF_CPLAYERSTATE = 0x8B8;
        protected const long OFF_HEALTH = 0x0C;
        protected const long OFF_CRITICAL_HEALTH = OFF_HEALTH + 4;
        protected const long OFF_POWERBEAM_OBTAINED = 0x2C;
        protected const long OFF_ICEBEAM_OBTAINED = 0x34;
        protected const long OFF_WAVEBEAM_OBTAINED = 0x3C;
        protected const long OFF_PLASMABEAM_OBTAINED = 0x44;
        protected const long OFF_MISSILES = 0x48;
        protected const long OFF_MAX_MISSILES = OFF_MISSILES + 4;
        protected const long OFF_SCANVISOR_OBTAINED = 0x54;
        protected const long OFF_MORPHBALLBOMBS_OBTAINED = 0x5C;
        protected const long OFF_POWERBOMBS = 0x60;
        protected const long OFF_MAX_POWERBOMBS = OFF_POWERBOMBS + 4;
        protected const long OFF_FLAMETHROWER_OBTAINED = 0x6C;
        protected const long OFF_THERMALVISOR_OBTAINED = 0x74;
        protected const long OFF_CHARGEBEAM_OBTAINED = 0x7C;
        protected const long OFF_SUPERMISSILE_OBTAINED = 0x84;
        protected const long OFF_GRAPPLEBEAM_OBTAINED = 0x8C;
        protected const long OFF_XRAYVISOR_OBTAINED = 0x94;
        protected const long OFF_ICESPREADER_OBTAINED = 0x9C;
        protected const long OFF_SPACEJUMPBOOTS_OBTAINED = 0xA4;
        protected const long OFF_MORPHBALL_OBTAINED = 0xAC;
        protected const long OFF_COMBATVISOR_OBTAINED = 0xB4;
        protected const long OFF_BOOSTBALL_OBTAINED = 0xBC;
        protected const long OFF_SPIDERBALL_OBTAINED = 0xC4;
        protected const long OFF_POWERSUIT_OBTAINED = 0xCC;
        protected const long OFF_GRAVITYSUIT_OBTAINED = 0xD4;
        protected const long OFF_VARIASUIT_OBTAINED = 0xDC;
        protected const long OFF_PHAZONSUIT_OBTAINED = 0xE4;
        protected const long OFF_ENERGYTANKS_OBTAINED = 0xEC;
        protected const long OFF_ENERGYREFILL_OBTAINED = 0xFC;
        protected const long OFF_UNKNOWN_ITEM_2_OBTAINED = 0x104;
        protected const long OFF_WAVEBUSTER_OBTAINED = 0x10C;
        protected const long OFF_ARTIFACT_OF_TRUTH_OBTAINED = 0x114;
        protected const long OFF_ARTIFACT_OF_STRENGTH_OBTAINED = 0x11C;
        protected const long OFF_ARTIFACT_OF_ELDER_OBTAINED = 0x124;
        protected const long OFF_ARTIFACT_OF_WILD_OBTAINED = 0x12C;
        protected const long OFF_ARTIFACT_OF_LIFEGIVER_OBTAINED = 0x134;
        protected const long OFF_ARTIFACT_OF_WARRIOR_OBTAINED = 0x13C;
        protected const long OFF_ARTIFACT_OF_CHOZO_OBTAINED = 0x144;
        protected const long OFF_ARTIFACT_OF_NATURE_OBTAINED = 0x14C;
        protected const long OFF_ARTIFACT_OF_SUN_OBTAINED = 0x154;
        protected const long OFF_ARTIFACT_OF_WORLD_OBTAINED = 0x15C;
        protected const long OFF_ARTIFACT_OF_SPIRIT_OBTAINED = 0x164;
        protected const long OFF_ARTIFACT_OF_NEWBORN_OBTAINED = 0x16C;

        protected virtual long CPlayer { get; }
        protected virtual long CWorld { get; }
        protected virtual long CGameState { get; }
        protected virtual long CPlayerState { get; }
        protected virtual long _IGT { get; }

        protected virtual bool _IsMorphed { get; }
        protected virtual bool _IsSwitchingState { get; }

        protected virtual int MaxMissiles { get; }
        protected virtual int MaxPowerBombs { get; }
        protected virtual int MaxEnergyTanks { get; }

        protected bool HaveEnergyTanks
        {
            get
            {
                return MaxEnergyTanks > 0;
            }
        }

        protected virtual bool HavePowerBeam { get; }
        protected virtual bool HaveIceBeam { get; }
        protected virtual bool HaveWaveBeam { get; }
        protected virtual bool HavePlasmaBeam { get; }

        protected bool HaveMissiles => HaveMissileLauncher && MaxMissiles > 0;

        protected virtual bool HaveMorphBallBombs { get; }

        protected bool HavePowerBombs => HavePowerBombLauncher && MaxPowerBombs > 0;

        protected virtual bool HaveFlamethrower { get; }
        protected virtual bool HaveScanVisor { get; }
        protected virtual bool HaveThermalVisor { get; }
        protected virtual bool HaveChargeBeam { get; }
        protected virtual bool HaveSuperMissile { get; }
        protected virtual bool HaveGrappleBeam { get; }
        protected virtual bool HaveXRayVisor { get; }
        protected virtual bool HaveIceSpreader { get; }
        protected virtual bool HaveSpaceJumpBoots { get; }
        protected virtual bool HaveMorphBall { get; }
        protected virtual bool HaveBoostBall { get; }
        protected virtual bool HaveSpiderBall { get; }
        protected virtual bool HaveGravitySuit { get; }
        protected virtual bool HaveVariaSuit { get; }
        protected virtual bool HavePhazonSuit { get; }
        protected virtual bool HaveWavebuster { get; }
        protected virtual bool Artifacts(int index) { return false; }
        protected virtual bool HaveUnlimitedMissiles { get; }
        protected virtual bool HaveUnlimitedPowerBombs { get; }
        protected virtual bool HaveMissileLauncher { get; }
        protected virtual bool HavePowerBombLauncher { get; }
        protected virtual int ProgressivePowerBeam { get; }
        protected virtual int ProgressiveWaveBeam { get; }
        protected virtual int ProgressiveIceBeam { get; }
        protected virtual int ProgressivePlasmaBeam { get; }
        protected virtual bool IsProgressiveBeamEnabled { get; }
        protected virtual bool IsCustomItemsPatchEnabled { get; }

        protected Dictionary<String, Image> img = new Dictionary<string, Image>();
        protected int missile_launcher_provided_ammo = 5;
        protected int missiles_per_expansion = 5;

        public Prime()
        {
            dynamic json = JsonSerializer.Deserialize<dynamic>(File.ReadAllText(Path.Combine(Program.ExecutableDir, "prime.json")));
            try {
                missile_launcher_provided_ammo = json.GetProperty("missiles_provided_ammo").GetInt32();
                missiles_per_expansion = json.GetProperty("missiles_per_expansion").GetInt32();
            } catch { }
            int outline_width = 2;
            var PrimeImagesPath = Path.Combine(Program.ExecutableDir, "img", "prime");
            img.Add("Energy Tanks", Image.FromFile(Path.Combine(PrimeImagesPath, "energytank.png")));
            img.Add("Missiles", Image.FromFile(Path.Combine(PrimeImagesPath, "missilelauncher.png")));
            img.Add("Morph Ball", Image.FromFile(Path.Combine(PrimeImagesPath, "morphball.png")));
            img.Add("Morph Ball Bombs", Image.FromFile(Path.Combine(PrimeImagesPath, "morphballbomb.png")));
            img.Add("Power Bombs", Image.FromFile(Path.Combine(PrimeImagesPath, "powerbomb.png")));
            img.Add("Boost Ball", Image.FromFile(Path.Combine(PrimeImagesPath, "boostball.png")));
            img.Add("Spider Ball", Image.FromFile(Path.Combine(PrimeImagesPath, "spiderball.png")));
            img.Add("Space Jump Boots", Image.FromFile(Path.Combine(PrimeImagesPath, "spacejumpboots.png")));
            img.Add("Varia Suit", Image.FromFile(Path.Combine(PrimeImagesPath, "variasuit.png")));
            img.Add("Gravity Suit", Image.FromFile(Path.Combine(PrimeImagesPath, "gravitysuit.png")));
            img.Add("Phazon Suit", Image.FromFile(Path.Combine(PrimeImagesPath, "phazonsuit.png")));
            img.Add("Power Beam", Image.FromFile(Path.Combine(PrimeImagesPath, "powerbeam.png")));
            img.Add("Wave Beam", Image.FromFile(Path.Combine(PrimeImagesPath, "wavebeam.png")));
            img.Add("Ice Beam", Image.FromFile(Path.Combine(PrimeImagesPath, "icebeam.png")));
            img.Add("Plasma Beam", Image.FromFile(Path.Combine(PrimeImagesPath, "plasmabeam.png")));
            img.Add("Charge Beam", Image.FromFile(Path.Combine(PrimeImagesPath, "chargebeam.png")));
            img.Add("Charge Beam (Power Beam)", Image.FromFile(Path.Combine(PrimeImagesPath, "chargebeam_power.png")));
            img.Add("Charge Beam (Wave Beam)", Image.FromFile(Path.Combine(PrimeImagesPath, "chargebeam_wave.png")));
            img.Add("Charge Beam (Ice Beam)", Image.FromFile(Path.Combine(PrimeImagesPath, "chargebeam_ice.png")));
            img.Add("Charge Beam (Plasma Beam)", Image.FromFile(Path.Combine(PrimeImagesPath, "chargebeam_plasma.png")));
            img.Add("Grapple Beam", Image.FromFile(Path.Combine(PrimeImagesPath, "grapplebeam.png")));
            img.Add("Super Missile", Image.FromFile(Path.Combine(PrimeImagesPath, "supermissile.png")));
            img.Add("Wavebuster", Image.FromFile(Path.Combine(PrimeImagesPath, "wavebuster.png")));
            img.Add("Ice Spreader", Image.FromFile(Path.Combine(PrimeImagesPath, "icespreader.png")));
            img.Add("Flamethrower", Image.FromFile(Path.Combine(PrimeImagesPath, "flamethrower.png")));
            img.Add("Scan Visor", Image.FromFile(Path.Combine(PrimeImagesPath, "scanvisor.png")));
            img.Add("Thermal Visor", Image.FromFile(Path.Combine(PrimeImagesPath, "thermalvisor.png")));
            img.Add("XRay Visor", Image.FromFile(Path.Combine(PrimeImagesPath, "xrayvisor.png")));
            img.Add("Artifacts", Image.FromFile(Path.Combine(PrimeImagesPath, "artifacts.png")));

            foreach (string key in img.Keys)
                img[key] = ImageUtils.MakeOutline(img[key], Color.Black, outline_width);
        }

        public override long IGT()
        {
            return this._IGT;
        }

        public override bool IsMorphed()
        {
            return _IsMorphed;
        }

        public override bool IsSwitchingState()
        {
            return _IsSwitchingState;
        }

        protected int Unk2ToCustomItem(string pickup)
        {
            switch(pickup)
            {
                case "Unlimited Missiles":
                    return (1 << 0);
                case "Unlimited Power Bombs":
                    return (1 << 1);
                case "Missile Launcher":
                    return (1 << 2);
                case "Power Bomb Launcher":
                    return (1 << 3);
                case "Spring Ball":
                    return (1 << 4);
                default:
                    throw new Exception($"{pickup} is not a custom item");
            }
        }

        public override bool HasPickup(string pickup)
        {
            switch(pickup)
            {
                case "Energy Tanks":
                    return HaveEnergyTanks;
                case "Missiles":
                    return HaveMissiles;
                case "Morph Ball":
                    return HaveMorphBall;
                case "Morph Ball Bombs":
                    return HaveMorphBallBombs;
                case "Boost Ball":
                    return HaveBoostBall;
                case "Spider Ball":
                    return HaveSpiderBall;
                case "Power Bombs":
                    return HavePowerBombs;
                case "Space Jump Boots":
                    return HaveSpaceJumpBoots;
                case "Varia Suit":
                    return HaveVariaSuit;
                case "Gravity Suit":
                    return HaveGravitySuit;
                case "Phazon Suit":
                    return HavePhazonSuit;
                case "Scan Visor":
                    return HaveScanVisor;
                case "Thermal Visor":
                    return HaveThermalVisor;
                case "XRay Visor":
                    return HaveXRayVisor;
                case "Power Beam":
                    return HavePowerBeam;
                case "Wave Beam":
                    return HaveWaveBeam;
                case "Ice Beam":
                    return HaveIceBeam;
                case "Plasma Beam":
                    return HavePlasmaBeam;
                case "Charge Beam":
                    return HaveChargeBeam;
                case "Charge Beam (Power Beam)":
                    return ProgressivePowerBeam >= 2;
                case "Charge Beam (Wave Beam)":
                    return ProgressiveWaveBeam >= 2;
                case "Charge Beam (Ice Beam)":
                    return ProgressiveIceBeam >= 2;
                case "Charge Beam (Plasma Beam)":
                    return ProgressivePlasmaBeam >= 2;
                case "Grapple Beam":
                    return HaveGrappleBeam;
                case "Super Missile":
                    return HaveSuperMissile;
                case "Wavebuster":
                    return HaveWavebuster;
                case "Ice Spreader":
                    return HaveIceSpreader;
                case "Flamethrower":
                    return HaveFlamethrower;
                case "Artifacts":
                    return GetPickupCount(pickup) > 0;
                default:
                    return false;
            }
        }

        public override int GetPickupCount(string pickup)
        {
            int count = 0;
            switch (pickup)
            {
                case "Energy Tanks":
                    return MaxEnergyTanks;
                case "Missiles":
                    if (HaveMissiles)
                    {
                        if (HaveUnlimitedMissiles)
                            return -1;
                        return (MaxMissiles - missile_launcher_provided_ammo) / missiles_per_expansion;
                    }
                    return 0;
                case "Morph Ball":
                    return HaveMorphBall ? 1 : 0;
                case "Morph Ball Bombs":
                    return HaveMorphBallBombs ? 1 : 0;
                case "Boost Ball":
                    return HaveBoostBall ? 1 : 0;
                case "Spider Ball":
                    return HaveSpiderBall ? 1 : 0;
                case "Power Bombs":
                    if (HavePowerBombs)
                    {
                        if (HaveUnlimitedPowerBombs)
                            return -1;
                        return MaxPowerBombs;
                    }
                    return 0;
                case "Space Jump Boots":
                    return HaveSpaceJumpBoots ? 1 : 0;
                case "Varia Suit":
                    return HaveVariaSuit ? 1 : 0;
                case "Gravity Suit":
                    return HaveGravitySuit ? 1 : 0;
                case "Phazon Suit":
                    return HavePhazonSuit ? 1 : 0;
                case "Scan Visor":
                    return HaveScanVisor ? 1 : 0;
                case "Thermal Visor":
                    return HaveThermalVisor ? 1 : 0;
                case "XRay Visor":
                    return HaveXRayVisor ? 1 : 0;
                case "Power Beam":
                    return HavePowerBeam ? 1 : 0;
                case "Wave Beam":
                    return HaveWaveBeam ? 1 : 0;
                case "Ice Beam":
                    return HaveIceBeam ? 1 : 0;
                case "Plasma Beam":
                    return HavePlasmaBeam ? 1 : 0;
                case "Charge Beam":
                    return HaveChargeBeam ? 1 : 0;
                case "Charge Beam (Power Beam)":
                    return ProgressivePowerBeam >= 2 ? 1 : 0;
                case "Charge Beam (Wave Beam)":
                    return ProgressiveWaveBeam >= 2 ? 1 : 0;
                case "Charge Beam (Ice Beam)":
                    return ProgressiveIceBeam >= 2 ? 1 : 0;
                case "Charge Beam (Plasma Beam)":
                    return ProgressivePlasmaBeam >= 2 ? 1 : 0;
                case "Grapple Beam":
                    return HaveGrappleBeam ? 1 : 0;
                case "Super Missile":
                    return HaveSuperMissile ? 1 : 0;
                case "Wavebuster":
                    return HaveWavebuster ? 1 : 0;
                case "Ice Spreader":
                    return HaveIceSpreader ? 1 : 0;
                case "Flamethrower":
                    return HaveFlamethrower ? 1 : 0;
                case "Artifacts":
                    for (int i = 1;i<=12;i++)
                        if (Artifacts(i - 1))
                            count++;
                    return count;
                default:
                    return 0;
            }
        }

        public override int GetIntState(string state)
        {
            throw new Exception($"Prime does not have {state} state!");
        }

        public override bool GetBoolState(string state)
        {
            if (state == "IsProgressiveBeamEnabled")
            {
                return IsProgressiveBeamEnabled;
            }

            throw new Exception($"Prime does not have {state} state!");
        }

        public override void SetIntState(string state, int value)
        {
            throw new Exception($"Prime does not have {state} state!");
        }

        public override void SetBoolState(string state, bool value)
        {
            throw new Exception($"Prime does not have {state} state!");
        }

        public override Image GetIcon(string pickup)
        {
            try {
                return img[pickup];
            } catch {
                return null;
            }
        }

        public override void Update() { }
    }
}
