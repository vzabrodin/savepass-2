using System;
using System.Xml.Serialization;
using Prism.Mvvm;

namespace Zabrodin.SavePass.Models
{
    public class SavePassItem : BindableBase
    {
        private string name;
        private string userName;
        private string password;
        private string email;
        private string webSite;
        private string notes;

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
            get => name;
            set => SetProperty(ref name, value);
        }

        [XmlAttribute]
        public string UserName
        {
            get => userName;
            set => SetProperty(ref userName, value);
        }

        [XmlAttribute]
        public string Password
        {
            get => password;
            set => SetProperty(ref password, value);
        }

        [XmlAttribute]
        public string Email
        {
            get => email;
            set => SetProperty(ref email, value);
        }

        [XmlAttribute]
        public string WebSite
        {
            get => webSite;
            set => SetProperty(ref webSite, value);
        }

        [XmlAttribute]
        public string Notes
        {
            get => notes;
            set => SetProperty(ref notes, value);
        }

        public bool ShouldSerializeName() => !String.IsNullOrWhiteSpace(Name);

        public bool ShouldSerializeUserName() => !String.IsNullOrWhiteSpace(Name);

        public bool ShouldSerializePassword() => !String.IsNullOrWhiteSpace(UserName);

        public bool ShouldSerializeEmail() => !String.IsNullOrWhiteSpace(Email);

        public bool ShouldSerializeWebSite() => !String.IsNullOrWhiteSpace(WebSite);

        public bool ShouldSerializeNotes() => !String.IsNullOrWhiteSpace(Notes);
    }
}
