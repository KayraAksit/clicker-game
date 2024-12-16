using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class DamageManagerScript : MonoBehaviour
{
    public CoinManager coinManager;  // Reference to CoinManager for upgrading
    public damageType TapDamage = new damageType(10, 10, 5, 1.2f, 1); // Tap Damage
    public List<damageType> passiveDamages = new List<damageType>(); //List to hold passive damages
    public int totalPower;

    public class damageType //struct for holding a damage
    {
        public int Damage { get; set; } //Amount of damage 
        public int upgradeCost { get; set; } //Cost of next upgrade
        public int upgradeConstant { get; set; } //Damage increase with each upgrade
        public float upgradeMultiplier { get; set; } //Multiplier to increase upgrade cost with each upgrade
        public int Level { get; set; } //Current upgrade level

        public damageType(int damage, int cost, int constant, float multip, int level)
        {
            Damage = damage;
            upgradeCost = cost;
            upgradeConstant = constant;
            upgradeMultiplier = multip;
            Level = level;
        }

    }



    public void Start()
    {
        passiveDamages.Add(new damageType(5, 20, 5, 1.2f, 0));
        passiveDamages.Add(new damageType(50, 500, 40, 1.4f, 0));
        for (int i = 0; i < passiveDamages.Count ; i++) //Add dummy passive damages with 0 level. TODO: fix, properly add passive damages
        {         
            totalPower += passiveDamages[i].Damage;
        }
        totalPower += TapDamage.Damage;
    }

    // Method to handle upgrading damage
    public void UpgradeTapDamage()
    {
        if (coinManager.coins >= TapDamage.upgradeCost)
        {
            coinManager.coins -= TapDamage.upgradeCost;  // Deduct coins for upgrade
            TapDamage.Damage += TapDamage.upgradeConstant;     // Increase tap damage
            TapDamage.Level += 1;
            TapDamage.upgradeCost = Mathf.RoundToInt(TapDamage.upgradeCost * TapDamage.upgradeMultiplier); //Exponentially increase upgrade cost

            coinManager.UpdateCoinText();
            totalPower += TapDamage.upgradeConstant;
        }
    }
    public void UpgradePassiveDamage(int idx)
    {
        /*
        if(passiveDamages.Count < idx+1) //passive damage is not yet unlocked 
        {
            if(coinManager.coins >= 30) // Check if enough conins for upgrade. TODO: fix 30
            {
                coinManager.coins -= 30;
                passiveDamages.Add(new damageType(5, 20, 5, 1.2f, 1));
            }
        }
        */
        if (coinManager.coins >= passiveDamages[idx].upgradeCost) //passive damage is unlocked, try upgrade
        {
            coinManager.coins -= passiveDamages[idx].upgradeCost;  // Deduct coins for upgrade
            //passiveDamages[idx].Damage += passiveDamages[idx].upgradeConstant;     // Increase tap damage
            passiveDamages[idx].Level += 1;
            passiveDamages[idx].upgradeCost = Mathf.RoundToInt(passiveDamages[idx].upgradeCost * passiveDamages[idx].upgradeMultiplier); //Exponentially increase upgrade cost

            coinManager.UpdateCoinText();
            totalPower += passiveDamages[idx].upgradeConstant;
        }
    }


    public int getTotalPassiveDamage()
    {
        int res = 0;
        foreach (var item in passiveDamages)
        {
            res += item.Damage *item.Level;
        }
        return res;
    }

}
