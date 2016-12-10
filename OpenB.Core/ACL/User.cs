using System;
using System.Collections.Generic;

namespace OpenB.Core.ACL
{
    public class User
    {
        public User(string userName, UserGroup primairyUserGroup)
        {
            if (string.IsNullOrEmpty(userName))
            {
                throw new ArgumentNullException(nameof(userName));
            }

            UserGroups = new List<UserGroup>();
            UserGroups.Add(primairyUserGroup);
        }

        public string UserName { get; private set; }

        public List<UserGroup> UserGroups { get; private set; }

        public override string ToString()
        {
            return string.Format("Username: {0}", UserName);
        }
    }

    public class UserGroup
    {
        public UserGroup(string key, string name, string description)
        {
            Description = description;
            Key = key;
            Name = name;
        }

        public string Name { get; private set; }
        public string Key { get; private set; }
        public string Description { get; private set; }

        public override bool Equals(object obj)
        {
            if (obj is UserGroup && (obj as UserGroup).Key == this.Key)
            {
                return true;
            }
            return false;
        }

        public override string ToString()
        {
            return string.Format("Key: {0} | Name: {1}", Key, Name);
        }
    }
    
   
}