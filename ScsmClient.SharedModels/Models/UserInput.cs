using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml.XPath;

namespace ScsmClient.SharedModels.Models
{
    public class UserInput: Dictionary<string, UserInputValue>
    {


        public string ToXml()
        {
            var strbuilder = new StringBuilder();
            strbuilder.AppendLine("<UserInputs>");

            foreach (var key in this.Keys)
            {
                var val = this[key];
                strbuilder.AppendLine(
                    $"<UserInput Question=\"{escapeToXml(key)}\" Answer=\"{escapeToXml(val.Value)}\" Type=\"{val.Type}\" />");
            }

            strbuilder.AppendLine("</UserInputs>");

            var xml = XDocument.Parse(strbuilder.ToString());
            return xml.ToString();
        }
        
        private string escapeToXml(string text)
        {
            return new XElement("t", text).LastNode.ToString();
        }

        public static UserInput FromXml(string xmlString)
        {
            var usrI = new UserInput();
            var xml = XDocument.Parse(xmlString);
            var uis = xml.XPathSelectElements("//UserInput").ToList();



            foreach (var xElement in uis)
            {
                var question = xElement.Attribute("Question")?.Value;
                var answer = xElement.Attribute("Answer")?.Value;
                var type = xElement.Attribute("Type")?.Value;

                usrI[question] = new UserInputValue(answer, type);
            }
            return usrI;
        }
    }

    public class UserInputValue
    {
        public string Value { get; set; }
        public string Type { get; set; }

        public UserInputValue() { }
        public UserInputValue(string value, string type)
        {
            Value = value;
            Type = type;
        }

        public UserInputValue(object value)
        {

            switch (value)
            {
                case int _int:
                    {
                        Value = _int.ToString();
                        Type = "int";
                        break;
                    }
                case decimal _dec:
                    {
                        Value = _dec.ToString();
                        Type = "decimal";
                        break;
                    }
                case double _dbl:
                    {
                        Value = _dbl.ToString();
                        Type = "double";
                        break;
                    }
                case string str:
                    {
                        Value = str;
                        Type = "string";
                        break;
                    }

                case DateTime _dt:
                    {
                        Value = _dt.ToString("yyyy-MM-ddTHH:mm:ss.FFFFF");
                        Type = "datetime";
                        break;
                    }
                case Guid _guid:
                    {
                        Value = _guid.ToString();
                        Type = "guid";
                        break;
                    }
                case bool _bool:
                    {
                        Value = _bool.ToString();
                        Type = "bool";
                        break;
                    }
            }
        }

        public static implicit operator UserInputValue(int value)
        {
            return new UserInputValue(value);
        }

        public static implicit operator UserInputValue(decimal value)
        {
            return new UserInputValue(value);
        }

        public static implicit operator UserInputValue(double value)
        {
            return new UserInputValue(value);
        }

        public static implicit operator UserInputValue(string value)
        {
            return new UserInputValue(value);
        }

        public static implicit operator UserInputValue(DateTime value)
        {
            return new UserInputValue(value);
        }

        public static implicit operator UserInputValue(Guid value)
        {
            return new UserInputValue(value);
        }

        public static implicit operator UserInputValue(bool value)
        {
            return new UserInputValue(value);
        }
    }
}
