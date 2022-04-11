using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MSApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ObservableCollection<string> Items;     // ObservableCollection

        public MainWindow()
        {
            InitializeComponent();
            Items = new ObservableCollection<string>(); //создаем внутрннее кол-во данных
            ItemsListBox.ItemsSource = Items;
        }

        private void SelectFileButton_Click(object sender, RoutedEventArgs e)
        {
            FilePathTextBox.Text = "";
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                FilePathTextBox.Text = openFileDialog.FileName;
            }
        }

        private void SelectFolderButton_Click(object sender, RoutedEventArgs e)
        {
            FolderPathTextBox.Text = "";
            var dialog = new FolderBrowserDialog();
            dialog.Description = "Выбор папки";
            dialog.ShowNewFolderButton = true;
            dialog.RootFolder = System.Environment.SpecialFolder.Desktop;
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                FolderPathTextBox.Text = dialog.SelectedPath;
            }
        }
        public string FolderPath1;
        private void SelectFolderButton1_Click(object sender, RoutedEventArgs e)
        {
            FolderPathTextBox1.Text = "";
            var dialog = new FolderBrowserDialog();
            dialog.Description = "Выбор папки";
            dialog.ShowNewFolderButton = true;
            dialog.RootFolder = System.Environment.SpecialFolder.Desktop;
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                FolderPathTextBox1.Text = dialog.SelectedPath;
            }
            FolderPath1 = FolderPathTextBox1.Text;
        }

        private void StartFileButton_Click(object sender, RoutedEventArgs e)
        {
            if (FilePathTextBox.Text.Trim() != "" || FilePathTextBox.Text.Trim() != "Файла не существует")
            {
                if (File.Exists(FilePathTextBox.Text))
                {
                    ShellExecute shellExecute = new ShellExecute();
                    shellExecute.Verb = ShellExecute.OpenFile;
                    shellExecute.Path = FilePathTextBox.Text;
                    shellExecute.Execute();
                }
                else
                {
                    FilePathTextBox.Text = "Файла не существует";
                }
            }
            else
            {
                FilePathTextBox.Text = "Файл не указан";
            }
        }
        private void OpenFileButton_Click(object sender, RoutedEventArgs e)
        {
            if (FilePathTextBox.Text.Trim() != "" || FilePathTextBox.Text.Trim() != "Файла не существует")
            {
                if (File.Exists(FilePathTextBox.Text))
                {
                    ShellExecute shellExecute = new ShellExecute();
                    shellExecute.Verb = ShellExecute.EditFile;
                    shellExecute.Path = FilePathTextBox.Text;
                    shellExecute.Execute();
                }
                else
                {
                    FilePathTextBox.Text = "Файла не существует";
                }
            }
            else
            {
                FilePathTextBox.Text = "Файл не указан";
            }
        }
        private void PrintFileButton_Click(object sender, RoutedEventArgs e)
        {
            if (FilePathTextBox.Text.Trim() != "" || FilePathTextBox.Text.Trim() != "Файла не существует")
            {
                if (File.Exists(FilePathTextBox.Text))
                {
                    ShellExecute shellExecute = new ShellExecute();
                    shellExecute.Verb = ShellExecute.PrintFile;
                    shellExecute.Path = FilePathTextBox.Text;
                    shellExecute.Execute();
                }
                else
                {
                    FilePathTextBox.Text = "Файла не существует";
                }
            }
            else
            {
                FilePathTextBox.Text = "Файл не указан";
            }
        }

        private void OpenFolderButton_Click(object sender, RoutedEventArgs e)
        {
            if (FolderPathTextBox.Text.Trim() != "" || FolderPathTextBox.Text.Trim() != "Папки не существует")
            {
                if (File.Exists(FolderPathTextBox.Text))
                {
                    ShellExecute shellExecute = new ShellExecute();
                    shellExecute.Verb = ShellExecute.ExploreFolder;
                    shellExecute.Path = FolderPathTextBox.Text;
                    shellExecute.Execute();
                }
                else
                {
                    FolderPathTextBox.Text = "Папки не существует";
                }
            }
            else
            {
                FolderPathTextBox.Text = "Папка не указана";
            }
        }

        private void SearchFolderButton_Click(object sender, RoutedEventArgs e)
        {
            if (FolderPathTextBox.Text.Trim() != "" || FolderPathTextBox.Text.Trim() != "Папки не существует")
            {
                if (File.Exists(FolderPathTextBox.Text))
                {
                    ShellExecute shellExecute = new ShellExecute();
                    shellExecute.Verb = ShellExecute.FindInFolder;
                    shellExecute.Path = FolderPathTextBox.Text;
                    shellExecute.Execute();
                }
                else
                {
                    FolderPathTextBox.Text = "Папки не существует";
                }
            }
            else
            {
                FolderPathTextBox.Text = "Папка не указана";
            }
        }

        private void AddFileButton_Click(object sender, RoutedEventArgs e)
        {
            if (FilePathTextBox.Text != "")
            {
                if (Items.IndexOf(FilePathTextBox.Text) == -1)
                {
                    if (File.Exists(FilePathTextBox.Text))
                    {
                        Items.Add(FilePathTextBox.Text);
                        ItemsListBox.Items.Refresh();
                    }
                }
            }
        }
        private void RemoveListFileButton_Click(object sender, RoutedEventArgs e)
        {
            Items.RemoveAt(ItemsListBox.SelectedIndex);
            ItemsListBox.Items.Refresh();
        }
        private void ClearListFileButton_Click(object sender, RoutedEventArgs e)
        {
            Items.Clear();
            ItemsListBox.Items.Refresh();
        }

        private void CopyFolderButton_Click(object sender, RoutedEventArgs e)
        {
            FileOperation.CopyFilesWithMasks(FolderPathTextBox.Text, FolderPathTextBox1.Text, MaskFilePathTextBox.Text);
        }

        private void MoveFolderButton_Click(object sender, RoutedEventArgs e)
        {
            FileOperation.MoveFilesWithMasks(FolderPathTextBox.Text, FolderPathTextBox1.Text, MaskFilePathTextBox.Text);
        }

        private void DeleteFolderButton_Click(object sender, RoutedEventArgs e)
        {
            FileOperation.DeleteFilesWithMasks(FolderPathTextBox.Text, MaskFilePathTextBox.Text);
        }

        private void CopyFileButton_Click(object sender, RoutedEventArgs e)
        {
            FileOperation.CopyFilesByList(Items, FolderPathTextBox1.Text);
        }

        private void MoveFileButton_Click(object sender, RoutedEventArgs e)
        {
            FileOperation.MoveFilesByList(Items, FolderPathTextBox1.Text);
        }

        private void RemoveFileButton_Click(object sender, RoutedEventArgs e)
        {
            FileOperation.DeleteFilesByList(Items);
        }

        private void RenameFileButton_Click(object sender, RoutedEventArgs e)
        {
            if (FilePathTextBox.Text.Trim() != "" || FilePathTextBox.Text.Trim() != "Файла не существует")
            {
                if (File.Exists(FilePathTextBox.Text))
                {
                    var strVal = Interaction.InputBox("Введите новое имя файла", "Переименование", System.IO.Path.GetFileName(FilePathTextBox.Text));
                    FileOperation.RenameFile(FilePathTextBox.Text, strVal);
                }
            } 
        }
    }
}
