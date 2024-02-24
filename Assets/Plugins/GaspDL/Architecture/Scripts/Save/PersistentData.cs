
namespace GaspDL.SaveSystem
{
    public class PersistentData<T>
    {
        public PersistentID id;
        public string tag;
        public T data;

        public PersistentData(T _data, PersistentID _id, string _tag)
        {
            data = _data;
            id = _id;
            tag = _tag;
        }
    }
}

