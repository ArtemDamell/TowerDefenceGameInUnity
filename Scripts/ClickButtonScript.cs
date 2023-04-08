using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickButtonScript : MonoBehaviour
{
    private int buttonValue;
    private GameObject tempClickedObject;
    private static GameObject tower;
    private static GameObject tempPos;
    private GameObject[] towerPrefabs;
    private GameManager gameManager;

    public Camera mainCamera;
    public GameObject towerChoisePanel;

    /// <summary>
    /// Checks if the player can place a tower and if they have enough gold to do so. If so, instantiates the tower.
    /// </summary>
    public void ClickedButtonValue(GameObject button)
    {
        buttonValue = Convert.ToInt32(button.name);

        if (CanPlace())
        {
            // Check and decriment tower cost
            if (towerPrefabs[buttonValue].GetComponent<TowerPrefabScript>().TowerValues.towerCost <= gameManager.Gold)
            {
                TowerInstantiate();
            }
        }
        else if (CanUpgrade())
        {
            // Check and decriment tower cost
            if (towerPrefabs[buttonValue].GetComponent<TowerPrefabScript>().TowerValues.towerCost <= gameManager.Gold)
            {
                // Destroy old prefab
                Destroy(tempClickedObject.transform.GetChild(0).gameObject);

                // Decrease gold(cost)
                //gameManager.Gold -= towerPrefabs[buttonValue].GetComponent<TowerPrefabScript>().TowerValues.towerCost;

                TowerInstantiate();
            }
        }
    }

    void Start()
    {
        gameManager = GameObject.Find("GameManagerBehaviour").GetComponent<GameManager>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            //PointerEventData pointerData = new PointerEventData(EventSystem.current);

            if (Physics.Raycast(ray, out hit, 1000f))
            {
                tempClickedObject = hit.transform.gameObject;
                //pointerData.position = Input.mousePosition;
                //EventSystem.current.

                if (tempClickedObject.tag == "OpenSpot")
                {
                    OpenMenu();
                }
            }
        }
    }

    /// <summary>
    /// Opens the menu for tower selection when a spot is clicked.
    /// </summary>
    private void OpenMenu()
    {
        towerChoisePanel.transform.position = tempClickedObject.transform.position;
        towerChoisePanel.SetActive(true);
        tempPos = tempClickedObject;
        towerPrefabs = GameObject.Find(tempClickedObject.name).GetComponent<OpenSpotScript>().towerPrefabs;
    }

    /// <summary>
    /// Checks if the current position is valid for placing an object.
    /// </summary>
    /// <returns>Returns true if the position is valid, false otherwise.</returns>
    private bool CanPlace()
    {
        int checkObject = tempPos.gameObject.transform.childCount;

        if (checkObject == 0)
            return true;
        return false;
    }

    /// <summary>
    /// Checks if the tower can be upgraded.
    /// </summary>
    /// <returns>Returns true if the tower can be upgraded, false otherwise.</returns>
    private bool CanUpgrade()
    {
        int checkObject = tempPos.gameObject.transform.childCount;
        GameObject tempPlacedTower = tempPos.transform.GetChild(0).gameObject;
        if (checkObject > 0 && tempPlacedTower.GetComponent<TowerPrefabScript>().TowerValues.UpgradeButtonIndex != buttonValue)
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// Closes the specified GameObject.
    /// </summary>
    /// <param name="clickObj">The GameObject to close.</param>
    public void CloseButton(GameObject clickObj)
    {
        clickObj.SetActive(false);
    }

    /// <summary>
    /// Decreases gold, instantiates a new tower, sets the tower's attack range and bullet force, and adds the pressed button value to the tower's script.
    /// </summary>
    private void TowerInstantiate()
    {
        // Decrease gold(cost)
        gameManager.Gold -= towerPrefabs[buttonValue].GetComponent<TowerPrefabScript>().TowerValues.towerCost;

        // Instantiate new tower
        tower = Instantiate(towerPrefabs[buttonValue],
            tempPos.transform.position, Quaternion.identity);
        tower.transform.SetParent(tempPos.transform);

        // Add current tower Range
        tower.transform.GetComponent<CircleCollider2D>().radius =
            tower.GetComponent<TowerPrefabScript>().TowerValues.towerAttackRange;

        // Set bullet Force
        tower.transform.gameObject.GetComponent<TowerPrefabScript>().TowerValues.bullet.GetComponent<BulletBehavior>().Damage = tower.transform.gameObject.GetComponent<TowerPrefabScript>().TowerValues.towerForce;

        // Add pressed button value to script
        tower.GetComponent<TowerPrefabScript>().TowerValues.UpgradeButtonIndex = buttonValue;
    }
}
