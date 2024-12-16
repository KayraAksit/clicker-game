using UnityEngine;

public class ButtonToggleScript : MonoBehaviour
{
    public GameObject upgradeMenuPanel;  // Reference to your Upgrade Menu Panel
    public bool isToogled = false;

    // This function will be called when the Menu Button is pressed
    public void ToggleUpgradeMenu()
    {
        if (upgradeMenuPanel != null)
        {    
            // Toggle the panel's active state
            upgradeMenuPanel.SetActive(!upgradeMenuPanel.activeSelf);
            isToogled = upgradeMenuPanel.activeSelf;
        }
    }
}
