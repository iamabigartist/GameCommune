namespace RTSFramework
{
    /// <summary>
    ///     Express a change to a value of a existing request
    /// </summary>
    public class ChangeRequestRequest : ChangeRequest
    {
        public ChangeRequestRequest(int pipeline_depth, PrimitiveChange change, PrimitiveData target) : base( pipeline_depth, change, target ) { }
    }
    /// <summary>
    ///     Express a change that will add a new request to A <see cref="IAddable" />
    /// </summary>
    public class AddRequestRequest : AddRequest
    {
        public AddRequestRequest(int pipeline_depth, Event _event, Request _target) : base( pipeline_depth )
        {
            this._event = _event;
            this._target = _target;
        }
        public override IAddable target => _event;
        Event _event;
        public override IAddable.IBeAddedable item => _target;
        Request _target;
    }
    public interface IEditEvent
    {
        public int pipeline_depth { get; }
        public (ChangeRequestRequest[] changes, AddRequestRequest[] adds) Edit(in Event e);
    }
}
