using RTSFramework_v1_0.DataBase.Model;
using RTSFramework_v1_0.Processor.Pipeline;
namespace RTSFramework_v1_0.DataBase.Request
{
    public abstract class UpdateRequest<T> : Request
        where T : struct
    {
        protected UpdateRequest(
            GamePipelineTable.Stage stage,
            IRequestFrom from,
            PrimitiveField<T> target) : base( stage, from )
        {
            this.target = target;
        }

        /// <summary>
        ///     The target that the change will be applied on
        /// </summary>
        protected PrimitiveField<T> target;
        public override IRequestTo to => target;

        /// <summary>
        ///     Get the new stage by the update
        /// </summary>
        /// <returns>The new target's stage</returns>
        public abstract T NewValue();

        public override void Process() { target.value = NewValue(); }
    }

    public class ExampleAddRequest : UpdateRequest<float>
    {
        public ExampleAddRequest(
            GamePipelineTable.Stage stage,
            IRequestFrom from,
            PrimitiveField<float> target,
            float addition) : base( stage, from, target )
        {
            this.addition = addition;
        }

        float addition;
        public override float NewValue() { return target.value + addition; }
    }
}
