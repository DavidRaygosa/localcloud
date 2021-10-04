using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Domain
{
    public class User : IdentityUser{
        public string FullName{get;set;}
        public string Language{get;set;}
        public Boolean isDark{get;set;}
        public Boolean firstLogin{get;set;}
    }
}