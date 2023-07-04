using HumanResource.Contexts;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Security.Principal;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Data;
using Microsoft.CodeAnalysis;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using HumanResource.Models;
using HumanResource.Models.HRMS;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Identity;


namespace HumanResource.Helpers
{
    public static class AuthHelper
    {
        public static string ToUpperFirstLetter(string source)
        {
            if (string.IsNullOrEmpty(source))
                return string.Empty;
            // Convert first letter to upper case.
            char[] letters = source.ToCharArray();
            letters[0] = char.ToUpper(letters[0]);
            return new string(letters);
        }
    }
}
