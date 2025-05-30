
using System.Collections.Generic;
using UnityEngine;

public class Upgrade
{
    public int id;
    public string name; 

    // constructor
    public Upgrade(int id, string name) {
        this.id = id;
        this.name = name;
    }

    // predefined upgrades
    public static List<Upgrade> upgradesList = new List<Upgrade>
    {
        new Upgrade(1, "Health +10"),
        new Upgrade(2, "Health +5"),
        new Upgrade(3, "Fast Duration+"),
        new Upgrade(4, "Slow Duration+"),
        new Upgrade(5, "Fast Cooldown+"),
        new Upgrade(6, "Slow Cooldown+"),
        new Upgrade(7, "Damage Up"),
        new Upgrade(8, "Fire Rate Up"),
        new Upgrade(9, "Slow Movement+"),
        new Upgrade(10, "Fast Movement+"),
        new Upgrade(11, "Boost+"),
        new Upgrade(12, "Shield +1"),
        new Upgrade(13, "Shield Reduction+")
    };

    public static void GetUpgrade(int id)
    {
        if (id == 1)
        {
            PlayerStats.AdditionalHealth += 10;
        }
        if (id == 2)
        {
            PlayerStats.AdditionalHealth += 5;
        }
        if (id == 3)
        {
            SpeedController.FastLimitUp += 0.2f;
        }
        if (id == 4)
        {
            SpeedController.SlowLimitUp += 0.2f;
        }
        if (id == 5)
        {
            SpeedController.FastCooldownReduction += 0.05f;
        }
        if (id == 6)
        {
            SpeedController.SlowCooldownReduction += 0.05f;
        }
        if (id == 7)
        {
            Bullet.DamageUpgrades += 1;
        }
        if (id == 8)
        {
            Shooting.FireRateUp += 0.25f;
        }
        if (id == 9)
        {
            PlayerController.SlowSpeedMod += 0.1f;
        }
        if (id == 10)
        {
            Enemy.FastSpeedMod += 0.05f; // value will be subtracted when being used so its fine to be positive here
            Enemy.FastSpeedMod = Mathf.Min(Enemy.FastSpeedMod, 4.95f); // make sure it doesn't go lower than the enemy move speed
        }
        if (id == 11)
        {
            PlayerController.BoostModifier += 0.1f;
        }
        if (id == 12)
        {
            PlayerStats.AdditionalShield += 1;
        }
        if (id == 13)
        {
            PlayerStats.AdditionalReduction += 0.05f;
        }
    }
}
