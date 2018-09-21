using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallElectEffectScript : MonoBehaviour
{
    public float lifeTime;

    void Start()
    {
        StartCoroutine(DeathCount());
    }

    private IEnumerator DeathCount()
    {
        yield return new WaitForSeconds(lifeTime);
        Destroy(gameObject);
    }
}
