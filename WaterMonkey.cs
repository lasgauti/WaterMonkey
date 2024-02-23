using MelonLoader;
using BTD_Mod_Helper;
using WaterMonkey;
using MelonLoader;
using BTD_Mod_Helper;
using BTD_Mod_Helper;
using BTD_Mod_Helper.Api.Components;
using BTD_Mod_Helper.Api.Towers;
using BTD_Mod_Helper.Extensions;
using HarmonyLib;
using Il2Cpp;
using Il2CppAssets.Scripts;
using Il2CppAssets.Scripts.Models;
using Il2CppAssets.Scripts.Models.Profile;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Abilities.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Attack;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Emissions;
using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;
using Il2CppAssets.Scripts.Models.TowerSets;
using Il2CppAssets.Scripts.Simulation.Objects;
using Il2CppAssets.Scripts.Simulation.SimulationBehaviors;
using Il2CppAssets.Scripts.Simulation.Towers;
using Il2CppAssets.Scripts.Simulation.Towers.Projectiles;
using Il2CppAssets.Scripts.Simulation.Towers.Weapons;
using Il2CppAssets.Scripts.Unity;
using Il2CppAssets.Scripts.Unity.Towers.Weapons;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppNewtonsoft.Json.Utilities;
using MelonLoader;
using Il2CppAssets.Scripts.Models.GenericBehaviors;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Attack.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Weapons.Behaviors;
using Il2CppAssets.Scripts.Models.Rounds;
using Il2CppAssets.Scripts.Simulation.Track;
using Il2CppSystem;
using Il2CppAssets.Scripts.Unity.UI_New.Main.HeroSelect;
using Il2CppAssets.Scripts.Unity.UI_New.InGame;
using Il2CppAssets.Scripts.Models.Bloons.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Projectiles;
using Il2CppAssets.Scripts.Utils;
using Il2CppSystem.IO;
using Il2CppAssets.Scripts.Models.Towers.Filters;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Abilities;
using Il2CppAssets.Scripts.Unity.Towers;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Emissions.Behaviors;
using System.Numerics;
using BTD_Mod_Helper.Api.Display;
using Il2CppAssets.Scripts.Unity.Display;
using Il2CppAssets.Scripts.Models.Towers.TowerFilters;
using System;
using System.Linq;
using Il2CppAssets.Scripts.Simulation.Towers.Behaviors.Abilities;
using UnityEngine;
using static Il2CppSystem.TypeIdentifiers;
using Il2CppAssets.Scripts.Simulation.Bloons;
using BTD_Mod_Helper.Api.Enums;

[assembly: MelonInfo(typeof(WaterMonkey.WaterMonkey), ModHelperData.Name, ModHelperData.Version, ModHelperData.RepoOwner)]
[assembly: MelonGame("Ninja Kiwi", "BloonsTD6")]

namespace WaterMonkey;

public class WaterMonkey : BloonsTD6Mod
{
    public override void OnApplicationStart()
    {
        ModHelper.Msg<WaterMonkey>("WaterMonkey loaded!");
    }
    public class WaterMonkeyTower : ModTower
    {
        public override TowerSet TowerSet => TowerSet.Primary;
        public override string BaseTower => TowerType.DartMonkey;
        public override int Cost => 300;
        public override string DisplayName => "Water Monkey";
        public override string Name => "WaterMonkey";
        public override int TopPathUpgrades => 5;
        public override int MiddlePathUpgrades => 5;
        public override int BottomPathUpgrades => 5;
        public override string Description => "Shoot 3 water drop at a slow rate that knockback Bloons. You can only place im on water.";
        public override string Icon => "Icon000";
        public override string Portrait => "Icon000";
        public override ParagonMode ParagonMode => ParagonMode.Base000;
        public override bool IsValidCrosspath(int[] tiers) =>
   ModHelper.HasMod("UltimateCrosspathing") || base.IsValidCrosspath(tiers);

        public override void ModifyBaseTowerModel(TowerModel towerModel)
        {
            var attackModel = towerModel.GetAttackModel();
            var weapon = attackModel.weapons[0];
            var projectile = weapon.projectile;
            Il2CppStructArray<Il2CppAssets.Scripts.Models.Map.AreaType> array;
            array = new[] { Il2CppAssets.Scripts.Models.Map.AreaType.water };
            towerModel.areaTypes = array;
            towerModel.ApplyDisplay<Display000>();
            towerModel.GetBehavior<DisplayModel>().positionOffset = new Il2CppAssets.Scripts.Simulation.SMath.Vector3(0, 0, -5);
            attackModel.range = 45;
        
            towerModel.range = 45;
            weapon.emission = new ArcEmissionModel("ArcEmissionModel_", 3, 0, 60, null, false, false);
            weapon.rate = 1.3f;
            projectile.display = new PrefabReference() { guidRef = GetDisplayGUID<WaterProj1>() };
            projectile.GetDamageModel().damage = 1;
            projectile.GetBehavior<TravelStraitModel>().lifespan *= 4;
            projectile.pierce = 1;
            var knockback = Game.instance.model.GetTowerFromId("NinjaMonkey-010").GetWeapon().projectile.GetBehavior<WindModel>().Duplicate();
            knockback.chance = 1f;
            knockback.distanceMin = 20;
            knockback.distanceMax = 20;
            projectile.AddBehavior(knockback);
        }
    }
    public class Display000 : ModDisplay
    {

        public override string BaseDisplay => GetDisplay(TowerType.ObynGreenfoot);
        public override void ModifyDisplayNode(UnityDisplayNode node)
        {
            foreach (var renderer in node.genericRenderers)
            {
                renderer.material.mainTexture = GetTexture("WaterMonkey000");
            }
        }
    }
    public class WaterSwarm : ModUpgrade<WaterMonkeyTower>
    {

        public override int Path => TOP;
        public override int Tier => 1;
        public override int Cost => 200;

        public override string Icon => "Icon100"; 
        public override string Description => "Shoot 5 water drop instead of 3.";
        public override void ApplyUpgrade(TowerModel towerModel)
        {
            var attackModel = towerModel.GetAttackModel();
            var weapon = attackModel.weapons[0];
            var projectile = weapon.projectile;
            weapon.emission = new ArcEmissionModel("ArcEmissionModel_", 5, 0, 50, null, false, false);
        }
    }
    public class SharpWater : ModUpgrade<WaterMonkeyTower>
    {

        public override int Path => TOP;
        public override int Tier => 2;
        public override int Cost => 500;

        public override string Icon => "Icon200";
        public override string Description => "Water can now pop an extra Bloon and can pop any Bloon Type.";
        public override void ApplyUpgrade(TowerModel towerModel)
        {
            var attackModel = towerModel.GetAttackModel();
            var weapon = attackModel.weapons[0];
            var projectile = weapon.projectile;
            projectile.pierce += 1;
            projectile.GetDamageModel().immuneBloonProperties = BloonProperties.None;
            projectile.display = new PrefabReference() { guidRef = GetDisplayGUID<WaterProj4>() };
        }
    }
    public class StrongFriendship : ModUpgrade<WaterMonkeyTower>
    {

        public override int Path => TOP;
        public override int Tier => 3;
        public override int Cost => 1500;
        public override string Icon => "Icon300";
        public override string Description => "Increase Pierce and grant a range and pierce boost of nearby Water Monkey.";
        public override void ApplyUpgrade(TowerModel towerModel)
        {
            var attackModel = towerModel.GetAttackModel();
            var weapon = attackModel.weapons[0];
            var projectile = weapon.projectile;
            projectile.pierce += 2;
            towerModel.AddBehavior(new RangeSupportModel("RangeBuff", true, 0.2f, 0f, "RangeBuff", new Il2CppReferenceArray<TowerFilterModel>(new TowerFilterModel[]
                {
                    new FilterInBaseTowerIdModel("FilterInBaseTowerIdModel_",
                        new Il2CppStringArray(new[] { WaterMonkeyTower.TowerID<WaterMonkeyTower>() }))
                }), false, "RangeBuff_", null));
            towerModel.AddBehavior(new PierceSupportModel("PierceBoost", true, 1f, "PierceBooost", new Il2CppReferenceArray<TowerFilterModel>(new TowerFilterModel[]
               {
                    new FilterInBaseTowerIdModel("FilterInBaseTowerIdModel_",
                        new Il2CppStringArray(new[] { WaterMonkeyTower.TowerID<WaterMonkeyTower>() }))
               }), false, "PierceBoost_", null));
            towerModel.GetBehavior<PierceSupportModel>().ApplyBuffIcon<IconBuffT3>();
            projectile.display = new PrefabReference() { guidRef = GetDisplayGUID<WaterProj5>() };
        }
    }
    public class IconBuffT3 : ModBuffIcon
    {
        protected override int Order => 1;
        public override string Icon => "Icon300";
        public override int MaxStackSize => 3;
    }
    public class IconBuffT4 : ModBuffIcon
    {
        protected override int Order => 1;
        public override string Icon => "Icon400";
        public override int MaxStackSize => 3;
    }
    public class IconBuffT5 : ModBuffIcon
    {
        protected override int Order => 1;
        public override string Icon => "Icon500";
        public override int MaxStackSize => 3;
    }
    public class StrongerFriendship : ModUpgrade<WaterMonkeyTower>
    {

        public override int Path => TOP;
        public override int Tier => 4;
        public override int Cost => 5000;
        public override string Icon => "Icon400";
        public override string Description => "Increase Popping Power. Now also grant more damage as well as more pierce and fire rate to nearby Water Monkey";
        public override void ApplyUpgrade(TowerModel towerModel)
        {
            var attackModel = towerModel.GetAttackModel();
            var weapon = attackModel.weapons[0];
            var projectile = weapon.projectile;
            projectile.pierce += 2;
            projectile.GetDamageModel().damage += 1;
            weapon.rate *= 0.75f;
            attackModel.range += 5;
            towerModel.range += 5;
            towerModel.RemoveBehavior<PierceSupportModel>();
            towerModel.AddBehavior(new PierceSupportModel("PierceBoost3", true, 3f, "PierceBooost3", new Il2CppReferenceArray<TowerFilterModel>(new TowerFilterModel[]
               {
                    new FilterInBaseTowerIdModel("FilterInBaseTowerIdModel_",
                        new Il2CppStringArray(new[] { WaterMonkeyTower.TowerID<WaterMonkeyTower>() }))
               }), false, "PierceBoost_", null));
            towerModel.GetBehavior<PierceSupportModel>().ApplyBuffIcon<IconBuffT4>();
            towerModel.AddBehavior(new DamageSupportModel("DamageBoos3t", true, 1f, "DamageBoost3", new Il2CppReferenceArray<TowerFilterModel>(new TowerFilterModel[]
             {
                    new FilterInBaseTowerIdModel("FilterInBaseTowerIdModel_",
                        new Il2CppStringArray(new[] { WaterMonkeyTower.TowerID<WaterMonkeyTower>() }))
             }), false, false, 60f));
            towerModel.AddBehavior(new RateSupportModel("AttackSpeedBoost3", 0.85f, true, "AttackSpeedBoost3", false, 1, new Il2CppReferenceArray<TowerFilterModel>(new TowerFilterModel[]
             {
                    new FilterInBaseTowerIdModel("FilterInBaseTowerIdModel_",
                        new Il2CppStringArray(new[] {  WaterMonkeyTower.TowerID<WaterMonkeyTower>() }))
             }), "AttackSpeedBoost_", null));
        }
    }
    public class PowerOfFriendship : ModUpgrade<WaterMonkeyTower>
    {

        public override int Path => TOP;
        public override int Tier => 5;
        public override int Cost => 40000;
        public override string Icon => "Icon500";
        public override string Description => "The Power Of Friendship.";
        public override void ApplyUpgrade(TowerModel towerModel)
        {
            var attackModel = towerModel.GetAttackModel();
            var weapon = attackModel.weapons[0];
            var projectile = weapon.projectile;
            projectile.pierce += 6;
            projectile.GetDamageModel().damage += 2;
            weapon.rate *= 0.85f;
            towerModel.RemoveBehavior<PierceSupportModel>();
            towerModel.AddBehavior(new PierceSupportModel("PierceBoos4t", true, 5f, "PierceBooost4", new Il2CppReferenceArray<TowerFilterModel>(new TowerFilterModel[]
               {
                    new FilterInBaseTowerIdModel("FilterInBaseTowerIdModel_",
                        new Il2CppStringArray(new[] { WaterMonkeyTower.TowerID<WaterMonkeyTower>() }))
               }), false, "PierceBoost_", null));
            towerModel.GetBehavior<PierceSupportModel>().ApplyBuffIcon<IconBuffT5>();
            towerModel.RemoveBehavior<DamageSupportModel>();
            towerModel.AddBehavior(new DamageSupportModel("DamageBoost4", true, 2f, "DamageBoost4", new Il2CppReferenceArray<TowerFilterModel>(new TowerFilterModel[]
             {
                    new FilterInBaseTowerIdModel("FilterInBaseTowerIdModel_",
                        new Il2CppStringArray(new[] { WaterMonkeyTower.TowerID<WaterMonkeyTower>() }))
             }), false, false, 60f));
            towerModel.RemoveBehavior<RateSupportModel>();
            towerModel.AddBehavior(new RateSupportModel("AttackSpeedBoost4", 0.72f, true, "AttackSpeedBoost4", false, 1, new Il2CppReferenceArray<TowerFilterModel>(new TowerFilterModel[]
             {
                    new FilterInBaseTowerIdModel("FilterInBaseTowerIdModel_",
                        new Il2CppStringArray(new[] {  WaterMonkeyTower.TowerID<WaterMonkeyTower>() }))
             }), "AttackSpeedBoost_", null));
            towerModel.RemoveBehavior<RangeSupportModel>();
            towerModel.AddBehavior(new RangeSupportModel("RangeBuff4", true, 0.35f, 0f, "RangeBuff4", new Il2CppReferenceArray<TowerFilterModel>(new TowerFilterModel[]
             {
                    new FilterInBaseTowerIdModel("FilterInBaseTowerIdModel_",
                        new Il2CppStringArray(new[] { WaterMonkeyTower.TowerID<WaterMonkeyTower>() }))
             }), false, "RangeBuff_", null));
            towerModel.AddBehavior(new FreeUpgradeSupportModel("FreeUpgrade4", 2, "FreeUpgrade4", new Il2CppReferenceArray<TowerFilterModel>(new TowerFilterModel[]
            {
                    new FilterInBaseTowerIdModel("FilterInBaseTowerIdModel_",
                        new Il2CppStringArray(new[] { WaterMonkeyTower.TowerID<WaterMonkeyTower>() }))
            })));
            projectile.display = new PrefabReference() { guidRef = GetDisplayGUID<WaterProj6>() };
        }
    }
    public class FastHand : ModUpgrade<WaterMonkeyTower>
    {

        public override int Path => MIDDLE;
        public override int Tier => 1;
        public override int Cost => 175;

        public override string Icon => "Icon010";
        public override string Description => "Shoot 33% faster";
        public override void ApplyUpgrade(TowerModel towerModel)
        {
            var attackModel = towerModel.GetAttackModel();
            var weapon = attackModel.weapons[0];
            var projectile = weapon.projectile;
            weapon.rate *= 0.77f;
        }
    }
    public class FurtherKnockback : ModUpgrade<WaterMonkeyTower>
    {

        public override int Path => MIDDLE;
        public override int Tier => 2;
        public override int Cost => 200;
        public override string Icon => "Icon020";

        public override string Description => "Increase knockback strength";
        public override void ApplyUpgrade(TowerModel towerModel)
        {
            var attackModel = towerModel.GetAttackModel();
            var weapon = attackModel.weapons[0];
            var projectile = weapon.projectile;
            var knockback = projectile.GetBehavior<WindModel>();
            knockback.distanceMax += 20;
            knockback.distanceMin += 20;
        }
    }
    public class StrongWind : ModUpgrade<WaterMonkeyTower>
    {

        public override int Path => MIDDLE;
        public override int Tier => 3;
        public override int Cost => 1250;

        public override string Icon => "Icon030";
        public override string Description => "Can now summons tornadoes that push back Bloons. Also increase knockback.";
        public override void ApplyUpgrade(TowerModel towerModel)
        {
            var attackModel = towerModel.GetAttackModel();
            var weapon = attackModel.weapons[0];
            var projectile = weapon.projectile;
            var knockback = projectile.GetBehavior<WindModel>();
            knockback.distanceMax += 15;
            knockback.distanceMin += 15;
            var druid = Game.instance.model.GetTower(TowerType.Druid, 3);
            var tornado = druid.GetAttackModels().First(model => model.name.Contains("Tornado")).Duplicate();
            tornado.weapons[0].animation = 1;
            tornado.range = towerModel.range;
            tornado.weapons[0].rate *= 1.15f;
            towerModel.AddBehavior(tornado);
            projectile.display = new PrefabReference() { guidRef = GetDisplayGUID<WaterProj7>() };
        }
    }
    public class UnderwaterLife : ModUpgrade<WaterMonkeyTower>
    {

        public override int Path => MIDDLE;
        public override int Tier => 4;
        public override int Cost => 4500;
        public override string Icon => "Icon040";
        public override string Description => "Further increase knockback strength. Underwater Life Ability: Generate 5 Lives and 1 extra for each Water Monkey on the Map.";
        public override void ApplyUpgrade(TowerModel towerModel)
        {
            var attackModel = towerModel.GetAttackModel();
            var weapon = attackModel.weapons[0];
            var projectile = weapon.projectile;
            var knockback = projectile.GetBehavior<WindModel>();
            knockback.distanceMax += 25;
            knockback.distanceMin += 25;
            var abilityModel = new AbilityModel("AbilityModel_UnderwaterWave", "Underwater Wave",
            "Generate 10 Lives and 3 extra for each Water Monkey on the Map.", 1, 0,
            GetSpriteReference(Icon), 50f, null, false, false, null,
            0, 0, 9999999, false, false);
            var sound = Game.instance.model.GetTower(TowerType.Druid, 0, 5).GetAbility().GetBehavior<CreateSoundOnAbilityModel>();
            abilityModel.AddBehavior(sound);
            towerModel.AddBehavior(abilityModel);
        }
    }
    public class EconomicSea : ModUpgrade<WaterMonkeyTower>
    {

        public override int Path => MIDDLE;
        public override int Tier => 5;
        public override int Cost => 32000;

        public override string Icon => "Icon050";
        public override string Description => "Knockback is way stronger, Underwater Life give 10 Lives and 2 extra per Water Monkey. Economic Sea Ability: Generate $250 and $75 extra for each Water Monkey on the Map. Every 5 damage he deal, he make 1 cash.";
        public override void ApplyUpgrade(TowerModel towerModel)
        {
            var attackModel = towerModel.GetAttackModel();
            var weapon = attackModel.weapons[0];
            var projectile = weapon.projectile;
            var knockback = projectile.GetBehavior<WindModel>();
            var tornado = towerModel.GetAttackModels().First(model => model.name.Contains("Tornado")).Duplicate();
            tornado.weapons[0].rate *= 0.35f;
            tornado.weapons[0].projectile.GetBehavior<TravelStraitModel>().lifespan *= 3;
            knockback.distanceMax += 50;
            knockback.distanceMin += 50;
            projectile.pierce += 4;
            projectile.GetDamageModel().damage += 2;
            projectile.display = new PrefabReference() { guidRef = GetDisplayGUID<WaterProj2>() };

            var abilityModel = new AbilityModel("AbilityModel_EconomicSea", "Economic Sea",
            "Generate 10 Lives and 3 extra for each Water Monkey on the Map.", 1, 0,
            GetSpriteReference(Icon), 100f, null, false, false, null,
            0, 0, 9999999, false, false);
            var sound = Game.instance.model.GetTower(TowerType.Druid, 0, 5).GetAbility().GetBehavior<CreateSoundOnAbilityModel>();
            abilityModel.AddBehavior(sound);
            towerModel.AddBehavior(abilityModel);
        }
    }
    public class RangedWater : ModUpgrade<WaterMonkeyTower>
    {

        public override int Path => BOTTOM;
        public override int Tier => 1;
        public override int Cost => 125;

        public override string Icon => "Icon001";
        public override string Description => "Increase range of the Water Monkey.";
        public override void ApplyUpgrade(TowerModel towerModel)
        {
            var attackModel = towerModel.GetAttackModel();
            var weapon = attackModel.weapons[0];
            var projectile = weapon.projectile;
            towerModel.range += 10;
            attackModel.range += 10;
        }
    }
    public class AdvancedWater : ModUpgrade<WaterMonkeyTower>
    {

        public override int Path => BOTTOM;
        public override int Tier => 2;
        public override int Cost => 350;

        public override string Icon => "Icon002";
        public override string Description => "Increase range and can now pop Camo Bloons.";
        public override void ApplyUpgrade(TowerModel towerModel)
        {
            var attackModel = towerModel.GetAttackModel();
            var weapon = attackModel.weapons[0];
            var projectile = weapon.projectile;
            towerModel.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
            towerModel.range += 5;
            attackModel.range += 5;
        }
    }
    public class DeadlyWater : ModUpgrade<WaterMonkeyTower>
    {

        public override int Path => BOTTOM;
        public override int Tier => 3;
        public override int Cost => 1400;

        public override string Icon => "Icon003";
        public override string Description => "Further increase range and damage.";
        public override void ApplyUpgrade(TowerModel towerModel)
        {
            var attackModel = towerModel.GetAttackModel();
            var weapon = attackModel.weapons[0];
            var projectile = weapon.projectile;
            towerModel.range += 7;
            attackModel.range += 7;
            projectile.GetDamageModel().damage += 3;
            projectile.display = new PrefabReference() { guidRef = GetDisplayGUID<WaterProj8>() };
        }
    }
    public class DangerousWave : ModUpgrade<WaterMonkeyTower>
    {

        public override int Path => BOTTOM;
        public override int Tier => 4;
        public override int Cost => 7200;

        public override string Icon => "Icon004";
        public override string Description => "Summons a deadly wave at a slow rate. Also increase damage and pierce.";
        public override void ApplyUpgrade(TowerModel towerModel)
        {
            var attackModel = towerModel.GetAttackModel();
            var weapon = attackModel.weapons[0];
            var projectile = weapon.projectile;

            towerModel.range += 6;
            attackModel.range += 6;
            projectile.GetDamageModel().damage += 2;
            var wave = attackModel.Duplicate();
            wave.weapons[0].emission = new SingleEmissionModel("SingleEmissionModel", null);
            wave.weapons[0].rate = 4.5f;
            wave.range = towerModel.range;
            wave.name = "AttackModel_DangerousWave";
            wave.weapons[0].projectile.GetBehavior<TravelStraitModel>().speed *= 1.5f;
            wave.weapons[0].projectile.GetDamageModel().damage = 125;
            wave.weapons[0].projectile.pierce = 30;
            wave.weapons[0].projectile.display = new PrefabReference() { guidRef = GetDisplayGUID<Wave>() };
            wave.weapons[0].projectile.scale *= 2;
            wave.weapons[0].projectile.radius *= 2;
            wave.weapons[0].projectile.GetDamageModel().immuneBloonProperties = BloonProperties.None;
            towerModel.AddBehavior(wave);
            towerModel.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
            projectile.GetDamageModel().damage += 5;
            projectile.pierce += 3;
            weapon.rate *= 0.9f;
        }
    }
    public class SupersonicWater : ModUpgrade<WaterMonkeyTower>
    {

        public override int Path => BOTTOM;
        public override int Tier => 5;
        public override int Cost => 45000;

        public override string Icon => "Icon005";
        public override string Description => "Shoot extremely fast and increase dangerous wave strength and knockback.";
        public override void ApplyUpgrade(TowerModel towerModel)
        {
            var attackModel = towerModel.GetAttackModel();
            var weapon = attackModel.weapons[0];
            var projectile = weapon.projectile;

            towerModel.range += 11;
            attackModel.range += 11;
            projectile.GetDamageModel().damage += 2;
            var wave = towerModel.GetAttackModel("AttackModel_DangerousWave");

            wave.weapons[0].rate /= 1.6f; ;
            wave.weapons[0].projectile.GetBehavior<WindModel>().affectMoab = true;
            wave.weapons[0].projectile.GetBehavior<WindModel>().distanceMin += 75;
            wave.weapons[0].projectile.GetBehavior<WindModel>().distanceMax += 75;
            wave.weapons[0].projectile.GetBehavior<TravelStraitModel>().speed *= 1.5f;
            wave.weapons[0].projectile.GetDamageModel().damage += 275;
            wave.weapons[0].projectile.pierce += 30;
            wave.weapons[0].projectile.scale *= 1.5f;
            wave.weapons[0].projectile.radius *= 1.5f;
            projectile.GetDamageModel().damage += 2;
            projectile.pierce += 2;
            weapon.rate *= 0.185f;
            projectile.display = new PrefabReference() { guidRef = GetDisplayGUID<WaterProj9>() };
        }
    }
    public class WaveMaster : ModParagonUpgrade<WaterMonkeyTower>
    {
        public override int Cost => 550000;
        public override string DisplayName => "Wave Master";
        public override string Description => "Master the way of sea.";
        public override string Portrait => "PortraitParagon";
        public override string Icon => "IconParagon";
        public override void ApplyUpgrade(TowerModel towerModel)
        {
            var attackModel = towerModel.GetAttackModel();
            var weapon = attackModel.weapons[0];
            var projectile = weapon.projectile;
            var knockback = projectile.GetBehavior<WindModel>();
            attackModel.range = 110;
            towerModel.range = 110;
            projectile.GetBehavior<TravelStraitModel>().speed *= 1.5f;
            projectile.display = new PrefabReference() { guidRef = GetDisplayGUID<WaterProj3>() };
            weapon.emission = new ArcEmissionModel("ArcEmissionModel_", 12, 0, 45, null, false, false);
            weapon.rate = 0.097f;
            projectile.pierce = 75;
            projectile.GetDamageModel().damage = 12f;
            projectile.GetDamageModel().immuneBloonProperties = BloonProperties.None;
            knockback.distanceMax = 175;
            knockback.distanceMin = 175;
            knockback.affectMoab = true;
            var wave = attackModel.Duplicate();
            wave.weapons[0].emission = new ArcEmissionModel("ArcEmissionModel_", 3, 0, 35, null, false, false);
            wave.weapons[0].rate = 1.75f;
            wave.range = towerModel.range;
            wave.name = "AttackModel_DangerousWave";
            wave.weapons[0].projectile.GetBehavior<TravelStraitModel>().speed *= 2f;
            wave.weapons[0].projectile.GetDamageModel().damage = 1250;
            wave.weapons[0].projectile.pierce = 200;
            wave.weapons[0].projectile.display = new PrefabReference() { guidRef = GetDisplayGUID<Wave2>() };
            wave.weapons[0].projectile.scale *= 3;
            wave.weapons[0].projectile.radius *= 3;
            wave.weapons[0].projectile.GetDamageModel().immuneBloonProperties = BloonProperties.None;
            towerModel.AddBehavior(wave);
            towerModel.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
            var drop = attackModel.Duplicate();
            drop.weapons[0].emission = new ArcEmissionModel("ArcEmissionModel_", 16, 0, 360, null, false, false);
            drop.weapons[0].rate = 0.5f;
            drop.range = towerModel.range;
            drop.name = "AttackModel_WaterDrop";
            drop.weapons[0].projectile.GetBehavior<TravelStraitModel>().speed *= 0.75f;
            drop.weapons[0].projectile.GetDamageModel().damage = 350;
            drop.weapons[0].projectile.pierce = 10;
            drop.weapons[0].projectile.display = new PrefabReference() { guidRef = GetDisplayGUID<WaterDrop>() };
            drop.weapons[0].projectile.scale *= 0.75f;
            drop.weapons[0].projectile.radius *= 0.75f;
            drop.weapons[0].projectile.GetDamageModel().immuneBloonProperties = BloonProperties.None;
            towerModel.AddBehavior(drop);
            towerModel.displayScale *= 1.35f;
            towerModel.GetBehavior<DisplayModel>().positionOffset = new Il2CppAssets.Scripts.Simulation.SMath.Vector3(0, 0, -7.5f);
            var abilityModel = new AbilityModel("AbilityModel_Paragon", "Paragon",
        "N/A", 1, 0,
        GetSpriteReference(Icon), 90f, null, false, false, null,
        0, 0, 9999999, false, false);
            var sound = Game.instance.model.GetTower(TowerType.Druid, 0, 5).GetAbility().GetBehavior<CreateSoundOnAbilityModel>();
            abilityModel.AddBehavior(sound);
            towerModel.AddBehavior(abilityModel);
            towerModel.AddBehavior(new PierceSupportModel("PierceBoost1", true, 10f, "PierceBooost1", new Il2CppReferenceArray<TowerFilterModel>(new TowerFilterModel[]
            {
                    new FilterInBaseTowerIdModel("FilterInBaseTowerIdModel_",
                        new Il2CppStringArray(new[] { WaterMonkeyTower.TowerID<WaterMonkeyTower>() }))
            }), false, "PierceBoost_", null));
            towerModel.GetBehavior<PierceSupportModel>().ApplyBuffIcon<IconBuffParagon>();
            towerModel.AddBehavior(new DamageSupportModel("DamageBoost1", true, 5f, "DamageBoost1", new Il2CppReferenceArray<TowerFilterModel>(new TowerFilterModel[]
             {
                    new FilterInBaseTowerIdModel("FilterInBaseTowerIdModel_",
                        new Il2CppStringArray(new[] { WaterMonkeyTower.TowerID<WaterMonkeyTower>() }))
             }), false, false, 60f));
            towerModel.AddBehavior(new RateSupportModel("AttackSpeedBoost1", 0.65f, true, "AttackSpeedBoost1", false, 1, new Il2CppReferenceArray<TowerFilterModel>(new TowerFilterModel[]
             {
                    new FilterInBaseTowerIdModel("FilterInBaseTowerIdModel_",
                        new Il2CppStringArray(new[] {  WaterMonkeyTower.TowerID<WaterMonkeyTower>() }))
             }), "AttackSpeedBoost_", null));
            towerModel.AddBehavior(new RangeSupportModel("RangeBuff1", true, 0.5f, 0f, "RangeBuff1", new Il2CppReferenceArray<TowerFilterModel>(new TowerFilterModel[]
             {
                    new FilterInBaseTowerIdModel("FilterInBaseTowerIdModel_",
                        new Il2CppStringArray(new[] { WaterMonkeyTower.TowerID<WaterMonkeyTower>() }))
             }), false, "RangeBuff_", null));
            towerModel.AddBehavior(new FreeUpgradeSupportModel("FreeUpgrade1", 3, "FreeUpgrade1", new Il2CppReferenceArray<TowerFilterModel>(new TowerFilterModel[]
            {
                    new FilterInBaseTowerIdModel("FilterInBaseTowerIdModel_",
                        new Il2CppStringArray(new[] { WaterMonkeyTower.TowerID<WaterMonkeyTower>() }))

            })));
            
        }

    }
    public class IconBuffParagon : ModBuffIcon
    {
        protected override int Order => 1;
        public override string Icon => "IconParagon";
        public override int MaxStackSize => 3;
    }
    public class ParagonDisplay : ModTowerDisplay<WaterMonkeyTower>
    {
        public override string BaseDisplay => GetDisplay(TowerType.ObynGreenfoot);

        public override bool UseForTower(int[] tiers)
        {
            return IsParagon(tiers);
        }

        public override int ParagonDisplayIndex => 0;

        public override void ModifyDisplayNode(UnityDisplayNode node)
        {
            foreach (var renderer in node.genericRenderers)
            {
                renderer.material.mainTexture = GetTexture("WaterMonkeyParagon");
            }
        }
    }
    public class WaterProj1 : ModDisplay
    {
        public override string BaseDisplay => Generic2dDisplay;
        public override float Scale => 1f;

        public override void ModifyDisplayNode(UnityDisplayNode node)
        {
            Set2DTexture(node, "WaterProjectile1");
        }
    }
    public class WaterProj2 : ModDisplay
    {
        public override string BaseDisplay => Generic2dDisplay;
        public override float Scale => 1f;

        public override void ModifyDisplayNode(UnityDisplayNode node)
        {
            Set2DTexture(node, "WaterProjectile2");
        }
    }
    public class WaterProj3 : ModDisplay
    {
        public override string BaseDisplay => Generic2dDisplay;
        public override float Scale => 1f;

        public override void ModifyDisplayNode(UnityDisplayNode node)
        {
            Set2DTexture(node, "WaterProjectile3");
        }
    }
    public class WaterProj4 : ModDisplay
    {
        public override string BaseDisplay => Generic2dDisplay;
        public override float Scale => 1f;

        public override void ModifyDisplayNode(UnityDisplayNode node)
        {
            Set2DTexture(node, "WaterProjectile4");
        }
    }
    public class WaterProj5 : ModDisplay
    {
        public override string BaseDisplay => Generic2dDisplay;
        public override float Scale => 1f;

        public override void ModifyDisplayNode(UnityDisplayNode node)
        {
            Set2DTexture(node, "WaterProjectile5");
        }
    }
    public class WaterProj6 : ModDisplay
    {
        public override string BaseDisplay => Generic2dDisplay;
        public override float Scale => 1f;

        public override void ModifyDisplayNode(UnityDisplayNode node)
        {
            Set2DTexture(node, "WaterProjectile6");
        }
    }
    public class WaterProj7 : ModDisplay
    {
        public override string BaseDisplay => Generic2dDisplay;
        public override float Scale => 1f;

        public override void ModifyDisplayNode(UnityDisplayNode node)
        {
            Set2DTexture(node, "WaterProjectile7");
        }
    }
    public class WaterProj8 : ModDisplay
    {
        public override string BaseDisplay => Generic2dDisplay;
        public override float Scale => 1f;

        public override void ModifyDisplayNode(UnityDisplayNode node)
        {
            Set2DTexture(node, "WaterProjectile8");
        }
    }
    public class WaterProj9 : ModDisplay
    {
        public override string BaseDisplay => Generic2dDisplay;
        public override float Scale => 1f;

        public override void ModifyDisplayNode(UnityDisplayNode node)
        {
            Set2DTexture(node, "WaterProjectile9");
        }
    }
    public class Wave : ModDisplay
    {
        public override string BaseDisplay => Generic2dDisplay;
        public override float Scale => 1.5f;

        public override void ModifyDisplayNode(UnityDisplayNode node)
        {
            Set2DTexture(node, "Wave1");
        }
    }
    public class Wave2 : ModDisplay
    {
        public override string BaseDisplay => Generic2dDisplay;
        public override float Scale => 1.5f;

        public override void ModifyDisplayNode(UnityDisplayNode node)
        {
            Set2DTexture(node, "Wave2");
        }
    }
    public class WaterDrop : ModDisplay
    {
        public override string BaseDisplay => Generic2dDisplay;
        public override float Scale => 1f;

        public override void ModifyDisplayNode(UnityDisplayNode node)
        {
            Set2DTexture(node, "WaterDrop");
        }
    }
    public override void OnAbilityCast(Ability ability)
    {
        if (ability.abilityModel.name.Contains("AbilityModel_UnderwaterWave"))
        {
            InGame game = InGame.instance;
            if (ability.tower.towerModel.tiers[1] == 4)
            {
                var health = 5;
                var towers = InGame.Bridge.GetAllTowers();
                foreach (var tts in towers.ToList())
                {
                    if (tts.tower.model.name.Contains("WaterMonkey"))
                    {
                        health += 1;
                    }
                }
                game.AddHealth(health);
            }
            else
            {
                var health = 10;
                var towers = InGame.Bridge.GetAllTowers();
                foreach (var tts in towers.ToList())
                {
                    if (tts.tower.model.name.Contains("WaterMonkey"))
                    {
                        health += 2;
                    }
                }
                game.AddHealth(health);
            }
        }
        if (ability.abilityModel.name.Contains("AbilityModel_EconomicSea"))
        {
            InGame game = InGame.instance;
            var cash = 250;
            var towers = InGame.Bridge.GetAllTowers();
            foreach (var tts in towers.ToList())
            {
                if (tts.tower.model.name.Contains("WaterMonkey"))
                {
                    cash += 75;
                }
            }
            ability.tower.cashEarned += cash;
            game.AddCash(cash);
        }
        if (ability.abilityModel.name.Contains("AbilityModel_Paragon"))
        {
            InGame game = InGame.instance;
            var cash = 350;
            var health = 12;
            var towers = InGame.Bridge.GetAllTowers();
            foreach (var tts in towers.ToList())
            {
                if (tts.tower.model.name.Contains("WaterMonkey"))
                {
                    cash += 85;
                    health += 3;
                }
            }
            ability.tower.cashEarned += cash;
            game.AddCash(cash);
            game.AddHealth(health);
        }
    }
    [HarmonyPatch(typeof(Bloon), nameof(Bloon.Damage))]
    internal static class Bloon_Damage
    {
        [HarmonyPrefix]
        private static void Prefix(Bloon __instance, float totalAmount, Projectile projectile, bool distributeToChildren, bool overrideDistributeBlocker, bool createEffect, Il2CppAssets.Scripts.Simulation.Towers.Tower tower, BloonProperties immuneBloonProperties = BloonProperties.None, bool canDestroyProjectile = true, bool ignoreNonTargetable = false, bool blockSpawnChildren = false, bool ignoreInvunerable = false)
        {
            //__instance,
            //totalAmount, projectile, distributeToChildren, overrideDistributeBlocker, createEffect, tower, immuneBloonProperties, canDestroyProjectile, ignoreNonTargetable, blockSpawnChildren, ignoreInvunerable
            if (!(tower is null))
            {
                var towerModel = tower.towerModel;
               
                if (towerModel.name.Contains("WaterMonkey") && towerModel.tiers[1] == 5)
                {
                    var cash = Mathf.Round(totalAmount / 5);
                    InGame.instance.AddCash(cash);
                    tower.cashEarned += (long)cash;
                }
                if (towerModel.name.Contains("WaterMonkey") && towerModel.isParagon)
                {
                    var cash = Mathf.Round(totalAmount / 1250);
                    InGame.instance.AddCash(cash);
                    tower.cashEarned += (long)cash;
                }
            }
        }
    }
}