using Store_TechniaclTask.Services.Resources;
using Store_TechniaclTask.Services.ViewModels.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using Microsoft.Net.Http.Headers;
using Store_TechniaclTask.DAL.Helper;
using System.IO;
using System.Security.Cryptography;
using Store_TechniaclTask.DAL.Model.Identity;

namespace Store_TechniaclTask.Services.HelperServices
{
   
    public static class ExtentionMethods
    {
        private const string RequestedWithHeader = "X-Requested-With";
        private const string XmlHttpRequest = "XMLHttpRequest";
        static public LocService locService
        {
            set
            {
                _locService = value;
            }
        }
        static LocService _locService;

        public static IEnumerable<T> DistinctBy<T, TKey>(this IEnumerable<T> enumerable, Func<T, TKey> keySelector)
        {
            return enumerable.GroupBy(keySelector).Select(grp => grp.First());
        }
        public static Expression<Func<T, bool>> AndAlso<T>(
    this Expression<Func<T, bool>> left,
    Expression<Func<T, bool>> right)
        {
            var param = Expression.Parameter(typeof(T), "x");
            var body = Expression.AndAlso(
                    Expression.Invoke(left, param),
                    Expression.Invoke(right, param)
                );
            var lambda = Expression.Lambda<Func<T, bool>>(body, param);
            return lambda;
        }

        public static IEnumerable<TreeItem<T>> GenerateTree<T, K>(
         this IEnumerable<T> collection,
         Func<T, K> id_selector,
         Func<T, K> parent_id_selector,
         K root_id = default(K))
        {
            foreach (var c in collection.Where(c => EqualityComparer<K>.Default.Equals(parent_id_selector(c), root_id)))
            {
                yield return new TreeItem<T>
                {
                    Item = c,
                    Children = collection.GenerateTree(id_selector, parent_id_selector, id_selector(c))
                };
            }
        }

        // public static IEnumerable<TreeItem<T>> GenerateTree<T, K>(
        //this IEnumerable<T> collection,
        //Func<T, K> id_selector,
        //Func<T, K> parent_id_selector,
        //K root_id = default(K))
        // {
        //     foreach (var c in collection.Where(c => EqualityComparer<K>.Default.Equals(parent_id_selector(c), root_id)))
        //     {
        //         yield return new TreeItem<T>
        //         {
        //             Item = c,
        //             Children = collection.GenerateTree(id_selector, parent_id_selector, id_selector(c))
        //         };
        //     }
        // }
        public static Dictionary<string, string> GetModelStateErrors(this ModelStateDictionary modelState)
        {
            var ErrorsList = new Dictionary<string, string>();

            for (int i = 0; i < modelState.Keys.ToList().Count(); i++)
            {
                if (modelState.Values.ToArray()[i].Errors.Any())
                {
                    var ErrorMessage = modelState.Values.ToArray()[i].Errors.FirstOrDefault()?.ErrorMessage;
                    ErrorsList.Add(modelState.Keys.ToArray()[i], ErrorMessage);
                }
            }
            return ErrorsList;
        }
        public static string Encrypt(this string clearText)
        {
            const string encryptionKey = "MAKV2SPBNI99212";
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (var encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(encryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (var ms = new MemoryStream())
                {
                    using (var cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }

        public static string Decrypt(this string cipherText)
        {
            const string encryptionKey = "MAKV2SPBNI99212";
            cipherText = cipherText.Replace(" ", "+");
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (var encryptor = Aes.Create())
            {
                var pdb = new Rfc2898DeriveBytes(encryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (var ms = new MemoryStream())
                {
                    using (var cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();

                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }
        public static string CreateUniqRequestNumber()
        {
            var TimeNow = DateTime.UtcNow;

            //return (TimeNow.Year + TimeNow.Month + TimeNow.Day + TimeNow.Hour + TimeNow.Minute + TimeNow.Second + TimeNow.Millisecond).ToString();
            return TimeNow.ToString("yyMMddHHmmssff");
        }   

        public static Predicate<T> Or<T>(params Predicate<T>[] predicates)
        {
            return delegate (T item)
            {
                foreach (Predicate<T> predicate in predicates)
                {
                    if (predicate(item))
                    {
                        return true;
                    }
                }
                return false;
            };
        }
        
        public static Predicate<T> And<T>(params Predicate<T>[] predicates)
        {
            return delegate (T item)
            {
                foreach (Predicate<T> predicate in predicates)
                {
                    if (!predicate(item))
                    {
                        return false;
                    }
                }
                return true;
            };
        }
        public static bool IsAjaxRequest(this HttpRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException("Null request");
            }

            if (request.Headers != null)
            {
                return request.Headers[RequestedWithHeader] == XmlHttpRequest;
            }

            return false;
        }  
        public static bool IsWebApiRequest(this HttpRequest request)
        {
            string authorization = request.Headers[HeaderNames.Authorization];
            return !string.IsNullOrEmpty(authorization) && authorization.StartsWith("Bearer ");
        }
        public static T CloneObj<T>(this T source) where T : class
        {
            // Don't serialize a null object, simply return the default for that object
            if (ReferenceEquals(source, null)) return null;

            // initialize inner objects individually
            // for example in default constructor some list property initialized with some values,
            // but in 'source' these items are cleaned -
            // without ObjectCreationHandling.Replace default constructor values will be added to result
            var deserializeSettings = new JsonSerializerSettings { ObjectCreationHandling = ObjectCreationHandling.Replace };

            return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(source), deserializeSettings);
        }
        public static IEnumerable<T> SelectManyRecursive<T>(this IEnumerable<T> source, Func<T, IEnumerable<T>> selector)
        {
            var result = source.SelectMany(selector);
            if (!result.Any())
            {
                return result;
            }
            return result.Concat(result.SelectManyRecursive(selector));
        }

        public static (int Years, int Monthes, int Days) CalculateAge(this DateTime Dob, DateTime? TillDate = null)
        {
            //DateTime Now = DateTime.Now;
            var Now = TillDate ?? DateTime.Now;
            if (Dob > Now)
            {
                return (-1, -1, -1);
            }
            int Years = new DateTime(DateTime.Now.Subtract(Dob).Ticks).Year - 1;
            DateTime PastYearDate = Dob.AddYears(Years);
            int Months = 0;
            for (int i = 1; i <= 12; i++)
            {
                if (PastYearDate.AddMonths(i) == Now)
                {
                    Months = i;
                    break;
                }
                else if (PastYearDate.AddMonths(i) >= Now)
                {
                    Months = i - 1;
                    break;
                }
            }
            int Days = Now.Subtract(PastYearDate.AddMonths(Months)).Days;
            //int Hours = Now.Subtract(PastYearDate).Hours;
            //int Minutes = Now.Subtract(PastYearDate).Minutes;
            //int Seconds = Now.Subtract(PastYearDate).Seconds;
            return (Years, Months, Days);
        }
        public static Type GetUnderlyingType(this MemberInfo member)
        {
            switch (member.MemberType)
            {
                case MemberTypes.Event:
                    return ((EventInfo)member).EventHandlerType;
                case MemberTypes.Field:
                    return ((FieldInfo)member).FieldType;
                case MemberTypes.Method:
                    return ((MethodInfo)member).ReturnType;
                case MemberTypes.Property:
                    return ((PropertyInfo)member).PropertyType;
                default:
                    throw new ArgumentException
                    (
                       "Input MemberInfo must be if type EventInfo, FieldInfo, MethodInfo, or PropertyInfo"
                    );
            }
        }
        public static bool IsSimpleType(this Type type)
        {
            return
               type.IsValueType ||
               type.IsPrimitive ||
               new[]
               {
               typeof(String),
               typeof(Decimal),
               typeof(DateTime),
               typeof(DateTimeOffset),
               typeof(TimeSpan),
               typeof(Guid)
               }.Contains(type) ||
               (Convert.GetTypeCode(type) != TypeCode.Object);
        }
        public static bool PublicInstancePropertiesEqual<T>(this T self, T to, params string[] ignore) where T : class
        {
            if (self != null && to != null)
            {
                var type = typeof(T);
                var ignoreList = new List<string>(ignore);
                var unequalProperties =
                    from pi in type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    where !ignoreList.Contains(pi.Name) && pi.GetUnderlyingType().IsSimpleType() && pi.GetIndexParameters().Length == 0
                    let selfValue = type.GetProperty(pi.Name).GetValue(self, null)
                    let toValue = type.GetProperty(pi.Name).GetValue(to, null)
                    where selfValue != toValue && (selfValue == null || !selfValue.Equals(toValue))
                    select selfValue;
                return !unequalProperties.Any();
            }
            return self == to;
        }

        public static string GetDisplayName(this PropertyInfo propertyInfo)
        {
            var DisplayName = propertyInfo.GetCustomAttribute<DisplayAttribute>()?.GetName() ?? propertyInfo.Name;
            return _locService.GetLocalizedHtmlString(DisplayName);
        }  
        public static string GetDisplayName(string texy)
        {
            return _locService.GetLocalizedHtmlString(texy);
        }
        public static string GetUserID(this ClaimsPrincipal User)
        {
            var UserID = User?.FindFirstValue(ClaimTypes.NameIdentifier);
            return UserID;
        }
        public static string GetUserName(this ClaimsPrincipal User)
        {
            var UserID = User?.FindFirstValue(ClaimTypes.Name);
            return UserID;
        }
        public static string GetUserFullName(this ClaimsPrincipal User)
        {
            var UserID = User?.FindFirstValue(ClaimTypes.Name);
            return UserID;
        } 
        public static string GetUserProfileImageUrl(this ClaimsPrincipal User)
        {
            var ProfileImage = User?.FindFirstValue(nameof(ApplicationUser.ProfileImage));
            return string.IsNullOrEmpty(ProfileImage) ?"":AttachmentOnserver.UsersAttachment+ ProfileImage;
        }    
    
        public static string GetUserRoleName(this ClaimsPrincipal User)
        {
            var Group = User?.FindFirstValue(ClaimTypes.Role);
            return Group;
        } 
        public static int GetCurrentUserClinicID(this ClaimsPrincipal User)
        {
            var ClinicIDStr = User?.FindFirstValue("ClinicID");
            int.TryParse(ClinicIDStr, out int ClinicID);
            return ClinicID;
        }
        public static string GetUserEmail(this ClaimsPrincipal User)
        {
            //return "";
            var email = User?.FindFirstValue(ClaimTypes.Email);
            return email;
        }
        public static string GetDisplayName(this Enum enumValue)
        {
            try
            {
                var DisplayName = enumValue.GetType()
                    .GetMember(enumValue.ToString())
                    .First()
                    .GetCustomAttribute<DisplayAttribute>()
                    .GetName();
                DisplayName = _locService.GetLocalizedHtmlString(DisplayName);
                return DisplayName;
            }
            catch
            {
                return "";
            }
        }
    }
}
