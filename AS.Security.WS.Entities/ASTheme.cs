using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;

namespace AS.Security.WS.Entities
{
    [Serializable]
    public class ASTheme
    {
        public ASTheme()
        { }

        public ASTheme(int themeId, string themeName, string themeImage, string themeDesc, DateTime dateCreated, string actvStatus)
        {
            ThemeId = themeId;
            ThemeName = themeName;
            ThemeImage = themeImage;
            Description = themeDesc;
            DateCreated = dateCreated;
            ActvStatus = actvStatus;
        }

        public int ThemeId { get; set; }
        public string ThemeName { get; set; }
        public string ThemeImage { get; set; }
        public string Description { get; set; }
        public string ActvStatus { get; set; }
        public DateTime DateCreated { get; set; }
    }

    [Serializable]
    public class ASThemeCollection : CollectionBase
    {

        public ASTheme this[int index]
        {
            get { return ((ASTheme)List[index]); }
            set { List[index] = value; }
        }

        public int Add(ASTheme value)
        {
            return (List.Add(value));
        }

        public int IndexOf(ASTheme value)
        {
            return (List.IndexOf(value));
        }

        public void Insert(int index, ASTheme value)
        {
            List.Insert(index, value);
        }

        public void Remove(ASTheme value)
        {
            List.Remove(value);
        }

        public bool Contains(ASTheme value)
        {
            // If value is not of type SecHierarchy, this will return false.
            return (List.Contains(value));
        }

    }
}
