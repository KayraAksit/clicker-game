using UnityEngine;
using TMPro;

public class UpgradeTapTextsScript : MonoBehaviour
{

    public DamageManagerScript damageManager;
    public TextMeshProUGUI costText;
    public TextMeshProUGUI levelText;
    public int damageIndex;

    private DamageManagerScript.damageType damageType;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(damageIndex == -1)
        {
            damageType = damageManager.TapDamage;
        }
        else if(damageManager.passiveDamages.Count >= damageIndex)
        {
            damageType = damageManager.passiveDamages[damageIndex];
        }

        costText.text = "Upgrade Cost: " + damageType.upgradeCost;
        levelText.text = "Current Level: " + damageType.Level;
    }

    public void UpdateText()
    {
        costText.text = "Upgrade Cost: " + damageType.upgradeCost;
        levelText.text = "Current Level: " + damageType.Level;
    }
}
