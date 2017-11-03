public interface IServiceFactory<T> where T : class {
    T GetService();
}