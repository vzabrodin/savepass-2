using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using Prism.Mvvm;
using Zabrodin.SavePass.Extensions;
using Zabrodin.SavePass.Security;

namespace Zabrodin.SavePass.Models
{
    public class SavePassRepository : BindableBase
    {
        private AESWrapper aes;
        private readonly ObservableCollection<SavePassItem> items;
        private bool hasChanges;
        private string filePath;

        private SavePassRepository()
        {
            items = new ObservableCollection<SavePassItem>();
            Items = new ReadOnlyObservableCollection<SavePassItem>(items);
            items.CollectionChanged += OnCollectionChanged;
        }

        private SavePassRepository(string filepath, string password)
        {
            FilePath = filepath;

            aes = new AESWrapper(password);

            byte[] data = File.ReadAllBytes(FilePath);
            string xml = aes.Decrypt(data);

            items = new ObservableCollection<SavePassItem>(xml.FromXml<SavePassFile>().Items);
            Items = new ReadOnlyObservableCollection<SavePassItem>(items);

            items.CollectionChanged += OnCollectionChanged;
            foreach (SavePassItem item in items)
                item.PropertyChanged += OnEntitiesPropertyChanged;
        }

        public bool HasChanges
        {
            get => hasChanges;
            private set => SetProperty(ref hasChanges, value);
        }

        public string FilePath
        {
            get => filePath;
            private set => SetProperty(ref filePath, value);
        }

        public ReadOnlyObservableCollection<SavePassItem> Items { get; }

        public void Add(SavePassItem item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            if (items.Contains(item) || items.Any(e => e.Id == item.Id))
                throw new ArgumentException("Item with same ID is already exists in colleciton");

            item.PropertyChanged += OnEntitiesPropertyChanged;
            items.Add(item);
        }

        public bool Remove(SavePassItem item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            item.PropertyChanged -= OnEntitiesPropertyChanged;
            return items.Remove(item);
        }

        public void Save()
        {
            string xml = new SavePassFile(items.ToArray()).ToXml();
            File.WriteAllBytes(FilePath, aes.Encrypt(xml));
            HasChanges = false;
        }

        public void Save(string filepath)
        {
            FilePath = filepath;
            Save();
        }

        public void SetPassword(string password)
        {
            aes = new AESWrapper(password);
            HasChanges = true;
        }

        public void SaveXml()
        {
            string xml = new SavePassFile(items.ToArray()).ToXml();
            File.WriteAllText(FilePath, xml);
        }

        public void SaveXml(string filepath)
        {
            FilePath = filepath;
            SaveXml();
        }

        private void OnEntitiesPropertyChanged(object sender, PropertyChangedEventArgs e) => HasChanges = true;

        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e) => HasChanges = true;

        public static SavePassRepository New() => new SavePassRepository();

        public static SavePassRepository FromFile(string filepath, string password)
            => new SavePassRepository(filepath, password);
    }
}