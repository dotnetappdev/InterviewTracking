using InterviewTracking.Maui.ViewModels;

namespace InterviewTracking.Maui.Views;

public partial class CompaniesAZPage : ContentPage
{
    private readonly InterviewListViewModel _viewModel;

    public CompaniesAZPage(InterviewListViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        
        // Load companies A-Z when page appears
        _viewModel.LoadCompaniesAZCommand.Execute(null);
    }
}
