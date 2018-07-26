using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml;
using System.Xml.Serialization;

namespace ide
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : Window
    {
        private Configuration configuration;

        public Settings(Configuration configuration)
        {
            InitializeComponent();
            this.configuration = configuration;
            int index = 0;
            try
            {
                foreach (var font in Fonts.SystemFontFamilies)
                {
                    System.Windows.Controls.Label lbl = new System.Windows.Controls.Label();
                    lbl.Content = font.Source;
                    lbl.FontFamily = new FontFamily(lbl.Content.ToString());
                    lbl.FontSize = 16;
                    combo_Font.Items.Add(lbl);
                    if (configuration.font == font.Source)
                    {
                        index = combo_Font.Items.Count - 1;
                    }
                }
                combo_Font.SelectedIndex = index;
                ClrPcker_Background.SelectedColor = (Color)ColorConverter.ConvertFromString(configuration.DefaultForeColor);
                ClrPcker_Comment.SelectedColor = (Color)ColorConverter.ConvertFromString(configuration.CommentForeColor);
                ClrPcker_Number.SelectedColor = (Color)ColorConverter.ConvertFromString(configuration.NumberForeColor);
                ClrPcker_Word.SelectedColor = (Color)ColorConverter.ConvertFromString(configuration.WordForeColor);
                ClrPcker_String.SelectedColor = (Color)ColorConverter.ConvertFromString(configuration.StringForeColor);
                ClrPcker_StringBack.SelectedColor = (Color)ColorConverter.ConvertFromString(configuration.StringEolBackColor);
                ClrPcker_Char.SelectedColor = (Color)ColorConverter.ConvertFromString(configuration.CharacterForeColor);
                ClrPcker_Verbatim.SelectedColor = (Color)ColorConverter.ConvertFromString(configuration.VerbatimForeColor);
                ClrPcker_Operator.SelectedColor = (Color)ColorConverter.ConvertFromString(configuration.OperatorForeColor);
                ClrPcker_Preprocessor.SelectedColor = (Color)ColorConverter.ConvertFromString(configuration.PreprocessorForeColor);
                txt_FontSize.Text = configuration.fontSize.ToString();
                txt_Line.Text = configuration.marginWidth.ToString();
                txt_Path.Text = configuration.compilerPath;
            }catch(Exception ex) { System.Windows.MessageBox.Show("Configuration error: " + ex.Message); }
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
            configuration.DefaultForeColor = ClrPcker_Background.SelectedColor.ToString();
            configuration.CommentForeColor = ClrPcker_Comment.SelectedColor.ToString();
            configuration.CommentLineDocForeColor = configuration.CommentForeColor;
            configuration.CommentLineForeColor = configuration.CommentForeColor;
            configuration.NumberForeColor = ClrPcker_Number.SelectedColor.ToString();
            configuration.WordForeColor = ClrPcker_Word.SelectedColor.ToString();
            configuration.Word2ForeColor = configuration.WordForeColor;
            configuration.StringForeColor = ClrPcker_String.SelectedColor.ToString();
            configuration.StringEolBackColor = ClrPcker_StringBack.SelectedColor.ToString();
            configuration.CharacterForeColor = ClrPcker_Char.SelectedColor.ToString();
            configuration.VerbatimForeColor = ClrPcker_Verbatim.SelectedColor.ToString();
            configuration.OperatorForeColor = ClrPcker_Operator.SelectedColor.ToString();
            configuration.PreprocessorForeColor = ClrPcker_Preprocessor.SelectedColor.ToString();
            configuration.font = ((System.Windows.Controls.Label)combo_Font.SelectedItem).Content.ToString();
            configuration.fontSize = Convert.ToInt32(txt_FontSize.Text);
            configuration.marginWidth = Convert.ToInt32(txt_Line.Text);
            configuration.compilerPath = txt_Path.Text;
            XmlSerializer x = new XmlSerializer(typeof(Configuration));
            StreamWriter s = new StreamWriter("conf.xml");
            x.Serialize(s, configuration);
            s.Dispose();
            this.DialogResult = true;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            //fbd.RootFolder = Environment.SpecialFolder.MyComputer;
            if(fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txt_Path.Text = fbd.SelectedPath;
            }
        }
    }
}
