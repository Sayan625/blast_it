using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;
public class GridController : MonoBehaviour
{
    [SerializeField] GridItem cubePrefab;
    [SerializeField] int rows = 5;
    [SerializeField] int columns = 5;
    [SerializeField] float spacing = 1f;
    [SerializeField] float distanceFromCamera = 10f;
    [SerializeField] Transform parentTransform;
    [SerializeField] internal static int[] colorCount = new int[3] { 0, 0, 0 };
    [SerializeField] float maxItem = 100;
    [SerializeField] float currItem = 100;
    [SerializeField] Image progress;
    [SerializeField] GameObject clickblocker;
    [SerializeField] LevelController levelController;

    internal static GridController instance;

    public List<customStack> allstacks = new List<customStack>(10);


    void OnEnable()
    {
        clickblocker.SetActive(true);
        instance = this;
        for (int i = 0; i < colorCount.Length; i++)
        {
            colorCount[i] = 0;
        }

        for (int i = 0; i < 10; i++)
        {
            allstacks.Add(new customStack());
        }
        SpawnGrid();
        StartCoroutine(InitiatingAnim());
    }

    void SpawnGrid()
    {

        float gridWidth = (columns - 1) * spacing;
        float gridHeight = (rows - 1) * spacing;

        Vector3 gridCenter = new Vector3(-gridWidth / 2, 0, -gridHeight / 2);

        Vector3 spawnPosition = Camera.main.transform.position + Camera.main.transform.forward * distanceFromCamera;
        spawnPosition.y = 0;

        for (int x = 0; x < columns; x++)
        {
            int id = UnityEngine.Random.Range(0, 2);
            if (levelController.level > 2)
                id = UnityEngine.Random.Range(0, 3);

            for (int y = 0; y < rows; y++)
            {
                if (y == rows / 2)
                {
                    id = UnityEngine.Random.Range(0, 2);
                    if (levelController.level > 2)
                        id = UnityEngine.Random.Range(0, 3);
                }

                Vector3 position = spawnPosition + gridCenter + new Vector3(x * spacing, 0, y * spacing);
                GameObject cube = Instantiate(cubePrefab.gameObject, position, Quaternion.identity);
                var item = cube.GetComponent<GridItem>();
                item.controller = this;
                item.col = x;
                item.id = id;
                item.SetColor(levelController.colors[id]);

                allstacks[x].colItems.Add(item);
                cube.name = $"cols:{x}, rows:{y}";

                if (parentTransform != null)
                    cube.transform.SetParent(parentTransform);

                if (id == 0) { colorCount[0]++; }
                else if (id == 1) { colorCount[1]++; }
                else { colorCount[2]++; }
            }
        }

    }

    IEnumerator InitiatingAnim()
    {

        for (int x = allstacks[0].colItems.Count-1; x >=0; x--)
        {
            for (int y = 0; y < allstacks.Count; y++)
            {

                allstacks[y].colItems[x].transform.DOScaleY(1f, 0.1f).SetEase(Ease.Linear);


            }
            yield return new WaitForSeconds(0.1f);
            for (int y = 0; y < allstacks.Count; y++)
            {

                allstacks[y].colItems[x].transform.DOScaleY(0.5f, 0.1f).SetEase(Ease.Linear);


            }
        }
        yield return new WaitForSeconds(1f);
        clickblocker.SetActive(false);

    }
    internal void CheckStack(int i)
    {
        if (allstacks[i].colItems.Count == 0)
        {
            levelController.won = true;
            levelController.winpanel.SetActive(true);
            return;
        }

        if (allstacks[i].colItems[0].isEmpty)
        {
            var obj = allstacks[i].colItems[0];
            allstacks[i].colItems.RemoveAt(0);
            Destroy(obj.gameObject);
            currItem--;
            progress.fillAmount = ((maxItem - currItem) / maxItem);
            foreach (var item in allstacks[i].colItems)
            {
                item.MoveDown();
            }
        }
    }




}
[Serializable]
public class customStack
{
    public List<GridItem> colItems = new List<GridItem>(10);
}
