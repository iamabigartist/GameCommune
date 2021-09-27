namespace RTSFramework_v1_0.DataBase.Requests
{
    public interface ICD_able<T>:IRequestTo
    {
        void Create(T element);
        void Delete(T element);
    }
}
