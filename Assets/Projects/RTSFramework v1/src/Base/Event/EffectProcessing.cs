namespace RTSFramework_v1.Base.Event
{

    /// <summary>
    ///     0. The initial state: EventEditors and Events (Generate outside the processor)
    ///     0.1. Events will find who shall edit itself and register itself into the editor.
    ///     In one pipeline stage:
    ///     1. EventEditors in this stage will raise editing requests to the existing events.
    ///     2. Editing Requests in this stage will be processed.
    ///     3. Requests in this stage will be processed.
    ///     4.Go through all the pipeline stages.
    ///     5.For different event types, events should be deleted and unregistered if temporary.
    ///     Every Editor itself will deal with the temporary event list remove and event shall be deleted by the system.
    /// </summary>
    public static class EffectProcessing { }
}
