using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
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
            String CurDir = Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar;
            dynamic json = JObject.Parse(File.ReadAllText(CurDir + "echoes.json"));
            try
            {
                dark_beam_provided_ammo = json.dark_beam_provided_ammo;
                dark_ammo_per_expansion = json.dark_ammo_per_expansion;
                light_beam_provided_ammo = json.light_beam_provided_ammo;
                light_ammo_per_expansion = json.light_ammo_per_expansion;
                seeker_launcher_provided_ammo = json.seeker_launcher_provided_ammo;
                missile_launcher_provided_ammo = json.missile_launcher_provided_ammo;
                missiles_per_expansion = json.missiles_per_expansion;
            }
            catch { }
            int outline_width = 2;
            img.Add("Energy Tanks", ImageUtils.MakeOutline(Image.FromFile(@"img/echoes/energy_tank.png"), Color.Black, outline_width));
            img.Add("Missiles", ImageUtils.MakeOutline(Image.FromFile(@"img/echoes/missile_launcher.png"), Color.Black, outline_width));
            img.Add("Seeker Launcher", ImageUtils.MakeOutline(Image.FromFile(@"img/echoes/seeker_launcher.png"), Color.Black, outline_width));
            img.Add("Morph Ball", ImageUtils.MakeOutline(Image.FromFile(@"img/echoes/morph_ball.png"), Color.Black, outline_width));
            img.Add("Morph Ball Bombs", ImageUtils.MakeOutline(Image.FromFile(@"img/echoes/morph_ball_bomb.png"), Color.Black, outline_width));
            img.Add("Power Bombs", ImageUtils.MakeOutline(Image.FromFile(@"img/echoes/power_bomb.png"), Color.Black, outline_width));
            img.Add("Boost Ball", ImageUtils.MakeOutline(Image.FromFile(@"img/echoes/boost_ball.png"), Color.Black, outline_width));
            img.Add("Spider Ball", ImageUtils.MakeOutline(Image.FromFile(@"img/echoes/spider_ball.png"), Color.Black, outline_width));
            img.Add("Space Jump Boots", ImageUtils.MakeOutline(Image.FromFile(@"img/echoes/space_jump_boots.png"), Color.Black, outline_width));
            img.Add("Screw Attack", ImageUtils.MakeOutline(Image.FromFile(@"img/echoes/screw_attack.png"), Color.Black, outline_width));
            img.Add("Dark Suit", ImageUtils.MakeOutline(Image.FromFile(@"img/echoes/dark_suit.png"), Color.Black, outline_width));
            img.Add("Light Suit", ImageUtils.MakeOutline(Image.FromFile(@"img/echoes/light_suit.png"), Color.Black, outline_width));
            img.Add("Gravity Boost", ImageUtils.MakeOutline(Image.FromFile(@"img/echoes/gravity_boost.png"), Color.Black, outline_width));
            img.Add("Power Beam", ImageUtils.MakeOutline(Image.FromFile(@"img/echoes/power_beam.png"), Color.Black, outline_width));
            img.Add("Dark Beam", ImageUtils.MakeOutline(Image.FromFile(@"img/echoes/dark_beam.png"), Color.Black, outline_width));
            img.Add("Light Beam", ImageUtils.MakeOutline(Image.FromFile(@"img/echoes/light_beam.png"), Color.Black, outline_width));
            img.Add("Annihilator Beam", ImageUtils.MakeOutline(Image.FromFile(@"img/echoes/annihilator_beam.png"), Color.Black, outline_width));
            img.Add("Charge Beam", ImageUtils.MakeOutline(Image.FromFile(@"img/echoes/charge_beam.png"), Color.Black, outline_width));
            img.Add("Grapple Beam", ImageUtils.MakeOutline(Image.FromFile(@"img/echoes/grapple_beam.png"), Color.Black, outline_width));
            img.Add("Super Missile", ImageUtils.MakeOutline(Image.FromFile(@"img/echoes/super_missile.png"), Color.Black, outline_width));
            img.Add("Darkburst", ImageUtils.MakeOutline(Image.FromFile(@"img/echoes/darkburst.png"), Color.Black, outline_width));
            img.Add("Sunburst", ImageUtils.MakeOutline(Image.FromFile(@"img/echoes/sunburst.png"), Color.Black, outline_width));
            img.Add("Sonic Boom", ImageUtils.MakeOutline(Image.FromFile(@"img/echoes/sonic_boom.png"), Color.Black, outline_width));
            img.Add("Scan Visor", ImageUtils.MakeOutline(Image.FromFile(@"img/echoes/scan_visor.png"), Color.Black, outline_width));
            img.Add("Dark Visor", ImageUtils.MakeOutline(Image.FromFile(@"img/echoes/dark_visor.png"), Color.Black, outline_width));
            img.Add("Echo Visor", ImageUtils.MakeOutline(Image.FromFile(@"img/echoes/echo_visor.png"), Color.Black, outline_width));
            img.Add("Dark Ammo Expansion", ImageUtils.MakeOutline(Image.FromFile(@"img/echoes/dark_ammo_expansion.png"), Color.Black, outline_width));
            img.Add("Light Ammo Expansion", ImageUtils.MakeOutline(Image.FromFile(@"img/echoes/light_ammo_expansion.png"), Color.Black, outline_width));
            img.Add("Double Damage", ImageUtils.MakeOutline(Image.FromFile(@"img/echoes/double_damage.png"), Color.Black, outline_width));
            img.Add("Cannon Ball", ImageUtils.MakeOutline(Image.FromFile(@"img/echoes/cannon_ball.png"), Color.Black, outline_width));
            img.Add("Energy Transfer Module", ImageUtils.MakeOutline(Image.FromFile(@"img/echoes/energy_transfer_module.png"), Color.Black, outline_width));
            img.Add("Violet Translator", ImageUtils.MakeOutline(Image.FromFile(@"img/echoes/violet_translator.png"), Color.Black, outline_width));
            img.Add("Amber Translator", ImageUtils.MakeOutline(Image.FromFile(@"img/echoes/amber_translator.png"), Color.Black, outline_width));
            img.Add("Emerald Translator", ImageUtils.MakeOutline(Image.FromFile(@"img/echoes/emerald_translator.png"), Color.Black, outline_width));
            img.Add("Cobalt Translator", ImageUtils.MakeOutline(Image.FromFile(@"img/echoes/cobalt_translator.png"), Color.Black, outline_width));
            img.Add("Dark Agon Keys", ImageUtils.MakeOutline(Image.FromFile(@"img/echoes/dark_agon_key.png"), Color.Black, outline_width));
            img.Add("Dark Torvus Keys", ImageUtils.MakeOutline(Image.FromFile(@"img/echoes/dark_torvus_key.png"), Color.Black, outline_width));
            img.Add("Ing Hive Keys", ImageUtils.MakeOutline(Image.FromFile(@"img/echoes/ing_hive_key.png"), Color.Black, outline_width));
            img.Add("Sky Temple Keys", ImageUtils.MakeOutline(Image.FromFile(@"img/echoes/sky_temple_key.png"), Color.Black, outline_width));
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
                return img[pickup];
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
