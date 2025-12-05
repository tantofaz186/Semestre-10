using UnityEngine;
public interface ISaveable<T> : ISaveable where T : SaveData 
{
    public new T Sincronize();
    public new void Load(T data);
}
public interface ISaveable
{
    public SaveData Sincronize();
    public void Load(SaveData data);
}