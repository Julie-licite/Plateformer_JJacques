using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy : MonoBehaviour
{
    //detruit mon canva quand j'en ai plus besoin
    public void DestroyCanva()

    {
        Destroy(gameObject);
    }
}
