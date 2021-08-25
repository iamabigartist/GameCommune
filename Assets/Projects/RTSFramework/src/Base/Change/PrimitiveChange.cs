namespace RTSFramework
{
    public class PrimitiveChange
    {
        public enum ChangeType
        {
            Add, Multiply, Flip, TurnOff, TurnOn
        }

        public ChangeType change_type;
        public float_data data;
    }
}
