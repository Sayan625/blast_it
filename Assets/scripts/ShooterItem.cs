using DG.Tweening;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.EventSystems;
using TMPro;

public class ShooterItem : MonoBehaviour
{
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] float shootDuration = 0.5f;
    [SerializeField] float rotationDuration = 0.2f;
    [SerializeField] int projectileCount;
    [SerializeField] internal int maxProjectileCount;
    [SerializeField] bool start=false;
    [SerializeField] TMP_Text text;
    [SerializeField]internal MeshRenderer mesh;
    [SerializeField] internal int id;
    [SerializeField] int slotIndex=-1;
    [SerializeField] bool completed;
    internal ShootController controller;
    Quaternion initialRotation;
    bool engaged=false;
    private void Start()
    {
        StartCoroutine(ShootRoutine());
        StartCoroutine(OnFinished());
    }


    IEnumerator ShootRoutine()
    {
        yield return new WaitUntil(() => start);
        print("here");
        controller.shooters.Add(this);
        while (projectileCount <= maxProjectileCount-1)
        {

            bool hasAtLeastOneMatch = false;

            hasAtLeastOneMatch = CheckMatch();
            print(hasAtLeastOneMatch);
            int c = 0;
            if (hasAtLeastOneMatch)
            {
                foreach (var item in GridController.instance.allstacks)
                {
                    if (item.colItems.Count > 0 && item.colItems[0].id == id)
                    {
                        Shoot(item.colItems[0]);
                        c++;
                        yield return new WaitForSeconds((rotationDuration));

                    }

                }
            transform.DORotateQuaternion(initialRotation, rotationDuration).SetEase(Ease.Linear);
                //yield return new WaitForSeconds(c * (rotationDuration + shootDuration + 0.11f) / 2);
            }
            yield return new WaitForSeconds(0.23f);
            print("loop end");

        }
        transform.DORotateQuaternion(initialRotation, rotationDuration).SetEase(Ease.Linear);
        yield return null;

        completed = true;
        LevelController.instance.won = true;
        foreach (var item in GridController.instance.allstacks) {

            if (item.colItems.Count != 0)
            {
                LevelController.instance.won = false;

            }
        }
        if (LevelController.instance.won) { 
                LevelController.instance.OnWin();

        }



    }

    IEnumerator OnFinished()
    {
        yield return new WaitUntil(() => completed);
        transform.DOScale(0, 0.25f).SetEase(Ease.OutBounce);
        yield return new WaitForSeconds(0.25f);
        controller.ClearSlot(slotIndex);
        controller.shooters.Remove(this);

    }
    private bool CheckMatch()
    {
        bool hasAtLeastOneMatch = false;
        foreach (var item in GridController.instance.allstacks)
        {
            if (item.colItems.Count > 0 && item.colItems[0].id == id)
            {
                hasAtLeastOneMatch = true;
                break;
            }
        }

        return hasAtLeastOneMatch;
    }

    internal void Initiate(int type, int maxprojectilecount) {

        id = type;
        maxProjectileCount = maxprojectilecount;
        text.text = maxProjectileCount.ToString();
        mesh.material.color = LevelController.instance.colors[type];
    }
    void Shoot(GridItem target)
    {
        if (projectileCount >= maxProjectileCount)
            return;
        projectileCount++;
        text.text = (maxProjectileCount - projectileCount).ToString();

        Vector3 directionToTarget = (target.transform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(directionToTarget);
        transform.DORotateQuaternion(lookRotation, rotationDuration)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
              {

                  GameObject projectile = Instantiate(projectilePrefab, transform.position, transform.rotation);

                  projectile.transform.DOMove(target.transform.position, shootDuration)
                  .SetEase(Ease.Linear)
                  .OnComplete(() =>
                     {
                         Destroy(projectile);
                         target.Remove();
                     });
              });


    }

    public void OnMouseDown()
    {
        if(engaged)
        return;
        engaged=true;
        int index = controller.GetSlotIndex();
        print(index);
        if (index < 0)
            return;
        slotIndex = index;
        transform.parent = controller.slots[slotIndex];
        Vector3 directionToTarget = (new Vector3(0, transform.localPosition.y, 0) - transform.localPosition).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(directionToTarget);
        transform.DORotateQuaternion(lookRotation, rotationDuration);
        transform.DOLocalMove(new Vector3(0, transform.localPosition.y, 0), 0.35f).OnComplete(()=> {
            transform.rotation = Quaternion.identity;
            start = true;


        });
    }




}
