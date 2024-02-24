using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public interface IContainer
{
    public List<IContainee> MyContainees { get; set; }
    public string ContaineeClass { get; set; }

#if UNITY_EDITOR
    public IContainee CreateContainees();
#endif

}


public class ContainerComposite : IContainer
{
    private List<IContainee> _containees;
    private string _containeeClass;

    public ScriptableObject Owner;
    public List<IContainee> MyContainees { get => _containees; set => _containees = value; }
    public string ContaineeClass { get => _containeeClass; set => _containeeClass = value; }

    public ContainerComposite(ScriptableObject owner, string containeeClass)
    {
        Owner = owner;
        ContaineeClass = containeeClass;
        MyContainees = new List<IContainee>();
    }

#if UNITY_EDITOR
    public IContainee CreateContainees()
    {
        IContainee containee = ScriptableObject.CreateInstance(ContaineeClass) as IContainee;
        containee.Name = ContaineeClass + "_" + (MyContainees.Count + 1);
        containee.Initialise(this);
        MyContainees.Add(containee);

        AssetDatabase.AddObjectToAsset(containee as ScriptableObject, Owner);
        AssetDatabase.SaveAssets();

        EditorUtility.SetDirty(Owner);
        EditorUtility.SetDirty(containee as ScriptableObject);

        return containee;
    }
#endif
}


