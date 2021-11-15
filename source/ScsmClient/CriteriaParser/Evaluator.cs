using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using doob.Reflectensions.Common;
using Microsoft.EnterpriseManagement;
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

        private Dictionary<string, (string alias, string reference)> _references = new Dictionary<string, (string alias, string reference)>();

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

            var references = String.Join(Environment.NewLine, _references.Select(r => r.Value.reference));
            var crit = $"<Criteria xmlns=\"http://Microsoft.EnterpriseManagement.Core.Criteria/\">{references}{expressionString}</Criteria>";
            return SimpleXml.Parse(crit);
        }

        private object EvaluateExpression(BoundExpression node)
        {
            switch (node.Kind)
            {
                case BoundNodeKind.LiteralExpression:
                    return EvaluateLiteralExpression((BoundLiteralExpression)node);
                case BoundNodeKind.PropertyExpression:
                    return EvaluatePropertyExpression((BoundPropertyExpression)node);
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

        private EvaluatorPropertyInfo EvaluatePropertyExpression(BoundPropertyExpression n)
        {
            var propertyName = n.Value.ToString().Substring(1);
            var propertyElement = BuildPropertyElement(propertyName);
            return propertyElement;
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

            EvaluatorPropertyInfo leftPropertyInfo = left is EvaluatorPropertyInfo lpi ? lpi : new EvaluatorPropertyInfo(left.ToString());
            EvaluatorPropertyInfo rightPropertyInfo = right is EvaluatorPropertyInfo rpi ? rpi : new EvaluatorPropertyInfo(right.ToString());


            string leftExpression = String.Empty;
            switch (leftPropertyInfo.Type)
            {
                case PropertyInfoType.Value:
                    {
                        throw new Exception("The Left Side of an Expression has to be a Property or a GenericProperty!");
                    }
                case PropertyInfoType.GenericProperty:
                    {
                        leftExpression = leftPropertyInfo.Path;
                        break;
                    }
                case PropertyInfoType.Property:
                    {
                        leftExpression = leftPropertyInfo.Path;
                        break;
                    }
            }


            string rightExpression = String.Empty;
            switch (rightPropertyInfo.Type)
            {
                case PropertyInfoType.Value:
                    {
                        switch (leftPropertyInfo.Type)
                        {
                            case PropertyInfoType.Property:
                                {
                                    rightExpression = ValueConverter.NormalizeValueForCriteria(rightPropertyInfo.Path, leftPropertyInfo.Property);
                                    break;
                                }
                            case PropertyInfoType.GenericProperty:
                                {
                                    rightExpression = ValueConverter.NormalizeGenericValueForCriteria(rightPropertyInfo.Path, leftPropertyInfo.GenericProperty);
                                    break;
                                }
                            case PropertyInfoType.Value:
                                {
                                    rightExpression = rightPropertyInfo.Path;
                                    break;
                                }
                        }

                        rightExpression = $"<Value>{rightExpression}</Value>";
                        break;
                    }
                case PropertyInfoType.GenericProperty:
                    {
                        rightExpression = rightPropertyInfo.Path;
                        break;
                    }
                case PropertyInfoType.Property:
                    {
                        rightExpression = rightPropertyInfo.Path;
                        break;
                    }
            }


            var expression = $"<SimpleExpression><ValueExpressionLeft>{leftExpression}</ValueExpressionLeft><Operator>{@operator}</Operator><ValueExpressionRight>{rightExpression}</ValueExpressionRight></SimpleExpression>";

            return expression;
            //string value = right.ToString();
            //if (value.StartsWith("@"))
            //{
            //    value = value.Substring(1);
            //    value = BuildPropertyElement(value).Path;
            //    var expr = $"<SimpleExpression><ValueExpressionLeft>{leftExpression}</ValueExpressionLeft><Operator>{@operator}</Operator><ValueExpressionRight>{value}</ValueExpressionRight></SimpleExpression>";
            //    return expr;
            //}
            //else
            //{
            //    if (propertyName.StartsWith("G:", StringComparison.OrdinalIgnoreCase))
            //    {
            //        value = ValueConverter.NormalizeGenericValueForCriteria(value, propertyName);
            //    }
            //    else
            //    {
            //        value = propertyElement.Property != null
            //            ? ValueConverter.NormalizeValueForCriteria(right, propertyElement.Property)
            //            : right.ToString();
            //    }
            //    var expr = $"<SimpleExpression><ValueExpressionLeft>{propertyElement.Path}</ValueExpressionLeft><Operator>{@operator}</Operator><ValueExpressionRight><Value>{value}</Value></ValueExpressionRight></SimpleExpression>";
            //    return expr;
            //}


        }



        private string UnaryExpression(object left, string @operator)
        {
            EvaluatorPropertyInfo leftPropertyInfo = left is EvaluatorPropertyInfo lpi ? lpi : new EvaluatorPropertyInfo(left.ToString());


            string leftExpression = String.Empty;
            switch (leftPropertyInfo.Type)
            {
                case PropertyInfoType.Value:
                    {

                        throw new Exception("The Left Side of an Expression has to be a Property or a GenericProperty!");
                    }
                case PropertyInfoType.GenericProperty:
                    {
                        leftExpression = leftPropertyInfo.Path;
                        break;
                    }
                case PropertyInfoType.Property:
                    {
                        leftExpression = leftPropertyInfo.Path;
                        break;
                    }
            }

            //var propertyName = left.ToString();
            //if (propertyName.StartsWith("@"))
            //{
            //    propertyName = propertyName.Substring(1);
            //}
            //var propertyElement = BuildPropertyElement(propertyName);

            return $"<UnaryExpression><ValueExpression>{leftExpression}</ValueExpression><Operator>{@operator}</Operator></UnaryExpression>";
        }

        private string Expression(string simpleExpression)
        {
            return $"<Expression>{simpleExpression}</Expression>";
        }

        private EvaluatorPropertyInfo BuildPropertyElement(string propertyName)
        {
            var regMatch = new Regex(@"^((?<type>[G|P]):)?(?<class>.+!)?(?<property>.+)?$", RegexOptions.IgnoreCase).Match(propertyName);
            if (!regMatch.Success)
            {
                throw new Exception("Can't parse PropertyName!");
            }
            
            var type = regMatch.Groups["type"]?.Value?.ToNull()?.ToUpper() ?? "";
            var pClass = regMatch.Groups["class"]?.Value;
            var property = regMatch.Groups["property"].Value;

            ManagementPackClass managementPackClass = (_baseIsTypeProjection ? _managementPackTypeProjection.TargetType : _baseManagementPackClass);

            switch (type)
            {
                case "":
                //{
                //    StringBuilder propertyElement = new StringBuilder();
                //    propertyElement.Append("<Property>$Context");

                //    if (!String.IsNullOrWhiteSpace(pClass))
                //    {
                //        var projectionPart = BuildProjectionPathPart(pClass);
                //        propertyElement.Append(projectionPart.path);
                //        managementPackClass = projectionPart.properyClass;
                //    }

                //    var propertyPart = BuildPropertyPathPart(property, managementPackClass);
                //    if (propertyPart != null)
                //    {
                //        propertyElement.Append(propertyPart.Path);
                //        propertyElement.Append("</Property>");
                //        return new EvaluatorPropertyInfo(propertyElement.ToString(), propertyPart.Property);
                //    }

                //    var prop = GenericProperty.GetGenericProperties().FirstOrDefault(p => p.PropertyName.Equals(property, StringComparison.OrdinalIgnoreCase));
                //    if (prop != null)
                //    {
                //        return new EvaluatorPropertyInfo($"<GenericProperty>{property}</GenericProperty>", prop);
                //    }

                //    throw new Exception($"Can't find property '{property}' for type '{managementPackClass?.Name}', nor a GenericProperty named '{property}'");
                //}
                case "P":
                    {
                        StringBuilder propertyElement = new StringBuilder();
                        propertyElement.Append("<Property>$Context");
                        if (!String.IsNullOrWhiteSpace(pClass))
                        {
                            var projectionPart = BuildProjectionPathPart(pClass);
                            propertyElement.Append(projectionPart.path);
                            managementPackClass = projectionPart.properyClass;
                        }

                        var propertyPart = BuildPropertyPathPart(property, managementPackClass);
                        if (propertyPart != null)
                        {
                            propertyElement.Append(propertyPart.Path);
                            propertyElement.Append("</Property>");
                            return new EvaluatorPropertyInfo(propertyElement.ToString(), propertyPart.Property);
                        }
                        throw new Exception($"Can't find property '{property}' for type '{managementPackClass?.Name}'");
                    }
                case "G":
                    {
                        var prop = GenericProperty.GetGenericProperties().FirstOrDefault(p => p.PropertyName.Equals(property, StringComparison.OrdinalIgnoreCase));
                        if (prop == null)
                        {
                            throw new Exception($"Can't find GenericProperty '{property}'");
                        }
                        if (string.IsNullOrWhiteSpace(pClass))
                        {
                            
                            return new EvaluatorPropertyInfo($"<GenericProperty>{property}</GenericProperty>", prop);
                        }
                        else
                        {
                            //StringBuilder propertyElement = new StringBuilder();
                            //propertyElement.Append("<GenericProperty Path=\"$Context");

                            var projectionPart = BuildProjectionPathPart(pClass);
                            //propertyElement.Append(projectionPart.path);
                            //managementPackClass = projectionPart.properyClass;

                            //propertyElement.Append($"$\">{property}</GenericProperty>");
                            var genpro =
                                $"<GenericProperty Path=\"$Context{projectionPart.path}$\">{property}</GenericProperty>";
                            return new EvaluatorPropertyInfo(genpro, prop);
                        }

                    }

                default:
                    throw new Exception($"Prefix '{type}' not valid!");
            }

        }

        private (string path, ManagementPackClass properyClass) BuildProjectionPathPart(string classesPart)
        {
            var parts = classesPart.Split('!').Select(p => p?.Trim().ToNull()).Where(p => p != null).ToList();

            StringBuilder relationshipPath = new StringBuilder();
            ManagementPackClass propertyClass = null;

            //ManagementPackRelationshipEndpoint currentManagementPackRelationshipEndpoint = null;
            KeyValuePair<ManagementPackRelationshipEndpoint, ITypeProjectionComponent>? relation = null;
            for (var i = 0; i < parts.Count; i++)
            {
                var part = parts[i];
                var isLast = i == parts.Count - 1;

                var mClass = _scsmClient.Types().GetClassByName(part);
                
                
                if (mClass == null)
                {

                    if (relation == null)
                    {
                        var trel = _managementPackTypeProjection.FirstOrDefault(p => p.Value.Alias == part);
                        if (trel.Value == null)
                        {
                            trel = _managementPackTypeProjection.FirstOrDefault(p => p.Key.ParentElement.Name == part);
                        }

                        relation = trel;
                    }
                    else
                    {
                        var trel = relation.Value.Value.FirstOrDefault(p => p.Value.Alias == part);
                        if (trel.Value == null)
                        {
                            trel = relation.Value.Value.FirstOrDefault(p => p.Key.ParentElement.Name == part);
                        }
                        //relation = relation.Value.Value.FirstOrDefault(p => p.Value.Alias == part) ??
                        //           relation.Value.Value.FirstOrDefault(p => p.Key.ParentElement.Name == part);

                        relation = trel;
                    }

                    if (relation.Value.Value == null)
                    {
                        throw new Exception(
                            $"Can't find a TypeProjection with Alias '{part}' or Relationship with Id '{part}'");
                    }
                    //relation = currentManagementPackeTypeProjection.FirstOrDefault(p => p.Key.ParentElement.Name == part);
                    mClass = relation.Value.Value.TargetType;

                }
                else
                {
                    relation = FindRelationEndpoint(_managementPackTypeProjection, mClass);
                }
                
               
                if (relation == null)
                {
                    throw new Exception($"Relation for Types '{part}' not found!");
                }
                var rel = relation.Value;

                if (isLast)
                {
                    propertyClass = rel.Key.Type.GetElement();
                }


                var managementPack = rel.Key.GetManagementPack();
               

                string mpAlias = null;
                if (_references.ContainsKey(managementPack.Name))
                {
                    mpAlias = _references[managementPack.Name].alias;
                }
                else
                {
                    mpAlias = $"A{Guid.NewGuid():N}";
                    var refString = $"<Reference Id=\"{managementPack.Name}\" PublicKeyToken=\"{managementPack.KeyToken}\" Version=\"{managementPack.Version}\" Alias=\"{mpAlias}\" />";
                    _references.Add(managementPack.Name, (mpAlias, refString));
                }

                var valueMP = rel.Value.TargetType.GetManagementPack();
                string valueMpAlias = null;
                if (_references.ContainsKey(valueMP.Name))
                {
                    valueMpAlias = _references[valueMP.Name].alias;
                }
                else
                {
                    valueMpAlias = $"A{Guid.NewGuid():N}";
                    var refString = $"<Reference Id=\"{valueMP.Name}\" PublicKeyToken=\"{valueMP.KeyToken}\" Version=\"{valueMP.Version}\" Alias=\"{valueMpAlias}\" />";
                    _references.Add(valueMP.Name, (valueMpAlias, refString));
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

                relationshipPath.Append($"/Path[Relationship='{mpAlias}!{rel.Key.ParentElement.Name}' {seedRole}TypeConstraint='{valueMpAlias}!{mClass.Name}']");

            }

            return (relationshipPath.ToString(), propertyClass);
        }

        private EvaluatorPropertyInfo BuildPropertyPathPart(string propertyName, ManagementPackClass managementPackClass)
        {
            managementPackClass = managementPackClass ?? (_baseIsTypeProjection ? _managementPackTypeProjection.TargetType : _baseManagementPackClass);

            var prop = FindProperty(propertyName, managementPackClass);
            if (prop == null)
            {
                return null;
                //throw new Exception($"Can't find property '{propertyName}' for type '{managementPackClass.Name}'");
            }
            var managementPack = prop.GetManagementPack();
            string mpAlias = null;
            if (_references.ContainsKey(managementPack.Name))
            {
                mpAlias = _references[managementPack.Name].alias;
            }
            else
            {
                mpAlias = $"A{Guid.NewGuid():N}";
                var refString = $"<Reference Id=\"{managementPack.Name}\" PublicKeyToken=\"{managementPack.KeyToken}\" Version=\"{managementPack.Version}\" Alias=\"{mpAlias}\" />";
                _references.Add(managementPack.Name, (mpAlias, refString));
            }

            var value = $"/Property[Type='{mpAlias}!{prop.ParentElement.Name}']/{prop.Name}$";

            return new EvaluatorPropertyInfo(value, prop);
        }

        private KeyValuePair<ManagementPackRelationshipEndpoint, ITypeProjectionComponent>? FindRelationEndpoint(ITypeProjectionComponent managementPackTypeProjection, ManagementPackClass relationClass)
        {
            foreach (var keyValuePair in managementPackTypeProjection)
            {
                if (keyValuePair.Value.TargetType.Id == relationClass.Id)
                {
                    return keyValuePair;
                }

                var found = FindRelationEndpoint(keyValuePair.Value, relationClass);
                if (found != null)
                {
                    return found;
                }

            }

            if (relationClass.Base?.GetElement() != null)
            {
                return FindRelationEndpoint(managementPackTypeProjection, relationClass.Base.GetElement());
            }
            return null;
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

    }



}