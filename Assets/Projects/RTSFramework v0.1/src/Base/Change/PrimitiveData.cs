using System;
namespace RTSFramework_v01
{
    /// <summary>
    ///     The basic data component
    /// </summary>
    public abstract class PrimitiveData
    {
        protected static ArgumentException WrongChangeTypeException(string change_name)
        {
            return new ArgumentException( "Wrong change type", change_name );
        }
        public abstract void ApplyChange(PrimitiveChange change);
    }


    public class float_data : PrimitiveData
    {
        public float_data(float value) { this.value = value; }
        public float value;

        public override void ApplyChange(PrimitiveChange change)
        {
            value = change.change_type switch
            {
                PrimitiveChange.ChangeType.Add      => value + change.data.value,
                PrimitiveChange.ChangeType.Multiply => value * change.data.value,
                PrimitiveChange.ChangeType.Flip     => throw WrongChangeTypeException( nameof(change) ),
                PrimitiveChange.ChangeType.TurnOff  => throw WrongChangeTypeException( nameof(change) ),
                PrimitiveChange.ChangeType.TurnOn   => throw WrongChangeTypeException( nameof(change) ),
                _                                   => throw new ArgumentOutOfRangeException()
            };
        }
    }
    public class int_data : PrimitiveData
    {
        public int_data(int value) { this.value = value; }
        public int value;
        public override void ApplyChange(PrimitiveChange change)
        {
            value = change.change_type switch
            {
                PrimitiveChange.ChangeType.Add      => value + (int)change.data.value,
                PrimitiveChange.ChangeType.Multiply => value * (int)change.data.value,
                PrimitiveChange.ChangeType.Flip     => throw WrongChangeTypeException( nameof(change) ),
                PrimitiveChange.ChangeType.TurnOff  => throw WrongChangeTypeException( nameof(change) ),
                PrimitiveChange.ChangeType.TurnOn   => throw WrongChangeTypeException( nameof(change) ),
                _                                   => throw new ArgumentOutOfRangeException()
            };
        }
    }
    public class boolean_data : PrimitiveData
    {
        public boolean_data(bool value) { this.value = value; }
        public bool value;
        public override void ApplyChange(PrimitiveChange change)
        {
            value = change.change_type switch
            {
                PrimitiveChange.ChangeType.Flip     => !value,
                PrimitiveChange.ChangeType.TurnOff  => false,
                PrimitiveChange.ChangeType.TurnOn   => true,
                PrimitiveChange.ChangeType.Add      => throw WrongChangeTypeException( nameof(change) ),
                PrimitiveChange.ChangeType.Multiply => throw WrongChangeTypeException( nameof(change) ),
                _                                   => throw new ArgumentOutOfRangeException()
            };
        }
    }
}
