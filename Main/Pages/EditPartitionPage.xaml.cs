using Main.ViewModels;

namespace Main.Pages;

public partial class EditPartitionPage : ContentPage, IQueryAttributable
{
    /// <summary>
    /// Applique les attributs de requête
    /// </summary>
    /// <param name="query">Les attributs de requête</param>
    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (BindingContext is EditPartitionViewModel vm)
        {
            if (query.TryGetValue("Name", out var objId) && objId is int key)
            {
                vm.Init(key);
            }
        }
    }

    public EditPartitionPage()
	{
		InitializeComponent();
	}
}