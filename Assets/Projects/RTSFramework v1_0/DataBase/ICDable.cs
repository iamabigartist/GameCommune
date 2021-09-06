namespace RTSFramework_v1_0.DataBase
{
    public interface ICD_able<T>
    {
        void Create(T element);
        void Delete(T element);
    }
}
