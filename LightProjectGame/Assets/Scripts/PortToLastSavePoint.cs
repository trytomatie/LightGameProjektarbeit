using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortToLastSavePoint : MonoBehaviour
{
    public void PortToLastSavepoint()
    {
        GameManager.ReloadThisLevel();
    }
}
