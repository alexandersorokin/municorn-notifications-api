using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace Municorn.Notifications.Api.Tests.DependencyInjection.Scope
{
    [PrimaryConstructor]
    internal partial class LimitOptionalArgumentsForGenericValidationMethodInfo : MethodInfo
    {
        private readonly MethodInfo implementation;
        private readonly int optionalArgumentLimit;

        public override object[] GetCustomAttributes(bool inherit) =>
            this.implementation.GetCustomAttributes(inherit);

        public override object[] GetCustomAttributes(Type attributeType, bool inherit) =>
            this.implementation.GetCustomAttributes(attributeType, inherit);

        public override bool IsDefined(Type attributeType, bool inherit) =>
            this.implementation.IsDefined(attributeType, inherit);

        public override Type? DeclaringType => this.implementation.DeclaringType;

        public override string Name => this.implementation.Name;

        public override Type? ReflectedType => this.implementation.ReflectedType;

        public override MethodImplAttributes GetMethodImplementationFlags() =>
            this.implementation.GetMethodImplementationFlags();

        public override ParameterInfo[] GetParameters() => this.implementation
            .GetParameters()
            .TakeWhile((parameter, index) => !parameter.IsOptional || index < this.optionalArgumentLimit)
            .ToArray();

        public override object? Invoke(object? obj, BindingFlags invokeAttr, Binder? binder, object?[]? parameters, CultureInfo? culture) =>
            this.implementation.Invoke(obj, invokeAttr, binder, parameters, culture);

        public override MethodAttributes Attributes => this.implementation.Attributes;

        public override RuntimeMethodHandle MethodHandle => this.implementation.MethodHandle;

        public override MethodInfo GetBaseDefinition() => this.implementation.GetBaseDefinition();

        public override ICustomAttributeProvider ReturnTypeCustomAttributes =>
            this.implementation.ReturnTypeCustomAttributes;

        public override Type ReturnType => this.implementation.ReturnType;

        public override bool IsGenericMethodDefinition => this.implementation.IsGenericMethodDefinition;

        public override Type[] GetGenericArguments() => this.implementation.GetGenericArguments();

        public override MethodInfo MakeGenericMethod(params Type[] typeArguments) => this.implementation.MakeGenericMethod(typeArguments);

        public override CallingConventions CallingConvention => this.implementation.CallingConvention;

        public override bool ContainsGenericParameters => this.implementation.ContainsGenericParameters;

        public override MethodImplAttributes MethodImplementationFlags =>
            this.implementation.MethodImplementationFlags;

        public override Delegate CreateDelegate(Type delegateType) => this.implementation.CreateDelegate(delegateType);

        public override Delegate CreateDelegate(Type delegateType, object? target) => this.implementation.CreateDelegate(delegateType, target);

        public override IEnumerable<CustomAttributeData> CustomAttributes => this.implementation.CustomAttributes;

        public override IList<CustomAttributeData> GetCustomAttributesData() => this.implementation.GetCustomAttributesData();

        public override MethodInfo GetGenericMethodDefinition() => this.implementation.GetGenericMethodDefinition();

        public override bool IsSecurityTransparent => this.implementation.IsSecurityTransparent;

        public override MethodBody? GetMethodBody() => this.implementation.GetMethodBody();

        public override bool HasSameMetadataDefinitionAs(MemberInfo other) => this.implementation.HasSameMetadataDefinitionAs(other);

        public override bool IsCollectible => this.implementation.IsCollectible;

        public override bool IsConstructedGenericMethod => this.implementation.IsConstructedGenericMethod;

        public override bool IsGenericMethod => this.implementation.IsGenericMethod;

        public override bool IsSecurityCritical => this.implementation.IsSecurityCritical;

        public override bool IsSecuritySafeCritical => this.implementation.IsSecuritySafeCritical;

        public override MemberTypes MemberType => this.implementation.MemberType;

        public override int MetadataToken => this.implementation.MetadataToken;

        public override Module Module => this.implementation.Module;

        public override ParameterInfo ReturnParameter => this.implementation.ReturnParameter;
    }
}