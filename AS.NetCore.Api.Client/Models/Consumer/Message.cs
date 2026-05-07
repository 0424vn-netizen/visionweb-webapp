using AS.NetCore.Api.Client.Enums;
using System;
using System.ComponentModel;

namespace AS.NetCore.Api.Client.Models
{
    public class Message : DataModel
    {   /// <summary>
        /// Code
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// Type
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// FieldName has error
        /// </summary>
        public string FieldName { get; set; }

        public dynamic Data { get; set; }

        /// <summary>
        /// Success Message build
        /// </summary>
        public static Message Success()
        {
            return new Message()
            {
                Code = (int)StatusCode.Success,
                Description = "Success",
                Type = "S"
            };
        }

        /// <summary>
        /// Error Message build
        /// </summary>
        public static Message Error(string message = null, string fieldName = null)
        {
            return new Message()
            {
                Code = (int)StatusCode.Fail,
                Description = message ?? "Error",
                Type = "F",
                FieldName = fieldName
            };
        }

        public static Message Error(StatusCode code, string message = null, string fieldName = null)
        {
            DescriptionAttribute attribute = Attribute.GetCustomAttribute(code.GetType().GetField(code.ToString()), typeof(DescriptionAttribute)) as DescriptionAttribute;
            if (message == null)
            {
                message = attribute?.Description;
            }

            return new Message()
            {
                Code = (int)code,
                Description = message ?? "Error",
                Type = "F",
                FieldName = fieldName
            };
        }

        public bool IsSuccess()
        {
            return (this.Code == (int)StatusCode.Success);
        }
    }
    public class DataAssociatedWithOtherData
    {
        public int Count { get; set; }

        public int AppGrpsDecTypesDecMersAssociateCount { get; set; }

        public int UsersAssociateCount { get; set; }
    }
}
