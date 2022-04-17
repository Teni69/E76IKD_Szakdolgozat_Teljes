using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    public int unitLayer = 8;

    public SpriteManager HealthbarSpriteManager;
    private combatState ClientState;
    
    void Start()
    {
        UnitController.onUnitSpawn += AddHealthbarToUnitSpawned;

        UnitController.onUnitDead += RemoveHealthbarToUnitDead;

        AddHealthBarToUnits();
    }

    //On spawn, add the healthbar
    public void AddHealthbarToUnitSpawned(GameObject unit)
    {
        GameObject healthbarClient = unit.transform.Find("HealthBar").gameObject;

        Sprite healthbarSprite = HealthbarSpriteManager.AddSprite(healthbarClient, 1f, 0.1667f, new Vector2(0f, 0.9f), new Vector2(0.1f, 0.1f), Vector3.zero, false);

        healthbarClient.GetComponent<HealthbarClient>().myHealthbarSprite = healthbarSprite;
    }

    //On death, remove the healthbar
    public void RemoveHealthbarToUnitDead(GameObject unit)
    {
        GameObject healthbarClient = unit.transform.Find("HealthBar").gameObject;
        Sprite healthbarSprite = healthbarClient.GetComponent<HealthbarClient>().myHealthbarSprite as Sprite;
        HealthbarSpriteManager.RemoveSprite(healthbarSprite);
    }

    public void UpdateHealthbarToUnit(GameObject unit, float HealthDiff)
    {
        float maxHealth = unit.GetComponent<UnitHealth>().MaxHealth;
        float currentHealth = unit.GetComponent<UnitHealth>().Health;
        float newHealth = currentHealth + HealthDiff;

        unit.GetComponent<UnitHealth>().Health = newHealth;

        if (newHealth > 0f)
        {
            GameObject healthbarClient = unit.transform.Find("HealthBar").gameObject;

            float NoOfSpritesAcross = 10f;
            float NoOfSpritesDown = 10f;

            float OnePercent = maxHealth / 100f;
            float HealthPercentage = newHealth / OnePercent;

            //Spriteok váltogatásához
            float UV_X = Mathf.Ceil(HealthPercentage % NoOfSpritesAcross);
            float UV_Y = Mathf.Floor(HealthPercentage / NoOfSpritesDown);

            UV_X /= 10f;
            UV_Y /= 10F;

            UV_X = 1f - UV_X;

            //Aktuális sprite eltávolítása
            Sprite currentHealthbarSprite = healthbarClient.GetComponent<HealthbarClient>().myHealthbarSprite as Sprite;
            HealthbarSpriteManager.RemoveSprite(currentHealthbarSprite);

            //Új sprite
            Sprite healthbarSprite = HealthbarSpriteManager.AddSprite(healthbarClient, 1f, 0.1667f, new Vector2(UV_X, UV_Y), new Vector2(0.1f, 0.1f), Vector3.zero, false);
            healthbarClient.GetComponent<HealthbarClient>().myHealthbarSprite = healthbarSprite;

            ClientState = unit.GetComponent<combatState>();
            //Elrejtés, ha nincs kijelölve
            if (!selectManager.units.Contains(unit) || ClientState.currentState == combatState.STATE.OutOfCombat)
            {
                HealthbarSpriteManager.HideSprite(healthbarClient.GetComponent<HealthbarClient>().myHealthbarSprite);
            }
        }
    }

    public void AddHealthBarToUnits()
    {
        GameObject[] GameObjectArray = FindObjectsOfType(typeof(GameObject)) as GameObject[];
        List<GameObject> unitList = new List<GameObject>();

        for (int i = 0; i < GameObjectArray.Length; i++)
        {
            if (GameObjectArray[i].layer == unitLayer)
                unitList.Add(GameObjectArray[i]);
        }

        if(unitList.Count == 0)
            return;

        GameObject[] Units = unitList.ToArray();

        for (int i = 0; i < Units.Length; i++)
        {
            AddHealthbarToUnitSpawned(Units[i].gameObject);
        }
            return;
    }
}
