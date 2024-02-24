using System.Collections.Generic;
using System;

[Serializable]
public class ModifiedValue<T> where T : struct, IComparable, IConvertible, IFormattable
{
    public T Value;
    public List<Modifier<T>> Modifiers;
    public Action<T> OnValueChanged;

    public ModifiedValue(T value)
    {
        Value = value;
        Modifiers = new List<Modifier<T>>();
    }

    public void SetValue(T value)
    {
        Value = value;
        OnValueChanged?.Invoke(GetValue());
    }

    public T GetValue() 
    {
        T value = Value;

        foreach (Modifier<T> mod in Modifiers)
        {
            if (typeof(T) == typeof(float))
            {
                float x = (float)Convert.ChangeType(value, typeof(float));
                float y = (float)Convert.ChangeType(mod.Value, typeof(float));
                value = (T)Convert.ChangeType(MathF.Round(x * y, 2), typeof(T));
            }
            else if (typeof(T) == typeof(double))
            {
                double x = (double)Convert.ChangeType(value, typeof(double));
                double y = (double)Convert.ChangeType(mod.Value, typeof(double));
                value = (T)Convert.ChangeType(Math.Round(x * y, 2), typeof(T));
            }
            else if (typeof(T) == typeof(int))
            {
                int x = (int)Convert.ChangeType(value, typeof(int));
                int y = (int)Convert.ChangeType(mod.Value, typeof(int));
                value = (T)Convert.ChangeType(x * y, typeof(T));
            } 
        }

        return value;
    }

    private T Multiply(T a, T b)
    {
        //dynamic x = a;
        //dynamic y = b;
        return a;
    }

    public void ResetModifiers()
    {
        Modifiers.Clear();
        OnValueChanged?.Invoke(GetValue());
    }

    public void AddModifier(Modifier<T> mod)
    {
        Modifiers.Add(mod);
        OnValueChanged?.Invoke(GetValue());
    }

    public void AddModifier(string name, T value)
    {
        Modifier<T> newMod = new Modifier<T> { Name = name, Value = value };
        Modifiers.Add(newMod);
        OnValueChanged?.Invoke(GetValue());
    }

    public void ModifyModifier(string name, T newValue)
    {
        var mod = Modifiers.Find(mod => mod.Name == name);
        mod.Value = newValue;
        OnValueChanged?.Invoke(GetValue());
    }

    public void RemoveModifier(Modifier<T> mod)
    {
        Modifiers.RemoveAll(m => m.Name == mod.Name);
        OnValueChanged?.Invoke(GetValue());
    }

    public void RemoveModifier(string name)
    {
        Modifiers.RemoveAll(m => m.Name == name);
        OnValueChanged?.Invoke(GetValue());
    }
}

[Serializable]
public struct Modifier<T> where T : struct, IComparable, IConvertible, IFormattable
{
    public string Name;
    public T Value;
}
