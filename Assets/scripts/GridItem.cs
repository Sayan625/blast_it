using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class GridItem : MonoBehaviour
{
    public bool isEmpty=false;
    [SerializeField] internal Rigidbody rb;
    [SerializeField] internal int col;
    [SerializeField] internal GridController controller;
    [SerializeField] internal int id;
    [SerializeField] MeshRenderer mesh;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    internal void MoveDown() {

        transform.DOLocalMoveZ(transform.localPosition.z-0.54f, 0.1f).SetEase(Ease.InOutBounce);
    }

    internal void SetColor(Color color) {

        mesh.material.color = color;

    }
    internal void Remove()
    {
        isEmpty = true;
        transform.DOScale(0f, 0.1f).SetEase(Ease.OutBounce).OnComplete(()=> { 
        
        controller.CheckStack(col); 
        
        });
    }


    private void OnDestroy()
    {
        DOTween.Kill(transform);
    }
}
