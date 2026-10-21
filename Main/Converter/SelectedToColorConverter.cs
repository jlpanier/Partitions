using System.Globalization;


namespace Main.Converter
{
    public class SelectedToColorConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            bool isSelected = value is bool b && b;

            var primarycolor = Application.Current!.Resources["Primary"] as Color;
            var backgroundcolor = Application.Current.Resources["Background"] as Color;

            var color1 = primarycolor ?? Colors.White;
            var color2 = backgroundcolor ?? Colors.Black;

            return isSelected ? color1 : color2;
       }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }

}
