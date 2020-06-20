using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionEffect : MonoBehaviour
{
    public GameObject effectPrefab;
    public float effectLifeTime = 3;
    Queue<GameObject> pool = new Queue<GameObject>();
    
    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.isTrigger)
        {
            return;
        }

        if (effectPrefab)
        {
            for (int i = 0; i < other.contacts.Length; i++)
            {
                var effect = GetEffect();
                if (effect)
                {
                    effect.transform.parent = transform;
                    effect.transform.position = other.contacts[i].point;
                    effect.transform.LookAt(other.contacts[i].point + other.contacts[i].normal);
                    effect.SetActive(true);
                    StartCoroutine(HideEffect(effect));
                }
            }
        }
    }

    private IEnumerator HideEffect(GameObject effect)
    {
        yield return new WaitForSeconds(effectLifeTime);
        effect.SetActive(false);
        pool.Enqueue(effect);
    }

    private GameObject GetEffect()
    {
        if (pool.Count > 0)
        {
            return pool.Dequeue();
        }

        return Instantiate(effectPrefab);
    }

    private void OnDestroy()
    {
        pool.Clear();
    }
}
