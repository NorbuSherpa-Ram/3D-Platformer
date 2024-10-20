using DG.Tweening;
using System;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public static EventHandler OnAnyCoinCollect;

    public float rotationDuration = 1f;
    public Vector3 rotationAxis = new Vector3(0, 360, 0);


    private void Start()
    {
        transform.DORotate(rotationAxis, rotationDuration, RotateMode.FastBeyond360)
           .SetEase(Ease.Linear)
           .SetLoops(-1, LoopType.Restart);
    }


    private void OnTriggerEnter(Collider other)
    {
        //PASS COIN VALUE AS EVENT ARGS IF DIFFRENT COIN HAVE DIFFRENT VALUE 
        OnAnyCoinCollect?.Invoke(this, EventArgs.Empty);
        Destroy(this.gameObject);
    }
}
