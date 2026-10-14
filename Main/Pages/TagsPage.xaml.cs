using Main.ViewModels;

namespace Main.Pages;

public partial class TagsPage : ContentPage, IQueryAttributable
{
    /// <summary>
    /// Applique les attributs de requête
    /// </summary>
    /// <param name="query">Les attributs de requête</param>
    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (BindingContext is TagsViewModel vm)
        {
            vm.Init();
        }
    }

    public TagsPage()
	{
		InitializeComponent();
	}
}