using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> tilePrefabs = new List<GameObject>();
    [SerializeField] private GameObject TreePrefab;
    [SerializeField] private int xTileCount = 8;
    [SerializeField] private int yTileCount = 8;
    [SerializeField] private float treeCooldown = 1f;
    public GameObject HousePrefab;
    [SerializeField] private TextMeshProUGUI scoreText;


    private List<GameObject> initialTile = new List<GameObject>();
    private List<GameObject> addedTile = new List<GameObject>();
    private List<GameObject> dirtTile = new List<GameObject>();
    private float lastTreePlantTime;
    private bool allDirtPlanted = false;
    public int score = 0;
    // Start is called before the first frame update
    void Start()
    {
        TileGeneration();
        GetDirtTile();
    }

    // Update is called once per frame
    void Update()
    {
        PlantTreeEverySecond();
    }

    private void TileGeneration()
    {
        Vector3 tileDimension = GetTileDimension(tilePrefabs[0]);

        for (int i = 0; i < xTileCount; i++)
        {
            for (int j = 0; j < yTileCount; j++) 
            {
                GameObject spawnedObject = new GameObject();
                spawnedObject.transform.position = new Vector3(i * tileDimension.x, 0, j * tileDimension.z);
                initialTile.Add(spawnedObject);
            }
        }

        ShuffleGameObjectList(initialTile);

        for (int i = 0; i < tilePrefabs.Count; i++)
        {
            GameObject instTile = Instantiate(tilePrefabs[i]);
            instTile.transform.position = initialTile[0].transform.position;
            TileProperties tileProp = instTile.AddComponent<TileProperties>();
            addedTile.Add(instTile);

            Destroy(initialTile[0]);
            initialTile.Remove(initialTile[0]);
        }

        while(initialTile.Count > 0)
        {
            int randNumber = UnityEngine.Random.Range(0, tilePrefabs.Count);
            GameObject instTile = Instantiate(tilePrefabs[randNumber]);
            instTile.transform.position = initialTile[0].transform.position;
            TileProperties tileProp = instTile.AddComponent<TileProperties>();
            addedTile.Add(instTile);

            Destroy(initialTile[0]);
            initialTile.Remove(initialTile[0]);
        }
    }

    private Vector3 GetTileDimension(GameObject tile)
    {
        Vector3 tileDimension;
        tileDimension = tile.GetComponent<MeshRenderer>().bounds.size;

        return tileDimension;
    }

    private List <GameObject> ShuffleGameObjectList(List <GameObject> list)
    {
        for(int i = list.Count - 1; i > 0; i--)
        {
            int j = UnityEngine.Random.Range(0, i + 1);
            GameObject temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }

        return list;
    }

    private void GetDirtTile()
    {
        for(int i = 0; i < addedTile.Count; i++)
        {
            if (addedTile[i].CompareTag("Dirt"))
            {
                dirtTile.Add(addedTile[i]);
            }
        }

        ShuffleGameObjectList(dirtTile);
    }

    private void PlantTreeEverySecond()
    {
        if (allDirtPlanted)
        {
            return;
        }

        if (dirtTile.Count == 0)
        {
            allDirtPlanted = true;
            Debug.Log("Tree is finished planting");
            return;
        }

        if (Time.time - lastTreePlantTime >= treeCooldown)
        {
            lastTreePlantTime = Time.time;
            
            for (int i = 0; i < dirtTile.Count; i++)
            {
                if (dirtTile[0].GetComponent<TileProperties>().haveHouse == false)
                {
                    GameObject instHouse = Instantiate(TreePrefab, dirtTile[0].transform);
                    instHouse.transform.localScale = new Vector3(0.5f, 2f, 0.5f);
                    dirtTile[0].GetComponent<TileProperties>().haveHouse = true;
                    dirtTile.Remove(dirtTile[0]);

                    return;
                }
                else
                {
                    dirtTile.Remove(dirtTile[0]);
                }
            }
            
        }
    }

    public void AddScore(int addedScore)
    {
        score += addedScore;
        scoreText.text = "Score: " + score.ToString();
    }
}
