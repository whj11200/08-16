using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HillsPack : MonoBehaviour, Iitem
{
    public float health = 50f;
    public void Use(GameObject target)
    {
        Debug.Log("체력회복" + health);
    }
}
