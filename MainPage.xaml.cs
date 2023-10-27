using BMICalculator.Resources.Languages;
using System.Globalization;

namespace BMICalculator;

public partial class MainPage : ContentPage
{
    public LocalizationResourceManager LocalizationResourceManager
      => LocalizationResourceManager.Instance;
    public MainPage()
	{
		InitializeComponent();
        BindingContext = this;
    }

    private void Button_Clicked(object sender, EventArgs e)
    {
        if (double.TryParse(WeightEntry.Text, out double weight) && double.TryParse(HeightEntry.Text, out double height))
        {
            double heightInMeters = height / 100;
            double bmi = weight / (heightInMeters * heightInMeters);

            Preferences.Set("Nick", NickEntry.Text);
            Preferences.Set("Weight", weight);
            Preferences.Set("Height", height);
            Preferences.Set($"LastMeasurementDate_{NickEntry.Text}", DateTime.Now.ToString());

            string bmiRange = GetBmiRange(bmi);
            string bmiFormatted = bmi.ToString("F2");
            DisplayAlert(LocalizationResourceManager["resultName"].ToString(), String.Format(LocalizationResourceManager["result"].ToString(), bmiFormatted, bmiRange), "OK");
        }
        else
        {
            DisplayAlert(LocalizationResourceManager["errorName"].ToString(), LocalizationResourceManager["error"].ToString(), "OK");
        }

    }
    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (Preferences.ContainsKey("Nick"))
        {
            NickEntry.Text = Preferences.Get("Nick", string.Empty);
        }
        if (Preferences.ContainsKey("Weight"))
        {
            double weight = Preferences.Get("Weight", 0.0);
            WeightEntry.Text = weight.ToString();
        }
        if (Preferences.ContainsKey("Height"))
        {
            double height = Preferences.Get("Height", 0.0);
            HeightEntry.Text = height.ToString();
        }
        string nick = NickEntry.Text;
        if (Preferences.ContainsKey($"LastMeasurementDate_{nick}"))
        {
            string lastMeasurementDate = Preferences.Get($"LastMeasurementDate_{nick}", string.Empty);
            LastMeasurementLabel.Text = " "+lastMeasurementDate+".";
        }
    }
    private string GetBmiRange(double bmi)
    {
        if (bmi < 16.0) return LocalizationResourceManager["starvation"].ToString();
        else if (bmi < 17.0) return LocalizationResourceManager["emaciation"].ToString();
        else if (bmi < 18.5) return LocalizationResourceManager["underweight"].ToString();
        else if (bmi < 25.0) return LocalizationResourceManager["normalValue"].ToString();
        else if (bmi < 30.0) return LocalizationResourceManager["overweight"].ToString();
        else if (bmi < 35.0) return LocalizationResourceManager["ObesityClassI"].ToString();
        else if (bmi < 40.0) return LocalizationResourceManager["ObesityClassII"].ToString();
        else return LocalizationResourceManager["ObesityClassIII"].ToString();
    }


    private void ChangeLanguageBtn_Clicked(object sender, EventArgs e)
    {
        var currentCulture = AppResources.Culture;

        if (currentCulture != null)
        {
            var switchToCulture = currentCulture.TwoLetterISOLanguageName.Equals("en", StringComparison.InvariantCultureIgnoreCase)
                ? new CultureInfo("pl-PL")
                : new CultureInfo("en-US");

            LocalizationResourceManager.Instance.SetCulture(switchToCulture);

            ChangeLanguageBtn.Text = LocalizationResourceManager["switchLanguage"].ToString();

            SemanticScreenReader.Announce(ChangeLanguageBtn.Text);
        }
    }
}

