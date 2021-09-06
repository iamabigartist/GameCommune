namespace RTSFramework_v1_0.DataBase
{
    public class PrimitiveField<T> where T : struct
    {
        public PrimitiveField(T value) { this.value = value; }
        public T value;
    }
}
