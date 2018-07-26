using ScintillaNET;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;

using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;
using System.Xml.Serialization;
using System.Xml;
using System.Drawing;
using LiveCharts;
using LiveCharts.Wpf;


namespace ide
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ProjectProperties projectProperties;
        private TreeViewItem treeItem;
        private List<string> projectFiles;
        private Configuration configuration;
        private ErrorParser errorParser;
        private Process program;
        public SeriesCollection ramValues { get; set; }
        public SeriesCollection cpuValues { get; set; }
        PerformanceCounter ramCounter;
        PerformanceCounter cpuCounter;
        Task task;
        CancellationTokenSource tokenSource;
        public MainWindow()
        {

            InitializeComponent();
            projectProperties = new ProjectProperties();
            treeItem = new TreeViewItem();
            projectFiles = new List<string>();

            editor.Indicators[8].Style = IndicatorStyle.StraightBox;
            editor.Indicators[8].OutlineAlpha = 50;
            editor.Indicators[8].Alpha = 100;
            editor.Indicators[8].Under = true;
            editor.Indicators[8].ForeColor = Color.Red;

            ramChart.AxisY.Clear();
            ramChart.AxisY.Add(new Axis
            {
                Title = "Memory usage (MB)",
                MinHeight = 0
            });

            cpuChart.AxisY.Clear();
            cpuChart.AxisY.Add(new Axis
            {
                Title = "CPU usage (%)",
                MinHeight = 0,
                MaxHeight = 100
            });
            ramValues = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Memory usage (MB)",
                    Values = new ChartValues<double>(),
                    LineSmoothness = 0,
                    ToolTip = "Memory usage (MB)"
                }
            };

            cpuValues = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "CPU usage",
                    Values = new ChartValues<double>(),
                    LineSmoothness=0,
                    ToolTip = "CPU usage (%)"
                }
            };
            DataContext = this;

        }

        private async void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            NewProject project = new NewProject(projectProperties, treeItem, projectFiles);
            if (project.ShowDialog() == true)
            {
                projectProperties.currentFile = null;
                editor.ClearAll();
                treeView1.Items.Clear();
                treeView1.Items.Add(treeItem);
                this.Title = projectProperties.projectPath;
                if (treeItem.Items.Count != 0)
                {
                    try
                    {
                        var loader = editor.CreateLoader(256);
                        var document = await LoadFileAsync(loader, projectProperties.projectPath + @"\" + "main" + projectProperties.extension, new CancellationTokenSource().Token);
                        editor.Document = document;
                    }
                    catch (Exception ex) { System.Windows.MessageBox.Show("Problem occured!"); }
                }
            }
        }

        private async Task<Document> LoadFileAsync(ILoader loader, string path, CancellationToken cancellationToken)
        {
            try
            {
                using (var file = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize: 4096, useAsync: false))
                using (var reader = new StreamReader(file))
                {
                    var count = 0;
                    var buffer = new char[4096];
                    while ((count = await reader.ReadAsync(buffer, 0, buffer.Length).ConfigureAwait(false)) > 0)
                    {
                        cancellationToken.ThrowIfCancellationRequested();

                        if (!loader.AddData(buffer, count))
                            throw new IOException("File can't be loaded!.");
                    }

                    return loader.ConvertToDocument();
                }
            }
            catch
            {
                loader.Release();
                throw;

            }
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Project file (*.proj)|*.proj";
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                projectProperties.projectPath = System.IO.Path.GetDirectoryName(ofd.FileName);
                this.Title = projectProperties.projectPath;
                using (FileStream fs = new FileStream(ofd.FileName, FileMode.Open))
                using (BinaryReader br = new BinaryReader(fs))
                {
                    projectProperties.extension = br.ReadString().Trim();
                    if (projectProperties.extension == ".c")
                        projectProperties.compiler = "gcc.exe";
                    else
                        projectProperties.compiler = "g++.exe";
                    projectProperties.projectName = br.ReadString().Trim();
                }
                var files = Directory.GetFiles(projectProperties.projectPath, "*").Where(s => s.EndsWith(projectProperties.extension) || s.EndsWith(".h"));
                treeItem.Header = projectProperties.projectName;
                projectProperties.currentFile = null;
                editor.ClearAll();
                treeView1.Items.Clear();
                treeItem.Items.Clear();
                projectFiles.Clear();
                treeView1.Items.Add(treeItem);
                foreach (var file in files)
                {
                    treeItem.Items.Add(new TreeViewItem() { Header = System.IO.Path.GetFileName(file) });
                    projectFiles.Add(file);
                }
                treeItem.ExpandSubtree();
                btnNewFile.IsEnabled = true;
                btnSave.IsEnabled = true;
                editor.IsEnabled = false;

            }
        }

        private async void treeView1_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TreeViewItem tvi = (TreeViewItem)((System.Windows.Controls.TreeView)sender).SelectedItem;
            if (tvi.Header.ToString() != projectProperties.projectName)
            {
                editor.IsEnabled = true;
                if (projectProperties.currentFile != null)
                {
                    saveChanges();
                }
                try
                {
                    var loader = editor.CreateLoader(256);
                    var document = await LoadFileAsync(loader, projectProperties.projectPath + @"\" + tvi.Header.ToString(), new CancellationTokenSource().Token);
                    editor.Document = document;
                    configuration.Configure(editor);
                    projectProperties.currentFile = tvi.Header.ToString();
                }
                catch (Exception ex) { System.Windows.MessageBox.Show("Problem occured!"); }
            }
        }

        private void saveChanges()
        {
            if (!string.IsNullOrEmpty(projectProperties.currentFile))
            {
                try
                {
                    using (FileStream fs = new FileStream(projectProperties.projectPath + @"\" + projectProperties.currentFile, FileMode.Create))
                    using (StreamWriter sw = new StreamWriter(fs))
                    {
                        sw.Write(editor.Text);
                    }
                }catch(Exception ex) { System.Windows.MessageBox.Show(ex.Message); }
            }
        }

        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            saveChanges();
        }

        private void MenuItem_Click_3(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void MenuItem_Click_4(object sender, RoutedEventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.InitialDirectory = System.IO.Path.GetFullPath(projectProperties.projectPath);
            sfd.RestoreDirectory = true;
            sfd.Filter = "C/C++ file|" + projectProperties.extension + "|Header file|.h";
            if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                using (FileStream fs = (FileStream)sfd.OpenFile())
                {
                    projectFiles.Add(sfd.FileName);
                    treeItem.Items.Add(new TreeViewItem() { Header = System.IO.Path.GetFileName(sfd.FileName) });
                }

            }

        }

        private void MenuItem_Click_5(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(projectProperties.projectPath))
                Compile();
        }

        private async Task<bool> Compile()
        {
            saveChanges();
            Refresh();
            if (program == null || program.HasExited)
            {

                editor.IndicatorClearRange(0, editor.TextLength);
                Process compiler = new Process();
                compiler.StartInfo.FileName = configuration.compilerPath + @"\" + projectProperties.compiler;
                string argument = "";
                for (int i = 0; i < projectFiles.Count; i++)
                {
                    argument += projectFiles[i];
                    argument += " ";
                }
                compiler.StartInfo.Arguments = argument + "-o " + projectProperties.projectPath + @"\" + projectProperties.projectName + ".exe";
                compiler.StartInfo.UseShellExecute = false;
                compiler.StartInfo.CreateNoWindow = true;
                compiler.StartInfo.RedirectStandardError = true;
                compiler.EnableRaisingEvents = true;
                var watch = System.Diagnostics.Stopwatch.StartNew();
                try
                {
                    compiler.Start();
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show("Compiler error! " + ex.Message);
                    throw;

                }
                string output = compiler.StandardError.ReadToEnd();
                watch.Stop();
                var elapsedMs = watch.Elapsed;
                richTextBox1.Document.Blocks.Clear();

                if (string.IsNullOrEmpty(output))
                {
                    errorListView.Items.Clear();
                    richTextBox1.Document.Blocks.Add(new Paragraph(new Run("Program compiled succesfully! Time: " + elapsedMs.ToString())));
                    return true;
                }
                else
                {
                    richTextBox1.Document.Blocks.Add(new Paragraph(new Run(output)));
                }
                errorParser = new ErrorParser(output);
                errorParser.Process();
                errorListView.Items.Clear();
                if (errorParser.errors.Count != 0)
                {
                    for (int i = 0; i < errorParser.errors.Count; i++)
                    {
                        System.Windows.Controls.ListViewItem lvi = new System.Windows.Controls.ListViewItem();//System.Windows.Forms.ListViewItem();
                        lvi.Content = errorParser.errors[i].error;
                        lvi.Tag = errorParser.errors[i];
                        errorListView.Items.Add(lvi);
                    }
                    int n = errorParser.errors[0].line;
                    string path = errorParser.errors[0].path;
                    var document = await LoadFileAsync(editor.CreateLoader(256), path, new CancellationTokenSource().Token);
                    editor.Document = document;
                    projectProperties.currentFile = System.IO.Path.GetFileName(path);
                    configuration.Configure(editor);
                    int start = editor.Lines[n - 1].Position;
                    int end = editor.Lines[n - 1].EndPosition;
                    editor.IndicatorClearRange(0, editor.TextLength);
                    editor.IndicatorCurrent = 8;
                    editor.IndicatorFillRange(start, end - start);
                    editor.IsEnabled = true;
                }

                compiler.WaitForExit();

            }
            else System.Windows.MessageBox.Show("Another instance of program is already running.");
            return false;


        }

        private void Run()
        {
            if (program == null || program.HasExited)
            {
                program = new Process();
                /* program.StartInfo.FileName = "cmd.exe";
                 program.StartInfo.Arguments = "/K " + projectProperties.projectPath + @"\" + projectProperties.projectName + ".exe" + " & echo. & pause & exit";
                 */
                program.StartInfo.FileName = projectProperties.projectPath + @"\" + projectProperties.projectName + ".exe";

                program.StartInfo.UseShellExecute = true;
                program.Start();
                ramCounter = new PerformanceCounter("Process", "Working Set", program.ProcessName);
                cpuCounter = new PerformanceCounter("Process", "% Processor Time", program.ProcessName);
            }
            else
                System.Windows.MessageBox.Show("Another instance of program is already running.");
        }

        private async void MenuItem_Click_6(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(projectProperties.projectPath))
            {
                try
                {
                    ClearChart(null, null);
                    bool compiled = await Compile();
                    if (compiled)
                        Run();
                }
                catch(Exception ex) { System.Windows.MessageBox.Show(ex.Message); }
                



                tokenSource = new CancellationTokenSource();
                task = Task.Run(() =>
                {
                    tokenSource.Token.ThrowIfCancellationRequested();
                    try
                    {
                        while (!program.HasExited)
                        {
                            if (tokenSource.Token.IsCancellationRequested)
                            {
                                tokenSource.Token.ThrowIfCancellationRequested();
                            }

                            double ram = ramCounter.NextValue();
                            double cpu = cpuCounter.NextValue();
                            ramValues[0].Values.Add(Convert.ToDouble(ram / 1024 / 1024));
                            cpuValues[0].Values.Add(Convert.ToDouble(cpu));
                            Thread.Sleep(1000);
                        }

                    }
                    catch (Exception ex) { }

                }, tokenSource.Token);

            }


        }



        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Configuration));
            FileStream fs = new FileStream("conf.xml", FileMode.Open);
            XmlReader xml = XmlReader.Create(fs);
            try
            {
                configuration = (Configuration)serializer.Deserialize(xml);
            }catch(Exception ex) { System.Windows.MessageBox.Show("Configuration error: " + ex.Message); }
        }

        private async void errorListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            System.Windows.Controls.ListViewItem lvi = (System.Windows.Controls.ListViewItem)((System.Windows.Controls.ListView)sender).SelectedItem;
            if (lvi != null)
            {
                int n = ((Error)lvi.Tag).line;
                string path = ((Error)lvi.Tag).path;
                var document = await LoadFileAsync(editor.CreateLoader(256), path, new CancellationTokenSource().Token);
                editor.Document = document;
                projectProperties.currentFile = System.IO.Path.GetFileName(path);
                configuration.Configure(editor);
                int start = editor.Lines[n - 1].Position;
                int end = editor.Lines[n - 1].EndPosition;
                editor.IndicatorClearRange(0, editor.TextLength);
                editor.IndicatorCurrent = 8;
                editor.IndicatorFillRange(start, end - start);
                editor.IsEnabled = true;
            }
        }

        private void MenuItem_Click_7(object sender, RoutedEventArgs e)
        {
            Refresh();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                tokenSource.Cancel();
            }
            catch (Exception ex) { }
            if (program != null && !program.HasExited)
                program.Kill();



        }

        private void Refresh()
        {
            if (!string.IsNullOrEmpty(projectProperties.projectPath))
            {
                try
                {
                    var files = Directory.GetFiles(projectProperties.projectPath, "*").Where(s => s.EndsWith(projectProperties.extension) || s.EndsWith(".h"));
                    treeItem.Header = projectProperties.projectName;
                    treeView1.Items.Clear();
                    treeItem.Items.Clear();
                    projectFiles.Clear();
                    treeView1.Items.Add(treeItem);
                    foreach (var file in files)
                    {
                        treeItem.Items.Add(new TreeViewItem() { Header = System.IO.Path.GetFileName(file) });
                        projectFiles.Add(file);
                    }
                    treeItem.ExpandSubtree();
                }catch(Exception ex) { System.Windows.MessageBox.Show(ex.Message); }
            }
        }

        private void editor_CharAdded(object sender, CharAddedEventArgs e)
        {
            char c = (char)e.Char;
            switch (c)
            {
                case '(':
                    editor.InsertText(-1, ")");
                    break;
                case '{':
                    editor.InsertText(-1, "}");
                    break;
                case '[':
                    editor.InsertText(-1, "]");
                    break;
                case '\'':
                    editor.InsertText(-1, "'");
                    break;
                case '"':
                    editor.InsertText(-1, "\"");
                    break;
            }
        }

        private void MenuItem_Click_8(object sender, RoutedEventArgs e)
        {
            if (new Settings(configuration).ShowDialog() == true)
            {
                configuration.Configure(editor);
            }
        }

        private void ClearChart(object sender, RoutedEventArgs e)
        {
            ramValues[0].Values.Clear();
            cpuValues[0].Values.Clear();
        }


    }
}



