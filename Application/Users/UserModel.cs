using System;
namespace Application.Users
{
    public class UserData
    {
        public string ID{get;set;}
        public string FullName{get;set;}
        public string Token{get;set;}
        public string TokenEncrypted{get;set;}
        public string Email{get;set;}
        public string Username{get;set;}
        public string Language{get;set;}
        public Boolean isDark{get;set;}
        public Image Image{get;set;}
        public Boolean firstLogin{get;set;}
    }
}