using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPack : MonoBehaviour ,Iitem
{
    public int ammo = 30;
    public void Use(GameObject target)
    {
       Debug.Log("ź���� ������"+ammo);
    }
}
