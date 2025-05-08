using CollegeDocumentService.ViewModels.Base;

namespace CollegeDocumentService.Navigators
{
    public interface IMainWindowNavigator
    {
        ViewModel CurrentView {  get; }
        Task NavigateTo<TViewModel>() where TViewModel : ViewModel;
    }

    public partial class MainWindowNavigator : ViewModel, IMainWindowNavigator
    {
        protected readonly Func<Type, ViewModel> _viewModelFactory;

        public MainWindowNavigator(
            Func<Type, ViewModel> viewModelFactory)
        {
            _viewModelFactory = viewModelFactory;
        }

        private ViewModel _currentView;

        public ViewModel CurrentView
        {
            get => _currentView;
            private set
            {
                if (_currentView != null)
                {
                    var newTypeOfVM = value.GetType();
                    var oldTypeOfVM = _currentView.GetType();

                    if (newTypeOfVM != oldTypeOfVM) return;
                }

                _currentView = value;
                OnPropertyChanged(nameof(CurrentView));
            }
        }

        public virtual async Task NavigateTo<TViewModel>() where TViewModel : ViewModel
        {
            var viewModel = _viewModelFactory.Invoke(typeof(TViewModel));
            CurrentView = viewModel;
            await viewModel.OnAppearingAsync();
        }
    }
}
