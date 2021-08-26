namespace RTSFramework
{
    /// <summary>
    ///     Express a change to a value of a existing request
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
        public AddRequestRequest(string subpipeline_name, Event _event, Request _target) : base( subpipeline_name )
        {
            this._event = _event;
            this._target = _target;
        }
        public override IAddable target => _event;
        Event _event;
        public override IAddable.IBeAddedable item => _target;
        Request _target;
    }
    public interface IEditEvent : IInPipelineStage
    {
        public (ChangeRequestRequest[] changes, AddRequestRequest[] adds) Edit(in Event e);
    }
}
