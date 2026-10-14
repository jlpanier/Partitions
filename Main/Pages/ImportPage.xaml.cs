using Main.ViewModels;

namespace Main.Pages;

public partial class ImportPage : ContentPage, IQueryAttributable
{
    /// <summary>
    /// Applique les attributs de requête
    /// </summary>
    /// <param name="query">Les attributs de requête</param>
    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (BindingContext is ImportViewModel vm)
        {
                vm.Init();
        }
    }
    public ImportPage()
	{
		InitializeComponent();
	}
}