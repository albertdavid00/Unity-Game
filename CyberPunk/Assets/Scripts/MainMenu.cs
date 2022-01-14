using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string firstLevel;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(firstLevel);
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quitting Game");
    }

    public void LoadData()
    {
        Debug.Log("Loading data...");
        PlayerData data = SaveData.LoadPlayer();
        SceneManager.LoadScene(data.currentLevel);
        PlayerHealthController.instance.currentHealth = data.health;

        Vector3 position;
        position.x = data.positionX;
        position.y = data.positionY;
        position.z = data.positionZ;
        PlayerController.instance.transform.position = position;

        if (data.pickedUpSniper) PlayerController.instance.AddGun("sniper");

        foreach (Gun gun in PlayerController.instance.allGuns)
        {
            switch (gun.gunName)
            {
                case "sniper":
                    gun.currentAmmo = data.sniperAmmo;
                    break;
                case "pistol":
                    gun.currentAmmo = data.pistolAmmo;
                    break;
                case "repeater":
                    gun.currentAmmo = data.repeaterAmmo;
                    break;
                default:
                    gun.currentAmmo = data.rocketAmmo;
                    break;
            }
        }
    }

}
