using LibFoxtaurAdmin.Services.Abstract;
using MessageBox.Avalonia;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia.Enums;

namespace LibFoxtaurAdmin.Services.Implementations;

public class UserMessagesService : IUserMessagesService
{
    public async Task ShowMessageAsync(string title, string text, Icon icon)
    {
        await MessageBoxManager.GetMessageBoxStandardWindow(
                new MessageBoxStandardParams()
                {
                    ContentTitle = title,
                    ContentMessage = text,
                    Icon = icon,
                    ButtonDefinitions = ButtonEnum.Ok
                })
            .Show();
    }
}