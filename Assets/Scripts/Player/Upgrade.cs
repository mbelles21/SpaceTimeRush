
using System.Collections.Generic;

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
        new Upgrade(3, "Fast Limit Up"),
        new Upgrade(4, "Slow Limit Up"),
        new Upgrade(5, "Fast Cooldown Reduction"),
        new Upgrade(6, "Slow Cooldown Reduction"),
        new Upgrade(7, "Damage Up"),
        new Upgrade(8, "Fire Rate Up")
    };

    public static void GetUpgrade(int id)
    {
        if(id == 1) {
            PlayerStats.AdditionalHealth += 10;
        }
        if(id == 2) {
            PlayerStats.AdditionalHealth += 5;
        }
        if(id == 3) {
            SpeedController.FastLimitUp += 0.5f; // gain 30 sec
        }
        if(id == 4) {
            SpeedController.SlowLimitUp += 0.5f; // gain 30 sec
        }
        if(id == 5) {
            SpeedController.FastCooldownReduction += 0.05f;
        }
        if(id == 6) {
            SpeedController.SlowCooldownReduction += 0.05f;
        }
        if(id == 7) {
            Bullet.DamageUpgrades += 1;
        }
        if(id == 8) {
            Shooting.FireRateUp += 0.25f;
        }
    }
}
