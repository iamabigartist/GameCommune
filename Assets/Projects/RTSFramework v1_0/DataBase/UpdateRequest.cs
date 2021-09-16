namespace RTSFramework_v1_0.DataBase
{
    public abstract class UpdateRequest<T> : Request
        where T : struct
    {
        protected UpdateRequest(string pipeline_name, PrimitiveField<T> target) : base( pipeline_name )
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

        public override void Process() { target.value = NewValue(); }
    }

    public class ExampleAddRequest : UpdateRequest<float>
    {
        public ExampleAddRequest(string pipeline_name, PrimitiveField<float> target, float addition) : base( pipeline_name, target )
        {
            this.addition = addition;
        }

        float addition;
        public override float NewValue() { return target.value + addition; }
    }
}
