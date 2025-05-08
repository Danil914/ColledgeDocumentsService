using CommunityToolkit.Mvvm.ComponentModel;

namespace CollegeDocumentService.ViewModels.Base
{
    public partial class ViewModel : ObservableObject
    {
        public virtual Task OnAppearingAsync() { return Task.CompletedTask; }
        public virtual Task OnDisappearingAsync() { return Task.CompletedTask;}
    }
}