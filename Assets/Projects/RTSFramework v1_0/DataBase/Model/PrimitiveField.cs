namespace RTSFramework_v1_0.DataBase.Model
{
    public class PrimitiveField<T> where T : struct
    {
        public PrimitiveField(T value) { this.value = value; }
        public T value;
    }

    ///Find a way to store the relation between field and gameobject/database
    /// and a way to express the meaning of the field itself
}
