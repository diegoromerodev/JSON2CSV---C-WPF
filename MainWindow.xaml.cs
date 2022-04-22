using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace JSON2CSV
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ConvertToCSV(object sender, RoutedEventArgs e)
        {
            var jsonRes = JsonConvert.DeserializeObject<dynamic>(JSONInput.Text);
            JObject firstItem;
            string objValues = "";
            bool isArray = jsonRes.Type.ToString() == "Array";
            if (isArray)
            {
                firstItem = jsonRes[0];
                foreach(JObject obj in jsonRes)
                {
                    objValues += "\n" + GetValues(obj);
                }
            } 
            else
            {
                firstItem = jsonRes;
                objValues += "\n" + GetValues(firstItem);
            }
            IEnumerable<JProperty> allProps = firstItem.Properties();
            IEnumerable<string> propsString = allProps.Select(prop => prop.Name);
            string headers = string.Join(", ", propsString);
            CSVOutput.Text = headers + objValues;
        }

        private string GetValues(JObject objToGet)
        {
            IEnumerable<JProperty> objProps = objToGet.Properties();
            IEnumerable<string> propsString = objProps.Select(prop => prop.Value.ToString());
            string values = string.Join(", ", propsString);
            return values;
        }

        private void ClearJSON(object sender, RoutedEventArgs e)
        {
            JSONInput.Clear();
            JSONInput_Initialized(sender, e);
            CSVOutput_Initialized(sender, e);
        }

        private void JSONInput_GotFocus(object sender, RoutedEventArgs e)
        {
            JSONInput.Text = "";
            JSONInput.Foreground = Brushes.Black;
            JSONInput.FontStyle = FontStyles.Normal;
        }

        private void JSONInput_Initialized(object sender, EventArgs e)
        {
            JSONInput.Text = "Enter a valid JSON input here...\n{\n\t\"key\": \"value\"\n}";
            JSONInput.Foreground = Brushes.Gray;
            JSONInput.FontStyle = FontStyles.Italic;
        }
        private void CSVOutput_Initialized(object sender, EventArgs e)
        {
            CSVOutput.Text = "You will see your CSV output here...";
            CSVOutput.Foreground = Brushes.Gray;
            CSVOutput.FontStyle = FontStyles.Italic;
        }
    }
}
