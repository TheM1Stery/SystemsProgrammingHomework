using TextCipher.ViewModels;

namespace TextCipher.Factories;

public class TabFactory : ITabFactory
{
    public TabFactory()
    {
        
    }
    
    public TabInfoViewModel Create(string message)
    {
        return new TabInfoViewModel(message);
    }
}