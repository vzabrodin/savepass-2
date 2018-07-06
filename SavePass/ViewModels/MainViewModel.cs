using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using Prism.Commands;
using Prism.Mvvm;
using Zabrodin.SavePass.DialogService;
using Zabrodin.SavePass.Extensions;
using Zabrodin.SavePass.Models;
using Zabrodin.SavePass.Properties;
using Zabrodin.SavePass.ViewModels.Contexts;

namespace Zabrodin.SavePass.ViewModels
{
    public class MainViewModel : BindableBase
    {
        #region Private members

        private readonly IDialogService dialogService;
        private SavePassRepository repository;
        private SavePassItem selectedItem;
        private ICollectionView items;

        #endregion

        #region Constructor

        public MainViewModel(IDialogService dialogService)
        {
            this.dialogService = dialogService;
            NewFileCommand = new DelegateCommand(OnNewFileCommand);
            OpenFileCommand = new DelegateCommand(OnOpenFileCommand);
            CloseFileCommand = new DelegateCommand(OnCloseFileCommand, OnCanCloseFileCommand);
            SaveFileCommand = new DelegateCommand(OnSaveFileCommand, OnCanSaveFileCommand);
            SaveFileAsCommand = new DelegateCommand(OnSaveFileAsCommand, OnCanSaveFileAsCommand);
            FileSettingsCommand = new DelegateCommand(OnFileSettingsCommand, OnCanFileSettingsCommand);
            AddItemCommand = new DelegateCommand(OnAddItemCommand, OnCanAddItemCommand);
            EditItemCommand = new DelegateCommand(OnEditItemCommand, OnCanEditItemCommand);
            RemoveItemCommand = new DelegateCommand(OnRemoveItemCommand, OnCanRemoveItemCommand);
            ExitCommand = new DelegateCommand(OnExitCommand);
            AboutCommand = new DelegateCommand(OnAboutCommand);
            CopyToClipboardCommand = new DelegateCommand<string>(OnCopyToClipboardCommand);
            OpenBrowserCommand = new DelegateCommand<string>(OnOpenBrowserCommand);

            // ReSharper disable once PossibleNullReferenceException
            Application.Current.MainWindow.Closing += OnShellClosing;
        }

        #endregion

        #region DelegateCommands

        public DelegateCommand NewFileCommand { get; }
        public DelegateCommand OpenFileCommand { get; }
        public DelegateCommand CloseFileCommand { get; }
        public DelegateCommand SaveFileCommand { get; }
        public DelegateCommand SaveFileAsCommand { get; }
        public DelegateCommand FileSettingsCommand { get; }
        public DelegateCommand AddItemCommand { get; }
        public DelegateCommand EditItemCommand { get; }
        public DelegateCommand RemoveItemCommand { get; }
        public DelegateCommand ExitCommand { get; }
        public DelegateCommand AboutCommand { get; }
        public DelegateCommand<string> CopyToClipboardCommand { get; }
        public DelegateCommand<string> OpenBrowserCommand { get; }

        #endregion

        #region Properties

        public ICollectionView Items
        {
            get => items;
            set => SetProperty(ref items, value);
        }

        public SavePassRepository Repository
        {
            get => repository;
            private set
            {
                SetProperty(ref repository, value);
                AddItemCommand.RaiseCanExecuteChanged();
                SaveFileCommand.RaiseCanExecuteChanged();
                SaveFileAsCommand.RaiseCanExecuteChanged();
                CloseFileCommand.RaiseCanExecuteChanged();
                FileSettingsCommand.RaiseCanExecuteChanged();
            }
        }

        public SavePassItem SelectedItem
        {
            get => selectedItem;
            set
            {
                SetProperty(ref selectedItem, value);
                EditItemCommand.RaiseCanExecuteChanged();
                RemoveItemCommand.RaiseCanExecuteChanged();
            }
        }

        #endregion

        #region Command handlers

        #region File

        private async void OnNewFileCommand()
        {
            if (await TrySave() == MessageBoxResult.Cancel)
                return;

            Repository = SavePassRepository.New();
            InitializeCollectionView();
        }

        private async void OnOpenFileCommand()
        {
            if (await TrySave() == MessageBoxResult.Cancel)
                return;

            await OpenFile();
        }

        private bool OnCanCloseFileCommand() => Repository != null;

        private async void OnCloseFileCommand() => await TrySave();

        private bool OnCanSaveFileCommand() => Repository != null;

        private async void OnSaveFileCommand() => await SaveFile();

        private bool OnCanSaveFileAsCommand() => Repository != null;

        private async void OnSaveFileAsCommand() => await SaveFileAs();

        private bool OnCanFileSettingsCommand() => !String.IsNullOrWhiteSpace(Repository?.FilePath);

        private async void OnFileSettingsCommand()
        {
            var context = await dialogService.Show(ViewNames.ChangePasswordDialogView,
                new Confirmation<string> { Title = Captions.ChangePassword });

            if (!context.Confirmed)
                return;

            Repository.SetPassword(context.Value);
        }

        // ReSharper disable once PossibleNullReferenceException
        private void OnExitCommand() => Application.Current.MainWindow.Close();

        #endregion

        #region Edit

        private bool OnCanAddItemCommand() => Repository != null;

        private async void OnAddItemCommand()
        {
            var context = await dialogService.Show(ViewNames.EditEntityDialogView,
                new Confirmation<SavePassItem>(new SavePassItem()) { Title = Captions.AddEntry });

            if (!context.Confirmed)
                return;

            Repository.Add(context.Value);
            SelectedItem = context.Value;
        }

        private bool OnCanEditItemCommand() => SelectedItem != null && Repository != null;

        private async void OnEditItemCommand()
        {
            await dialogService.Show(ViewNames.EditEntityDialogView,
                new Confirmation<SavePassItem>(SelectedItem) { Title = Captions.EditEntry });
        }

        private bool OnCanRemoveItemCommand() => SelectedItem != null && Repository != null;

        private async void OnRemoveItemCommand()
        {
            MessageBoxResult result = await dialogService.ShowMessageBox(
                Messages.DoYouWantToRemoveSelectedEntry,
                Captions.RemoveEntry,
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result != MessageBoxResult.Yes)
                return;

            Repository.Remove(SelectedItem);
            SelectedItem = null;
        }

        #endregion

        #region About

        private async void OnAboutCommand() => await dialogService.ShowMessageBox(
            String.Format(Messages.AboutProgram_Name_Author_Version, Captions.ProgramName, "Doge", "2.0.14.88"),
            Captions.AboutProgram);

        #endregion

        private void OnCopyToClipboardCommand(string value)
        {
            try
            {
                if (!String.IsNullOrWhiteSpace(value))
                    Clipboard.SetText(value);
            }
            catch
            {
                // ignored
            }
        }

        private void OnOpenBrowserCommand(string value)
        {
            try
            {
                if (!String.IsNullOrWhiteSpace(value) && Uri.IsWellFormedUriString(value, UriKind.Absolute))
                    Process.Start(value);
            }
            catch
            {
                // ignored
            }
        }

        #endregion

        #region Private methods

        private async Task OpenFile(OpenFileContext context = null)
        {
            context = await dialogService.Show(ViewNames.OpenFileDialogView,
                context ?? new OpenFileContext
                {
                    Title = Captions.OpenFile,
                    IsSave = false
                });
            if (!context.Confirmed)
                return;

            try
            {
                Repository = SavePassRepository.FromFile(context.FilePath, context.Password);
                InitializeCollectionView();
            }
            catch (IOException ex)
            {
                Repository = null;
                Items = null;

                await dialogService.ShowMessageBox(ex.Message, Captions.Error,
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                await OpenFile(context);
            }
            catch (CryptographicException)
            {
                Repository = null;
                Items = null;

                await dialogService.ShowMessageBox(Captions.WrongPassword, Captions.Error,
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                await OpenFile(context);
            }
        }

        private async Task<bool> SaveFile()
        {
            if (String.IsNullOrWhiteSpace(Repository.FilePath))
                return await SaveFileAs();

            Repository.Save();
            return true;
        }

        private async Task<bool> SaveFileAs()
        {
            var context = await dialogService.Show(ViewNames.OpenFileDialogView,
                new OpenFileContext
                {
                    Title = Captions.SaveAsEllipsis,
                    IsSave = true
                });
            if (!context.Confirmed)
                return false;

            Repository.SetPassword(context.Password);
            Repository.Save(context.FilePath);

            return true;
        }

        private async Task<MessageBoxResult> TrySave()
        {
            if (Repository == null || !Repository.HasChanges)
            {
                Repository = null;
                Items = null;
                return MessageBoxResult.No;
            }

            string fileName = !String.IsNullOrWhiteSpace(Repository.FilePath)
                ? Path.GetFileName(Repository.FilePath)
                : Captions.Untitled;

            MessageBoxResult result = await dialogService.ShowMessageBox(
                String.Format(Messages.DoYouWantToSaveChangesToX, fileName),
                Captions.SaveChangesQuestion,
                MessageBoxButton.YesNoCancel,
                MessageBoxImage.Question);

            switch (result)
            {
                case MessageBoxResult.Yes:
                    if (!await SaveFile())
                        return MessageBoxResult.Cancel;
                    Repository = null;
                    Items = null;
                    break;
                case MessageBoxResult.No:
                    Repository = null;
                    Items = null;
                    break;
            }

            return result;
        }

        private void OnShellClosing(object sender, CancelEventArgs args)
        {
            args.Cancel = true;
            Task.Run(() => OnExit());
        }

        private void OnExit()
        {
            Application.Current.Dispatcher.ExecuteWithCheck(async () =>
            {
                if (await TrySave() == MessageBoxResult.Cancel)
                    return;

                // ReSharper disable once PossibleNullReferenceException
                Application.Current.MainWindow.Closing -= OnShellClosing;
                Application.Current.MainWindow.Close();
            });
        }

        private void InitializeCollectionView()
        {
            Items = new CollectionViewSource { Source = Repository.Items }.View;
            Items.SortDescriptions.Add(new SortDescription(nameof(SavePassItem.Name), ListSortDirection.Ascending));
        }

        #endregion
    }
}