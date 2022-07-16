using TextCipher.Services;
using TextCipher.ViewModels;

namespace TextCipher.Factories;

public class TabFactory : ITabFactory
{
    private readonly IEncryptionService _encryptionService;
    private readonly ITextFileGetterService _textFileGetterService;

    public TabFactory(IEncryptionService encryptionService, ITextFileGetterService textFileGetterService)
    {
        _encryptionService = encryptionService;
        _textFileGetterService = textFileGetterService;
    }
    
    public TabInfoViewModel Create()
    {
        return new TabInfoViewModel(_encryptionService, _textFileGetterService);
    }
}