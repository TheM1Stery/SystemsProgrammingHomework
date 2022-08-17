namespace MVVMUtils;

public interface IViewModelFactory<TBaseViewModel>
{
    public TBaseViewModel Create<TViewModel>() where TViewModel : class, TBaseViewModel;
}