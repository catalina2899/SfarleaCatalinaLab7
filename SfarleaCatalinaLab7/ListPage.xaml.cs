using SfarleaCatalinaLab7.Models;
namespace SfarleaCatalinaLab7;

public partial class ListPage : ContentPage
{
	public ListPage()
	{
		InitializeComponent();
	}
    async void OnSaveButtonClicked(object sender, EventArgs e)
    {
        var slist = (ShopList)BindingContext;
        slist.Date = DateTime.UtcNow;
        await App.Database.SaveShopListAsync(slist);
        await Navigation.PopAsync();
    }
    async void OnDeleteButtonClicked(object sender, EventArgs e)
    {
        var slist = (ShopList)BindingContext;
        await App.Database.DeleteShopListAsync(slist);
        await Navigation.PopAsync();
    }
    async void OnChooseButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ProductPage((ShopList)
    this.BindingContext)
        {
            BindingContext = new Product()
        });

    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        var shopl = (ShopList)BindingContext;

        listView.ItemsSource = await App.Database.GetListProductsAsync(shopl.ID);
    }

    async void OnDeleteItemButtonClicked(object sender, EventArgs e)
    {
        var shopl = (ShopList)BindingContext;
        var selectedProduct = listView.SelectedItem as Product;

        if (selectedProduct == null)
        {
            await DisplayAlert("Warning",
                               "Please select an item from the list first.",
                               "OK");
            return;
        }

        bool confirm = await DisplayAlert("Delete item",
                                         $"Delete '{selectedProduct.Description}' from this shopping list?",
                                         "Yes", "No");
        if (!confirm)
            return;

        
        await App.Database.DeleteListProductAsync(shopl.ID, selectedProduct.ID);

        listView.ItemsSource = await App.Database.GetListProductsAsync(shopl.ID);
    }
}
