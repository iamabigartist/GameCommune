using RTSFramework_v0_1.src.Base.Change;
using RTSFramework_v0_1.src.Base.Pipeline;
using RTSFramework_v0_1.src.Base.Request;
namespace RTSFramework_v0_1.src.Base.Event
{
    /// <summary>
    ///     Express a change to a stage of a existing request
    /// </summary>
    public class ChangeRequestRequest : ChangeRequest
    {
        public ChangeRequestRequest(string subpipeline_name, PrimitiveChange change, PrimitiveData target) :
            base( subpipeline_name, change, target ) { }
    }
    /// <summary>
    ///     Express a change that will add a new request to A <see cref="IAddable" />
    /// </summary>
    public class AddRequestRequest : AddRequest
    {
        public AddRequestRequest(string subpipeline_name, Effect _effect, Request.Request _target) : base( subpipeline_name )
        {
            this._effect = _effect;
            this._target = _target;
        }
        public override IAddable target => _effect;
        Effect _effect;
        public override IAddable.IBeAddedable item => _target;
        Request.Request _target;
    }
    public interface IEditEffect : IInPipelineStage
    {
        public (ChangeRequestRequest[] changes, AddRequestRequest[] adds) Edit(in Effect e);
    }
}
