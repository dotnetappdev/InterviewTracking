using InterviewTracking.Maui.ViewModels;

namespace InterviewTracking.Maui.Views;

public partial class InterviewListPage : ContentPage
{
    private readonly InterviewListViewModel _viewModel;

    public InterviewListPage(InterviewListViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.InitializeAsync();
    }
}
