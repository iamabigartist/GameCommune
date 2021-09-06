using System;
namespace RTSFramework_v0_2.src.Data1
{
    /// <summary>
    ///     The basic data that can be changed by a <see cref="PrimitiveChange" />
    /// </summary>
    public class PrimitiveData<T> where T : struct
    {
        public PrimitiveData(T value) { this.value = value; }
        T value;
        public void ApplyChange(PrimitiveChange<T> change)
        {
            dynamic change_value = change.data;
            value = change.type switch
            {
                PrimitiveChange<T>.Type.Set      => change_value,
                PrimitiveChange<T>.Type.Add      => value + change_value,
                PrimitiveChange<T>.Type.Multiply => value * change_value,
                PrimitiveChange<T>.Type.Flip     => !(value as dynamic),
                _                                => throw new ArgumentOutOfRangeException()
            };

        }
    }
    public class PrimitiveChange<T> where T : struct
    {
        public enum Type
        {
            Set, Add, Multiply, Flip
        }

        public Type type;
        public PrimitiveData<T> data;
        public PrimitiveChange(Type type, T value)
        {
            this.type = type;
            data = new PrimitiveData<T>( value );
        }
    }

}
