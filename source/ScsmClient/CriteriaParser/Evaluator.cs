using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.EnterpriseManagement.Configuration;
using ScsmClient.CriteriaParser.Binding;
using ScsmClient.Helper;

namespace ScsmClient.CriteriaParser
{
    internal sealed class Evaluator
    {
        private readonly BoundExpression _root;

        private SCSMClient _scsmClient;
        private ManagementPackClass _baseManagementPackClass;
        private ManagementPackTypeProjection _managementPackTypeProjection;
        private ValueConverter ValueConverter { get; }

        private bool _baseIsTypeProjection; 

        private List<string> _references = new List<string>();

        public Evaluator(BoundExpression root, SCSMClient scsmClient)
        {
            _root = root;
            _scsmClient = scsmClient;
            ValueConverter = new ValueConverter(_scsmClient);
        }

        public SimpleXml Evaluate(ManagementPackTypeProjection managementPackTypeProjection)
        {
            _managementPackTypeProjection = managementPackTypeProjection;
            _baseIsTypeProjection = true;
            return Evaluate();
        }


        public SimpleXml Evaluate(ManagementPackClass baseManagementPackClass)
        {
            _baseManagementPackClass = baseManagementPackClass;
            return Evaluate();
        }

        private SimpleXml Evaluate()
        {
            var expressionString = EvaluateExpression(_root);

            var references = String.Join(Environment.NewLine, _references);
            var crit = $"<Criteria xmlns=\"http://Microsoft.EnterpriseManagement.Core.Criteria/\">{references}{expressionString}</Criteria>";
            return SimpleXml.Parse(crit);
        }

        private object EvaluateExpression(BoundExpression node)
        {
            switch (node.Kind)
            {
                case BoundNodeKind.LiteralExpression:
                    return EvaluateLiteralExpression((BoundLiteralExpression)node);
                case BoundNodeKind.NullExpression:
                    return EvaluateNullExpression((BoundNullExpression)node);
                //case BoundNodeKind.VariableExpression:
                //    return this.EvaluateVariableExpression((BoundVariableExpression)node);
                //case BoundNodeKind.AssignmentExpression:
                //    return EvaluateAssignmentExpression((BoundAssignmentExpression)node);
                case BoundNodeKind.UnaryExpression:
                    return EvaluateUnaryExpression((BoundUnaryExpression)node);
                case BoundNodeKind.BinaryExpression:
                    return EvaluateBinaryExpression((BoundBinaryExpression)node);
                default:
                    throw new Exception($"Unexpected node {node.Kind}");
            }
        }

        private static object EvaluateLiteralExpression(BoundLiteralExpression n)
        {
            return n.Value;
        }

        private static object EvaluateNullExpression(BoundNullExpression n)
        {
            return new NullValue();
        }

        private object EvaluateUnaryExpression(BoundUnaryExpression u)
        {
            var operand = EvaluateExpression(u.Operand);

            switch (u.Op.Kind)
            {
                case BoundUnaryOperatorKind.Identity:
                    return (int)operand;
                case BoundUnaryOperatorKind.Negation:
                    return -(int)operand;
                case BoundUnaryOperatorKind.LogicalNegation:
                    return !(bool)operand;
                default:
                    throw new Exception($"Unexpected unary operator {u.Op}");
            }
        }

        private object EvaluateBinaryExpression(BoundBinaryExpression b)
        {
            var left = EvaluateExpression(b.Left);
            var right = EvaluateExpression(b.Right);

            switch (b.Op.Kind)
            {
                //case BoundBinaryOperatorKind.Addition:
                //    return (int)left + (int)right;
                //case BoundBinaryOperatorKind.Subtraction:
                //    return (int)left - (int)right;
                //case BoundBinaryOperatorKind.Multiplication:
                //    return (int)left * (int)right;
                //case BoundBinaryOperatorKind.Division:
                    //return (int)left / (int)right;
                case BoundBinaryOperatorKind.LogicalAnd:
                    return Expression(And(left, right));
                case BoundBinaryOperatorKind.LogicalOr:
                    return Expression(Or(left, right));
                case BoundBinaryOperatorKind.Equals:
                {
                    if (right is NullValue)
                    {
                        return Expression(UnaryExpression(left, "IsNull"));
                    }
                    return Expression(SimpleExpression(left, "Equal", right));
                }

                case BoundBinaryOperatorKind.NotEquals:
                {
                    if (right is NullValue)
                    {
                        return Expression(UnaryExpression(left, "IsNotNull"));
                    }
                    return Expression(SimpleExpression(left, "NotEqual", right));
                }
                    
                case BoundBinaryOperatorKind.Like:
                    return Expression(SimpleExpression(left, "Like", right));
                case BoundBinaryOperatorKind.NotLike:
                    return Expression(SimpleExpression(left, "NotLike", right));
                case BoundBinaryOperatorKind.Greater:
                    return Expression(SimpleExpression(left, "Greater", right));
                case BoundBinaryOperatorKind.Less:
                    return Expression(SimpleExpression(left, "Less", right));
                case BoundBinaryOperatorKind.GreaterOrEquals:
                    return Expression(SimpleExpression(left, "GreaterEqual", right));
                case BoundBinaryOperatorKind.LessOrEquals:
                    return Expression(SimpleExpression(left, "LessEqual", right));
                default:
                    throw new Exception($"Unexpected binary operator {b.Op}");
            }
        }


        private string And(object left, object right)
        {
            return $"<And>{left}{right}</And>";
        }

        private string Or(object left, object right)
        {
            return $"<Or>{left}{right}</Or>";
        }

        private string SimpleExpression(object left, string @operator, object right)
        {

            var propertyName = left.ToString();
            var propertyElement = BuildPropertyElement(propertyName);

            string value = right.ToString();

            if (propertyName.StartsWith("G:", StringComparison.OrdinalIgnoreCase))
            {
                value = ValueConverter.NormalizeGenericValueForCriteria(value, propertyName);
            }
            else
            {
                value = propertyElement.property != null
                    ? ValueConverter.NormalizeValueForCriteria(right, propertyElement.property)
                    : right.ToString();
            }
            

            var expr = $"<SimpleExpression><ValueExpressionLeft>{propertyElement.path}</ValueExpressionLeft><Operator>{@operator}</Operator><ValueExpressionRight><Value>{value}</Value></ValueExpressionRight></SimpleExpression>";
            return expr;
        }

        private string UnaryExpression(object left, string @operator)
        {
            var propertyName = left.ToString();
            var propertyElement = BuildPropertyElement(propertyName);

            return $"<UnaryExpression><ValueExpression>{propertyElement.path}</ValueExpression><Operator>{@operator}</Operator></UnaryExpression>";
        }

        private string Expression(string simpleExpression)
        {
            return $"<Expression>{simpleExpression}</Expression>";
        }


        private (string path, ManagementPackProperty property) BuildPropertyElement(string propertyName)
        {
            if (propertyName.Contains("!"))
            {
                return BuildRelationshipProperty(propertyName);
            }

            return BuildTypeProperty(propertyName);
        }

        private (string path, ManagementPackProperty property) BuildTypeProperty(string propertyName)
        {
            string prefix = "P";
            if (propertyName.Contains(":"))
            {
                var spl = propertyName.Split(':');
                prefix = spl[0];
                propertyName = spl[1];
            }


            switch (prefix.ToUpper())
            {
                case "P":
                {
                    var prop = FindProperty(propertyName);
                    if (prop == null)
                    {
                        var managementPackClass = _baseIsTypeProjection ? _managementPackTypeProjection.TargetType : _baseManagementPackClass;
                        throw new Exception($"Can't find property '{propertyName}' for type '{managementPackClass.Name}'");
                    }
                    var managementPack = prop.GetManagementPack();
                    var mpAlias = $"Alias_{managementPack.Name.Replace('.', '_')}";
                    var refString =
                        $"<Reference Id=\"{managementPack.Name}\" PublicKeyToken=\"{managementPack.KeyToken}\" Version=\"{managementPack.Version}\" Alias=\"{mpAlias}\" />";

                    if (!_references.Contains(refString))
                    {
                        _references.Add(refString);
                    }


                    var value = $"$Context/Property[Type='{mpAlias}!{prop.ParentElement.Name}']/{prop.Name}$";

                    return ($"<Property>{value}</Property>", prop);
                }
                case "G":
                    return ($"<GenericProperty>{propertyName}</GenericProperty>", null);
                default:
                    throw new Exception($"Prefix '{prefix}' not valid!");
            }
        }

        private (string path, ManagementPackProperty property) BuildRelationshipProperty(string propertyName)
        {

            var splitted = propertyName.Split('!');
            var realtionship = splitted[0];
            propertyName = splitted[1];


            var rel =_managementPackTypeProjection.FirstOrDefault(end => end.Value.TargetType.Name == realtionship);

            var managementPack = rel.Key.GetManagementPack();

            var mpAlias = $"Alias_{managementPack.Name.Replace('.', '_')}";
            var refString =
                $"<Reference Id=\"{managementPack.Name}\" PublicKeyToken=\"{managementPack.KeyToken}\" Version=\"{managementPack.Version}\" Alias=\"{mpAlias}\" />";

            if (!_references.Contains(refString))
            {
                _references.Add(refString);
            }

            string seedRole = null;
            if (rel.Value is ManagementPackTypeProjectionComponent managementPackTypeProjectionComponent)
            {
                var regMatch = new Regex(@"SeedRole='(?<seedrole>.+)'", RegexOptions.IgnoreCase).Match(managementPackTypeProjectionComponent.Path);
                if (regMatch.Success)
                {
                    seedRole = $"SeedRole='{regMatch.Groups["seedrole"]?.Value}' ";
                }
                
            }

            var path = $"<Property>$Context/Path[Relationship='{mpAlias}!{rel.Key.ParentElement.Name}' {seedRole}TypeConstraint='{mpAlias}!{realtionship}']/Property[Type='{mpAlias}!{realtionship}']/{propertyName}$</Property>";
            return (path, null);

        }


        private ManagementPackProperty FindProperty(string propertyName, ManagementPackClass managementPackClass = null)
        {
            managementPackClass = managementPackClass ?? (_baseIsTypeProjection ? _managementPackTypeProjection.TargetType : _baseManagementPackClass);

            if (propertyName.Contains("."))
            {
                var splitted = propertyName.Split('.');
                var className = string.Join(".", splitted.Take(splitted.Length - 1));
                propertyName = splitted.Last();
                managementPackClass = FindParentClass(className, managementPackClass);
            }

            
            var prop = managementPackClass.PropertyCollection.FirstOrDefault(p =>
                    p.Name.Equals(propertyName, StringComparison.Ordinal)) ??
                managementPackClass.PropertyCollection.FirstOrDefault(p =>
                    p.Name.Equals(propertyName, StringComparison.OrdinalIgnoreCase));

            if (prop != null)
            {
                return prop;
            }

            var baseClass = managementPackClass.Base?.GetElement();
            if (baseClass != null)
            {
                return FindProperty(propertyName, baseClass);
            }

            return null;
            
            
        }

        private ManagementPackClass FindParentClass(string className, ManagementPackClass currentClass)
        {
            var currentClassName = currentClass.Name;
            if (!currentClassName.Contains('.'))
            {
                currentClassName = $".{currentClassName}";
            }

            if (currentClassName.EndsWith($".{className}", StringComparison.OrdinalIgnoreCase))
            {
                return currentClass;
            }

            var parent = currentClass.Base?.GetElement();
            return parent == null ? null : FindParentClass(className, parent);
        }


        //private string PrepareElementValue(object value, ManagementPackProperty property)
        //{
        //    if (property == null)
        //    {
        //        return value.ToString();
        //    }

        //    switch (property.Type)
        //    {
        //        case ManagementPackEntityPropertyTypes.@enum:
        //        {

        //        }
        //    }
        //}
    }


   
}