namespace RTSFramework_v0_1.src.Base.Change
{
    public class PrimitiveChange
    {
        public enum ChangeType
        {
            Add, Multiply, Flip, TurnOff, TurnOn
        }

        public ChangeType change_type;
        public float_data data;
        public PrimitiveChange(ChangeType change_type, float_data data)
        {
            this.change_type = change_type;
            this.data = data;
        }
    }
}
