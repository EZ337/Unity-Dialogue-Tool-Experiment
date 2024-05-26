using UnityEngine;

public class MethodExposer : MonoBehaviour
{
    public void MethodA()
    {
        Debug.Log("MethodA called");
    }

    public void MethodB(string message)
    {
        Debug.Log("MethodB called with message: " + message);
    }

    private void MethodC()
    {
        Debug.Log("I am a private method");
    }
}
