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
using LiveCharts;
using LiveCharts.Wpf;

namespace ide
{
    /// <summary>
    /// Interaction logic for NewProject.xaml
    /// </summary>
    public partial class NewProject : Window
    {
        private ProjectProperties conf;
        private TreeViewItem treeItem;
        private List<string> projectFiles;
      
        public NewProject()
        {
            InitializeComponent();
        }

        public NewProject(ProjectProperties c, TreeViewItem item, List<string> projectFiles)
        {
            this.conf = c;
            this.treeItem = item;
            this.projectFiles = projectFiles;
            InitializeComponent();
           
        }

       

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog fd = new FolderBrowserDialog();
            if (!txtLocation.Text.Equals(""))
            {
                fd.SelectedPath = txtLocation.Text;
            }
            DialogResult dr = fd.ShowDialog();
            if (dr == System.Windows.Forms.DialogResult.OK && !string.IsNullOrWhiteSpace(fd.SelectedPath))
            {
                txtLocation.Text = fd.SelectedPath;
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            string item = ((ListBoxItem)listBox1.SelectedItem).Content.ToString();
            string path = txtLocation.Text;
            string name = txtName.Text;

            if(item == "")
            {
                System.Windows.MessageBox.Show("You must choose project type!");
                return;
            }

            if (path == "")
            {
                System.Windows.MessageBox.Show("You must choose project location!");
                return;
            }

            if (name == "")
            {
                System.Windows.MessageBox.Show("You must choose project name!");
                return;
            }

            if (item == "C")
            {
                conf.extension = ".c";
                conf.compiler = "gcc.exe";
            }
            else
            {
                conf.extension = ".cpp";
                conf.compiler = "g++.exe";
            }
            conf.projectPath = path + @"\" + name;
            conf.projectName = name;

            if(Directory.Exists(path + @"\" + name))
            {
                System.Windows.MessageBox.Show("Directory with the name '" + name + "' already exists!");
                return;
            }

            try
            {
                DirectoryInfo di = Directory.CreateDirectory(path + @"\" + name);
                treeItem.Header = name;
                treeItem.Items.Clear();
                projectFiles.Clear();
                if (checkBox1.IsChecked == true)
                {
                    File.Create(di.FullName + @"\" + "main" + conf.extension).Close();
                    treeItem.Items.Add(new TreeViewItem() { Header = "main" + conf.extension});
                    treeItem.ExpandSubtree();
                    projectFiles.Add(di.FullName + @"\" + "main" + conf.extension);
                    
                }

                using (FileStream fs = new FileStream(conf.projectPath + @"\" + name + ".proj", FileMode.Create))
                    using(BinaryWriter bw = new BinaryWriter(fs))
                {
                    bw.Write(conf.extension);
                    bw.Write(conf.projectName);
                }
            }
            catch (Exception ex) { }
            this.DialogResult = true;
            this.Close();
        }
    }
}
