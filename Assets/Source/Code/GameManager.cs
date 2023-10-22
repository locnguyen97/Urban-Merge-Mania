using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public List<GameLevel> levels;

    private int startIndex = 0;

    private int currentIndex;
    [SerializeField] List<GameObject> particleVFXs;

    [SerializeField] private List<MapPoint> listPoint = new List<MapPoint>();

    [SerializeField] private List<Sprite> listId1;
    [SerializeField] private List<Sprite> listId2;
    [SerializeField] private List<Sprite> listId3;
    [SerializeField] private List<Sprite> listId4;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        currentIndex = startIndex;

        StartGame();
    }

    void StartGame()
    {
        levels[currentIndex].gameObject.SetActive(true);
        listPoint = FindObjectsOfType<MapPoint>().ToList();
        List<MapPoint> listCache = new List<MapPoint>();
        foreach (var p in listPoint)
        {
            listCache.Add(p);
        }

        while (listCache.Count > 0)
        {
            int id = Random.Range(0, 4);

            var p1 = listCache[Random.Range(0, listCache.Count)];
            p1.SetData(id, GetListSpr(id + 1));
            listCache.Remove(p1);

            var p2 = listCache[Random.Range(0, listCache.Count)];
            p2.SetData(id, GetListSpr(id + 1));
            listCache.Remove(p2);
        }
    }

    List<Sprite> GetListSpr(int id)
    {
        switch (id)
        {
            case 1: return listId1;
            case 2: return listId2;
            case 3: return listId3;
            case 4: return listId4;
            default: return listId1;
        }
    }

    private bool isChecking = false;

    public void CheckLevelUp()
    {
        isChecking = true;

        Invoke(nameof(CheckGame), 0.25f);
    }

    void CheckGame()
    {
        isChecking = false;
        if (checkWinGame())
        {
            StartCoroutine(LevelUp());
        }
    }

    bool checkWinGame()
    {
        int count = 0;
        foreach (var point in listPoint)
        {
            if (point.level == point.maxLevel)
            {
                count++;
            }
        }

        return count == listPoint.Count;
    }

    IEnumerator LevelUp()
    {
        currentIndex += 1;
        GameObject explosion =
            Instantiate(particleVFXs[Random.Range(0, particleVFXs.Count)], transform.position, transform.rotation);
        Destroy(explosion, .75f);
        yield return new WaitForSeconds(1);
        levels[currentIndex-1].gameObject.SetActive(false);
        if (currentIndex == 3)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            currentIndex = 0;
            StartGame();
        }
        else
        {
            StartGame();
        }
    }


    // drag obj
    public MapPoint selectedObject;

    void Update()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (Input.GetMouseButtonDown(0))
        {
            Collider2D targetObject = Physics2D.OverlapPoint(mousePosition);

            if (targetObject)
            {
                MapPoint point = targetObject.GetComponent<MapPoint>();
                if (point != null)
                {
                    if (!point.isBlock && point.level < point.maxLevel)
                    {
                        if (selectedObject)
                        {
                            if (point != selectedObject)
                            {
                                if (selectedObject.id == point.id && selectedObject.level == point.level)
                                {
                                    GameObject explosion =
                                        Instantiate(particleVFXs[Random.Range(0, particleVFXs.Count)],
                                            point.transform.position, transform.rotation);
                                    Destroy(explosion, .75f);
                                    selectedObject.LevelUp(false);
                                    point.LevelUp(true);
                                    selectedObject = null;
                                }
                            }
                            else
                            {
                                selectedObject.CancelPickUp();
                                selectedObject = null;
                            }
                        }
                        else
                        {
                            selectedObject = targetObject.GetComponent<MapPoint>();
                            selectedObject.PickUp();
                        }
                    }
                }
            }
        }
    }
}