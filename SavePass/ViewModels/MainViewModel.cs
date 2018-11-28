using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using Zabrodin.SavePass.DialogService;
using Zabrodin.SavePass.Extensions;
using Zabrodin.SavePass.Models;
using Zabrodin.SavePass.Properties;
using Zabrodin.SavePass.ViewModels.Contexts;
using IDialogService = Zabrodin.SavePass.DialogService.IDialogService;

namespace Zabrodin.SavePass.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        #region Private members

        private readonly IDialogService dialogService;

        #endregion

        #region Constructor

        public MainViewModel(IDialogService dialogService)
        {
            this.dialogService = dialogService;

            // ReSharper disable once PossibleNullReferenceException
            Application.Current.MainWindow.Closing += OnShellClosing;
        }

        #endregion

        #region Properties

        public ICollectionView Items
        {
            get => GetProperty(() => Items);
            set => SetProperty(() => Items, value);
        }

        public SavePassRepository Repository
        {
            get => GetProperty(() => Repository);
            private set => SetProperty(() => Repository, value);
        }

        public SavePassItem SelectedItem
        {
            get => GetProperty(() => SelectedItem);
            set => SetProperty(() => SelectedItem, value);
        }

        #endregion

        #region Command handlers

        #region File

        [Command]
        public async void NewFile()
        {
            if (await TrySave() == MessageBoxResult.Cancel)
                return;

            Repository = SavePassRepository.New();
            InitializeCollectionView();
        }
        
        [Command]
        public async void OpenFile()
        {
            if (await TrySave() == MessageBoxResult.Cancel)
                return;

            await OpenFileInternal();
        }

        public bool CanCloseFile() => Repository != null;

        [Command]
        public async void CloseFile() => await TrySave();

        public bool CanSaveFile() => Repository != null;

        [Command]
        public async void SaveFile() => await SaveFileInternal();

        public bool CanSaveFileAs() => Repository != null;

        [Command]
        public async void SaveFileAsC() => await SaveFileAs();

        public bool CanFileSettings() => !String.IsNullOrWhiteSpace(Repository?.FilePath);

        [Command]
        public async void FileSettings()
        {
            var context = await dialogService.Show(ViewNames.ChangePasswordDialogView,
                new Confirmation<string> { Title = Captions.ChangePassword });

            if (!context.Confirmed)
                return;

            Repository.SetPassword(context.Value);
        }

        // ReSharper disable once PossibleNullReferenceException
        [Command]
        public void OnExitCommand() => Application.Current.MainWindow.Close();

        #endregion

        #region Edit

        public bool CanAddItem() => Repository != null;

        [Command]
        public async void AddItem()
        {
            var context = await dialogService.Show(ViewNames.EditEntityDialogView,
                new Confirmation<SavePassItem>(new SavePassItem()) { Title = Captions.AddEntry });

            if (!context.Confirmed)
                return;

            Repository.Add(context.Value);
            SelectedItem = context.Value;
        }

        public bool CanEditItem() => SelectedItem != null && Repository != null;

        [Command]
        public async void EditItem()
        {
            await dialogService.Show(ViewNames.EditEntityDialogView,
                new Confirmation<SavePassItem>(SelectedItem) { Title = Captions.EditEntry });
        }

        public bool CanRemoveItem() => SelectedItem != null && Repository != null;

        [Command]
        public async void RemoveItem()
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

        [Command]
        public async void About() => await dialogService.ShowMessageBox(
            String.Format(Messages.AboutProgram_Name_Author_Version, Captions.ProgramName, "Doge", "2.0.14.88"),
            Captions.AboutProgram);

        #endregion

        [Command]
        public void CopyToClipboard(string value)
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

        [Command]
        public void OpenBrowser(string value)
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

        private async Task OpenFileInternal(OpenFileContext context = null)
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
                await OpenFileInternal(context);
            }
            catch (CryptographicException)
            {
                Repository = null;
                Items = null;

                await dialogService.ShowMessageBox(Captions.WrongPassword, Captions.Error,
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                await OpenFileInternal(context);
            }
        }

        private async Task<bool> SaveFileInternal()
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
                    if (!await SaveFileInternal())
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