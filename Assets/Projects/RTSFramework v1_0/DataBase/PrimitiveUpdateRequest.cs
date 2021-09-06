namespace RTSFramework_v1_0.DataBase
{
    public abstract class PrimitiveUpdateRequest<T> where T : struct
    {
        protected PrimitiveUpdateRequest(PrimitiveField<T> target)
        {
            this.target = target;
        }

        /// <summary>
        ///     The target that the change will be applied on
        /// </summary>
        protected PrimitiveField<T> target;

        /// <summary>
        ///     Get the new stage by the update
        /// </summary>
        /// <returns>The new target's stage</returns>
        public abstract T NewValue();

        public void Apply() { target.value = NewValue(); }
    }

    public class ExampleAddRequest : PrimitiveUpdateRequest<float>
    {
        public ExampleAddRequest(PrimitiveField<float> target, float addition) : base( target )
        {
            this.addition = addition;
        }

        float addition;
        public override float NewValue() { return target.value + addition; }
    }
}
