using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkbox : MonoBehaviour
{
    public string prefKey;

    public void Enable(bool value)
    {
        gameObject.SetActive(value);
    }
}
