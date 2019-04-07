Original idea from @LotteMakesStuff:
https://gist.github.com/LotteMakesStuff/d6a9a4944fc667e557083108606b7d22

heres a tiny little example of how to use it
```C#
public class AutohookTest : MonoBehaviour
{
    [Autohook] // <-- yeah its that easy!
    public Rigidbody rigidbody;
}
```

```C#
public class AutohookTest : MonoBehaviour
{
    [Autohook (Context.Self, Visility.Disabled] // <-- yeah its that easy!
    public Rigidbody rigidbody;
}
```

You can watch a demo of this in action here https://youtu.be/faVt09NGzws

[![ko-fi](https://www.ko-fi.com/img/githubbutton_sm.svg)](https://ko-fi.com/A08215TT)
