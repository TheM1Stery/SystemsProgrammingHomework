using SimpleInjector;

namespace CustomerDb;

public partial class App
{
    // Because this class is a part of Composition root, this class does not use Service Locator pattern.
    private partial class ViewModelFactory<T>
    {
        private readonly Container _container;

        public ViewModelFactory(Container container)
        {
            _container = container;
        }

        public T Create<TViewModel>() where TViewModel : class, T
        {
            return _container.GetInstance<TViewModel>();
        }
    }
}