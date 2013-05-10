using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Reflection;

namespace The203.CollectionJson.Core
{

    public static class Extensions
    {
	   // TODO:  Should we cache this?  Eventually?
	   public static bool ExposedToClient(this PropertyInfo propInfo)
	   {
		  if (propInfo.GetCustomAttributes(typeof(HideFromClientAttribute), true).Any())
			 return false;

		  Type type = propInfo.PropertyType;
		  return type.IsScalar();
	   }

	   public static bool IsScalar(this Type type)
	   {

		  if (type.IsPrimitive)
			 return true;
		  if (type == typeof(string))
			 return true;
		  if (type == typeof(Guid))
			 return true;
		  if (type == typeof(Decimal))
			 return true;
		  if (type == typeof(DateTime))
			 return true;
		  if (type.IsEnum)
			 return true;
		  if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
		  {
			 Type nullableArg = type.GetGenericArguments().First();
			 return nullableArg.IsScalar();
		  }
		  return false;
	   }

	   public static string GetPropertyName<T, TProp>(this Expression<Func<T, TProp>> propertyExpression)
	   {
		  MemberExpression memberExpression = GetMemberExpression(propertyExpression);
		  var memberPath = new StringBuilder(memberExpression.Member.Name, 64); //TODO: do an analysis to tune the init'd capacity
		  // this section is soley for multiple property paths - ie [object].[child].[property]
		  //TODO: Add overide with flag for returning possible multi-dot path or just the 'final' property
		  while (memberExpression.Expression is MemberExpression)
		  {
			 memberExpression = (MemberExpression)memberExpression.Expression;
			 // it's assumed that 2 inserts will perform better than string.Format()
			 memberPath.Insert(0, '.');
			 memberPath.Insert(0, memberExpression.Member.Name);
		  }
		  return memberPath.ToString();
	   }

	   public static string GetPropertyName<T>(this Expression<Func<T>> propertyExpression)
	   {
		  MemberExpression memberExpression = GetMemberExpression(propertyExpression);
		  return memberExpression.Member.Name;
	   }

	   public static Type GetExpressionType<T>(this Expression<Func<T>> propertyExpression)
	   {
		  MemberExpression memberExpression = GetMemberExpression(propertyExpression);
		  return memberExpression.Expression.Type;
	   }

	   public static string GetPropertyDisplayName<T>(this Expression<Func<T>> propertyExpression)
	   {
		  var type = propertyExpression.GetExpressionType();
		  var propertyName = propertyExpression.GetPropertyName();
		  var rtn = propertyName;
		  
		  return rtn;
	   }

	   private static MemberExpression GetMemberExpression<T>(Expression<Func<T>> propertyExpression)
	   {
		  MemberExpression memberExpression = propertyExpression.Body as MemberExpression;
		  if (memberExpression == null && propertyExpression.Body is UnaryExpression)
			 memberExpression = ((UnaryExpression)propertyExpression.Body).Operand as MemberExpression;
		  if (memberExpression == null)
			 throw new ArgumentException("Input expression must be a member expression or unary expression.", "propertyExpression");

		  return memberExpression;
	   }

	   private static MemberExpression GetMemberExpression<T, TProp>(Expression<Func<T, TProp>> propertyExpression)
	   {
		  MemberExpression memberExpression = propertyExpression.Body as MemberExpression;
		  if (memberExpression == null && propertyExpression.Body is UnaryExpression)
			 memberExpression = ((UnaryExpression)propertyExpression.Body).Operand as MemberExpression;
		  if (memberExpression == null)
			 throw new ArgumentException("Input expression must be a member expression or unary expression.", "propertyExpression");

		  return memberExpression;
	   }
    }
}
