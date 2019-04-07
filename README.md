heres a tiny little example of how to use it
```C#
public class AutohookTest : MonoBehaviour
{
    [Autohook] // <-- yeah its that easy!
    public Rigidbody rigidbody;
    
    // Update is called once per frame
    void Update()
    {
        // do something
        rigidbody.AddForce(Vector3.up);
    }
}
```
You can watch a demo of this in action here https://youtu.be/faVt09NGzws

[![ko-fi](https://www.ko-fi.com/img/githubbutton_sm.svg)](https://ko-fi.com/A08215TT)