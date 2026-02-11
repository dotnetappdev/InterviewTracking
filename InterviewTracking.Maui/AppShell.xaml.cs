using InterviewTracking.Maui.Views;

namespace InterviewTracking.Maui;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
		
		// Register routes for navigation
		Routing.RegisterRoute("InterviewDetail", typeof(InterviewDetailPage));
		Routing.RegisterRoute("AddEditInterview", typeof(AddEditInterviewPage));
	}
}
