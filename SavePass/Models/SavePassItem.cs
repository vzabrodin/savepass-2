using System;
using System.Xml.Serialization;
using DevExpress.Mvvm;

namespace Zabrodin.SavePass.Models
{
    public class SavePassItem : BindableBase
    {
        public SavePassItem()
            : this(Guid.NewGuid())
        {
        }

        public SavePassItem(Guid id)
        {
            Id = id;
        }

        [XmlAttribute]
        public Guid Id { get; }

        [XmlAttribute]
        public string Name
        {
            get => GetProperty(() => Name);
            set => SetProperty(() => Name, value);
        }

        [XmlAttribute]
        public string UserName
        {
            get => GetProperty(() => UserName);
            set => SetProperty(() => UserName, value);
        }

        [XmlAttribute]
        public string Password
        {
            get => GetProperty(() => Password);
            set => SetProperty(() => Password, value);
        }

        [XmlAttribute]
        public string Email
        {
            get => GetProperty(() => Email);
            set => SetProperty(() => Email, value);
        }

        [XmlAttribute]
        public string WebSite
        {
            get => GetProperty(() => WebSite);
            set => SetProperty(() => WebSite, value);
        }

        [XmlAttribute]
        public string Notes
        {
            get => GetProperty(() => Notes);
            set => SetProperty(() => Notes, value);
        }

        public bool ShouldSerializeName() => !String.IsNullOrWhiteSpace(Name);

        public bool ShouldSerializeUserName() => !String.IsNullOrWhiteSpace(Name);

        public bool ShouldSerializePassword() => !String.IsNullOrWhiteSpace(UserName);

        public bool ShouldSerializeEmail() => !String.IsNullOrWhiteSpace(Email);

        public bool ShouldSerializeWebSite() => !String.IsNullOrWhiteSpace(WebSite);

        public bool ShouldSerializeNotes() => !String.IsNullOrWhiteSpace(Notes);
    }
}
