using RTSFramework_v1_0.DataBase.Model;
using RTSFramework_v1_0.Processor.Pipeline;
namespace RTSFramework_v1_0.DataBase.Requests
{
    public abstract class UpdateRequest<T> : Request
        where T : struct
    {
        protected UpdateRequest(
            GamePipelineTable.Stage stage,
            IRequestFrom from,
            bool temporary,
            PrimitiveField<T> target_field) : base( stage, from, temporary )
        {
            this.target_field = target_field;
        }

        /// <summary>
        ///     The target_field that the change will be applied on
        /// </summary>
        protected PrimitiveField<T> target_field;
        public override IRequestTo to => target_field;

        /// <summary>
        ///     Get the new stage by the update
        /// </summary>
        /// <returns>The new target_field's stage</returns>
        public abstract T NewValue();

        public override void Process() { target_field.value = NewValue(); }
    }

    public class ExampleAddRequest : UpdateRequest<float>
    {
        public ExampleAddRequest(
            GamePipelineTable.Stage stage,
            IRequestFrom from,
            PrimitiveField<float> target,
            bool temporary,
            float addition) : base( stage, from, temporary, target )
        {
            this.addition = addition;
        }

        float addition;
        public override float NewValue() { return target_field.value + addition; }
    }
}
