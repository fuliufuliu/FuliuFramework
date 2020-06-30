using System.Collections;
using DG.Tweening;
using UnityEngine;

public class TestDotween : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(ChangeSpeed());
    }

    private IEnumerator ChangeSpeed()
    {
        yield return new WaitForSeconds(Random.Range(1.3f, 1.5f)); 
        var tween = transform.DOLocalMove(new Vector3(80, 5, 253f), 4).SetLoops(1, LoopType.Yoyo).SetEase(Ease.Linear);
        yield return new WaitForSeconds(Random.Range(1.3f, 1.5f));
        tween.ChangeValues(transform.localPosition, new Vector3(80, 5, 253f), 0.5f);
    }
}
