using MPItemTracker2;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text.Json;
using Utils;

namespace Wrapper.Echoes
{
    public class Echoes : Metroid
    {
        protected const long OFF_PLAYTIME = 0x48;
        protected const long OFF_CWORLD = 0x8D0;
        protected const long OFF_CWORLD_MREA = 0x88;
        protected const long OFF_CWORLD_MLVLID = 0x6C;
        protected const long OFF_CWORLD_MORPHSTATE = 0x2F0;
        protected const long OFF_CPLAYER = 0x1624;
        protected const long OFF_CPLAYER_MORPHSTATE = 0x38C;
        protected const long OFF_CPLAYER_CPLAYERMORPH = 0x1174;
        protected const long OFF_CPLAYER_ENABLEDINPUT_ARRAY = 0x13D4;
        protected const long OFF_CPLAYERMORPH_USEDBOOST = 0x28;
        protected const long OFF_CPLAYERMORPH_MORPHSTATE = 0x74;
        protected const long OFF_CPLAYERSTATE = 0x1314;

        internal const long OFF_HEALTH = 0x14;
        internal const long OFF_POWERBEAM_OBTAINED = 0x60;
        internal const long OFF_DARKBEAM_OBTAINED = 0x6C;
        internal const long OFF_LIGHTBEAM_OBTAINED = 0x78;
        internal const long OFF_ANNIHILATORBEAM_OBTAINED = 0x84;
        internal const long OFF_SUPERMISSILE_OBTAINED = 0x90;
        internal const long OFF_DARKBURST_OBTAINED = 0x9C;
        internal const long OFF_SUNBURST_OBTAINED = 0xA8;
        internal const long OFF_SONICBOOM_OBTAINED = 0xB4;
        internal const long OFF_SCANVISOR_OBTAINED = 0xCC;
        internal const long OFF_DARKVISOR_OBTAINED = 0xD8;
        internal const long OFF_ECHOVISOR_OBTAINED = 0xE4;
        internal const long OFF_VARIASUIT_OBTAINED = 0xEC;
        internal const long OFF_DARKSUIT_OBTAINED = 0xFC;
        internal const long OFF_LIGHTSUIT_OBTAINED = 0x108;
        internal const long OFF_MORPHBALL_OBTAINED = 0x114;
        internal const long OFF_BOOSTBALL_OBTAINED = 0x11C;
        internal const long OFF_SPIDERBALL_OBTAINED = 0x12C;
        internal const long OFF_MORPHBALLBOMBS_OBTAINED = 0x134;
        internal const long OFF_CHARGEBEAM_OBTAINED = 0x168;
        internal const long OFF_GRAPPLEBEAM_OBTAINED = 0x174;
        internal const long OFF_SPACEJUMPBOOTS_OBTAINED = 0x180;
        internal const long OFF_GRAVITYBOOST_OBTAINED = 0x188;
        internal const long OFF_SEEKERLAUNCHER_OBTAINED = 0x198;
        internal const long OFF_SCREWATTACK_OBTAINED = 0x1A4;
        internal const long OFF_SKY_TEMPLE_KEY_1_OBTAINED = 0x1BC;
        internal const long OFF_SKY_TEMPLE_KEY_2_OBTAINED = OFF_SKY_TEMPLE_KEY_1_OBTAINED + 0x0C;
        internal const long OFF_SKY_TEMPLE_KEY_3_OBTAINED = OFF_SKY_TEMPLE_KEY_2_OBTAINED + 0x0C;
        internal const long OFF_DARK_AGON_KEY_1_OBTAINED = 0x1E0;
        internal const long OFF_DARK_AGON_KEY_2_OBTAINED = OFF_DARK_AGON_KEY_1_OBTAINED + 0x0C;
        internal const long OFF_DARK_AGON_KEY_3_OBTAINED = OFF_DARK_AGON_KEY_2_OBTAINED + 0x0C;
        internal const long OFF_DARK_TORVUS_KEY_1_OBTAINED = OFF_DARK_AGON_KEY_3_OBTAINED + 0x0C;
        internal const long OFF_DARK_TORVUS_KEY_2_OBTAINED = OFF_DARK_TORVUS_KEY_1_OBTAINED + 0x0C;
        internal const long OFF_DARK_TORVUS_KEY_3_OBTAINED = OFF_DARK_TORVUS_KEY_2_OBTAINED + 0x0C;
        internal const long OFF_ING_HIVE_KEY_1_OBTAINED = OFF_DARK_TORVUS_KEY_3_OBTAINED + 0x0C;
        internal const long OFF_ING_HIVE_KEY_2_OBTAINED = OFF_ING_HIVE_KEY_1_OBTAINED + 0x0C;
        internal const long OFF_ING_HIVE_KEY_3_OBTAINED = OFF_ING_HIVE_KEY_2_OBTAINED + 0x0C;
        internal const long OFF_ENERGYTANKS = 0x254;
        internal const long OFF_MAX_ENERGYTANKS = OFF_ENERGYTANKS + 4;
        internal const long OFF_POWERBOMBS = 0x260;
        internal const long OFF_MAX_POWERBOMBS = OFF_POWERBOMBS + 4;
        internal const long OFF_MISSILES = 0x26C;
        internal const long OFF_MAX_MISSILES = OFF_MISSILES + 4;
        internal const long OFF_DARKBEAM_AMMO = 0x27C;
        internal const long OFF_MAX_DARKBEAM_AMMO = OFF_DARKBEAM_AMMO + 4;
        internal const long OFF_LIGHTBEAM_AMMO = 0x288;
        internal const long OFF_MAX_LIGHTBEAM_AMMO = OFF_LIGHTBEAM_AMMO + 4;
        internal const long OFF_MP_INVISIBILITY_OBTAINED = 0x30C;
        internal const long OFF_MP_DOUBLE_DAMAGE_OBTAINED = 0x318;
        internal const long OFF_MP_INVINCIBILITY_OBTAINED = 0x324;
        internal const long OFF_MP_UNLIMITED_MISSILES_OBTAINED = 0x42C;
        internal const long OFF_MP_UNLIMITED_BEAM_AMMO_OBTAINED = 0x438;
        internal const long OFF_MP_DARK_SHIELD_OBTAINED = 0x444;
        internal const long OFF_MP_LIGHT_SHIELD_OBTAINED = 0x450;
        internal const long OFF_MP_ABSORB_ATTACK_OBTAINED = 0x45C;
        internal const long OFF_MP_DEATH_BALL_OBTAINED = 0x468;
        internal const long OFF_MP_SCAN_VIRUS_OBTAINED = 0x474;
        internal const long OFF_MP_DISABLE_BALL_OBTAINED = 0x4B0;
        internal const long OFF_MP_HACKED_EFFECT_OBTAINED = 0x4D4;
        internal const long OFF_MP_CANNON_BALL_OBTAINED = 0x4DC;
        internal const long OFF_VIOLET_TRANSLATOR_OBTAINED = 0x4EC;
        internal const long OFF_AMBER_TRANSLATOR_OBTAINED = 0x4F8;
        internal const long OFF_EMERALD_TRANSLATOR_OBTAINED = 0x504;
        internal const long OFF_COBALT_TRANSLATOR_OBTAINED = 0x510;
        internal const long OFF_SKY_TEMPLE_KEY_4_OBTAINED = 0x51C;
        internal const long OFF_SKY_TEMPLE_KEY_5_OBTAINED = OFF_SKY_TEMPLE_KEY_4_OBTAINED + 0x0C;
        internal const long OFF_SKY_TEMPLE_KEY_6_OBTAINED = OFF_SKY_TEMPLE_KEY_5_OBTAINED + 0x0C;
        internal const long OFF_SKY_TEMPLE_KEY_7_OBTAINED = OFF_SKY_TEMPLE_KEY_6_OBTAINED + 0x0C;
        internal const long OFF_SKY_TEMPLE_KEY_8_OBTAINED = OFF_SKY_TEMPLE_KEY_7_OBTAINED + 0x0C;
        internal const long OFF_SKY_TEMPLE_KEY_9_OBTAINED = OFF_SKY_TEMPLE_KEY_8_OBTAINED + 0x0C;
        internal const long OFF_ENERGY_TRANSFER_MODULE_OBTAINED = 0x564;

        protected virtual long CPlayer { get; }
        protected virtual long CPlayerMorph { get; }
        protected virtual long CWorld { get; }
        protected virtual long CGameState { get; }
        protected virtual long CPlayerState { get; }
        protected virtual long _IGT { get; }

        protected virtual int MorphState { get; }

        protected virtual bool WasLaunchedByCannon { get; }

        protected virtual bool HasControl { get; }

        protected virtual int MaxMissiles { get; }
        protected virtual int MaxPowerBombs { get; }
        protected virtual int MaxEnergyTanks { get; }
        protected virtual int MaxDarkAmmo { get; }
        protected virtual int MaxLightAmmo { get; }

        protected bool HaveEnergyTanks
        {
            get
            {
                return MaxEnergyTanks > 0;
            }
        }

        protected bool HaveDarkAmmo
        {
            get
            {
                return MaxDarkAmmo > 0;
            }
        }

        protected virtual bool HavePowerBeam { get; }

        protected virtual bool HaveDarkBeam { get; }

        protected bool HaveLightAmmo
        {
            get
            {
                return MaxLightAmmo > 0;
            }
        }

        protected virtual bool HaveLightBeam { get; }
        protected virtual bool HaveAnnihilatorBeam { get; }

        protected bool HaveMissiles
        {
            get
            {
                return MaxMissiles > 0;
            }
        }

        protected virtual bool HaveMorphBallBombs { get; }

        protected bool HavePowerBombs
        {
            get
            {
                return MaxPowerBombs > 0;
            }
        }

        protected virtual bool HaveSonicBoom { get; }
        protected virtual bool HaveDarkVisor { get; }
        protected virtual bool HaveChargeBeam { get; }
        protected virtual bool HaveSeekerLauncher { get; }
        protected virtual bool HaveSuperMissile { get; }
        protected virtual bool HaveGrappleBeam { get; }
        protected virtual bool HaveScrewAttack { get; }
        protected virtual bool HaveEchoVisor { get; }
        protected virtual bool HaveSunburst { get; }
        protected virtual bool HaveSpaceJumpBoots { get; }
        protected virtual bool HaveMorphBall { get; }
        protected virtual bool HaveBoostBall { get; }
        protected virtual bool HaveSpiderBall { get; }
        protected virtual bool HaveLightSuit { get; }
        protected virtual bool HaveDarkSuit { get; }
        protected virtual bool HaveGravityBoost { get; }
        protected virtual bool HaveScanVisor { get; }
        protected virtual bool HaveDarkburst { get; }
        protected virtual bool HaveVioletTranslator { get; }
        protected virtual bool HaveAmberTranslator { get; }
        protected virtual bool HaveEmeraldTranslator { get; }
        protected virtual bool HaveCobaltTranslator { get; }
        protected virtual bool HaveEnergyTransferModule { get; }
        protected virtual bool HaveDoubleDamage { get; }
        protected virtual bool HaveUnlimitedMissiles { get; }
        protected virtual bool HaveUnlimitedBeamAmmo { get; }
        protected virtual bool HaveCannonBall { get; }
        protected virtual bool DarkAgonKeys(int index) { return false; }
        protected virtual bool DarkTorvusKeys(int index) { return false; }
        protected virtual bool IngHiveKeys(int index) { return false; }
        protected virtual bool SkyTempleKeys(int index) { return false; }

        protected Dictionary<String, Image> img = new Dictionary<string, Image>();
        protected int dark_beam_provided_ammo = 50;
        protected int dark_ammo_per_expansion = 50;
        protected int light_beam_provided_ammo = 50;
        protected int light_ammo_per_expansion = 50;
        protected int seeker_launcher_provided_ammo = 5;
        protected int missile_launcher_provided_ammo = 5;
        protected int missiles_per_expansion = 5;

        public Echoes()
        {
            dynamic json = JsonSerializer.Deserialize<dynamic>(File.ReadAllText(Path.Combine(Program.ExecutableDir, "echoes.json")));
            try {
                dark_beam_provided_ammo = json.GetProperty("dark_beam_provided_ammo").GetInt32();
                dark_ammo_per_expansion = json.GetProperty("dark_ammo_per_expansion").GetInt32();
                light_beam_provided_ammo = json.GetProperty("light_beam_provided_ammo").GetInt32();
                light_ammo_per_expansion = json.GetProperty("light_ammo_per_expansion").GetInt32();
                seeker_launcher_provided_ammo = json.GetProperty("seeker_launcher_provided_ammo").GetInt32();
                missile_launcher_provided_ammo = json.GetProperty("missiles_provided_ammo").GetInt32();
                missiles_per_expansion = json.GetProperty("missiles_per_expansion").GetInt32();
            } catch { }
            int outline_width = 2;
            var EchoesImagesPath = Path.Combine(Program.ExecutableDir, "img", "echoes");
            img.Add("Energy Tanks", Image.FromFile(Path.Combine(EchoesImagesPath, "energy_tank.png")));
            img.Add("Missiles", Image.FromFile(Path.Combine(EchoesImagesPath, "missile_launcher.png")));
            img.Add("Seeker Launcher", Image.FromFile(Path.Combine(EchoesImagesPath, "seeker_launcher.png")));
            img.Add("Morph Ball", Image.FromFile(Path.Combine(EchoesImagesPath, "morph_ball.png")));
            img.Add("Morph Ball Bombs", Image.FromFile(Path.Combine(EchoesImagesPath, "morph_ball_bomb.png")));
            img.Add("Power Bombs", Image.FromFile(Path.Combine(EchoesImagesPath, "power_bomb.png")));
            img.Add("Boost Ball", Image.FromFile(Path.Combine(EchoesImagesPath, "boost_ball.png")));
            img.Add("Spider Ball", Image.FromFile(Path.Combine(EchoesImagesPath, "spider_ball.png")));
            img.Add("Space Jump Boots", Image.FromFile(Path.Combine(EchoesImagesPath, "space_jump_boots.png")));
            img.Add("Screw Attack", Image.FromFile(Path.Combine(EchoesImagesPath, "screw_attack.png")));
            img.Add("Dark Suit", Image.FromFile(Path.Combine(EchoesImagesPath, "dark_suit.png")));
            img.Add("Light Suit", Image.FromFile(Path.Combine(EchoesImagesPath, "light_suit.png")));
            img.Add("Gravity Boost", Image.FromFile(Path.Combine(EchoesImagesPath, "gravity_boost.png")));
            img.Add("Power Beam", Image.FromFile(Path.Combine(EchoesImagesPath, "power_beam.png")));
            img.Add("Dark Beam", Image.FromFile(Path.Combine(EchoesImagesPath, "dark_beam.png")));
            img.Add("Light Beam", Image.FromFile(Path.Combine(EchoesImagesPath, "light_beam.png")));
            img.Add("Annihilator Beam", Image.FromFile(Path.Combine(EchoesImagesPath, "annihilator_beam.png")));
            img.Add("Charge Beam", Image.FromFile(Path.Combine(EchoesImagesPath, "charge_beam.png")));
            img.Add("Grapple Beam", Image.FromFile(Path.Combine(EchoesImagesPath, "grapple_beam.png")));
            img.Add("Super Missile", Image.FromFile(Path.Combine(EchoesImagesPath, "super_missile.png")));
            img.Add("Darkburst", Image.FromFile(Path.Combine(EchoesImagesPath, "darkburst.png")));
            img.Add("Sunburst", Image.FromFile(Path.Combine(EchoesImagesPath, "sunburst.png")));
            img.Add("Sonic Boom", Image.FromFile(Path.Combine(EchoesImagesPath, "sonic_boom.png")));
            img.Add("Scan Visor", Image.FromFile(Path.Combine(EchoesImagesPath, "scan_visor.png")));
            img.Add("Dark Visor", Image.FromFile(Path.Combine(EchoesImagesPath, "dark_visor.png")));
            img.Add("Echo Visor", Image.FromFile(Path.Combine(EchoesImagesPath, "echo_visor.png")));
            img.Add("Dark Ammo Expansion", Image.FromFile(Path.Combine(EchoesImagesPath, "dark_ammo_expansion.png")));
            img.Add("Light Ammo Expansion", Image.FromFile(Path.Combine(EchoesImagesPath, "light_ammo_expansion.png")));
            img.Add("Double Damage", Image.FromFile(Path.Combine(EchoesImagesPath, "double_damage.png")));
            img.Add("Cannon Ball", Image.FromFile(Path.Combine(EchoesImagesPath, "cannon_ball.png")));
            img.Add("Energy Transfer Module", Image.FromFile(Path.Combine(EchoesImagesPath, "energy_transfer_module.png")));
            img.Add("Violet Translator", Image.FromFile(Path.Combine(EchoesImagesPath, "violet_translator.png")));
            img.Add("Amber Translator", Image.FromFile(Path.Combine(EchoesImagesPath, "amber_translator.png")));
            img.Add("Emerald Translator", Image.FromFile(Path.Combine(EchoesImagesPath, "emerald_translator.png")));
            img.Add("Cobalt Translator", Image.FromFile(Path.Combine(EchoesImagesPath, "cobalt_translator.png")));
            img.Add("Dark Agon Keys", Image.FromFile(Path.Combine(EchoesImagesPath, "dark_agon_key.png")));
            img.Add("Dark Torvus Keys", Image.FromFile(Path.Combine(EchoesImagesPath, "dark_torvus_key.png")));
            img.Add("Ing Hive Keys", Image.FromFile(Path.Combine(EchoesImagesPath, "ing_hive_key.png")));
            img.Add("Sky Temple Keys", Image.FromFile(Path.Combine(EchoesImagesPath, "sky_temple_key.png")));

            foreach (string key in img.Keys)
                img[key] = ImageUtils.MakeOutline(img[key], Color.Black, outline_width);
        }

        public override long IGT()
        {
            return this._IGT;
        }

        public override bool IsMorphed()
        {
            return (MorphState & 1) == 1;
        }

        public override bool IsSwitchingState()
        {
            return MorphState < 0 && MorphState > 1;
        }

        public override bool HasPickup(string pickup)
        {
            switch (pickup)
            {
                case "Dark Ammo Expansion":
                    return HaveDarkAmmo;
                case "Light Ammo Expansion":
                    return HaveLightAmmo;
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
                case "Dark Suit":
                    return HaveDarkSuit;
                case "Light Suit":
                    return HaveLightSuit;
                case "Progressive Suit":
                    return HaveDarkSuit || HaveLightSuit;
                case "Gravity Boost":
                    return HaveGravityBoost;
                case "Scan Visor":
                    return HaveScanVisor;
                case "Dark Visor":
                    return HaveDarkVisor;
                case "Echo Visor":
                    return HaveEchoVisor;
                case "Power Beam":
                    return HavePowerBeam;
                case "Dark Beam":
                    return HaveDarkBeam;
                case "Light Beam":
                    return HaveLightBeam;
                case "Annihilator Beam":
                    return HaveAnnihilatorBeam;
                case "Charge Beam":
                    return HaveChargeBeam;
                case "Grapple Beam":
                    return HaveGrappleBeam;
                case "Screw Attack":
                    return HaveScrewAttack;
                case "Seeker Launcher":
                    return HaveSeekerLauncher;
                case "Super Missile":
                    return HaveSuperMissile;
                case "Darkburst":
                    return HaveDarkburst;
                case "Sunburst":
                    return HaveSunburst;
                case "Sonic Boom":
                    return HaveSonicBoom;
                case "Violet Translator":
                    return HaveVioletTranslator;
                case "Amber Translator":
                    return HaveAmberTranslator;
                case "Emerald Translator":
                    return HaveEmeraldTranslator;
                case "Cobalt Translator":
                    return HaveCobaltTranslator;
                case "Energy Transfer Module":
                    return HaveEnergyTransferModule;
                case "Double Damage":
                    return HaveDoubleDamage;
                case "Cannon Ball":
                    return Checked_HasCannonBall;
                case "Dark Agon Keys":
                    return GetPickupCount(pickup) > 0;
                case "Dark Torvus Keys":
                    return GetPickupCount(pickup) > 0;
                case "Ing Hive Keys":
                    return GetPickupCount(pickup) > 0;
                case "Sky Temple Keys":
                    return GetPickupCount(pickup) > 0;
                default:
                    return false;
            }
        }

        public override int GetPickupCount(string pickup)
        {
            int count = 0, i;
            switch (pickup)
            {
                case "Dark Ammo Expansion":
                    if (HaveUnlimitedBeamAmmo)
                        return -1;
                    if (HaveDarkBeam)
                        return (MaxDarkAmmo - dark_beam_provided_ammo) / dark_ammo_per_expansion;
                    return MaxDarkAmmo / dark_ammo_per_expansion;
                case "Light Ammo Expansion":
                    if (HaveUnlimitedBeamAmmo)
                        return -1;
                    if (HaveLightBeam)
                        return (MaxLightAmmo - light_beam_provided_ammo) / light_ammo_per_expansion;
                    return MaxLightAmmo / light_ammo_per_expansion;
                case "Energy Tanks":
                    return MaxEnergyTanks;
                case "Missiles":
                    if (HaveMissiles)
                    {
                        if (HaveUnlimitedMissiles)
                            return -1;
                        return (MaxMissiles - missile_launcher_provided_ammo - ((HaveSeekerLauncher ? 1 : 0) * seeker_launcher_provided_ammo)) / missiles_per_expansion;
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
                    return MaxPowerBombs;
                case "Space Jump Boots":
                    return HaveSpaceJumpBoots ? 1 : 0;
                case "Dark Suit":
                    return HaveDarkSuit ? 1 : 0;
                case "Light Suit":
                    return HaveLightSuit ? 1 : 0;
                case "Progressive Suit":
                    if (HaveLightSuit)
                        return 2;
                    if (HaveDarkSuit)
                        return 1;
                    return 0;
                case "Gravity Boost":
                    return HaveGravityBoost ? 1 : 0;
                case "Scan Visor":
                    return HaveScanVisor ? 1 : 0;
                case "Dark Visor":
                    return HaveDarkVisor ? 1 : 0;
                case "Echo Visor":
                    return HaveEchoVisor ? 1 : 0;
                case "Power Beam":
                    return HavePowerBeam ? 1 : 0;
                case "Dark Beam":
                    return HaveDarkBeam ? 1 : 0;
                case "Light Beam":
                    return HaveLightBeam ? 1 : 0;
                case "Annihilator Beam":
                    return HaveAnnihilatorBeam ? 1 : 0;
                case "Charge Beam":
                    return HaveChargeBeam ? 1 : 0;
                case "Grapple Beam":
                    return HaveGrappleBeam ? 1 : 0;
                case "Screw Attack":
                    return HaveScrewAttack ? 1 : 0;
                case "Seeker Launcher":
                    return HaveSeekerLauncher ? 1 : 0;
                case "Super Missile":
                    return HaveSuperMissile ? 1 : 0;
                case "Darkburst":
                    return HaveDarkburst ? 1 : 0;
                case "Sunburst":
                    return HaveSunburst ? 1 : 0;
                case "Sonic Boom":
                    return HaveSonicBoom ? 1 : 0;
                case "Violet Translator":
                    return HaveVioletTranslator ? 1 : 0;
                case "Amber Translator":
                    return HaveAmberTranslator ? 1 : 0;
                case "Emerald Translator":
                    return HaveEmeraldTranslator ? 1 : 0;
                case "Cobalt Translator":
                    return HaveCobaltTranslator ? 1 : 0;
                case "Energy Transfer Module":
                    return HaveEnergyTransferModule ? 1 : 0;
                case "Double Damage":
                    return HaveDoubleDamage ? 1 : 0;
                case "Cannon Ball":
                    return Checked_HasCannonBall ? 1 : 0;
                case "Dark Agon Keys":
                    for (i = 1; i <= 3; i++)
                        if (DarkAgonKeys(i - 1))
                            count++;
                    return count;
                case "Dark Torvus Keys":
                    for (i = 1; i <= 3; i++)
                        if (DarkTorvusKeys(i - 1))
                            count++;
                    return count;
                case "Ing Hive Keys":
                    for (i = 1; i <= 3; i++)
                        if (IngHiveKeys(i - 1))
                            count++;
                    return count;
                case "Sky Temple Keys":
                    for (i = 1; i <= 9; i++)
                        if (SkyTempleKeys(i - 1))
                            count++;
                    return count;
                default:
                    return 0;
            }
        }

        public override int GetIntState(string state)
        {
            throw new Exception($"Echoes does not have {state} state!");
        }

        public override bool GetBoolState(string state)
        {
            if (state == "HasControl")
            {
                return HasControl;
            }

            if (state == "WasLaunchedByCannon")
            {
                return WasLaunchedByCannon;
            }

            throw new Exception($"Echoes does not have {state} state!");
        }

        public override void SetIntState(string state, int value)
        {
            throw new Exception($"Echoes does not have {state} state!");
        }

        public override void SetBoolState(string state, bool value)
        {
            if (state == "WasLaunchedByCannon" ||
                state == "HasControl")
            {
                throw new Exception($"Cannot set {state} state!");
            }

            throw new Exception($"Echoes does not have {state} state!");
        }

        public override Image GetIcon(string pickup)
        {
            try
            {
                switch (pickup)
                {
                    case "Progressive Suit":
                        if (HaveLightSuit)
                            return img["Light Suit"];
                        if (HaveDarkSuit)
                            return img["Dark Suit"];
                        return null;
                    default:
                        return img[pickup];
                }
            }
            catch
            {
                return null;
            }
        }

        bool Prev_WasLaunchedByCannon = false;
        bool Checked_HasCannonBall = false;

        public override void Update()
        {
            // Update cannon ball item
            bool Cur_WasLaunchedByCannon = GetBoolState("WasLaunchedByCannon");
            // if not launched by cannon
            if (!Cur_WasLaunchedByCannon && Prev_WasLaunchedByCannon == Cur_WasLaunchedByCannon)
            {
                Checked_HasCannonBall = HaveCannonBall;
            }
            else
            {
                // if we came from a cannon
                if(Prev_WasLaunchedByCannon)
                {
                    if (!HaveCannonBall)
                    {
                        Checked_HasCannonBall = false;
                    }
                }
            }
            // did we previously used cannon?
            Prev_WasLaunchedByCannon = Cur_WasLaunchedByCannon;
        }
    }
}
