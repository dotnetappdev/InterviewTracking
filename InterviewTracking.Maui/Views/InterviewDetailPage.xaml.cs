using InterviewTracking.Maui.ViewModels;

namespace InterviewTracking.Maui.Views;

public partial class InterviewDetailPage : ContentPage
{
    public InterviewDetailPage(InterviewDetailViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
