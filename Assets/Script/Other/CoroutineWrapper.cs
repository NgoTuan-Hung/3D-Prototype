using System.Collections;
using UnityEngine;

// we will use this class for referencing
// coroutine itself
public class CoroutineWrapper
{
    public Coroutine coroutine;

    public static bool CheckCoroutineNotNull(Coroutine coroutine)
    {
        return coroutine != null;
    }
}