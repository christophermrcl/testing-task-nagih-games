using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TileProperties : MonoBehaviour, IPointerClickHandler
{
    public bool haveTree = false;
    public bool haveHouse = false;
    private GameManager gameManager;

    private void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if ((gameObject.CompareTag("Dirt") || gameObject.CompareTag("Desert")) && !haveTree && !haveHouse)
        {
            GameObject house = Instantiate(gameManager.HousePrefab, gameObject.transform);
            house.transform.localScale = new Vector3(-.09f, .3f, .07f);
            haveHouse = true;

            if (gameObject.CompareTag("Dirt"))
            {
                gameManager.AddScore(10);
            }else if (gameObject.CompareTag("Desert"))
            {
                gameManager.AddScore(2);
            }
        }
    }
}
