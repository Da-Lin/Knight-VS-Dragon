using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingText : MonoBehaviour
{

    Vector3 offset = new Vector3(0, 2f, 0);
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 1.5f);

        transform.position += offset;
    }

}
