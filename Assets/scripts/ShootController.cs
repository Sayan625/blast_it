using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using System;
using System.Collections.Generic;

public class ShootController : MonoBehaviour
{
    [SerializeField] internal Transform[] slots;

    [SerializeField] int columns;
    [SerializeField] int rows;
    [SerializeField] float spacing;
    [SerializeField] float distanceFromCamera;
    [SerializeField] List<int> options;
    [SerializeField] internal List<ShooterItem> shooters;
    [SerializeField] ShooterItem shooterPrefab;
    void Start()
    {
        SpawnGrid();
    }

    internal int GetSlotIndex()
    {
        int index = -1;
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].childCount == 0)
            {
                index = i;
                break;
            }
        }

        return index;

    }

    internal void ClearSlot(int i)
    {
        Destroy(slots[i].transform.GetChild(0).gameObject);
    }
    void SpawnGrid()
    {
        FillOptions();
        //Shuffle();
        rows = (int)(shooters.Count / columns);
        float gridWidth = (columns - 1) * spacing;
        float gridHeight = (rows - 1) * spacing;

        Vector3 gridCenter = new Vector3(-gridWidth / 2, 0, -gridHeight / 2);

        Vector3 spawnPosition = Camera.main.transform.position + Camera.main.transform.forward * distanceFromCamera;
        spawnPosition.y = 0;
        for (int i = 0; i < shooters.Count; i++)
        {
            int y = i / columns;     
            int x = i % columns;



            Vector3 position = spawnPosition + gridCenter + new Vector3(x * spacing, 0, y * spacing);
            shooters[i].transform.position = position;
            shooters[i].controller = this;
            shooters[i].transform.parent = this.transform;
            
        }

        shooters.Clear();
    }

    void FillOptions()
    {
        for (int i = 0; i < GridController.colorCount.Length; i++)
        {
            if (GridController.colorCount[i] == 0)
                continue;
            int number = GridController.colorCount[i];
            //int divisor = 20;

            //int multiples = number / divisor; 
            //int remainder = number % divisor;

            //for (int j = 0; j < multiples; j++)
            //{
            //    GameObject shooter = Instantiate(shooterPrefab.gameObject);
            //    var item = shooter.GetComponent<ShooterItem>();
            //    item.Initiate(i, 20);
            //    shooters.Add(item);

            //}
            //if (remainder != 0) {
            GameObject shooter1 = Instantiate(shooterPrefab.gameObject);
                var item1 = shooter1.GetComponent<ShooterItem>();
                item1.Initiate(i, number);

                shooters.Add(item1);
            //}


        }
    }


}
