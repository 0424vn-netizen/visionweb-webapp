using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.VW.Scheduler.Cybersource.Auth.Model
{
    public class ModelValidationCollection
    {
        public ICollection<ModelValidation> ModelErrors { get; set; }
        public ModelValidationCollection()
        {
            this.ModelErrors = new List<ModelValidation>();
        }
        public bool IsValid
        {
            get { return !ModelErrors.Any(); }
        }
        public int Count
        {
            get { return this.ModelErrors.Count; }
        }
        public bool Remove(string key)
        {
            if (ModelErrors.Any(p => p.Key == key))
            {
                ModelErrors = ModelErrors.Where(p => p.Key != key).ToList();
                return true;
            }
            return false;
        }
        public void Clear()
        {
            this.ModelErrors.Clear();
        }
        public ModelValidationCollection Add(ValidationResult error)
        {
            if (this.ModelErrors == null)
                this.ModelErrors = new List<ModelValidation>();
            var modelValidations = this.ModelErrors.Where(p => error.MemberNames.Contains(p.Key));
            if (modelValidations != null && modelValidations.Any())
            {
                modelValidations.ToList().ForEach(x => {
                    if (!x.Errors.Contains(error.ErrorMessage))
                        x.Errors.Add(error.ErrorMessage);
                });
            }
            else
            {
                if (error.MemberNames != null && error.MemberNames.Any())
                {
                    foreach (var member in error.MemberNames)
                    {
                        this.ModelErrors.Add(new ModelValidation { Key = member, Errors = new List<string> { error.ErrorMessage } });
                    }
                }
                else
                {
                    this.ModelErrors.Add(new ModelValidation { Errors = new List<string> { error.ErrorMessage } });
                }
            }
            return this;
        }
        public ModelValidationCollection AddCollection(ModelValidationCollection errorCollection)
        {
            if (errorCollection == null || errorCollection.Count <= 0)
                return this;
            if (this.ModelErrors == null)
                this.ModelErrors = new List<ModelValidation>();
            for (int i = 0; i < errorCollection.Count; i++)
            {
                this.ModelErrors.Add(errorCollection.ModelErrors.ElementAt(i));
            }
            return this;
        }
        public ModelValidationCollection AddRange(List<ValidationResult> errors)
        {
            if (errors == null || errors.Count <= 0)
                return this;
            for (int i = 0; i < errors.Count; i++)
            {
                this.Add(errors[i]);
            }
            return this;
        }
        public ModelValidation this[string key]
        {
            get { return ModelErrors.FirstOrDefault(p => p.Key == key); }
        }
    }
    public class ModelValidation
    {
        public string Key { get; set; }
        public List<string> Errors { get; set; }
    }
}
