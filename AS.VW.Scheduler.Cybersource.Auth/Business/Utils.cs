using AS.VW.Scheduler.Cybersource.Auth.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AS.VW.Scheduler.Cybersource.Auth.Business
{
    public static class Utils
    {
        private static readonly List<SensitiveTag> SensitiveTags = new List<SensitiveTag>
        {
            new SensitiveTag("securityCode", "[0-9]{3,4}", "xxxxx"),
            new SensitiveTag("number", "(\\s*\\p{N}\\s*)+(\\p{N}{4})(\\s*)", "xxxxx$2"),
            new SensitiveTag("cardNumber", "(\\s*\\p{N}\\s*)+(\\p{N}{4})(\\s*)", "xxxxx$2"),
            new SensitiveTag("expirationMonth", "[0-1][0-9]", "xxxx"),
            new SensitiveTag("expirationYear", "2[0-9][0-9][0-9]", "xxxx"),
            new SensitiveTag("account", "(\\s*\\p{N}\\s*)+(\\p{N}{4})(\\s*)", "xxxxx$2"),
            new SensitiveTag("routingNumber", "[0-9]+", "xxxxx"),
            new SensitiveTag("email", "[a-z0-9!#$%&'*+\\/=?^_`{|}~-]+(?:.[a-z0-9!#$%&'*+\\/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?", "xxxxx"),
            new SensitiveTag("firstName", "([a-zA-Z]+( )?[a-zA-Z]*'?-?[a-zA-Z]*( )?([a-zA-Z]*)?)", "xxxxx"),
            new SensitiveTag("lastName", "([a-zA-Z]+( )?[a-zA-Z]*'?-?[a-zA-Z]*( )?([a-zA-Z]*)?)", "xxxxx"),
            new SensitiveTag("phoneNumber", "(\\+[0-9]{1,2} )?\\(?[0-9]{3}\\)?[ .-]?[0-9]{3}[ .-]?[0-9]{4}", "xxxxx"),
            new SensitiveTag("type", "[-A-Za-z0-9 ]+", "xxxxx"),
            new SensitiveTag("token", "[-.A-Za-z0-9 ]+", "xxxxx"),
            new SensitiveTag("signature", "[-.A-Za-z0-9 ]+", "xxxxx"),
            new SensitiveTag("prefix", "(\\s*)(\\p{N}{4})(\\s*)(\\p{N}{2})(\\s*\\p{N}*\\s*)+", "$2$4xxxxx"),
            new SensitiveTag("bin", "(\\s*)(\\p{N}{4})(\\s*)(\\p{N}{2})(\\s*\\p{N}*\\s*)+", "$2$4xxxxx")
        };
        public static Dictionary<string, string> GetSensitiveTags()
        {
            var configTags = new Dictionary<string, string>();                     
            for (int i = 0; i < SensitiveTags.Count; i++)
            {
                string tagName = SensitiveTags[i].tagName;
                string pattern = SensitiveTags[i].pattern;
                string replacement = SensitiveTags[i].replacement;
                pattern = (string.IsNullOrEmpty(pattern) ? ("\\\"" + tagName + "\\\":\\\".+\\\"") : ("\\\"" + tagName + "\\\":\\\"" + pattern + "\\\""));
                replacement = "\"" + tagName + "\":\"" + replacement + "\"";
                configTags.Add(pattern, replacement);
            }

            return configTags;
        }
        public static string MaskSensitiveData(string str)
        {
            try
            {
                var sensitiveTags = GetSensitiveTags();
                foreach (KeyValuePair<string, string> sensitiveTag in sensitiveTags)
                {
                    if (sensitiveTag.Key.StartsWith("\\\"number\\\"") || sensitiveTag.Key.StartsWith("\\\"cardNumber\\\"") || sensitiveTag.Key.StartsWith("\\\"account\\\"") || sensitiveTag.Key.StartsWith("\\\"prefix\\\"") || sensitiveTag.Key.StartsWith("\\\"bin\\\""))
                    {
                        string text = sensitiveTag.Key.Split(':')[0];
                        string text2 = "(((\\s*[s/-]*\\s*)+)\\p{N}((\\s*[s/-]*\\s*)+))+";                        
                        foreach (Match item in Regex.Matches(str, text + ":\\\"" + text2 + "\\\"",  RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(1000)))
                        {
                            string text3 = item.ToString();
                            text3 = text3.Replace(" ", "");
                            text3 = text3.Replace("-", "");
                            str = str.Replace(item.ToString(), text3);
                        }
                    }

                    str = Regex.Replace(str, sensitiveTag.Key, sensitiveTag.Value, RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(1000));
                }
            }
            catch (Exception)
            {
                return string.Empty;
            }

            return str;
        }

        public static string HandleValidation(object obj)
        {
            ModelValidationCollection modelValidation = new ModelValidationCollection();
            bool valid = TryValidateObject(obj, ref modelValidation);

            if (!valid && modelValidation != null && modelValidation.ModelErrors != null)
            {
                var msg = new StringBuilder();
                foreach (var e in modelValidation.ModelErrors)
                {
                    foreach (string s in e.Errors)
                    {
                        msg.Append(s + "\n");
                    }
                }
                return msg.ToString();
            }

            return string.Empty;
        }
        public static bool TryValidateObject(object model, ref ModelValidationCollection results)
        {
            var context = new ValidationContext(model);
            var errors = new List<ValidationResult>();
            var valid = Validator.TryValidateObject(model, context, errors, true);
            if (results == null)
            {
                results = new ModelValidationCollection();
            }
            results.AddRange(errors);
            return valid;
        }
    }
}
