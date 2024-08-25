using MPItemTracker2;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text.Json;
using Utils;

namespace Wrapper.Corruption
{
    public class Corruption : Metroid
    {
        protected const long OFF_PLAYTIME = 0x38;
        protected const long OFF_CPLAYER = 0x2184;
        protected const long OFF_CPLAYER_MORPHSTATE = 0x358;
        protected const long OFF_CPLAYERSTATE = 0x35A0;

        internal const long OFF_HEALTH = 0x8;
        internal const long OFF_MAX_HEALTH = OFF_HEALTH + 0x4;
        internal const long OFF_POWERBEAM_OBTAINED = 0x58;
        internal const long OFF_PLASMABEAM_OBTAINED = 0x64;
        internal const long OFF_NOVABEAM_OBTAINED = 0x70;
        internal const long OFF_CHARGEBEAM_OBTAINED = 0x7C;
        internal const long OFF_MISSILES = 0x84;
        internal const long OFF_MAX_MISSILES = OFF_MISSILES + 4;
        internal const long OFF_ICEMISSILE_OBTAINED = 0x94;
        internal const long OFF_SEEKERLAUNCHER_OBTAINED = 0xA0;
        internal const long OFF_GRAPPLELASSO_OBTAINED = 0xAC;
        internal const long OFF_GRAPPLESWING_OBTAINED = 0xB8;
        internal const long OFF_GRAPPLEVOLTAGE_OBTAINED = 0xC4;
        internal const long OFF_MORPHBALLBOMBS_OBTAINED = 0xD0;
        internal const long OFF_COMBATVISOR_OBTAINED = 0xDC;
        internal const long OFF_SCANVISOR_OBTAINED = 0xE8;
        internal const long OFF_COMMANDVISOR_OBTAINED = 0xF4;
        internal const long OFF_XRAYVISOR_OBTAINED = 0x100;
        internal const long OFF_SPACEJUMPBOOTS_OBTAINED = 0x10C;
        internal const long OFF_SCREWATTACK_OBTAINED = 0x118;
        internal const long OFF_SUITTYPE_OBTAINED = 0x120;
        internal const long OFF_PEDSUIT_OBTAINED = 0x12C;
        internal const long OFF_ENERGYTANKS = 0x144;
        internal const long OFF_MAX_ENERGYTANKS = OFF_ENERGYTANKS + 4;
        internal const long OFF_ENERGY_CELL_1_OBTAINED = 0x168;
        internal const long OFF_ENERGY_CELL_2_OBTAINED = OFF_ENERGY_CELL_1_OBTAINED + 0x0C;
        internal const long OFF_ENERGY_CELL_3_OBTAINED = OFF_ENERGY_CELL_2_OBTAINED + 0x0C;
        internal const long OFF_ENERGY_CELL_4_OBTAINED = OFF_ENERGY_CELL_3_OBTAINED + 0x0C;
        internal const long OFF_ENERGY_CELL_5_OBTAINED = 0x198;
        internal const long OFF_ENERGY_CELL_6_OBTAINED = OFF_ENERGY_CELL_5_OBTAINED + 0x0C;
        internal const long OFF_ENERGY_CELL_7_OBTAINED = OFF_ENERGY_CELL_6_OBTAINED + 0x0C;
        internal const long OFF_ENERGY_CELL_8_OBTAINED = OFF_ENERGY_CELL_7_OBTAINED + 0x0C;
        internal const long OFF_ENERGY_CELL_9_OBTAINED = OFF_ENERGY_CELL_8_OBTAINED + 0x0C;
        internal const long OFF_MORPHBALL_OBTAINED = 0x1D8;
        internal const long OFF_BOOSTBALL_OBTAINED = 0x1E4;
        internal const long OFF_SPIDERBALL_OBTAINED = 0x1F0;
        internal const long OFF_HYPERMODE_OBTAINED = 0x1FC;
        internal const long OFF_HYPERBEAM_OBTAINED = 0x208;
        internal const long OFF_HYPERMISSILE_OBTAINED = 0x214;
        internal const long OFF_HYPERBALL_OBTAINED = 0x220;
        internal const long OFF_HYPERGRAPPLE_OBTAINED = 0x22C;
        internal const long OFF_SHIP_GRAPPLEBEAM_OBTAINED = 0x268;
        internal const long OFF_SHIP_MISSILES = 0x270;
        internal const long OFF_MAX_SHIP_MISSILES = OFF_SHIP_MISSILES + 4;
        internal const long OFF_ENERGY_CELL_1_USED = 0x2D0;
        internal const long OFF_ENERGY_CELL_2_USED = OFF_ENERGY_CELL_1_USED + 0xC;
        internal const long OFF_ENERGY_CELL_3_USED = OFF_ENERGY_CELL_2_USED + 0xC;
        internal const long OFF_ENERGY_CELL_4_USED = OFF_ENERGY_CELL_3_USED + 0xC;
        internal const long OFF_ENERGY_CELL_5_USED = OFF_ENERGY_CELL_4_USED + 0xC;
        internal const long OFF_ENERGY_CELL_6_USED = OFF_ENERGY_CELL_5_USED + 0xC;
        internal const long OFF_ENERGY_CELL_7_USED = OFF_ENERGY_CELL_6_USED + 0xC;
        internal const long OFF_ENERGY_CELL_8_USED = OFF_ENERGY_CELL_7_USED + 0xC;
        internal const long OFF_ENERGY_CELL_9_USED = OFF_ENERGY_CELL_8_USED + 0xC;

        protected virtual long CPlayer { get; }
        protected virtual long CGameState { get; }
        protected virtual long CPlayerState { get; }
        protected virtual long _IGT { get; }

        protected virtual bool _IsMorphed { get; }
        protected virtual bool _IsSwitchingState { get; }

        protected virtual int MaxMissiles { get; }
        protected virtual int MaxEnergyTanks { get; }
        protected virtual int MaxShipMissiles { get; }

        protected bool HaveEnergyTanks
        {
            get
            {
                return MaxEnergyTanks > 0;
            }
        }

        protected virtual bool HavePlasmaBeam { get; }
        protected virtual bool HaveNovaBeam { get; }
        protected virtual bool HaveChargeBeam { get; }

        protected bool HaveMissiles
        {
            get
            {
                return MaxMissiles > 0;
            }
        }

        protected bool HaveShipMissiles
        {
            get
            {
                return MaxShipMissiles > 0;
            }
        }

        protected virtual bool HaveShipGrappleBeam { get; }

        protected virtual bool HaveMorphBall { get; }
        protected virtual bool HaveMorphBallBombs { get; }
        protected virtual bool HaveBoostBall { get; }
        protected virtual bool HaveSpiderBall { get; }
        protected virtual bool HaveHyperBall { get; }
        protected virtual bool HaveIceMissile { get; }
        protected virtual bool HaveSeekerLauncher { get; }
        protected virtual bool HaveHyperMissile { get; }
        protected virtual bool HaveGrappleLasso { get; }
        protected virtual bool HaveGrappleSwing { get; }
        protected virtual bool HaveGrappleVoltage { get; }
        protected virtual bool HaveHyperGrapple { get; }
        protected virtual bool HaveScrewAttack { get; }
        protected virtual bool HaveScanVisor { get; }
        protected virtual bool HaveCommandVisor { get; }
        protected virtual bool HaveXRayVisor { get; }
        protected virtual bool HaveSpaceJumpBoots { get; }
        protected virtual bool HaveHypermode { get; }
        protected virtual bool HaveHazardShield { get; }
        protected virtual bool EnergyCells(int index) { return false; }

        protected Dictionary<String, Image> img = new Dictionary<string, Image>();
        protected int missile_launcher_provided_ammo = 5;
        protected int missiles_per_expansion = 5;

        public Corruption()
        {
            dynamic json = JsonSerializer.Deserialize<dynamic>(File.ReadAllText(Path.Combine(Program.ExecutableDir, "corruption.json")));
            try {
                missile_launcher_provided_ammo = json.GetProperty("missiles_provided_ammo").GetInt32();
                missiles_per_expansion = json.GetProperty("missiles_per_expansion").GetInt32();
            } catch { }
            int outline_width = 2;
            var CorruptionImagesPath = Path.Combine(Program.ExecutableDir, "img", "corruption");
            img.Add("Energy Tanks", Image.FromFile(Path.Combine(CorruptionImagesPath, "energy_tank.png")));
            img.Add("Ship Missiles", Image.FromFile(Path.Combine(CorruptionImagesPath, "ship_missile.png")));
            img.Add("Ship Grapple Beam", Image.FromFile(Path.Combine(CorruptionImagesPath, "ship_grapple_beam.png")));
            img.Add("Morph Ball", Image.FromFile(Path.Combine(CorruptionImagesPath, "morph_ball.png")));
            img.Add("Morph Ball Bombs", Image.FromFile(Path.Combine(CorruptionImagesPath, "morph_ball_bomb.png")));
            img.Add("Boost Ball", Image.FromFile(Path.Combine(CorruptionImagesPath, "boost_ball.png")));
            img.Add("Spider Ball", Image.FromFile(Path.Combine(CorruptionImagesPath, "spider_ball.png")));
            img.Add("Hyper Ball", Image.FromFile(Path.Combine(CorruptionImagesPath, "hyper_ball.png")));
            img.Add("Space Jump Boots", Image.FromFile(Path.Combine(CorruptionImagesPath, "space_jump_boots.png")));
            img.Add("Screw Attack", Image.FromFile(Path.Combine(CorruptionImagesPath, "screw_attack.png")));
            img.Add("Hypermode", Image.FromFile(Path.Combine(CorruptionImagesPath, "hypermode.png")));
            img.Add("Hazard Shield", Image.FromFile(Path.Combine(CorruptionImagesPath, "hazard_shield.png")));
            img.Add("Plasma Beam", Image.FromFile(Path.Combine(CorruptionImagesPath, "plasma_beam.png")));
            img.Add("Nova Beam", Image.FromFile(Path.Combine(CorruptionImagesPath, "nova_beam.png")));
            img.Add("Charge Beam", Image.FromFile(Path.Combine(CorruptionImagesPath, "charge_beam.png")));
            img.Add("Grapple Lasso", Image.FromFile(Path.Combine(CorruptionImagesPath, "grapple_lasso.png")));
            img.Add("Grapple Swing", Image.FromFile(Path.Combine(CorruptionImagesPath, "grapple_swing.png")));
            img.Add("Grapple Voltage", Image.FromFile(Path.Combine(CorruptionImagesPath, "grapple_voltage.png")));
            img.Add("Hyper Grapple", Image.FromFile(Path.Combine(CorruptionImagesPath, "hyper_grapple.png")));
            img.Add("Missiles", Image.FromFile(Path.Combine(CorruptionImagesPath, "missile_launcher.png")));
            img.Add("Ice Missile", Image.FromFile(Path.Combine(CorruptionImagesPath, "ice_missile.png")));
            img.Add("Seeker Launcher", Image.FromFile(Path.Combine(CorruptionImagesPath, "seeker_launcher.png")));
            img.Add("Hyper Missile", Image.FromFile(Path.Combine(CorruptionImagesPath, "hyper_missile.png")));
            img.Add("Scan Visor", Image.FromFile(Path.Combine(CorruptionImagesPath, "scan_visor.png")));
            img.Add("Command Visor", Image.FromFile(Path.Combine(CorruptionImagesPath, "command_visor.png")));
            img.Add("XRay Visor", Image.FromFile(Path.Combine(CorruptionImagesPath, "xray_visor.png")));
            img.Add("Energy Cells", Image.FromFile(Path.Combine(CorruptionImagesPath, "energy_cell.png")));

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

        public override bool HasPickup(string pickup)
        {
            int i;
            switch (pickup)
            {
                case "Energy Tanks":
                    return HaveEnergyTanks;
                case "Ship Missiles":
                    return HaveShipMissiles;
                case "Ship Grapple Beam":
                    return HaveShipGrappleBeam;
                case "Morph Ball":
                    return HaveMorphBall;
                case "Morph Ball Bombs":
                    return HaveMorphBallBombs;
                case "Boost Ball":
                    return HaveBoostBall;
                case "Spider Ball":
                    return HaveSpiderBall;
                case "Hyper Ball":
                    return HaveHyperBall;
                case "Space Jump Boots":
                    return HaveSpaceJumpBoots;
                case "Screw Attack":
                    return HaveScrewAttack;
                case "Hypermode":
                    return HaveHypermode;
                case "Hazard Shield":
                    return HaveHazardShield;
                case "Plasma Beam":
                    return HavePlasmaBeam;
                case "Nova Beam":
                    return HaveNovaBeam;
                case "Charge Beam":
                    return HaveChargeBeam;
                case "Grapple Lasso":
                    return HaveGrappleLasso;
                case "Grapple Swing":
                    return HaveGrappleSwing;
                case "Grapple Voltage":
                    return HaveGrappleVoltage;
                case "Hyper Grapple":
                    return HaveHyperGrapple;
                case "Missiles":
                    return HaveMissiles;
                case "Ice Missile":
                    return HaveIceMissile;
                case "Seeker Launcher":
                    return HaveSeekerLauncher;
                case "Hyper Missile":
                    return HaveHyperMissile;
                case "Scan Visor":
                    return HaveScanVisor;
                case "Command Visor":
                    return HaveCommandVisor;
                case "XRay Visor":
                    return HaveXRayVisor;
                case "Energy Cells":
                    for (i = 1; i <= 9; i++)
                        if (EnergyCells(i - 1))
                            return true;
                    return false;
                default:
                    return false;
            }
        }

        public override int GetPickupCount(string pickup)
        {
            int count = 0, i;
            switch (pickup)
            {
                case "Energy Tanks":
                    return MaxEnergyTanks;
                case "Ship Missiles":
                    return MaxShipMissiles;
                case "Ship Grapple Beam":
                    return HaveShipGrappleBeam ? 1 : 0;
                case "Morph Ball":
                    return HaveMorphBall ? 1 : 0;
                case "Morph Ball Bombs":
                    return HaveMorphBallBombs ? 1 : 0;
                case "Boost Ball":
                    return HaveBoostBall ? 1 : 0;
                case "Spider Ball":
                    return HaveSpiderBall ? 1 : 0;
                case "Hyper Ball":
                    return HaveHyperBall ? 1 : 0;
                case "Space Jump Boots":
                    return HaveSpaceJumpBoots ? 1 : 0;
                case "Screw Attack":
                    return HaveScrewAttack ? 1 : 0;
                case "Hypermode":
                    return HaveHypermode ? 1 : 0;
                case "Hazard Shield":
                    return HaveHazardShield ? 1 : 0;
                case "Plasma Beam":
                    return HavePlasmaBeam ? 1 : 0;
                case "Nova Beam":
                    return HaveNovaBeam ? 1 : 0;
                case "Charge Beam":
                    return HaveChargeBeam ? 1 : 0;
                case "Grapple Lasso":
                    return HaveGrappleLasso ? 1 : 0;
                case "Grapple Swing":
                    return HaveGrappleSwing ? 1 : 0;
                case "Grapple Voltage":
                    return HaveGrappleVoltage ? 1 : 0;
                case "Hyper Grapple":
                    return HaveHyperGrapple ? 1 : 0;
                case "Missiles":
                    return (MaxMissiles - missile_launcher_provided_ammo) / missiles_per_expansion;
                case "Ice Missile":
                    return HaveIceMissile ? 1 : 0;
                case "Seeker Launcher":
                    return HaveSeekerLauncher ? 1 : 0;
                case "Hyper Missile":
                    return HaveHyperMissile ? 1 : 0;
                case "Scan Visor":
                    return HaveScanVisor ? 1 : 0;
                case "Command Visor":
                    return HaveCommandVisor ? 1 : 0;
                case "XRay Visor":
                    return HaveXRayVisor ? 1 : 0;
                case "Energy Cells":
                    for (i = 1; i <= 9; i++)
                        if (EnergyCells(i - 1))
                            count++;
                    return count;
                default:
                    return 0;
            }
        }

        public override Image GetIcon(string pickup)
        {
            try
            {
                return img[pickup];
            }
            catch
            {
                return null;
            }
        }

        public override void Update() { }
    }
}
