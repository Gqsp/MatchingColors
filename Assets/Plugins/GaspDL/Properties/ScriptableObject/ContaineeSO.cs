using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public interface IContainee
{
    public IContainer MyContainer { get; set; }
    public string Name { get; set; }
    public Sprite Icon { get; set; }

#if UNITY_EDITOR
    public void Initialise(IContainer myContainer);

#endif

}

public class ContaineeComposite : IContainee
{
    private IContainer _container;
    private string _name;
    private Sprite _icon;

    public IContainer MyContainer { get => _container; set => _container = value; }
    public string Name { get => _name; set => _name = value; }
    public Sprite Icon { get => _icon; set => _icon = value; }

#if UNITY_EDITOR
    public void Initialise(IContainer myContainer)
    {
        MyContainer = myContainer;
    }
#endif
}


