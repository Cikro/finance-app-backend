namespace finance_app.Types.Interfaces
{
    interface ICrudService<T>
    {
        T CreateItems(T item);
        T GetItems(T item);
        T DeleteItems(int itemId);
        T UpdateItems(T item);
    }
}
