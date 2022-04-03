namespace AnN3x.StyledGUI.Collections;

public interface IStorageService
{
    public T Get<T>(StringHandle name) where T : IStoredType;
    public bool TryGet<T>(StringHandle name, out T value) where T : IStoredType;
    public bool Contains<T>(StringHandle name) where T : IStoredType;
}
