// NOTE DONT put in an editor folder!
using UnityEngine;


public enum Context
{
    Self = 0,
    Parent = 1,
    Child = 2
}

public enum Visibility
{
    Visible = 0,
    Disabled = 1,
    Hidden = 2
}

public class AutohookAttribute : PropertyAttribute
{
    private readonly Context context = Context.Self;
    public Context Context { get { return context; } }
    private readonly Visibility visibility = Visibility.Hidden;
    public Visibility Visibility { get { return visibility; } }

    public AutohookAttribute()
    {
        context = Context.Self;
        visibility = Visibility.Hidden;
    }

    public AutohookAttribute(Context context)
    {
        this.context = context;
    }
    
    
    public AutohookAttribute(Visibility visibility)
    {
        this.visibility = visibility;
    }

    
    public AutohookAttribute(Context context, Visibility visibility)
    {
        this.context = context;
        this.visibility = visibility;
    }
}
